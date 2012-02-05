using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using MediaControl.Client.WindowsPhone.ViewModels;

namespace MediaControl.Client.WindowsPhone
{
    public partial class AlbumPage : PhoneApplicationPage
    {
        private object _selectedItem;
        private static object _previousDataContext = null;

        public AlbumPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait;
            Loaded += new RoutedEventHandler(MainPage_Loaded);

            PageTransitionList.Completed += new EventHandler(PageTransitionList_Completed);

            // Set the data context of the listbox control to the album list
            //DataContext = new AlbumsViewModel();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Reset page transition
            ResetPageTransitionList.Begin();

            if (DataContext != null)
            {
                ((AlbumViewModel)DataContext).LoadAlbumTracks();
                _previousDataContext = DataContext;
            }
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.Back)
            {
                DataContext = _previousDataContext;
            }

            base.OnNavigatingFrom(e);
        }

        private void PageTransitionList_Completed(object sender, EventArgs e)
        {
            // Set datacontext of details page to selected listbox item
            NavigationService.Navigate(new Uri("/NowPlayingPage.xaml", UriKind.Relative));
        }

        private void ListBoxOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Capture selecte5d item data
            _selectedItem = (sender as ListBox).SelectedItem;

            if (_selectedItem != null)
            {
                // Play the current track and flip to now playing
                MediaService.Current.PlayTrack(_selectedItem as MediaLibrary.Track, null, null);

                // Start page transition animation
                PageTransitionList.Begin();
            }
        }
    }
}