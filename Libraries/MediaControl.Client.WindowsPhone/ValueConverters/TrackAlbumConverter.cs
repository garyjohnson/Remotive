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

namespace MediaControl.Client.WindowsPhone.ValueConverters
{
    public class TrackAlbumConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            MediaLibrary.Track track = null;
            if (value is MediaPlayback.Track)
            {
                track = ((MediaPlayback.Track)value).ConvertToTrack();
            }
            else if (value is MediaLibrary.Track)
            {
                track = (MediaLibrary.Track)value;
            }

            if (track != null)
            {
                return string.Format("{0} - {1}", track.ArtistName.ToUpper(), track.AlbumName.ToUpper());
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
