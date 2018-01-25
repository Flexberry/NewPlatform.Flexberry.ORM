namespace ICSSoft.STORMNET
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Класс с linq-расширениями для DataObject и его потомков.
    /// </summary>
    public static class DataObjectExtension
    {
        /// <summary>
        /// Проверить, было ли изменено свойство в сравнении с копией данных.
        /// </summary>
        /// <typeparam name="T">Тип объекта. Указывать явно не требуется.</typeparam>
        /// <param name="dataObject">Сам объект, над которым будем проводить проверку.</param>
        /// <param name="propertyExpression">Выражение, которое будет возвращать поле объекта. Например, "x => x.Свойство1".</param>
        /// <returns><c>True</c>, если значение отличается от значения в копии данных.</returns>
        public static bool IsAlteredProperty<T>(this T dataObject, Expression<Func<T, object>> propertyExpression) where T : DataObject
        {
            return dataObject.IsAlteredProperty(Information.ExtractPropertyPath(propertyExpression));
        }
    }
}
