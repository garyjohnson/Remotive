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
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;
using System.Windows.Data;
using Sharkfist.Phone.SilverlightCore;
using System.Diagnostics;
using System.ComponentModel;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone.Album
{
    public partial class AlbumPage : MediaPhonePage, IStateHandler
    {
        private object _selectedItem;

        public AlbumPage()
        {
            InitializeComponent();

            if (!Designer.IsInDesignMode)
            {
                Loaded += new RoutedEventHandler(MainPage_Loaded);
                _albumArtDownloadedHandler = new EventHandler<EventArgs<MediaLibrary.Album, System.Windows.Media.Imaging.BitmapImage>>(Instance_AlbumArtDownloaded);
            }
            else
            {
                albumImage.Source = new BitmapImage(new Uri("/MediaControl.Client.WindowsPhone;component/Images/SampleData.jpg"));
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext == null)
            {
                MediaLibrary.Album album = Serializer.DeserializeFromQueryString<MediaLibrary.Album>(NavigationContext.QueryString);
                DataContext = new AlbumViewModel(album);
                ((AlbumViewModel)DataContext).LoadAlbumTracks();
            }

            AlbumArtCache.Instance.AlbumArtDownloaded += _albumArtDownloadedHandler;

            AlbumViewModel viewModel = (AlbumViewModel)DataContext;
            AlbumArtCache.Instance.GetAlbumArtAsync(viewModel.Album);
        }

        private EventHandler<EventArgs<MediaLibrary.Album, System.Windows.Media.Imaging.BitmapImage>> _albumArtDownloadedHandler = null;
        public void Instance_AlbumArtDownloaded(object sender, EventArgs<MediaLibrary.Album, System.Windows.Media.Imaging.BitmapImage> e)
        {
            if (e.Value1.ID == ((AlbumViewModel)DataContext).Album.ID)
            {
                albumImage.Source = e.Value2;
            }
        }

        private void ListBoxOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Capture selecte5d item data
            _selectedItem = (sender as ListBox).SelectedItem;

            if (_selectedItem != null)
            {
                // Play the current track and flip to now playing
                App.GetService<IRequestService>().PlayTrack((MediaLibrary.Track)_selectedItem, (mediaState, state, error) =>
                {
                    // Start page transition animation
                    string navigatedUrl = "/NowPlaying/NowPlayingPage.xaml";
                    NavigationService.Navigate(new Uri(navigatedUrl, UriKind.Relative));
                }, null);
            }
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            App.GetService<IRequestService>().PlayTrack((MediaLibrary.Track)sender,
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
            List<MediaLibrary.Track> tracks = new List<MediaLibrary.Track>();
            tracks.Add((MediaLibrary.Track)sender);
            App.GetService<IRequestService>().PlayTracks(tracks, true,
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
                    TrackListViewItem trackItem = container.FindVisualChild<TrackListViewItem>();
                    if (trackItem != null)
                    {
                        Sharkfist.Phone.SilverlightCore.ContextMenu menu = Sharkfist.Phone.SilverlightCore.ContextMenu.GetContextMenu(trackItem);
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
            // Restoring from tombstoning base.OnNavigatedTo(e);
            if (!e.ClearData)
            {
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