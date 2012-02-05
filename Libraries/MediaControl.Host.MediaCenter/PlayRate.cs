using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaControl.Host.MediaCenter
{
    public static class PlayRate 
    {
        public const float Stop = 0.0f;
        public const float Pause = 1.0f;
        public const float Play = 2.0f;
        public const float FastFoward1 = 3.0f;
        public const float FastFoward2 = 4.0f;
        public const float FastFoward3 = 5.0f;
        public const float Rewind1 = 6.0f;
        public const float Rewind2 = 7.0f;
        public const float Rewind3 = 8.0f;
        public const float SlowMotion1 = 9.0f;
        public const float SlowMotion2 = 10.0f;
        public const float SlowMotion3 = 11.0f;
    }
}
