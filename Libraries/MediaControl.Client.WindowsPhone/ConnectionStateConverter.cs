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

namespace MediaControl.Client.WindowsPhone
{
    public class ConnectionStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is ConnectionState)
            {
                switch ((ConnectionState)value)
                {
                    case ConnectionState.None:
                        {
                            return string.Empty;
                        }
                    case ConnectionState.Busy:
                        {
                            return "loading...";
                        }
                    case ConnectionState.Connecting:
                        {
                            return "attempting to reconnect...";
                        }
                    default:
                        {
                            throw new InvalidOperationException("Unknown connection state.");
                        }
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
