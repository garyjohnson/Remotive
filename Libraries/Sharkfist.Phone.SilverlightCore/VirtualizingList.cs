using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Sharkfist.Phone.SilverlightCore
{
    [DataContract()]
    public class VirtualizingList<T> : IList, IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public VirtualizingList() { }

        public VirtualizingList(IVirtualizingListSource<T> listSource) : this()
        {
            ListSource = listSource;
        }
        
        private IVirtualizingListSource<T> _listSource;
        [IgnoreDataMember()]
        public IVirtualizingListSource<T> ListSource
        {
            get { return _listSource; }
            set
            {
                // Unwire previous source
                if (_listSource != null)
                {
                    _listSource.GetRangeCompleted -= new EventHandler<GetRangeCompletedArgs<T>>(_listSource_GetRangeCompleted);
                }

                _listSource = value;
                if (_listSource != null)
                {
                    _listSource.GetRangeCompleted += new EventHandler<GetRangeCompletedArgs<T>>(_listSource_GetRangeCompleted);
                }

                FirePropertyChanged("ListSource");
            }
        }

        private const int FETCH_SIZE = 200;
        private const int MAX_AGE = 3;
        private int _cursorPosition = 0;

        private List<int> _rangeRequests = null;
        [IgnoreDataMember()]
        private List<int> RangeRequests
        {
            get
            {
                if (_rangeRequests == null)
                {
                    _rangeRequests = new List<int>();
                }

                return _rangeRequests;
            }
        }

        private Dictionary<int, IList<T>> _cache = new Dictionary<int, IList<T>>();
        [DataMember()]
        public Dictionary<int, IList<T>> Cache
        {
            get { return _cache; }
            set { _cache = value; }
        }

        private List<int> _ageQueue = new List<int>();
        [DataMember()]
        public List<int> AgeQueue
        {
            get { return _ageQueue; }
            set { _ageQueue = value; }
        }

        // This list contains all the indexes we had to return null for because we didn't have
        // the item when requested. When the colleciton comes back we will notify of remove / add
        // for these items.
        private List<int> _emptyItems = new List<int>();
        [DataMember()]
        public List<int> EmptyItems
        {
            get { return _emptyItems; }
            set { _emptyItems = value; }
        }

        private void _listSource_GetRangeCompleted(object sender, GetRangeCompletedArgs<T> e)
        {
            int retrievedIndex = e.Index / FETCH_SIZE;
            RangeRequests.Remove(retrievedIndex);
            AddToCache(retrievedIndex, e.Items);

            if (CollectionChanged != null)
            {
                // SLOW: We want to prefetch to avoid this if possible
                // First, notify of the original items removal
                for(int i = _emptyItems.Count - 1; i >= 0; i--)
                {
                    int emptyIndex = _emptyItems[i];
                    if(emptyIndex >= e.Index && emptyIndex < e.Index + e.Count)
                    {
                        _emptyItems.Remove(emptyIndex);

                        NotifyCollectionChangedEventArgs removeArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, null, emptyIndex);
                        CollectionChanged(this, removeArgs);

                        NotifyCollectionChangedEventArgs addArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.Items[emptyIndex - e.Index], emptyIndex);
                        CollectionChanged(this, addArgs);
                    }
                }
            }

            if (_cursorPosition > 0)
            {
                if (!_cache.ContainsKey(_cursorPosition - 1))
                {
                    GetRangeAsync(_cursorPosition - 1, FETCH_SIZE);
                }
            }
            else if (!_cache.ContainsKey(_cursorPosition + 1))
            {
                GetRangeAsync(_cursorPosition + 1, FETCH_SIZE);
            }
        }

        private void GetRangeAsync(int cursorPos, int fetchSize)
        {
            int index = cursorPos * fetchSize;
            if (index < Count)
            {
                if (!RangeRequests.Contains(cursorPos))
                {
                    RangeRequests.Add(cursorPos);

                    int size = fetchSize;
                    if ((index + fetchSize - Count) > 0)
                    {
                        size = Count - index;
                    }

                    _listSource.GetRangeAsync(cursorPos * fetchSize, size);
                }
            }
        }

        private void AddToCache(int index, IList<T> value)
        {
            // If the age queue contains
            if (_ageQueue.Contains(index))
            {
                _ageQueue.Remove(index);
            }

            if(_cache.ContainsKey(index))
            {
                _cache[index] = value;
            }
            else
            {
                _cache.Add(index, value);
            }

            _ageQueue.Add(index);
            ClearCache();
        }

        private void ClearCache()
        {
            int overflow = _ageQueue.Count - MAX_AGE;
            if (overflow > 0)
            {
                for (int i = 0; i < overflow; i++)
                {
                    int key = _ageQueue[0];
                    _cache.Remove(key);
                    _ageQueue.RemoveAt(0);
                }
            }
        }

        #region IList

        public object this[int index]
        {
            get
            {
               Debug.WriteLine("Virtualizing List: Request for item {0}", index);

                // Sanity check. Was getting calls for -1 index 
                // for some reason
                if (index < 0 || index > Count)
                {
                    return null;
                }

                int cursorPos = index / FETCH_SIZE;
                int diff = index % FETCH_SIZE;

                if (_cache.ContainsKey(cursorPos))
                {
                    // We have the thing that was requested, 
                    // go fetch the nearest range if we don't have it
                    if (cursorPos > 0 && diff < FETCH_SIZE / 2)
                    {
                        if (!_cache.ContainsKey(cursorPos - 1))
                        {
                            GetRangeAsync(cursorPos - 1, FETCH_SIZE);
                        }
                    }
                    else if (diff > FETCH_SIZE / 2)
                    {
                        if (!_cache.ContainsKey(cursorPos + 1))
                        {
                            GetRangeAsync(cursorPos + 1, FETCH_SIZE);
                        }
                    }

                    return _cache[cursorPos][diff];
                }
                
                GetRangeAsync(cursorPos, FETCH_SIZE);

                // Add index to list to notify that we got the item
                if (!_emptyItems.Contains(index))
                {
                    _emptyItems.Add(index);
                }
                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get { return ListSource.Count; }
        }

        public int IndexOf(object value)
        {
            return -1;
        }

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion IList

        #region IList<T>

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        T IList<T>.this[int index]
        {
            get
            {
                return (T)this[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }
        
        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
            
        #endregion IList<T>

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void FireCollectionChanged(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
        {
            if (CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index);
                CollectionChanged(this, args);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
