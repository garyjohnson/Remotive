/* This code was pulled from
 * http://diditwith.net/2007/03/23/SolvingTheProblemWithEventsWeakEventHandlers.aspx
 * and was written by Dustin Campbell.
 * No explicit license or permission was given to use this code,
 * so we should avoid using it. 
 * I have set it to not compile so it is not part of the binaries unless we need it.
 * Knowing how silverlight and wpf are with memory leaks, we may need it.
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
using System.ComponentModel;

namespace Sharkfist.Phone.SilverlightCore
{
    public delegate void UnregisterCallback<E>(EventHandler<E> eventHandler)
  where E : EventArgs;

    public interface IWeakEventHandler<E>
      where E : EventArgs
    {
        EventHandler<E> Handler { get; }
    }

    public class WeakEventHandler<T, E> : IWeakEventHandler<E>
        where T : class
        where E : EventArgs
    {
        private delegate void OpenEventHandler(T @this, object sender, E e);

        private WeakReference m_TargetRef;
        private OpenEventHandler m_OpenHandler;
        private EventHandler<E> m_Handler;
        private UnregisterCallback<E> m_Unregister;

        public WeakEventHandler(EventHandler<E> eventHandler, UnregisterCallback<E> unregister)
        {
            m_TargetRef = new WeakReference(eventHandler.Target);
            m_OpenHandler = (OpenEventHandler)Delegate.CreateDelegate(typeof(OpenEventHandler),
              null, eventHandler.Method);
            m_Handler = Invoke;
            m_Unregister = unregister;
        }

        public void Invoke(object sender, E e)
        {
            T target = (T)m_TargetRef.Target;

            if (target != null)
                m_OpenHandler.Invoke(target, sender, e);
            else if (m_Unregister != null)
            {
                m_Unregister(m_Handler);
                m_Unregister = null;
            }
        }

        public EventHandler<E> Handler
        {
            get { return m_Handler; }
        }

        public static implicit operator EventHandler<E>(WeakEventHandler<T, E> weh)
        {
            return weh.m_Handler;
        }
    }

    public static partial class ExtensionMethods
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
