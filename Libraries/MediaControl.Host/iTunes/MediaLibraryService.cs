using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaControl.Common;
using System.ServiceModel;
using System.IO;
using System.Globalization;
using System.ServiceModel.Description;
using MediaControl.Common.Music;
using Id3Tag.HighLevel;
using Id3Tag.HighLevel.Id3Frame;
using Id3Tag;
using System.Windows.Media.Imaging;
using iTunesLib;

namespace MediaControl.Host.iTunes
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class MediaLibraryService : IMediaLibraryService
    {
        public MediaLibraryService() { }

        public IEnumerable<Album> GetAlbums()
        {
            List<Album> returnedAlbums = new List<Album>();
            List<string> albumNames = new List<string>();
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                foreach (IITFileOrCDTrack track in iTunes.LibraryPlaylist.Tracks)
                {
                    if (!albumNames.Contains(track.Album))
                    {
                        Album album = track.GetAlbum();
                        albumNames.Add(album.Title);
                        returnedAlbums.Add(album);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
            finally
            {
                // Marshall.ReleaseComObject?
            }

            return returnedAlbums;
        }

        public IEnumerable<Track> GetTracksByArtist(Artist artist)
        {
            return getTracksByArtist(artist.Name, false);
        }

        public IEnumerable<Track> GetTracksByAlbumArtist(AlbumArtist artist)
        {
            return getTracksByArtist(artist.Name, true);
        }

        public IEnumerable<Album> GetAlbumsByArtist(Artist artist)
        {
            return getAlbumsByArtist(artist, false);
        }

        private IEnumerable<Album> getAlbumsByArtist(Artist artist, bool isAlbumArtist)
        {
            List<Album> returnedAlbums = new List<Album>();
            List<string> albumNames = new List<string>();
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();

                if(isAlbumArtist)
                {
                    foreach (IITFileOrCDTrack track in iTunes.LibraryPlaylist.Tracks)
                    {
                        // If the artist doesn't match, or if we already have the album,
                        // just keep going
                        if (!string.Equals(track.AlbumArtist, artist.Name) || albumNames.Contains(track.Album))
                        {
                            continue;
                        }

                        Album album = track.GetAlbum();
                        albumNames.Add(album.Title);
                        returnedAlbums.Add(album);
                    }
                }
                else
                {
                    foreach(IITFileOrCDTrack track in iTunes.LibraryPlaylist.Search(artist.Name, ITPlaylistSearchField.ITPlaylistSearchFieldArtists))
                    {
                        if (!string.Equals(track.Artist, artist.Name) || albumNames.Contains(track.Album))
                        {
                            continue;
                        }

                        Album album = track.GetAlbum();
                        albumNames.Add(album.Title);
                        returnedAlbums.Add(album);
                    }
                }                
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
            finally
            {
                // Marshall.ReleaseComObject?
            }

            return returnedAlbums;
        }

        private IEnumerable<Track> getTracksByArtist(string artistName, bool isAlbumArtist)
        {
            List<Track> returnedTracks = new List<Track>();
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();

                if (isAlbumArtist)
                {
                    foreach (IITFileOrCDTrack track in iTunes.LibraryPlaylist.Tracks)
                    {
                        // If the artist doesn't match, or if we already have the album,
                        // just keep going
                        if (!string.Equals(track.AlbumArtist, artistName))
                        {
                            continue;
                        }

                        returnedTracks.Add(track.GetTrack());
                    }
                }
                else
                {
                    foreach (IITFileOrCDTrack track in iTunes.LibraryPlaylist.Search(artistName, ITPlaylistSearchField.ITPlaylistSearchFieldArtists))
                    {
                        if (!string.Equals(track.Artist, artistName))
                        {
                            continue;
                        }

                        returnedTracks.Add(track.GetTrack());
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
            finally
            {
                // Marshall.ReleaseComObject?
            }

            return returnedTracks;
        }

        public byte[] GetAlbumArtByAlbum(Album album)
        {
            byte[] imageData = null;
            Track foundTrack = null;
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();

                foreach (IITFileOrCDTrack track in iTunes.LibraryPlaylist.Search(album.Title,
                    ITPlaylistSearchField.ITPlaylistSearchFieldAlbums))
                {
                    if (!string.Equals(track.Artist, album.ArtistName))
                    {
                        continue;
                    }

                    foundTrack = track.GetTrack();
                    break;
                }

                if (foundTrack != null && File.Exists(foundTrack.FilePath))
                {
                    using (FileStream fileStream = new FileStream(foundTrack.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        try
                        {
                            TagContainer tags = new Id3TagManager().ReadV2Tag(fileStream);
                            if (tags != null)
                            {
                                IFrame apicFrame = tags.SearchFrame("APIC");
                                if (apicFrame != null)
                                {
                                    PictureFrame pictureFrame = FrameUtilities.ConvertToPictureFrame(apicFrame);
                                    imageData = pictureFrame.PictureData.ToArray<byte>();
                                }
                            }
                        }
                        catch (Id3TagException) { /* Do nothing */}
                    }
                }

                return imageData;
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
            finally
            {
                // Marshall.ReleaseComObject?
            }
        }

        public byte[] GetAlbumArtByTrack(Track track)
        {
            byte[] imageData = null;
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();

                if (track != null && File.Exists(track.FilePath))
                {
                    using (FileStream fileStream = new FileStream(track.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        try
                        {
                            TagContainer tags = new Id3TagManager().ReadV2Tag(fileStream);
                            if (tags != null)
                            {
                                IFrame apicFrame = tags.SearchFrame("APIC");
                                if (apicFrame != null)
                                {
                                    PictureFrame pictureFrame = FrameUtilities.ConvertToPictureFrame(apicFrame);
                                    imageData = pictureFrame.PictureData.ToArray<byte>();
                                }
                            }
                        }
                        catch (Id3TagException) { /* Do nothing */}
                    }
                }

                return imageData;
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
            finally
            {
                // Marshall.ReleaseComObject?
            }
        }

        public IEnumerable<Artist> GetArtists()
        {
            List<Artist> returnedArtists = new List<Artist>();
            List<string> artistNames = new List<string>();
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                foreach (IITFileOrCDTrack track in iTunes.LibraryPlaylist.Tracks)
                {
                    if (!artistNames.Contains(track.Artist))
                    {
                        artistNames.Add(track.Artist);
                        returnedArtists.Add(track.GetArtist());
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
            finally
            {
                // Marshall.ReleaseComObject?
            }

            return returnedArtists;
        }

        public IEnumerable<AlbumArtist> GetAlbumArtists()
        {
            List<AlbumArtist> returnedArtists = new List<AlbumArtist>();
            List<string> artistNames = new List<string>();
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                foreach (IITFileOrCDTrack track in iTunes.LibraryPlaylist.Tracks)
                {
                    if (!artistNames.Contains(track.Artist))
                    {
                        artistNames.Add(track.Artist);
                        returnedArtists.Add(track.GetAlbumArtist());
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
            finally
            {
                // Marshall.ReleaseComObject?
            }

            return returnedArtists;
        }

        public IEnumerable<Track> GetTracks(Album album)
        {
            List<Track> returnedTracks = new List<Track>();
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                
                foreach (IITFileOrCDTrack track in iTunes.LibraryPlaylist.Search(album.Title, ITPlaylistSearchField.ITPlaylistSearchFieldArtists))
                {
                    if (!string.Equals(track.AlbumArtist, album.ArtistName))
                    {
                        continue;
                    }

                    returnedTracks.Add(track.GetTrack());
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
            finally
            {
                // Marshall.ReleaseComObject?
            }

            return returnedTracks;
        }

        public MediaLibraryCapabilities GetCapabilities()
        {
            return new MediaLibraryCapabilities()
            {
                IsNowPlayingListAvailable = false
            };
        }

        public void Ping()
        {
            // Do nothing
        }


        public int InitializeAlbumArtistList()
        {
            throw new NotImplementedException();
        }

        public void TeardownAlbumArtistList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AlbumArtist> GetAlbumArtistRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AlbumArtist> GetAlbumArtistRange(Guid requestId, int index, int count)
        {
            throw new NotImplementedException();
        }


        public int GetAlbumArtistCount()
        {
            throw new NotImplementedException();
        }

        public int GetArtistCount()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Artist> GetArtistRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public int GetAlbumCount()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Album> GetAlbumRange(int index, int count)
        {
            throw new NotImplementedException();
        }


        public System.Version GetServerVersion()
        {
            throw new NotImplementedException();
        }
    }
}
