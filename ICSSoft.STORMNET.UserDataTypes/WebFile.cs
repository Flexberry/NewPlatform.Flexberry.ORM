namespace ICSSoft.STORMNET.UserDataTypes
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Структура для хранения файлов в Web-приложении.
    /// </summary>
    [StoreInstancesInType(typeof(Business.SQLDataService), typeof(string))]
    [Serializable]
    public class WebFile
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Размер.
        /// </summary>
        public int Size { get; set; }

        private string _value;

        /// <summary>
        /// Конструктор без параметров, нужен для Activator.CreateInstance.
        /// </summary>
        public WebFile()
        {
        }

        public static explicit operator string(WebFile value)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, value);
                stream.Flush();
                stream.Position = 0;
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public static explicit operator WebFile(string value)
        {
            byte[] b = Convert.FromBase64String(value);
            using (var stream = new MemoryStream(b))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (WebFile)formatter.Deserialize(stream);
            }
        }

        public override string ToString()
        {
            // var xmlSerializer = new XmlSerializer(typeof(WebFile));
            // var memoryStream = new MemoryStream();
            // using (var xmlTextWriter =
            //    new XmlTextWriter(memoryStream, Encoding.UTF8) { Formatting = Formatting.Indented })
            // {
            //    xmlSerializer.Serialize(xmlTextWriter, this);
            //    memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            //    string xmlText = new UTF8Encoding().GetString(memoryStream.ToArray());
            //    memoryStream.Dispose();
            //    return xmlText;
            // }

            return this.Name;
        }
    }
}
