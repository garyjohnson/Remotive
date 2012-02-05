using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MediaControl.Common.Music;
using System.Xml;
using System.Security;

namespace MediaControl.Host.MediaCenter
{
    public class PlaylistService
    {
        private static readonly PlaylistService _current = new PlaylistService();
        public static PlaylistService Current
        {
            get { return _current; }
        }

        private PlaylistService() { }

        private readonly List<FileInfo> _openPlaylists = new List<FileInfo>();

        public void InitializeService() { }
        public void TeardownService() 
        {
            DisposePlaylists();
        }

        private void DisposePlaylists()
        {
            foreach (FileInfo file in _openPlaylists)
            {
                try
                {
                    file.Delete();
                }
                catch (IOException) { }
                catch (SecurityException) { }
                catch (UnauthorizedAccessException) { }
            }

            _openPlaylists.Clear();
        }

        public string CreatePlaylist(string name, IEnumerable<Track> tracks)
        {
            try
            {
                FileInfo tempFile = new FileInfo(string.Format("{0}{1}{2}.wpl", Path.GetTempPath(), Path.DirectorySeparatorChar, name));
                _openPlaylists.Add(tempFile);

                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    OmitXmlDeclaration = true
                };

                using (XmlWriter writer = XmlWriter.Create(tempFile.FullName, settings))
                {
                    // Build the playlist manually.
                    writer.WriteRaw("<?wpl version=\"1.0\"?>");
                    writer.WriteStartElement("smil");
                    writer.WriteStartElement("head");
                    writer.WriteStartElement("meta");
                    writer.WriteAttributeString("name", "Generator");
                    writer.WriteAttributeString("content", "Microsoft Windows Media Player -- 12.0.7600.16415");
                    writer.WriteEndElement();

                    writer.WriteStartElement("meta");
                    writer.WriteAttributeString("name", "ItemCount");
                    writer.WriteAttributeString("content", tracks.Count<Track>().ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("title");
                    writer.WriteString(name);
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteStartElement("body");
                    writer.WriteStartElement("seq");
                    foreach (Track track in tracks)
                    {
                        writer.WriteStartElement("media");
                        writer.WriteAttributeString("src", track.FilePath);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                return tempFile.FullName;
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }
    }
}
