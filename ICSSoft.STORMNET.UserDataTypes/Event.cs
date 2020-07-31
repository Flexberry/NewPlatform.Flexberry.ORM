using System;
using System.Xml.Linq;

namespace ICSSoft.STORMNET.UserDataTypes
{
    /// <summary>
    /// Пользовательский тип - событие;.
    /// </summary>
    [StoreInstancesInType("ICSSoft.STORMNET.Business.C2RDataService.C2RDataService, ICSSoft.STORMNET.Business.C2RDataService",
                          "ICSSoft.STORMNET.UserDataTypes.Record, ICSSoft.STORMNET.UserDataTypes")]
    [StoreInstancesInType(typeof(Business.SQLDataService), typeof(string))]
    [Serializable]
    public class Event
    {
        /// <summary>
        /// Тема события.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание события.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Время начала события.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Время конца события.
        /// </summary>
        public NullableDateTime FinishTime { get; set; }

        /// <summary>
        /// Автор события.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Место события.
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// Категория события (info / warning / error / проч).
        /// </summary>
        public string Category { get; set; }

        public static explicit operator string(Event value)
        {
            // Лучше будет использовать Json, но нет возможности
            var xml = new XElement("event");

            if (value.Name != null)
            {
                xml.Add(new XAttribute("type", value.Name));
            }

            if (value.Description != null)
            {
                xml.Add(new XAttribute("description", value.Description));
            }

            if (value.Author != null)
            {
                xml.Add(new XAttribute("author", value.Author));
            }

            if (value.Place != null)
            {
                xml.Add(new XAttribute("place", value.Place));
            }

            if (value.Category != null)
            {
                xml.Add(new XAttribute("category", value.Category));
            }

            if (value.StartTime != DateTime.MinValue)
            {
                xml.Add(new XAttribute("start", value.StartTime));
            }

            if (value.FinishTime != null)
            {
                xml.Add(new XAttribute("finish", value.FinishTime));
            }

            return xml.ToString();
        }

        public static explicit operator Event(string value)
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

            var eventValue = new Event();

            var attr = xml.Attribute("name");
            eventValue.Name = attr != null ? attr.Value : null;

            attr = xml.Attribute("description");
            eventValue.Description = attr != null ? attr.Value : null;

            attr = xml.Attribute("start");
            eventValue.StartTime = attr != null ? DateTime.Parse(attr.Value) : DateTime.MinValue;

            attr = xml.Attribute("finish");
            eventValue.FinishTime = attr != null ? new NullableDateTime { Value = DateTime.Parse(attr.Value) } : null;

            attr = xml.Attribute("author");
            eventValue.Author = attr != null ? attr.Value : null;

            attr = xml.Attribute("place");
            eventValue.Place = attr != null ? attr.Value : null;

            attr = xml.Attribute("category");
            eventValue.Category = attr != null ? attr.Value : null;

            return eventValue;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", StartTime, Name);
        }
    }
}
