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

    /// <summary>
    /// Интерфейс для сервисов пессиместических блокировок
    /// </summary>
    public interface ILockService
    {
        /// <summary>
        /// Сервис данных, с которым нужно пойти в базу данных с блокировками
        /// </summary>
        IDataService DataService { get; set; }

        /// <summary>
        /// Убить все блокировки пользователя
        /// </summary>
        /// <param name="userName"></param>
        void ClearAllUserLocks(string userName);

        /// <summary>
        /// Убить указанную блокировку для указанного пользователя
        /// </summary>
        /// <param name="lockName"></param>
        /// <param name="userName"></param>
        void ClearLock(string lockName, string userName);

        /// <summary>
        /// Устанавливает блокировку. Если удачно - то возвращается пустая строка, если нет - UserName.
        /// </summary>
        /// <returns>Имя пользователя, которым занят ресурс</returns>
        string SetLock(string lockName, string userName);
    }

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
                ld.GetFunction(ld.funcEQ, new VariableDef(ld.StringType, "STORMMainObjectKey"), lockName),
                ld.GetFunction(ld.funcEQ, new VariableDef(ld.StringType, "UserName"), userName)
                ));
            }
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
        /// Устанавливает блокировку. Если удачно - то возвращается пустая строка, если нет - UserName.
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
                    lcs.LimitFunction = ld.GetFunction(ld.funcEQ, new VariableDef(ld.StringType, "STORMMainObjectKey"), lockName);

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
    }

}