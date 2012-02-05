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
using Sharkfist.Phone.SilverlightCore;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.Serialization;
using MediaControl.Client.WindowsPhone.Services;
using Microsoft.Phone.Marketplace;

namespace MediaControl.Client.WindowsPhone.Music
{
    [DataContract()]
    public class AlbumArtistsViewModel : ViewModel, IVirtualizingListSource<AlbumArtist>
    {
        public AlbumArtistsViewModel() { }

        private VirtualizingList<AlbumArtist> _artists = null;
        [DataMember()]
        public VirtualizingList<AlbumArtist> Artists
        {
            get
            {
                return _artists;
            }
            set
            {
                _artists = value;
                value.ListSource = this;
                NotifyPropertyChanged("Artists");
            }
        }

        public override void Refresh()
        {
            LoadArtists();
        }

        public void LoadArtists()
        {
            GetCountAsync();
        }

        private int? _artistCount = null;
        [DataMember()]
        public int? ArtistCount
        {
            get { return _artistCount; }
            set { _artistCount = value; }
        }

        [IgnoreDataMember()]
        int IVirtualizingListSource<AlbumArtist>.Count
        {
            get
            {
                if (!_artistCount.HasValue)
                {
                    throw new InvalidOperationException("Count must be initialized before accessing.");
                }

                return _artistCount.Value;
            }
        }

        void IVirtualizingListSource<AlbumArtist>.GetRangeAsync(int startIndex, int count)
        {
            IsBusy = true;
            App.GetService<IRequestService>().GetAlbumArtistsRangeAsync(startIndex, count,
                (artists, state, error) =>
                {
                    IsBusy = false;
                    if (error == null && GetRangeCompleted != null)
                    {
                        GetRangeCompleted(this, new GetRangeCompletedArgs<AlbumArtist>(startIndex, count, artists));
                    }
                },
                null);
        }

        private void GetCountAsync()
        {
            IsBusy = true;
            App.GetService<IRequestService>().GetAlbumArtistCountAsync(
                (result, state, error) =>
                {
                    IsBusy = false;
                    if (error == null)
                    {
                        if (new LicenseInformation().IsTrialOrTest() && result > 50)
                        {
                            _artistCount = 50;
                        }
                        else
                        {
                            _artistCount = result;
                        }
                        this.Artists = new VirtualizingList<AlbumArtist>(this);
                    }
                },
                null);
        }

        public event EventHandler<GetRangeCompletedArgs<AlbumArtist>> GetRangeCompleted;
    }
}
