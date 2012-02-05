using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using MediaControl.Client.WindowsPhone.MediaLibrary;

namespace MediaControl.Client.WindowsPhone.ValueConverters
{
    public class AlbumTitleConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is MediaLibrary.Album)
            {
                MediaLibrary.Album album = (MediaLibrary.Album)value;
                string artistName = "Unknown Artist";
                if(!string.IsNullOrEmpty(album.ArtistName))
                {
                    artistName = album.ArtistName;
                }

                string albumTitle = "Unknown Album";
                if (!string.IsNullOrEmpty(album.Title))
                {
                    albumTitle = album.Title;
                }

                return string.Format("{0} - {1}", artistName.ToUpper(), albumTitle.ToUpper());
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
