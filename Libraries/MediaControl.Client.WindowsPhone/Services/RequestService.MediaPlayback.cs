using System;
using System.Collections.Generic;
using MediaControl.Client.WindowsPhone.MediaPlayback;
using Sharkfist.Phone.SilverlightCore;
using System.Collections.ObjectModel;

namespace MediaControl.Client.WindowsPhone
{
    public partial class RequestService
    {
        private delegate void GetMediaStateDelegate(GetMediaStateRequest request, object state);
        public void GetMediaStateAsync(Action<MediaState, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<MediaState, object, Exception>, object> pair =
                    new Tuple<Action<MediaState, object, Exception>, object>(callback, state);
                QueueLowPriorityRequest(new GetMediaStateDelegate(PlaybackServiceClient.GetMediaStateAsync), new GetMediaStateRequest(), pair);
            }
        }

        private void PlaybackServiceClient_GetMediaStateCompleted(object sender, GetMediaStateCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error, true))
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState =
                       (Tuple<Action<MediaState, object, Exception>, object>)e.UserState;

                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.GetMediaStateResult, innerState.Item2, null);
                }
            }
        }

        private delegate void PlayTrackDelegate(PlayTrackRequest request, object state);
        public void PlayTrack(MediaLibrary.Track track,
            Action<MediaState, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = new Tuple<Action<MediaState, object, Exception>, object>(callback, state);
                QueueRequest(new PlayTrackDelegate(PlaybackServiceClient.PlayTrackAsync), new PlayTrackRequest(track.ConvertToTrack()), innerState);
            }
        }

        private void PlaybackServiceClient_PlayTrackCompleted(object sender, PlayTrackCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error, true))
            {
                Tuple<Action<MediaState, object, Exception>, object> innerPair = (Tuple<Action<MediaState, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerPair.Item1 != null)
                {
                    innerPair.Item1.Invoke(null, innerPair.Item2, e.Error);
                }
                else if (innerPair.Item1 != null)
                {
                    innerPair.Item1.Invoke(e.Result.PlayTrackResult, innerPair.Item2, null);
                }
            }
        }

        private delegate void PlayDelegate(PlayRequest request, object state);
        public void PlayAsync(Action<MediaState, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = new Tuple<Action<MediaState, object, Exception>, object>(callback, state);
                QueueRequest(new PlayDelegate(PlaybackServiceClient.PlayAsync), new PlayRequest(), innerState);
            }
        }

        private void PlaybackServiceClient_PlayCompleted(object sender, PlayCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error, true))
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = (Tuple<Action<MediaState, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.PlayResult, innerState.Item2, null);
                }
            }
        }

        private delegate void PauseDelegate(PauseRequest request, object state);
        public void PauseAsync(Action<MediaState, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = new Tuple<Action<MediaState, object, Exception>, object>(callback, state);
                QueueRequest(new PauseDelegate(PlaybackServiceClient.PauseAsync), new PauseRequest(), innerState);
            }
        }

        private void PlaybackServiceClient_PauseCompleted(object sender, PauseCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error, true))
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = (Tuple<Action<MediaState, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.PauseResult, innerState.Item2, null);
                }
            }
        }

        private delegate void NextTrackDelegate(NextTrackRequest request, object state);
        public void NextTrackAsync(Action<MediaState, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = new Tuple<Action<MediaState, object, Exception>, object>(callback, state);
                QueueRequest(new NextTrackDelegate(PlaybackServiceClient.NextTrackAsync), new NextTrackRequest(), innerState);
            }
        }

        private void PlaybackServiceClient_NextTrackCompleted(object sender, NextTrackCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error, true))
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = (Tuple<Action<MediaState, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.NextTrackResult, innerState.Item2, null);
                }
            }
        }

        private delegate void PreviousTrackDelegate(PrevTrackRequest request, object state);
        public void PreviousTrackAsync(Action<MediaState, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = new Tuple<Action<MediaState, object, Exception>, object>(callback, state);
                QueueRequest(new PreviousTrackDelegate(PlaybackServiceClient.PrevTrackAsync), new PrevTrackRequest(), innerState);
            }
        }

        private void PlaybackServiceClient_PrevTrackCompleted(object sender, PrevTrackCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error, true))
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = (Tuple<Action<MediaState, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.PrevTrackResult, innerState.Item2, null);
                }
            }
        }

        private delegate void PlayTracksDelegate(PlayTracksRequest request, object state);
        public void PlayTracks(IEnumerable<MediaLibrary.Track> queueTracks,
            bool enqueue, Action<MediaState, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                List<MediaPlayback.Track> playbackTracks = new List<MediaPlayback.Track>();
                foreach (MediaLibrary.Track libraryTrack in queueTracks)
                {
                    playbackTracks.Add(libraryTrack.ConvertToTrack());
                }

                Tuple<Action<MediaState, object, Exception>, object> innerState = new Tuple<Action<MediaState, object, Exception>, object>(callback, state);
                QueueRequest(new PlayTracksDelegate(PlaybackServiceClient.PlayTracksAsync), new PlayTracksRequest(playbackTracks, enqueue), innerState);
            }
        }

        private void PlaybackServiceClient_PlayTracksCompleted(object sender, PlayTracksCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error, true))
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = (Tuple<Action<MediaState, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.PlayTracksResult, innerState.Item2, null);
                }
            }
        }

        private delegate void PlayArtistDelegate(PlayArtistRequest request, object state);
        public void PlayArtist(MediaLibrary.Artist artist, bool enqueue,
            Action<MediaState, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = new Tuple<Action<MediaState, object, Exception>, object>(callback, state);
                QueueRequest(new PlayArtistDelegate(PlaybackServiceClient.PlayArtistAsync), new PlayArtistRequest(artist.ConvertToArtist(), enqueue), innerState);
            }
        }

        private void PlaybackServiceClient_PlayArtistCompleted(object sender, PlayArtistCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error, true))
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = (Tuple<Action<MediaState, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.PlayArtistResult, innerState.Item2, null);
                }
            }
        }

        private delegate void PlayAlbumArtistDelegate(PlayAlbumArtistRequest request, object state);
        public void PlayAlbumArtist(MediaLibrary.AlbumArtist artist, bool enqueue,
            Action<MediaState, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = new Tuple<Action<MediaState, object, Exception>, object>(callback, state);
                QueueRequest(new PlayAlbumArtistDelegate(PlaybackServiceClient.PlayAlbumArtistAsync), new PlayAlbumArtistRequest(artist.ConvertToArtist(), enqueue), innerState);
            }
        }

        private void PlaybackServiceClient_PlayAlbumArtistCompleted(object sender, PlayAlbumArtistCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error, true))
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = (Tuple<Action<MediaState, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.PlayAlbumArtistResult, innerState.Item2, null);
                }
            }
        }

        private delegate void PlayAlbumDelegate(PlayAlbumRequest request, object state);
        public void PlayAlbum(MediaLibrary.Album album, bool enqueue,
            Action<MediaState, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = new Tuple<Action<MediaState, object, Exception>, object>(callback, state);
                QueueRequest(new PlayAlbumDelegate(PlaybackServiceClient.PlayAlbumAsync), new PlayAlbumRequest(album.ConvertToAlbum(), enqueue), innerState);
            }
        }

        private void PlaybackServiceClient_PlayAlbumCompleted(object sender, PlayAlbumCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error, true))
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = (Tuple<Action<MediaState, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.PlayAlbumResult, innerState.Item2, null);
                }
            }
        }

        private delegate void SetVolumeDelegate(SetVolumeRequest request, object state);
        public void SetVolumeAsync(double volume, Action<MediaState, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = new Tuple<Action<MediaState, object, Exception>, object>(callback, state);
                QueueRequest(new SetVolumeDelegate(PlaybackServiceClient.SetVolumeAsync), new SetVolumeRequest(volume), innerState);
            }
        }

        private void PlaybackServiceClient_SetVolumeCompleted(object sender, SetVolumeCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error))
            {
                Tuple<Action<MediaState, object, Exception>, object> innerState = (Tuple<Action<MediaState, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.SetVolumeResult, innerState.Item2, null);
                }
            }
        }
    }
}
