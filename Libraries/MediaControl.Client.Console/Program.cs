using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaControl.Client.Console.MediaLibrary;

namespace MediaControl.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            MediaService.Current.GetAlbumsAsync((albums, state) =>
                {
                    foreach (Album album in albums)
                    {
                        System.Console.WriteLine(album.ArtistName + " - " + album.Title);
                    }

                    System.Console.ReadLine();
                }, null);

            System.Console.ReadLine();
        }
    }
}
