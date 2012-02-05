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

namespace MediaControl.Client.WindowsPhone.Music
{
    // We need the item template to have a fixed height for ui virtualization (and in turn, data virtualization) to work
    // properly. We set the height of the parent grid based on how many albums there are.
    public class AlbumArtistHeightConverter : IValueConverter
    {
        public const int ALBUM_HEIGHT = 99;
        public const int TEXT_HEIGHT = 34;
        public const double ALBUMS_IN_ROW = 4.0d;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null & value is int)
            {
                int albumCount = (int)value;
                double rows = Math.Ceiling(((double)albumCount) / ALBUMS_IN_ROW);
                return ((rows * ALBUM_HEIGHT) + TEXT_HEIGHT);
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
