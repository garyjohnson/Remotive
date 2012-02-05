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
using Sharkfist.Phone.SilverlightCore;
using System.Diagnostics;
using System.Text;
using System.Runtime.Serialization;
using MediaControl.Client.WindowsPhone.Services;
using Microsoft.Phone.Marketplace;

namespace MediaControl.Client.WindowsPhone.Music
{
    [DataContract()]
    public class ArtistsViewModel : ViewModel, IVirtualizingListSource<MediaLibrary.Artist>
    {
        public ArtistsViewModel() 
        {
        }

        private VirtualizingList<MediaLibrary.Artist> _artists = null;
        [DataMember()]
        public VirtualizingList<MediaLibrary.Artist> Artists
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
        int IVirtualizingListSource<MediaLibrary.Artist>.Count
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

        void IVirtualizingListSource<MediaLibrary.Artist>.GetRangeAsync(int startIndex, int count)
        {
            IsBusy = true;
            App.GetService<IRequestService>().GetArtistRangeAsync(startIndex, count,
                (artists, state, error) =>
                {
                    IsBusy = false;
                    if (error == null && GetRangeCompleted != null)
                    {
                        GetRangeCompleted(this, new GetRangeCompletedArgs<MediaLibrary.Artist>(startIndex, count, artists));
                    }
                },
                null);
        }

        private void GetCountAsync()
        {
            IsBusy = true;
            App.GetService<IRequestService>().GetArtistCountAsync(
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
                        this.Artists = new VirtualizingList<MediaLibrary.Artist>(this);
                    }
                },
                null);
        }

        public event EventHandler<GetRangeCompletedArgs<MediaLibrary.Artist>> GetRangeCompleted;
    }
}
