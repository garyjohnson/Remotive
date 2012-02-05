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

namespace MediaControl.Client.WindowsPhone
{
    public class EasySlider : Slider
    {
        public EasySlider()
        { 
            SizeChanged += new SizeChangedEventHandler(PhoneSlider_SizeChanged);
            this.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(EasySlider_ManipulationStarted);
            this.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(EasySlider_ManipulationCompleted);
        }

        public bool IsManipulating = false;

        private void EasySlider_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            IsManipulating = false;
        }

        private void EasySlider_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            IsManipulating = true;
        } 

        private void PhoneSlider_SizeChanged(object sender, SizeChangedEventArgs e) 
        { 
            if (e.NewSize.Width > 0 && e.NewSize.Height > 0) 
            { 
                Rect clipRect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height); 
                if (Orientation == Orientation.Horizontal) 
                { 
                    clipRect.X -= 12; 
                    clipRect.Width += 24; 
                    object margin = Resources["PhoneHorizontalMargin"]; 
                    if (margin != null) 
                    { 
                        Margin = (Thickness)margin; 
                    } 
                } 
                else 
                { 
                    clipRect.Y -= 12; 
                    clipRect.Height += 24; 
                    object margin = Resources["PhoneVerticalMargin"]; 
                    if (margin != null) 
                    { 
                        Margin = (Thickness)margin; 
                    } 
                } 
  
                this.Clip = new RectangleGeometry() { Rect = clipRect }; 
            } 
        } 
    } 
} 
