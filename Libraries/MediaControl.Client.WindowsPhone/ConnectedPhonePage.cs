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
using Microsoft.Phone.Controls;
using System.Windows.Data;
using Sharkfist.Phone.SilverlightCore;
using System.Diagnostics;

namespace MediaControl.Client.WindowsPhone
{
    public class ConnectedPhonePage : PhoneApplicationPage
    {
        public ConnectedPhonePage() { }

        public static readonly DependencyProperty IsConnectedProperty =
            DependencyProperty.Register("IsConnected", typeof(bool), typeof(ConnectedPhonePage),
            new PropertyMetadata(true, new PropertyChangedCallback((sender, args) =>
            {
            })));

        public bool IsConnected
        {
            get { return (bool)GetValue(IsConnectedProperty); }
            set { SetValue(IsConnectedProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(ConnectedPhonePage),
            new PropertyMetadata(false, new PropertyChangedCallback((sender, args) =>
            {
            })));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
    }
}
