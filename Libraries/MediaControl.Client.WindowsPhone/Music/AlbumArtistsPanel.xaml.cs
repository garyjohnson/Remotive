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
    public partial class AlbumArtistsPanel : NavigationPanel
    {
        public AlbumArtistsPanel()
        {
            InitializeComponent();
        }

        public override void Load()
        {
            if (DataContext == null)
            {
                DataContext = new AlbumArtistsViewModel();
                ((AlbumArtistsViewModel)DataContext).LoadArtists();
            }
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
                        //System.Diagnostics.Debug.WriteLine("VOffset: " + vscroll);
                        listBox.SetVerticalScrollOffset(vscroll);
                        Loaded -= _loadedHandler;
                    }
                });

            Loaded += _loadedHandler;
        }

        public override void Deactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            base.Deactivated(sender, e);

            double vscroll = listBox.GetVerticalScrollOffset();
            Configuration.SaveStateSetting<object>("DataContext", DataContext);
            Configuration.SaveStateSetting<double>("VScrollPosition", vscroll);
        }

        private void AlbumArtistControl_AlbumClicked(object sender, AlbumClickedArgs e)
        {
            string nextPage = string.Format("/Album/AlbumPage.xaml?{0}", Serializer.SerializeToQueryString<MediaLibrary.Album>(e.Album));
            FireNavigate(nextPage);
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            App.GetService<IRequestService>().PlayAlbumArtist((AlbumArtist)sender,
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
            App.GetService<IRequestService>().PlayAlbumArtist((AlbumArtist)sender,
                true,
                (media, state, error) =>
                {
                    if (error == null)
                    {
                        App.GetService<IMediaStateService>().CurrentMediaState = media;
                    }
                }, null);
        }
    }
}