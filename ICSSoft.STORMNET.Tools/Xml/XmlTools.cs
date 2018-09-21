namespace ICSSoft.STORMNET.Tools
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Инструменты для работы с Xml.
    /// </summary>
    public class XmlTools
    {
        /// <summary>
        /// Конвертация <see cref="System.Xml.Linq.XElement"/> в <see cref="System.Xml.XmlDocument"/>.
        /// </summary>
        /// <param name="xElement">
        /// XDocument для конвертации.
        /// </param>
        /// <param name="versionXmlDoc">
        /// Версия создаваемого xml документа.
        /// </param>
        /// <param name="encodingXmlDoc">
        /// Кодировка для создаваемого xml документа.
        /// </param>
        /// <returns>
        /// Результат конвертации XmlDocument.
        /// </returns>
        public static XmlDocument GetXDocumentByXElement(XElement xElement, string versionXmlDoc, string encodingXmlDoc)
        {
            var reader = xElement.CreateReader();
            reader.MoveToContent();
            var document = new XmlDocument();
            document.LoadXml(reader.ReadOuterXml());

            XmlDeclaration declaration = document.CreateXmlDeclaration(versionXmlDoc, encodingXmlDoc, null);
            document.InsertBefore(declaration, document.DocumentElement);

            return document;
        }

        /// <summary>
        /// Прочитать Xml-файл. Может автоматически определить кодировку.
        /// </summary>
        /// <param name="filePath">
        /// Путь до Xml-файла.
        /// </param>
        /// <param name="encoding">
        /// Кодировка, которую необходимо использовать для чтения Xml-файла.
        /// </param>
        /// <returns>
        /// Вычитанный Xml-файл в формате <see cref="XmlDocument"/>.
        /// </returns>
        public static XmlDocument LoadXml(string filePath, Encoding encoding = null)
        {
            // Данную операцию .Net выполнит самостоятельно, но так будет наглядней какая кодировка используется по умолчанию.
            if (encoding == null)
                encoding = Encoding.UTF8;

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                throw new ArgumentException("Путь до файла должен быть не пустым. Файл должен быть доступен в файловой системе.", "filePath");

            // Читаем Xml-файл с указанной кодировкой.
            var streamRead = new StreamReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), encoding);
            var xmldoc = new XmlDocument();
            xmldoc.Load(streamRead);
            streamRead.Close();

            // Определяем кодировку использующуюся внутри Xml.
            var firstChild = xmldoc.FirstChild as XmlDeclaration;

            if (firstChild != null)
            {
                Encoding realEncoding = Encoding.GetEncoding(firstChild.Encoding);

                // Если кодировка внутри Xml отличалась от той в которой мы прочитали файл, то перечитываем его.
                if (!realEncoding.Equals(encoding))
                    return LoadXml(filePath, realEncoding);
            }

            return xmldoc;
        }
    }
}
