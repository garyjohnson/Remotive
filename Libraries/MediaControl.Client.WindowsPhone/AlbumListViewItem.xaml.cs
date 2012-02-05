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
using System.Diagnostics;
using System.Windows.Data;
using MediaControl.Client.WindowsPhone.ValueConverters;

namespace MediaControl.Client.WindowsPhone
{
    public partial class AlbumListViewItem : ListViewItemBase
    {
        static AlbumListViewItem()
        {
            TiltEffect.TiltableItems.Add(typeof(AlbumListViewItem));
        }

        public AlbumListViewItem()
        {
            InitializeComponent();

            Binding itemBinding = new Binding("Text");
            itemBinding.Source = this;
            ItemText.SetBinding(TextBlock.TextProperty, itemBinding);

            Binding detailsBinding = new Binding("Details");
            detailsBinding.Source = this;
            DetailsText.SetBinding(TextBlock.TextProperty, detailsBinding);

            Binding idBinding = new Binding("AlbumID");
            idBinding.Source = this;
            albumControl.SetBinding(AlbumControl.AlbumIDProperty, idBinding);

            //Binding contextBinding = new Binding("DataContext");
            //contextBinding.Source = this;
            //contextBinding.Converter = new NullToVisibilityConverter();
            //albumControl.SetBinding(AlbumControl.VisibilityProperty, contextBinding);
        }

        // Use this property to watch for album change
        public static readonly DependencyProperty AlbumIDProperty =
            DependencyProperty.Register("AlbumID", typeof(string), typeof(AlbumListViewItem),
            new PropertyMetadata(null));     

        public string AlbumID
        {
            get { return (string)GetValue(AlbumIDProperty); }
            set { SetValue(AlbumIDProperty, value); }
        }
    }
}
