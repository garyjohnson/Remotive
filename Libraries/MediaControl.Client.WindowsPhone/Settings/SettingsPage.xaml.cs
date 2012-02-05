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
using Microsoft.Phone.Controls;
using Sharkfist.Phone.SilverlightCore;
using MediaControl.Client.WindowsPhone.Services;
using System.ComponentModel;

namespace MediaControl.Client.WindowsPhone.Settings
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
		{
			// Required to initialize variables
			InitializeComponent();

            if (!DesignerProperties.IsInDesignTool)
            {
                Opacity = 0;
            }

            LoadSettings();
		}

        private void SaveSettings()
        {
            Configuration.SaveSetting("hostname", address.Text);
            Configuration.SaveSetting("port", port.Text);
            App.GetService<IRequestService>().ResetClients();
        }

        private void LoadSettings()
        {
            address.Text = Configuration.LoadSetting("hostname", string.Empty);
            port.Text = Configuration.LoadSetting("port", "8888");
        }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(address.Text))
            {
                errorText.Text = "Server address cannot be blank or empty.";
                address.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(port.Text))
            {
                errorText.Text = "Port cannot be blank or empty. The default port is 8888.";
                port.Focus();
                return false;
            }

            if (!Uri.IsWellFormedUriString(string.Format("http://{0}:{1}", address.Text, port.Text), UriKind.RelativeOrAbsolute))
            {
                errorText.Text = "The server address is invalid or could not be found. Please check your settings and try again.";
                address.Focus();
                return false;
            }

            errorText.Text = string.Empty;
            return true;
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            if (!App.IsErrorPage(e.Uri))
            {
                e.Cancel = !Save();
            }
        }

        private bool Save()
        {
            if (Validate())
            {
                SaveSettings();
                return true;
            }

            return false;
        }

        private bool SaveAndClose()
        {
            if (Save())
            {
                bool isFirstLaunch = Configuration.LoadSetting<bool>("IsFirstLaunch", true);
                if (isFirstLaunch || !NavigationService.CanGoBack)
                {
                    if (isFirstLaunch)
                    {
                        App.GetService<IMediaStateService>().StartUpdating();
                        Configuration.SaveSetting<bool>("IsFirstLaunch", false);
                    }

                    string navigatedUrl = "/Music/MusicPage.xaml";
                    NavigationService.Navigate(new Uri(navigatedUrl, UriKind.Relative));
                }
                else
                {
                    NavigationService.GoBack();
                }

                return true;
            }

            return false;
        }

        private void address_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                port.Focus();
            }
        }

        private void port_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                this.Focus();
                SaveAndClose();
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsCurrentPage())
            {
                // Only do this if we are navigating to this page
                if (Configuration.LoadSetting<bool>("IsFirstLaunch", true))
                {
                    SplashScreen splashScreen = new SplashScreen();
                    splashScreen.ShowPopupAsync((state) =>
                    {
                    }, null);
                }

                // After the settings screen is shown, make the page behind it visible (before the settings
                // screen closes)
                Opacity = 100;
            }
        }

        private bool IsCurrentPage()
        {
            return (NavigationService.Source.OriginalString == "/Settings/SettingsPage.xaml");
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndClose();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            SaveAndClose();
        }
    }
}