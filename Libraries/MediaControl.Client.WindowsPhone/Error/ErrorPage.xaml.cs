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
using Microsoft.Phone.Controls;
using MediaControl.Client.WindowsPhone;
using Microsoft.Phone.Tasks;
using System.Globalization;
using System.Text;
using Sharkfist.Phone.SilverlightCore;

namespace MediaControl_Client_WindowsPhone
{
    public partial class ErrorPage : PhoneApplicationPage
    {
        public ErrorPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            CloseApplication();
        }

        private void sendErrorButton_Click(object sender, RoutedEventArgs e)
        {
            Exception ex = null;
            if (Configuration.TryLoadStateSetting<Exception>("UnhandledException", out ex))
            {
                SendEmail(ex);
            }

            CloseApplication();
        }

        private void CloseApplication()
        {
            throw new CloseApplicationException();
        }

        private void SendEmail(Exception ex)
        {
            string currentDateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            EmailComposeTask task = new EmailComposeTask()
            {
                To = "sharkfist@hotmail.com",
                Subject = string.Format("Unhandled Remotive Error on {0}", currentDateTime),
                Body = string.Format("An error occurred in Remotive on {1}:{0}{2}",
                Environment.NewLine,
                currentDateTime,

                GetExceptionString(ex))
            };

            task.Show();
        }

        private static string GetExceptionString(Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            Exception currentException = ex;

            do
            {
                builder.Append("Type: ");
                builder.Append(currentException.GetType().Name);
                builder.AppendLine();

                builder.Append("Message: ");
                builder.Append(currentException.Message);
                builder.AppendLine();


                builder.Append("StackTrace: ");
                builder.Append(currentException.StackTrace);
                builder.AppendLine();

                currentException = currentException.InnerException;
            }
            while (currentException != null);

            return builder.ToString();
        }
    }
}
