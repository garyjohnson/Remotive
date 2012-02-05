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
using Microsoft.Phone.Shell;

namespace Sharkfist.Phone.SilverlightCore
{
    public class ActivatedDataEventArgs : EventArgs
    {
        public ActivatedDataEventArgs(bool clearData)
        {
            ClearData = clearData;
        }

        private bool _clearData = false;
        public bool ClearData
        {
            get { return _clearData; }
            private set { _clearData = value; }
        }
    }
}
