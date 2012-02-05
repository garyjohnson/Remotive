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

namespace MediaControl.Client.WindowsPhone
{
    public class AlbumClickedArgs : EventArgs
    {
        public AlbumClickedArgs(MediaLibrary.Album album)
        {
            Album = album;
        }

        public MediaLibrary.Album Album { get; private set; }
    }
}
