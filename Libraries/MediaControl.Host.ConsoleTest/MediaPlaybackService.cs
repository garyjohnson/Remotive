using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaControl.Common;
using System.ServiceModel;
using MediaControl.Common.Music;

namespace MediaControl.Host.ConsoleTest
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class MediaPlaybackService : IMediaPlaybackService
    {
        public MediaState Play()
        {
            Console.WriteLine("Play");
            return GetMediaState();
        }

        public MediaState Pause()
        {
            Console.WriteLine("Pause");
            return GetMediaState();
        }

        public MediaState NextTrack()
        {
            Console.WriteLine("NextTrack");
            return GetMediaState();
        }

        public MediaState PrevTrack()
        {
            Console.WriteLine("PrevTrack");
            return GetMediaState();
        }

        public Common.Music.Track GetNowPlaying()
        {
            return new Common.Music.Track()
            {
                AlbumName = "Mer De Noms",
                ArtistName = "A Perfect Circle",
                Title = "Judith",
                TrackNumber = 05,
                Duration = new TimeSpan(0, 2, 33),
                FilePath = @"\\homeserver\music\apc-mdn\judith.mp3"
            };
        }

        public MediaState PlayTrack(Track track)
        {
            Console.WriteLine("Play Track");
            return GetMediaState();
        }

        public MediaState GetMediaState()
        {
            return new MediaState()
            {
                CurrentTrack = GetNowPlaying(),
                CurrentTime = DateTime.Now,
                PlaybackPosition = new TimeSpan(0, 0, 33),
                PlaybackState = GetPlaybackState()
            };
        }

        private Common.Music.PlaybackState GetPlaybackState()
        {
            return Common.Music.PlaybackState.Play;
        }

        public MediaPlaybackCapabilities GetCapabilities()
        {
            return new MediaPlaybackCapabilities()
            {
                IsPlaybackMidlistAvailable = true,
                IsToggleLoopAvailable = true,
                IsToggleShuffleAvailable = true
            };
        }

        public MediaState PlayTracks(IEnumerable<Track> trackQueue, bool enqueue)
        {
            throw new NotImplementedException();
        }

        public MediaState PlayArtist(Artist artist, bool enqueue)
        {
            throw new NotImplementedException();
        }

        public MediaState PlayAlbum(Album album, bool enqueue)
        {
            throw new NotImplementedException();
        }


        public void Ping()
        {
            throw new NotImplementedException();
        }


        public MediaState PlayAlbumArtist(AlbumArtist artist, bool enqueue)
        {
            throw new NotImplementedException();
        }
    }
}
