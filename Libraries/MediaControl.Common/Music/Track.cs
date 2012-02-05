using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaControl.Common.Music
{
    [DataContract]
    public class Track
    {
        [DataMember]
        public string ArtistName { get; set; }
        [DataMember]
        public TimeSpan Duration { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string FilePath { get; set; }
        [DataMember]
        public int TrackNumber { get; set; }
        [DataMember]
        public string AlbumName { get; set; }
    }
}
