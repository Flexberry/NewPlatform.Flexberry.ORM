namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Specialized;

    using ICSSoft.STORMNET.Collections;
    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using NewPlatform.Flexberry.Services;

    /// <summary>
    /// Интерфейс для сервисов пессиместических блокировок
    /// </summary>
    //public interface ILockService
    //{
    //    /// <summary>
    //    /// Сервис данных, с которым нужно пойти в базу данных с блокировками
    //    /// </summary>
    //    IDataService DataService { get; set; }

    //    /// <summary>
    //    /// Убить все блокировки пользователя
    //    /// </summary>
    //    /// <param name="userName"></param>
    //    void ClearAllUserLocks(string userName);

    //    /// <summary>
    //    /// Убить указанную блокировку для указанного пользователя
    //    /// </summary>
    //    /// <param name="lockName"></param>
    //    /// <param name="userName"></param>
    //    void ClearLock(string lockName, string userName);

    //    /// <summary>
    //    /// Устанавливает блокировку. Если удачно - то возвращается пустая строка, если нет - UserName.
    //    /// </summary>
    //    /// <returns>Имя пользователя, которым занят ресурс</returns>
    //    string SetLock(string lockName, string userName);


    //    /// <summary>
    //    /// Locks the object with specified key.
    //    /// </summary>
    //    /// <param name="dataObjectKey">The data object key.</param>
    //    /// <param name="userName">Name of the user who locks the object.</param>
    //    /// <returns>Information about lock.</returns>
    //    LockData LockObject(object dataObjectKey, string userName);

    //    /// <summary>
    //    /// Gets the lock of object with specified key.
    //    /// </summary>
    //    /// <param name="dataObjectKey">The data object key.</param>
    //    /// <returns>Information about lock.</returns>
    //    LockData GetLock(object dataObjectKey);

    //    /// <summary>
    //    /// Unlocks the object.
    //    /// </summary>
    //    /// <param name="dataObjectKey">The data object key.</param>
    //    /// <returns>Returns <c>true</c> if object is successfully unlocked or <c>false</c> if it's not exist.</returns>
    //    bool UnlockObject(object dataObjectKey);

    //    /// <summary>
    //    /// Unlocks all objects.
    //    /// </summary>
    //    void UnlockAllObjects();
    //}

    public class LockServiceWithTime: ILockService
    {
        private static string name = "LockServiceWithTime";


        private TimeSpan m_spPeriod;
        private IDataService ds;

        //TODO: ivashkevich: task1677: принимать датасервис через di, period вытаскивать на прямую из COPCacheOptions.SlidingExpiration
        //TODO: ivashkevich: task1677: при установке и снятии блокировок учесть, что датасервис может поддерживать IDataServiceWrapper, тогда необходимо итерировать датасервисы (иерархически). Так же учитывать что можно использовать кокретный датаесвис, зависимы от aria. Вероятно при установке - в зависимости от aria, а при снятии - итерированием по всем датасервисам
        public LockServiceWithTime(TimeSpan period, IDataService ds = null)
        {
            this.m_spPeriod = period;
            this.ds = ds;
        }

        //TODO: ivashkevich: task1677: убрать и принимать датасервис только через di
        /// <summary>
        /// Сервис данных, с которым нужно пойти в базу данных с блокировками
        /// </summary>
        public IDataService DataService
        {
            get
            {
                return ds;
            }
            set
            {
                ds = value;
            }
        }

        /// <summary>
        /// Убить все блокировки пользователя
        /// </summary>
        /// <param name="userName"></param>
        public void ClearAllUserLocks(string userName)
        {
            SQLWhereLanguageDef ld = SQLWhereLanguageDef.LanguageDef;

            prv_ClearLocksByFunction(
                ld.GetFunction(ld.funcEQ, new VariableDef(ld.StringType, "UserName"), userName)
                );
        }

        /// <summary>
        /// Убить указанную блокировку для указанного пользователя
        /// </summary>
        /// <param name="lockName"></param>
        /// <param name="userName"></param>
        public void ClearLock(string lockName, string userName)
        {
            SQLWhereLanguageDef ld = SQLWhereLanguageDef.LanguageDef;

            if (!string.IsNullOrEmpty(lockName))
            {

                prv_ClearLocksByFunction(ld.GetFunction(ld.funcAND,
                ld.GetFunction(ld.funcEQ, new VariableDef(ld.StringType, SQLWhereLanguageDef.StormMainObjectKey), lockName),
                ld.GetFunction(ld.funcEQ, new VariableDef(ld.StringType, "UserName"), userName)
                ));
            }
        }

        public LockInfo GetLock(object dataObjectKey)
        {
            throw new NotImplementedException();
        }

        public LockInfo LockObject(object dataObjectKey, string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Убить блокировки согласно переданной функции ограничений
        /// </summary>
        /// <param name="func"></param>
        public void prv_ClearLocksByFunction(Function func)
        {
            SQLWhereLanguageDef ld = SQLWhereLanguageDef.LanguageDef;

            LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(LockData), "V");
            lcs.LimitFunction = func;

            DataObject[] locks = ds.LoadObjects(lcs);
            if (locks.Length > 0)
            {
                foreach (DataObject lck in locks)
                {
                    lck.SetStatus(ObjectStatus.Deleted);
                }
            }

            ds.UpdateObjects(ref locks);

        }

        /// <summary>
        /// Устанавливает блокировку. Если удачно - то возвращается UserName под которым устанавливается, если нет - UserName под которым установленно.
        /// </summary>
        /// <returns>Имя пользователя, которым занят ресурс</returns>
        public string SetLock(string lockName, string userName)
        {
            string result = null; // если не удалось поставить блокировку - будет null

            if (!string.IsNullOrEmpty(lockName))
            {

                lock (name)
                {

                    SQLWhereLanguageDef ld = SQLWhereLanguageDef.LanguageDef;

                    LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(LockData), "V");
                    lcs.LimitFunction = ld.GetFunction(ld.funcEQ, new VariableDef(ld.StringType, SQLWhereLanguageDef.StormMainObjectKey), lockName);

                    DataObject[] locks = ds.LoadObjects(lcs);
                    if (locks.Length > 0)
                    { //Есть блокировка. Проверяем, не выдохлась ли она. Если выдохлась, то убиваем её.
                        LockData exlock = (LockData)locks[0];

                        if (exlock.TM + m_spPeriod < DateTime.Now)
                        {
                            exlock.SetStatus(ObjectStatus.Deleted);
                            ds.UpdateObjects(ref locks);
                        }
                        else
                        {
                            if (exlock.UserName == userName) //Пользователь тот же, нужно обновить время блокировки
                            {
                                exlock.TM = DateTime.Now;
                                ds.UpdateObject(exlock);
                            }
                            result = exlock.UserName; //Возвращаем имя пользователя, которым занят ресурс
                        }
                    }

                    if (locks.Length == 0) //Блокировки не было, либо она была снята, нужно выставить новую блокировку
                    {
                        LockData newlock = new LockData();
                        newlock.__PrimaryKey = lockName;
                        newlock.UserName = userName;
                        ds.UpdateObject(newlock);
                        result = newlock.UserName;
                    }
                }
            }
            return result;
        }

        public void UnlockAllObjects()
        {
            throw new NotImplementedException();
        }

        public bool UnlockObject(object dataObjectKey)
        {
            throw new NotImplementedException();
        }

       
    }

    public class LockService:ILockService
    {
        /// <summary>
        /// Сервис данных, через который по умолчанию нужно делать запрос к БД.
        /// </summary>
        private IDataService dataService = DataServiceProvider.DataService;

        /// <summary>
        /// Конструктор без параметров. В качестве сервиса данных по умолчанию будет использоваться DataServiceProvider.DataService.
        /// </summary>
        public LockService()
            : this(null)
        {
        }

        /// <summary>
        /// Конструктор с определением сервиса данных.
        /// </summary>
        /// <param name="dataService">
        /// Сервис данных, через который по умолчанию нужно делать запрос к БД.
        /// Если передан null, то будет использоваться DataServiceProvider.DataService.
        /// </param>
        public LockService(IDataService dataService)
        {
            this.dataService = dataService ?? DataServiceProvider.DataService;
        }

        #region Constants and Fields

        /// <summary>
        /// The viewsbytypes.
        /// </summary>
        private static readonly TypeBaseCollection viewsbytypes = new TypeBaseCollection();

        /// <summary>
        /// Кеш блокировок
        /// </summary>
        private readonly SortedList AllLocks = new SortedList();

        /// <summary>
        /// Для совместимости
        /// </summary>
        private static string username = string.Empty;

        #endregion

        #region Public Properties

        /// <summary>
        /// Использовать ли имя компьютера в блокировке
        /// </summary>
        public static bool UseMachineNameInKey { get; set; }
        public IDataService DataService { get => dataService; set => dataService=value; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Удалить все блокировки текущего юзера (какие есть в базе).
        /// В качестве сервиса данных используется переданный сервис данных.
        /// <param name="dataService">Сервис данных.</param>
        /// </summary>
        public static void ClearAllUserLocks(IDataService dataService)
        {
            var ds = dataService ?? DataServiceProvider.DataService;

            // убиваем все блокировки, оставшиеся с предыдущих времен
            SQLWhereLanguageDef lg = SQLWhereLanguageDef.LanguageDef;
            var vd = new VariableDef(lg.GetObjectTypeForNetType(typeof(string)), "UserName");

            Function func = lg.GetFunction(
                lg.funcEQ, vd, GetUserName() + (UseMachineNameInKey ? " @ " + Environment.MachineName : string.Empty));

            var lcs1 = new LoadingCustomizationStruct(0);

            var view = new View(typeof(LockData), View.ReadType.WithRelated);

            lcs1.Init(null, func, new[] { typeof(LockData) }, view, null);

            DataObject[] arr = ds.LoadObjects(lcs1);

            foreach (DataObject obj in arr)
            {
                obj.SetStatus(ObjectStatus.Deleted);
            }

            ds.UpdateObjects(ref arr);
        }

        /// <summary>
        /// Удалить все блокировки текущего юзера (какие есть в базе).
        /// В качестве сервиса данных используется DataServiceProvider.DataService.
        /// Если нужно использовать другой DataService, используйте другую перегрузку метода.
        /// </summary>
        public static void ClearAllUserLocks()
        {
            ClearAllUserLocks(DataServiceProvider.DataService);
        }

        /// <summary>
        /// The get user name.
        /// </summary>
        /// <returns>
        /// The get user name.
        /// </returns>
        [Obsolete("Use ICSSoft.Services.CurrentUserService.CurrentUser.FriendlyName instead")]
        public static string GetUserName()
        {
            return OldGetUserName();

            // return Services.CurrentUserService.CurrentUser.FriendlyName;
        }

        /// <summary>
        /// The set user name.
        /// </summary>
        /// <param name="newusername">
        /// The newusername.
        /// </param>
        [Obsolete("Use ICSSoft.Services.CurrentUserService.CurrentUser.FriendlyName instead")]
        public static void SetUserName(string newusername)
        {
            OldSetUserName(newusername);

            // Services.CurrentUserService.CurrentUser.FriendlyName = newusername;
        }

        /// <summary>
        /// удалить все текущие блокировки (осуществимые текущим экземпляром сервиса)
        /// </summary>
        public void ClearAllLocks()
        {
            for (int i = 0; i < AllLocks.Count; i++)
            {
                var l = (LockData)AllLocks.GetByIndex(i);
                ClearLock(l.LockKey, l.UserName);
                i--;
            }
        }

        /// <summary>
        /// Очистить блокировку
        /// </summary>
        /// <param name="LockKey">
        /// </param>
        /// <param name="userName">
        /// </param>
        public void ClearLock(string LockKey, string userName)
        {
            if (dataService == null)
            {
                throw new DataServiceNotFoundException();
            }

            var ld = new LockData();
            ld.LockKey = LockKey;
            ld.UserName = userName + (UseMachineNameInKey ? " @ " + Environment.MachineName : string.Empty);
            if (AllLocks.Contains(ld.CombinedKey()))
            {
                ld.SetStatus(ObjectStatus.Deleted);
                DataObject dobj = ld;
                dataService.UpdateObject(ref dobj);
                AllLocks.Remove(ld.CombinedKey());
            }
        }

        /// <summary>
        /// Очистить блокировку
        /// </summary>
        /// <param name="dobj">
        /// </param>
        /// <param name="userName">
        /// </param>
        public void ClearLock(DataObject dobj, string userName)
        {
            ClearLock(dobj.GetType().FullName + ":" + dobj.__PrimaryKey, userName);
        }

        /// <summary>
        /// Очистить блокировку
        /// </summary>
        /// <param name="LockKey">
        /// </param>
        public void ClearLock(string LockKey)
        {
            ClearLock(LockKey, GetUserName());
        }

        /// <summary>
        /// Очистить блокировку
        /// </summary>
        /// <param name="dobj">
        /// </param>
        public void ClearLock(DataObject dobj)
        {
            ClearLock(dobj, GetUserName());
        }

        /// <summary>
        /// Снять блокировку с объекта для текущего пользователя
        /// </summary>
        /// <param name="dobj">
        /// Объект данных
        /// </param>
        /// <param name="ds">
        /// Сервис данных
        /// </param>
        public void ClearWebLock(DataObject dobj, IDataService ds)
        {
            ClearWebLock(dobj, GetUserName(), ds);
        }

        /// <summary>
        /// Снять блокировку с объекта для указанного пользователя
        /// </summary>
        /// <param name="dobj">
        /// Объект данных
        /// </param>
        /// <param name="userName">
        /// Пользователь
        /// </param>
        /// <param name="ds">
        /// Сервис данных
        /// </param>
        public void ClearWebLock(DataObject dobj, string userName, IDataService ds)
        {
            ClearWebLock(dobj.GetType().FullName + ":" + dobj.__PrimaryKey, userName, ds);
        }

        /// <summary>
        /// Снять блокировку по ключу для указанного пользователя
        /// </summary>
        /// <param name="LockKey">
        /// ключ блокировки
        /// </param>
        /// <param name="userName">
        /// пользователь
        /// </param>
        /// <param name="ds">
        /// Сервис данных
        /// </param>
        public void ClearWebLock(string LockKey, string userName, IDataService ds)
        {
            var ld = new LockData();
            ld.LockKey = LockKey;
            ld.UserName = userName;
            try
            {
                ds.LoadObject(ld);
                ld.SetStatus(ObjectStatus.Deleted);
                DataObject dobj = ld;
                ds.UpdateObject(ref dobj);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Получить блокировку
        /// </summary>
        /// <param name="dobj">
        /// </param>
        /// <returns>
        /// The get lock.
        /// </returns>
        public string GetLock(DataObject dobj)
        {
            if (dataService == null)
            {
                throw new DataServiceNotFoundException();
            }

            var ld = new LockData();
            ld.LockKey = dobj.GetType().FullName + ":" + dobj.__PrimaryKey;
            ld.UserName = GetUserName() + (UseMachineNameInKey ? " @ " + Environment.MachineName : string.Empty);
            try
            {
                dataService.LoadObject(ld, false, true);
                return ld.UserName;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Получить блокировки (в т.ч. на детейлы).
        /// </summary>
        /// <param name="dobj">
        /// Для какого объекта спрашиваем блокировку.
        /// </param>
        /// <returns>
        /// Массив имен пользователей, для которых установлены блокировки на данный объект.
        /// </returns>
        public string[] GetLocks(DataObject dobj)
        {
            //bool f;
            return GetLocks(dobj, out _);
        }

        /// <summary>
        /// Получить блокировки
        /// </summary>
        /// <param name="dobj">
        /// Для какого объекта спрашиваем блокировку
        /// </param>
        /// <param name="retdailscontains">
        /// </param>
        /// <returns>
        /// </returns>
        public string[] GetLocks(DataObject dobj, out bool retdailscontains)
        {
            Type dotype = dobj.GetType();
            if (!viewsbytypes.Contains(dotype))
            {
                CreateViewForCheckLocks(dotype);
            }

            var view = (View)viewsbytypes[dotype];
            dataService.LoadObject(view, dobj, false, true);
            retdailscontains = false;
            foreach (DetailInView d in view.Details)
            {
                var dar = (DetailArray)Information.GetPropValueByName(dobj, d.Name);
                if (dar.Count > 0)
                {
                    retdailscontains = true;
                }
            }

            var locks = new StringCollection();
            GetAllLocks(dobj, locks, view);
            if (locks.Count > 0)
            {
                var res = new string[locks.Count];
                locks.CopyTo(res, 0);
                return res;
            }

            return new string[0];
        }

        /// <summary>
        /// Получить блокировку
        /// </summary>
        /// <param name="dobj">
        /// Объект данных, для которого получаем блокировку
        /// </param>
        /// <param name="ds">
        /// Сервис данных
        /// </param>
        /// <returns>
        /// The get web lock.
        /// </returns>
        public string GetWebLock(DataObject dobj, IDataService ds)
        {
            if (ds == null)
            {
                throw new DataServiceNotFoundException();
            }

            var ld = new LockData();
            ld.LockKey = dobj.GetType().FullName + ":" + dobj.__PrimaryKey;
            ld.UserName = GetUserName() + (UseMachineNameInKey ? " @ " + Environment.MachineName : string.Empty);
            try
            {
                ds.LoadObject(ld, false, true);
                return ld.UserName;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// The set lock.
        /// </summary>
        /// <param name="LockKey">
        /// The lock key.
        /// </param>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// The set lock.
        /// </returns>
        /// <exception cref="DataServiceNotFoundException">
        /// </exception>
        public string SetLock(string LockKey, string userName)
        {
            if (dataService == null)
            {
                throw new DataServiceNotFoundException();
            }

            var ld = new LockData();
            ld.LockKey = LockKey;
            ld.UserName = userName + (UseMachineNameInKey ? " @ " + Environment.MachineName : string.Empty);
            try
            {
                dataService.LoadObject(ld, false, true);
                return ld.UserName;
            }
            catch
            {
                DataObject dobj = ld;
                dataService.UpdateObject(ref dobj);
                if (!AllLocks.ContainsKey(ld.CombinedKey()))
                {
                    AllLocks.Add(ld.CombinedKey(), ld);
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// The set lock.
        /// </summary>
        /// <param name="dobj">
        /// The dobj.
        /// </param>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// The set lock.
        /// </returns>
        public string SetLock(DataObject dobj, string userName)
        {
            return SetLock(dobj.GetType().FullName + ":" + dobj.__PrimaryKey, userName);
        }

        /// <summary>
        /// The set lock.
        /// </summary>
        /// <param name="LockKey">
        /// The lock key.
        /// </param>
        /// <returns>
        /// The set lock.
        /// </returns>
        public string SetLock(string LockKey)
        {
            return SetLock(LockKey, GetUserName());
        }

        /// <summary>
        /// The set lock.
        /// </summary>
        /// <param name="dobj">
        /// The dobj.
        /// </param>
        /// <returns>
        /// The set lock.
        /// </returns>
        public string SetLock(DataObject dobj)
        {
            return SetLock(dobj, GetUserName());
        }

        /// <summary>
        /// Установить блокировку на объект
        /// </summary>
        /// <param name="dobj">
        /// Объект данны
        /// </param>
        /// <param name="ds">
        /// Сервис данных
        /// </param>
        /// <returns>
        /// Имя пользователя, под которым блокировка находится,
        /// string.Empty - если блокировки не было и мы её только поставили
        /// </returns>
        public string SetWebLock(DataObject dobj, IDataService ds)
        {
            return SetWebLock(dobj, GetUserName(), ds);
        }

        /// <summary>
        /// Установить блокировку на объект для указанного пользователя
        /// </summary>
        /// <param name="dobj">
        /// Объект данных
        /// </param>
        /// <param name="userName">
        /// Пользователь
        /// </param>
        /// <param name="ds">
        /// Сервис данных
        /// </param>
        /// <returns>
        /// Имя пользователя, под которым блокировка находится,
        /// string.Empty - если блокировки не было и мы её только поставили
        /// </returns>
        public string SetWebLock(DataObject dobj, string userName, IDataService ds)
        {
            return SetWebLock(dobj.GetType().FullName + ":" + dobj.__PrimaryKey, userName, ds);
        }

        /// <summary>
        /// Установить блокировку
        /// </summary>
        /// <param name="LockKey">
        /// Ключ
        /// </param>
        /// <param name="userName">
        /// Пользователь
        /// </param>
        /// <param name="ds">
        /// Сервис данных
        /// </param>
        /// <returns>
        /// Имя пользователя, под которым блокировка находится,
        /// string.Empty - если блокировки не было и мы её только поставили
        /// </returns>
        public string SetWebLock(string LockKey, string userName, IDataService ds)
        {
            var ld = new LockData();
            ld.LockKey = LockKey;
            ld.UserName = userName;
            try
            {
                // try/catch это не является хорошим вариантом
                // по соображениям производительности,
                // поэтому, в целях оптимизации скорее всего
                // придётся переписать данный код на цивилизованную проверку
                // наличия блокировок
                ds.LoadObject(ld, false, true);
                return ld.UserName;
            }
            catch
            {
                DataObject dobj = ld;
                ds.UpdateObject(ref dobj);
                return string.Empty;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create view for check locks.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        private static void CreateViewForCheckLocks(Type type)
        {
            var view = new View(type, View.ReadType.WithRelated);
            LightingView(view);
            viewsbytypes.Add(type, view);
        }

        /// <summary>
        /// The lighting view.
        /// </summary>
        /// <param name="view">
        /// The view.
        /// </param>
        private static void LightingView(View view)
        {
            view.Properties = new[] { new PropertyInView("__PrimaryKey", string.Empty, false, string.Empty,0,String.Empty) };
            foreach (DetailInView div in view.Details)
            {
                LightingView(div.View);
            }
        }

        /// <summary>
        /// The old get user name.
        /// </summary>
        /// <returns>
        /// The old get user name.
        /// </returns>
        private static string OldGetUserName()
        {
            //if (HttpContext.Current != null)
            //{
            //    return HttpContext.Current.User.Identity.Name;
            //}

            //if (username == string.Empty)
            //{
            //    try
            //    {
            //        var ds = new DirectorySearcher(
            //            "(&(objectClass=user)(sAMAccountName= " + Environment.UserName + "))", new[] { "cn" })
            //        {
            //            CacheResults = true
            //        };
            //        SearchResult sr = ds.FindOne();
            //        username = sr.Properties["cn"][0].ToString();
            //    }
            //    catch
            //    {
            //        username = Environment.UserName;
            //    }
            //}

            //return username;
            return null;
        }

        /// <summary>
        /// The old set user name.
        /// </summary>
        /// <param name="newusername">
        /// The newusername.
        /// </param>
        private static void OldSetUserName(string newusername)
        {
            username = newusername;
        }

        /// <summary>
        /// The get all locks.
        /// </summary>
        /// <param name="dobj">
        /// The dobj.
        /// </param>
        /// <param name="locks">
        /// The locks.
        /// </param>
        /// <param name="view">
        /// The view.
        /// </param>
        private void GetAllLocks(DataObject dobj, StringCollection locks, View view)
        {
            string res = GetLock(dobj);
            if (res != string.Empty)
            {
                locks.Add(res);
            }

            foreach (DetailInView d in view.Details)
            {
                var dar = (DetailArray)Information.GetPropValueByName(dobj, d.Name);
                foreach (DataObject detobj in dar)
                {
                    GetAllLocks(detobj, locks, d.View);
                }
            }
        }

        public void ClearAllUserLocks(string userName)
        {
            throw new NotImplementedException();
        }

        public LockInfo LockObject(object dataObjectKey, string userName)
        {
            throw new NotImplementedException();
        }

        public LockInfo GetLock(object dataObjectKey)
        {
            throw new NotImplementedException();
        }

        public bool UnlockObject(object dataObjectKey)
        {
            throw new NotImplementedException();
        }

        public void UnlockAllObjects()
        {
            throw new NotImplementedException();
        }
       

        #endregion
    }

}