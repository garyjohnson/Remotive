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

namespace Sharkfist.Phone.SilverlightCore
{
    public class RotationCenterConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FrameworkElement child = value as FrameworkElement;
            FrameworkElement rootVisual = Application.Current.RootVisual as FrameworkElement;

            if (child != null && rootVisual != null)
            {
                Point transformPoint = new Point();
                //try
                //{
                    GeneralTransform transform = child.TransformToVisual(rootVisual);
                    transformPoint = transform.Transform(new Point(0, 0));
                //}
                //catch (ArgumentException)
                //{

                //}

                double x = 1.0d / -child.ActualWidth;
                double y = 1.0d / -child.ActualHeight;

                return new Point(x * rootVisual.ActualWidth, y * rootVisual.ActualHeight);
            }

            return new Point(0.5, 0.5);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
