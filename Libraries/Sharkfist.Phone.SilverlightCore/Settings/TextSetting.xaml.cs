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
using System.Windows.Data;

namespace Sharkfist.Phone.SilverlightCore.Settings
{
    public partial class TextSetting : UserControl
    {
        public TextSetting()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SettingNameProperty =
            DependencyProperty.Register("SettingName", typeof(string), typeof(TextSetting), new PropertyMetadata(null));

        public string SettingName
        {
            get { return (string)GetValue(SettingNameProperty); }
            set { SetValue(SettingNameProperty, value); }
        }

        public static readonly DependencyProperty SettingValueProperty =
            DependencyProperty.Register("SettingValue", typeof(string), typeof(TextSetting), new PropertyMetadata(null));

        public string SettingValue
        {
            get { return (string)GetValue(SettingValueProperty); }
            set { SetValue(SettingValueProperty, value); }
        }

        public void FocusInput()
        {
            input.Focus();
        }
    }
}
