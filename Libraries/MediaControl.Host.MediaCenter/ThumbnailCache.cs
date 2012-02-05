using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaControl.Host.MediaCenter
{
    internal static class ThumbnailCache
    {
        private const int MAX_THUMBNAILS = 100;
        private static readonly Queue<string> _imageQueue = new Queue<string>(MAX_THUMBNAILS);
        private static readonly Dictionary<string, byte[]> _imageDataLookup = new Dictionary<string, byte[]>();
        private static readonly object _syncLock = new object();

        public static void AddThumbnail(string id, byte[] thumbnailData)
        {
            lock (_syncLock)
            {
                if (_imageQueue.Count >= MAX_THUMBNAILS)
                {
                    string dequeuedId = _imageQueue.Dequeue();
                    if (_imageDataLookup.ContainsKey(dequeuedId))
                    {
                        _imageDataLookup.Remove(dequeuedId);
                    }
                }

                _imageQueue.Enqueue(id);
                _imageDataLookup.Add(id, thumbnailData);
            }
        }

        public static bool TryGetThumbnail(string id, out byte[] thumbnailData)
        {
            lock (_syncLock)
            {
                if (_imageDataLookup.ContainsKey(id))
                {
                    thumbnailData = _imageDataLookup[id];
                    return true;
                }
            }

            thumbnailData = null;
            return false;
        }
    }
}
