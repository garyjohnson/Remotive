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
using Sharkfist.Phone.SilverlightCore;
using System.Runtime.Serialization;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone.Albums
{
    [DataContract()]
    public class ArtistAlbumsViewModel : ViewModel
    {
        public ArtistAlbumsViewModel() { }

        public ArtistAlbumsViewModel(Artist artist)
        {
            _artist = artist;
        }

        private Artist _artist = null;
        [DataMember()]
        public Artist Artist
        {
            get { return _artist; }
            set
            {
                _artist = value;
                NotifyPropertyChanged("Artist");
            }
        }

        private ObservableCollection<MediaLibrary.Album> _albums = new ObservableCollection<MediaLibrary.Album>();
        [DataMember()]
        public ObservableCollection<MediaLibrary.Album> Albums
        {
            get
            {
                return _albums;
            }
            set
            {
                _albums = value;
                NotifyPropertyChanged("Albums");
            }
        }

        public override void Refresh()
        {
            LoadAlbums();
        }

        public void LoadAlbums()
        {
            if (_artist != null)
            {
                IsBusy = true;
                App.GetService<IRequestService>().GetAlbumsByArtistAsync(_artist, (albums, state, error) =>
                {
                    IsBusy = false;
                    if (error == null)
                    {
                        ObservableCollection<MediaLibrary.Album> albumsTemp = new ObservableCollection<MediaLibrary.Album>();
                        foreach(MediaLibrary.Album album in albums)
                        {
                            albumsTemp.Add(album);
                        }

                        Albums = albumsTemp;
                    }
                }, null);
            }
        }
    }
}