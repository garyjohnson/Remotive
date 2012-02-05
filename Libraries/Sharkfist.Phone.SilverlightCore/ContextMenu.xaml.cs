using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using System.Windows.Media.Animation;

namespace Sharkfist.Phone.SilverlightCore
{
    [ContentProperty("Items")]
    public partial class ContextMenu : UserControl
    {
        public ContextMenu()
        {
            SetValue(ItemsProperty, new List<ContextMenuItem>());
            InitializeComponent();

            hold.Completed += new EventHandler(hold_Completed);
            openDownProgress.Completed += new EventHandler(openDownProgress_Completed);
            openUpProgress.Completed += new EventHandler(openUpProgress_Completed);
            openDown.Completed += new EventHandler(open_Completed);
            openUp.Completed += new EventHandler(open_Completed);
            reset.Completed += new EventHandler(reset_Completed);
        }

        public const double APPLICATION_BAR_HEIGHT = 72.0d;

        public static readonly DependencyProperty ContextMenuProperty =
            DependencyProperty.RegisterAttached("ContextMenu", typeof(ContextMenu), typeof(ContextMenu),
            new PropertyMetadata(null, (sender, args) =>
            {
                if (args.OldValue != null)
                {
                    ((ContextMenu)args.OldValue).DetachFromOwner(sender as FrameworkElement);
                }

                if (args.NewValue != null)
                {
                    ((ContextMenu)args.NewValue).AttachToOwner(sender as FrameworkElement);
                }
            }));

        public static ContextMenu GetContextMenu(DependencyObject owner)
        {
            return (ContextMenu)owner.GetValue(ContextMenuProperty);
        }

        public static void SetContextMenu(DependencyObject owner, ContextMenu value)
        {
            owner.SetValue(ContextMenuProperty, value);
        }

        public static readonly DependencyProperty AttachedItemProperty = 
            DependencyProperty.Register("AttachedItem", typeof(FrameworkElement), typeof(ContextMenu),
            new PropertyMetadata(null));

        public FrameworkElement AttachedItem
        {
            get { return (FrameworkElement)GetValue(AttachedItemProperty); }
            set{SetValue(AttachedItemProperty, value);}
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(List<ContextMenuItem>), typeof(ContextMenu),
            new PropertyMetadata(null));

        public List<ContextMenuItem> Items
        {
            get { return (List<ContextMenuItem>)GetValue(ItemsProperty); }
        }

        private const double MAX_DRAG_DISTANCE = 20.0d;
        private Point? _dragOrigin = null;
        private bool? _isAppBarVisible = null;
        private ScrollBarVisibility? _scrollBarVisibility = null;
        private Popup _contextMenuPopup = null;
        private AnimationState _contextMenuState = AnimationState.None;
        
        private MouseEventHandler _mouseMoveHandler = null;
        private void owner_MouseMove(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("Owner Mouse Move");
            
            if (_dragOrigin.HasValue &&
                (_contextMenuState == AnimationState.InitialDelay ||
                _contextMenuState == AnimationState.ShowingProgress))
            {
                Point currentPosition = e.GetPosition(Application.Current.RootVisual);
                if (_dragOrigin.Value.GetDistance(currentPosition) > MAX_DRAG_DISTANCE)
                {
                    ((UIElement)sender).ReleaseMouseCapture();

                    _dragOrigin = null;
                    ClosePopup();
                }
            }
        }

        private MouseButtonEventHandler _mouseLeftButtonUpHandler = null;
        private void owner_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("Owner Mouse Up");
            
            _dragOrigin = null;
            ((UIElement)sender).ReleaseMouseCapture();

            // Check this before ClosePopup, as ClosePopup
            // will change the state
            if (_contextMenuState != AnimationState.None &&
                _contextMenuState != AnimationState.InitialDelay)
            {
                e.Handled = true;
            }

            if (_contextMenuState == AnimationState.InitialDelay ||
                _contextMenuState == AnimationState.ShowingProgress)
            {
                ClosePopup();
            }
        }

