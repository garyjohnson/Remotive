using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Sharkfist.Phone.SilverlightCore;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone
{
	public partial class AlbumArtistControl : UserControl
	{
        static AlbumArtistControl()
        {
            TiltEffect.TiltableItems.Add(typeof(AlbumArtistControl));
        }

		public AlbumArtistControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void AlbumControl_Click(object sender, EventArgs e)
        {
            FrameworkElement depSender = sender as FrameworkElement;
            if(depSender != null)
            {
                MediaLibrary.Album album = depSender.DataContext as MediaLibrary.Album;
                if (album != null)
                {
                    if (AlbumClicked != null)
                    {
                        AlbumClicked(this, new AlbumClickedArgs(album));
                    }
                }
            }            
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            App.GetService<IRequestService>().PlayArtist((MediaControl.Client.WindowsPhone.MediaLibrary.Artist)sender,
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
            App.GetService<IRequestService>().PlayArtist((MediaControl.Client.WindowsPhone.MediaLibrary.Artist)sender,
                true,
                (media, state, error) =>
                {
                    if (error == null)
                    {
                        App.GetService<IMediaStateService>().CurrentMediaState = media;
                    }
                }, null);
        }

        public event EventHandler<AlbumClickedArgs> AlbumClicked;
    }
}