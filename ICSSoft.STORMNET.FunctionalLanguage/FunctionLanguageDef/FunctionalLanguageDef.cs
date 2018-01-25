namespace ICSSoft.STORMNET.FunctionalLanguage
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// Определение языка ограничений для конструирования ограничивающих функций
    /// </summary> 
    [NotStored]
    [Serializable]
    public abstract class FunctionalLanguageDef : DataObject
    {
        private DetailArrayOfObjectType _fieldTypes;
        private DetailArrayOfVariableDef _fieldVariables;
        private DetailArrayOfFunctionDef _fieldFunctions;

        /// <summary>
        /// Имя сборки ICSSoft.STORMNET.UI, где раньше лежали классы ExternalLangDef.
        /// </summary>
        private const string UilibraryName = "ICSSoft.STORMNET.UI";

        /// <summary>
        /// Имя сборки ExternalLangDef, где раньше лежали только ссылки на классы ExternalLangDef.
        /// </summary>
        private const string ExternalLangDefLibraryName = "ExternalLangDef";

        /// <summary>
        /// Индекс последней функции в списке
        /// </summary>
        public virtual int MaxFuncID { get { return 0; } }

        /// <summary>
        /// Получить определение функции
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual FunctionDef GetFunctionDef(int id)
        {
            foreach (FunctionDef fd in Functions)
            {
                if (fd.ID == id)
                    return fd;
            }

            throw new Exception("Неизвестный идентификатор сериализованной функции." +
                                "Возможно, следует добавить в проект ссылку на сборку ExternalLangDef");
        }

        /// <summary>
        /// Получить определение функции по его строковому представлению
        /// </summary>
        /// <param name="stringedView">Строковое представление идентифицирующие определение функции</param>
        /// <returns>Найденное строковое представление. Выдает исключение в случае неудачи, поэтому результат не может быть пустым.</returns>
        public virtual FunctionDef GetFunctionDefByStringedView(string stringedView)
        {
            foreach (FunctionDef fd in Functions)
            {
                if (fd.StringedView == stringedView)
                    return fd;
            }

            throw new Exception("Неизвестное строковое представление сериализованной функции." +
                                "Возможно, следует добавить в проект ссылку на сборку ExternalLangDef");
        }

        /// <summary>
        /// Получить наше описание типа по имени
        /// </summary>
        /// <param name="typeName">Имя типа</param>
        /// <returns></returns>
        public virtual ObjectType GetObjectType(string typeName)
        {
            foreach (ObjectType ot in Types)
            {
                if (ot.StringedView == typeName)
                    return ot;
            }

            return null;
        }

        /// <summary>
        /// Разбор функции "по-косточкам" в специальный массив
        /// </summary>
        /// <param name="f">функция</param>
        /// <returns>new object[] { f.FunctionDef.ID, pars, types }</returns>
        public object FunctionToSimpleStruct(Function f)
        {
            var pars = new ArrayList();
            var types = new ArrayList();
            int parCount = f.Parameters.Count;
            DetailArrayOfFunctionalParameterDef functionalParameterDef = f.FunctionDef.Parameters;
            int fParDefCount = functionalParameterDef.Count;
            for (int i = 0; i < parCount; i++)
            {
                object par = f.Parameters[i];
                if (par is Function)
                {
                    types.Add("Func");
                    pars.Add(FunctionToSimpleStruct(par as Function));
                }
                else if (par is VariableDef)
                {
                    types.Add(par.GetType().AssemblyQualifiedName);
                    pars.Add((par as VariableDef).ToSimpleValue());
                }
                else
                {
                    types.Add(null);
                    FunctionParameterDef fpd = (i < fParDefCount) ? functionalParameterDef[i] : functionalParameterDef[fParDefCount - 1];
                    pars.Add(fpd.Type.ValueToSimpleValue(par));
                }
            }
            return new object[] { f.FunctionDef.ID, pars, types };
        }

        /// <summary>
        /// Восстановление функции из простой структуры 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public Function FunctionFromSimpleStruct(object val)
        {
            var vals = (object[])val;
            var f = new Function();
            f.FunctionDef = GetFunctionDef((int)vals[0]);
            var pars = (ArrayList)vals[1];
            var types = (ArrayList)vals[2];
            int count = pars.Count;
            DetailArrayOfFunctionalParameterDef functionalParameterDef = f.FunctionDef.Parameters;
            int fpdCount = functionalParameterDef.Count;
            for (int i = 0; i < count; i++)
            {
                object type = types[i];
                object par = pars[i];
                if (type == null)
                {
                    FunctionParameterDef fpd = (i < fpdCount) ? functionalParameterDef[i] : functionalParameterDef[fpdCount - 1];
                    f.Parameters.Add(fpd.Type.SimpleValueToValue(par));
                }
                else if ((string)type == "Func")
                {
                    f.Parameters.Add(FunctionFromSimpleStruct(par));
                }
                else
                {
                    string stype = (string)type;
                    // by fat
                    // это из за того, что раньше адв лимит лежал отдельно, а сейчас в уи
                    // для поддержки ранее созданных фильтров. Криво конечно, но что поделаешь...
                    stype = stype.Replace("ICSSoft.STORMNET.Windows.Forms.AdvLimit", "ICSSoft.STORMNET.UI");
                    stype = stype.Replace("ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null", "ICSSoft.STORMNET.UI");
                    Type tp;
                    try
                    {
                        tp = Tools.AssemblyLoader.GetTypeWithAssemblyName(stype);
                        
                        // Обеспечение обратной совместимости: в старых версиях в первую очередь сборку необходимо искать в другом месте.
                        Type oldType = TryGetOldType(stype, false);
                        if (oldType != null)
                        {
                            tp = oldType;
                        }
                    }
                    catch (Exception)
                    {
                        if (stype.Contains(UilibraryName))
                        {
                            /* Некоторые типы с пространством имён "ICSSoft.STORMNET.Windows.Forms"
                             * были перенесены из сборки ICSSoft.STORMNET.UI в ExternalLangDef,
                             * поэтому для обратной совместимости иногда может потребоваться следующий вызов.
                             */
                            tp = Tools.AssemblyLoader.GetTypeWithAssemblyName(
                                stype.Replace(UilibraryName, ExternalLangDefLibraryName));
                        }
                        else if (stype.Contains(ExternalLangDefLibraryName))
                        {
                            /* Данная конвертация сделана для обратной совместимости:
                             * если подложить данную сборку в старую версию, то можно будет открывать ограничения в новом формате.
                             */
                            tp = TryGetOldType(stype, true);
                        }
                        else
                        {
                            throw;
                        }
                    }

                    var vd = (VariableDef)Activator.CreateInstance(tp);
                    vd.FromSimpleValue(par, this);
                    f.Parameters.Add(vd);
                }
            }
            return f;
        }

        /// <summary>
        /// Попытка получения типа из UI-сборки вместо ExternalLangDef (для обратной совместимости после переноса классов из UI в ExternalLangDef).
        /// </summary>
        /// <param name="typeName">Исходное имя типа.</param>
        /// <param name="throwException">Следует ли пробрасывать исключение, если тип не удалось найти.</param>
        /// <returns>Найденный тип (или null, если в UI такой сборки уже нет).</returns>
        private static Type TryGetOldType(string typeName, bool throwException)
        {
            try
            {
                if (typeName.Contains(ExternalLangDefLibraryName))
                {
                    /* Данная конвертация сделана для обратной совместимости:
                     * если подложить данную сборку в старую версию, то можно будет открывать ограничения в новом формате.
                     */
                    return Tools.AssemblyLoader.GetTypeWithAssemblyName(
                        typeName.Replace(ExternalLangDefLibraryName, UilibraryName));
                }
            }
            catch (Exception)
            {
                if (throwException)
                {
                    throw;
                }
            }

            return null;
        }
        
        /// <summary>
        /// Тип функции для возврата значения
        /// </summary>
        protected ObjectType fieldUpFunctionType;

        /// <summary>
        /// Типы (Детейл)
        /// </summary>
        public DetailArrayOfObjectType Types { get { return _fieldTypes; } set { _fieldTypes = value; } }

        /// <summary>
        /// Переменные (Детейл)
        /// </summary>
        public DetailArrayOfVariableDef Variables { get { return _fieldVariables; } set { _fieldVariables = value; } }
        
        /// <summary>
        /// Функции (Детейл)
        /// </summary>
        public DetailArrayOfFunctionDef Functions { get { return _fieldFunctions; } set { _fieldFunctions = value; } }
        
        /// <summary>
        /// Тип функции для возврата значения
        /// </summary>
        public ObjectType UpFunctionType { get { return fieldUpFunctionType; } }

        /// <summary>
        /// Получатель ObjectType по .NET-типу (для DataObject возвращается тип первичного ключа)
        /// </summary>
        /// <param name="type">.NET-тип</param>
        /// <returns>ObjectType-тип</returns>
        public virtual ObjectType GetObjectTypeForNetType(Type type)
        {
            foreach (ObjectType t in Types)
                if (t.NetCompatibilityType == type)
                    return t;
            foreach (ObjectType t in Types)
                if (CompatibilityTypeTest.Check(type, t.NetCompatibilityType) != TypesCompatibilities.No)
                    return t;
            return null;
        }

        /// <summary>
        /// Конструктор (вызывается InitializeDefs())
        /// </summary>
        public FunctionalLanguageDef()
        {
            _fieldTypes = new DetailArrayOfObjectType(this);
            _fieldVariables = new DetailArrayOfVariableDef(this);
            _fieldFunctions = new DetailArrayOfFunctionDef(this);
            InitializeDefs();
        }

        /// <summary>
        /// Инициализация определений функций языка (для определения связки количества и типов параметров)
        /// </summary>
        abstract protected void InitializeDefs();

        /// <summary>
        /// Не найдена функция по сигнатуре
        /// </summary>
        public class NotFoundFunctionBySignatureException : Exception
        {
            /// <summary>
            /// 
            /// </summary>
            public NotFoundFunctionBySignatureException() { }
        }

        /// <summary>
        /// Не найден параметр функции
        /// </summary>
        public class NotFoundFunctionParametersException : Exception
        {
            /// <summary>
            /// 
            /// </summary>
            public NotFoundFunctionParametersException() { }
        }

        /// <summary>
        /// Список функций с ключом в виде строкового определения
        /// </summary>
        [XmlIgnore]
        [NonSerialized]
        public SortedList FunctionsByStringedViewList = new SortedList();


        [XmlArray("FunctionsByStringedViewList")]
        //[XmlIgnore]
        public FunctionDef[] _S_FunctionsByStringedViewList
        {
            get
            {
                //подсмотрено тут https://social.msdn.microsoft.com/Forums/en-US/f5a79f67-28d4-4dd6-9deb-5356ab43060f/problem-with-serialisation-of-sortedlist-?forum=csharpgeneral
                ArrayList[] funcs = new ArrayList[FunctionsByStringedViewList.Count];


                ArrayList result = new ArrayList();
                foreach (ArrayList arr in FunctionsByStringedViewList.Values)
                {
                    foreach (FunctionDef f in arr)
                        result.Add(f);
                }
                return (FunctionDef[]) result.ToArray();
            }
            set
            {
                FunctionsByStringedViewList.Clear();
                for (int i = 0; i < value.Length; i++)
                {
                    FunctionDef f = (FunctionDef)value[i];
                    if (!FunctionsByStringedViewList.ContainsKey(f.StringedView))
                        FunctionsByStringedViewList.Add(f.StringedView, new ArrayList());
                    ((ArrayList)FunctionsByStringedViewList[f.StringedView]).Add(f);
                }
            }
        }


        private static string m_objNull = "CONST";

        /// <summary>
        /// Инициализировать массив функции с ключом в виде строкового определения
        /// </summary>
        public void InitFunctionsByStringedViewList()
        {
            lock (m_objNull)
            {
                //1. ищем все функции для данной Function String
                if (FunctionsByStringedViewList.Count == 0)
                {
                    foreach (FunctionDef f in _fieldFunctions)
                    {
                        if (!FunctionsByStringedViewList.ContainsKey(f.StringedView))
                            FunctionsByStringedViewList.Add(f.StringedView, new ArrayList());
                        ((ArrayList)FunctionsByStringedViewList[f.StringedView]).Add(f);
                    }
                }
            }
        }

        private SortedList FunctionsByParametersTypes = new SortedList();

        /// <summary>
        /// Создание ограничивающей функции
        /// </summary>
        /// <param name="functionString">Функция (langdef.funcEQ, например)</param>
        /// <param name="parameters">Параметры. Например, new VariableDef(langdef.StringType, "Фамилия"), "Иванов"</param>
        /// <returns>Ограничивающая функция</returns>
        public virtual Function GetFunction(string functionString, params object[] parameters)
        {
            InitFunctionsByStringedViewList();

            if (!FunctionsByStringedViewList.ContainsKey(functionString))
                throw new NotFoundFunctionBySignatureException();

            if ((functionString == "=" || functionString == "<>") && parameters.Count() == 2)
            { // В этом месте нет возможности получить доступ к привычным константам ExternalLangDef
                if (parameters[0] == null && parameters[1] == null)
                { // По сути null == null (True) или null != null (NOT(True))
                    return functionString == "=" ? GetFunction("True") : GetFunction("NOT", GetFunction("True"));
                }

                object[] parametersNew = null;
                if (parameters[0] == null && parameters[1] != null)
                {
                    parametersNew = new object[] { parameters[1] };
                }
                else if (parameters[0] != null && parameters[1] == null)
                {
                    parametersNew = new object[] { parameters[0] };
                }

                if (parametersNew != null)
                { // По сути "Сущность" is null или "Сущность" is not null
                    return GetFunction(functionString == "=" ? "ISNULL" : "NOTISNULL", parametersNew);
                }
            }

            var keySB = new StringBuilder(functionString);
            keySB.Append(";");
            foreach (object prm in parameters)
            {
                var partype = new StringBuilder();
                if (prm is Function)
                {
                    partype.Append("FunctionType(");
                    partype.Append((prm as Function).FunctionDef.ReturnType.StringedView);
                    partype.Append(")");
                }
                else if (prm is VariableDef)
                {
                    partype.Append("FunctionType(");
                    partype.Append((prm as VariableDef).Type.StringedView);
                    partype.Append(")");
                }
                else
                {
                    if (prm == null)
                    { // Без этого кидается exception, по которому сложно определить проблему
                        throw new NullReferenceException("В конструктор функции ограничения передан null в качестве параметра в недопустимой позиции.");
                    }

                    partype.Append(prm.GetType().FullName);
                }

                keySB.Append(partype.ToString());
                keySB.Append(";");
            }
            string key = keySB.ToString();
            if (FunctionsByParametersTypes.ContainsKey(key))
            {
                var fd = (FunctionDef)FunctionsByParametersTypes[key];
                var f = new Function(fd, parameters);
                return f;
            }

            lock (m_objNull)
            {
                if (FunctionsByParametersTypes.ContainsKey(key))
                {
                    var fd = (FunctionDef)FunctionsByParametersTypes[key];
                    var f = new Function(fd, parameters);
                    return f;
                }

                var functionArrList = (ArrayList)FunctionsByStringedViewList[functionString];
                for (int i = 0; i < functionArrList.Count; i++)
                {
                    var fd = (FunctionDef)functionArrList[i];
                    var f = new Function(fd, parameters);
                    if (f.CheckWithoutSubFoldersSafetly())
                    {
                        FunctionsByParametersTypes.Add(key, fd);
                        return f;
                    }
                }
            }

            throw new NotFoundFunctionParametersException();
        }
    }

    /// <summary>
    /// DetailArray Of VariableDef
    /// </summary>
    public class DetailArrayOfVariableDef : DetailArray
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="masterObj"></param>
        public DetailArrayOfVariableDef(FunctionalLanguageDef masterObj)
            : base(typeof(VariableDef), masterObj)
        { }

        /// <summary>
        /// return (VariableDef)ItemByIndex(index);
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VariableDef this[int index]
        {
            get { return (VariableDef)ItemByIndex(index); }
        }
    }

    /// <summary>
    /// DetailArray Of FunctionDef
    /// </summary>
    public class DetailArrayOfFunctionDef : DetailArray
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="masterObj"></param>
        public DetailArrayOfFunctionDef(FunctionalLanguageDef masterObj)
            : base(typeof(FunctionDef), masterObj)
        { }

        /// <summary>
        /// return (FunctionDef)ItemByIndex(index);
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FunctionDef this[int index]
        {
            get { return (FunctionDef)ItemByIndex(index); }
        }
    }

    /// <summary>
    /// DetailArray Of ObjectType
    /// </summary>
    public class DetailArrayOfObjectType : DetailArray
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="masterObj"></param>
        public DetailArrayOfObjectType(FunctionalLanguageDef masterObj)
            : base(typeof(ObjectType), masterObj)
        { }
        /// <summary>
        ///  return (ObjectType)ItemByIndex(index);
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ObjectType this[int index]
        {
            get { return (ObjectType)ItemByIndex(index); }
        }

        //чтобы работала XML сериализация
        public void Add(Object dataobject) 
        {
            base.Add(dataobject);
        }
    }


}
