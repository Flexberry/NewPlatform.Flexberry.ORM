namespace ICSSoft.STORMNET.KeyGen
{
    using System;

    /// <summary>
    /// Абстрактный генератор ключей.
    /// </summary>
    public abstract class BaseKeyGenerator
    {
        /// <summary>
        /// Генерировать первичный ключ.
        /// </summary>
        public abstract object Generate(Type dataObjectType);

        /// <summary>
        /// Генерировать первичный ключ.
        /// </summary>
        public abstract object Generate(Type dataObjectType, object sds);

        /// <summary>
        /// Генерировать первичный ключ.
        /// </summary>
        public abstract object GenerateUniqe(Type dataObjectType);

        /// <summary>
        /// Генерировать первичный ключ.
        /// </summary>
        public abstract object GenerateUniqe(Type dataObjectType, object sds);

        /// <summary>
        /// Вернуть тип ключа.
        /// </summary>
        public abstract Type KeyType { get; }

        /// <summary>
        /// Уникален ли первичный ключ.
        /// </summary>
        public abstract bool Unique { get; }
    }
}