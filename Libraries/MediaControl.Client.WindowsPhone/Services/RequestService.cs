using System;
using System.ServiceModel;
using MediaControl.Client.WindowsPhone.MediaLibrary;
using MediaControl.Client.WindowsPhone.MediaPlayback;
using Sharkfist.Phone.SilverlightCore;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;

namespace MediaControl.Client.WindowsPhone
{
    public partial class RequestService : MediaControl.Client.WindowsPhone.Services.IRequestService
    {
        public RequestService()
        {
            _requestTimer = new Timer(new TimerCallback(TimerElapsed), null, 1, Timeout.Infinite);
        }

        private void InitializeLibraryHandlers()
        {
            LibraryServiceClient.GetAlbumArtByAlbumCompleted += new EventHandler<GetAlbumArtByAlbumCompletedEventArgs>(LibraryServiceClient_GetAlbumArtByAlbumCompleted);
            LibraryServiceClient.GetAlbumArtByTrackCompleted += new EventHandler<GetAlbumArtByTrackCompletedEventArgs>(LibraryServiceClient_GetAlbumArtByTrackCompleted);
            LibraryServiceClient.GetTracksCompleted += new EventHandler<GetTracksCompletedEventArgs>(LibraryServiceClient_GetTracksCompleted);
            LibraryServiceClient.GetAlbumsByArtistCompleted += new EventHandler<GetAlbumsByArtistCompletedEventArgs>(LibraryServiceClient_GetAlbumsByArtistCompleted);

            LibraryServiceClient.PingCompleted += new EventHandler<MediaLibrary.PingCompletedEventArgs>(LibraryServiceClient_PingCompleted);
            LibraryServiceClient.GetServerVersionCompleted += new EventHandler<GetServerVersionCompletedEventArgs>(LibraryServiceClient_GetServerVersionCompleted);

            LibraryServiceClient.GetAlbumArtistRangeCompleted += new EventHandler<GetAlbumArtistRangeCompletedEventArgs>(LibraryServiceClient_GetAlbumArtistRangeCompleted);
            LibraryServiceClient.GetAlbumRangeCompleted += new EventHandler<GetAlbumRangeCompletedEventArgs>(LibraryServiceClient_GetAlbumRangeCompleted);
            LibraryServiceClient.GetArtistRangeCompleted +=new EventHandler<GetArtistRangeCompletedEventArgs>(LibraryServiceClient_GetArtistRangeCompleted);

            LibraryServiceClient.GetAlbumArtistCountCompleted += new EventHandler<GetAlbumArtistCountCompletedEventArgs>(LibraryServiceClient_GetAlbumArtistCountCompleted);
            LibraryServiceClient.GetAlbumCountCompleted += new EventHandler<GetAlbumCountCompletedEventArgs>(LibraryServiceClient_GetAlbumCountCompleted);
            LibraryServiceClient.GetArtistCountCompleted += new EventHandler<GetArtistCountCompletedEventArgs>(LibraryServiceClient_GetArtistCountCompleted);

        }

