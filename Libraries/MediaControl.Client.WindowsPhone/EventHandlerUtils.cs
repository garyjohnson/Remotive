/* This code was pulled from
 * http://diditwith.net/2007/03/23/SolvingTheProblemWithEventsWeakEventHandlers.aspx
 * and was written by Dustin Campbell.
 * No explicit license or permission was given to use this code.
 */

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
using System.Reflection;

namespace MediaControl.Client.WindowsPhone
{
    public static class EventHandlerUtils
    {
        public static EventHandler<E> MakeWeak<E>(this EventHandler<E> eventHandler, UnregisterCallback<E> unregister)
          where E : EventArgs
        {
            if (eventHandler == null)
            {
                throw new ArgumentNullException("eventHandler");
            }

            if (eventHandler.Method.IsStatic || eventHandler.Target == null)
            {
                throw new ArgumentException("Only instance methods are supported.", "eventHandler");
            }

            Type wehType = typeof(WeakEventHandler<,>).MakeGenericType(eventHandler.Method.DeclaringType, typeof(E));
            ConstructorInfo wehConstructor = wehType.GetConstructor(new Type[] { typeof(EventHandler<E>), typeof(UnregisterCallback<E>) });

            IWeakEventHandler<E> weh = (IWeakEventHandler<E>)wehConstructor.Invoke(new object[] { eventHandler, unregister });

            return weh.Handler;
        }
    }
}
