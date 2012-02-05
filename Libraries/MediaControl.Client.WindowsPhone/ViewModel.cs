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
using System.Runtime.Serialization;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone
{
    [DataContract()]
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                App.GetService<IRequestService>().Refresh += new EventHandler<EventArgs>(ViewModel_Refresh);
            }
        }

        public void ViewModel_Refresh(object sender, EventArgs e)
        {
            Refresh();
        }

        private bool _isBusy = false;
        [IgnoreDataMember()]
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }

        public virtual void Refresh() { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
