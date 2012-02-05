using System;
using System.Windows.Data;

namespace MediaControl.Client.WindowsPhone
{
    public class TimeSpanConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TimeSpan? timeSpan = value as TimeSpan?;
            if (timeSpan != null && timeSpan.HasValue)
            {
                string negative = string.Empty;
                if (timeSpan.Value.TotalSeconds < 0)
                {
                    negative = "-";
                }

                return string.Format("{0}{1}:{2}", 
                    negative,
                    Math.Abs(timeSpan.Value.Minutes).ToString("0"), 
                    Math.Abs(timeSpan.Value.Seconds).ToString("00"));
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
