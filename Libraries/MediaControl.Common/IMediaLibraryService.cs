using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaControl.Common.Music;
using System.ServiceModel;

namespace MediaControl.Common
{
    [ServiceContract(Namespace="http://www.ggjonline.com/ServiceModel/MediaControl")]
    public interface IMediaLibraryService
    {
        [OperationContract]
        IEnumerable<Album> GetAlbumsByArtist(Artist artist);

        [OperationContract]
        IEnumerable<Track> GetTracks(Album album);

        [OperationContract]
        IEnumerable<Track> GetTracksByArtist(Artist artist);

        [OperationContract]
        IEnumerable<Track> GetTracksByAlbumArtist(AlbumArtist artist);

        [OperationContract]
        byte[] GetAlbumArtByTrack(Track track);

        [OperationContract]
        byte[] GetAlbumArtByAlbum(Album album);

        [OperationContract]
        MediaLibraryCapabilities GetCapabilities();

        [OperationContract]
        void Ping();

        [OperationContract]
        int GetAlbumArtistCount();

        [OperationContract]
        IEnumerable<AlbumArtist> GetAlbumArtistRange(int index, int count);

        [OperationContract]
        int GetArtistCount();

        [OperationContract]
        IEnumerable<Artist> GetArtistRange(int index, int count);

        [OperationContract]
        int GetAlbumCount();

        [OperationContract]
        IEnumerable<Album> GetAlbumRange(int index, int count);

        [OperationContract]
        Version GetServerVersion();
    }
}
