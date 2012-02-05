using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.IO;
using Sharkfist.Phone.SilverlightCore;
using System.Threading;
using System.Diagnostics;
using MediaControl.Client.WindowsPhone.Services;

namespace MediaControl.Client.WindowsPhone
{
    public class AlbumArtCache : IStateHandler
    {
        private const int MAX_IMAGE_COUNT = 50;
        private readonly object _syncLock = new object();
        private Dictionary<string, BitmapImage> _albumArtCache = new Dictionary<string, BitmapImage>();
        private Dictionary<string, int> _albumArtAge = new Dictionary<string, int>();

        private readonly object _requestLock = new object();
        private readonly List<MediaLibrary.Album> _queuedRequests = new List<MediaLibrary.Album>();

        private Timer _requestTimer;

        private static readonly AlbumArtCache _instance = new AlbumArtCache();
        public static AlbumArtCache Instance
        {
            get { return _instance; }
        }

        private AlbumArtCache()
        {
            _requestTimer = new Timer(RequestTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
        }

        private void StartTimer(int delay, int period)
        {
            _requestTimer.Change(delay, period);
        }

        private void StopTimer()
        {
            _requestTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void RequestTimerElapsed(object state)
        {
            StopTimer();

            if (_queuedRequests.Count > 0)
            {
                lock (_requestLock)
                {
                    if (_queuedRequests.Count > 0)
                    {
                        // Execute a request here
                        // when done, start the timer again
                        MediaLibrary.Album request = _queuedRequests[0];
                        _queuedRequests.RemoveAt(0);
                        DownloadAlbumArt(request);
                    }
                }
            }
        }

        public void GetAlbumArtAsync(MediaLibrary.Album album)
        {
            if (string.IsNullOrEmpty(album.ID))
            {
                return;
            }

            SetAlbumArtAge(album);

            if (_albumArtCache.ContainsKey(album.ID))
            {
                FireAlbumArtDownloaded(album, _albumArtCache[album.ID]);
            }
            else
            {
                lock (_requestLock)
                {
                    _queuedRequests.Add(album);
                    StartTimer(0, 300);
                }
            }
        }

        public void CancelRequest(MediaLibrary.Album album)
        {
            if (_queuedRequests.Contains(album))
            {
                lock (_requestLock)
                {
                    if (_queuedRequests.Contains(album))
                    {
                        Debug.WriteLine("Cancelled request for {0}", album.ID);
                        _queuedRequests.Remove(album);
                    }
                }
            }
        }

        private void DownloadAlbumArt(MediaLibrary.Album album)
        {
            App.GetService<IRequestService>().GetAlbumArtByAlbumAsync(album,
                    (innerAlbum, imageData, state, error) =>
                    {
                        BitmapImage albumArt = null;
                        if (error == null && imageData != null && imageData.Length > 0)
                        {
                            albumArt = new BitmapImage();
                            if (imageData != null)
                            {
                                try
                                {
                                    using (MemoryStream artStream = new MemoryStream(imageData))
                                    {
                                        albumArt.SetSource(artStream);
                                    }
                                }
                                catch (Exception)
                                {
                                    albumArt = null;
                                }
                            }
                        }

                        lock (_syncLock)
                        {
                            if (_albumArtAge.ContainsKey(innerAlbum.ID))
                            {
                                if (_albumArtCache.ContainsKey(innerAlbum.ID))
                                {
                                    _albumArtCache[innerAlbum.ID] = albumArt;
                                }
                                else
                                {
                                    _albumArtCache.Add(innerAlbum.ID, albumArt);
                                }
                            }


                            FireAlbumArtDownloaded(innerAlbum, _albumArtCache[innerAlbum.ID]);
                        }

                        // Now, start the timer up again
                        StartTimer(0, 0);
                    }, null);
        }

        private void TrimExcessAlbumArt()
        {
            lock (_syncLock)
            {
                if (_albumArtAge.Keys.Count > MAX_IMAGE_COUNT)
                {
                    int excessCount = _albumArtAge.Keys.Count - MAX_IMAGE_COUNT;

                    var albumIds = (from a in _albumArtAge
                                    orderby a.Value descending
                                    select a.Key).Take(excessCount);

                    foreach (string albumId in albumIds)
                    {
                        _albumArtAge.Remove(albumId);
                        if (_albumArtCache.ContainsKey(albumId))
                        {
                            _albumArtCache.Remove(albumId);
                            //System.Diagnostics.Debug.WriteLine("Removed {0}", albumId);
                        }
                    }
                }
            }
        }

        private void SetAlbumArtAge(MediaLibrary.Album album)
        {
            lock (_syncLock)
            {
                bool isCurrentArtYoungest = false;
                if (_albumArtAge.ContainsKey(album.ID))
                {
                    isCurrentArtYoungest = (_albumArtAge[album.ID] == 0);
                    if (!isCurrentArtYoungest)
                    {
                        _albumArtAge[album.ID] = 0;
                    }
                }
                else
                {
                    _albumArtAge.Add(album.ID, 0);
                }

                List<string> keys = new List<string>();
                if (!isCurrentArtYoungest)
                {
                    foreach (string key in _albumArtAge.Keys)
                    {
                        keys.Add(key);

                    }

                    foreach (string key in keys)
                    {
                        if (string.Equals(key, album.ID))
                        {
                            _albumArtAge[key] = 0;
                        }
                        else
                        {
                            _albumArtAge[key]++;
                        }
                    }
                }
            }

            TrimExcessAlbumArt();
        }

        private void FireAlbumArtDownloaded(MediaLibrary.Album album, BitmapImage albumArt)
        {
            if (_albumArtDownloaded != null)
            {
                _albumArtDownloaded(this, new EventArgs<MediaLibrary.Album, BitmapImage>(album, albumArt));
            }
        }

        private event EventHandler<EventArgs<MediaLibrary.Album, BitmapImage>> _albumArtDownloaded;
        public event EventHandler<EventArgs<MediaLibrary.Album, BitmapImage>> AlbumArtDownloaded
        {
            add { _albumArtDownloaded += value.MakeWeak<EventArgs<MediaLibrary.Album, BitmapImage>>(eh => _albumArtDownloaded -= eh); }
            remove { }
        }

        public void Launching(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
        }

        public void Activated(object sender, ActivatedDataEventArgs e)
        {
        }

        public void Deactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            Configuration.SaveSetting<Dictionary<string, BitmapImage>>("_albumArtCache", _albumArtCache);
            Configuration.SaveSetting<Dictionary<string, int>>("_albumArtAge", _albumArtAge);
        }

        public void Closing(object sender, Microsoft.Phone.Shell.ClosingEventArgs e)
        {
            Dictionary<string, BitmapImage> albumArtCache = null;
            Dictionary<string, int> albumArtAge = null;
            if (Configuration.TryLoadSetting<Dictionary<string, BitmapImage>>("_albumArtCache", out albumArtCache))
            {
                if (Configuration.TryLoadSetting<Dictionary<string, int>>("_albumArtAge", out albumArtAge))
                {
                    lock (_syncLock)
                    {
                        _albumArtAge = albumArtAge;
                        _albumArtCache = albumArtCache;
                    }
                }
            }
        }
    }
}