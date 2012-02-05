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

namespace MediaControl.Client.WindowsPhone.Music
{
    public class Hack
    {
        public static readonly DependencyProperty HeightUpdateProperty = DependencyProperty.RegisterAttached("HeightUpdate", typeof(double), typeof(Hack),
            new PropertyMetadata(new PropertyChangedCallback((sender, args) =>
            {
                FrameworkElement senderElement = sender as FrameworkElement;
                if (senderElement != null)
                {
                    senderElement.SetValue(FrameworkElement.HeightProperty, args.NewValue);
                }
            })));

        public static int GetHeightUpdate(DependencyObject target)
        {
            return (int)target.GetValue(Hack.HeightUpdateProperty);
        }

        public static void SetHeightUpdate(DependencyObject target, int value)
        {
            target.SetValue(Hack.HeightUpdateProperty, value);
        }
    }
}
