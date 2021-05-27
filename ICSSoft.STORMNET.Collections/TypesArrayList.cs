namespace ICSSoft.STORMNET.Collections
{
    using System;
    using System.Collections;

    /// <summary>
    /// Список типов.
    /// </summary>
    [Serializable]
    public class TypesArrayList : IEnumerable
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private class TypeComaprer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                string xn = ((Type)x).FullName;
                string yn = ((Type)y).FullName;
                return string.Compare(xn, yn);
            }

            #endregion
        }

        private static IComparer stComparer;

        private static IComparer Comparer
        {
            get
            {
                if (stComparer == null)
                {
                    stComparer = new TypeComaprer();
                }

                return stComparer;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dataArray).GetEnumerator();
        }

        private ArrayList dataArray;

        /// <summary>
        ///
        /// </summary>
        /// <param name="capacity"></param>
        public TypesArrayList(int capacity)
        {
            dataArray = new ArrayList(capacity);
        }

        /// <summary>
        ///
        /// </summary>
        public TypesArrayList()
        {
            dataArray = new ArrayList();
        }

        /// <summary>
        /// емкость.
        /// </summary>
        public virtual int Capacity
        {
            get { return dataArray.Capacity; }
            set { dataArray.Capacity = value; }
        }

        /// <summary>
        /// количество элементов.
        /// </summary>
        public virtual int Count
        {
            get { return dataArray.Count; }
        }

        /// <summary>
        /// вернуть по индексу.
        /// </summary>
        public virtual Type this[int index]
        {
            get { return (Type)dataArray[index]; }
            set { dataArray[index] = value; }
        }

        /// <summary>
        /// объект для синхронизации доступа к массиву.
        /// </summary>
        public virtual object SyncRoot
        {
            get { return dataArray.SyncRoot; }
        }

        /// <summary>
        /// Array wrapper.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ArrayList Adapter(IList list)
        {
            return ArrayList.Adapter(list);
        }

        /// <summary>
        /// добавить.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual int Add(Type value)
        {
            return dataArray.Add(value);
        }

        /// <summary>
        /// поиск.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual int BinarySearch(Type value)
        {
            return dataArray.BinarySearch(value, Comparer);
        }

        /// <summary>
        /// поиск.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual int BinarySearch(
            int index,
            int count,
            Type value)
        {
            return dataArray.BinarySearch(index, count, value, Comparer);
        }

        /// <summary>
        /// очистить.
        /// </summary>
        public virtual void Clear()
        {
            dataArray.Clear();
        }

        /// <summary>
        /// проверка на вхождение.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool Contains(
            Type item)
        {
            return dataArray.Contains(item);
        }

        /// <summary>
        /// копирование.
        /// </summary>
        /// <param name="array"></param>
        public virtual void CopyTo(
            Type[] array)
        {
            dataArray.CopyTo(array);
        }

        /// <summary>
        /// копирование.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public virtual void CopyTo(
            Type[] array,
            int arrayIndex)
        {
            dataArray.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// копирование.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// <param name="count"></param>
        public virtual void CopyTo(
            int index,
            Type[] array,
            int arrayIndex,
            int count)
        {
            dataArray.CopyTo(index, array, arrayIndex, count);
        }

        /// <summary>
        /// проверка на равенство.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(
            object obj)
        {
            if (obj is TypesArrayList)
            {
                return dataArray.Equals(((TypesArrayList)obj).dataArray);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// проверка на равенство.
        /// </summary>
        /// <param name="objA"></param>
        /// <param name="objB"></param>
        /// <returns></returns>
        public static new bool Equals(
            object objA,
            object objB)
        {
            return objA.Equals(objB);
        }

        /// <summary>
        /// индекс объекта.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual int IndexOf(
            Type value)
        {
            return dataArray.IndexOf(value);
        }

        /// <summary>
        /// индекс объекта.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public virtual int IndexOf(
            Type value,
            int startIndex)
        {
            return dataArray.IndexOf(value, startIndex);
        }

        /// <summary>
        /// индекс объекта.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual int IndexOf(
            Type value,
            int startIndex,
            int count)
        {
            return dataArray.IndexOf(value, startIndex, count);
        }

        /// <summary>
        /// Вставить объект.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public virtual void Insert(
            int index,
            Type value)
        {
            dataArray.Insert(index, value);
        }

        /// <summary>
        /// индекс объекта с конца.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual int LastIndexOf(
            Type value)
        {
            return dataArray.LastIndexOf(value);
        }

        /// <summary>
        /// индекс объекта с конца.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public virtual int LastIndexOf(
            Type value,
            int startIndex)
        {
            return dataArray.LastIndexOf(value, startIndex);
        }

        /// <summary>
        /// индекс объекта с конца.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual int LastIndexOf(
            Type value,
            int startIndex,
            int count)
        {
            return dataArray.LastIndexOf(value, startIndex, count);
        }

        /// <summary>
        /// удалить объект.
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Remove(
            Type obj)
        {
            dataArray.Remove(obj);
        }

        /// <summary>
        /// удалить по индексу.
        /// </summary>
        /// <param name="index"></param>
        public virtual void RemoveAt(
            int index)
        {
            dataArray.RemoveAt(index);
        }

        /// <summary>
        /// удалить кучу.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public virtual void RemoveRange(
            int index,
            int count)
        {
            dataArray.RemoveRange(index, count);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TypesArrayList Repeat(
            TypesArrayList value,
            int count)
        {
            TypesArrayList res = new TypesArrayList();
            res.dataArray = ArrayList.Repeat(value.dataArray, count);
            return res;
        }

        /// <summary>
        /// вывернуть.
        /// </summary>
        public virtual void Reverse()
        {
            dataArray.Reverse();
        }

        /// <summary>
        /// вывернуть.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public virtual void Reverse(
            int index,
            int count)
        {
            dataArray.Reverse(index, count);
        }

        /// <summary>
        /// отсортировать.
        /// </summary>
        public virtual void Sort()
        {
            dataArray.Sort(Comparer);
        }

        /// <summary>
        /// отсортировать.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public virtual void Sort(
            int index,
            int count)
        {
            dataArray.Sort(index, count, Comparer);
        }

        /// <summary>
        /// преобразовать в массив.
        /// </summary>
        /// <returns></returns>
        public virtual Type[] ToArray()
        {
            return (Type[])dataArray.ToArray(typeof(Type));
        }

        /// <summary>
        /// в строку.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return dataArray.ToString();
        }

        /// <summary>
        /// обрезать лишнее.
        /// </summary>
        public virtual void TrimToSize()
        {
            dataArray.TrimToSize();
        }
    }
}
