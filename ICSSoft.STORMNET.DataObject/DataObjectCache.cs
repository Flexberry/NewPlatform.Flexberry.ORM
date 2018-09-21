namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Кеш объектов данных, ключ кешей контекст вызова.
    /// </summary>
    public class DataObjectCache
    {
        /// <summary>
        /// Кеш объектов данных.
        /// </summary>
        public DataObjectCache()
        {
            _objectCaches = new ArrayList();
            _objectCachesUseCounter = new ArrayList();
        }

        /// <summary>
        /// Создавалка объектов.
        /// </summary>
        public static ObjectCreator Creator
        {
            get
            {
                return _objectCreator ?? (_objectCreator = new ObjectCreator());
            }

            set
            {
                _objectCreator = value;
            }
        }

        /// <summary>
        /// The m_ object creator.
        /// </summary>
        private static ObjectCreator _objectCreator;

        /// <summary>
        /// Кеш объектов данных.
        /// </summary>
        private readonly ArrayList _objectCaches;

        /// <summary>
        /// Кэш объектов данных с использованием счётчиков.
        /// </summary>
        private readonly ArrayList _objectCachesUseCounter;

        /// <summary>
        /// Есть объекты в кеше.
        /// </summary>
        private bool NoCaches
        {
            get
            {
                return _objectCaches == null || _objectCaches.Count == 0;
            }
        }

        /// <summary>
        /// Индекс последнего кеша.
        /// </summary>
        private int _lastCacheIndex = -1;

        /// <summary>
        /// Начать кеширование.
        /// </summary>
        /// <param name="clipParentCache">
        /// Запретить использовать родительский кеш.
        /// </param>
        public void StartCaching(bool clipParentCache)
        {
            lock (_objectCaches)
            {
                if (NoCaches)
                    clipParentCache = true;
                if (clipParentCache)
                {
                    _objectCaches.Add(new Dictionary<TypeKeyPair, WeakReference>(new TypeKeyPairEqualityComparer()));
                    _objectCachesUseCounter.Add(1);
                    _lastCacheIndex++;
                }
                else
                {
                    _objectCachesUseCounter[_lastCacheIndex] = ((int)_objectCachesUseCounter[_lastCacheIndex]) + 1;
                }
            }
        }

        /// <summary>
        /// Закончить кеширование.
        /// </summary>
        public void StopCaching()
        {
            lock (_objectCaches)
            {
                if (!NoCaches)
                {
                    int cacheCount = (int)_objectCachesUseCounter[_lastCacheIndex];
                    cacheCount -= 1;
                    if (cacheCount > 0)
                    {
                        _objectCachesUseCounter[_lastCacheIndex] = cacheCount;
                    }
                    else
                    {
                        _objectCaches.RemoveAt(_lastCacheIndex);
                        _objectCachesUseCounter.RemoveAt(_lastCacheIndex);
                        _lastCacheIndex--;
                    }
                }
            }
        }

        /// <summary>
        /// Get the living data object.
        /// </summary>
        /// <param name="typeofdataobject">
        /// The type of data object.
        /// </param>
        /// <param name="key">
        /// The key of data object.
        /// </param>
        /// <returns>
        /// The <see cref="DataObject"/>.
        /// </returns>
        public DataObject GetLivingDataObject(Type typeofdataobject, object key)
        {
            lock (_objectCaches)
            {
                return PrvGetLivingDataObject(typeofdataobject, key);
            }
        }

        /// <summary>
        /// Получить "живой" внутри приложения объект данных по указанию
        /// типа объекта данных и первичного ключа.
        /// Возвращается <c>null</c>, если объект не найден или он уже "умер".
        /// </summary>
        /// <param name="typeofdataobject">
        /// Тип объекта данных.
        /// </param>
        /// <param name="key">
        /// Ключ объекта данных.
        /// </param>
        /// <returns>
        /// Объект данных.
        /// </returns>
        public DataObject CreateDataObject(Type typeofdataobject, object key)
        {
            key = Information.TranslateValueToPrimaryKeyType(typeofdataobject, key);

            DataObject dobj = null;
            if (!NoCaches)
                dobj = PrvGetLivingDataObject(typeofdataobject, key);
            if (dobj == null)
            {
                dobj = (DataObject)Creator.CreateObject(typeofdataobject);
                dobj.__PrimaryKey = key;
                if (!NoCaches)
                {
                    lock (_objectCaches)
                    {
                        if (PrvGetLivingDataObject(typeofdataobject, key) == null)
                        {
                            AddLivingDataObject(dobj);
                        }
                    }
                }
            }

            return dobj;
        }

        /// <summary>
        /// Добавить объект в кеш.
        /// </summary>
        /// <param name="dobj">
        /// Объект для добавления.
        /// </param>
        public void AddDataObject(DataObject dobj)
        {
            lock (_objectCaches)
            {
                if (!NoCaches)
                {
                    AddLivingDataObject(dobj);
                }
            }
        }

        /// <summary>
        /// Изменить ключ у кешированного объекта.
        /// </summary>
        /// <param name="dataobject">
        /// Объект данных.
        /// </param>
        /// <param name="oldkey">
        /// Предыдущий ключ.
        /// </param>
        internal void ChangeKeyForLivingDataObject(DataObject dataobject, object oldkey)
        {
            lock (_objectCaches)
            {
                if (!NoCaches)
                {
                    object obj = GetLivingDataObject(dataobject.GetType(), oldkey);
                    if (obj != null)
                    {
                        PrvRemoveLivingDataObject(dataobject.GetType(), oldkey);
                        AddLivingDataObject(dataobject);
                    }
                }
            }
        }

        /// <summary>
        /// Удалить объект из кеша.
        /// </summary>
        /// <param name="typeofdataobject">
        /// Тип объекта.
        /// </param>
        /// <param name="key">
        /// Первичный ключ.
        /// </param>
        internal void RemoveLivingDataObject(Type typeofdataobject, object key)
        {
            lock (_objectCaches)
            {
                if (key.ToString() == "{fbd82b95-cba3-4f84-9fc2-cbb08ce9da9a}")
                {
                }

                PrvRemoveLivingDataObject(typeofdataobject, key);
            }
        }

        /// <summary>
        /// Удалить "живой" объект данных из кеша.
        /// </summary>
        /// <param name="typeofdataobject">
        /// Тип объекта данных.
        /// </param>
        /// <param name="key">
        /// Ключ объекта данных.
        /// </param>
        private void PrvRemoveLivingDataObject(Type typeofdataobject, object key)
        {
            if (!NoCaches)
            {
                key = Information.TranslateValueToPrimaryKeyType(typeofdataobject, key);

                TypeKeyPair pairkey = new TypeKeyPair(typeofdataobject, key);
                if (_lastCacheIndex == -1)
                    return;
                Dictionary<TypeKeyPair, WeakReference> sl = (Dictionary<TypeKeyPair, WeakReference>)_objectCaches[_lastCacheIndex];

                if (sl.ContainsKey(pairkey))
                {
                    WeakReference wr = sl[pairkey];
                    if (!wr.IsAlive)
                        sl.Remove(pairkey);
                }
            }
        }

        /// <summary>
        /// Добавить объект в кеш.
        /// </summary>
        /// <param name="dataobject">
        /// Объект данных.
        /// </param>
        private void AddLivingDataObject(DataObject dataobject)
        {
            if (NoCaches)
                throw new DOCacheNotFoundException();

            WeakReference wr = new WeakReference(dataobject);
            TypeKeyPair tkp = new TypeKeyPair(dataobject.GetType(), dataobject.__PrimaryKey);
            if (_lastCacheIndex > _objectCaches.Count - 1)
            {
                LogService.LogDebug(_lastCacheIndex + ":" + _objectCaches.Count);
            }

            Dictionary<TypeKeyPair, WeakReference> sl = (Dictionary<TypeKeyPair, WeakReference>)_objectCaches[_lastCacheIndex];
            try
            {
                sl.Remove(tkp);
            }
            catch
            {
            }

            try
            {
                sl.Add(tkp, wr);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Получить "живой" внутри приложения объект данных по указанию
        /// типа объекта данных и первичного ключа.
        /// Возвращается null, если объект не найден или он уже "умер".
        /// </summary>
        /// <param name="typeofdataobject">
        /// The typeofdataobject.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="DataObject"/>.
        /// </returns>
        private DataObject PrvGetLivingDataObject(Type typeofdataobject, object key)
        {
            key = Information.TranslateValueToPrimaryKeyType(typeofdataobject, key);
            DataObject result;
            if (NoCaches)
                return null;
            WeakReference wr = null;
            try
            {
                Dictionary<TypeKeyPair, WeakReference> cacheList = (Dictionary<TypeKeyPair, WeakReference>)_objectCaches[_lastCacheIndex];
                TypeKeyPair typeKeyPair = new TypeKeyPair(typeofdataobject, key);
                if (cacheList.ContainsKey(typeKeyPair))
                {
                    wr = cacheList[typeKeyPair];
                }
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
            }

            if (wr != null)
            {
                if (wr.IsAlive)
                {
                    result = (DataObject)wr.Target;
                }
                else
                {
                    PrvRemoveLivingDataObject(typeofdataobject, key);
                    result = null;
                }
            }
            else
            {
                result = null;
            }

            return result;
        }
    }
}