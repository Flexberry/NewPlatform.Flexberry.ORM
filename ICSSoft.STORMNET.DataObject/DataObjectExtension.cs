namespace ICSSoft.STORMNET
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Класс с linq-расширениями для DataObject и его потомков.
    /// </summary>
    public static class DataObjectExtension
    {
        /// <summary>
        /// Проверить, установлено ли значение в указанное свойство
        /// (требуется в случае, когда состояние загрузки -- LightLoaded).
        /// </summary>
        /// <typeparam name="T">Тип объекта. Указывать явно не требуется.</typeparam>
        /// <param name="dataObject">Сам объект, над которым будем проводить проверку.</param>
        /// <param name="propertyExpression">Выражение, которое будет возвращать поле объекта. Например, "x => x.Свойство1".</param>
        /// <returns><c>True</c>, если значение инициировано в копии данных.</returns>
        public static bool CheckLoadedProperty<T>(this T dataObject, Expression<Func<T, object>> propertyExpression) where T : DataObject
        {
            return dataObject.CheckLoadedProperty(Information.ExtractPropertyPath(propertyExpression));
        }

        /// <summary>
        /// Проверить, установлены ли значение в указанных свойствах
        /// (требуется в случае, когда состояние загрузки -- LightLoaded).
        /// </summary>
        /// <typeparam name="T">Тип объекта. Указывать явно не требуется.</typeparam>
        /// <param name="dataObject">Сам объект, над которым будем проводить проверку.</param>
        /// <param name="propertyExpressions">Перечисление выражений, которые будут возвращать поле объекта. Например, "x => x.Свойство1".</param>
        /// <returns><c>True</c>, если все перечисленные свойства инициированы в копии данных.</returns>
        public static bool CheckLoadedProperties<T>(this T dataObject, params Expression<Func<T, object>>[] propertyExpressions) where T : DataObject
        {
            return propertyExpressions.All(dataObject.CheckLoadedProperty);
        }

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

        /// <summary>
        /// Проверить, было ли изменены свойства в сравнении с копией данных.
        /// </summary>
        /// <typeparam name="T">Тип объекта. Указывать явно не требуется.</typeparam>
        /// <param name="dataObject">Сам объект, над которым будем проводить проверку.</param>
        /// <param name="propertyExpressions">Перечисление выражений, которые будут возвращать поле объекта. Например, "x => x.Свойство1".</param>
        /// <returns><c>True</c>, если хотя бы одно значение отличается от значений в копии данных.</returns>
        public static bool IsAlteredProperties<T>(this T dataObject, params Expression<Func<T, object>>[] propertyExpressions) where T : DataObject
        {
            return propertyExpressions.Any(dataObject.IsAlteredProperty);
        }
    }
}
