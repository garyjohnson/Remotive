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
using System.Windows.Threading;
using Microsoft.Xna.Framework;

namespace Sharkfist.Phone.SilverlightCore
{
    public class XNAAsyncDispatcher : IApplicationService
    {
        private DispatcherTimer frameworkDispatcherTimer;

        public XNAAsyncDispatcher(TimeSpan dispatchInterval)
        {
            frameworkDispatcherTimer = new DispatcherTimer();
            frameworkDispatcherTimer.Tick += new EventHandler(frameworkDispatcherTimer_Tick);
            frameworkDispatcherTimer.Interval = dispatchInterval;
        }

        void frameworkDispatcherTimer_Tick(object sender, EventArgs e)
        {
            FrameworkDispatcher.Update();
        }

        public void StartService(ApplicationServiceContext context)
        {
            frameworkDispatcherTimer.Start();
        }

        public void StopService()
        {
            frameworkDispatcherTimer.Stop();
        }
    }
}
