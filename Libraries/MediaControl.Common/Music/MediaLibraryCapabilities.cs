using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaControl.Common.Music
{
    [DataContract]
    public class MediaLibraryCapabilities
    {
        [DataMember]
        public bool IsNowPlayingListAvailable { get; set; }
    }
}
