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
using Sharkfist.Phone.SilverlightCore;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Windows.Data;

namespace MediaControl.Client.WindowsPhone
{
    public partial class AlbumControl : UserControl
    {
        static AlbumControl()
        {
            TiltEffect.TiltableItems.Add(typeof(AlbumControl));
        }

        public AlbumControl()
        {
            InitializeComponent();

            Binding imageBinding = new Binding("AlbumArt");
            imageBinding.Source = this;
            image.SetBinding(Image.SourceProperty, imageBinding);
        }

        // Use this property to watch for album change
        public static readonly DependencyProperty AlbumIDProperty =
            DependencyProperty.Register("AlbumID", typeof(string), typeof(AlbumControl),
            new PropertyMetadata(null, new PropertyChangedCallback((sender, args) =>
            {
                AlbumControl item = (AlbumControl)sender;
                AlbumArtCache.Instance.AlbumArtDownloaded += new EventHandler<EventArgs<MediaLibrary.Album, BitmapImage>>(item.Instance_AlbumArtDownloaded);
                ((AlbumControl)sender).GetAlbumArt();
            })));

        public string AlbumID
        {
            get { return (string)GetValue(AlbumIDProperty); }
            set { SetValue(AlbumIDProperty, value); }
        }

        public static readonly DependencyProperty AlbumArtProperty =
            DependencyProperty.Register("AlbumArt", typeof(BitmapImage), typeof(AlbumControl),
            new PropertyMetadata(null, new PropertyChangedCallback((sender, args) =>
                {
                    AlbumControl item = (AlbumControl)sender;
                    if (args.NewValue != null)
                    {
                        ((Storyboard)item.Resources["ShowAlbumArt"]).Begin();
                        item.grid.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ((Storyboard)item.Resources["HideAlbumArt"]).Begin();
                        item.grid.Visibility = Visibility.Visible;
                    }
                })));

        public BitmapImage AlbumArt
        {
            get { return (BitmapImage)GetValue(AlbumArtProperty); }
            set { SetValue(AlbumArtProperty, value); }
        }

        public void Instance_AlbumArtDownloaded(object sender, EventArgs<MediaLibrary.Album, BitmapImage> e)
        {
            if (string.Equals(e.Value1.ID, AlbumID))
            {
                AlbumArt = e.Value2;
            }
        }

        private void GetAlbumArt()
        {
            if (AlbumArt != null)
            {
                AlbumArt = null;
            }

            if (!DesignerProperties.IsInDesignTool)
            {
                if (!string.IsNullOrEmpty(AlbumID))
                {
                    AlbumArtCache.Instance.GetAlbumArtAsync((MediaLibrary.Album)DataContext);
                }
                else
                {
                    grid.Visibility = System.Windows.Visibility.Visible;
                }
            }
            else
            {
                AlbumArt = new BitmapImage(new Uri("/Images/Album/Album1.png", UriKind.Relative));
            }
        }

        private bool _clickStarted = false;
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _clickStarted = false;
            ReleaseMouseCapture();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (CaptureMouse())
            {
                _clickStarted = true;
            }
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);
            _clickStarted = false;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (_clickStarted)
            {
                if (Click != null)
                {
                    Click(this, EventArgs.Empty);
                }

                _clickStarted = false;
                ReleaseMouseCapture();
            }
        }

        public event EventHandler Click;
    }
}