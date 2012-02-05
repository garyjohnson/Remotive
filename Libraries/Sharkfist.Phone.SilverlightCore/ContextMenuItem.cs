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
using System.ComponentModel;

namespace Sharkfist.Phone.SilverlightCore
{
    public class ContextMenuItem : INotifyPropertyChanged
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                FirePropertyChanged("Text");
            }
        }

        public void FireClick(object sender)
        {
            if (null != Click)
            {
                Click(sender, new RoutedEventArgs());
            }

        }

        public event RoutedEventHandler Click;

        private void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
