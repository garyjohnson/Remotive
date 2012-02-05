using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaControl.Common.Music
{
    [DataContract]
    public class MediaPlaybackCapabilities
    {

        [DataMember]
        public bool IsToggleShuffleAvailable { get; set; }


        [DataMember]
        public bool IsToggleLoopAvailable { get; set; }

        /// <summary>
        /// Determines if a list can be queued and a song played from any 
        /// position in the list. If not available, the list will be queued
        /// and the first song will be played (if shuffle is off).
        /// </summary>
        [DataMember]
        public bool IsPlaybackMidlistAvailable { get; set; }
    }
}
