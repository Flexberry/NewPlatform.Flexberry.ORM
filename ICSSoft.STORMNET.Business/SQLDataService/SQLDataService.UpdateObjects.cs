namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Data service for SQL storage.
    /// </summary>
    public abstract partial class SQLDataService : System.ComponentModel.Component, IDataService
    {
        /// <summary>
        /// Обновить хранилище по объектам (есть параметр, указывающий, всегда ли необходимо взводить ошибку
        /// и откатывать транзакцию при неудачном запросе в базу данных). Если
        /// он true, всегда взводится ошибка. Иначе, выполнение продолжается.
        /// Однако, при этом есть опасность преждевременного окончания транзакции, с переходом для остальных
        /// запросов режима транзакционности в autocommit. Проявлением проблемы являются ошибки навроде:
        /// The COMMIT TRANSACTION request has no corresponding BEGIN TRANSACTION.
        /// </summary>
        /// <param name="objects">Объекты для обновления.</param>
        /// <param name="DataObjectCache">Кэш объектов данных.</param>
        /// <param name="AlwaysThrowException">Если произошла ошибка в базе данных, не пытаться выполнять других запросов, сразу взводить ошибку и откатывать транзакцию.</param>
        public virtual void UpdateObjects(ref DataObject[] objects, DataObjectCache DataObjectCache, bool AlwaysThrowException)
        {
            if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
            {
                var tps = new List<Type>();
                foreach (DataObject d in objects)
                {
                    Type t = d.GetType();
                    if (!tps.Contains(t))
                    {
                        tps.Add(t);
                    }
                }

                string cs = ChangeCustomizationString(tps.ToArray());
                customizationString = string.IsNullOrEmpty(cs) ? customizationString : cs;
            }

            using (DbTransactionWrapper dbTransactionWrapper = new DbTransactionWrapper(this))
            {
                try
                {
                    UpdateObjectsByExtConn(ref objects, DataObjectCache, AlwaysThrowException, dbTransactionWrapper);
                    dbTransactionWrapper.CommitTransaction();
                }
                catch (Exception)
                {
                    dbTransactionWrapper.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// Обновить хранилище по объектам. При ошибках делается попытка возобновления транзакции с другого запроса,
        /// т.к. предполагается, что запросы должны быть выполнены в другом порядке.
        /// </summary>
        /// <param name="objects">Объекты данных для обновления.</param>
        /// <param name="DataObjectCache">Кэш объектов данных.</param>
        public virtual void UpdateObjects(ref DataObject[] objects, DataObjectCache DataObjectCache)
        {
            UpdateObjects(ref objects, DataObjectCache, false);
        }

        /// <summary>
        /// Обновить хранилище по объектам.
        /// </summary>
        /// <param name="objects">Объекты данных для обновления.</param>
        public virtual void UpdateObjects(ref DataObject[] objects)
        {
            UpdateObjects(ref objects, new DataObjectCache());
        }

        /// <summary>
        /// Обновить хранилище по объектам.
        /// </summary>
        /// <param name="objects">Объекты данных для обновления.</param>
        /// <param name="AlwaysThrowException">Если произошла ошибка в базе данных, не пытаться выполнять других запросов, сразу взводить ошибку и откатывать транзакцию.</param>
        public virtual void UpdateObjects(ref DataObject[] objects, bool AlwaysThrowException)
        {
            UpdateObjects(ref objects, new DataObjectCache(), AlwaysThrowException);
        }
    }
}
