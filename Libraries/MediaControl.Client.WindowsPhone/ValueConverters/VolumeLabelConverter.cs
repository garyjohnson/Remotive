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
    public class VolumeLabelConverter : IValueConverter
    {
        private const int MAX_VOLUME = 50;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double parsedValue = 0;
            if (value != null && double.TryParse(value.ToString(), out parsedValue))
            {
                int volume = (int)(parsedValue * MAX_VOLUME);
                if (volume == 0)
                {
                    return "Mute";
                }
                else
                {
                    return volume.ToString("00");
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
