using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaControl.Common.Music
{
    [DataContract]
    public class Album
    {
        [DataMember]
        public string ArtistName { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public DateTime ReleaseYear { get; set; }
        [DataMember]
        public byte[] AlbumArt { get; set; }
        [DataMember]
        public string Genre { get; set; }
        [DataMember]
        public string ID { get; set; }
    }
}
