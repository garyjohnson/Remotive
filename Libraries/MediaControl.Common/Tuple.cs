using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MediaControl.Common
{
    [DataContract()]
    public class Tuple<T, U>
    {
        public Tuple(T value1, U value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        [DataMember()]
        public T Value1 { get; private set; }
        [DataMember()]
        public U Value2 { get; private set; }
    }
}
