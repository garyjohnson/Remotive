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
using System.ServiceModel;
using MediaControl.Client.MediaLibrary;
using System.Collections.Generic;

namespace MediaControl.Client
{
    public class MediaLibraryService
    {
        private static readonly MediaLibraryService _current = new MediaLibraryService();
        public static MediaLibraryService Current
        {
            get { return _current; }
        }

        private MediaLibraryService()
        {
            libraryService = httpFactory.CreateChannel();
        }

        private MediaLibrary.IMediaLibraryService libraryService = null;

        private readonly ChannelFactory<MediaLibrary.IMediaLibraryService> httpFactory =
            new ChannelFactory<MediaLibrary.IMediaLibraryService>(new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress(@"http://VM-WIN7-XP/MediaLibrary"));

        public IAsyncResult BeginGetAlbums(Action<List<Album>, object> callback, object state)
        {
            Pair<Action<List<Album>, object>, object> pair = new Pair<Action<List<Album>, object>, object>(callback, state);

            return libraryService.BeginGetAlbums(new MediaLibrary.GetAlbumsRequest(), new AsyncCallback((result)=>
            {
                Pair<Action<List<Album>, object>, object> innerPair = (Pair<Action<List<Album>, object>, object>)result.AsyncState;
                GetAlbumsResponse response = libraryService.EndGetAlbums(result);

                innerPair.Value1.BeginInvoke(response.GetAlbumsResult, innerPair.Value2, null, null);
            }), 
            pair);                
        }

        //public System.Collections.Generic.IEnumerable<Artist> GetArtists()
        //{
        //    return libraryService.GetArtists();
        //}

        //public System.Collections.Generic.IEnumerable<Artist> GetAlbumArtists()
        //{
        //    return libraryService.GetAlbumArtists();
        //}

        //public System.Collections.Generic.IEnumerable<Track> GetTracks(Album album)
        //{
        //    return libraryService.GetTracks(album);
        //}
    }
}
