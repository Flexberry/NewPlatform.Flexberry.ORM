using System;
using System.Xml.Linq;

namespace ICSSoft.STORMNET.UserDataTypes
{
    /// <summary>
    /// Класс, описывающий единичный контакт пользователя (e-mail, телефон и проч.)
    /// </summary>
    [StoreInstancesInType("ICSSoft.STORMNET.Business.C2RDataService.C2RDataService, ICSSoft.STORMNET.Business.C2RDataService",
                          "ICSSoft.STORMNET.UserDataTypes.Record, ICSSoft.STORMNET.UserDataTypes")]
    [StoreInstancesInType(typeof(Business.SQLDataService), typeof(string))]
    [Serializable]
    public class GeoData
    {
        /// <summary>
        /// Имя объекта.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Система координат.
        /// </summary>
        public string AxesSystem { get; set; }

        /// <summary>
        /// Полигоны в формате WKT (см. http://en.wikipedia.org/wiki/Well-known_text).
        /// </summary>
        public string Poligons { get; set; }

        public static explicit operator string(GeoData value)
        {
            // Лучше будет использовать Json, но нет возможности
            var xml = new XElement("geoData");
            if (value.Name != null)
            {
                xml.Add(new XAttribute("name", value.Name));
            }

            if (value.AxesSystem != null)
            {
                xml.Add(new XAttribute("axes", value.AxesSystem));
            }

            if (value.Poligons != null)
            {
                xml.Add(new XAttribute("wkt", value.Poligons));
            }

            return xml.ToString();
        }

        public static explicit operator GeoData(string value)
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

            var geoData = new GeoData();

            var attr = xml.Attribute("name");
            geoData.Name = attr != null ? attr.Value : null;

            attr = xml.Attribute("axes");
            geoData.AxesSystem = attr != null ? attr.Value : null;

            attr = xml.Attribute("wkt");
            geoData.Poligons = attr != null ? attr.Value : null;

            return geoData;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Poligons);
        }
    }
}
