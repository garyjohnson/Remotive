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
using System.Runtime.Serialization;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone.Album
{
    [DataContract()]
    public class AlbumViewModel : ViewModel
    {
        public AlbumViewModel(MediaLibrary.Album album)
        {
            Album = album;
        }

        private MediaLibrary.Album _album;
        [DataMember()]
        public MediaLibrary.Album Album
        {
            get { return (MediaLibrary.Album)_album; }
            set
            {
                _album = value;
                NotifyPropertyChanged("Album");
            }
        }

        public override void Refresh()
        {
            LoadAlbumTracks();
        }

        public void LoadAlbumTracks()
        {
            IsBusy = true;

            App.GetService<IRequestService>().GetTracksAsync(_album, (tracks, state, error) =>
                {
                    IsBusy = false;

                    if (error == null)
                    {
                        Tracks.Clear();
                        foreach (Track track in tracks)
                        {
                            Tracks.Add(track);
                        }
                    }
                }, null);
        }

        private ObservableCollection<Track> _tracks = new ObservableCollection<Track>();
        [DataMember()]
        public ObservableCollection<Track> Tracks
        {
            get { return _tracks; }
            set { _tracks = value; }
        }
    }
}
