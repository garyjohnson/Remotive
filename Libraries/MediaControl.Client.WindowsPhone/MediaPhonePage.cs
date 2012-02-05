using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.ComponentModel;
using MediaControl.Client.WindowsPhone.MediaPlayback;
using Microsoft.Phone.Shell;
using MediaControl.Client.WindowsPhone.NowPlaying;
using Sharkfist.Phone.SilverlightCore;
using MediaControl.Client.WindowsPhone.Services;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Marketplace;

namespace MediaControl.Client.WindowsPhone
{
    public class MediaPhonePage : ConnectedPhonePage
    {
        public MediaPhonePage()
        {
            BuildApplicationBar();
            if (!Designer.IsInDesignMode)
            {
                App.GetService<IMediaStateService>().MediaStateChanged += new EventHandler<EventArgs>(MediaPhonePage_MediaStateChanged);
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            ForceCloseVolumePopup();
            base.OnNavigatingFrom(e);
        }

        public void MediaPhonePage_MediaStateChanged(object sender, EventArgs e)
        {
            MediaState state = App.GetService<IMediaStateService>().CurrentMediaState;
            if (state != null)
            {
                UpdatePlayIcon(state.PlaybackState);
            }
        }

        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = true;

            // Buttons
            _prevButton = new ApplicationBarIconButton(new Uri("/Images/PrevTrack.png", UriKind.Relative));
            _prevButton.Text = "previous";
            _prevButton.Click += new EventHandler(_prevButton_Click);

            _playPauseButton = new ApplicationBarIconButton(new Uri("/Images/Play.png", UriKind.Relative));
            _playPauseButton.Text = "play";
            _playPauseButton.Click += new EventHandler(_playPauseButton_Click);

            _nextButton = new ApplicationBarIconButton(new Uri("/Images/NextTrack.png", UriKind.Relative));
            _nextButton.Text = "next";
            _nextButton.Click += new EventHandler(_nextButton_Click);

            _volumeButton = new ApplicationBarIconButton(new Uri("/Images/Volume.png", UriKind.Relative));
            _volumeButton.Text = "volume";
            _volumeButton.Click += new EventHandler(_volumeButton_Click);

            _settingsMenu = new ApplicationBarMenuItem("settings");
            _settingsMenu.Click += new EventHandler(_settingsMenu_Click);

            _purchaseMenu = new ApplicationBarMenuItem("purchase");
            _purchaseMenu.Click += new EventHandler(_purchaseMenu_Click);

            _helpMenu = new ApplicationBarMenuItem("help");
            _helpMenu.Click += new EventHandler(_helpMenu_Click);

            ApplicationBar.Buttons.Add(_prevButton);
            ApplicationBar.Buttons.Add(_playPauseButton);
            ApplicationBar.Buttons.Add(_nextButton);
            ApplicationBar.Buttons.Add(_volumeButton);

            BuildApplicationBar(ApplicationBar);

            // Add settings to the end of the menu items
            ApplicationBar.MenuItems.Add(_settingsMenu);

            if (new LicenseInformation().IsTrialOrTest())
            {
                ApplicationBar.MenuItems.Add(_purchaseMenu);
            }
            else
            {
                ApplicationBar.MenuItems.Add(_helpMenu);
            }
        }

        private void _helpMenu_Click(object sender, EventArgs e)
        {
            EmailComposeTask task = new EmailComposeTask()
            {
                To = "sharkfist@hotmail.com",
                Subject = "Remotive Help Request"
            };

            task.Show();
        }

        VolumePopup _volumePopup = null;
        bool _volumeIsOpen = false;

        private void _volumeButton_Click(object sender, EventArgs e)
        {
            if (_volumePopup == null)
            {
                _volumePopup = new VolumePopup();
            }

            if (!_volumeIsOpen)
            {
                OpenVolumePopup();
            }
            else
            {
                CloseVolumePopup();
            }
        }

        private void OpenVolumePopup()
        {
            _volumeIsOpen = true;
            _volumePopup.ShowPopupAsync((state) =>
            {
            }, null);
        }

        private void CloseVolumePopup()
        {
            _volumePopup.ClosePopupAsync((state) =>
            {
                _volumeIsOpen = false;
            }, null);
        }

        private void ForceCloseVolumePopup()
        {
            if (_volumePopup != null)
            {
                _volumePopup.ForceClosePopup();
            }
            _volumeIsOpen = false;
        }

        private void _purchaseMenu_Click(object sender, EventArgs e)
        {
            new MarketplaceDetailTask().Show();
        }

        private void _settingsMenu_Click(object sender, EventArgs e)
        {
            string navigatedUrl = "/Settings/SettingsPage.xaml";
            NavigationService.Navigate(new Uri(navigatedUrl, UriKind.Relative));
        }

        protected virtual void BuildApplicationBar(IApplicationBar appBar) { }

        private ApplicationBarIconButton _playPauseButton;
        private ApplicationBarIconButton _prevButton;
        private ApplicationBarIconButton _nextButton;
        private ApplicationBarIconButton _volumeButton;

        private ApplicationBarMenuItem _settingsMenu;
        private ApplicationBarMenuItem _purchaseMenu;
        private ApplicationBarMenuItem _helpMenu;

        private void UpdatePlayIcon(PlaybackState state)
        {
            if (state == PlaybackState.Play)
            {
                _playPauseButton.IconUri = new Uri("/Images/Pause.png", UriKind.Relative);
                _playPauseButton.Text = "pause";

            }
            else
            {
                _playPauseButton.IconUri = new Uri("/Images/Play.png", UriKind.Relative);
                _playPauseButton.Text = "play";
            }
        }


        private void _playPauseButton_Click(object sender, EventArgs e)
        {
            MediaState state = App.GetService<IMediaStateService>().CurrentMediaState;
            if (state != null)
            {
                if (state.PlaybackState ==
                    MediaPlayback.PlaybackState.Play)
                {
                    App.GetService<IRequestService>().PauseAsync((mediaState, innerState, error) =>
                    {
                        App.GetService<IMediaStateService>().CurrentMediaState = mediaState;
                    }, null);
                }
                else
                {
                    App.GetService<IRequestService>().PlayAsync((mediaState, innerState, error) =>
                    {
                        App.GetService<IMediaStateService>().CurrentMediaState = mediaState;
                    }, null);
                }
            }
        }

        private void _nextButton_Click(object sender, EventArgs e)
        {
            App.GetService<IRequestService>().NextTrackAsync((mediaState, state, error) =>
            {
                App.GetService<IMediaStateService>().CurrentMediaState = mediaState;
            }, null);
        }

        private void _prevButton_Click(object sender, EventArgs e)
        {
            App.GetService<IRequestService>().PreviousTrackAsync((mediaState, state, error) =>
            {
                App.GetService<IMediaStateService>().CurrentMediaState = mediaState;
            }, null);
        }
    }
}
