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

namespace Sharkfist.Phone.SilverlightCore
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }

    public class EventArgs<T, U> : EventArgs
    {
        public EventArgs(T value1, U value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public T Value1 { get; set; }
        public U Value2 { get; set; }
    }
}
