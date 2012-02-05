using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaControl.Common;
using System.ServiceModel;
using Microsoft.MediaCenter.Hosting;
using Microsoft.MediaCenter;
using WMPLib;
using MediaControl.Common.Music;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace MediaControl.Host.MediaCenter
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class MediaPlaybackService : IMediaPlaybackService
    {
        public MediaPlaybackService() { }

        public MediaState Play()
        {
            try
            {
                MediaExperience experience = MyAddIn.MediaCenterHost.GetMediaExperience();
                if (experience != null && experience.Transport != null)
                {
                    experience.Transport.PlayRate = PlayRate.Play;
                }
                return GetMediaState();
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public MediaState Pause()
        {
            try
            {
                MediaExperience experience = MyAddIn.MediaCenterHost.GetMediaExperience();
                if (experience != null && experience.Transport != null)
                {
                    experience.Transport.PlayRate = PlayRate.Pause;
                }
                return GetMediaState();
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public MediaState NextTrack()
        {
            try
            {
                MediaExperience experience = MyAddIn.MediaCenterHost.GetMediaExperience();
                if (experience != null && experience.Transport != null)
                {
                    experience.Transport.SkipForward();
                }
                return GetMediaState();
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public MediaState PrevTrack()
        {
            try
            {
                MediaExperience experience = MyAddIn.MediaCenterHost.GetMediaExperience();
                if (experience != null && experience.Transport != null)
                {
                    experience.Transport.SkipBack();
                }
                return GetMediaState();
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public MediaState PlayTrack(Common.Music.Track track)
        {
            try
            {
                MyAddIn.MediaCenterHost.MediaCenterEnvironment.PlayMedia(Microsoft.MediaCenter.MediaType.Audio,
                    track.FilePath, false);
                return GetMediaState();
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public MediaState GetMediaState()
        {
            try
            {
                return new MediaState()
                {
                    CurrentTrack = GetNowPlaying(),
                    PlaybackState = GetPlaybackState(),
                    PlaybackPosition = GetPlaybackPosition(),
                    CurrentTime = DateTime.Now,
                    Volume = GetVolumePercent()
                };
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        private double GetVolumePercent()
        {
            return CoreAudioApi.Audio.MasterVolume;
        }

        //private const int VOLUME_MAX = 65535;
        //private double GetVolumePercent()
        //{
        //    MediaCenterEnvironment environment = MyAddIn.MediaCenterHost.MediaCenterEnvironment;
        //    if (environment != null)
        //    {
        //        AudioMixer mixer = environment.AudioMixer;
        //        if (mixer != null)
        //        {
        //            double volume = (double)mixer.Volume / (double)VOLUME_MAX;
        //            return volume;
        //        }
        //    }

        //    return 0.0d;
        //}

        private TimeSpan GetPlaybackPosition()
        {
            MediaExperience experience = MyAddIn.MediaCenterHost.GetMediaExperience();

            //if (experience == null ||
            //     experience.Transport == null ||
            //    (int)experience.MediaType != (int)Microsoft.MediaCenter.MediaType.Audio)

            if (experience == null ||
                experience.Transport == null)
            {
                return TimeSpan.MinValue;
            }

            return experience.Transport.Position;
        }

        private Track GetNowPlaying()
        {
            MediaExperience experience = MyAddIn.MediaCenterHost.GetMediaExperience();

            //if (experience == null ||
            //     experience.Transport == null ||
            //    (int)experience.MediaType != (int)Microsoft.MediaCenter.MediaType.Audio)

            if (experience == null ||
                experience.Transport == null)
            {
                return null;
            }

            MediaMetadata nowPlayingData = experience.MediaMetadata;
            Track nowPlaying = new Track()
            {
                ArtistName = nowPlayingData.ContainsKey("TrackArtist") ? nowPlayingData["TrackArtist"].ToString() : string.Empty,
                FilePath = nowPlayingData.ContainsKey("Uri") ? nowPlayingData["Uri"].ToString() : string.Empty,
                Title = nowPlayingData.ContainsKey("TrackTitle") ? nowPlayingData["TrackTitle"].ToString() : string.Empty,
                AlbumName = nowPlayingData.ContainsKey("AlbumTitle") ? nowPlayingData["AlbumTitle"].ToString() : string.Empty,
            };

            int trackDuration = 0;
            if (nowPlayingData.ContainsKey("TrackDuration"))
            {
                int.TryParse(nowPlayingData["TrackDuration"].ToString(), out trackDuration);
            }
            nowPlaying.Duration = new TimeSpan(0, 0, trackDuration);

            int trackNumber = 0;
            if (nowPlayingData.ContainsKey("TrackNumber"))
            {
                int.TryParse(nowPlayingData["TrackNumber"].ToString(), out trackNumber);
            }
            nowPlaying.TrackNumber = trackNumber;

            return nowPlaying;
        }

        private Common.Music.PlaybackState GetPlaybackState()
        {
            MediaExperience experience = MyAddIn.MediaCenterHost.GetMediaExperience();

            //if (experience == null ||
            //     experience.Transport == null ||
            //    (int)experience.MediaType != (int)Microsoft.MediaCenter.MediaType.Audio)

            if (experience == null ||
                experience.Transport == null)
            {
                return PlaybackState.Unknown;
            }

            MediaTransport transport = experience.Transport;
            return GetPlaybackStateFromPlayRate(transport.PlayRate);
        }

        private PlaybackState GetPlaybackStateFromPlayRate(float playRate)
        {
            if (playRate == PlayRate.FastFoward1)
            {
                return PlaybackState.FastFoward;
            }
            else if (playRate == PlayRate.FastFoward2)
            {
                return PlaybackState.FastFoward2;
            }
            else if (playRate == PlayRate.FastFoward3)
            {
                return PlaybackState.FastFoward3;
            }
            else if (playRate == PlayRate.Pause)
            {
                return PlaybackState.Pause;
            }
            else if (playRate == PlayRate.Play)
            {
                return PlaybackState.Play;
            }
            else if (playRate == PlayRate.Rewind1)
            {
                return PlaybackState.Rewind;
            }
            else if (playRate == PlayRate.Rewind2)
            {
                return PlaybackState.Rewind2;
            }
            else if (playRate == PlayRate.Rewind3)
            {
                return PlaybackState.Rewind3;
            }
            else if (playRate == PlayRate.SlowMotion1)
            {
                return PlaybackState.SlowMotion;
            }
            else if (playRate == PlayRate.SlowMotion2)
            {
                return PlaybackState.SlowMotion2;
            }
            else if (playRate == PlayRate.SlowMotion3)
            {
                return PlaybackState.SlowMotion3;
            }
            else if (playRate == PlayRate.Stop)
            {
                return PlaybackState.Stop;
            }

            return PlaybackState.Unknown;
        }

        private void LoadPlaylist(IEnumerable<Track> trackQueue)
        {
            try
            {
                if (MyAddIn.MediaCenterHost.MediaCenterEnvironment == null)
                {
                    return;
                }

                string playlistPath = PlaylistService.Current.CreatePlaylist(Guid.NewGuid().ToString(), trackQueue);
                MyAddIn.MediaCenterHost.MediaCenterEnvironment.PlayMedia(Microsoft.MediaCenter.MediaType.Audio,
                        playlistPath, false);
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public MediaState EnqueueTracks(IEnumerable<Track> trackQueue, Track playTrack)
        {
            // NOTE: In media center we will ignore playtrack since we can't do anything with it.
            // The media center api sucks so hard.
            try
            {
                if (MyAddIn.MediaCenterHost.MediaCenterEnvironment == null)
                {
                    return null;
                }

                foreach (Track track in trackQueue)
                {
                    MyAddIn.MediaCenterHost.MediaCenterEnvironment.PlayMedia(Microsoft.MediaCenter.MediaType.Audio,
                            track.FilePath, true);
                }

                return GetMediaState();
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public MediaPlaybackCapabilities GetCapabilities()
        {
            return new MediaPlaybackCapabilities()
            {
                IsToggleShuffleAvailable = false,
                IsToggleLoopAvailable = false,
                IsPlaybackMidlistAvailable = false
            };
        }

        public MediaState PlayTracks(IEnumerable<Track> trackQueue, bool enqueue)
        {
            try
            {
                if (MyAddIn.MediaCenterHost.MediaCenterEnvironment != null)
                {
                    bool shouldEnqueue = enqueue;
                    foreach (Track track in trackQueue)
                    {
                        MyAddIn.MediaCenterHost.MediaCenterEnvironment.PlayMedia(Microsoft.MediaCenter.MediaType.Audio,
                                track.FilePath, shouldEnqueue);
                        // After the first track, enqueue everything else
                        // in either case.
                        shouldEnqueue = true;
                    }
                }

                return GetMediaState();
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public MediaState PlayArtist(Artist artist, bool enqueue)
        {
            MediaLibraryService service = new MediaLibraryService();
            return PlayTracks(service.GetTracksByArtist(artist), enqueue);
        }

        public MediaState PlayAlbumArtist(AlbumArtist artist, bool enqueue)
        {
            MediaLibraryService service = new MediaLibraryService();
            return PlayTracks(service.GetTracksByAlbumArtist(artist), enqueue);
        }

        public MediaState PlayAlbum(Album album, bool enqueue)
        {
            MediaLibraryService service = new MediaLibraryService();
            return PlayTracks(service.GetTracks(album), enqueue);
        }


        public void Ping()
        {
            // Do nothing
        }

        public MediaState SetVolume(double volume)
        {
            Debug.WriteLine("Entered SetVolume with value {0}", volume.ToString());
            Debug.Assert(volume >= 0.0d && volume <= 1.0d, "Volume must be between 0.0d and 1.0d.");

            CoreAudioApi.Audio.MasterVolume = (float)volume;

            return GetMediaState();
        }

        //public MediaState SetVolume(double volume)
        //{
        //    Debug.WriteLine("Entered SetVolume with value {0}", volume.ToString());
        //    Debug.Assert(volume >= 0.0d && volume <= 1.0d, "Volume must be between 0.0d and 1.0d.");

        //    int vol = (int)(volume * (double)VOLUME_MAX);
        //    MediaCenterEnvironment environment = MyAddIn.MediaCenterHost.MediaCenterEnvironment;
        //    if (environment != null)
        //    {
        //        AudioMixer mixer = environment.AudioMixer;
        //        if (mixer != null)
        //        {
        //            mixer.Mute = false;

        //            int currentVolume = mixer.Volume;
        //            if (currentVolume != vol)
        //            {
        //                bool adjustUp = currentVolume < vol;

        //                do
        //                {
        //                    Debug.WriteLine(string.Format("adjustUp: {0}, mixer.Volume: {1}, vol: {2}", adjustUp.ToString(), mixer.Volume.ToString(), vol.ToString()));

        //                    if (adjustUp)
        //                    {
        //                        mixer.VolumeUp();
        //                    }
        //                    else
        //                    {
        //                        mixer.VolumeDown();
        //                    }
        //                    Thread.Sleep(100);
        //                } while ((adjustUp && mixer.Volume < vol) ||
        //                    (!adjustUp && mixer.Volume > vol));

        //                Debug.WriteLine(string.Format("Left volume loop, volume at {0}", mixer.Volume.ToString()));
        //            }
        //        }
        //    }

        //    return GetMediaState();
        //}
    }
}
