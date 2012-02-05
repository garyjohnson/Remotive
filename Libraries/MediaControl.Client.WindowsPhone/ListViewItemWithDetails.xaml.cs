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
using System.Windows.Media.Imaging;
using System.ComponentModel;
using Sharkfist.Phone.SilverlightCore;

namespace MediaControl.Client.WindowsPhone
{
    public partial class ListViewItemWithDetails : ListViewItemBase, IDisposable
    {
        static ListViewItemWithDetails()
        {
            TiltEffect.TiltableItems.Add(typeof(ListViewItemWithDetails));
        }

        public ListViewItemWithDetails()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
        }
    }
}
