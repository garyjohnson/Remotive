using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MediaControl.Common;
using System.ServiceModel.Description;

namespace MediaControl.Host.iTunes
{
    internal class EndpointService
    {
        private static readonly EndpointService _current = new EndpointService();
        public static EndpointService Current
        {
            get { return _current; }
        }

        private EndpointService() { }

        private readonly ServiceHost libraryServiceHost = new ServiceHost(typeof(MediaLibraryService),
            new Uri(@"http://localhost:8888/MediaLibrary")); 

        private readonly ServiceHost playbackServiceHost = new ServiceHost(typeof(MediaPlaybackService),
            new Uri(@"http://localhost:8888/MediaPlayback"));

        private readonly ServiceHost streamingServiceHost = new ServiceHost(typeof(MediaStreamingService),
            new Uri(@"http://localhost:8888/MediaStreaming"));

        public void InitializeService()
        {
            libraryServiceHost.AddServiceEndpoint(typeof(IMediaLibraryService),
                new BasicHttpBinding(BasicHttpSecurityMode.None), string.Empty); 

            playbackServiceHost.AddServiceEndpoint(typeof(IMediaPlaybackService),
                new BasicHttpBinding(BasicHttpSecurityMode.None), string.Empty);

            BasicHttpBinding streamBinding = new BasicHttpBinding(BasicHttpSecurityMode.None)
            {
                TransferMode = System.ServiceModel.TransferMode.StreamedResponse
            };

            streamingServiceHost.AddServiceEndpoint(typeof(IMediaStreamingService),
                streamBinding, string.Empty);

            ServiceMetadataBehavior libraryBehavior = new ServiceMetadataBehavior();
            libraryBehavior.HttpGetEnabled = true;
            libraryServiceHost.Description.Behaviors.Add(libraryBehavior);
            libraryServiceHost.AddServiceEndpoint(typeof(IMetadataExchange),
                MetadataExchangeBindings.CreateMexHttpBinding(), @"mex");

            ServiceMetadataBehavior playbackBehavior = new ServiceMetadataBehavior();
            playbackBehavior.HttpGetEnabled = true;
            playbackServiceHost.Description.Behaviors.Add(playbackBehavior);
            playbackServiceHost.AddServiceEndpoint(typeof(IMetadataExchange),
                MetadataExchangeBindings.CreateMexHttpBinding(), @"mex");

            ServiceMetadataBehavior streamBehavior = new ServiceMetadataBehavior();
            streamBehavior.HttpGetEnabled = true;
            streamingServiceHost.Description.Behaviors.Add(streamBehavior);
            streamingServiceHost.AddServiceEndpoint(typeof(IMetadataExchange),
                MetadataExchangeBindings.CreateMexHttpBinding(), @"mex");

            libraryServiceHost.Open();
            playbackServiceHost.Open();
            streamingServiceHost.Open();
        }

        public void TeardownService()
        {
            libraryServiceHost.Close();
            playbackServiceHost.Close();
            streamingServiceHost.Close();
        }
    }
}