        private void InitializePlaybackHandlers()
        {
            PlaybackServiceClient.GetMediaStateCompleted += new EventHandler<GetMediaStateCompletedEventArgs>(PlaybackServiceClient_GetMediaStateCompleted);
            PlaybackServiceClient.NextTrackCompleted += new EventHandler<NextTrackCompletedEventArgs>(PlaybackServiceClient_NextTrackCompleted);
            PlaybackServiceClient.PrevTrackCompleted += new EventHandler<PrevTrackCompletedEventArgs>(PlaybackServiceClient_PrevTrackCompleted);
            PlaybackServiceClient.PlayCompleted += new EventHandler<PlayCompletedEventArgs>(PlaybackServiceClient_PlayCompleted);
            PlaybackServiceClient.PauseCompleted += new EventHandler<PauseCompletedEventArgs>(PlaybackServiceClient_PauseCompleted);

            PlaybackServiceClient.PlayAlbumCompleted += new EventHandler<PlayAlbumCompletedEventArgs>(PlaybackServiceClient_PlayAlbumCompleted);
            PlaybackServiceClient.PlayArtistCompleted += new EventHandler<PlayArtistCompletedEventArgs>(PlaybackServiceClient_PlayArtistCompleted);
            PlaybackServiceClient.PlayAlbumArtistCompleted += new EventHandler<PlayAlbumArtistCompletedEventArgs>(PlaybackServiceClient_PlayAlbumArtistCompleted);
            PlaybackServiceClient.PlayTrackCompleted += new EventHandler<PlayTrackCompletedEventArgs>(PlaybackServiceClient_PlayTrackCompleted);
            PlaybackServiceClient.PlayTracksCompleted += new EventHandler<PlayTracksCompletedEventArgs>(PlaybackServiceClient_PlayTracksCompleted);

            PlaybackServiceClient.SetVolumeCompleted += new EventHandler<SetVolumeCompletedEventArgs>(PlaybackServiceClient_SetVolumeCompleted);
        }

        private void TeardownLibraryHandlers()
        {
            if (_libraryServiceClient != null)
            {
                _libraryServiceClient.GetAlbumArtByAlbumCompleted -= new EventHandler<GetAlbumArtByAlbumCompletedEventArgs>(LibraryServiceClient_GetAlbumArtByAlbumCompleted);
                _libraryServiceClient.GetAlbumArtByTrackCompleted -= new EventHandler<GetAlbumArtByTrackCompletedEventArgs>(LibraryServiceClient_GetAlbumArtByTrackCompleted);
                _libraryServiceClient.GetTracksCompleted -= new EventHandler<GetTracksCompletedEventArgs>(LibraryServiceClient_GetTracksCompleted);
                _libraryServiceClient.GetAlbumsByArtistCompleted -= new EventHandler<GetAlbumsByArtistCompletedEventArgs>(LibraryServiceClient_GetAlbumsByArtistCompleted);

                _libraryServiceClient.PingCompleted -= new EventHandler<MediaLibrary.PingCompletedEventArgs>(LibraryServiceClient_PingCompleted);
                LibraryServiceClient.GetServerVersionCompleted -= new EventHandler<GetServerVersionCompletedEventArgs>(LibraryServiceClient_GetServerVersionCompleted);

                _libraryServiceClient.GetAlbumArtistRangeCompleted -= new EventHandler<GetAlbumArtistRangeCompletedEventArgs>(LibraryServiceClient_GetAlbumArtistRangeCompleted);
                _libraryServiceClient.GetAlbumRangeCompleted -= new EventHandler<GetAlbumRangeCompletedEventArgs>(LibraryServiceClient_GetAlbumRangeCompleted);
                _libraryServiceClient.GetArtistRangeCompleted -= new EventHandler<GetArtistRangeCompletedEventArgs>(LibraryServiceClient_GetArtistRangeCompleted);

                _libraryServiceClient.GetAlbumArtistCountCompleted -= new EventHandler<GetAlbumArtistCountCompletedEventArgs>(LibraryServiceClient_GetAlbumArtistCountCompleted);
                _libraryServiceClient.GetAlbumCountCompleted -= new EventHandler<GetAlbumCountCompletedEventArgs>(LibraryServiceClient_GetAlbumCountCompleted);
                _libraryServiceClient.GetArtistCountCompleted -= new EventHandler<GetArtistCountCompletedEventArgs>(LibraryServiceClient_GetArtistCountCompleted);

            }
        }
        
