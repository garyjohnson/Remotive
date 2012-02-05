using System;
using System.Collections.Generic;
using MediaControl.Client.WindowsPhone.MediaLibrary;
using Sharkfist.Phone.SilverlightCore;
using System.Collections.ObjectModel;

namespace MediaControl.Client.WindowsPhone
{
    public partial class RequestService
    {
        private delegate void GetAlbumsByArtistDelegate(GetAlbumsByArtistRequest request, object state);
        public void GetAlbumsByArtistAsync(MediaLibrary.Artist artist, Action<List<MediaLibrary.Album>, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<List<MediaLibrary.Album>, object, Exception>, object> innerState =
                    new Tuple<Action<List<MediaLibrary.Album>, object, Exception>, object>(callback, state);
                QueueRequest(new GetAlbumsByArtistDelegate(LibraryServiceClient.GetAlbumsByArtistAsync), new GetAlbumsByArtistRequest(artist), innerState);
            }
        }

        private void LibraryServiceClient_GetAlbumsByArtistCompleted(object sender, GetAlbumsByArtistCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error))
            {
                Tuple<Action<List<MediaLibrary.Album>, object, Exception>, object> innerState = (Tuple<Action<List<MediaLibrary.Album>, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.GetAlbumsByArtistResult, innerState.Item2, null);
                }
            }
        }

        private delegate void GetAlbumArtByTrackDelegate(GetAlbumArtByTrackRequest request, object state);
        public void GetAlbumArtByTrackAsync(MediaLibrary.Track track,
            Action<MediaLibrary.Track, byte[], object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<MediaLibrary.Track, byte[], object, Exception>, object, MediaLibrary.Track> innerState =
                    new Tuple<Action<MediaLibrary.Track, byte[], object, Exception>, object, MediaLibrary.Track>(callback, state, track);
                QueueLowPriorityRequest(new GetAlbumArtByTrackDelegate(LibraryServiceClient.GetAlbumArtByTrackAsync), new GetAlbumArtByTrackRequest(track), innerState);
            }
        }

        public void LibraryServiceClient_GetAlbumArtByTrackCompleted(object sender, GetAlbumArtByTrackCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error))
            {
                Tuple<Action<MediaLibrary.Track, byte[], object, Exception>, object, MediaLibrary.Track> innerState = (Tuple<Action<MediaLibrary.Track, byte[], object, Exception>, object, MediaLibrary.Track>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(innerState.Item3, null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(innerState.Item3, e.Result.GetAlbumArtByTrackResult, innerState.Item2, null);
                }
            }
        }

        private delegate void GetAlbumArtByAlbumDelegate(GetAlbumArtByAlbumRequest request, object state);
        public void GetAlbumArtByAlbumAsync(MediaLibrary.Album album,
            Action<MediaLibrary.Album, byte[], object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<MediaLibrary.Album, byte[], object, Exception>, object, MediaLibrary.Album> innerState =
                    new Tuple<Action<MediaLibrary.Album, byte[], object, Exception>, object, MediaLibrary.Album>(callback, state, album);
                QueueLowPriorityRequest(new GetAlbumArtByAlbumDelegate(LibraryServiceClient.GetAlbumArtByAlbumAsync), new GetAlbumArtByAlbumRequest(album), innerState);
            }
        }

        public void LibraryServiceClient_GetAlbumArtByAlbumCompleted(object sender, GetAlbumArtByAlbumCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error))
            {
                Tuple<Action<MediaLibrary.Album, byte[], object, Exception>, object, MediaLibrary.Album> innerState = (Tuple<Action<MediaLibrary.Album, byte[], object, Exception>, object, MediaLibrary.Album>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(innerState.Item3, null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(innerState.Item3, e.Result.GetAlbumArtByAlbumResult, innerState.Item2, null);
                }
            }
        }

        private delegate void GetTracksDelegate(GetTracksRequest request, object state);
        public void GetTracksAsync(MediaLibrary.Album album,
            Action<List<MediaLibrary.Track>, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<List<MediaLibrary.Track>, object, Exception>, object> innerState =
                    new Tuple<Action<List<MediaLibrary.Track>, object, Exception>, object>(callback, state);
                QueueRequest(new GetTracksDelegate(LibraryServiceClient.GetTracksAsync), new GetTracksRequest(album), innerState);
            }
        }

        private void LibraryServiceClient_GetTracksCompleted(object sender, GetTracksCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error))
            {
                Tuple<Action<List<MediaLibrary.Track>, object, Exception>, object> innerState =
                    (Tuple<Action<List<MediaLibrary.Track>, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.GetTracksResult, innerState.Item2, null);
                }
            }
        }

        private delegate void PingDelegate(MediaLibrary.PingRequest request, object state);
        public void Ping(Action<object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<object, Exception>, object> innerState = new Tuple<Action<object, Exception>, object>(callback, state);
                QueueRequest(new PingDelegate(LibraryServiceClient.PingAsync), new MediaLibrary.PingRequest(), innerState);
            }
        }

        private void LibraryServiceClient_PingCompleted(object sender, MediaLibrary.PingCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error))
            {
                Tuple<Action<object, Exception>, object> innerState = (Tuple<Action<object, Exception>, object>)e.UserState;
                if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(innerState.Item2, e.Error);
                }
            }
        }

        private delegate void GetAlbumArtistsRangeDelegate(GetAlbumArtistRangeRequest request, object state);
        public void GetAlbumArtistsRangeAsync(int index, int count, Action<List<MediaLibrary.AlbumArtist>, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<List<MediaLibrary.AlbumArtist>, object, Exception>, object> innerState =
                    new Tuple<Action<List<MediaLibrary.AlbumArtist>, object, Exception>, object>(callback, state);
                QueueRequest(new GetAlbumArtistsRangeDelegate(LibraryServiceClient.GetAlbumArtistRangeAsync), new GetAlbumArtistRangeRequest(index, count), innerState);
            }
        }

        private void LibraryServiceClient_GetAlbumArtistRangeCompleted(object sender, GetAlbumArtistRangeCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error))
            {
                Tuple<Action<List<MediaLibrary.AlbumArtist>, object, Exception>, object> innerState =
                       (Tuple<Action<List<MediaLibrary.AlbumArtist>, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.GetAlbumArtistRangeResult, innerState.Item2, null);
                }
            }
        }

        private delegate void GetAlbumRangeDelegate(GetAlbumRangeRequest request, object state);
        public void GetAlbumRangeAsync(int index, int count, Action<List<MediaLibrary.Album>, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<List<MediaLibrary.Album>, object, Exception>, object> innerState =
                    new Tuple<Action<List<MediaLibrary.Album>, object, Exception>, object>(callback, state);
                QueueRequest(new GetAlbumRangeDelegate(LibraryServiceClient.GetAlbumRangeAsync), new GetAlbumRangeRequest(index, count), innerState);
            }
        }

        private void LibraryServiceClient_GetAlbumRangeCompleted(object sender, GetAlbumRangeCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error))
            {
                Tuple<Action<List<MediaLibrary.Album>, object, Exception>, object> innerState =
                       (Tuple<Action<List<MediaLibrary.Album>, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.GetAlbumRangeResult, innerState.Item2, null);
                }
            }
        }

        private delegate void GetArtistRangeDelegate(GetArtistRangeRequest request, object state);
        public void GetArtistRangeAsync(int index, int count, Action<List<MediaLibrary.Artist>, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<List<MediaLibrary.Artist>, object, Exception>, object> innerState =
                    new Tuple<Action<List<MediaLibrary.Artist>, object, Exception>, object>(callback, state);
                QueueRequest(new GetArtistRangeDelegate(LibraryServiceClient.GetArtistRangeAsync), new GetArtistRangeRequest(index, count), innerState);
            }
        }

        private void LibraryServiceClient_GetArtistRangeCompleted(object sender, GetArtistRangeCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error))
            {
                Tuple<Action<List<MediaLibrary.Artist>, object, Exception>, object> innerState =
                       (Tuple<Action<List<MediaLibrary.Artist>, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(null, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.GetArtistRangeResult, innerState.Item2, null);
                }
            }
        }

        private delegate void GetArtistCountDelegate(GetArtistCountRequest request, object state);
        public void GetArtistCountAsync(Action<int, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<int, object, Exception>, object> innerState =
                    new Tuple<Action<int, object, Exception>, object>(callback, state);
                QueueRequest(new GetArtistCountDelegate(LibraryServiceClient.GetArtistCountAsync), new GetArtistCountRequest(), innerState);
            }
        }

        private void LibraryServiceClient_GetArtistCountCompleted(object sender, GetArtistCountCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error))
            {
                Tuple<Action<int, object, Exception>, object> innerState =
                       (Tuple<Action<int, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(-1, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.GetArtistCountResult, innerState.Item2, null);
                }
            }
        }

        private delegate void GetAlbumCountDelegate(GetAlbumCountRequest request, object state);
        public void GetAlbumCountAsync(Action<int, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<int, object, Exception>, object> innerState =
                    new Tuple<Action<int, object, Exception>, object>(callback, state);
                QueueRequest(new GetAlbumCountDelegate(LibraryServiceClient.GetAlbumCountAsync), new GetAlbumCountRequest(), innerState);
            }
        }

        private void LibraryServiceClient_GetAlbumCountCompleted(object sender, GetAlbumCountCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error))
            {
                Tuple<Action<int, object, Exception>, object> innerState =
                       (Tuple<Action<int, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(-1, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.GetAlbumCountResult, innerState.Item2, null);
                }
            }
        }

        private delegate void GetAlbumArtistCountDelegate(GetAlbumArtistCountRequest request, object state);
        public void GetAlbumArtistCountAsync(Action<int, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<int, object, Exception>, object> innerState =
                    new Tuple<Action<int, object, Exception>, object>(callback, state);
                QueueRequest(new GetAlbumArtistCountDelegate(LibraryServiceClient.GetAlbumArtistCountAsync), new GetAlbumArtistCountRequest(), innerState);
            }
        }

        private void LibraryServiceClient_GetAlbumArtistCountCompleted(object sender, GetAlbumArtistCountCompletedEventArgs e)
        {
            if (RequestCompleted(e.Error))
            {
                Tuple<Action<int, object, Exception>, object> innerState =
                       (Tuple<Action<int, object, Exception>, object>)e.UserState;
                if (e.Error != null && innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(-1, innerState.Item2, e.Error);
                }
                else if (innerState.Item1 != null)
                {
                    innerState.Item1.Invoke(e.Result.GetAlbumArtistCountResult, innerState.Item2, null);
                }
            }
        }

        private delegate void GetServerVersionDelegate(GetServerVersionRequest request, object state);
        public void GetServerVersionAsync(Action<System.Version, object, Exception> callback, object state)
        {
            lock (_syncLock)
            {
                Tuple<Action<System.Version, object, Exception>, object> innerState =
                    new Tuple<Action<System.Version, object, Exception>, object>(callback, state);
                QueueRequest(new GetServerVersionDelegate(LibraryServiceClient.GetServerVersionAsync), new GetServerVersionRequest(), innerState);
            }
        }

        private void LibraryServiceClient_GetServerVersionCompleted(object sender, GetServerVersionCompletedEventArgs e)
        {
            System.Version version = new System.Version("1.0.0.0");
            if (e.Error != null)
            {
                MediaControl.Client.WindowsPhone.MediaLibrary.Version result = e.Result.GetServerVersionResult;
                version = new System.Version(result._Major, result._Minor, result._Build, result._Revision);
            }
            
            Tuple<Action<System.Version, object, Exception>, object> innerState =
                       (Tuple<Action<System.Version, object, Exception>, object>)e.UserState;
            innerState.Item1.Invoke(version, innerState, e.Error);
        }
    }
}
