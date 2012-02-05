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
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using MediaControl.Client.WindowsPhone.MediaPlayback;
using System.ComponentModel;
using Microsoft.Phone.Shell;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Sharkfist.Phone.SilverlightCore;

namespace MediaControl.Client.WindowsPhone.NowPlaying
{
    public partial class NowPlayingPage : MediaPhonePage
    {
        public NowPlayingPage()
        {
            InitializeComponent();

            if (!Designer.IsInDesignMode)
            {
                //StartPlaybackPolling();
                DataContext = new NowPlayingViewModel(Dispatcher);
            }
        }
    }
}
