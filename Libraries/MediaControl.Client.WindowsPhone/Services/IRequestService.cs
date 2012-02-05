using System;
namespace MediaControl.Client.WindowsPhone.Services
{
    interface IRequestService
    {
        void FireRefresh();
        void GetAlbumArtByAlbumAsync(MediaControl.Client.WindowsPhone.MediaLibrary.Album album, Action<MediaControl.Client.WindowsPhone.MediaLibrary.Album, byte[], object, Exception> callback, object state);
        void GetAlbumArtByTrackAsync(MediaControl.Client.WindowsPhone.MediaLibrary.Track track, Action<MediaControl.Client.WindowsPhone.MediaLibrary.Track, byte[], object, Exception> callback, object state);
        void GetAlbumArtistCountAsync(Action<int, object, Exception> callback, object state);
        void GetAlbumArtistsRangeAsync(int index, int count, Action<System.Collections.Generic.List<MediaControl.Client.WindowsPhone.MediaLibrary.AlbumArtist>, object, Exception> callback, object state);
        void GetAlbumCountAsync(Action<int, object, Exception> callback, object state);
        void GetAlbumRangeAsync(int index, int count, Action<System.Collections.Generic.List<MediaControl.Client.WindowsPhone.MediaLibrary.Album>, object, Exception> callback, object state);
        void GetAlbumsByArtistAsync(MediaControl.Client.WindowsPhone.MediaLibrary.Artist artist, Action<System.Collections.Generic.List<MediaControl.Client.WindowsPhone.MediaLibrary.Album>, object, Exception> callback, object state);
        void GetArtistCountAsync(Action<int, object, Exception> callback, object state);
        void GetArtistRangeAsync(int index, int count, Action<System.Collections.Generic.List<MediaControl.Client.WindowsPhone.MediaLibrary.Artist>, object, Exception> callback, object state);
        void GetMediaStateAsync(Action<MediaControl.Client.WindowsPhone.MediaPlayback.MediaState, object, Exception> callback, object state);
        void GetServerVersionAsync(Action<Version, object, Exception> callback, object state);
        void GetTracksAsync(MediaControl.Client.WindowsPhone.MediaLibrary.Album album, Action<System.Collections.Generic.List<MediaControl.Client.WindowsPhone.MediaLibrary.Track>, object, Exception> callback, object state);
        bool IsBusy { get; set; }
        void LibraryServiceClient_GetAlbumArtByAlbumCompleted(object sender, MediaControl.Client.WindowsPhone.MediaLibrary.GetAlbumArtByAlbumCompletedEventArgs e);
        void LibraryServiceClient_GetAlbumArtByTrackCompleted(object sender, MediaControl.Client.WindowsPhone.MediaLibrary.GetAlbumArtByTrackCompletedEventArgs e);
        void NextTrackAsync(Action<MediaControl.Client.WindowsPhone.MediaPlayback.MediaState, object, Exception> callback, object state);
        void PauseAsync(Action<MediaControl.Client.WindowsPhone.MediaPlayback.MediaState, object, Exception> callback, object state);
        void Ping(Action<object, Exception> callback, object state);
        void PingUntilSuccessful(Action<object> callback, object state);
        void PlayAlbum(MediaControl.Client.WindowsPhone.MediaLibrary.Album album, bool enqueue, Action<MediaControl.Client.WindowsPhone.MediaPlayback.MediaState, object, Exception> callback, object state);
        void PlayAlbumArtist(MediaControl.Client.WindowsPhone.MediaLibrary.AlbumArtist artist, bool enqueue, Action<MediaControl.Client.WindowsPhone.MediaPlayback.MediaState, object, Exception> callback, object state);
        void PlayArtist(MediaControl.Client.WindowsPhone.MediaLibrary.Artist artist, bool enqueue, Action<MediaControl.Client.WindowsPhone.MediaPlayback.MediaState, object, Exception> callback, object state);
        void PlayAsync(Action<MediaControl.Client.WindowsPhone.MediaPlayback.MediaState, object, Exception> callback, object state);
        void PlayTrack(MediaControl.Client.WindowsPhone.MediaLibrary.Track track, Action<MediaControl.Client.WindowsPhone.MediaPlayback.MediaState, object, Exception> callback, object state);
        void PlayTracks(System.Collections.Generic.IEnumerable<MediaControl.Client.WindowsPhone.MediaLibrary.Track> queueTracks, bool enqueue, Action<MediaControl.Client.WindowsPhone.MediaPlayback.MediaState, object, Exception> callback, object state);
        void PreviousTrackAsync(Action<MediaControl.Client.WindowsPhone.MediaPlayback.MediaState, object, Exception> callback, object state);
        event EventHandler<EventArgs> Refresh;
        void ResetClients();
        void SetVolumeAsync(double volume, Action<MediaControl.Client.WindowsPhone.MediaPlayback.MediaState, object, Exception> callback, object state);
        event EventHandler<EventArgs> StateChanged;
        void Teardown();
    }
}
