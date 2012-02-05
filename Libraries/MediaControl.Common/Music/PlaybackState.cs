using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaControl.Common.Music
{
    public enum PlaybackState { 
        Unknown,
        Stop, Play, Pause,
        FastFoward, FastFoward2, FastFoward3,
        Rewind, Rewind2, Rewind3,
        SlowMotion, SlowMotion2, SlowMotion3
    };
}
