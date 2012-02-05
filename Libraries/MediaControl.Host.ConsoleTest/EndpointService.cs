using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaControl.Common;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace MediaControl.Host.ConsoleTest
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

        public void InitializeService()
        {
            BasicHttpBinding libraryBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            libraryBinding.MaxReceivedMessageSize = 67108864;
            libraryServiceHost.AddServiceEndpoint(typeof(IMediaLibraryService),
                libraryBinding, string.Empty);

            BasicHttpBinding playbackBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            playbackBinding.MaxReceivedMessageSize = 67108864;
            playbackServiceHost.AddServiceEndpoint(typeof(IMediaPlaybackService),
                playbackBinding, string.Empty);

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

            libraryServiceHost.Open();
            playbackServiceHost.Open();
        }

        public void TeardownService()
        {
            libraryServiceHost.Close();
            playbackServiceHost.Close();
        }
    }
}
