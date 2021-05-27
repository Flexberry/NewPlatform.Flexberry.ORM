using System;
using System.Xml.Linq;

namespace ICSSoft.STORMNET.UserDataTypes
{
    /// <summary>
    /// Пользовательский тип - изображение;.
    /// </summary>
    [StoreInstancesInType("ICSSoft.STORMNET.Business.C2RDataService.C2RDataService, ICSSoft.STORMNET.Business.C2RDataService",
                          "ICSSoft.STORMNET.UserDataTypes.Record, ICSSoft.STORMNET.UserDataTypes")]
    [StoreInstancesInType(typeof(Business.SQLDataService), typeof(string))]
    [Serializable]
    public class Image : IComparableType
    {
        /// <summary>
        /// Имя файла - изображения.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Формат изображения.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Ширина изображения.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота изображения.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Изображение (в байтах).
        /// Временное решение, планируется сделать BLOB'ом, когда в Lily появится поддержка чтения BLOB'ов из вложенных записей.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Путь до файла.
        /// Указывается в случае, если изображение храниться не в базе.
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Сравнение объектов по содержанию, а не по ссылке.
        /// </summary>
        /// <param name="obj">Картинка для сравнения с текущей.</param>
        /// <returns>Результат сравнения.</returns>
        public int Compare(object obj)
        {
            var image = (Image)obj;

            // Проверяем на равенство, если не равны, то допустим результат сравнения будет 1 - это не так важно.
            // Сравнение идет строго по важности свойств. Контент проверяется последним из-за больших объемов.
            return Name == image.Name && Format == image.Format && Height == image.Height && Width == image.Width &&
                   Data == image.Data
                       ? 0
                       : 1;
        }

        public static explicit operator string(Image value)
        {
            // Лучше будет использовать Json, но нет возможности
            var xml = new XElement("image");

            if (value.Name != null)
            {
                xml.Add(new XAttribute("name", value.Name));
            }

            if (value.Format != null)
            {
                xml.Add(new XAttribute("format", value.Format));
            }

            xml.Add(new XAttribute("Width", value.Width));
            xml.Add(new XAttribute("Height", value.Height));

            if (!string.IsNullOrEmpty(value.Data))
            {
                xml.Add(new XAttribute("rawData", value.Data));
            }

            if (!string.IsNullOrEmpty(value.URL))
            {
                xml.Add(new XAttribute("URL", value.URL));
            }

            return xml.ToString();
        }

        public static explicit operator Image(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            // Лучше будет использовать Json, но нет возможности
            var xml = XElement.Parse(value);
            if (xml == null)
            {
                throw new ArgumentException("Ошибка при десериализации");
            }

            var image = new Image();

            var attr = xml.Attribute("name");
            image.Name = attr != null ? attr.Value : null;

            attr = xml.Attribute("format");
            image.Format = attr != null ? attr.Value : null;

            attr = xml.Attribute("rawData");
            image.Data = attr != null ? attr.Value : null;

            attr = xml.Attribute("Width");
            image.Width = attr != null ? int.Parse(attr.Value) : 0;

            attr = xml.Attribute("Height");
            image.Height = attr != null ? int.Parse(attr.Value) : 0;

            attr = xml.Attribute("URL");
            image.URL = attr != null ? attr.Value : null;

            return image;
        }

        public override string ToString()
        {
            return string.Format("Изображение: {0}", Name);
        }
    }
}
