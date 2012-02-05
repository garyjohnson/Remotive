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

namespace Sharkfist.Phone.SilverlightCore
{
    public class Request
    {
        public Request(Delegate action, params object[] parameters)
        {
            _action = action;
            _parameters = parameters;
        }

        private Delegate _action;
        private object[] _parameters;

        public void Execute()
        {
            _action.DynamicInvoke(_parameters);
        }
    }
}
