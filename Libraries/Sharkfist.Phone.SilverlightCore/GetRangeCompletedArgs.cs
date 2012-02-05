using System;
using System.Collections.Generic;

namespace Sharkfist.Phone.SilverlightCore
{
    public class GetRangeCompletedArgs<T> : EventArgs
    {
        public GetRangeCompletedArgs(int index, int count, IList<T> items)
        {
            Index = index;
            Count = count;
            Items = items;
        }

        public int Index { get; private set; }
        public int Count { get; private set; }
        public IList<T> Items { get; private set; }
    }
}
