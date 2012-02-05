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
    public class PopupControl : UserControl
    {
        private static readonly object _syncLock = new object();
        protected Popup _popup;
        private bool? _isAppBarVisible = null;

        protected Action<object> _showCallback;
        protected object _showState;
        protected Action<object> _closeCallback;
        protected object _closeState;

        public virtual void ShowPopupAsync(Action<object> showCallback, object showState)
        {
            _showCallback = showCallback;
            _showState = showState;

            if (_popup == null)
            {
                _popup = new Popup();

                _popup.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                _popup.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                _popup.HorizontalOffset = 0;
                _popup.VerticalOffset = 0;
                _popup.Child = this;

                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            }

            FrameworkElement rootVisual = (FrameworkElement)Application.Current.RootVisual;
            _popup.Height = rootVisual.ActualHeight;
            _popup.Width = rootVisual.ActualWidth;
            Height = rootVisual.ActualHeight;
            Width = rootVisual.ActualWidth;

            _popup.IsOpen = true;

            OnShowPopup();
        }

        protected virtual bool HideApplicationBar
        {
            get { return true; }
        }

        protected virtual void OnShowPopup()
        {
            ShowPopupCompleted();
        }

        protected void ShowPopupCompleted()
        {
            if (HideApplicationBar)
            {
                PhoneApplicationFrame rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                if (rootFrame != null)
                {
                    PhoneApplicationPage currentPage = rootFrame.Content as PhoneApplicationPage;
                    lock (_syncLock)
                    {
                        if (currentPage != null && !_isAppBarVisible.HasValue && currentPage.ApplicationBar != null)
                        {
                            _isAppBarVisible = currentPage.ApplicationBar.IsVisible;
                            currentPage.ApplicationBar.IsVisible = false;
                        }
                    }
                }
            }

            InvokeShowCallback();
        }

        protected virtual void InvokeShowCallback()
        {
            if (_showCallback != null)
            {
                _showCallback.Invoke(_showState);
            }
        }

        protected virtual void OnClosePopup()
        {
            //Debug.WriteLine("Source: {0}, OnClosePopup base.", this.GetType().Name);
            ClosePopupCompleted();
        }

        protected void ClosePopupCompleted()
        {
            if (_popup != null)
            {
                _popup.IsOpen = false;
                _popup.Child = null;
                _popup = null;
            }

            if (HideApplicationBar)
            {
                lock (_syncLock)
                {
                    if (_isAppBarVisible.HasValue)
                    {

                        PhoneApplicationFrame rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                        if (rootFrame != null)
                        {
                            PhoneApplicationPage currentPage = rootFrame.Content as PhoneApplicationPage;
                            if (currentPage != null && currentPage.ApplicationBar != null)
                            {
                                currentPage.ApplicationBar.IsVisible = _isAppBarVisible.Value;
                            }
                        }

                        _isAppBarVisible = null;
                    }
                }
            }

            InvokeCloseCallback();
        }

        protected virtual void InvokeCloseCallback()
        {
            if (_closeCallback != null)
            {
                _closeCallback.Invoke(_closeState);
            }
        }

        public void ClosePopupAsync(Action<object> closeCallback, object closeState)
        {
            _closeCallback = closeCallback;
            _closeState = closeState;

            OnClosePopup();
        }

        public void ForceClosePopup()
        {
            ClosePopupCompleted();
        }
    }
}
