using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaControl.Common.Music
{
    [DataContract]
    public class MediaState
    {
        [DataMember]
        public PlaybackState PlaybackState { get; set; }
        [DataMember]
        public Track CurrentTrack { get; set; }
        [DataMember]
        public DateTime CurrentTime { get; set; }
        [DataMember]
        public TimeSpan PlaybackPosition { get; set; }
        [DataMember]
        public double Volume { get; set; }
    }
}
