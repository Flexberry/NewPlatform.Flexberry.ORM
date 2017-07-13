using System.Runtime.Serialization;

namespace ICSSoft.STORMNET.UserDataTypes
{
    /// <summary>
    /// Бинарный объект.
    /// Используется для хранения данных в хранилищах поддерживающих бинарные объекты.
    /// </summary>
    [DataContract]
    public class Blob
    {
        /// <summary>
        /// Наименование. Используется по разному.  
        /// Например, после десериализации от Lily-сервера поле Name остается пустым - фактически Name толком не используется.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Размеры бинарного объекта (колличество байт).
        /// </summary>
        [DataMember(Name = "size")]
        public long Size { get; set; }

        /// <summary>
        /// Тип бинарного объекта.
        /// Бинарный объект имеет свои типа специфичные для каждого хранилища в отдельности.
        /// </summary>
        [DataMember(Name = "mediaType")]
        public string MimeType { get; set; }

        /// <summary>
        /// Значение бинарного объекта в виде набора байт.
        /// </summary>
        [DataMember(Name = "value")]
        public byte[] Value { get; set; }       
    }
}