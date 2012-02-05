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
using Microsoft.Phone.Marketplace;

namespace MediaControl.Client.WindowsPhone.Music
{
    [DataContract()]
    public class AlbumsViewModel : ViewModel, IVirtualizingListSource<MediaLibrary.Album>
    {
        public AlbumsViewModel() { }

        public AlbumsViewModel(Artist artist)
        {
            _artist = artist;
        }

        private Artist _artist = null;

        private VirtualizingList<MediaLibrary.Album> _albums = null;
        [DataMember()]
        public VirtualizingList<MediaLibrary.Album> Albums
        {
            get
            {
                return _albums;
            }
            set
            {
                _albums = value;
                value.ListSource = this;
                NotifyPropertyChanged("Albums");
            }
        }

        public override void Refresh()
        {
            LoadAlbums();
        }

        public void LoadAlbums()
        {
            GetCountAsync();
        }

        private int? _albumCount = null;
        [DataMember()]
        public int? AlbumCount
        {
            get { return _albumCount; }
            set { _albumCount = value; }
        }

        [IgnoreDataMember()]
        int IVirtualizingListSource<MediaLibrary.Album>.Count
        {
            get
            {
                if (!_albumCount.HasValue)
                {
                    throw new InvalidOperationException("Count must be initialized before accessing.");
                }

                return _albumCount.Value;
            }
        }

        void IVirtualizingListSource<MediaLibrary.Album>.GetRangeAsync(int startIndex, int count)
        {
            IsBusy = true;
            App.GetService<IRequestService>().GetAlbumRangeAsync(startIndex, count,
                (albums, state, error) =>
                {
                    IsBusy = false;
                    if (error == null && GetRangeCompleted != null)
                    {
                        GetRangeCompleted(this, new GetRangeCompletedArgs<MediaLibrary.Album>(startIndex, count, albums));
                    }
                },
                null);
        }

        private void GetCountAsync()
        {
            IsBusy = true;
            App.GetService<IRequestService>().GetAlbumCountAsync(
                (result, state, error) =>
                {
                    IsBusy = false;
                    if (error == null)
                    {
                        if (new LicenseInformation().IsTrialOrTest() && result > 50)
                        {
                            _albumCount = 50;
                        }
                        else
                        {
                            _albumCount = result;
                        }
                        IsBusy = true;
                        this.Albums = new VirtualizingList<MediaLibrary.Album>(this);
                    }
                },
                null);
        }

        public event EventHandler<GetRangeCompletedArgs<MediaLibrary.Album>> GetRangeCompleted;
    }
}