        private void TeardownPlaybackHandlers()
        {
            if (_playbackServiceClient != null)
            {
                _playbackServiceClient.GetMediaStateCompleted -= new EventHandler<GetMediaStateCompletedEventArgs>(PlaybackServiceClient_GetMediaStateCompleted);
                _playbackServiceClient.NextTrackCompleted -= new EventHandler<NextTrackCompletedEventArgs>(PlaybackServiceClient_NextTrackCompleted);
                _playbackServiceClient.PrevTrackCompleted -= new EventHandler<PrevTrackCompletedEventArgs>(PlaybackServiceClient_PrevTrackCompleted);
                _playbackServiceClient.PlayCompleted -= new EventHandler<PlayCompletedEventArgs>(PlaybackServiceClient_PlayCompleted);
                _playbackServiceClient.PauseCompleted -= new EventHandler<PauseCompletedEventArgs>(PlaybackServiceClient_PauseCompleted);

                _playbackServiceClient.PlayAlbumCompleted -= new EventHandler<PlayAlbumCompletedEventArgs>(PlaybackServiceClient_PlayAlbumCompleted);
                _playbackServiceClient.PlayArtistCompleted -= new EventHandler<PlayArtistCompletedEventArgs>(PlaybackServiceClient_PlayArtistCompleted);
                _playbackServiceClient.PlayAlbumArtistCompleted -= new EventHandler<PlayAlbumArtistCompletedEventArgs>(PlaybackServiceClient_PlayAlbumArtistCompleted);
                _playbackServiceClient.PlayTrackCompleted -= new EventHandler<PlayTrackCompletedEventArgs>(PlaybackServiceClient_PlayTrackCompleted);
                _playbackServiceClient.PlayTracksCompleted -= new EventHandler<PlayTracksCompletedEventArgs>(PlaybackServiceClient_PlayTracksCompleted);
            }
        }

        private readonly Queue<Request> _requestQueue = new Queue<Request>();
        private readonly Queue<Request> _lowPriorityRequestQueue = new Queue<Request>();

        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;

