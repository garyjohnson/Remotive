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
using MediaControl.Client.WindowsPhone.MediaLibrary;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MediaControl.Client.WindowsPhone.Album
{
    public class AlbumSampleViewModel : ViewModel
    {
        public AlbumSampleViewModel()
        {
            LoadAlbumTracks();
        }

        public MediaLibrary.Album Album
        {
            get 
            {
                return new MediaLibrary.Album()
                {
                    ArtistName = "Chromeo",
                    Genre = "Electronic",
                    ReleaseYear = new DateTime(2006, 1, 1),
                    Title = "Fancy Footwork"
                };
            }
        }

        public void LoadAlbumTracks()
        {
            _tracks.Clear();

            _tracks.Add(new Track()
            {
                TrackNumber = 1,
                AlbumName = "Fancy Footwork",
                ArtistName = "Chromeo",
                Duration = new TimeSpan(0,3,32),
                FilePath = @"D:\Music\Chromeo - Fancy Footwork\01 - Chromeo - Intro.mp3",
                Title = "Intro"
            });

            _tracks.Add(new Track()
            {
                TrackNumber = 2,
                AlbumName = "Fancy Footwork",
                ArtistName = "Chromeo",
                Duration = new TimeSpan(0, 3, 32),
                FilePath = @"D:\Music\Chromeo - Fancy Footwork\02 - Chromeo - Tenderoni.mp3",
                Title = "Tenderoni"
            });

            _tracks.Add(new Track()
            {
                TrackNumber = 3,
                AlbumName = "Fancy Footwork",
                ArtistName = "Chromeo",
                Duration = new TimeSpan(0, 3, 32),
                FilePath = @"D:\Music\Chromeo - Fancy Footwork\03 - Chromeo - Fancy Footwork.mp3",
                Title = "Fancy Footwork"
            });

            _tracks.Add(new Track()
            {
                TrackNumber = 4,
                AlbumName = "Fancy Footwork",
                ArtistName = "Chromeo",
                Duration = new TimeSpan(0, 3, 32),
                FilePath = @"D:\Music\Chromeo - Fancy Footwork\04 - Chromeo - Bonafied Lovin (Tough Guys).mp3",
                Title = "Bonafied Lovin (Tough Guys)"
            });

            _tracks.Add(new Track()
            {
                TrackNumber = 5,
                AlbumName = "Fancy Footwork",
                ArtistName = "Chromeo",
                Duration = new TimeSpan(0, 3, 32),
                FilePath = @"D:\Music\Chromeo - Fancy Footwork\05 - Chromeo - My Girl Is Calling Me (A Liar).mp3",
                Title = "My Girl Is Calling Me (A Liar)"
            });

            _tracks.Add(new Track()
            {
                TrackNumber = 6,
                AlbumName = "Fancy Footwork",
                ArtistName = "Chromeo",
                Duration = new TimeSpan(0, 3, 32),
                FilePath = @"D:\Music\Chromeo - Fancy Footwork\06 - Chromeo - Outta Sight.mp3",
                Title = "Outta Sight"
            });

            _tracks.Add(new Track()
            {
                TrackNumber = 7,
                AlbumName = "Fancy Footwork",
                ArtistName = "Chromeo",
                Duration = new TimeSpan(0, 3, 32),
                FilePath = @"D:\Music\Chromeo - Fancy Footwork\07 - Chromeo - Opening Up (Ce Soir on Danse).mp3",
                Title = "Opening Up (Ce Soir on Danse)"
            });

            _tracks.Add(new Track()
            {
                TrackNumber = 8,
                AlbumName = "Fancy Footwork",
                ArtistName = "Chromeo",
                Duration = new TimeSpan(0, 3, 32),
                FilePath = @"D:\Music\Chromeo - Fancy Footwork\08 - Chromeo - Momma's Boy.mp3",
                Title = "Momma's Boy"
            });

            _tracks.Add(new Track()
            {
                TrackNumber = 9,
                AlbumName = "Fancy Footwork",
                ArtistName = "Chromeo",
                Duration = new TimeSpan(0, 3, 32),
                FilePath = @"D:\Music\Chromeo - Fancy Footwork\09 - Chromeo - Call Me Up.mp3",
                Title = "Call Me Up"
            });

            _tracks.Add(new Track()
            {
                TrackNumber = 10,
                AlbumName = "Fancy Footwork",
                ArtistName = "Chromeo",
                Duration = new TimeSpan(0, 3, 32),
                FilePath = @"D:\Music\Chromeo - Fancy Footwork\10 - Chromeo - Waiting 4 U.mp3",
                Title = "Waiting 4 U"
            });

            _tracks.Add(new Track()
            {
                TrackNumber = 11,
                AlbumName = "Fancy Footwork",
                ArtistName = "Chromeo",
                Duration = new TimeSpan(0, 3, 32),
                FilePath = @"D:\Music\Chromeo - Fancy Footwork\11 - Chromeo - 100%.mp3",
                Title = "100%"
            });
        }

        private readonly ObservableCollection<Track> _tracks = new ObservableCollection<Track>();
        public ObservableCollection<Track> Tracks
        {
            get { return _tracks; }
        }
    }
}
