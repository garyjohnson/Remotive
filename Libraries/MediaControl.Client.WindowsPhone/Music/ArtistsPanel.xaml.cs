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
using System.Windows.Data;
using Sharkfist.Phone.SilverlightCore;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone.Music
{
    public partial class ArtistsPanel : NavigationPanel
    {
        public ArtistsPanel()
        {
            InitializeComponent();
        }

        private object _selectedItem;

        public override void Load()
        {
            if (DataContext == null)
            {
                DataContext = new ArtistsViewModel();
                ((ArtistsViewModel)DataContext).LoadArtists();
            }
        }

        public override void Unload()
        {
            DataContext = null;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Capture selected item data
            _selectedItem = (sender as ListBox).SelectedItem;

            if (_selectedItem != null)
            {
                string nextPage = string.Format("/Albums/AlbumsPage.xaml?{0}", Serializer.SerializeToQueryString<Artist>(_selectedItem));
                // reset selected item so user can click on it again later
                (sender as ListBox).SelectedItem = null;
                FireNavigate(nextPage);
            }
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            App.GetService<IRequestService>().PlayArtist((Artist)sender,
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
            App.GetService<IRequestService>().PlayArtist((Artist)sender,
                true,
                (media, state, error) =>
                {
                    if (error == null)
                    {
                        App.GetService<IMediaStateService>().CurrentMediaState = media;
                    }
                }, null);
        }

        public override bool HandleBackButton()
        {
            bool handled = false;

            VirtualizingStackPanel panel = artistList.FindVisualChild<VirtualizingStackPanel>();
            foreach (UIElement child in panel.Children)
            {
                ListViewItem item = child.FindVisualChild<ListViewItem>();
                if (item != null)
                {
                    Sharkfist.Phone.SilverlightCore.ContextMenu menu = Sharkfist.Phone.SilverlightCore.ContextMenu.GetContextMenu(item);
                    if (menu != null)
                    {
                        handled |= menu.HandleBackButton();
                    }
                }
            }
            return handled;
        }

        public RoutedEventHandler _loadedHandler = null;
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
                    //System.Diagnostics.Debug.WriteLine("VOffset: " + vscroll);
                    artistList.SetVerticalScrollOffset(vscroll);
                    Loaded -= _loadedHandler;
                }
            });

            Loaded += _loadedHandler;
        }

        public override void Deactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            base.Deactivated(sender, e);

            double vscroll = artistList.GetVerticalScrollOffset();
            Configuration.SaveStateSetting<object>("DataContext", DataContext);
            Configuration.SaveStateSetting<double>("VScrollPosition", vscroll);
        }
    }
}