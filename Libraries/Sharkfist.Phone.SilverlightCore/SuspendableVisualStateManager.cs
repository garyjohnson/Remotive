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
using System.Collections.Generic;

namespace Sharkfist.Phone.SilverlightCore
{
    public class SuspendableVisualStateManager : VisualStateManager
    {
        //private readonly Queue<string> _states = new Queue<string>();
        private bool _isSuspended = false;

        public void Suspend()
        {
            _isSuspended = true;
        }

        public void Resume(Control target)
        {
            _isSuspended = false;

            //while (_states.Count > 0)
            //{
            //    string state = _states.Dequeue();
            //    GoToState(target, state, false);
            //}
        }

        protected override bool GoToStateCore(Control control, FrameworkElement templateRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
        {
            if (_isSuspended)
            {
                //_states.Enqueue(stateName);
               // return true;
                return false;
            }
            else
            {
                return base.GoToStateCore(control, templateRoot, stateName, group, state, useTransitions);
            }
        }
    }
}
