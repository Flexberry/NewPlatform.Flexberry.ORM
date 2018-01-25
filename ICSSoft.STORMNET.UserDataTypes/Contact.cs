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
    public class Contact : IComparableType
    {
        /// <summary>
        /// Имя контакта (например, рабочий телефон)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значение контакта (например, номер телефона)
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Имя контакта (например, телефон)
        /// </summary>
        public string ContactType { get; set; }    
        
        /// <summary>
        /// Сравнение объектов по содержанию, а не по ссылке
        /// </summary>
        /// <param name="obj">Контакт для сравнения с текущим</param>
        /// <returns>Результат сравнения</returns>
        public int Compare(object obj)
        {
            var contact = (Contact) obj;

            // проверяем на равенство, если не равны, то допустим результат сравнения будет 1 - это не так важно.
            return Name == contact.Name && Value == contact.Value && ContactType == contact.ContactType
                       ? 0
                       : 1;
        }

        public static explicit operator string(Contact value)
        {
            // Лучше будет использовать Json, но нет возможности
            var xml = new XElement("contact");
            if (value.ContactType != null) xml.Add(new XAttribute("type", value.ContactType));
            if (value.Name != null) xml.Add(new XAttribute("name", value.Name));
            if (value.Value != null) xml.Add(new XAttribute("value", value.Value));

            return xml.ToString();
        }

        public static explicit operator Contact(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            // Лучше будет использовать Json, но нет возможности
            var xml = XElement.Parse(value);
            if (xml == null) throw new ArgumentException("Ошибка при десериализации");

            var contact = new Contact();

            var attr = xml.Attribute("type");
            contact.ContactType = attr != null ? attr.Value : null;

            attr = xml.Attribute("name");
            contact.Name = attr != null ? attr.Value : null;

            attr = xml.Attribute("value");
            contact.Value = attr != null ? attr.Value : null;

            return contact;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Value);
        }
    }
}