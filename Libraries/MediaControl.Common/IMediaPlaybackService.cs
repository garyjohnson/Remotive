using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MediaControl.Common.Music;

namespace MediaControl.Common
{
    [ServiceContract(Namespace = "http://www.ggjonline.com/ServiceModel/MediaControl")]
    public interface IMediaPlaybackService
    {
        [OperationContract]
        MediaState Play();

        [OperationContract]
        MediaState Pause();

        [OperationContract]
        MediaState NextTrack();

        [OperationContract]
        MediaState PrevTrack();

        [OperationContract]
        MediaState PlayTrack(Track track);

        [OperationContract]
        MediaState GetMediaState();

        [OperationContract]
        MediaState PlayTracks(IEnumerable<Track> trackQueue, bool enqueue);

        [OperationContract]
        MediaState PlayArtist(Artist artist, bool enqueue);

        [OperationContract]
        MediaState PlayAlbumArtist(AlbumArtist artist, bool enqueue);

        [OperationContract]
        MediaState PlayAlbum(Album album, bool enqueue);

        [OperationContract]
        MediaPlaybackCapabilities GetCapabilities();

        [OperationContract]
        void Ping();

        [OperationContract]
        MediaState SetVolume(double volume);
    }
}
