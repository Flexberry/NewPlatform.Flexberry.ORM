namespace ICSSoft.STORMNET.Tools
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// Бинарный сереализатор.
    /// </summary>
    public class BinarySerializer
    {
        /// <summary>
        /// Десереализация объекта сереализованного ранее в бинарный формат.
        /// </summary>
        /// <typeparam name="TData">
        /// Тип с атрибутом <see cref="SerializableAttribute"/>.
        /// Типы всех сереализуемых полей и свойств класса должны также иметь этот атрибут.
        /// </typeparam>
        /// <param name="serializedObject">
        /// Сереализованный прежде объект в виде строки.
        /// </param>
        /// <returns>Десереализованный объект.</returns>
        public static TData DeserializeFromString<TData>(string serializedObject)
        {
            byte[] bytes = Convert.FromBase64String(serializedObject);
            return DeserializeFromBytes<TData>(bytes);
        }

        /// <summary>
        /// Сереализация объекта в бинарный формат.
        /// </summary>
        /// <typeparam name="TData">
        /// Тип с атрибутом <see cref="SerializableAttribute"/>.
        /// Типы всех сереализуемых полей и свойств класса должны также иметь этот атрибут.
        /// </typeparam>
        /// <param name="object">
        /// Объект, который необходимо сереализовать.
        /// </param>
        /// <returns>Сереализованный объект в виде строки.</returns>
        public static string SerializeToString<TData>(TData @object)
        {
            return Convert.ToBase64String(SerializeToBytes(@object));
        }

        /// <summary>
        /// Десереализация объекта сереализованного ранее в бинарный формат.
        /// </summary>
        /// <typeparam name="TData">
        /// Тип с атрибутом <see cref="SerializableAttribute"/>.
        /// Типы всех сереализуемых полей и свойств класса должны также иметь этот атрибут.
        /// </typeparam>
        /// <param name="serializedObject">
        /// Сереализованный прежде объект в виде массива байт.
        /// </param>
        /// <returns>Десереализованный объект.</returns>
        public static TData DeserializeFromBytes<TData>(byte[] serializedObject)
        {
            using (var stream = new MemoryStream(serializedObject))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (TData)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Сереализация объекта в бинарный формат.
        /// </summary>
        /// <typeparam name="TData">
        /// Тип с атрибутом <see cref="SerializableAttribute"/>.
        /// Типы всех сереализуемых полей и свойств класса должны также иметь этот атрибут.
        /// </typeparam>
        /// <param name="object">
        /// Объект, который необходимо сереализовать.
        /// </param>
        /// <returns>Сереализованный объект в виде массива байт.</returns>
        public static byte[] SerializeToBytes<TData>(TData @object)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, @object);
                stream.Flush();
                stream.Position = 0;
                return stream.ToArray();
            }
        }
    }
}
