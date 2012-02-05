using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Sharkfist.Phone.SilverlightCore;
using System.ComponentModel;

namespace MediaControl.Client.WindowsPhone
{
	public partial class SplashScreen : ModalPopupControl
	{
		public SplashScreen()
		{
			// Required to initialize variables
			InitializeComponent();

            if (!DesignerProperties.IsInDesignTool)
            {
                Loaded += new RoutedEventHandler(SplashScreen_Loaded);
            }
		}

        private Storyboard _slideInStoryboard = null;
        private Storyboard _slideOutStoryboard = null;

        private void InitializeAnimations()
        {
            _slideInStoryboard = new Storyboard();
            DoubleAnimation slideInAnimation = new DoubleAnimation();
            slideInAnimation.To = 0;
            slideInAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(600));
            slideInAnimation.EasingFunction = new QuadraticEase()
            {
                EasingMode = EasingMode.EaseOut
            };
            slideInAnimation.Completed += new EventHandler(slideInAnimation_Completed);
            Storyboard.SetTarget(slideInAnimation, _popup);
            Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath("VerticalOffset"));
            _slideInStoryboard.Children.Add(slideInAnimation);

            _slideOutStoryboard = new Storyboard();
            DoubleAnimation slideOutAnimation = new DoubleAnimation();
            slideOutAnimation.To = -800;
            slideOutAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(600));
            slideOutAnimation.EasingFunction = new QuadraticEase()
            {
                EasingMode = EasingMode.EaseOut
            };
            slideOutAnimation.Completed += new EventHandler(slideOutAnimation_Completed);
            Storyboard.SetTarget(slideOutAnimation, _popup);
            Storyboard.SetTargetProperty(slideOutAnimation, new PropertyPath("VerticalOffset"));
            _slideOutStoryboard.Children.Add(slideOutAnimation);
        }

        protected override void OnShowPopup()
        {
            InitializeAnimations();
            _slideInStoryboard.Begin();
            //ShowPopupCompleted();
        }

        protected override void OnClosePopup()
        {
            _slideOutStoryboard.Begin();
        }

        private void slideOutAnimation_Completed(object sender, EventArgs e)
        {
            ClosePopupCompleted();
        }

        private void slideInAnimation_Completed(object sender, EventArgs e)
        {
            ShowPopupCompleted();
        }

        private void SplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            RadioAnimation.Begin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            base.ClosePopupAsync(null, null);
        }
	}
}