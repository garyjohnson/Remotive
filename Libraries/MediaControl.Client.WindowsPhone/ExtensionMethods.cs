
using System.Windows.Navigation;
using System;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Generic;
namespace MediaControl.Client.WindowsPhone
{
    public static class ExtensionMethods
    {
        public static void Navigate(this NavigationService service, Uri source, object dataContext)
        {
            PageState.Current.CurrentPageUri = source;
            PageState.Current.CurrentDataContext = dataContext;
            service.Navigate(PageState.Current.CurrentPageUri);
        }

        public static MediaLibrary.Track ConvertToTrack(this MediaPlayback.Track track)
        {
            return new MediaLibrary.Track()
            {
                AlbumName = track.AlbumName,
                ArtistName = track.ArtistName,
                Duration = track.Duration,
                FilePath = track.FilePath,
                Title = track.Title,
                TrackNumber = track.TrackNumber
            };
        }

        public static bool EqualsTrack(this MediaLibrary.Track track, MediaLibrary.Track track2)
        {
            if (track == null && track2 == null)
            {
                return true;
            }
            else if (track == null || track2 == null)
            {
                return false;
            }

            return (track.FilePath == track2.FilePath);
        }

        public static bool EqualsTrack(this MediaLibrary.Track track, MediaPlayback.Track track2)
        {
            if (track == null && track2 == null)
            {
                return true;
            }
            else if (track == null || track2 == null)
            {
                return false;
            }

            return (track.FilePath == track2.FilePath);
        }

        public static bool EqualsTrack(this MediaPlayback.Track track, MediaPlayback.Track track2)
        {
            if (track == null && track2 == null)
            {
                return true;
            }
            else if (track == null || track2 == null)
            {
                return false;
            }

            return (track.FilePath == track2.FilePath);
        }

        public static bool EqualsTrack(this MediaPlayback.Track track, MediaLibrary.Track track2)
        {
            if (track == null && track2 == null)
            {
                return true;
            }
            else if (track == null || track2 == null)
            {
                return false;
            }

            return (track.FilePath == track2.FilePath); 
        }

        public static MediaPlayback.Track ConvertToTrack(this MediaLibrary.Track track)
        {
            return new MediaPlayback.Track()
            {
                AlbumName = track.AlbumName,
                ArtistName = track.ArtistName,
                Duration = track.Duration,
                FilePath = track.FilePath,
                Title = track.Title,
                TrackNumber = track.TrackNumber
            };
        }

        public static MediaPlayback.Artist ConvertToArtist(this MediaLibrary.Artist artist)
        {
            return new MediaPlayback.Artist()
            {
                Name = artist.Name
            };
        }

        public static MediaPlayback.AlbumArtist ConvertToArtist(this MediaLibrary.AlbumArtist artist)
        {
            MediaPlayback.AlbumArtist newArtist = new MediaPlayback.AlbumArtist();
            newArtist.Name = artist.Name;
            newArtist.Albums = new List<MediaPlayback.Album>();
            foreach (MediaLibrary.Album album in artist.Albums)
            {
                newArtist.Albums.Add(album.ConvertToAlbum());
            }

            return newArtist;
        }

        public static MediaPlayback.Album ConvertToAlbum(this MediaLibrary.Album album)
        {
            return new MediaPlayback.Album()
            {
                AlbumArt = album.AlbumArt,
                ArtistName = album.ArtistName,
                Genre = album.Genre,
                ID = album.ID,
                ReleaseYear = album.ReleaseYear,
                Title = album.Title
            };
        }
    }
}
