namespace ICSSoft.STORMNET.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// коллекция со Type -  ключами.
    /// </summary>
    [Serializable]
    public class TypeBaseCollection : ISerializable
    {
        private System.Collections.ArrayList types = new System.Collections.ArrayList();
        private System.Collections.ArrayList values = new System.Collections.ArrayList();

        /// <summary>
        ///
        /// </summary>
        public TypeBaseCollection()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public TypeBaseCollection(SerializationInfo info, StreamingContext context)
        {
            Type[] typesarr = (Type[])info.GetValue("types", typeof(Type[]));
            object[] valuesarr = (object[])info.GetValue("values", typeof(object[]));

            lock (types)
            {
                types.AddRange(typesarr);
            }

            lock (values)
            {
                values.AddRange(valuesarr);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            lock (types)
            {
                info.AddValue("types", types.ToArray(typeof(Type)));
            }

            lock (values)
            {
                info.AddValue("values", values.ToArray(typeof(object)));
            }
        }

        /// <summary>
        /// содержит ли ключ.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(Type key)
        {
            lock (types)
            {
                return types.IndexOf(key) >= 0;
            }
        }

        /// <summary>
        /// почистить.
        /// </summary>
        public void Clear()
        {
            lock (types)
            {
                types.Clear();
            }

            lock (values)
            {
                values.Clear();
            }
        }

        /// <summary>
        /// количество.
        /// </summary>
        public int Count
        {
            get { return types.Count; }
        }

        /// <summary>
        /// добавить элемент.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(Type key, object value)
        {
            if (!Contains(key))
            {
                lock (types)
                {
                    types.Add(key);
                }

                lock (values)
                {
                    values.Add(value);
                }
            }
            else
            {
                throw new ArgumentException("Key already exists", "key");
            }
        }

        /// <summary>
        /// доступ по ключу.
        /// </summary>
        public object this[Type key]
        {
            get
            {
                int index = types.IndexOf(key);
                if (index >= 0)
                {
                    lock (values)
                    {
                        return values[index];
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("key", string.Empty);
                }
            }

            set
            {
                int index = -1;
                lock (types)
                {
                    index = types.IndexOf(key);
                }

                if (index >= 0)
                {
                    lock (values)
                    {
                        values[index] = value;
                    }
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        /// <summary>
        /// доступ по индексу.
        /// </summary>
        public object this[int index]
        {
            get
            {
                lock (types)
                {
                    lock (values)
                    {
                        if (index < types.Count && index >= 0)
                        {
                            return values[index];
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("index", string.Empty);
                        }
                    }
                }
            }

            set
            {
                lock (types)
                {
                    lock (values)
                    {
                        if (index < types.Count && index >= 0)
                        {
                            values[index] = value;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("index", string.Empty);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// вставить элемент.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Insert(int index, Type key, object value)
        {
            if (!Contains(key))
            {
                lock (types)
                {
                    lock (values)
                    {
                        if (index >= 0 && index <= types.Count)
                        {
                            for (int i = types.Count; i > index; i--)
                            {
                                types[i] = types[i - 1];
                                values[i] = types[i - 1];
                            }

                            types[index] = key;
                            values[index] = value;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("index", string.Empty);
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("Key already exists", "key");
            }
        }

        /// <summary>
        /// удалить по индексу.
        /// </summary>
        /// <param name="index"></param>
        public void Remove(int index)
        {
            lock (types)
            {
                lock (values)
                {
                    if (index >= 0 && index <= types.Count)
                    {
                        types.RemoveAt(index);
                        values.RemoveAt(index);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("index", string.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// удалить по ключу.
        /// </summary>
        /// <param name="key"></param>
        public void Remove(Type key)
        {
            if (Contains(key))
            {
                Remove(types.IndexOf(key));
            }
        }

        /// <summary>
        /// ключ по индексу.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Type Key(int index)
        {
            lock (types)
            {
                lock (values)
                {
                    if (index >= 0 && index <= types.Count)
                    {
                        return (Type)types[index];
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("index", string.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// вернуть по шаблону(наиболее подходящий).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetMostCompatible(Type key)
        {
            if (Contains(key))
            {
                return this[key];
            }
            else
            {
                Type objectType = typeof(object);
                while (key != objectType)
                {
                    key = key.BaseType;
                    if (Contains(key))
                    {
                        return this[key];
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// вернуть по шаблону.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object[] GetCompatible(Type key)
        {
            System.Collections.ArrayList res = new System.Collections.ArrayList();
            lock (types)
            {
                foreach (Type curType in types)
                {
                    if (curType == key || key.IsSubclassOf(curType))
                    {
                        res.Add(curType);
                    }
                }
            }

            object[] resa = new object[res.Count];
            res.CopyTo(resa);
            return resa;
        }

        /// <summary>
        /// Вернуть элементы коллекции в виде стандартного словаря.
        /// </summary>
        /// <returns>Объект словаря System.Collections.Generic.Dictionary.</returns>
        public Dictionary<Type, object> ToDictionary()
        {
            var result = new Dictionary<Type, object>();
            for (int i = 0; i < types.Count; i++)
            {
                result.Add(types[i] as Type, values[i]);
            }

            return result;
        }
    }
}
