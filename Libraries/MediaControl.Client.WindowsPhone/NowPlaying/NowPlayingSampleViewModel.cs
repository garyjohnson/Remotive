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

namespace MediaControl.Client.WindowsPhone.NowPlaying
{
    public class NowPlayingSampleViewModel : ViewModel
    {
        public NowPlayingSampleViewModel() 
        {
        }

        public ImageSource CurrentAlbumArt
        {
            get { return (ImageSource)new BitmapImage(new Uri(@"/MediaControl.Client.WindowsPhone;component/Images/SampleData.jpg", UriKind.Relative)); }
        }

        public int EstimatedPlaybackPercent
        {
            get { return 80; }
        }

        public TimeSpan EstimatedPlaybackProgress
        {
            get { return TimeSpan.FromSeconds(124); }
        }

        public TimeSpan CurrentTimeRemaining
        {
            get { return TimeSpan.FromSeconds(32); }
        }

        public MediaState CurrentMediaState
        {
            get 
            {
                return new MediaState()
                {
                    CurrentTrack = new Track()
                    {
                        AlbumName = "Fancy Footwork",
                        ArtistName = "Chromeo",
                        Duration = TimeSpan.FromSeconds(156),
                        Title = "Tenderoni",
                        TrackNumber = 2,
                        FilePath = ""
                    },
                    PlaybackState = PlaybackState.Play,
                    PlaybackPosition = TimeSpan.FromSeconds(124),
                    CurrentTime = DateTime.Now
                };
            }
        }
    }
}
