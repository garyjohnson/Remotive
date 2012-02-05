using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;

namespace MediaControl.Common
{
    [ServiceContract(Namespace = "http://www.ggjonline.com/ServiceModel/MediaControl")]
    public interface IMediaStreamingService
    {
        [OperationContract]
        Stream GetAudioStream(string url);

        [OperationContract]
        void Ping();
    }
}
