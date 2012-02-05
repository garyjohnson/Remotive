using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using Sharkfist.Phone.SilverlightCore;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone
{
	public partial class SettingsPopup : UserControl
	{
		public SettingsPopup()
		{
			// Required to initialize variables
			InitializeComponent();
            LoadSettings();
		}

        private Action<object> _completedCallback = null;
        private object _completedState = null;
        private Popup _currentPopup = null;
        private bool? _isAppBarOpen = null;

        private Storyboard _slideInStoryboard = null;
        private Storyboard _slideOutStoryboard = null;

        private void InitializeAnimations()
        {
            _slideInStoryboard = new Storyboard(); 
            DoubleAnimation slideInAnimation = new DoubleAnimation();
            slideInAnimation.To = 0;
            slideInAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(600));
            slideInAnimation.EasingFunction = new QuadraticEase()
            {
                 EasingMode = EasingMode.EaseOut
            };
            slideInAnimation.Completed += new EventHandler(_slideInAnimation_Completed);
            Storyboard.SetTarget(slideInAnimation, _currentPopup);
            Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath("VerticalOffset"));
            _slideInStoryboard.Children.Add(slideInAnimation);

            _slideOutStoryboard = new Storyboard();
            DoubleAnimation slideOutAnimation = new DoubleAnimation();
            slideOutAnimation.To = -800;
            slideOutAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(600));
            slideOutAnimation.EasingFunction = new QuadraticEase()
            {
                EasingMode = EasingMode.EaseOut
            };
            slideOutAnimation.Completed += new EventHandler(_slideOutAnimation_Completed);
            Storyboard.SetTarget(slideOutAnimation, _currentPopup);
            Storyboard.SetTargetProperty(slideOutAnimation, new PropertyPath("VerticalOffset"));
            _slideOutStoryboard.Children.Add(slideOutAnimation);
        }

        public void ShowPopupAsync(Action<object> completed, object state)
        {
            _completedCallback = completed;
            _completedState = state;

            if (_currentPopup == null)
            {
                _currentPopup = new Popup();
                InitializeAnimations();
                _currentPopup.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                _currentPopup.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            }

            PhoneApplicationFrame phoneFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (phoneFrame != null)
            {
                PhoneApplicationPage page = phoneFrame.Content as PhoneApplicationPage;
                if (page != null)
                {
                    _isAppBarOpen = page.ApplicationBar.IsVisible;
                    page.ApplicationBar.IsVisible = false;
                }
            }

            _currentPopup.Child = this;
            Width = ((FrameworkElement)Application.Current.RootVisual).ActualWidth;
            Height = ((FrameworkElement)Application.Current.RootVisual).ActualHeight;
            _currentPopup.Width = ((FrameworkElement)Application.Current.RootVisual).ActualWidth;
            _currentPopup.Height = ((FrameworkElement)Application.Current.RootVisual).ActualHeight;
            _currentPopup.HorizontalOffset = 0;
            _currentPopup.VerticalOffset = -800;
            _currentPopup.IsOpen = true;

            _slideInStoryboard.Begin();
        }

        private void _slideOutAnimation_Completed(object sender, EventArgs e)
        {
            ClosePopupAsync();
        }

        private void _slideInAnimation_Completed(object sender, EventArgs e)
        {
        }

        private void ClosePopupAsync()
        {
            SaveSettings();

            if (_isAppBarOpen.HasValue)
            {
                PhoneApplicationFrame phoneFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                if (phoneFrame != null)
                {
                    PhoneApplicationPage page = phoneFrame.Content as PhoneApplicationPage;
                    if (page != null)
                    {
                        page.ApplicationBar.IsVisible = _isAppBarOpen.Value;
                    }
                }
            }

            if (_currentPopup != null)
            {
                _currentPopup.IsOpen = false;
                _currentPopup.Child = null;
            }

            if (_completedCallback != null)
            {
                _completedCallback.Invoke(_completedState);
            }
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

        private void SaveAndClose()
        {
            if (Validate())
            {
                _slideOutStoryboard.Begin();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveAndClose();
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
	}
}