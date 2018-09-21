namespace ICSSoft.STORMNET.Collections
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// коллекция со строковыми ключами
    /// </summary>
    [Serializable]
    public class NameObjectCollection : System.Collections.Specialized.NameObjectCollectionBase, ISerializable
    {
        System.Collections.Specialized.StringCollection keys = new System.Collections.Specialized.StringCollection();

        /// <summary>
        ///
        /// </summary>
        public NameObjectCollection()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public NameObjectCollection(SerializationInfo info, StreamingContext context)
        {
            string[] allkeys = (string[])info.GetValue("allkeys", typeof(string[]));
            object[] allvalues = (object[])info.GetValue("allvalues", typeof(object[]));
            for (int i = 0; i < allkeys.Length; i++)
            {
                Add(allkeys[i], allvalues[i]);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("allkeys", this.GetAllKeys());
            info.AddValue("allvalues", this.GetAllValues());
        }

        /// <summary>
        /// добавить элемент
        /// </summary>
        /// <param name="name">имя</param>
        /// <param name="value">значение</param>
        public void Add(string name, object value)
        {
            lock (this) // (Колчанов 20150327) Добавил. Иначе падает при многопоточной работе, говорит иногда, что ключ уже добавлен.
            {
                if (!keys.Contains(name))
                {
                    BaseAdd(name, value);
                    keys.Add(name);
                }
            }
        }

        /// <summary>
        /// Добавить элемент, при этом ключом будет случайный Guid, приведённый к строке
        /// (Сам метод нужен для того, чтобы работала XML-сериализация, логика не)
        /// </summary>
        /// <param name="value"></param>
        public void Add(object value)
        {
            BaseAdd(null, value);
            keys.Add(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// очистить
        /// </summary>
        public void Clear()
        {
            BaseClear();
            keys.Clear();
        }

        /// <summary>
        /// доступ по порядковому номеру
        /// </summary>
        public object this[int index]
        {
            get
            {
                return BaseGet(index);
            }

            set
            {
                BaseSet(index, value);
            }
        }

        /// <summary>
        /// доступ по имени элемента
        /// </summary>
        public object this[string name]
        {
            get
            {
                return BaseGet(name);
            }

            set
            {
                BaseSet(name, value);
            }
        }

        /// <summary>
        /// взять по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object Get(int index)
        {
            return BaseGet(index);
        }

        /// <summary>
        /// взять по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Get(string name)
        {
            return BaseGet(name);
        }

        /// <summary>
        /// положить по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Set(int index, object value)
        {
            BaseSet(index, value);
        }

        /// <summary>
        /// положить по имени
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set(string name, object value)
        {
            BaseSet(name, value);
        }

        /// <summary>
        /// получить все ключи-имена
        /// </summary>
        /// <returns></returns>
        public string[] GetAllKeys()
        {
            return BaseGetAllKeys();
        }

        /// <summary>
        /// получить все значения
        /// </summary>
        /// <returns></returns>
        public object[] GetAllValues()
        {
            return BaseGetAllValues();
        }

        /// <summary>
        /// получить все значения оперделенного типа
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object[] GetAllValues(Type type)
        {
            return BaseGetAllValues(type);
        }

        /// <summary>
        /// получить ключ по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetKey(int index)
        {
            return BaseGetKey(index);
        }

        /// <summary>
        /// есть ли непустые ключи
        /// </summary>
        /// <returns></returns>
        public bool HasKeys()
        {
            return BaseHasKeys();
        }

        /// <summary>
        /// удалить по имени
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            BaseRemove(name);
            keys.Remove(name);
        }

        /// <summary>
        /// удалить по индексу
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
            keys.RemoveAt(index);
        }

        /// <summary>
        /// есть ли значение с заданным ключем
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsKey(string name)
        {
            return keys.Contains(name);
        }
    }
}