namespace NewPlatform.Flexberry.Orm.KeyGen
{
    using ICSSoft.STORMNET.KeyGen;
    using System;

    /// <summary>
    /// Генератор ключей типа GUID.
    /// </summary>
    public class SystemGuidGenerator : BaseKeyGenerator
    {
        /// <summary>
        /// Генерировать Guid.
        /// </summary>
        public override object Generate(Type dataObjectType)
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Генерировать Guid.
        /// </summary>
        public override object Generate(Type dataObjectType, object sds)
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Генерировать Guid.
        /// </summary>
        public override object GenerateUniqe(Type dataObjectType)
        {
            return Generate(dataObjectType);
        }

        /// <summary>
        /// Генерировать Guid.
        /// </summary>
        public override object GenerateUniqe(Type dataObjectType, object sds)
        {
            return Generate(dataObjectType, sds);
        }

        /// <summary>
        /// Вернуть тип ключа.
        /// </summary>
        public override Type KeyType
        {
            get { return typeof(Guid); }
        }

        /// <summary>
        /// Уникален ли первичный ключ.
        /// </summary>
        public override bool Unique
        {
            get { return true; }
        }
    }
}
