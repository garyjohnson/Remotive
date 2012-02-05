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
    public static class Designer
    {
        private static readonly UserControl _control = new UserControl();
        public static bool IsInDesignMode
        {
            get { return DesignerProperties.GetIsInDesignMode(_control); }
        }
    }
}
