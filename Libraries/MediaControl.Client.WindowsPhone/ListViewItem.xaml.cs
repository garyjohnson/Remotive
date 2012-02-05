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
    public partial class ListViewItem : ListViewItemBase, IDisposable
    {
        static ListViewItem()
        {
            TiltEffect.TiltableItems.Add(typeof(ListViewItem));
        }

        public ListViewItem()
        {
            InitializeComponent();
        }

        //protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);

        //    OnClick.Begin();
        //}

        //protected override void OnMouseLeave(MouseEventArgs e)
        //{
        //    base.OnMouseLeave(e);

        //    OnRelease.Begin();
        //}

        //protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonUp(e);

        //    OnRelease.Begin();
        //}

        public void Dispose()
        {
        }
    }
}
