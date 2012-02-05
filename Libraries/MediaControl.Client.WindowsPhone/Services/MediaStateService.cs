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
using System.Threading;
using System.IO;
using System.Windows.Threading;
using MediaControl.Client.WindowsPhone.MediaPlayback;
using System.Windows.Media.Imaging;
using Sharkfist.Phone.SilverlightCore;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone
{
    public class MediaStateService : MediaControl.Client.WindowsPhone.Services.IMediaStateService
    {
        public MediaStateService()
        {
            InitializeTimers();
        }

        public void StartUpdating()
        {
            StartTimer(_nowPlayingTimer, 0, REFRESH_INTERVAL);
            StartTimer(_estimateTimer, 0, 1000);
        }

        public void StopUpdating()
        {
            StopTimer(_nowPlayingTimer);
            StopTimer(_estimateTimer);
        }

        // How frequently the playback status is refreshed, in milliseconds.
        // Regardless of this value, a new request will not be sent out until
        // the current one returns.
        private const uint REFRESH_INTERVAL = 7000;
        private Timer _nowPlayingTimer = null;
        private Timer _estimateTimer = null;

        private ImageSource _currentAlbumArt = null;
        public ImageSource CurrentAlbumArt
        {
            get { return _currentAlbumArt; }
            set
            {
                _currentAlbumArt = value;

                if (_albumArtChanged != null)
                {
                    _albumArtChanged(this, EventArgs.Empty);
                }
            }
        }

        private int _estimatedPlaybackPercent = 0;
        public int EstimatedPlaybackPercent
        {
            get { return _estimatedPlaybackPercent; }
            set
            {
                _estimatedPlaybackPercent = value;
            }
        }

        private TimeSpan _estimatedPlaybackProgress = TimeSpan.MinValue;
        public TimeSpan EstimatedPlaybackProgress
        {
            get { return _estimatedPlaybackProgress; }
            set
            {
                _estimatedPlaybackProgress = value;

                if (CurrentMediaState != null &&
                    CurrentMediaState.CurrentTrack != null)
                {
                    CurrentTimeRemaining = new TimeSpan(0,0, (int)(EstimatedPlaybackProgress.TotalSeconds - CurrentMediaState.CurrentTrack.Duration.TotalSeconds));
                    EstimatedPlaybackPercent = (int)((_estimatedPlaybackProgress.TotalSeconds / _currentMediaState.CurrentTrack.Duration.TotalSeconds) * 100);
                }

                if (_progressChanged != null)
                {
                    _progressChanged(this, EventArgs.Empty);
                }
            }
        }

        private TimeSpan _currentTimeRemaining = TimeSpan.MinValue;
        public TimeSpan CurrentTimeRemaining
        {
            get { return _currentTimeRemaining; }
            set
            {
                _currentTimeRemaining = value;
            }
        }

        private MediaPlayback.Track _previousTrack = null;
        private MediaState _currentMediaState = null;
        public MediaState CurrentMediaState
        {
            get { return _currentMediaState; }
            set
            {
                if (_currentMediaState != null)
                {
                    _previousTrack = _currentMediaState.CurrentTrack;
                }

                _currentMediaState = value;

                if (_currentMediaState != null && _currentMediaState.CurrentTrack != null)
                {
                    // Set the playback position
                    EstimatedPlaybackProgress = _currentMediaState.PlaybackPosition;

                    // If the track has changed, get the album art.
                    if (_previousTrack == null || 
                        (!_previousTrack.EqualsTrack(_currentMediaState.CurrentTrack)))
                    {
                        // Update the album art
                        App.GetService<IRequestService>().GetAlbumArtByTrackAsync(_currentMediaState.CurrentTrack.ConvertToTrack(), 
                            (track, imageData, state, error) =>
                        {
                            if (error == null)
                            {
                                if (imageData == null)
                                {
                                    CurrentAlbumArt = null;
                                }
                                else
                                {
                                    MemoryStream memStream = new MemoryStream(imageData);
                                    BitmapImage albumArt = new BitmapImage();
                                    albumArt.SetSource(memStream);
                                    CurrentAlbumArt = albumArt;
                                }
                            }
                        }, null);
                    }
                }

                if (_mediaStateChanged != null)
                {
                    _mediaStateChanged(this, EventArgs.Empty);
                }
            }
        }

        public void RefreshState()
        {
            // Stop the timer and send the request
            StopTimer(_nowPlayingTimer);
            App.GetService<IRequestService>().GetMediaStateAsync((mediaState, state, error) =>
            {
                if (error == null)
                {
                    if (mediaState != null)
                    {
                        DateTime then = (DateTime)state;
                        TimeSpan difference = DateTime.Now.Subtract(then);
                        mediaState.PlaybackPosition = mediaState.PlaybackPosition.Add(new TimeSpan(0, 0, (int)(difference.TotalSeconds / 2)));
                    }

                    CurrentMediaState = mediaState;
                }

                StartTimer(_nowPlayingTimer, REFRESH_INTERVAL, REFRESH_INTERVAL);

            }, DateTime.Now);
        }

        private void InitializeTimers()
        {
            _nowPlayingTimer = new Timer(new TimerCallback((sender) =>
            {
                RefreshState();
            }));

            _estimateTimer = new Timer(new TimerCallback((sender) =>
                {
                    if (EstimatedPlaybackProgress > TimeSpan.MinValue &&
                        EstimatedPlaybackProgress < TimeSpan.MaxValue &&
                        CurrentMediaState != null &&
                        CurrentMediaState.CurrentTrack != null &&
                        EstimatedPlaybackProgress < CurrentMediaState.CurrentTrack.Duration &&
                        CurrentMediaState.PlaybackState == PlaybackState.Play)
                    {
                        EstimatedPlaybackProgress = EstimatedPlaybackProgress.Add(new TimeSpan(0, 0, 1));
                    }
                }));

        }

        private void StartTimer(Timer timer, uint dueTime, uint period)
        {
            timer.Change(dueTime, period);
        }

        private void StopTimer(Timer timer)
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private event EventHandler<EventArgs> _albumArtChanged;
        public event EventHandler<EventArgs> AlbumArtChanged
        {
            add { _albumArtChanged += value.MakeWeak<EventArgs>(eh => _albumArtChanged -= eh); }
            remove { }
        }

        private event EventHandler<EventArgs> _progressChanged;
        public event EventHandler<EventArgs> ProgressChanged
        {
            add { _progressChanged += value.MakeWeak<EventArgs>(eh => _progressChanged -= eh); }
            remove { }
        }

        private event EventHandler<EventArgs> _mediaStateChanged;
        public event EventHandler<EventArgs> MediaStateChanged
        {
            add { _mediaStateChanged += value.MakeWeak<EventArgs>(eh => _mediaStateChanged -= eh); }
            remove { }
        }
    }
}
