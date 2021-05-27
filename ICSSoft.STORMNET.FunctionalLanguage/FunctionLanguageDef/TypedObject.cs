using System;
using ICSSoft.STORMNET;

namespace ICSSoft.STORMNET.FunctionalLanguage
{
    /// <summary>
    /// Расширение класса <see cref="ViewedObject"/> за счёт введения <see cref="ObjectType"/>-типа (атрибут Type).
    /// </summary>
    [NotStored]
    public abstract class TypedObject : ViewedObject
    {
        private ObjectType fieldType;

        /// <summary>
        /// ObjectType-тип.
        /// </summary>
        public virtual ObjectType Type
        {
            get { return fieldType; }
            set { fieldType = value; }
        }

        /// <summary>
        /// конструктор.
        /// </summary>
        public TypedObject()
        {
        }

        /// <summary>
        /// конструктор.
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="objStringedView"></param>
        /// <param name="objCaption"></param>
        /// <param name="objImagedView"></param>
        public TypedObject(ObjectType objType, string objStringedView, string objCaption)
            : base(objStringedView, objCaption)
        {
            Type = objType;
        }
    }
}
