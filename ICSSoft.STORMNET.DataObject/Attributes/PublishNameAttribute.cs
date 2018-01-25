using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSSoft.STORMNET
{
    /// <summary>
    /// Атрибут устанавливающий имя типа и набора сущностей при использовании в ODataService.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class PublishNameAttribute : System.Attribute
    {
        private string typePublishName;
        private string entitySetPublishName;

        /// <summary>
        /// Конструктор атрибута для класса.
        /// </summary>
        /// <param name="typePublishName">Имя типа для публикации.</param>
        /// <param name="entitySetPublishName">Имя набора сущностей для публикации.</param>
        public PublishNameAttribute(string typePublishName, string entitySetPublishName)
        {
            this.typePublishName = typePublishName;
            this.entitySetPublishName = entitySetPublishName;
        }

        /// <summary>
        /// Конструктор атрибута для свойства.
        /// </summary>
        /// <param name="typePublishName">Имя типа для публикации.</param>
        /// <param name="entitySetPublishName">Имя набора сущностей для публикации.</param>
        public PublishNameAttribute(string typePublishName)
        {
            this.typePublishName = typePublishName;
        }

        /// <summary>
        /// Имя типа для публикации.
        /// </summary>
        public string TypePublishName { get { return typePublishName; } }

        /// <summary>
        /// Имя набора сущностей для публикации.
        /// </summary>
        public string EntitySetPublishName { get { return entitySetPublishName; } }
    }
}
