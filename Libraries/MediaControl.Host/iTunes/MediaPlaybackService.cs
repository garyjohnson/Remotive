using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaControl.Common;
using System.ServiceModel;
using MediaControl.Common.Music;
using iTunesLib;

namespace MediaControl.Host.iTunes
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class MediaPlaybackService: IMediaPlaybackService
    {
        public MediaPlaybackService() { }

        public MediaState Play()
        {
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                iTunes.Play();
                return GetMediaState();
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

        public MediaState Pause()
        {
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                iTunes.Pause();
                return GetMediaState();
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

        public MediaState NextTrack()
        {
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                iTunes.NextTrack();
                return GetMediaState();
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

        public MediaState PrevTrack()
        {
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                iTunes.PreviousTrack();
                return GetMediaState();
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

        public MediaState PlayTrack(Common.Music.Track track)
        {
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                iTunes.PlayFile(track.FilePath);
                return GetMediaState();
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

        public MediaState GetMediaState()
        {
            return new MediaState()
            {
                CurrentTrack = GetNowPlaying(),
                PlaybackState = GetPlaybackState(),
                PlaybackPosition = GetPlaybackPosition(),
                CurrentTime = DateTime.Now
            };     
        }

        private TimeSpan GetPlaybackPosition()
        {
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                if (iTunes.PlayerState != ITPlayerState.ITPlayerStateStopped)
                {
                    return TimeSpan.FromSeconds(iTunes.PlayerPosition);
                }
                else
                {
                    return new TimeSpan();
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
        }

        private Track GetNowPlaying()
        {
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();

                if (iTunes.CurrentTrack != null)
                {
                    IITFileOrCDTrack fileTrack = iTunes.CurrentTrack as IITFileOrCDTrack;
                    if (fileTrack != null)
                    {
                        return fileTrack.GetTrack();
                    }
                }

                return null;
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

        private Common.Music.PlaybackState GetPlaybackState()
        {
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                switch(iTunes.PlayerState)
                {
                    case ITPlayerState.ITPlayerStateFastForward:
                        {
                            return Common.Music.PlaybackState.FastFoward;
                        }
                    case ITPlayerState.ITPlayerStatePlaying:
                        {
                            return Common.Music.PlaybackState.Play;
                        }
                    case ITPlayerState.ITPlayerStateRewind:
                        {
                            return Common.Music.PlaybackState.Rewind;
                        }
                    case ITPlayerState.ITPlayerStateStopped:
                        {
                            return Common.Music.PlaybackState.Stop;
                        }
                }

                return PlaybackState.Unknown;
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

        private void LoadPlaylist(IEnumerable<Track> trackQueue)
        {
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();

                // TODO Implement
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

        public MediaState EnqueueTracks(IEnumerable<Track> trackQueue, Track playTrack)
        {
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                
                //TODO implement

                return GetMediaState();
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
            iTunesApp iTunes = null;

            try
            {
                iTunes = new iTunesApp();
                IITPlaylist nowPlaying = GetOrCreateNowPlaying();
                //TODO implement

                return GetMediaState();
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

        private const string NOW_PLAYING = "Media Control - Now Playing";
        private IITPlaylist GetOrCreateNowPlaying()
        {
            iTunesApp iTunes = new iTunesApp();
            IITPlaylist nowPlaying = iTunes.LibrarySource.Playlists.get_ItemByName(NOW_PLAYING);
            if (nowPlaying == null || nowPlaying.Kind != ITPlaylistKind.ITPlaylistKindLibrary)
            {
                if (nowPlaying.Kind != ITPlaylistKind.ITPlaylistKindLibrary)
                {
                    nowPlaying.Delete();
                }

                nowPlaying = iTunes.CreatePlaylistInSource(NOW_PLAYING, iTunes.LibrarySource);
            }

            return nowPlaying;
        }

        public MediaState PlayArtist(Artist artist, bool enqueue)
        {
            MediaLibraryService service = new MediaLibraryService();
            return PlayTracks(service.GetTracksByArtist(artist), enqueue);
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

        public MediaState PlayAlbumArtist(AlbumArtist artist, bool enqueue)
        {
            throw new NotImplementedException();
        }

        public MediaState SetVolume(double volume)
        {
            throw new NotImplementedException();
        }
    }
}
