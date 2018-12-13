namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.DirectoryServices;
    using System.Web;

    using ICSSoft.STORMNET.Collections;
    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

    /// <summary>
    /// Классический сервис блокировок
    /// </summary>
    public class LockService
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
            bool f;
            return GetLocks(dobj, out f);
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
            view.Properties = new[] { new PropertyInView("__PrimaryKey", string.Empty, false, string.Empty) };
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
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.User.Identity.Name;
            }

            if (username == string.Empty)
            {
                try
                {
                    var ds = new DirectorySearcher(
                        "(&(objectClass=user)(sAMAccountName= " + Environment.UserName + "))", new[] { "cn" })
                        {
                           CacheResults = true
                        };
                    SearchResult sr = ds.FindOne();
                    username = sr.Properties["cn"][0].ToString();
                }
                catch
                {
                    username = Environment.UserName;
                }
            }

            return username;
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

        #endregion
    }
}