        private MouseButtonEventHandler _mouseLeftButtonDownHandler = null;
        private void owner_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("Owner Mouse Down");
            _dragOrigin = e.GetPosition(Application.Current.RootVisual);
            _contextMenuState = AnimationState.InitialDelay;
            //Debug.WriteLine("Initial Delay");
            ((UIElement)sender).CaptureMouse();
            hold.Begin();
        }

        public bool HandleBackButton()
        {
            bool returnValue = (_contextMenuState != AnimationState.None);
            if (returnValue)
            {
                ClosePopup();
            }

            return returnValue;
        }

        private MouseEventHandler _mouseCaptureLostHandler = null;
        private void owner_MouseCaptureLost(object sender, MouseEventArgs args)
        {
           /// Debug.WriteLine("Owner Capture Lost");

            if (_dragOrigin.HasValue &&
                (_contextMenuState == AnimationState.InitialDelay))
            {
                _dragOrigin = null;
                ClosePopup();
            }
        }
        
        private void hold_Completed(object sender, EventArgs e)
        {
            _contextMenuState = AnimationState.ShowingProgress;
            //Debug.WriteLine("Showing Progress");
            ShowPopupAsync();
        }

        private void openUpProgress_Completed(object sender, EventArgs e)
        {
            _contextMenuState = AnimationState.Opening;
            //Debug.WriteLine("Opening");

            // If the application bar is visible, hide it.
            FrameworkElement rootVisual = (FrameworkElement)Application.Current.RootVisual;
            PhoneApplicationFrame rootFrame = rootVisual as PhoneApplicationFrame;
            if (rootFrame != null)
            {
                PhoneApplicationPage currentPage = rootFrame.Content as PhoneApplicationPage;
                if (currentPage != null)
                {
                    _isAppBarVisible = currentPage.ApplicationBar.IsVisible;
                    currentPage.ApplicationBar.IsVisible = false;
                }
            }

            openUp.Begin();
        }

        private void openDownProgress_Completed(object sender, EventArgs e)
        {
            _contextMenuState = AnimationState.Opening;
            //Debug.WriteLine("Opening");

            // If the application bar is visible, hide it.
            FrameworkElement rootVisual = (FrameworkElement)Application.Current.RootVisual;
            PhoneApplicationFrame rootFrame = rootVisual as PhoneApplicationFrame;
            if (rootFrame != null)
            {
                PhoneApplicationPage currentPage = rootFrame.Content as PhoneApplicationPage;
                if (currentPage != null)
                {
                    _isAppBarVisible = currentPage.ApplicationBar.IsVisible;
                    currentPage.ApplicationBar.IsVisible = false;
                }
            }

            openDown.Begin();
        }

        private void open_Completed(object sender, EventArgs e)
        {
            _contextMenuState = AnimationState.Opened;
            //Debug.WriteLine("Opened");
        }

        private void reset_Completed(object sender, EventArgs e)
        {
            _contextMenuState = AnimationState.None;
            //Debug.WriteLine("None");
        }

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("Layout Root Mouse Down");
            // Close the menu if the user clicks anywhere outside the context menu
            ClosePopup();
        }

        private void LayoutRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("Layout Root Mouse Up");
            if (_contextMenuState == AnimationState.ShowingProgress)
            {
                e.Handled = true;
                ClosePopup();
            }
        }

        private void contextMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("Context Menu Mouse Down");
            // Handle the input on the context menu so that the layout root doesn't get it
            e.Handled = true;
        }

        private void ShowPopupAsync()
        {
            if (_contextMenuPopup == null)
            {
                _contextMenuPopup = new Popup();

                _contextMenuPopup.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                _contextMenuPopup.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                _contextMenuPopup.HorizontalOffset = 0;
                _contextMenuPopup.VerticalOffset = 0;
                _contextMenuPopup.Child = this;

                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            }

            FrameworkElement rootVisual = (FrameworkElement)Application.Current.RootVisual;
            _contextMenuPopup.Height = rootVisual.ActualHeight;
            _contextMenuPopup.Width = rootVisual.ActualWidth;
            Height = rootVisual.ActualHeight;
            Width = rootVisual.ActualWidth;

            // Find the item bounds compared to the root visual
            Rect bounds = new Rect();
            try
            {
                GeneralTransform transform = AttachedItem.TransformToVisual(Application.Current.RootVisual);
                bounds = transform.TransformBounds(new Rect(0, 0, AttachedItem.ActualWidth, AttachedItem.ActualHeight));
            }
            catch (Exception)
            {
                // If we failed here it's because something isn't part of the visual tree yet.
                // Just cancel out of everything
                ClosePopup();
                return;
            }

            ScrollViewer scroll = AttachedItem.FindVisualParent<ScrollViewer>();
            if (scroll != null)
            {
                _scrollBarVisibility = scroll.VerticalScrollBarVisibility;
                scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }

            Pivot pivot = AttachedItem.FindVisualParent<Pivot>();
            if (pivot != null)
            {
                if (_pivotSelectionChangedHandler == null)
                {
                    _pivotSelectionChangedHandler = new SelectionChangedEventHandler(pivot_SelectionChanged);
                }
                pivot.SelectionChanged += _pivotSelectionChangedHandler;
            }

            bool isOpeningUp = (bounds.Y + contextBorder.Height + APPLICATION_BAR_HEIGHT >= Height);

            if (isOpeningUp)
            {
                contextBorder.Margin = new Thickness(0, bounds.Y - contextBorder.Height, 0, 0);
            }
            else
            {
                contextBorder.Margin = new Thickness(0, bounds.Y + bounds.Height, 0, 0);
            }

            _contextMenuPopup.IsOpen = true;

            StartItemFadeOutAnimation();

            if (isOpeningUp)
            {
                openUpProgress.Begin();
            }
            else
            {
                openDownProgress.Begin();
            }
        }

        private SelectionChangedEventHandler _pivotSelectionChangedHandler = null;
        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // We can't stop this (easily). So if the user swipes to the left or right
            // on the first hold, just close the popup.
            ClosePopup();
        }

        private void ClosePopup()
        {
            // Stop all animations
            hold.Stop();
            openDownProgress.Stop();
            openUpProgress.Stop();
            openDown.Stop();
            openUp.Stop();

            reset.Begin();

            if (_contextMenuPopup != null)
            {
                _contextMenuPopup.IsOpen = false;
                _contextMenuPopup.Child = null;
                _contextMenuPopup = null;
            }

            Pivot pivot = AttachedItem.FindVisualParent<Pivot>();
            if (pivot != null)
            {
                pivot.SelectionChanged -= _pivotSelectionChangedHandler;
            }

            if (_scrollBarVisibility.HasValue)
            {
                ScrollViewer scroll = AttachedItem.FindVisualParent<ScrollViewer>();
                if (scroll != null)
                {
                    scroll.VerticalScrollBarVisibility = _scrollBarVisibility.Value;
                }
            }

            // If the application bar is visible, hide it.
            if (_isAppBarVisible.HasValue)
            {
                PhoneApplicationFrame rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                if (rootFrame != null)
                {
                    PhoneApplicationPage currentPage = rootFrame.Content as PhoneApplicationPage;
                    if (currentPage != null)
                    {
                        currentPage.ApplicationBar.IsVisible = _isAppBarVisible.Value;
                    }
                }
            }

            _contextMenuState = AnimationState.None;
            //Debug.WriteLine("None");

            StartItemFadeInAnimation();
        }

        private void AttachToOwner(FrameworkElement owner)
        {
            AttachedItem = owner;

            if (_mouseMoveHandler == null)
            {
                _mouseMoveHandler = new MouseEventHandler(owner_MouseMove);
            }

            if (_mouseLeftButtonDownHandler == null)
            {
                _mouseLeftButtonDownHandler = new MouseButtonEventHandler(owner_MouseLeftButtonDown);
            }

            if (_mouseLeftButtonUpHandler == null)
            {
                _mouseLeftButtonUpHandler = new MouseButtonEventHandler(owner_MouseLeftButtonUp);
            }

            if (_mouseCaptureLostHandler == null)
            {
                _mouseCaptureLostHandler = new MouseEventHandler(owner_MouseCaptureLost);
            }

            owner.MouseLeftButtonDown += _mouseLeftButtonDownHandler;
            owner.MouseLeftButtonUp += _mouseLeftButtonUpHandler;
            owner.MouseMove += _mouseMoveHandler;
            owner.LostMouseCapture += _mouseCaptureLostHandler;

        }

        private void DetachFromOwner(FrameworkElement owner)
        {
            AttachedItem = null;

            owner.MouseMove -= _mouseMoveHandler;
            owner.MouseLeftButtonDown -= _mouseLeftButtonDownHandler;
            owner.MouseLeftButtonUp -= _mouseLeftButtonUpHandler;
        }

        private double _itemFadeOutScale = 0.95d;
        private double _itemFadeOutOpacity = 0.65d;
        private Duration _itemFadeDuration = new Duration(TimeSpan.FromSeconds(0.4d));

        private void StartItemFadeInAnimation()
        {
            if (AttachedItem != null)
            {
                ItemsControl itemsControl = AttachedItem.FindVisualParent<ItemsControl>();
                if (itemsControl != null)
                {
                    ListBoxItem attachedContainer = AttachedItem.FindVisualParent<ListBoxItem>();
                    if (attachedContainer != null)
                    {
                        Storyboard itemFadeInAnimation = new Storyboard();
                        for(int i = 0; i < itemsControl.Items.Count; i++)
                        {
                            FrameworkElement container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                            // Animate all of them except the attached item.
                            if (container != null && container != attachedContainer)
                            {
                                container = (FrameworkElement)container.FindVisualChild<FrameworkElement>();
                                if (!(container.RenderTransform is ScaleTransform))
                                {
                                    container.RenderTransform = new ScaleTransform();
                                }

                                GeneralTransform transform = container.TransformToVisual(attachedContainer);
                                Point transformPoint = transform.Transform(new Point(0, 0));
                                double x = 1.0d / -container.ActualWidth;
                                double y = 1.0d / -container.ActualHeight;

                                container.RenderTransformOrigin = new Point(x * transformPoint.X, y * transformPoint.Y);

                                DoubleAnimation scaleXInAnimation = new DoubleAnimation();
                                scaleXInAnimation.To = 1.0d;
                                scaleXInAnimation.Duration = _itemFadeDuration;
                                Storyboard.SetTargetProperty(scaleXInAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                                Storyboard.SetTarget(scaleXInAnimation, container);
                                itemFadeInAnimation.Children.Add(scaleXInAnimation);

                                DoubleAnimation scaleYInAnimation = new DoubleAnimation();
                                scaleYInAnimation.To = 1.0d;
                                scaleYInAnimation.Duration = _itemFadeDuration;
                                Storyboard.SetTargetProperty(scaleYInAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                                Storyboard.SetTarget(scaleYInAnimation, container);
                                itemFadeInAnimation.Children.Add(scaleYInAnimation);

                                DoubleAnimation opacityInAnimation = new DoubleAnimation();
                                opacityInAnimation.To = 1.0d;
                                opacityInAnimation.Duration = _itemFadeDuration;
                                Storyboard.SetTargetProperty(opacityInAnimation, new PropertyPath("(UIElement.Opacity)"));
                                Storyboard.SetTarget(opacityInAnimation, container);
                                itemFadeInAnimation.Children.Add(opacityInAnimation);
                                
                            }
                        }

                        itemFadeInAnimation.Begin();
                    }
                }
            }
        }

        private void StartItemFadeOutAnimation()
        {
            if (AttachedItem != null)
            {
                ItemsControl itemsControl = AttachedItem.FindVisualParent<ItemsControl>();
                if (itemsControl != null)
                {
                    ListBoxItem attachedContainer = AttachedItem.FindVisualParent<ListBoxItem>();
                    if (attachedContainer != null)
                    {
                        Storyboard itemFadeOutAnimation = new Storyboard();
                        itemFadeOutAnimation.Duration = _itemFadeDuration;

                        for (int i = 0; i < itemsControl.Items.Count; i++)
                        {
                            FrameworkElement container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                            // Animate all of them except the attached item.
                            if (container != null && container != attachedContainer)
                            {
                                container = (FrameworkElement)container.FindVisualChild<FrameworkElement>();
                                if (!(container.RenderTransform is ScaleTransform))
                                {
                                    container.RenderTransform = new ScaleTransform();
                                }

                                Point transformPoint = new Point();
                                try
                                {
                                    GeneralTransform transform = container.TransformToVisual(attachedContainer);
                                    transformPoint = transform.Transform(new Point(0, 0));
                                }
                                catch (ArgumentException)
                                {
                                    ClosePopup();
                                    return;
                                }

                                double x = 1.0d / -container.ActualWidth;
                                double y = 1.0d / -container.ActualHeight;

                                container.RenderTransformOrigin = new Point(x * transformPoint.X, y * transformPoint.Y);

                                DoubleAnimation scaleXOutAnimation = new DoubleAnimation();
                                scaleXOutAnimation.To = _itemFadeOutScale;
                                scaleXOutAnimation.Duration = _itemFadeDuration;
                                Storyboard.SetTargetProperty(scaleXOutAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                                Storyboard.SetTarget(scaleXOutAnimation, container);
                                itemFadeOutAnimation.Children.Add(scaleXOutAnimation);

                                DoubleAnimation scaleYOutAnimation = new DoubleAnimation();
                                scaleYOutAnimation.To = _itemFadeOutScale;
                                scaleYOutAnimation.Duration = _itemFadeDuration;
                                Storyboard.SetTargetProperty(scaleYOutAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                                Storyboard.SetTarget(scaleYOutAnimation, container);
                                itemFadeOutAnimation.Children.Add(scaleYOutAnimation);

                                DoubleAnimation opacityOutAnimation = new DoubleAnimation();
                                opacityOutAnimation.To = _itemFadeOutOpacity;
                                opacityOutAnimation.Duration = _itemFadeDuration;
                                Storyboard.SetTargetProperty(opacityOutAnimation, new PropertyPath("(UIElement.Opacity)"));
                                Storyboard.SetTarget(opacityOutAnimation, container);
                                itemFadeOutAnimation.Children.Add(opacityOutAnimation);
                            }
                        }

                        itemFadeOutAnimation.Begin();
                    }
                }
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ContextMenuItem item = ((Button)sender).DataContext as ContextMenuItem;
            if (item != null)
            {
                item.FireClick(AttachedItem.DataContext);
            }

            ClosePopup();
        }

        private enum AnimationState { None, InitialDelay, ShowingProgress, Opening, Opened };
    }
}
