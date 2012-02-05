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
using Sharkfist.Phone.SilverlightCore;

namespace MediaControl.Client.WindowsPhone
{
    public partial class TrackListViewItem : ListViewItemBase
    {
        static TrackListViewItem()
        {
            TiltEffect.TiltableItems.Add(typeof(TrackListViewItem));
        }

        public TrackListViewItem()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TrackNumberProperty =
            DependencyProperty.Register("TrackNumber", typeof(int), typeof(TrackListViewItem),
            new PropertyMetadata(0));

        public int TrackNumber
        {
            get { return (int)GetValue(TrackNumberProperty); }
            set { SetValue(TrackNumberProperty, value); }
        }

        //protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);

        //    OnClick.Begin();
        //}

        //protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonUp(e);

        //    OnRelease.Begin();
        //}

        //protected override void OnMouseLeave(MouseEventArgs e)
        //{
        //    base.OnMouseLeave(e);

        //    OnRelease.Begin();
        //}
    }
}
