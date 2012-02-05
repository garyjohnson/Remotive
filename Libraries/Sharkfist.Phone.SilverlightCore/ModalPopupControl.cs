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
using System.Windows.Controls.Primitives;
using System.Diagnostics;

namespace Sharkfist.Phone.SilverlightCore
{
    // Modal popup control handles its own closing.
    public class ModalPopupControl : PopupControl
    {
        protected override void InvokeShowCallback()
        {
        }

        protected override void InvokeCloseCallback()
        {
            base.InvokeShowCallback();
            base.InvokeCloseCallback();
        }
    }
}
