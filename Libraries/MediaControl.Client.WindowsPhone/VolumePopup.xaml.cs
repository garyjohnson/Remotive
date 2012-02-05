using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Sharkfist.Phone.SilverlightCore;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using System.Threading;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone
{
    public partial class VolumePopup : PopupControl
    {
        public VolumePopup()
        {
            InitializeComponent();

            if (!Designer.IsInDesignMode)
            {
                IMediaStateService mediaStateService = App.GetService<IMediaStateService>();
                mediaStateService.MediaStateChanged += new EventHandler<EventArgs>(VolumePopup_MediaStateChanged);

                if (mediaStateService.CurrentMediaState != null)
                {
                    SetSliderValue(mediaStateService.CurrentMediaState.Volume);
                }
            }
        }

        private const int STATUSBARHEIGHT = 30;
        private const int APPBARHEIGHT = 72;
        private const int MARGIN = 20;

        private Action<object> _completedCallback = null;
        private object _completedState = null;

        private Storyboard _slideInStoryboard = null;
        private Storyboard _slideOutStoryboard = null;

        protected override bool HideApplicationBar
        {
            get { return false; }
        }

        private void InitializeAnimations()
        {
            _slideInStoryboard = new Storyboard(); 
            DoubleAnimation slideInAnimation = new DoubleAnimation();
            slideInAnimation.To = 400 - MARGIN;
            slideInAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(600));
            slideInAnimation.EasingFunction = new QuadraticEase()
            {
                 EasingMode = EasingMode.EaseOut
            };
            slideInAnimation.Completed += new EventHandler(_slideInAnimation_Completed);
            Storyboard.SetTarget(slideInAnimation, _popup);
            Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath("HorizontalOffset"));
            _slideInStoryboard.Children.Add(slideInAnimation);

            _slideOutStoryboard = new Storyboard();
            DoubleAnimation slideOutAnimation = new DoubleAnimation();
            slideOutAnimation.To = 480;
            slideOutAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(600));
            slideOutAnimation.EasingFunction = new QuadraticEase()
            {
                EasingMode = EasingMode.EaseOut
            };
            slideOutAnimation.Completed += new EventHandler(_slideOutAnimation_Completed);
            Storyboard.SetTarget(slideOutAnimation, _popup);
            Storyboard.SetTargetProperty(slideOutAnimation, new PropertyPath("HorizontalOffset"));
            _slideOutStoryboard.Children.Add(slideOutAnimation);
        }

        public override void ShowPopupAsync(Action<object> completed, object state)
        {
            _completedCallback = completed;
            _completedState = state;

            if (_popup == null)
            {
                _popup = new Popup();
                InitializeAnimations();
            }

            _popup.Child = this;
            Width = 80;
            Height = 500;
            _popup.Width = Width;
            _popup.Height = Height;
            _popup.HorizontalOffset = 480;
            _popup.VerticalOffset = 800 - (APPBARHEIGHT + MARGIN + Height);
            _popup.IsOpen = true;

            _slideInStoryboard.Begin();
        }

        private void _slideOutAnimation_Completed(object sender, EventArgs e)
        {
            if (_popup != null)
            {
                _popup.IsOpen = false;
                _popup.Child = null;
            }

            if (_completedCallback != null)
            {
                _completedCallback.Invoke(_completedState);
            }

            ClosePopupCompleted();
        }

        private void _slideInAnimation_Completed(object sender, EventArgs e)
        {
            ShowPopupCompleted();
        }

        protected override void OnClosePopup()
        {
            _slideOutStoryboard.Begin();
        }

        // The timer is to reduce the amount of requests we send
        private Timer _sliderTimer;
        private bool _timerSet = false;
        private readonly object _syncLock = new object();

        private void SetSliderValue(double value)
        {
            if (value != slider.Value)
            {
                slider.Value = value;
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_sliderTimer == null)
            {
                _sliderTimer = new Timer(new TimerCallback((innerSender)=>
                    {
                        lock (_syncLock)
                        {
                            _sliderTimer.Change(Timeout.Infinite, Timeout.Infinite);
                            _timerSet = false;
                            Dispatcher.BeginInvoke(() =>
                                {
                                    // Go send the volume request
                                    IMediaStateService mediaStateService = App.GetService<IMediaStateService>();
                                    if (slider.Value != mediaStateService.CurrentMediaState.Volume)
                                    {
                                        Debug.WriteLine("Slider value request: from {0} to {1}", mediaStateService.CurrentMediaState.Volume, slider.Value);
                                        IRequestService requestService = App.GetService<IRequestService>();
                                        requestService.SetVolumeAsync(slider.Value,
                                            (mediaState, state, error) =>
                                            {
                                                IMediaStateService innerStateService = App.GetService<IMediaStateService>();
                                                innerStateService.CurrentMediaState = mediaState;
                                            }, null);
                                    }
                                });
                        }
                    }));
            }

            if (!_timerSet)
            {
                lock (_syncLock)
                {
                    if (!_timerSet)
                    {
                        _timerSet = true;
                        _sliderTimer.Change(300, Timeout.Infinite);
                    }
                }
            }
        }

        public void VolumePopup_MediaStateChanged(object sender, EventArgs e)
        {
            if (!slider.IsManipulating)
            {
                IMediaStateService mediaStateService = App.GetService<IMediaStateService>();
                double volume = mediaStateService.CurrentMediaState.Volume;
                Debug.WriteLine("Got volume from server: {0}", volume);
                SetSliderValue(volume);
            }
        }
    }
}
