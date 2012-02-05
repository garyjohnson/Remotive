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
using MediaControl.Client.WindowsPhone.MediaLibrary;
using Microsoft.Phone.Shell;
using System.Windows.Data;
using Sharkfist.Phone.SilverlightCore;
using System.ComponentModel;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone.Albums
{
    public partial class AlbumsPage : MediaPhonePage, IStateHandler
    {
        private object _selectedItem;

        public AlbumsPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the album list
            if (!Designer.IsInDesignMode)
            {
                Loaded += new RoutedEventHandler(MainPage_Loaded);
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext == null)
            {
                MediaLibrary.Artist artist = Serializer.DeserializeFromQueryString<MediaLibrary.Artist>(NavigationContext.QueryString);
                DataContext = new ArtistAlbumsViewModel(artist);
                ((ArtistAlbumsViewModel)DataContext).LoadAlbums();
            }
        }

        private void ListBoxOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Capture selected item data
            _selectedItem = (sender as ListBox).SelectedItem;

            if (_selectedItem != null)
            {
                // Start page transition animation
                string navigatedUrl = string.Format("/Album/AlbumPage.xaml?{0}", Serializer.SerializeToQueryString<MediaLibrary.Album>(_selectedItem));
                NavigationService.Navigate(new Uri(navigatedUrl, UriKind.Relative));
            }
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            App.GetService<IRequestService>().PlayAlbum((MediaLibrary.Album)sender,
                false,
                (media, state, error) =>
                {
                    if (error == null)
                    {
                        App.GetService<IMediaStateService>().CurrentMediaState = media;
                    }
                }, null);
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            App.GetService<IRequestService>().PlayAlbum((MediaLibrary.Album)sender,
                true,
                (media, state, error) =>
                {
                    if (error == null)
                    {
                        App.GetService<IMediaStateService>().CurrentMediaState = media;
                    }
                }, null);
        }

        private void _nowPlayingMenuItem_Click(object sender, EventArgs e)
        {
            string navigatedUrl = "/NowPlaying/NowPlayingPage.xaml";
            NavigationService.Navigate(new Uri(navigatedUrl, UriKind.Relative));
        }

        private ApplicationBarMenuItem _nowPlayingMenuItem;

        protected override void BuildApplicationBar(Microsoft.Phone.Shell.IApplicationBar appBar)
        {
            _nowPlayingMenuItem = new ApplicationBarMenuItem("now playing");
            _nowPlayingMenuItem.Click += new EventHandler(_nowPlayingMenuItem_Click);
            ApplicationBar.MenuItems.Add(_nowPlayingMenuItem);
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            bool handled = false;

            foreach (object item in ListBoxOne.Items)
            {
                ListBoxItem container = ListBoxOne.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                if (container != null)
                {
                    AlbumListViewItem albumItem = container.FindVisualChild<AlbumListViewItem>();
                    if (albumItem != null)
                    {
                        Sharkfist.Phone.SilverlightCore.ContextMenu menu = Sharkfist.Phone.SilverlightCore.ContextMenu.GetContextMenu(albumItem);
                        if (menu != null)
                        {
                            handled |= menu.HandleBackButton();
                        }
                    }
                }
            }

            e.Cancel = handled;

            base.OnBackKeyPress(e);
        }

        public void Launching(object sender, LaunchingEventArgs e)
        {
        }

        private RoutedEventHandler _loadedHandler;
        public void Activated(object sender, ActivatedDataEventArgs e)
        {
            if (!e.ClearData)
            {
                // Restoring from tombstoning base.OnNavigatedTo(e);
                object context = null;
                if (Configuration.TryLoadStateSetting<object>("DataContext", out context))
                {
                    DataContext = context;
                }

                _loadedHandler = new RoutedEventHandler((innerSender, args) =>
                {
                    double vscroll = 0;
                    if (Configuration.TryLoadStateSetting<double>("VScrollPosition", out vscroll))
                    {
                        //System.Diagnostics.Debug.WriteLine("VOffset: " + vscroll);
                        ListBoxOne.SetVerticalScrollOffset(vscroll);
                        Loaded -= _loadedHandler;
                    }
                });

                Loaded += _loadedHandler;
            }
        }

        public void Deactivated(object sender, DeactivatedEventArgs e)
        {
            // save state
            double vscroll = ListBoxOne.GetVerticalScrollOffset();
            Configuration.SaveStateSetting<object>("DataContext", DataContext);
            Configuration.SaveStateSetting<double>("VScrollPosition", vscroll);
        }

        public void Closing(object sender, ClosingEventArgs e)
        {
        }
    }
}