                if (_stateChanged != null)
                {
                    _stateChanged(this, EventArgs.Empty);
                }
            }
        }

        private bool _isTearingDown = false;
        private readonly object _syncLock = new object();
        private Timer _requestTimer;
        private Request _currentRequest = null;

        private void QueueRequest(Delegate action, params object[] parameters)
        {
            System.Diagnostics.Debug.WriteLine("Queueing request for {0}", action);
            lock (_syncLock)
            {
                _requestQueue.Enqueue(new Request(action, parameters));
            }
        }

        private void QueueLowPriorityRequest(Delegate action, params object[] parameters)
        {
            lock (_syncLock)
            {
                _lowPriorityRequestQueue.Enqueue(new Request(action, parameters));
            }
        }

        private void TimerElapsed(object state)
        {
            try
            {
                if (!IsBusy && (_requestQueue.Count > 0 || _lowPriorityRequestQueue.Count > 0))
                {
                    lock (_syncLock)
                    {
                        if (!IsBusy && (_requestQueue.Count > 0 || _lowPriorityRequestQueue.Count > 0))
                        {
                            IsBusy = true;

                            if (_requestQueue.Count > 0)
                            {
                                _currentRequest = _requestQueue.Dequeue();
                            }
                            else
                            {
                                _currentRequest = _lowPriorityRequestQueue.Dequeue();
                            }

                            Debug.WriteLine("Request executing");
                            _currentRequest.Execute();
                        }
                    }
                }
            }
            finally
            {
                _requestTimer.Change(1, Timeout.Infinite);
            }
        }

        private bool RequestCompleted(Exception error)
        {
            return RequestCompleted(error, false);
        }

        private bool RequestCompleted(Exception error, bool failSilently)
        {
            Debug.WriteLine("Request completing");
            if (_isTearingDown)
            {
                return false;
            }

            MessageBoxResult result = MessageBoxResult.Cancel;

            if (error != null && (error is TimeoutException || error is ArgumentNullException))
            {
                result = MessageBox.Show("Remotive is unable to find the server. Press OK to retry.", "", MessageBoxButton.OKCancel);
            }
            else if (error != null)
            {
                result = MessageBox.Show("Remotive encountered an error. Press OK to retry.", "", MessageBoxButton.OKCancel);
            }

            if (result == MessageBoxResult.OK)
            {
                _currentRequest.Execute();
            }
            else
            {
                lock (_syncLock)
                {
                    IsBusy = false;
                }
            }

            // If there was an error, the current request should not continue
            return (error == null);
        }

        public void Teardown()
        {
            _isTearingDown = true;

            _requestTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _requestTimer.Dispose();

            _requestQueue.Clear();
            _lowPriorityRequestQueue.Clear();
            _currentRequest = null;

            TeardownLibraryHandlers();
            TeardownPlaybackHandlers();
        }

        private MediaLibraryServiceClient _libraryServiceClient = null;
        private MediaLibraryServiceClient LibraryServiceClient
        {
            get
            {
                if (_libraryServiceClient == null ||
                _libraryServiceClient.State == CommunicationState.Faulted)
                {
                    string host = Configuration.LoadSetting("hostname", string.Empty);
                    string port = Configuration.LoadSetting("port", string.Empty);

                    BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                    binding.MaxReceivedMessageSize = 67108864;
                    _libraryServiceClient = new MediaLibraryServiceClient(binding,
                    new EndpointAddress(string.Format(@"http://{0}:{1}/MediaLibrary", host, port)));
                    InitializeLibraryHandlers();
                }

                return _libraryServiceClient;
            }
            set
            {
                TeardownLibraryHandlers();
                _libraryServiceClient = value;
            }
        }

        private MediaPlaybackServiceClient _playbackServiceClient = null;
        private MediaPlaybackServiceClient PlaybackServiceClient
        {
            get
            {
                if (_playbackServiceClient == null ||
                _playbackServiceClient.State == CommunicationState.Faulted)
                {

                    string host = Configuration.LoadSetting("hostname");
                    string port = Configuration.LoadSetting("port");

                    BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                    binding.MaxReceivedMessageSize = 67108864;
                    _playbackServiceClient = new MediaPlaybackServiceClient(binding,
                   new EndpointAddress(string.Format(@"http://{0}:{1}/MediaPlayback", host, port)));
                    InitializePlaybackHandlers();
                }

                return _playbackServiceClient;
            }
            set
            {
                TeardownPlaybackHandlers();
                _playbackServiceClient = value;
            }
        }

        public void FireRefresh()
        {
            if (_refresh != null)
            {
                _refresh(this, EventArgs.Empty);
            }
        }

        private event EventHandler<EventArgs> _refresh;
        public event EventHandler<EventArgs> Refresh
        {
            add { _refresh += value.MakeWeak<EventArgs>(eh => _refresh -= eh); }
            remove { }
        }

        public void ResetClients()
        {
            LibraryServiceClient = null;
            PlaybackServiceClient = null;
            FireRefresh();
        }

        public void PingUntilSuccessful(Action<object> callback, object state)
        {
            Tuple<Action<object>, object> originalState = new Tuple<Action<object>,object>(callback, state);
            PingUntilSuccessfulRecursive(originalState);
        }

        private void PingUntilSuccessfulRecursive(Tuple<Action<object>, object> originalState)
        {
            try
            {
                Ping((innerState, error) =>
                {
                    Tuple<Action<object>, object> innerOriginalState = (Tuple<Action<object>, object>)innerState;
                    if (error != null)
                    {
                        PingUntilSuccessfulRecursive(innerOriginalState);
                    }
                    else
                    {
                        innerOriginalState.Item1.Invoke(innerOriginalState.Item2);
                    }
                }, originalState);
            }
            catch (Exception)
            {
                PingUntilSuccessfulRecursive(originalState);
            }
        }

        private event EventHandler<EventArgs> _stateChanged;
        public event EventHandler<EventArgs> StateChanged
        {
            add { _stateChanged += value.MakeWeak<EventArgs>(eh => _stateChanged -= eh); }
            remove { }
        }
    }
}
