namespace ICSSoft.STORMNET.Tools
{
    using System;
    using System.Globalization;
    using System.Xml;
    
    /// <summary>
    /// Manager для работы с xml. Упрощает создание новых вершин, запись и чтение атрибутов различных типов.
    /// </summary>
    public class XMLManager
    {
        /// <summary>
        /// Экземпляр xml-Документа с которым работает класс.
        /// </summary>
        private readonly XmlDocument _document;

        #region static methods

        /// <summary>
        /// Получить значение атрибута указанной вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="name">Наименование атрибута, значение которого будет читаться.</param>
        /// <returns>Строковое значение атрибута вершины.</returns>
        public static string GetAttributeValue(XmlNode node, string name)
        {
            if (node.Attributes == null)
                return null;

            var attr = node.Attributes[name];
            return (attr != null) ? attr.Value : null;
        }

        /// <summary>
        /// Попытаться прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="value">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Удалось ли прочитать значение атрибута вершины.</returns>
        public static bool TryReadValue(XmlNode node, string attributeName, ref int value)
        {
            string attributeValue = GetAttributeValue(node, attributeName);
            int parsedValue;

            if (int.TryParse(attributeValue, out parsedValue))
            {
                value = parsedValue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="defaultValue">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Прочитаное значение атрибута вершины.</returns>
        public static int ReadValue(XmlNode node, string attributeName, int defaultValue)
        {
            int result = defaultValue;
            TryReadValue(node, attributeName, ref result);
            return result;
        }

        /// <summary>
        /// Попытаться прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="value">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Удалось ли прочитать значение атрибута вершины.</returns>
        public static bool TryReadValue(XmlNode node, string attributeName, ref long value)
        {
            string attributeValue = GetAttributeValue(node, attributeName);
            long parsedValue;

            if (long.TryParse(attributeValue, out parsedValue))
            {
                value = parsedValue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="defaultValue">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Прочитаное значение атрибута вершины.</returns>
        public static long ReadValue(XmlNode node, string attributeName, long defaultValue)
        {
            long result = defaultValue;
            TryReadValue(node, attributeName, ref result);
            return result;
        }

        /// <summary>
        /// Попытаться прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="value">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Удалось ли прочитать значение атрибута вершины.</returns>
        public static bool TryReadValue(XmlNode node, string attributeName, ref bool value)
        {
            string attributeValue = GetAttributeValue(node, attributeName);
            bool parsedValue;

            if (bool.TryParse(attributeValue, out parsedValue))
            {
                value = parsedValue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="defaultValue">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Прочитаное значение атрибута вершины.</returns>
        public static bool ReadValue(XmlNode node, string attributeName, bool defaultValue)
        {
            bool result = defaultValue;
            TryReadValue(node, attributeName, ref result);
            return result;
        }

        /// <summary>
        /// Попытаться прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="value">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Удалось ли прочитать значение атрибута вершины.</returns>
        public static bool TryReadValue(XmlNode node, string attributeName, ref double value)
        {
            string attributeValue = GetAttributeValue(node, attributeName);
            double parsedValue;

            if (attributeValue != null && TypeManager.TryParseDouble(attributeValue, out parsedValue))
            {
                value = parsedValue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="defaultValue">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Прочитаное значение атрибута вершины.</returns>
        public static double ReadValue(XmlNode node, string attributeName, double defaultValue)
        {
            double result = defaultValue;
            TryReadValue(node, attributeName, ref result);
            return result;
        }

        /// <summary>
        /// Попытаться прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="value">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Удалось ли прочитать значение атрибута вершины.</returns>
        public static bool TryReadValue(XmlNode node, string attributeName, ref string value)
        {
            string attributeValue = GetAttributeValue(node, attributeName);

            if (attributeValue != null)
            {
                value = attributeValue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="defaultValue">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Прочитаное значение атрибута вершины.</returns>
        public static string ReadValue(XmlNode node, string attributeName, string defaultValue)
        {
            string result = defaultValue;
            TryReadValue(node, attributeName, ref result);
            return result;
        }

        /// <summary>
        /// Попытаться прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="value">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Удалось ли прочитать значение атрибута вершины.</returns>
        public static bool TryReadValue(XmlNode node, string attributeName, ref decimal value)
        {
            string attributeValue = GetAttributeValue(node, attributeName);
            decimal parsedValue;

            if (attributeValue != null && TypeManager.TryParseDecimal(attributeValue, out parsedValue))
            {
                value = parsedValue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="defaultValue">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Прочитаное значение атрибута вершины.</returns>
        public static decimal ReadValue(XmlNode node, string attributeName, decimal defaultValue)
        {
            decimal result = defaultValue;
            TryReadValue(node, attributeName, ref result);
            return result;
        }

        /// <summary>
        /// Попытаться прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="value">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Удалось ли прочитать значение атрибута вершины.</returns>
        public static bool TryReadValue(XmlNode node, string attributeName, ref DateTime value)
        {
            string attributeValue = GetAttributeValue(node, attributeName);
            DateTime parsedValue;

            if (DateTime.TryParse(attributeValue, out parsedValue))
            {
                value = parsedValue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="defaultValue">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Прочитаное значение атрибута вершины.</returns>
        public static DateTime ReadValue(XmlNode node, string attributeName, DateTime defaultValue)
        {
            DateTime result = defaultValue;
            TryReadValue(node, attributeName, ref result);
            return result;
        }

        /// <summary>
        /// Попытаться прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="value">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Удалось ли прочитать значение атрибута вершины.</returns>
        public static bool TryReadValue(XmlNode node, string attributeName, ref Guid value)
        {
            string attributeValue = GetAttributeValue(node, attributeName);

            if (string.IsNullOrEmpty(attributeValue))
                return false;

            try
            {
                value = new Guid(attributeValue);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Прочитать значение атрибута вершины.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="defaultValue">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Прочитаное значение атрибута вершины.</returns>
        public static Guid ReadValue(XmlNode node, string attributeName, Guid defaultValue)
        {
            Guid result = defaultValue;
            TryReadValue(node, attributeName, ref result);
            return result;
        }

        /// <summary>
        /// Попытаться прочитать значение атрибута вершины.
        /// Метод для чтения перечислений.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="value">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Удалось ли прочитать значение атрибута вершины.</returns>
        public static bool TryReadValue<T>(XmlNode node, string attributeName, ref T value)
        {
            string attributeValue = GetAttributeValue(node, attributeName);

            if (string.IsNullOrEmpty(attributeValue))
                return false;

            try
            {
                if (value is Enum)
                    value = (T)Enum.Parse(typeof(T), attributeValue);
                else
                    value = (T)Convert.ChangeType(attributeValue, typeof(T));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Прочитать значение атрибута вершины.
        /// Метод для чтения перечислений.
        /// </summary>
        /// <param name="node">Вершина, атрибут которой будет искаться.</param>
        /// <param name="attributeName">Наименование атрибута, значение которого будет читаться.</param>
        /// <param name="defaultValue">Значение по умолчанию для читаемого атрибута. Оно будет указано в случае, если атрибут не будет найден или не сможет прочитаться.</param>
        /// <returns>Прочитаное значение атрибута вершины.</returns>
        public static T ReadValue<T>(XmlNode node, string attributeName, T defaultValue)
        {
            T result = defaultValue;
            TryReadValue(node, attributeName, ref result);
            return result;
        }

        #endregion static methods

        /// <summary>
        /// Экземпляр xml-Документа с которым работает класс.
        /// </summary>
        public XmlDocument XmlDocument
        {
            get { return _document; }
        }

        /// <summary>
        /// Создать экземпляр manager'а для работы с xml.
        /// </summary>
        /// <param name="xmlDocument">Xml-Документ с которым будет работать класс выполняя операции добавления вершин, чтения и записи значений атрибутов.</param>
        public XMLManager(XmlDocument xmlDocument)
        {
            _document = xmlDocument;
        }

        /// <summary>
        /// Добавить вершину в xml-документ.
        /// </summary>
        /// <param name="parentNode">Родительская вершина, внутрь которой следует добавить вновь созданую. Если создаваемая вершина должна стать корневой, то необходимо передать сам документ.</param>
        /// <param name="name">Наименование создаваемой вершины.</param>
        /// <returns>Созданная вершина в xml-документе.</returns>
        public XmlNode AppendChild(XmlNode parentNode, string name)
        {
            XmlElement el = _document.CreateElement(name);
            parentNode.AppendChild(el);
            return el;
        }

        /// <summary>
        /// Добавить атрибут для вершины.
        /// </summary>
        /// <param name="node">Вершина для которой следует добавить атрибут.</param>
        /// <param name="name">Наименование создаваемого атрибута.</param>
        /// <param name="value">Значение атрибута в строковом виде. Данный метод в основном используется самим классом. Лучше использовать специальные методы записи значений атрибутов.</param>
        /// <returns>Созданный атрибут для вершины.</returns>
        public XmlAttribute AppendAttribute(XmlNode node, string name, string value)
        {
            XmlAttribute attr = _document.CreateAttribute(name);
            attr.Value = value;
            node.Attributes.Append(attr);
            return attr;
        }

        /// <summary>
        /// Записать значение атрибута для вершины.
        /// </summary>
        /// <param name="node">Вершина для атрибута которой будет записано значение.</param>
        /// <param name="name">Наименование атрибута значение которое необходимо записать.</param>
        /// <param name="value">Значение, которое необходимо записать в атрибут.</param>
        public void WriteValue(XmlNode node, string name, string value)
        {
            AppendAttribute(node, name, value);
        }

        /// <summary>
        /// Записать значение атрибута для вершины.
        /// </summary>
        /// <param name="node">Вершина для атрибута которой будет записано значение.</param>
        /// <param name="name">Наименование атрибута значение которое необходимо записать.</param>
        /// <param name="value">Значение, которое необходимо записать в атрибут.</param>
        public void WriteValue(XmlNode node, string name, int value)
        {
            // Не важно с какой культурой запишем значение в xml, при чтении будем автоматически её подбирать.
            WriteValue(node, name, value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Записать значение атрибута для вершины.
        /// </summary>
        /// <param name="node">Вершина для атрибута которой будет записано значение.</param>
        /// <param name="name">Наименование атрибута значение которое необходимо записать.</param>
        /// <param name="value">Значение, которое необходимо записать в атрибут.</param>
        public void WriteValue(XmlNode node, string name, bool value)
        {
            WriteValue(node, name, value.ToString());
        }

        /// <summary>
        /// Записать значение атрибута для вершины.
        /// </summary>
        /// <param name="node">Вершина для атрибута которой будет записано значение.</param>
        /// <param name="name">Наименование атрибута значение которое необходимо записать.</param>
        /// <param name="value">Значение, которое необходимо записать в атрибут.</param>
        public void WriteValue(XmlNode node, string name, double value)
        {
            // Не важно с какой культурой запишем значение в xml, при чтении будем автоматически её подбирать.
            WriteValue(node, name, value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Записать значение атрибута для вершины.
        /// </summary>
        /// <param name="node">Вершина для атрибута которой будет записано значение.</param>
        /// <param name="name">Наименование атрибута значение которое необходимо записать.</param>
        /// <param name="value">Значение, которое необходимо записать в атрибут.</param>
        public void WriteValue(XmlNode node, string name, DateTime value)
        {
            WriteValue(node, name, value.ToShortDateString());
        }

        /// <summary>
        /// Записать значение атрибута для вершины.
        /// </summary>
        /// <param name="node">Вершина для атрибута которой будет записано значение.</param>
        /// <param name="name">Наименование атрибута значение которое необходимо записать.</param>
        /// <param name="value">Значение, которое необходимо записать в атрибут.</param>
        public void WriteValue(XmlNode node, string name, decimal value)
        {
            // Не важно с какой культурой запишем значение в xml, при чтении будем автоматически её подбирать.
            WriteValue(node, name, value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Записать значение атрибута для вершины.
        /// </summary>
        /// <param name="node">Вершина для атрибута которой будет записано значение.</param>
        /// <param name="name">Наименование атрибута значение которое необходимо записать.</param>
        /// <param name="value">Значение, которое необходимо записать в атрибут.</param>
        public void WriteValue(XmlNode node, string name, Guid value)
        {
            WriteValue(node, name, value.ToString());
        }

        /// <summary>
        /// Записать значение атрибута для вершины.
        /// </summary>
        /// <param name="node">Вершина для атрибута которой будет записано значение.</param>
        /// <param name="name">Наименование атрибута значение которое необходимо записать.</param>
        /// <param name="value">Значение, которое необходимо записать в атрибут.</param>
        public void WriteValue(XmlNode node, string name, long value)
        {
            // Не важно с какой культурой запишем значение в xml, при чтении будем автоматически её подбирать.
            WriteValue(node, name, value.ToString(CultureInfo.CurrentCulture));
        }
        
        /// <summary>
        /// Записать значение атрибута для вершины.
        /// </summary>
        /// <param name="node">Вершина для атрибута которой будет записано значение.</param>
        /// <param name="name">Наименование атрибута значение которое необходимо записать.</param>
        /// <param name="value">Значение, которое необходимо записать в атрибут.</param>
        public void WriteValue(XmlNode node, string name, Enum value)
        {
            WriteValue(node, name, value.ToString());
        }
    }
}
