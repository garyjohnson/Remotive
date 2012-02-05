using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaControl.Common.Music;
using iTunesLib;
using System.Globalization;

namespace MediaControl.Host.iTunes
{
    internal static class ExtensionMethods
    {
        public static Artist GetArtist(this IITFileOrCDTrack track)
        {
            Artist artist = new Artist();
            artist.Name = track.Artist;
            return artist;
        }

        public static AlbumArtist GetAlbumArtist(this IITFileOrCDTrack track)
        {
            AlbumArtist artist = new AlbumArtist();
            artist.Name = track.AlbumArtist;
            return artist;
        }

        public static Track GetTrack(this IITFileOrCDTrack track)
        {
            Track innerTrack = new Track();
            innerTrack.Title = track.Name;
            int trackNumber = track.TrackNumber;
            innerTrack.FilePath = track.Location;
            innerTrack.AlbumName = track.Album;
            innerTrack.ArtistName = track.Artist;
            innerTrack.Duration = TimeSpan.FromSeconds(track.Duration);
            return innerTrack;
        }

        public static Album GetAlbum(this IITFileOrCDTrack track)
        {
            Album album = new Album();
            album.Title = track.Album;
            if (!string.IsNullOrEmpty(track.AlbumArtist))
            {
                album.ArtistName = track.AlbumArtist;
            }
            else
            {
                album.ArtistName = track.Artist;
            }
            album.Genre = track.Genre;
            // Should be somewhat unique, iTunes doesn't have an album id concept
            album.ID = album.ArtistName + album.Title;

            string releaseYear = track.Year.ToString();
            if (!string.IsNullOrEmpty(releaseYear))
            {
                DateTime releaseDate;
                if (DateTime.TryParseExact(releaseYear, new string[] { "yyyy" },
                    CultureInfo.CurrentCulture.NumberFormat,
                    DateTimeStyles.None, out releaseDate))
                {
                    album.ReleaseYear = releaseDate;
                }
            }

            return album;
        }
    }
}
