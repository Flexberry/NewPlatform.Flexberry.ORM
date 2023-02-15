namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// коллекция с доступом по типу-свойству.
    /// </summary>
    public class TypePropertyAtrValueCollection
    {
        // private SortedList sl = new SortedList();
        private Dictionary<long, object> sl = new Dictionary<long, object>();

        /// <summary>
        ///
        /// </summary>
        public TypePropertyAtrValueCollection()
        {
        }

        /// <summary>
        /// Количество элементов в коллекции.
        /// </summary>
        public int Count
        {
            get { return sl.Count; }
        }

        /// <summary>
        /// свойство-доступ.
        /// </summary>
        public object this[Type tp, string propname]
        {
            get
            {
                // string key = tp.AssemblyQualifiedName+"."+propname;
                long key = tp.GetHashCode() * 10000000000 + propname.GetHashCode();
                object retObj;
                if (sl.TryGetValue(key, out retObj))
                {
                    return retObj;
                }

                return null;
                /* Так было раньше
                try
                {
                    return sl[key];
                }
                catch
                {
                    return null;
                }
                 * */
            }

            set
            {
                // string key = tp.AssemblyQualifiedName + "." + propname;
                long key = tp.GetHashCode() * 10000000000 + propname.GetHashCode();

                if (sl.ContainsKey(key))
                {
                    sl.Remove(key);
                }

                sl.Add(key, value);
            }
        }
    }

    /// <summary>
    /// коллекция с доступом по типу.
    /// </summary>
    public class TypeAtrValueCollection
    {
        // private SortedList sl = new SortedList();
        private Dictionary<int, object> sl = new Dictionary<int, object>();

        /// <summary>
        ///
        /// </summary>
        public TypeAtrValueCollection()
        {
        }

        /// <summary>
        /// Количество элементов в коллекции.
        /// </summary>
        public int Count
        {
            get { return sl.Count; }
        }

        /// <summary>
        /// свойство-доступ.
        /// </summary>
        public object this[Type tp]
        {
            get
            {
                // string key = tp.AssemblyQualifiedName;
                int key = tp.GetHashCode();
                object retObj;
                if (sl.TryGetValue(key, out retObj))
                {
                    return retObj;
                }

                return null;
                /*
                try
                {
                    return sl[key];
                }
                catch
                {
                    return null;
                }
                 * */
            }

            set
            {
                // string key = tp.AssemblyQualifiedName;
                int key = tp.GetHashCode();

                if (sl.ContainsKey(key))
                {
                    sl.Remove(key);
                }

                sl.Add(key, value);
            }
        }
    }
}
