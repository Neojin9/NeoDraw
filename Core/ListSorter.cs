using System;
using System.Collections.Generic;

namespace NeoDraw.Core{

    public class ListSorter<T> : IComparer<T> {

        public readonly string Name;
        public readonly Func<T, bool> Allow;
        public readonly Func<T, T, int> Comparer;

        public ListSorter(string name, Func<T, T, int> comparer, Func<T, bool> allow) {

            Name = name;
            Comparer = comparer;
            Allow = allow;

        }

        #region IComparer<T> Members

        public int Compare(T t1, T t2) => Comparer(t1, t2);

        #endregion

    }

}
