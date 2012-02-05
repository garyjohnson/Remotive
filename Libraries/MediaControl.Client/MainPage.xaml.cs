using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MediaControl.Client.MediaLibrary;
using System.Collections.Generic;
namespace MediaControl.Client
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            MediaLibraryService.Current.BeginGetAlbums((albums, state) =>
                {
                    foreach (Album album in albums)
                    {
                        Albums.Add(album);
                    }
                }, null);

            //MediaControl.Client
            //foreach (IAlbum album in MediaLibraryService.Current.GetAlbums())
            //{
            //    Albums.Add(album);
            //}
        }

        private static readonly DependencyProperty AlbumsProperty =
            DependencyProperty.Register("Albums", typeof(ObservableCollection<Album>), typeof(MainPage),
            new PropertyMetadata(new ObservableCollection<Album>()));

        public ObservableCollection<Album> Albums
        {
            get { return (ObservableCollection<Album>)GetValue(AlbumsProperty); }
            set { SetValue(AlbumsProperty, value); }
        }
    }
}
