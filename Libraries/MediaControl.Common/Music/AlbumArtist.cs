﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaControl.Common.Music
{
    [DataContract]
    public class AlbumArtist
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IEnumerable<Album> Albums { get; set; }
    }
}
