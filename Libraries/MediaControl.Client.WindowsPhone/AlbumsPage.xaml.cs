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
using System.Windows.Navigation;
using MediaControl.Client.WindowsPhone.ViewModels;
using MediaControl.Client.WindowsPhone.MediaLibrary;

namespace MediaControl.Client.WindowsPhone 
{
    public partial class AlbumsPage : PhoneApplicationPage
    {
        private object _selectedItem;

        public AlbumsPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait;
            Loaded += new RoutedEventHandler(MainPage_Loaded);

            PageTransitionList.Completed += new EventHandler(PageTransitionList_Completed);
            
            // Set the data context of the listbox control to the album list
            DataContext = new AlbumsViewModel();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ((AlbumsViewModel)DataContext).Albums.Clear();
            ((AlbumsViewModel)DataContext).LoadAlbums();

            // Reset page transition
            ResetPageTransitionList.Begin();
        }

        private void PageTransitionList_Completed(object sender, EventArgs e)
        {
            // Set datacontext of details page to selected listbox item
            NavigationService.Navigate(new Uri("/AlbumPage.xaml", UriKind.Relative));
            FrameworkElement root = Application.Current.RootVisual as FrameworkElement;
            root.DataContext = new AlbumViewModel(_selectedItem as Album);
        }

        private void ListBoxOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Capture selected item data
            _selectedItem = (sender as ListBox).SelectedItem;

            if (_selectedItem != null)
            {
                // Start page transition animation
                PageTransitionList.Begin();
            }
        }
    }
}