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
using System.ComponentModel;
using MediaControl.Client.WindowsPhone.MediaPlayback;
using System.Threading;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Threading;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone.NowPlaying
{
    public class NowPlayingViewModel : ViewModel
    {
        public NowPlayingViewModel(Dispatcher dispatcher) 
        {
            _dispatcher = dispatcher;

            IMediaStateService stateService = App.GetService<IMediaStateService>();

            CurrentMediaState = stateService.CurrentMediaState;
            EstimatedPlaybackPercent = stateService.EstimatedPlaybackPercent;
            EstimatedPlaybackProgress = stateService.EstimatedPlaybackProgress;
            CurrentTimeRemaining = stateService.CurrentTimeRemaining;
            CurrentAlbumArt = stateService.CurrentAlbumArt;

            stateService.AlbumArtChanged += new EventHandler<EventArgs>(stateService_AlbumArtChanged);
            stateService.MediaStateChanged += new EventHandler<EventArgs>(stateService_MediaStateChanged);
            stateService.ProgressChanged += new EventHandler<EventArgs>(stateService_ProgressChanged);

            stateService.RefreshState();
        }

        public override void Refresh()
        {
            IMediaStateService stateService = App.GetService<IMediaStateService>();
            stateService.RefreshState();
        }

        private Dispatcher _dispatcher = null;

        public void stateService_ProgressChanged(object sender, EventArgs e)
        {
            IMediaStateService stateService = App.GetService<IMediaStateService>();

            _dispatcher.BeginInvoke(() =>
                {
                    EstimatedPlaybackPercent = stateService.EstimatedPlaybackPercent;
                    EstimatedPlaybackProgress = stateService.EstimatedPlaybackProgress;
                    CurrentTimeRemaining = stateService.CurrentTimeRemaining;
                });
        }

        public void stateService_MediaStateChanged(object sender, EventArgs e)
        {
            IMediaStateService stateService = App.GetService<IMediaStateService>();

            _dispatcher.BeginInvoke(() =>
                {
                    CurrentMediaState = stateService.CurrentMediaState;
                });
        }

        public void stateService_AlbumArtChanged(object sender, EventArgs e)
        {
            IMediaStateService stateService = App.GetService<IMediaStateService>();

            _dispatcher.BeginInvoke(() =>
                {
                    CurrentAlbumArt = stateService.CurrentAlbumArt;
                });
        }

        private ImageSource _currentAlbumArt = null;
        public ImageSource CurrentAlbumArt
        {
            get { return _currentAlbumArt; }
            set
            {
                _currentAlbumArt = value;
                NotifyPropertyChanged("CurrentAlbumArt");
            }
        }

        private int _estimatedPlaybackPercent = 0;
        public int EstimatedPlaybackPercent
        {
            get { return _estimatedPlaybackPercent; }
            set
            {
                _estimatedPlaybackPercent = value;
                NotifyPropertyChanged("EstimatedPlaybackPercent");
            }
        }

        private TimeSpan _estimatedPlaybackProgress = TimeSpan.MinValue;
        public TimeSpan EstimatedPlaybackProgress
        {
            get { return _estimatedPlaybackProgress; }
            set
            {
                _estimatedPlaybackProgress = value;
                NotifyPropertyChanged("EstimatedPlaybackProgress");
            }
        }

        private TimeSpan _currentTimeRemaining = TimeSpan.MinValue;
        public TimeSpan CurrentTimeRemaining
        {
            get { return _currentTimeRemaining; }
            set
            {
                _currentTimeRemaining = value;
                NotifyPropertyChanged("CurrentTimeRemaining");
            }
        }

        private MediaState _currentMediaState = null;
        public MediaState CurrentMediaState
        {
            get { return _currentMediaState; }
            set
            {
                _currentMediaState = value;
                NotifyPropertyChanged("CurrentMediaState");
            }
        }
    }
}
