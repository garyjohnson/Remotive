using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.ComponentModel;
using Microsoft.Phone.Shell;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Sharkfist.Phone.SilverlightCore;
using System.Diagnostics;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone.Music
{
	public partial class MusicPage : MediaPhonePage, IStateHandler
	{
        public MusicPage()
        {
            InitializeComponent();
            this.ApplicationBar.IsVisible = false;

            if (!DesignerProperties.IsInDesignTool)
            {
                Loaded += new RoutedEventHandler(MusicPage_Loaded);
            }
        }

        private void MusicPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeMusicPage();
        }

        private void InitializeMusicPage()
        {
            this.ApplicationBar.IsVisible = true;
            _isReady = true;
            InitializePanel(((PivotItem)pivot.SelectedItem).Content as NavigationPanel);
        }

        bool _isReady = false;

        private void panel_Navigate(object sender, EventArgs<string> e)
        {
            string navigatedUrl = e.Value;
            NavigationService.Navigate(new Uri(navigatedUrl, UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            string navigatedUrl = "/NowPlaying/NowPlayingPage.xaml";
            NavigationService.Navigate(new Uri(navigatedUrl, UriKind.Relative));
        }

        private void _nowPlayingMenuItem_Click(object sender, EventArgs e)
        {
            string navigatedUrl = "/NowPlaying/NowPlayingPage.xaml";
            NavigationService.Navigate(new Uri(navigatedUrl, UriKind.Relative));
        }

        private ApplicationBarMenuItem _nowPlayingMenuItem;
        protected override void BuildApplicationBar(Microsoft.Phone.Shell.IApplicationBar appBar)
        {
            _nowPlayingMenuItem = new ApplicationBarMenuItem("now playing");
            _nowPlayingMenuItem.Click += new EventHandler(_nowPlayingMenuItem_Click);
            appBar.MenuItems.Add(_nowPlayingMenuItem);
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (pivot.SelectedItem != null && pivot.SelectedItem is PivotItem)
            {
                PivotItem selectedItem = (PivotItem)pivot.SelectedItem;
                if (selectedItem.Content != null && selectedItem.Content is NavigationPanel)
                {
                    e.Cancel = ((NavigationPanel)selectedItem.Content).HandleBackButton();
                }
            }

            base.OnBackKeyPress(e);
        }

        private void InitializePanel(NavigationPanel panel)
        {
            if (panel != null)
            {
                Binding busyBinding = new Binding("DataContext.IsBusy");
                busyBinding.Source = panel;
                SetBinding(IsBusyProperty, busyBinding);

                panel.Load();
            }
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                PivotItem pivotItem = e.RemovedItems[0] as PivotItem;
                if (pivotItem != null)
                {
                    NavigationPanel panel = pivotItem.Content as NavigationPanel;
                    if (panel != null)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            panel.Unload();
                        });
                    }
                }

                ClearValue(IsConnectedProperty);
                ClearValue(IsBusyProperty);

            }

            if (e.AddedItems.Count > 0 && _isReady)
            {
                NavigationPanel panel = ((PivotItem)e.AddedItems[0]).Content as NavigationPanel;
                if (panel != null)
                {
                    Dispatcher.BeginInvoke(() =>
                        {
                            InitializePanel(panel);
                        });
                }
            }
        }



        public void Launching(object sender, LaunchingEventArgs e)
        {
            if (pivot.SelectedItem != null && pivot.SelectedItem is PivotItem)
            {
                PivotItem selectedItem = (PivotItem)pivot.SelectedItem;
                if (selectedItem.Content != null && selectedItem.Content is NavigationPanel)
                {
                    ((NavigationPanel)selectedItem.Content).Launching(sender, e);
                }
            }
        }

        public void Activated(object sender, ActivatedDataEventArgs e)
        {
            if (!e.ClearData)
            {
                int pivotIndex = 0;
                if (Configuration.TryLoadStateSetting<int>("SelectedPivot", out pivotIndex))
                {
                    pivot.SelectedItem = pivot.Items[pivotIndex];
                }

                if (pivot.SelectedItem != null && pivot.SelectedItem is PivotItem)
                {
                    PivotItem selectedItem = (PivotItem)pivot.SelectedItem;
                    if (selectedItem.Content != null && selectedItem.Content is NavigationPanel)
                    {
                        ((NavigationPanel)selectedItem.Content).Activated(sender, e);
                    }
                }
            }
        }

        public void Deactivated(object sender, DeactivatedEventArgs e)
        {
             Configuration.SaveStateSetting<int>("SelectedPivot", pivot.SelectedIndex);

            if (pivot.SelectedItem != null && pivot.SelectedItem is PivotItem)
            {
                PivotItem selectedItem = (PivotItem)pivot.SelectedItem;
                if (selectedItem.Content != null && selectedItem.Content is NavigationPanel)
                {
                    ((NavigationPanel)selectedItem.Content).Deactivated(sender, e);
                }
            }
        }

        public void Closing(object sender, ClosingEventArgs e)
        {
            if (pivot.SelectedItem != null && pivot.SelectedItem is PivotItem)
            {
                PivotItem selectedItem = (PivotItem)pivot.SelectedItem;
                if (selectedItem.Content != null && selectedItem.Content is NavigationPanel)
                {
                    ((NavigationPanel)selectedItem.Content).Closing(sender, e);
                }
            }
        }
	}
}