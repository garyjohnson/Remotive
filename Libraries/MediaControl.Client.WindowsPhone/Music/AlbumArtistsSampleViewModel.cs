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
using MediaControl.Client.WindowsPhone.MediaLibrary;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MediaControl.Client.WindowsPhone.Music
{
    public class AlbumArtistsSampleViewModel : ViewModel
    {
        public AlbumArtistsSampleViewModel()
        {
            LoadArtists();
        }

        public void LoadArtists()
        {
            AlbumArtist chk = new AlbumArtist()
            {
                Name = "!!!",
                Albums = new List<MediaLibrary.Album>()
            };

            chk.Albums.Add(new MediaLibrary.Album()
            {
                 ArtistName = "!!!",
                  Title = "!!!"
            });

            chk.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "!!!",
                Title = "Louden Up Now"
            });

            Artists.Add(chk);

            AlbumArtist trail = new AlbumArtist()
            {
                Name = "...And You Will Know Us By The Trail Of Dead",
                Albums = new List<MediaLibrary.Album>()
            };

            trail.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "...And You Will Know Us By The Trail Of Dead",
                Title = "Festival Thyme"
            });

            trail.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "...And You Will Know Us By The Trail Of Dead",
                Title = "The Century of Self"
            });

            trail.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "...And You Will Know Us By The Trail Of Dead",
                Title = "Worlds Apart"
            });

            Artists.Add(trail);

            AlbumArtist bff = new AlbumArtist()
            {
                Name = "Ben Folds Five",
                Albums = new List<MediaLibrary.Album>()
            };

            bff.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Ben Folds Five",
                Title = "Naked Baby Photos"
            });

            bff.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Ben Folds Five",
                Title = "The Unauthorized Biography of Reinhold Messner"
            });

            bff.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Ben Folds Five",
                Title = "Whatever and Ever Amen"
            });

            bff.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Ben Folds Five",
                Title = "Ben Folds Five"
            });

            Artists.Add(bff);

            AlbumArtist beck = new AlbumArtist()
            {
                Name = "Beck",
                Albums = new List<MediaLibrary.Album>()
            };

            beck.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Beck",
                Title = "Guero"
            }); 
            
            beck.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Beck",
                Title = "Guerolito"
            });

            beck.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Beck",
                Title = "Mellow Gold"
            });

            beck.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Beck",
                Title = "Midnight Vultures"
            });

            beck.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Beck",
                Title = "Modern Guilt"
            });

            beck.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Beck",
                Title = "Mutations"
            });

            beck.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Beck",
                Title = "Odelay"
            });

            beck.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Beck",
                Title = "Sea Change"
            });

            beck.Albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "Beck",
                Title = "The Information"
            });

            Artists.Add(beck);
        }

        private readonly ObservableCollection<AlbumArtist> _artists = new ObservableCollection<AlbumArtist>();
        public ObservableCollection<AlbumArtist> Artists
        {
            get { return _artists; }
        }
    }
}
