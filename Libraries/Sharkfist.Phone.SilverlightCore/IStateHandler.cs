using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Shell;
using Sharkfist.Phone.SilverlightCore;

namespace Sharkfist.Phone.SilverlightCore
{
    public interface IStateHandler
    {
        void Launching(object sender, LaunchingEventArgs e);
        void Activated(object sender, ActivatedDataEventArgs e);
        void Deactivated(object sender, DeactivatedEventArgs e);
        void Closing(object sender, ClosingEventArgs e);
    }
}
