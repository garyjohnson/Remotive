using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharkfist.Phone.SilverlightCore
{
    public interface IVirtualizingListSource<T>
    {
        int Count { get; }
        void GetRangeAsync(int startIndex, int count);
        event EventHandler<GetRangeCompletedArgs<T>> GetRangeCompleted;
    }
}
