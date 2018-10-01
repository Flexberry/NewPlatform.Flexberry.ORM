namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Linq;

    /// <summary>
    /// Базовый абстрактный класс для всех бизнес-серверов.
    /// </summary>
    public abstract class BusinessServer
    {
        /// <summary>
        /// Объект, предназначенный для создания новых объектов. При удаленной работе настраивается на создание объектов
        /// на клиенте.
        /// </summary>
        private ObjectCreator m_objObjectCreator = new ObjectCreator();

        private IDataService m_objDataService;

        private System.Collections.ArrayList m_arrObjectsToUpdate;

        private int _order = 0;

        /// <summary>
        /// Упорядочение бизнес-серверов. 0 - выполнится раньше остальных, int.MaxValue - выполнится последним. По-умолчанию: 0.
        /// </summary>
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }


        private DataObjectCache _dataObjectCache = null;

        /// <summary>
        /// Кэш, установливается при инициализации для использования сервиса
        /// </summary>
        public DataObjectCache DataObjectCache {
            get { if (_dataObjectCache == null)
                { _dataObjectCache = new DataObjectCache();_dataObjectCache.StartCaching(false); } return _dataObjectCache; }
            set { _dataObjectCache = value; } }

        /// <summary>
        /// Ссылка на обновляемые объекты (устанавливается сервисом данных).
        /// </summary>
        public System.Collections.ArrayList ObjectsToUpdate
        {
            get
            {
                return m_arrObjectsToUpdate;
            }
            set
            {
                m_arrObjectsToUpdate = value;
            }
        }

        /// <summary>
        /// Сервис данных, на котором сработает этот Бизнес-сервер.
        /// </summary>
        public virtual IDataService DataService
        {
            get
            {
                return m_objDataService ?? DataServiceProvider.DataService;
            }

            set
            {
                m_objDataService = value;
            }
        }


        /// <summary>
        /// Установить "создаватель" объектов.
        /// </summary>
        /// <param name="creator">Устанавливаемый "создаватель" объектов.</param>
        public void SetCreator(ObjectCreator creator)
        {
            m_objObjectCreator = creator;

            DataObjectCache.Creator = creator;
        }

        private System.Reflection.MethodInfo Method = null;

        /// <summary>
        /// Определяем метод, в который записан бизнес-сервер для типа объекта. 
        /// Например, для класса "Журнал", это будет "OnUpdateЖурнал" с определённой сигнатурой.
        /// </summary>
        /// <param name="objectType">
        /// Тип объектов, для которого ищем бизнес-сервер.
        /// </param>
        public void SetType(Type objectType)
        {
            const string BsMethodNameStart = "OnUpdate";
            foreach (var methodInfo in GetType().GetMethods())
            {
                string methodinfoName = methodInfo.Name;
                System.Reflection.ParameterInfo[] ps;

                if (methodinfoName.StartsWith(BsMethodNameStart) 
                    && methodInfo.ReturnType == typeof(DataObject[])
                    && (ps = methodInfo.GetParameters()).Length == 1
                    && (
                        (ps[0].ParameterType.IsSubclassOf(typeof(DataObject)) && ps[0].ParameterType == objectType) // Это на случай, если BS навешен непосредственно на класс или его предка.
                        || (ps[0].ParameterType.IsInterface && objectType.GetInterfaces().Contains(ps[0].ParameterType))) // Это на случай, если BS навешен на интерфейс его предка.
                    && BsMethodNameStart + ps[0].ParameterType.Name == methodinfoName 
                    && ps[0].Name == "UpdatedObject")
                {
                    Method = methodInfo;
                }
            }
        }

        public BusinessServer()
        {
        }

        /// <summary>
        /// Вызвать действия привязанные на события при сохранении объекта.
        /// </summary>
        /// <param name="UpdateObject">Сам объект.</param>
        /// <returns>Что еще поменялось.</returns>
        public DataObject[] OnUpdateDataobject(DataObject UpdateObject)
        {
            if (Method == null)
                return new DataObject[0];
            try
            {
                return (DataObject[])Method.Invoke(this, new object[] { UpdateObject });
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    Exception ex = e.InnerException;
                    if (ex.GetType() == typeof(Exception))
                        throw ex;
                    if (ex.GetType().FullName.StartsWith("System."))
                        throw e;
                    throw ex;
                }

                throw e;    
            }
        }

        /// <summary>
        /// Создать новый объект. Все объекты, в дальнейшем передаваемые на клиента должны быть созданы через данный метод.
        /// В этом случае эти объекты будут создаваться на клиенте.
        /// </summary>
        /// <param name="type">Тип объекта данных.</param>
        /// <returns>Созданный объект.</returns>
        protected object prv_CreateObject(Type type)
        {
            return m_objObjectCreator.CreateObject(type);
        }

        /// <summary>
        /// Скопировать объект данных при работе с ремоутингом (т.к. CopyTo в этом случае не срабатывает)
        /// Объект datadest должен быть загружен также, как и datasource.
        /// </summary>
        /// <param name="datasource">
        /// Исходный объект данных.
        /// </param>
        /// <param name="datadest">
        /// Целевой объект данных.
        /// </param>
        protected void prv_CopyDataObject(DataObject datasource, DataObject datadest)
        {
            DataObject obj = datadest;

            DataObject dataobj = datasource;

            obj.__PrimaryKey = dataobj.__PrimaryKey;

            string[] lp = dataobj.GetLoadedProperties();

            foreach (string propname in lp)
            {
                bool bcopy = true;

                object val = Information.GetPropValueByName(dataobj, propname);

                if (val != null)
                {
                    if (val is DataObject)
                    {
                        // Master.
                        DataObject val2 = (DataObject)DataObjectCache.Creator.CreateObject(val.GetType());

                        val2.SetExistObjectPrimaryKey((val as DataObject).__PrimaryKey);

                        Information.SetPropValueByName(obj, propname, val2);
                        
                        // Тут ничего не делаем.
                        bcopy = false;
                    }
                    else if (val is DetailArray)
                    {
                        DetailArray det = (DetailArray)Information.GetPropValueByName(obj, propname);

                        foreach (DataObject line in (DetailArray)val)
                        {
                            DataObject destline = det.GetByKey(line.__PrimaryKey);

                            if (destline == null)
                            {
                                destline = (DataObject)DataObjectCache.Creator.CreateObject(line.GetType());

                                destline.SetExistObjectPrimaryKey(line.__PrimaryKey);

                                destline.SetStatus(ObjectStatus.Created);

                                det.AddObject(destline);
                            }

                            prv_CopyDataObject(line, destline);
                        }

                        foreach (DataObject ln in det)
                        {
                            DataObject dd = (val as DetailArray).GetByKey(ln.__PrimaryKey);

                            if (dd == null)
                            {
                                ln.SetStatus(ObjectStatus.Deleted);
                            }
                        }

                        // Тут ничего не делаем.
                        bcopy = false;
                    }
                }

                if (bcopy)
                {
                    // просто пихаем сюда
                    Information.SetPropValueByName(obj, propname, val);
                }
            }

            foreach (string key in dataobj.DynamicProperties.GetAllKeys())
            {
                obj.DynamicProperties.Add(key, dataobj.DynamicProperties[key]);
            }
        }
    }
}
