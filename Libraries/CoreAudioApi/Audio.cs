using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreAudioApi
{
    public class Audio
    {
        private static MMDevice _audioDevice = null;
        private static MMDevice AudioDevice
        {
            get
            {
                if (_audioDevice == null)
                {
                    MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
                    _audioDevice = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
                }

                return _audioDevice;
            }
        }

        public static float MasterVolume
        {
            get
            {
                return AudioDevice.AudioEndpointVolume.MasterVolumeLevelScalar;
            }

            set
            {
                AudioDevice.AudioEndpointVolume.MasterVolumeLevelScalar = value;
            }
        }

        public static bool IsMuted
        {
            get
            {
                return AudioDevice.AudioEndpointVolume.Mute;
            }
            set
            {
                AudioDevice.AudioEndpointVolume.Mute = value;
            }
        }

    }
}
