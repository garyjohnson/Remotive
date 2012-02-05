using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaControl.Common.Music;
using WMPLib;
using System.Globalization;
using Microsoft.MediaCenter.Hosting;
using Microsoft.MediaCenter;
using System.Reflection;

namespace MediaControl.Host.MediaCenter
{
    public static class ExtensionMethods
    {
        public static MediaExperience GetMediaExperience(this AddInHost addInHost)
        {
            if (addInHost != null && addInHost.MediaCenterEnvironment != null)
            {
                MediaExperience experience = addInHost.MediaCenterEnvironment.MediaExperience;
                if (experience == null)
                {
                    Utility.ResetMediaExperienceCache(addInHost.MediaCenterEnvironment);
                    experience = addInHost.MediaCenterEnvironment.MediaExperience;
                }

                return experience;
                
            }

            return null;
        }

        public static Track GetTrack(this IWMPMedia wmpTrack)
        {
            Track track = new Track();
            track.Title = wmpTrack.getItemInfo(MediaAttributes.Title);
            int trackNumber = 0;
            if (int.TryParse(wmpTrack.getItemInfo(MediaAttributes.WMTrackNumber), out trackNumber))
            {
                track.TrackNumber = trackNumber;
            }

            track.FilePath = wmpTrack.sourceURL;
            track.AlbumName = wmpTrack.getItemInfo(MediaAttributes.WMAlbumTitle);
            track.ArtistName = wmpTrack.getItemInfo(MediaAttributes.Artist);
            double secondsDuration = 0.0d;
            if (double.TryParse(wmpTrack.getItemInfo(MediaAttributes.Duration), out secondsDuration))
            {
                track.Duration = TimeSpan.FromSeconds(secondsDuration);
            }

            return track;
        }

        public static Album GetAlbum(this IWMPMedia wmpTrack)
        {
            Album album = new Album();
            album.Title = wmpTrack.getItemInfo(MediaAttributes.WMAlbumTitle);
            album.ArtistName = wmpTrack.getItemInfo(MediaAttributes.WMAlbumArtist);
            album.Genre = wmpTrack.getItemInfo(MediaAttributes.WMGenre);
            album.ID = wmpTrack.getItemInfo(MediaAttributes.AlbumID);

            string releaseYear = wmpTrack.getItemInfo(MediaAttributes.ReleaseDateYear);
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
