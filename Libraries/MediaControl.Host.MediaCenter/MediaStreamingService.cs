using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaControl.Common;
using System.IO;

namespace MediaControl.Host.MediaCenter
{
    public class MediaStreamingService : IMediaStreamingService
    {
        public System.IO.Stream GetAudioStream(string url)
        {
            return new FileStream(url, FileMode.Open, FileAccess.Read);
        }

        public void Ping()
        {
            throw new NotImplementedException();
        }
    }
}
