using System;
using System.Windows;
using System.Windows.Data;
using MediaControl.Client.WindowsPhone.MediaPlayback;

namespace MediaControl.Client.WindowsPhone.ValueConverters
{
    public class PlaybackStateImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value is PlaybackState)
            {
                PlaybackState state = (PlaybackState)value;
                if (state != PlaybackState.Play)
                {
                    return new Uri("/Images/Play.png");
                }
                else if (state == PlaybackState.Play)
                {
                    return new Uri("/Images/Pause.png");
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
