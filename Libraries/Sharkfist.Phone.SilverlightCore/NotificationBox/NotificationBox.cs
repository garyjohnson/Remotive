/*
 * Pulled from http://blogs.microsoft.co.il/blogs/tomershamam/archive/2010/10/19/windows-phone-7-custom-message-box.aspx
 * Tomer Shamam's Microsoft Blog on 03/28/2011
 * Used without explicit license.
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
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using System.ComponentModel;
using Microsoft.Phone.Shell;

namespace Sharkfist.Phone.SilverlightCore
{
    /// <summary>
    /// Represents a message-box like control for displaying a message box
    /// with custom actions an option to persist 'show message again' state.
    /// </summary>
    public class NotificationBox : Control
    {
        #region Fields

        private static Popup _popup;
        private static bool _appBarVisibility;

        #endregion

        #region Properties

        #region CommandsSource Property

        /// <summary>
        /// Gets or sets a collection of <see cref="NotificationBoxCommand"/>.
        /// </summary>
        public IEnumerable<NotificationBoxCommand> CommandsSource
        {
            get { return (IEnumerable<NotificationBoxCommand>)GetValue(CommandsSourceProperty); }
            set { SetValue(CommandsSourceProperty, value); }
        }

        /// <value>Identifies the CommandsSource dependency property</value>
        public static readonly DependencyProperty CommandsSourceProperty =
            DependencyProperty.Register(
            "CommandsSource",
            typeof(IEnumerable<NotificationBoxCommand>),
            typeof(NotificationBox),
              new PropertyMetadata(default(IEnumerable<NotificationBoxCommand>), CommandsSourceChanged));

        private static void CommandsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newCommands = e.NewValue as IEnumerable<NotificationBoxCommand>;
            if (newCommands != null)
            {
                var box = d as NotificationBox;
                foreach (var newCommand in newCommands)
                {
                    newCommand.Owner = box;
                }
            }
        }

        #endregion

        #region Title Property

        /// <summary>
        /// Gets or sets the title text to display.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <value>Identifies the Title dependency property</value>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
            "Title",
            typeof(string),
            typeof(NotificationBox),
              new PropertyMetadata(default(string)));

        #endregion

        #region Message Property

        /// <summary>
        /// Gets or sets the message text to display.
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        /// <value>Identifies the Message dependency property</value>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
            "Message",
            typeof(string),
            typeof(NotificationBox),
              new PropertyMetadata(default(string)));

        #endregion

        #region ShowAgainOption Property

        /// <summary>
        /// Gets or sets a value for indicating if this message is to be shown again.
        /// </summary>
        public bool ShowAgainOption
        {
            get { return (bool)GetValue(ShowAgainOptionProperty); }
            set { SetValue(ShowAgainOptionProperty, value); }
        }

        /// <value>Identifies the ShowAgainOption dependency property</value>
        public static readonly DependencyProperty ShowAgainOptionProperty =
            DependencyProperty.Register(
            "ShowAgainOption",
            typeof(bool),
            typeof(NotificationBox),
              new PropertyMetadata(default(bool), ShowAgainOptionChanged));

        /// <summary>
        /// Invoked on ShowAgainOption change.
        /// </summary>
        /// <param name="d">The object that was changed</param>
        /// <param name="e">Dependency property changed event arguments</param>
        private static void ShowAgainOptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var message = d as NotificationBox;
            Settings[message.UniqueKey] = e.NewValue;
        }

        #endregion

        #region ShowAgainText Property

        /// <summary>
        /// Gets or sets the text asking if this message should be shown again.
        /// </summary>
        public string ShowAgainText
        {
            get { return (string)GetValue(ShowAgainTextProperty); }
            set { SetValue(ShowAgainTextProperty, value); }
        }

        /// <value>Identifies the ShowAgainText dependency property</value>
        public static readonly DependencyProperty ShowAgainTextProperty =
            DependencyProperty.Register(
            "ShowAgainText",
            typeof(string),
            typeof(NotificationBox),
              new PropertyMetadata(default(string)));

        #endregion

        #region ShowAgainVisibility Property

        /// <summary>
        /// Gets or sets a value indicating if the show again message is visible or not.
        /// </summary>
        public Visibility ShowAgainVisibility
        {
            get { return (Visibility)GetValue(ShowAgainVisibilityProperty); }
            set { SetValue(ShowAgainVisibilityProperty, value); }
        }

        /// <value>Identifies the ShowAgainVisibility dependency property</value>
        public static readonly DependencyProperty ShowAgainVisibilityProperty =
            DependencyProperty.Register(
            "ShowAgainVisibility",
            typeof(Visibility),
            typeof(NotificationBox),
              new PropertyMetadata(default(Visibility)));

        #endregion

        private static IsolatedStorageSettings Settings
        {
            get { return IsolatedStorageSettings.ApplicationSettings; }
        }

        private string UniqueKey { get; set; }

        private Action<bool> SuppressionCallback { get; set; }

        #endregion

        #region Ctor

        private NotificationBox()
        {
            DefaultStyleKey = typeof(NotificationBox);
        }

        #endregion        

        #region Utilities

        /// <summary>
        /// Displays a notification box with title, message and custom actions.
        /// </summary>
        /// <param name="title">The title of this message.</param>
        /// <param name="message">The message body text.</param>
        /// <param name="commands">A collection of actions.</param>
        public static void Show(string title, string message, params NotificationBoxCommand[] commands)
        {
            if (_popup != null)
            {
                throw new InvalidOperationException("Message is already shown.");
            }

            HandleBackKeyAndAppBar();
            _popup = new Popup
            {
                IsOpen = true,
                Child = new NotificationBox
                {
                    Width = CurrentPage.ActualWidth,
                    Height = CurrentPage.ActualHeight,
                    Title = title,
                    Message = message,
                    CommandsSource = commands,
                    ShowAgainVisibility = Visibility.Collapsed
                }
            };                       
        }        

        /// <summary>
        /// Displays a notification box with title, message and custom actions.
        /// In addition a message asking if this message should be shown again next time.
        /// </summary>
        /// <param name="title">The title of this message.</param>
        /// <param name="message">The message body text.</param>        
        /// <param name="showAgainText">The text asking if this message should be shown again.</param>
        /// <param name="forceShowAgain">Value indicating whether to force message display in case that the user suppressed this message, </param>
        /// <param name="suppression">Callback for indicating whether message suppressed or not..</param>
        /// <param name="uniqueKey">Unique key representing a specific message identity.</param>
        /// <param name="commands">A collection of actions.</param>
        public static void ShowAgain(string title, string message, string showAgainText, bool forceShowAgain, Action<bool> suppression, string uniqueKey, params NotificationBoxCommand[] commands)
        {
            if (_popup != null)
            {
                throw new InvalidOperationException("Message is already shown.");
            }

            bool showAgain;
            if (!Settings.TryGetValue(uniqueKey, out showAgain))
            {
                showAgain = true;
                Settings[uniqueKey] = showAgain;
            }

            if (showAgain || forceShowAgain)
            {
                HandleBackKeyAndAppBar();
                _popup = new Popup
                {
                    IsOpen = true,
                    Child = new NotificationBox
                    {
                        Width = CurrentPage.ActualWidth,
                        Height = CurrentPage.ActualHeight,
                        UniqueKey = uniqueKey.ToString(),
                        Title = title,
                        Message = message,
                        CommandsSource = commands,
                        ShowAgainOption = showAgain,
                        ShowAgainText = showAgainText,
                        ShowAgainVisibility = Visibility.Visible,
                        SuppressionCallback = suppression
                    }
                };
            }
        }

        /// <summary>
        /// Displays a notification box with title, message and custom actions.
        /// In addition a message asking if this message should be shown again next time.
        /// </summary>
        /// <param name="title">The title of this message.</param>
        /// <param name="message">The message body text.</param>        
        /// <param name="forceShowAgain">Value indicating whether to force message display in case that the user suppressed this message, </param>
        /// <param name="suppression">Callback for indicating whether message suppressed or not..</param>
        /// <param name="uniqueKey">Unique key representing a specific message identity.</param>
        /// <param name="commands">A collection of actions.</param>
        public static void ShowAgain(string title, string message, bool forceShowAgain, Action<bool> suppression, string uniqueKey, params NotificationBoxCommand[] commands)
        {
            ShowAgain(title, message, "Show this message again", forceShowAgain, suppression, uniqueKey, commands);
        }

        internal void Close()
        {
            if (SuppressionCallback != null)
            {
                SuppressionCallback(!ShowAgainOption);
            }

            ClosePopup();
        }

        #endregion

        #region Privates

        private static PhoneApplicationPage CurrentPage
        {
            get
            {
                var rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                var currentPage = rootFrame.Content as PhoneApplicationPage;
                return currentPage;
            }
        }

        private static void HandleBackKeyAndAppBar()
        {
            CurrentPage.BackKeyPress += parentPage_BackKeyPress;
            if (CurrentPage.ApplicationBar != null)
            {
                _appBarVisibility = CurrentPage.ApplicationBar.IsVisible;
                CurrentPage.ApplicationBar.IsVisible = false;
            }
        }

        private static void parentPage_BackKeyPress(object sender, CancelEventArgs e)
        {
            CurrentPage.BackKeyPress -= parentPage_BackKeyPress;            
            ClosePopup();

            e.Cancel = true;
        }

        private static void ClosePopup()
        {
            if (_popup != null)
            {
                _popup.IsOpen = false;
                _popup = null;
            }

            if (CurrentPage.ApplicationBar != null)
            {
                CurrentPage.ApplicationBar.IsVisible = _appBarVisibility;
            }
        }

        #endregion
    }
}
