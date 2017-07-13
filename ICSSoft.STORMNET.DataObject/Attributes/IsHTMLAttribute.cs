namespace ICSSoft.STORMNET
{
    using System;

    /// <summary>
    /// Атрибут, указывающий на то, что свойство содержит HTML.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IsHTMLAttribute : Attribute
    {
        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса IsHTML.
        /// </summary>
        /// <param name="isHtml">Содержит HTML - <c>true</c>, не содержит HTML - <c>false</c>.</param>
        public IsHTMLAttribute(bool isHtml)
        {
            Value = isHtml;
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса IsHTML.
        /// </summary>
        public IsHTMLAttribute()
        {
            Value = true;
        }

        /// <summary>
        /// Нехранимое / хранимое.
        /// </summary>
        public bool Value { get; private set; }
    }
}
