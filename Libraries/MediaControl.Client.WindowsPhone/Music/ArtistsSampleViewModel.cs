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

namespace MediaControl.Client.WindowsPhone.Music
{
    public class ArtistsSampleViewModel : ViewModel
    {
        public ArtistsSampleViewModel()
        {
            LoadArtists();
        }

        public void LoadArtists()
        {
            _artists.Add(new Artist()
            {
                Name = "!!!"
            });

            _artists.Add(new Artist()
            {
                Name = "...And You Will Know Us From The Trail Of Dead"
            });

            _artists.Add(new Artist()
            {
                Name = "Ben Folds Five"
            });

            _artists.Add(new Artist()
            {
                Name = "Beck"
            });

            _artists.Add(new Artist()
            {
                Name = "Calvin Harris"
            });

            _artists.Add(new Artist()
            {
                Name = "Chromeo"
            });

            _artists.Add(new Artist()
            {
                Name = "The Mars Volta"
            });

            _artists.Add(new Artist()
            {
                Name = "System of a Down"
            });

            _artists.Add(new Artist()
            {
                Name = "The Zutons"
            });
        }

        private readonly ObservableCollection<Artist> _artists = new ObservableCollection<Artist>();
        public ObservableCollection<Artist> Artists
        {
            get { return _artists; }
        }
    }
}
