using System;
namespace MediaControl.Client.WindowsPhone.Services
{
    interface IMediaStateService
    {
        event EventHandler<EventArgs> AlbumArtChanged;
        System.Windows.Media.ImageSource CurrentAlbumArt { get; set; }
        MediaControl.Client.WindowsPhone.MediaPlayback.MediaState CurrentMediaState { get; set; }
        TimeSpan CurrentTimeRemaining { get; set; }
        int EstimatedPlaybackPercent { get; set; }
        TimeSpan EstimatedPlaybackProgress { get; set; }
        event EventHandler<EventArgs> MediaStateChanged;
        event EventHandler<EventArgs> ProgressChanged;
        void StartUpdating();
        void StopUpdating();
        void RefreshState();
    }
}
