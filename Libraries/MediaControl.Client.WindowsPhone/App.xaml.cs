using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Sharkfist.Phone.SilverlightCore;
using MediaControl.Client.WindowsPhone.Services;
using Microsoft.Phone.Marketplace;
using Microsoft.Phone.Info;
using Microsoft.Phone.Tasks;
using System.Globalization;
using System.Text;

namespace MediaControl.Client.WindowsPhone
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private static readonly object _syncLock = new object();

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

#if DEBUG
            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                MemoryDiagnosticsHelper.Start(true);
            }
#endif

            // Standard Silverlight initialization
            InitializeComponent();

            InitializeServices();

            // Phone-specific initialization
            InitializePhoneApplication();
        }

        public static void RegisterService<T>(T service)
        {
            lock (_syncLock)
            {
                _services[typeof(T)] = service;
            }
        }

        public static T GetService<T>() where T : class
        {
            if (_services.ContainsKey(typeof(T)))
            {
                lock (_syncLock)
                {
                    if (_services.ContainsKey(typeof(T)))
                    {
                        return _services[typeof(T)] as T;
                    }

                }
            }

            return null;
        }

        private void InitializeServices()
        {
            RegisterService<IRequestService>(new RequestService());
            RegisterService<IMediaStateService>(new MediaStateService());
            if (!Configuration.LoadSetting<bool>("IsFirstLaunch", true))
            {
                App.GetService<IMediaStateService>().StartUpdating();
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            IStateHandler stateHandler = RootFrame.Content as IStateHandler;
            if (stateHandler != null)
            {
                stateHandler.Launching(sender, e);
            }
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private bool _isActivated = false;
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            //Test.IsTrialTest = false;
            _isActivated = true;
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            App.GetService<IMediaStateService>().StopUpdating();
            App.GetService<IRequestService>().Teardown();

            bool isTrial = new LicenseInformation().IsTrialOrTest();
            if (isTrial)
            {
                Configuration.SaveStateSetting<bool>("IsTrial", isTrial);
            }

            IStateHandler stateHandler = RootFrame.Content as IStateHandler;
            if (stateHandler != null)
            {
                stateHandler.Deactivated(sender, e);
            }
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            IStateHandler stateHandler = RootFrame.Content as IStateHandler;
            if (stateHandler != null)
            {
                stateHandler.Closing(sender, e);
            }
        }

        // Code to execute if a navigation fails
        void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        public static bool IsErrorPage(Uri pageUri)
        {
            return (pageUri.OriginalString == "/Error/ErrorPage.xaml");
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }

            if (e.ExceptionObject is CloseApplicationException || this.RootFrame == null)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
            Configuration.SaveStateSetting<Exception>("UnhandledException", e.ExceptionObject);
            this.RootFrame.Navigate(new Uri("/Error/ErrorPage.xaml", UriKind.Relative));
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // TransitionFrame has a bug where if navigating too fast, 
            // you get a blank page. AnimatingFrame, on the other hand,
            // has a bug where if navigating too fast, the app crashes.
            // I choose AnimatingFrame for it's explicit failure. Plus it happens
            // less frequently than TransitionFrame.
            RootFrame = new AnimatingFrame();
            //RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;

            string navigatedUrl = "/Settings/SettingsPage.xaml";
            if (!Configuration.LoadSetting<bool>("IsFirstLaunch", true))
            {
                navigatedUrl = "/Music/MusicPage.xaml";
            }

            this.RootFrame.Navigate(new Uri(navigatedUrl, UriKind.Relative));
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            if (_isActivated)
            {
                App.GetService<IMediaStateService>().StartUpdating();

                bool wasTrial = false;
                Configuration.TryLoadStateSetting<bool>("IsTrial", out wasTrial);
                bool isTrial = new LicenseInformation().IsTrialOrTest();

                IStateHandler stateHandler = RootFrame.Content as IStateHandler;
                if (stateHandler != null)
                {
                    stateHandler.Activated(null, new ActivatedDataEventArgs((wasTrial != isTrial)));
                }

                _isActivated = false;
            }

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}