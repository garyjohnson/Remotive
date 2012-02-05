using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using Id3Tag;
using Id3Tag.HighLevel;
using Id3Tag.HighLevel.Id3Frame;
using MediaControl.Common;
using MediaControl.Common.Music;
using WMPLib;
using System.Threading;
using System.Reflection;

namespace MediaControl.Host.MediaCenter
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class MediaLibraryService : IMediaLibraryService
    {
        public MediaLibraryService()
        {
        }

        public static void CacheMediaLibrary()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((state)=>
                {
                    List<Album> returnedAlbums = new List<Album>();
                    WindowsMediaPlayer mediaPlayer = null;

                    try
                    {
                        //LogUtility.LogMessage("Started caching");

                        mediaPlayer = new WindowsMediaPlayer();
                        CacheAlbums(mediaPlayer);
                        CacheArtists(mediaPlayer);
                        CacheAlbumArtists(mediaPlayer);

                        //LogUtility.LogMessage("Finished caching");
                    }
                    catch (Exception ex)
                    {
                        LogUtility.LogException(ex);
                        throw;
                    }
                    finally
                    {
                        if (mediaPlayer != null)
                        {
                            mediaPlayer.close();
                        }
                    }
                }));
        }

        private static void CacheAlbums(WindowsMediaPlayer mediaPlayer)
        {
                // Get the album titles
                IWMPStringCollection albumTitles = mediaPlayer.mediaCollection
                    .getAttributeStringCollection(MediaAttributes.WMAlbumTitle, MediaType.Audio);

                for (int i = 0; i < albumTitles.count; i++)
                {
                    if (!_albumCache.ContainsKey(albumTitles.Item(i).ToLower()))
                    {
                        string albumTitle = albumTitles.Item(i);
                        if (albumTitle != null && !string.IsNullOrEmpty(albumTitle))
                        {
                            IWMPPlaylist albumPlaylist = mediaPlayer.mediaCollection.getByAlbum(albumTitle);

                            Album album = new Album();
                            album.Title = albumTitle;

                            if (albumPlaylist.count >= 1)
                            {
                                IWMPMedia albumMedia = albumPlaylist.get_Item(0);
                                album.ArtistName = albumMedia.getItemInfo(MediaAttributes.WMAlbumArtist);
                                album.Genre = albumMedia.getItemInfo(MediaAttributes.WMGenre);
                                album.ID = albumMedia.getItemInfo(MediaAttributes.AlbumID);

                                string releaseYear = albumMedia.getItemInfo(MediaAttributes.ReleaseDateYear);
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
                            }

                            _albumCache.Add(album.Title.ToLower(), album);
                        }
                    }
                }
        }

        private static void CacheArtists(WindowsMediaPlayer mediaPlayer)
        {
                // Get the album titles
                IWMPStringCollection artistNames = mediaPlayer.mediaCollection
                    .getAttributeStringCollection(MediaAttributes.DisplayArtist, MediaType.Audio);

                for (int i = 0; i < artistNames.count; i++)
                {
                    if (!_artistCache.ContainsKey(artistNames.Item(i).ToLower()))
                    {
                        string artistName = artistNames.Item(i);
                        if (artistName != null && !string.IsNullOrEmpty(artistName))
                        {
                            Artist artist = new Artist()
                            {
                                Name = artistName
                            };

                            _artistCache.Add(artist.Name.ToLower(), artist);
                        }
                    }
                }
        }

        private static void CacheAlbumArtists(WindowsMediaPlayer mediaPlayer)
        {
            // Get the album artists
            IWMPStringCollection artistNames = mediaPlayer.mediaCollection
                .getAttributeStringCollection(MediaAttributes.WMAlbumArtist, MediaType.Audio);

            for (int i = 0; i < artistNames.count; i++)
            {
                if (!_albumArtistCache.ContainsKey(artistNames.Item(i).ToLower()))
                {
                    string artistName = artistNames.Item(i);
                    if (artistName != null && !string.IsNullOrEmpty(artistName))
                    {
                        AlbumArtist artist = new AlbumArtist()
                        {
                            Name = artistName,
                            Albums = getAlbumsByArtist(mediaPlayer, artistNames.Item(i), true)
                        };

                        _albumArtistCache.Add(artist.Name.ToLower(), artist);
                    }
                }
            }
        }

        //private bool _isCached = false;
        private static readonly Dictionary<string, AlbumArtist> _albumArtistCache = new Dictionary<string, AlbumArtist>();
        private static readonly Dictionary<string, Album> _albumCache = new Dictionary<string, Album>();
        private static readonly Dictionary<string, Artist> _artistCache = new Dictionary<string, Artist>();

        #region Public Methods

        public System.Version GetServerVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

        public IEnumerable<Album> GetAlbums()
        {
            try
            {
                return _albumCache.Values.ToList<Album>();
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
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
            return getAlbumsByArtist(artist.Name, false);
        }

        public byte[] GetAlbumArtByAlbum(Album album)
        {
            WindowsMediaPlayer mediaPlayer = null;
            byte[] imageData = null;

            try
            {
                mediaPlayer = new WindowsMediaPlayer();
                IWMPPlaylist foundTracks = mediaPlayer.mediaCollection.getByAttribute(MediaAttributes.AlbumID, album.ID);
                string collectionId = string.Empty;
                string albumId = string.Empty;

                if (foundTracks.count == 0)
                {
                    return null;
                }

                IWMPMedia foundTrack = foundTracks.get_Item(0);
                collectionId = foundTrack.getItemInfo(MediaAttributes.WMWMCollectionID);
                albumId = foundTrack.getItemInfo(MediaAttributes.AlbumID);

                if (!ThumbnailCache.TryGetThumbnail(albumId, out imageData))
                {
                    imageData = GetTrackArt(mediaPlayer, foundTrack, collectionId);
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
                if (mediaPlayer != null)
                {
                    mediaPlayer.close();
                }
            }
        }

        public byte[] GetAlbumArtByTrack(Track track)
        {
            WindowsMediaPlayer mediaPlayer = null;
            byte[] imageData = null;

            try
            {
                mediaPlayer = new WindowsMediaPlayer();
                IWMPPlaylist foundTracks = mediaPlayer.mediaCollection.getByName(track.Title);
                string collectionId = string.Empty;
                string albumId = string.Empty;

                //LogUtility.LogMessage("Found " + foundTracks.count + " tracks matching title " + track.Title);
                for (int i = 0; i < foundTracks.count; i++)
                {
                    IWMPMedia foundTrack = foundTracks.get_Item(i);
                    if (track.FilePath == foundTrack.sourceURL)
                    {
                        //LogUtility.LogMessage("Found a track with sourceURL matching filepath");
                        collectionId = foundTrack.getItemInfo(MediaAttributes.WMWMCollectionID);
                        albumId = foundTrack.getItemInfo(MediaAttributes.AlbumID);

                        if (!ThumbnailCache.TryGetThumbnail(albumId, out imageData))
                        {
                            imageData = GetTrackArt(mediaPlayer, foundTrack, collectionId);
                            if (imageData != null)
                            {
                                break;
                            }
                        }
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
                if (mediaPlayer != null)
                {
                    mediaPlayer.close();
                }
            }
        }

        private byte[] GetTrackArt(WindowsMediaPlayer player, IWMPMedia track, string collectionId)
        {
            byte[] imageData = null;
            if (!string.IsNullOrEmpty(track.sourceURL) && File.Exists(track.sourceURL))
            {
                // Try and find the album art that WMP, WMC, Zune creates. I'd much rather read from
                // a local JPEG than try to read from media that's currently playing and probably locked
                FileInfo pathInfo = new FileInfo(track.sourceURL);
                //LogUtility.LogMessage(pathInfo.FullName + " exists");
                FileInfo albumArtInfo = new FileInfo(string.Format("{0}{1}AlbumArt_{2}_Large.jpg",
                    pathInfo.DirectoryName, Path.DirectorySeparatorChar, collectionId));

                if (!albumArtInfo.Exists)
                {
                    albumArtInfo = new FileInfo(string.Format("{0}{1}AlbumArt_{2}_Small.jpg",
                       pathInfo.DirectoryName, Path.DirectorySeparatorChar, collectionId));
                }

                if (!albumArtInfo.Exists)
                {
                    albumArtInfo = new FileInfo(string.Format("{0}{1}ZuneAlbumArt.jpg",
                       pathInfo.DirectoryName, Path.DirectorySeparatorChar));
                }

                //LogUtility.LogMessage("Album art path is " + albumArtInfo.FullName);
                if (albumArtInfo.Exists)
                {
                    //LogUtility.LogMessage("Album art exists");
                    using (FileStream artStream = albumArtInfo.OpenRead())
                    {
                        byte[] artInfo = new byte[artStream.Length];
                        artStream.Read(artInfo, 0, (int)artStream.Length);
                        imageData = artInfo;
                    }
                }
            }

            // Couldn't get the art from the folder.jpg, so now try to read the id3 tag
            if (File.Exists(track.sourceURL))
            {
                using (FileStream fileStream = new FileStream(track.sourceURL, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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

        public IEnumerable<Artist> GetArtists()
        {
            try
            {
                return _artistCache.Values.ToList<Artist>();
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public IEnumerable<Track> GetTracks(Album album)
        {
            List<Track> returnedTracks = new List<Track>();
            WindowsMediaPlayer mediaPlayer = null;

            try
            {
                mediaPlayer = new WindowsMediaPlayer();

                IWMPPlaylist albumTracks = mediaPlayer.mediaCollection.getByAttribute(MediaAttributes.AlbumID, album.ID);

                for (int i = 0; i < albumTracks.count; i++)
                {
                    IWMPMedia trackMedia = albumTracks.get_Item(i);
                    returnedTracks.Add(trackMedia.GetTrack());
                }

                return returnedTracks;
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
            finally
            {
                if (mediaPlayer != null)
                {
                    mediaPlayer.close();
                }
            }
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

        #endregion Public Methods

        #region Virtualizing Methods

        public int GetAlbumArtistCount()
        {
            try
            {
                return _albumArtistCache.Count;
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public IEnumerable<AlbumArtist> GetAlbumArtistRange(int index, int count)
        {
            try
            {
                List<AlbumArtist> artists = _albumArtistCache.Values.ToList<AlbumArtist>();
                return artists.GetRange(index, count);
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public int GetArtistCount()
        {
            try
            {
                return _artistCache.Count;
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public IEnumerable<Artist> GetArtistRange(int index, int count)
        {
            try
            {
                List<Artist> artists = _artistCache.Values.ToList<Artist>();
                return artists.GetRange(index, count);
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public int GetAlbumCount()
        {
            try
            {
                return _albumCache.Count;
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public IEnumerable<Album> GetAlbumRange(int index, int count)
        {
            try
            {
                List<Album> albums = _albumCache.Values.ToList<Album>();
                return albums.GetRange(index, count);
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        #endregion Virtualizing Methods

        #region Private Methods

        private IEnumerable<Album> getAlbumsByArtist(string artistName, bool isAlbumArtist)
        {
            WindowsMediaPlayer mediaPlayer = null;
            
            try
            {
                mediaPlayer = new WindowsMediaPlayer();
                return getAlbumsByArtist(mediaPlayer, artistName, isAlbumArtist);
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
            finally
            {
                if (mediaPlayer != null)
                {
                    mediaPlayer.close();
                }
            }
        }

        private static IEnumerable<Album> getAlbumsByArtist(WindowsMediaPlayer mediaPlayer, string artistName, bool isAlbumArtist)
        {
            //LogUtility.LogMessage("Starting getAlbumsByArtist");
            Dictionary<string, Album> returnedAlbums = new Dictionary<string, Album>();
            IWMPPlaylist trackPlaylist = null;

            //LogUtility.LogMessage("Starting getByAttribute");
            if (isAlbumArtist)
            {
                trackPlaylist = mediaPlayer.mediaCollection.getByAttribute(MediaAttributes.WMAlbumArtist, artistName);
                    
            }
            else
            {
                trackPlaylist = mediaPlayer.mediaCollection.getByAttribute(MediaAttributes.DisplayArtist, artistName);
            }
            //LogUtility.LogMessage("Ending getByAttribute");
            for (int i = 0; i < trackPlaylist.count; i++)
            {

                //LogUtility.LogMessage("Starting get_Item");
                IWMPMedia track = trackPlaylist.get_Item(i);

                //LogUtility.LogMessage("Ending get_Item");
                //LogUtility.LogMessage("Starting getItemInfo");
                string albumId = track.getItemInfo(MediaAttributes.AlbumID);
                //LogUtility.LogMessage("Ending getItemInfo");


                //LogUtility.LogMessage("Starting add and GetAlbum()");
                if (!returnedAlbums.ContainsKey(albumId))
                {
                    returnedAlbums.Add(albumId, track.GetAlbum());
                }
                //LogUtility.LogMessage("Ending add and GetAlbum()");
            }

            //LogUtility.LogMessage("Ending getAlbumsByArtist");
            return returnedAlbums.Values.ToList<Album>();
        }

        private IEnumerable<Track> getTracksByArtist(string artistName, bool isAlbumArtist)
        {
            List<Track> returnedTracks = new List<Track>();
            WindowsMediaPlayer mediaPlayer = null;

            try
            {
                mediaPlayer = new WindowsMediaPlayer();

                IWMPPlaylist trackPlaylist = null;
                if (isAlbumArtist)
                {
                    trackPlaylist = mediaPlayer.mediaCollection.getByAttribute(MediaAttributes.WMAlbumArtist, artistName);
                }
                else
                {
                    trackPlaylist = mediaPlayer.mediaCollection.getByAttribute(MediaAttributes.DisplayArtist, artistName);
                }

                for (int i = 0; i < trackPlaylist.count; i++)
                {
                    IWMPMedia track = trackPlaylist.get_Item(i);
                    returnedTracks.Add(track.GetTrack());
                }

                return returnedTracks;
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
            finally
            {
                if (mediaPlayer != null)
                {
                    mediaPlayer.close();
                }
            }
        }

        #endregion Private Methods
    }
}
