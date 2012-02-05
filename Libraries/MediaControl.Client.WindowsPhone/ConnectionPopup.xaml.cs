using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using Sharkfist.Phone.SilverlightCore;
using System.Diagnostics;

namespace MediaControl.Client.WindowsPhone
{
	public partial class ConnectingPopup : PopupControl
	{
        public ConnectingPopup()
		{
			InitializeComponent();

            if (!Designer.IsInDesignMode)
            {
                this.FadeInStoryboard.Completed += new EventHandler(FadeInStoryboard_Completed);
                this.FadeOutStoryboard.Completed += new EventHandler(FadeOutStoryboard_Completed);
            }
		}

        public static readonly DependencyProperty StateProperty =
        DependencyProperty.Register("State", typeof(ConnectionState), typeof(ConnectingPopup), new PropertyMetadata(ConnectionState.None));

        public ConnectionState State
        {
            get { return (ConnectionState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        protected override void OnShowPopup()
        {
            FadeOutStoryboard.Stop();
            FadeInStoryboard.Begin();
        }

        protected override void OnClosePopup()
        {
            //Debug.WriteLine("Source: {0}, State: {1}, OnClosePopup override.", this.GetType().Name, State);
            FadeInStoryboard.Stop();
            FadeOutStoryboard.Begin();
        }

        private void FadeOutStoryboard_Completed(object sender, EventArgs e)
        {
            //Debug.WriteLine("Source: {0}, State: {1}, FadeOutStoryboard Completed.", this.GetType().Name, State);
            //if (State == ConnectionState.None)
            //{
                ClosePopupCompleted();
            //}
        }

        private void FadeInStoryboard_Completed(object sender, EventArgs e)
        {
            //Debug.WriteLine("Source: {0}, State: {1}, FadeInStoryboard Completed.", this.GetType().Name, State);
            //if (State != ConnectionState.None)
            //{
                ShowPopupCompleted();
            //}
        }
	}
}