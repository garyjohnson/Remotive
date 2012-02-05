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

namespace MediaControl.Client.WindowsPhone.Albums
{
    public class ArtistAlbumsSampleViewModel : ViewModel
    {
        public ArtistAlbumsSampleViewModel()
        {
            Artist = new MediaLibrary.Artist()
            {
                Name = "TOOL"
            };
            LoadAlbums();
        }

        public ArtistAlbumsSampleViewModel(Artist artist)
        {
            _artist = artist;
            LoadAlbums();
        }

        public void LoadAlbums()
        {
            _albums.Clear();
            _albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "TOOL",
                Title = "10,000 Days"
            });

            _albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "TOOL",
                Title = "Lateralus"
            });

            _albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "TOOL",
                Title = "Ænima"
            });

            _albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "TOOL",
                Title = "Opiate"
            });

            _albums.Add(new MediaLibrary.Album()
            {
                ArtistName = "TOOL",
                Title = "Salival"
            });
        }

        private Artist _artist = null;
        public Artist Artist
        {
            get { return _artist; }
            set
            {
                _artist = value;
                NotifyPropertyChanged("Artist");
            }
        }

        private readonly ObservableCollection<MediaLibrary.Album> _albums = new ObservableCollection<MediaLibrary.Album>();
        public ObservableCollection<MediaLibrary.Album> Albums
        {
            get { return _albums; }
        }
    }
}
