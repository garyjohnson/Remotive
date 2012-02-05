using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaControl.Common.Music;
using MediaControl.Common;
using System.ServiceModel;
using System.IO;

namespace MediaControl.Host.ConsoleTest
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class MediaLibraryService : IMediaLibraryService
    {
        public MediaLibraryService() { }

        public IEnumerable<Album> GetAlbums()
        {
            yield return new Album()
            {
                Title = "Guero",
                ArtistName = "Beck"
            };

            yield return new Album()
            {
                Title = "Deloused in the Comatorium",
                ArtistName = "The Mars Volta"
            };

            yield return new Album()
            {
                Title = "Dark Side of the Moon", 
                ArtistName = "Pink Floyd"
            };

            yield return new Album()
            {
                Title = "Spice Girls Greatest Hits", 
                ArtistName = "Spice Girls"
            };

            yield return new Album()
            {
                Title = "Abbey Road",
                ArtistName = "The Beatles"
            };

            yield return new Album()
            {
                Title = "Animals",
                ArtistName = "Pink Floyd"
            };

            yield return new Album()
            {
                Title = "Greatest Hits",
                ArtistName = "Rod Stewart"
            };

            yield return new Album()
            {
                Title = "Hotel California",
                ArtistName = "The Eagles"
            };

            yield return new Album()
            {
                Title = "Night by Night",
                ArtistName = "Chromeo"
            };

            yield return new Album()
            {
                Title = "Guero",
                ArtistName = "Beck"
            };

            yield return new Album()
            {
                Title = "Deloused in the Comatorium",
                ArtistName = "The Mars Volta"
            };

            yield return new Album()
            {
                Title = "Dark Side of the Moon",
                ArtistName = "Pink Floyd"
            };

            yield return new Album()
            {
                Title = "Spice Girls Greatest Hits",
                ArtistName = "Spice Girls"
            };

            yield return new Album()
            {
                Title = "Abbey Road",
                ArtistName = "The Beatles"
            };

            yield return new Album()
            {
                Title = "Animals",
                ArtistName = "Pink Floyd"
            };

            yield return new Album()
            {
                Title = "Greatest Hits",
                ArtistName = "Rod Stewart"
            };

            yield return new Album()
            {
                Title = "Hotel California",
                ArtistName = "The Eagles"
            };

            yield return new Album()
            {
                Title = "Night by Night",
                ArtistName = "Chromeo"
            };

            yield return new Album()
            {
                Title = "Guero",
                ArtistName = "Beck"
            };

            yield return new Album()
            {
                Title = "Deloused in the Comatorium",
                ArtistName = "The Mars Volta"
            };

            yield return new Album()
            {
                Title = "Dark Side of the Moon",
                ArtistName = "Pink Floyd"
            };

            yield return new Album()
            {
                Title = "Spice Girls Greatest Hits",
                ArtistName = "Spice Girls"
            };

            yield return new Album()
            {
                Title = "Abbey Road",
                ArtistName = "The Beatles"
            };

            yield return new Album()
            {
                Title = "Animals",
                ArtistName = "Pink Floyd"
            };

            yield return new Album()
            {
                Title = "Greatest Hits",
                ArtistName = "Rod Stewart"
            };

            yield return new Album()
            {
                Title = "Hotel California",
                ArtistName = "The Eagles"
            };

            yield return new Album()
            {
                Title = "Night by Night",
                ArtistName = "Chromeo"
            };

            yield return new Album()
            {
                Title = "Guero",
                ArtistName = "Beck"
            };

            yield return new Album()
            {
                Title = "Deloused in the Comatorium",
                ArtistName = "The Mars Volta"
            };

            yield return new Album()
            {
                Title = "Dark Side of the Moon",
                ArtistName = "Pink Floyd"
            };

            yield return new Album()
            {
                Title = "Spice Girls Greatest Hits",
                ArtistName = "Spice Girls"
            };

            yield return new Album()
            {
                Title = "Abbey Road",
                ArtistName = "The Beatles"
            };

            yield return new Album()
            {
                Title = "Animals",
                ArtistName = "Pink Floyd"
            };

            yield return new Album()
            {
                Title = "Greatest Hits",
                ArtistName = "Rod Stewart"
            };

            yield return new Album()
            {
                Title = "Hotel California",
                ArtistName = "The Eagles"
            };

            yield return new Album()
            {
                Title = "Night by Night",
                ArtistName = "Chromeo"
            };

            yield return new Album()
            {
                Title = "Guero",
                ArtistName = "Beck"
            };

            yield return new Album()
            {
                Title = "Deloused in the Comatorium",
                ArtistName = "The Mars Volta"
            };

            yield return new Album()
            {
                Title = "Dark Side of the Moon",
                ArtistName = "Pink Floyd"
            };

            yield return new Album()
            {
                Title = "Spice Girls Greatest Hits",
                ArtistName = "Spice Girls"
            };

            yield return new Album()
            {
                Title = "Abbey Road",
                ArtistName = "The Beatles"
            };

            yield return new Album()
            {
                Title = "Animals",
                ArtistName = "Pink Floyd"
            };

            yield return new Album()
            {
                Title = "Greatest Hits",
                ArtistName = "Rod Stewart"
            };

            yield return new Album()
            {
                Title = "Hotel California",
                ArtistName = "The Eagles"
            };

            yield return new Album()
            {
                Title = "Night by Night",
                ArtistName = "Chromeo"
            };

            yield return new Album()
            {
                Title = "Guero",
                ArtistName = "Beck"
            };

            yield return new Album()
            {
                Title = "Deloused in the Comatorium",
                ArtistName = "The Mars Volta"
            };

            yield return new Album()
            {
                Title = "Dark Side of the Moon",
                ArtistName = "Pink Floyd"
            };

            yield return new Album()
            {
                Title = "Spice Girls Greatest Hits",
                ArtistName = "Spice Girls"
            };

            yield return new Album()
            {
                Title = "Abbey Road",
                ArtistName = "The Beatles"
            };

            yield return new Album()
            {
                Title = "Animals",
                ArtistName = "Pink Floyd"
            };

            yield return new Album()
            {
                Title = "Greatest Hits",
                ArtistName = "Rod Stewart"
            };

            yield return new Album()
            {
                Title = "Hotel California",
                ArtistName = "The Eagles"
            };

            yield return new Album()
            {
                Title = "Night by Night",
                ArtistName = "Chromeo"
            };
        }

        public IEnumerable<Artist> GetArtists()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AlbumArtist> GetAlbumArtists()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Track> GetTracks(Album album)
        {
            throw new NotImplementedException();
        }

        public byte[] GetAlbumArtByTrack(Track track)
        {
            FileInfo albumArtInfo = new FileInfo(@"Images\merdenoms.jpg");
            if (albumArtInfo.Exists)
            {
                using (FileStream artStream = albumArtInfo.OpenRead())
                {
                    byte[] artInfo = new byte[artStream.Length];
                    artStream.Read(artInfo, 0, (int)artStream.Length);
                    return artInfo;
                }
            }

            return null;
        }


        public IEnumerable<Album> GetAlbumsByArtist(Artist artist)
        {
            throw new NotImplementedException();
        }


        public MediaLibraryCapabilities GetCapabilities()
        {
            return new MediaLibraryCapabilities()
            {
                IsNowPlayingListAvailable = true
            };
        }


        public IEnumerable<Track> GetTracksByArtist(Artist artist)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Track> GetTracksByAlbumArtist(AlbumArtist artist)
        {
            throw new NotImplementedException();
        }


        public byte[] GetAlbumArtByAlbum(Album album)
        {
            FileInfo albumArtInfo = new FileInfo(@"Images\merdenoms.jpg");
            if (albumArtInfo.Exists)
            {
                using (FileStream artStream = albumArtInfo.OpenRead())
                {
                    byte[] artInfo = new byte[artStream.Length];
                    artStream.Read(artInfo, 0, (int)artStream.Length);
                    return artInfo;
                }
            }

            return null;
        }
    }
}
