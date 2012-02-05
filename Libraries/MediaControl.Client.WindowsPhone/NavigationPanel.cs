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
using Sharkfist.Phone.SilverlightCore;

namespace MediaControl.Client.WindowsPhone
{
    public class NavigationPanel : UserControl, IStateHandler
    {
        public virtual void Load()
        {
        }

        public virtual void Unload()
        {
        }

        public virtual bool HandleBackButton()
        {
            return false;
        }

        public virtual void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }

        public virtual void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
        }

        protected void FireNavigate(string path)
        {
            if (Navigate != null)
            {
                Navigate(this, new EventArgs<string>(path));
            }
        }

        public event EventHandler<EventArgs<string>> Navigate;

        public virtual void Launching(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
        }

        public virtual void Activated(object sender, ActivatedDataEventArgs e)
        {
        }

        public virtual void Deactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
        }

        public virtual void Closing(object sender, Microsoft.Phone.Shell.ClosingEventArgs e)
        {
        }
    }
}
