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
using MediaControl.Client.WindowsPhone.MediaLibrary;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Windows.Data;
using MediaControl.Client.WindowsPhone.Albums;
using Sharkfist.Phone.SilverlightCore;
using System.Diagnostics;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone.Music
{
    public partial class AlbumsPanel : NavigationPanel
    {
        public AlbumsPanel()
        {
            InitializeComponent();
        }

        public override bool HandleBackButton()
        {
            bool handled = false;

            foreach (LazyListBoxItem item in albumList.GetVisibleItems())
            {
                AlbumListViewItem albumItem = item.FindVisualChild<AlbumListViewItem>();
                if (albumItem != null)
                {
                    ContextMenu menu = ContextMenu.GetContextMenu(albumItem);
                    if (menu != null)
                    {
                        handled |= menu.HandleBackButton();
                    }
                }
            }

            return handled;
        }

        private object _selectedItem;

        public override void Load()
        {
            if (DataContext == null)
            {
                DataContext = new AlbumsViewModel();
                ((AlbumsViewModel)DataContext).LoadAlbums();
            }

        }

        private void ListBoxOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Capture selected item data
            _selectedItem = (sender as ListBox).SelectedItem;

            if (_selectedItem != null)
            {
                string nextPage = string.Format("/Album/AlbumPage.xaml?{0}", Serializer.SerializeToQueryString<MediaLibrary.Album>(_selectedItem));
                FireNavigate(nextPage);
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

        private RoutedEventHandler _loadedHandler = null;
        public override void Activated(object sender, ActivatedDataEventArgs e)
        {
            base.Activated(sender, e);

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
                        Debug.WriteLine("VOffset: " + vscroll);
                        albumList.SetVerticalScrollOffset(vscroll);
                        Loaded -= _loadedHandler;
                    }
                });

            Loaded += _loadedHandler;
        }

        public override void Deactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            base.Deactivated(sender, e);

            double vscroll = albumList.GetVerticalScrollOffset();
            Configuration.SaveStateSetting<object>("DataContext", DataContext);
            Configuration.SaveStateSetting<double>("VScrollPosition", vscroll);
        }
    }
}

