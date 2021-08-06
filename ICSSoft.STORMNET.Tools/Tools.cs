namespace ICSSoft.STORMNET.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Collections.Specialized;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text.RegularExpressions;
    using System.Xml;

    using ICSSoft.STORMNET.Collections;

    /// <summary>
    /// Инструмент для сериализации-десериализации в XML.
    /// </summary>
    public class ToolXML
    {
        /// <summary>
        ///
        /// </summary>
        public ToolXML()
        {
        }

        /// <summary>
        /// Получение объекта данных из ранее полученного XML документа.
        /// </summary>
        /// <param name="dataObject"> Объект данных, в который будем десериализовывать. </param>
        /// <param name="xmlDoc"> Сериализованный объект данных. </param>
        public static void XMLDocument2DataObject(ref ICSSoft.STORMNET.DataObject dataObject, XmlDocument xmlDoc)
        {
            if (dataObject == null)
            {
                throw new ArgumentNullException("dataObject");
            }

            var dataObjectCache = new DataObjectCache();
            dataObjectCache.StartCaching(false);
            try
            {
                var xmlMainEl = (XmlElement)xmlDoc.FirstChild;

                XmlNode xmlNode = xmlMainEl.SelectSingleNode("Assemblies");
                if (xmlNode != null)
                {
                    XmlNodeList xmlAssemblies = xmlNode.ChildNodes;

                    var assemblies = new SortedList();
                    for (int i = 0; i < xmlAssemblies.Count; i++)
                    {
                        assemblies.Add(xmlAssemblies[i].Name, ((XmlElement)xmlAssemblies[i]).GetAttribute("Assembly"));
                    }

                    var xmlEl = (XmlElement)xmlMainEl.FirstChild;
                    if (xmlEl.Name == "Assemblies")
                    {
                        xmlEl = (XmlElement)xmlMainEl.LastChild;
                    }

                    prv_XmlElement2DataObject(xmlEl, dataObject, assemblies, dataObjectCache, new Dictionary<string, DataObject>());
                }
                else
                {
                    throw new Exception("Не найдено описание подключаемых сборок в сериализованном объекте");
                }
            }
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        /// <summary>
        /// Получение XML документа из объекта данных.
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static XmlDocument DataObject2XMLDocument(ref ICSSoft.STORMNET.DataObject dataObject)
        {
            return DataObject2XMLDocument(ref dataObject, false, true, true);
        }

        /// <summary>
        /// Получение XML документа из объекта данных.
        /// </summary>
        /// <param name="dataObject"></param>
        /// <param name="serializeAggregators"></param>
        /// <returns></returns>
        public static XmlDocument DataObject2XMLDocument(ref ICSSoft.STORMNET.DataObject dataObject, bool serializeAggregators)
        {
            return DataObject2XMLDocument(ref dataObject, serializeAggregators, true, true);
        }

        /// <summary>
        /// Получение XML документа из объекта данных.
        /// </summary>
        /// <param name="dataObject"></param>
        /// <param name="serializeAggregators"></param>
        /// <param name="setObjectLoadingStateLoaded"></param>
        /// <param name="setObjectStatusCreated"></param>
        /// <returns></returns>
        public static XmlDocument DataObject2XMLDocument(ref ICSSoft.STORMNET.DataObject dataObject, bool serializeAggregators,
            bool setObjectLoadingStateLoaded, bool setObjectStatusCreated)
        {
            return DataObject2XMLDocument(
                ref dataObject,
                serializeAggregators,
                setObjectLoadingStateLoaded,
                setObjectStatusCreated,
                false);
        }

        /// <summary>
        /// Получение XML документа из объекта данных.
        /// </summary>
        /// <param name="dataObject"> Сам объект данных. </param>
        /// <param name="serializeAggregators"> Следует ли сериализовать детейлы. </param>
        /// <param name="setObjectLoadingStateLoaded"> Установить LoadingState объекта в Loaded. </param>
        /// <param name="setObjectStatusCreated"> Установить ObjectStatus объекта в Created. </param>
        /// <param name="serializeMasters"> Проводить полную сериализацию мастеров объектов. </param>
        /// <returns> Сериализованное представление объекта. </returns>
        public static XmlDocument DataObject2XMLDocument(
            ref ICSSoft.STORMNET.DataObject dataObject,
            bool serializeAggregators,
            bool setObjectLoadingStateLoaded,
            bool setObjectStatusCreated,
            bool serializeMasters)
        {
            var xmlDoc = new XmlDocument();
            if (dataObject.GetStatus() != ObjectStatus.Deleted)
            { // Удалённые объекты сохранять не следует
                xmlDoc.LoadXml(@"<" + "STORMNETXMLDataService" + " />");
                var xmlMainEl = (XmlElement)xmlDoc.FirstChild;
                var xmlEl = (XmlElement)xmlMainEl.AppendChild(xmlDoc.CreateElement(dataObject.GetType().ToString()));

                var assemblies = new SortedList();

                prv_DataObject2XmlElement(
                    xmlEl,
                    dataObject,
                    assemblies,
                    serializeAggregators,
                    setObjectLoadingStateLoaded,
                    setObjectStatusCreated,
                    serializeMasters,
                    new List<string>());

                var xmlAssemblies = (XmlElement)xmlDoc.CreateElement("Assemblies");
                for (int i = 0; i < assemblies.Count; i++)
                {
                    var xmlAssembly = (XmlElement)xmlAssemblies.AppendChild(xmlDoc.CreateElement((string)assemblies.GetKey(i)));
                    xmlAssembly.SetAttribute("Assembly", (string)assemblies.GetByIndex(i));
                }

                xmlMainEl.AppendChild(xmlAssemblies);
            }
            else if (setObjectStatusCreated)
            { // Необходимо вернуть пустой, вновь созданный объект
                ConstructorInfo ci = dataObject.GetType().GetConstructor(new System.Type[0]);
                dataObject = (DataObject)ci.Invoke(new object[0]);
            }

            return xmlDoc;
        }

        /// <summary>
        /// Получение XML документа из объекта данных.
        /// </summary>
        /// <param name="xmlEl"> Текущий XmlElement, куда записываются данные и от которого создаются потомки. </param>
        /// <param name="dataObject"> Сериализуемый объект данных. </param>
        /// <param name="assemblies"> Имена сборок (будут записаны в xml). </param>
        /// <param name="serializeAggregators"> Следует ли сериализовать детейлы. </param>
        /// <param name="setObjectLoadingStateLoaded"> Установить LoadingState объекта в Loaded. </param>
        /// <param name="setObjectStatusCreated"> Установить ObjectStatus объекта в Created. </param>
        /// <param name="serializeMasters"> Сериализовать мастеров объектов. </param>
        /// <param name="usedPrimaryKeyList"> Вспомогательный список первичных ключей объектов, которые уже были сериализованы. </param>
        /// <returns> Сериализованное представление объекта. </returns>
        private static void prv_DataObject2XmlElement(
            XmlElement xmlEl,
            ICSSoft.STORMNET.DataObject dataObject,
            SortedList assemblies,
            bool serializeAggregators,
            bool setObjectLoadingStateLoaded,
            bool setObjectStatusCreated,
            bool serializeMasters,
            ICollection<string> usedPrimaryKeyList)
        {
            if (serializeMasters && usedPrimaryKeyList.Contains(dataObject.__PrimaryKey.ToString()))
            { // Это значит, что данный объект уже был сериализован (это надстройка для адекватной сериализации мастеров)
                xmlEl.SetAttribute("Type", dataObject.GetType().ToString());
                xmlEl.SetAttribute("__PrimaryKey", dataObject.__PrimaryKey.ToString());
                return;
            }

            if (serializeMasters)
            { // Надстройка для адекватной сериализации мастеров
                usedPrimaryKeyList.Add(dataObject.__PrimaryKey.ToString());
            }

            string[] propNames = Information.GetStorablePropertyNames(dataObject.GetType());
            for (int i = 0; i < propNames.Length; i++)
            {
                PropertyInfo propertyinfo = dataObject.GetType().GetProperty(propNames[i]);
                if (!prv_IsAgregator(propertyinfo) || serializeAggregators)
                {
                    if (propertyinfo.PropertyType.IsSubclassOf(typeof(DetailArray)))
                    { // Значит, это детейлы
                        var xmlElDetailArray = (XmlElement)xmlEl.AppendChild(xmlEl.OwnerDocument.CreateElement(propNames[i]));
                        var detailarray = (ICSSoft.STORMNET.DetailArray)Information.GetPropValueByName(dataObject, propNames[i]);
                        if (detailarray != null)
                        {
                            int j = 0;
                            while (j < detailarray.Count)
                            {
                                if (detailarray.ItemByIndex(j).GetStatus() != ObjectStatus.Deleted)
                                { // Удалённые объекты сохранять не следует
                                    string strtype = detailarray.ItemByIndex(j).GetType().ToString();
                                    var xmlElDetailObject = (XmlElement)xmlElDetailArray.AppendChild(xmlEl.OwnerDocument.CreateElement(strtype));
                                    if (!assemblies.ContainsKey(strtype))
                                    {
                                        assemblies.Add(strtype, detailarray.ItemByIndex(j).GetType().Assembly.FullName);
                                    }

                                    prv_DataObject2XmlElement(
                                        xmlElDetailObject,
                                        detailarray.ItemByIndex(j),
                                        assemblies,
                                        serializeAggregators,
                                        setObjectLoadingStateLoaded,
                                        setObjectStatusCreated,
                                        serializeMasters,
                                        usedPrimaryKeyList);
                                }
                                else
                                { // Тогда нужно грохнуть из массива
                                    detailarray.Remove(detailarray.ItemByIndex(j));
                                    j--;
                                }

                                j++;
                            }
                        }
                    }
                    else
                    {
                        if (propertyinfo.PropertyType.IsSubclassOf(typeof(DataObject)))
                        { // Значит, это мастер
                            object mastervalue = Information.GetPropValueByName(dataObject, propNames[i]);

                            if (mastervalue != null)
                            {
                                string mastertype = mastervalue.GetType().ToString();
                                if (!assemblies.ContainsKey(mastertype))
                                {
                                    assemblies.Add(mastertype, mastervalue.GetType().Assembly.FullName);
                                }

                                var xmlElMaster = (XmlElement)xmlEl.AppendChild(xmlEl.OwnerDocument.CreateElement(propNames[i]));
                                var masterDataObject =
                                    (DataObject)Information.GetPropValueByName(dataObject, propNames[i]);
                                if (!serializeMasters)
                                { // Старое поведение, которое не сериализует мастеров
                                    xmlElMaster.SetAttribute("Type", mastervalue.GetType().ToString());
                                    xmlElMaster.SetAttribute("__PrimaryKey", masterDataObject.__PrimaryKey.ToString());
                                }
                                else
                                { // Новое поведение, которое будет сериализовать мастеров
                                    if (!usedPrimaryKeyList.Contains(masterDataObject.__PrimaryKey.ToString()))
                                    {
                                        xmlElMaster.SetAttribute("__Type", mastervalue.GetType().ToString());
                                    }

                                    prv_DataObject2XmlElement(
                                        xmlElMaster,
                                        masterDataObject,
                                        assemblies,
                                        serializeAggregators,
                                        setObjectLoadingStateLoaded,
                                        setObjectStatusCreated,
                                        serializeMasters,
                                        usedPrimaryKeyList);
                                }
                            }
                        }
                        else
                        { // Значит, это собственный атрибут
                            object propvalue = Information.GetPropValueByName(dataObject, propNames[i]) ?? string.Empty;
                            xmlEl.SetAttribute(propNames[i], propvalue.ToString());
                        }
                    }
                }
            }

            xmlEl.SetAttribute("DynamicProperties", dataObject.DynamicProperties.Count > 0 ? ToolBinarySerializer.ObjectToString(dataObject.DynamicProperties) : string.Empty);

            if (setObjectLoadingStateLoaded)
            {
                dataObject.SetLoadingState(LoadingState.Loaded);
            }

            if (setObjectStatusCreated)
            {
                dataObject.SetStatus(ObjectStatus.Created);
            }
        }

        /// <summary>
        /// Извлечение объекта данных из строки.
        /// </summary>
        /// <param name="xmlEl"> Текущий элемент xml. </param>
        /// <param name="dataObject"> Текущий объект данных. </param>
        /// <param name="assemblies"> Необходимые сборки. </param>
        /// <param name="DataObjectCache"> DataObjectCache. </param>
        /// <param name="deserializedObjectsList"> Словарь десериализованных объектов с их первичными ключами. </param>
        private static void prv_XmlElement2DataObject(
            XmlElement xmlEl,
            ICSSoft.STORMNET.DataObject dataObject,
            SortedList assemblies,
            DataObjectCache DataObjectCache,
            Dictionary<string, ICSSoft.STORMNET.DataObject> deserializedObjectsList)
        {
            if (!deserializedObjectsList.ContainsKey(dataObject.__PrimaryKey.ToString()))
            {
                deserializedObjectsList.Add(dataObject.__PrimaryKey.ToString(), dataObject);
            }

            var storableprops = new ArrayList(Information.GetStorablePropertyNames(dataObject.GetType()));
            var order = new StringCollection();
            order.AddRange(Information.GetLoadingOrder(dataObject.GetType()));

            foreach (string propname in order)
            { // Прочитка в соответствии с указанным порядком
                prv_ReadProperty(xmlEl, dataObject, propname, assemblies, DataObjectCache, deserializedObjectsList);
            }

            XmlAttributeCollection xmlattributes = xmlEl.Attributes;
            XmlNodeList xmlchilds = xmlEl.ChildNodes;

            if (xmlattributes != null)
            {
                foreach (XmlAttribute xmlattribute in xmlattributes)
                {
                    if (!order.Contains(xmlattribute.Name) && storableprops.Contains(xmlattribute.Name))
                    {
                        prv_ReadAttribute(xmlattribute, dataObject);
                    }
                }
            }

            if (xmlchilds != null)
            {
                foreach (XmlNode xmlchild in xmlchilds)
                {
                    Type proptype = Information.GetPropertyType(dataObject.GetType(), xmlchild.Name);
                    if (proptype.IsSubclassOf(typeof(DataObject)))
                    { // Это мастер
                        prv_ReadMaster(xmlchild, dataObject, assemblies, DataObjectCache, deserializedObjectsList);
                    }
                    else
                    { // Это детейл
                        if (!order.Contains(xmlchild.Name))
                        {
                            var detail = (DetailArray)Information.GetPropValueByName(dataObject, xmlchild.Name);
                            XmlNodeList xmldetailobjects = xmlchild.ChildNodes;
                            if (xmldetailobjects != null)
                            {
                                prv_ReadDetail(xmldetailobjects, detail, assemblies, DataObjectCache, deserializedObjectsList);
                            }
                        }
                    }
                }
            }

            if (xmlEl.HasAttribute("DynamicProperties"))
            {
                if (xmlEl.HasAttribute("DynamicProperties"))
                {
                    string dpstr = xmlEl.GetAttribute("DynamicProperties");
                    if (string.IsNullOrEmpty(dpstr))
                    {
                        dataObject.DynamicProperties = new NameObjectCollection();
                    }
                    else
                    {
                        dataObject.DynamicProperties = (NameObjectCollection)ToolBinarySerializer.ObjectFromString(dpstr);
                    }
                }
            }

            dataObject.InitDataCopy();
            dataObject.SetLoadingState(LoadingState.Loaded);
            dataObject.SetStatus(ObjectStatus.UnAltered);
        }

        /// <summary>
        /// Прочитать свойство объекта (с целью его дальнейшей десериализации).
        /// </summary>
        /// <param name="xmlEl"> Текущий элемент xml. </param>
        /// <param name="dataObject"> Текущий объект данных. </param>
        /// <param name="propname"> Читаемое свойство объекта. </param>
        /// <param name="assemblies"> Необходимые сборки. </param>
        /// <param name="DataObjectCache"> DataObjectCache. </param>
        /// <param name="deserializedObjectsList"> Словарь десериализованных объектов с их первичными ключами. </param>
        private static void prv_ReadProperty(
            XmlElement xmlEl,
            ICSSoft.STORMNET.DataObject dataObject,
            string propname,
            SortedList assemblies,
            DataObjectCache DataObjectCache,
            Dictionary<string, ICSSoft.STORMNET.DataObject> deserializedObjectsList)
        {
            Type proptype = Information.GetPropertyType(dataObject.GetType(), propname);
            if (proptype.IsSubclassOf(typeof(DataObject)))
            { // Значит, мастер
                XmlNode masternode = xmlEl.GetElementsByTagName(propname)[0];
                prv_ReadMaster(masternode, dataObject, assemblies, DataObjectCache, deserializedObjectsList);
            }
            else
            {
                if (proptype.IsSubclassOf(typeof(DetailArray)))
                { // Значит, детейл
                    var detail = (DetailArray)Information.GetPropValueByName(dataObject, propname);
                    XmlNode detailnode = xmlEl.GetElementsByTagName(propname)[0];
                    if (detailnode != null)
                    {
                        XmlNodeList xmldetailobjects = detailnode.ChildNodes;
                        if (xmldetailobjects != null)
                        {
                            prv_ReadDetail(xmldetailobjects, detail, assemblies, DataObjectCache, deserializedObjectsList);
                        }
                    }
                }
                else
                { // Значит, это обычный атрибут
                    XmlAttribute attr = xmlEl.GetAttributeNode(propname);
                    if (attr != null)
                    {
                        prv_ReadAttribute(attr, dataObject);
                    }
                }
            }
        }

        /// <summary>
        /// Извлечение мастера из сериализованного представления.
        /// </summary>
        /// <param name="masternode"> Текущий элемент xml. </param>
        /// <param name="dataObject"> Текущий объект данных. </param>
        /// <param name="assemblies"> Необходимые сборки. </param>
        /// <param name="DataObjectCache"> DataObjectCache. </param>
        /// <param name="deserializedObjectsList"> Словарь десериализованных объектов с их первичными ключами. </param>
        private static void prv_ReadMaster(
            XmlNode masternode,
            ICSSoft.STORMNET.DataObject dataObject,
            SortedList assemblies,
            DataObjectCache DataObjectCache,
            Dictionary<string, ICSSoft.STORMNET.DataObject> deserializedObjectsList)
        {
            XmlNode specialTypeNode = masternode.Attributes.GetNamedItem("__Type");
            string skey = masternode.Attributes.GetNamedItem("__PrimaryKey").Value;
            DataObject masterobject = null;
            if (deserializedObjectsList.ContainsKey(skey))
            {
                masterobject = deserializedObjectsList[skey];
            }
            else
            {
                string stype = specialTypeNode != null ? specialTypeNode.Value : masternode.Attributes.GetNamedItem("Type").Value;
                string asmName = (string)assemblies[stype];
                Assembly asm = AssemblyLoader.LoadAssembly(asmName);
                Type mastertype = asm.GetType(stype);
                masterobject = DataObjectCache.CreateDataObject(mastertype, Information.TranslateValueToPrimaryKeyType(mastertype, skey));
                if (specialTypeNode != null)
                { // То есть это был особым образом сериализованный мастер
                    prv_XmlElement2DataObject(
                        (XmlElement)masternode, masterobject, assemblies, DataObjectCache, deserializedObjectsList);
                }
            }

            Information.SetPropValueByName(dataObject, masternode.Name, masterobject);
        }

        /// <summary>
        /// Извлечение детейла из сериализованного представления.
        /// </summary>
        /// <param name="xmldetailobjects"> Текущий элемент xml. </param>
        /// <param name="detail"> Текущий список детейлов. </param>
        /// <param name="assemblies"> Необходимые сборки. </param>
        /// <param name="DataObjectCache"> DataObjectCache. </param>
        /// <param name="deserializedObjectsList"> Словарь десериализованных объектов с их первичными ключами. </param>
        private static void prv_ReadDetail(
            XmlNodeList xmldetailobjects,
            DetailArray detail,
            SortedList assemblies,
            DataObjectCache DataObjectCache,
            Dictionary<string, ICSSoft.STORMNET.DataObject> deserializedObjectsList)
        {
            for (int j = 0; j < xmldetailobjects.Count; j++)
            {
                XmlNode xmldetailobject = xmldetailobjects[j];
                Assembly asm = AssemblyLoader.LoadAssembly((string)assemblies[xmldetailobject.Name]);
                System.Type dotype = asm.GetType(xmldetailobject.Name);
                DataObject detailobject = DataObjectCache.CreateDataObject(dotype, Information.TranslateValueToPrimaryKeyType(dotype, ((XmlElement)xmldetailobject).GetAttribute("__PrimaryKey")));
                prv_XmlElement2DataObject((XmlElement)xmldetailobject, detailobject, assemblies, DataObjectCache, deserializedObjectsList);
                detail.AddObject(detailobject);
            }
        }

        private static void prv_ReadAttribute(XmlAttribute attr, DataObject dataObject)
        {
            Information.SetPropValueByName(dataObject, attr.Name, attr.Value);
        }

        private static bool prv_IsAgregator(PropertyInfo propertyinfo)
        {
            bool result = false;
            object[] attributes = propertyinfo.GetCustomAttributes(typeof(AgregatorAttribute), true);
            if (attributes.Length > 0)
            {
                result = true;
            }

            return result;
        }
    }

    /// <summary>
    /// Инструмент для бинарной сериализации-десериализации
    /// Используется для Function
    /// Сериализованные байты конвертируются в ToBase64String.
    /// </summary>
    public class ToolBinarySerializer
    {
        /// <summary>
        /// Сериализация объекта при помощи BinaryFormatter.
        /// </summary>
        /// <param name="o">Объект.</param>
        /// <returns>Строка.</returns>
        public static string ObjectToString(object o)
        {
            MemoryStream strm = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            // formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
            formatter.Serialize(strm, o);
            strm.Position = 0;
            byte[] binData = new byte[strm.Length];
            strm.Read(binData, 0, (int)strm.Length);

            return Convert.ToBase64String(binData);
        }

        /// <summary>
        /// Десериализация объекта при помощи BinaryFormatter.
        /// </summary>
        /// <param name="s">Сериализованный объект.</param>
        /// <returns>Востановленный объект.</returns>
        public static object ObjectFromString(string s)
        {
            return ObjectFromString(s, null);
        }

        /// <summary>
        /// Десериализация объекта при помощи BinaryFormatter.
        /// </summary>
        /// <param name="s">Сериализованный объект.</param>
        /// <param name="binder">
        /// Binder, который необходимо указать, если при десереализации
        /// необходимо реализовать собственную логику по поиску типов.
        /// </param>
        /// <returns>Востановленный объект.</returns>
        public static object ObjectFromString(string s, SerializationBinder binder)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (binder != null)
            {
                formatter.Binder = binder;
            }

            MemoryStream strm = new MemoryStream();
            formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
            byte[] fileData = Convert.FromBase64String(s);
            strm.Write(fileData, 0, fileData.GetUpperBound(0) + 1);
            strm.Position = 0;
            return formatter.Deserialize(strm);
        }
    }

    /// <summary>
    /// Помощь при загрузке сборок, которые были подписаны.
    /// </summary>
    public static class AssemblyLoader
    {
        /// <summary>
        /// Загрузка сборки сначала по полному переданному пути, потом с обрезкой PublicKeyToken (Если не получилось, то Exception).
        /// </summary>
        /// <param name="asmName">Полное имя сборки.</param>
        /// <returns></returns>
        public static Assembly LoadAssembly(string asmName)
        {
            if (string.IsNullOrEmpty(asmName))
            {
                throw new ArgumentNullException("asmName");
            }

            Assembly asm = null;
            AssemblyName asmN = new AssemblyName(asmName);
            try
            {
                asm = Assembly.Load(asmN);
            }
            catch { }
            string shortAsmName = string.Empty;
            string exStr = string.Empty;
            if (asm == null && (asmName.IndexOf("Culture=") > -1 || asmName.IndexOf("PublicKeyToken=") > -1))
            {
                int indCult = asmName.LastIndexOf("Culture=", StringComparison.InvariantCultureIgnoreCase) - 2;
                int indPKT = asmName.LastIndexOf("PublicKeyToken=", StringComparison.InvariantCultureIgnoreCase) - 2;
                int ind = indCult > indPKT ? indPKT : indCult;
                shortAsmName = asmName.Substring(0, ind);
                try
                {
                    AssemblyName asmNShort = new AssemblyName(shortAsmName);
                    asm = Assembly.Load(asmNShort);
                }
                catch
                {
                    Regex regex = new Regex(@"(?<=Version=(\d+)\.(\d+))\..*?(?=,|$)");

                    try
                    {
                        asm = Assembly.Load(regex.Replace(shortAsmName, string.Empty));
                    }
                    catch (Exception ex)
                    {
                        exStr = ex.ToString();
                    }
                }
            }

            if (asm == null)
            {
                throw new Exception("Не могу найти сборку: [" + asmName + "] или [" + shortAsmName + "] с ошибкой: " + exStr);
            }

            return asm;
        }

        /// <summary>
        /// Получить тип по имени с указанием сборки (должен работать, даже если изменился PublicKeyToken).
        /// </summary>
        /// <param name="assemblyQualifiedName">Полное имя типа.</param>
        /// <returns>Тип.</returns>
        public static Type GetTypeWithAssemblyName(string assemblyQualifiedName)
        {
            if (string.IsNullOrEmpty(assemblyQualifiedName))
            {
                throw new ArgumentNullException("assemblyQualifiedName");
            }

            Type retType = null;
            try
            {
                retType = Type.GetType(assemblyQualifiedName, false, true);
            }
            catch { }
            if (retType != null)
            {
                return retType;
            }

            string shortName = string.Empty, exStr = string.Empty;
            if (assemblyQualifiedName.IndexOf("Culture=") > -1 || assemblyQualifiedName.IndexOf("PublicKeyToken=") > -1)
            {
                int indCult = assemblyQualifiedName.LastIndexOf("Culture=", StringComparison.InvariantCultureIgnoreCase) - 2;
                int indPKT = assemblyQualifiedName.LastIndexOf("PublicKeyToken=", StringComparison.InvariantCultureIgnoreCase) - 2;
                int ind = indCult > indPKT ? indPKT : indCult;

                shortName = assemblyQualifiedName.Substring(0, ind);
                try
                {
                    retType = Type.GetType(shortName, false, true);
                }
                catch
                {
                    Regex regex = new Regex(@"(?<=Version=(\d+)\.(\d+))\..*?(?=,|$)");

                    try
                    {
                        retType = Type.GetType(regex.Replace(shortName, string.Empty));
                    }

                    catch (Exception ex)
                    {
                        exStr = ex.ToString();
                    }
                }
            }

            if (retType == null)
            {
                throw new Exception("Не могу найти тип: [" + assemblyQualifiedName + "] или [" + shortName + "] с ошибкой: " + exStr);
            }

            return retType;
        }

        /// <summary>
        /// Обработчик события на неразрешённую сборку. Пробуем загружать сборки для десериализации по неполному описанию.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns>null - если не нашлось ничего.</returns>
        public static Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            string asmName = args.Name;

            // MessageBox.Show("Разрешаю сборку: " + asmName);
            Assembly asm = null;
            if (asmName.IndexOf("Culture=") > -1 || asmName.IndexOf("PublicKeyToken=") > -1)
            {
                int indCult = asmName.LastIndexOf("Culture=", StringComparison.InvariantCultureIgnoreCase) - 2;
                int indPKT = asmName.LastIndexOf("PublicKeyToken=", StringComparison.InvariantCultureIgnoreCase) - 2;
                int ind = indCult > indPKT ? indPKT : indCult;
                string shortAsmName = asmName.Substring(0, ind);
                try
                {
                    AssemblyName asmNShort = new AssemblyName(shortAsmName);
                    asm = Assembly.Load(asmNShort);
                }
                catch
                {
                }
            }

            return asm;
        }

        /// <summary>
        /// Разрешить тип.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Assembly CurrentDomainTypeResolve(object sender, ResolveEventArgs args)
        {
            string assemblyQualifiedName = args.Name;
            string shortName = assemblyQualifiedName;

            // TODO: comment this
            // MessageBox.Show("Resolve " + assemblyQualifiedName);
            if (assemblyQualifiedName.IndexOf("Culture=") > -1 || assemblyQualifiedName.IndexOf("PublicKeyToken=") > -1)
            {
                int indCult =
                    assemblyQualifiedName.LastIndexOf("Culture=", StringComparison.InvariantCultureIgnoreCase) - 2;
                int indPKT = assemblyQualifiedName.LastIndexOf("PublicKeyToken=", StringComparison.InvariantCultureIgnoreCase) - 2;
                int ind = indCult > indPKT ? indPKT : indCult;

                shortName = assemblyQualifiedName.Substring(0, ind);
            }

            Type type = Type.GetType(shortName, false);
            if (type != null)
            {
                return type.Assembly;
            }

            return null;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class AllowAllAssemblyVersionsDeserializationBinder : System.Runtime.Serialization.SerializationBinder
    {
        public override Type BindToType(string asmName, string typeName)
        {
            // MessageBox.Show("BindToType: " + asmName + " type " + typeName);
            if (asmName == null)
            {
                throw new ArgumentNullException("asmName");
            }

            Type typeToDeserialize = null;

            if (asmName.IndexOf("Culture=") > -1 || asmName.IndexOf("PublicKeyToken=") > -1)
            {
                int indCult = asmName.LastIndexOf("Culture=", StringComparison.InvariantCultureIgnoreCase) - 2;
                int indPKT = asmName.LastIndexOf("PublicKeyToken=", StringComparison.InvariantCultureIgnoreCase) - 2;
                int ind = indCult > indPKT ? indPKT : indCult;
                string shortAsmName = asmName.Substring(0, ind);

                // Get the type using the typeName and assemblyName
                typeToDeserialize = Type.GetType(string.Format("{0}, {1}",
                                                               typeName, shortAsmName));
            }

            return typeToDeserialize;
        }
    }

    public class ExternalModule
    {
        private ExternalModule()
        {
        }

        public delegate object[] GetParametersForContructorDelegate(Type moduleType);

        public static Type[] GetTypes(string filename, Type filter)
        {
            ArrayList arl = new ArrayList();
            if (System.IO.Path.GetDirectoryName(filename) == string.Empty)
            {
                filename = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\" + filename;
            }

            Assembly asm = Assembly.LoadFile(filename);
            foreach (Type t in asm.GetTypes())
            {
                if (t == filter)
                {
                    arl.Add(t);
                }
                else if (t.IsSubclassOf(filter))
                {
                    arl.Add(t);
                }
                else
                {
                    Type tt = t.GetInterface(filter.Name);
                    if (tt != null)
                    {
                        arl.Add(t);
                    }
                }
            }

            return (Type[])arl.ToArray(typeof(Type));
        }

        public static Type[] GetTypes(string filename)
        {
            return GetTypes(filename, typeof(object));
        }

        public static object[] GetInstances(string filename, Type filter)
        {
            return GetInstances(filename, filter, new object[0]);
        }

        public static object[] GetInstances(string filename, Type filter, object[] parameters)
        {
            Type[] alltypes = GetTypes(filename, filter);
            object[] res = new object[alltypes.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = Activator.CreateInstance(alltypes[i], parameters);
            }

            return res;
        }

        public static object[] GetInstances(string filename, Type filter, GetParametersForContructorDelegate parametersDelegate)
        {
            if (parametersDelegate == null)
            {
                return GetInstances(filename, filter, new object[0]);
            }
            else
            {
                Type[] alltypes = GetTypes(filename, filter);
                object[] res = new object[alltypes.Length];
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = Activator.CreateInstance(alltypes[i], parametersDelegate(alltypes[i]));
                }

                return res;
            }
        }
    }

    public class ToolZIP
    {
        // *** Start programmer edit section *** (Compressor CustomMembers)

        /// <summary>
        /// Сжатие.
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static MemoryStream Compress(MemoryStream ms)
        {
            MemoryStream restream = new MemoryStream();

            ICSharpCode.SharpZipLib.Zip.ZipOutputStream s = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(restream);

            s.SetLevel(9); // 0 - store only to 9 - means best compression

            byte[] buffer = new byte[ms.Length];

            ms.Seek(0, System.IO.SeekOrigin.Begin);

            ms.Read(buffer, 0, buffer.Length);

            ICSharpCode.SharpZipLib.Zip.ZipEntry entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry("TempEntry");

            s.PutNextEntry(entry);

            s.Write(buffer, 0, buffer.Length);

            s.Finish();

            restream.Seek(0, SeekOrigin.Begin);

            return restream;
        }

        /// <summary>
        /// Разжатие.
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static MemoryStream Decompress(MemoryStream ms)
        {
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            ICSharpCode.SharpZipLib.Zip.ZipInputStream zs = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(ms);

            ICSharpCode.SharpZipLib.Zip.ZipEntry theEntry;

            try
            {
                theEntry = zs.GetNextEntry();
            }
            catch (Exception e)
            {
                return ms;
            }

            MemoryStream resstream = new MemoryStream();

            // ****
            byte[] data = new byte[4096];
            int size;

            do
            {
                size = zs.Read(data, 0, data.Length);
                resstream.Write(data, 0, size);
            }
            while (size > 0);

            // *****

            // b = new byte[zs.Length];

            // zs.Read(b,0,b.Length);

            // resstream.Write(b,0,b.Length);

            resstream.Seek(0, SeekOrigin.Begin);
            zs.Close();
            return resstream;
        }

        /// <summary>
        /// Сжатие и кодирование в base64.
        /// </summary>
        /// <param name="несжатаяСтрока">входная строчка.</param>
        /// <returns>сжатая строчка.</returns>
        public static string Compress(string несжатаяСтрока)
        {
            MemoryStream ms = new MemoryStream();

            // StreamWriter strw = new StreamWriter(ms);
            // strw.Write(несжатаяСтрока);
            BinaryWriter binw = new BinaryWriter(ms);
            binw.Write(несжатаяСтрока);

            // binw.Close();

            MemoryStream retMS = Compress(ms);
            byte[] byteArr = retMS.ToArray();
            binw.Close();
            ms.Close();
            retMS.Close();
            return Convert.ToBase64String(byteArr);
        }

        /// <summary>
        /// Разжатие.
        /// </summary>
        /// <param name="сжатаяСтрока">сжатая строчка.</param>
        /// <returns>исходная строка.</returns>
        public static string Decompress(string сжатаяСтрока)
        {
            MemoryStream ms = new MemoryStream();

            // StreamWriter strw = new StreamWriter(ms);
            // strw.Write(несжатаяСтрока);
            BinaryWriter binw = new BinaryWriter(ms);
            binw.Write(Convert.FromBase64String(сжатаяСтрока));

            MemoryStream retMS = Decompress(ms);
            binw.Close();
            ms.Close();
            BinaryReader binr = new BinaryReader(retMS);
            string retStr = binr.ReadString();
            binr.Close();
            retMS.Close();
            return retStr;
        }

        // *** End programmer edit section *** (Compressor CustomMembers)
    }
}
