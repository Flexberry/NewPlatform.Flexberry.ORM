using System;

namespace ICSSoft.STORMNET.Business
{
    /// <summary>
    /// Summary description for DataServiceUsingTypeCustomizer.
    /// </summary>
    public struct PropertyUsingTypes
    {
        /// <summary>
        /// тип объекта данных.
        /// </summary>
        public Type DataObjectType;

        /// <summary>
        /// Свойство.
        /// </summary>
        public string PropertyName;

        /// <summary>
        /// типы которые можно использовать для этого свойства.
        /// </summary>
        public Type[] UsingTypes;

        /// <summary>
        ///
        /// </summary>
        /// <param name="doType"> тип объекта данных.</param>
        /// <param name="propName">Свойство.</param>
        /// <param name="usTypes">типы которые можно использовать для этого свойства.</param>
        public PropertyUsingTypes(Type doType, string propName, params Type[] usTypes)
        {
            DataObjectType = doType;
            PropertyName = propName;
            UsingTypes = usTypes;
        }
    }

    /// <summary>
    /// Настройка сервиса данных в части <see cref="PropertyUsingTypes"/>.
    /// </summary>
    public class DataServiceUsingTypeCustomizer
    {
        /// <summary>
        ///
        /// </summary>
        // private PropertyUsingTypes[] usingTypes;
        /// <summary>
        ///
        /// </summary>
        /// <param name="ustypes"></param>
        public DataServiceUsingTypeCustomizer(params PropertyUsingTypes[] ustypes)
        {
        }
    }
}
