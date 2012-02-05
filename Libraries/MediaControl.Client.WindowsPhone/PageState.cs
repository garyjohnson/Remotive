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

namespace MediaControl.Client.WindowsPhone
{
    public class PageState : DependencyObject
    {
        private static readonly PageState _pageState = new PageState();
        public static PageState Current
        {
            get { return _pageState; }
        }

        public static readonly DependencyProperty CurrentDataContextProperty =
            DependencyProperty.Register("CurrentDataContext", typeof(object), typeof(PageState),
            new PropertyMetadata(null));

        public object CurrentDataContext
        {
            get { return GetValue(CurrentDataContextProperty); }
            set { SetValue(CurrentDataContextProperty, value); }
        }

        public static readonly DependencyProperty CurrentPageUriProperty =
            DependencyProperty.Register("CurrentPageUri", typeof(Uri), typeof(PageState),
            new PropertyMetadata(null));

        public Uri CurrentPageUri
        {
            get { return (Uri)GetValue(CurrentPageUriProperty); }
            set { SetValue(CurrentPageUriProperty, value); }
        }
    }
}
