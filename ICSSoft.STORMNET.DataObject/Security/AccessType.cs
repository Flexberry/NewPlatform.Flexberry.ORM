namespace ICSSoft.STORMNET
{
    using System;

    /// <summary>
    /// Тип проверки полномочий для объекта
    /// </summary>
    public enum AccessType
    {
        /// <summary>
        /// не производится никакой проверки
        /// </summary>
        none,

        /// <summary>
        /// производится проверка только над текущим классом
        /// </summary>
        @this,

        /// <summary>
        /// производится проверка для базового класса
        /// </summary>
        @base,

        /// <summary>
        /// производится проверка как для базового класса, так и для текущего
        /// </summary>
        @this_and_base
    }

    /// <summary>
    /// Помещать ли свойство в автоматически генерируемые прадставления
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AccessTypeAttribute : System.Attribute
    {
        /// <summary>
        /// Значение (true/false)
        /// </summary>
        public AccessType value = AccessType.none;

        /// <summary>
        ///
        /// </summary>
        /// <param name="val"></param>
        public AccessTypeAttribute(AccessType val)
        {
            value = val;
        }
    }
}