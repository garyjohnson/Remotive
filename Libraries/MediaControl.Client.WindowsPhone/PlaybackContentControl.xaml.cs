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

namespace MediaControl.Client.WindowsPhone
{
    public partial class PlaybackContentControl : ContentControl
    {
        public PlaybackContentControl()
        {
            InitializeComponent();
        }

        public event EventHandler PlayClick;
        public event EventHandler AddClick;

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (PlayClick != null)
            {
                PlayClick(this, EventArgs.Empty);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            if (AddClick != null)
            {
                AddClick(this, EventArgs.Empty);
            }
        }
    }
}
