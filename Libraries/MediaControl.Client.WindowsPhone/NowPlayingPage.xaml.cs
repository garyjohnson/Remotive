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
using MediaControl.Client.WindowsPhone.ViewModels;
using MediaControl.Client.WindowsPhone.MediaPlayback;
using System.ComponentModel;
using Microsoft.Phone.Shell;

namespace MediaControl.Client.WindowsPhone
{
    public partial class NowPlayingPage : PhoneApplicationPage
    {
        public NowPlayingPage()
        {
            InitializeComponent();
            
            Loaded += new RoutedEventHandler(NowPlayingPage_Loaded);
            DataContext = new NowPlayingViewModel(this.Dispatcher);
            ((INotifyPropertyChanged)DataContext).PropertyChanged += new PropertyChangedEventHandler(NowPlayingPage_PropertyChanged);
        }

        private void NowPlayingPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentMediaState")
            {
                NowPlayingViewModel viewModel = (NowPlayingViewModel)DataContext;
                if (viewModel != null && viewModel.CurrentMediaState != null)
                {
                    UpdatePlayIcon(((NowPlayingViewModel)DataContext).CurrentMediaState.PlaybackState);
                }
            }
        }

        private void NowPlayingPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Reset page transition
            ResetPageTransitionList.Begin();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ListBoxOne_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void prevTrack_Click(object sender, EventArgs e)
        {
            MediaService.Current.PreviousTrackAsync((mediaState, state)=>
                {
                    ((NowPlayingViewModel)DataContext).CurrentMediaState = mediaState;
                }, null);
        }

        private void play_Click(object sender, EventArgs e)
        {
            if (((NowPlayingViewModel)DataContext).CurrentMediaState.PlaybackState == 
                MediaPlayback.PlaybackState.Play)
            {
                MediaService.Current.PauseAsync((mediaState, state) =>
                {
                    ((NowPlayingViewModel)DataContext).CurrentMediaState = mediaState;
                    
                }, null);
            }
            else
            {
                MediaService.Current.PlayAsync((mediaState, state) =>
                {
                    ((NowPlayingViewModel)DataContext).CurrentMediaState = mediaState;
                }, null);
            }
        }

        private void nextTrack_Click(object sender, EventArgs e)
        {
            MediaService.Current.NextTrackAsync((mediaState, state) =>
            {
                ((NowPlayingViewModel)DataContext).CurrentMediaState = mediaState;
            }, null);
        }

        private void UpdatePlayIcon(PlaybackState state)
        {
            if (state == PlaybackState.Play)
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IconUri = new Uri("/Images/Pause.png", UriKind.Relative);
            }
            else
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IconUri = new Uri("/Images/Play.png", UriKind.Relative);
            }
        }
    }
}
