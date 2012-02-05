using System;
using System.Collections.Generic;
using System.ServiceModel;
using MediaControl.Client.Console.MediaLibrary;

namespace MediaControl.Client.Console
{
    public class MediaService
    {
        private static readonly MediaService _current = new MediaService();
        public static MediaService Current
        {
            get { return _current; }
        }

        private MediaService()
        {
            LibraryServiceClient.GetAlbumsCompleted += new EventHandler<GetAlbumsCompletedEventArgs>(serviceClient_GetAlbumsCompleted);
            LibraryServiceClient.GetAlbumArtCompleted += new EventHandler<GetAlbumArtCompletedEventArgs>(LibraryServiceClient_GetAlbumArtCompleted);
          
        }

        private string hostName = "localhost";
        //private string hostName = "VM-WIN7-X86";
        //private string hostName = "1420p";
        private string portNumber = "8888";


        private MediaLibraryServiceClient _libraryServiceClient = null;
        private MediaLibraryServiceClient LibraryServiceClient
        {
            get
            {
                if (_libraryServiceClient == null ||
                _libraryServiceClient.State == CommunicationState.Faulted)
                {
                    BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                    binding.MaxReceivedMessageSize = 67108864;
                    _libraryServiceClient = new MediaLibraryServiceClient(binding,
                    new EndpointAddress(string.Format(@"http://{0}:{1}/MediaLibrary", hostName, portNumber)));
                }

                return _libraryServiceClient;
            }
        }

       
        public void GetAlbumsAsync(Action<List<Album>, object> callback, object state)
        {
            Pair<Action<List<Album>, object>, object> pair = new Pair<Action<List<Album>, object>, object>(callback, state);
            LibraryServiceClient.GetAlbumsAsync(new GetAlbumsRequest(), pair);
        }

        private void serviceClient_GetAlbumsCompleted(object sender, GetAlbumsCompletedEventArgs e)
        {
            Pair<Action<List<Album>, object>, object> innerPair = (Pair<Action<List<Album>, object>, object>)e.UserState;
            innerPair.Value1.Invoke(e.Result.GetAlbumsResult, innerPair.Value2);
        }

        public void GetAlbumArtAsync(MediaControl.Client.Console.MediaLibrary.Track track,
            Action<byte[], object> callback, object state)
        {
            Pair<Action<byte[], object>, object> pair =
                new Pair<Action<byte[], object>, object>(callback, state);
            LibraryServiceClient.GetAlbumArtAsync(new GetAlbumArtRequest(track), pair);
        }

        private void LibraryServiceClient_GetAlbumArtCompleted(object sender, GetAlbumArtCompletedEventArgs e)
        {
            Pair<Action<byte[], object>, object> innerPair = (Pair<Action<byte[], object>, object>)e.UserState;
            innerPair.Value1.Invoke(e.Result.GetAlbumArtResult, innerPair.Value2);
        }
    }
}
