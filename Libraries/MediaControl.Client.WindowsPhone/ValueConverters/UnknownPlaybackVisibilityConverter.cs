using System;
using System.Windows;
using System.Windows.Data;
using MediaControl.Client.WindowsPhone.MediaPlayback;

namespace MediaControl.Client.WindowsPhone.ValueConverters
{
    public class UnknownPlaybackVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is MediaState)
            {
                PlaybackState state = ((MediaState)value).PlaybackState;
                if (state == PlaybackState.Unknown)
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
