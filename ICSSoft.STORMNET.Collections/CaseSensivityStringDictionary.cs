namespace ICSSoft.STORMNET.Collections
{
    using System;

    /// <summary>
    /// Summary description for CaseSensivityStringDictionary.
    /// </summary>
    public class CaseSensivityStringDictionary : System.Collections.Specialized.NameObjectCollectionBase
    {
        private bool changed = false;
        private string[] sortedKeys = new string[0];

        /// <summary>
        ///
        /// </summary>
        public CaseSensivityStringDictionary()
        {
        }

        /// <summary>
        /// добавить элемент.
        /// </summary>
        /// <param name="name">имя.</param>
        /// <param name="value">значение.</param>
        public void Add(string name, string value)
        {
            changed = true;
            BaseAdd(name, value);
        }

        /// <summary>
        /// очистить.
        /// </summary>
        public void Clear()
        {
            changed = true;
            BaseClear();
        }

        /// <summary>
        /// доступ по порядковому номеру.
        /// </summary>
        public string this[int index]
        {
            get
            {
                return (string)BaseGet(index);
            }

            set
            {
                BaseSet(index, value);
            }
        }

        /// <summary>
        /// доступ по имени элемента.
        /// </summary>
        public string this[string name]
        {
            get
            {
                return (string)BaseGet(name);
            }

            set
            {
                BaseSet(name, value);
            }
        }

        /// <summary>
        /// взять по индексу.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string Get(int index)
        {
            return (string)BaseGet(index);
        }

        /// <summary>
        /// взять по имени.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Get(string name)
        {
            return (string)BaseGet(name);
        }

        /// <summary>
        /// положить по индексу.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, string value)
        {
            BaseSet(index, value);
        }

        /// <summary>
        /// положить по имени.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set(string name, string value)
        {
            BaseSet(name, value);
        }

        /// <summary>
        /// получить все ключи-имена.
        /// </summary>
        /// <returns></returns>
        public string[] GetAllKeys()
        {
            return BaseGetAllKeys();
        }

        /// <summary>
        /// получить все значения.
        /// </summary>
        /// <returns></returns>
        public string[] GetAllValues()
        {
            return (string[])BaseGetAllValues();
        }

        /// <summary>
        /// получить все значения оперделенного типа.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string[] GetAllValues(Type type)
        {
            return (string[])BaseGetAllValues(type);
        }

        /// <summary>
        /// получить ключ по индексу.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetKey(int index)
        {
            return BaseGetKey(index);
        }

        /// <summary>
        /// есть ли непустые ключи.
        /// </summary>
        /// <returns></returns>
        public bool HasKeys()
        {
            return BaseHasKeys();
        }

        /// <summary>
        /// удалить по имени.
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            changed = true;
            BaseRemove(name);
        }

        /// <summary>
        /// удалить по индексу.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            changed = true;
            BaseRemoveAt(index);
        }

        /// <summary>
        /// есть ли значение с заданным ключем.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsKey(string name)
        {
            if (changed)
            {
                sortedKeys = BaseGetAllKeys();
                Array.Sort(sortedKeys);
            }

            return Array.BinarySearch(sortedKeys, name) >= 0;
        }
    }
}
