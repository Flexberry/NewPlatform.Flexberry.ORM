namespace ICSSoft.STORMNET.Business.LINQProvider.Extensions
{
    using System;
    using System.IO;

    using Microsoft.Spatial;

    /// <summary>
    /// Методы расширения Gis для LINQProvider.
    /// </summary>
    public static class GisExtensions
    {
        /// <summary>
        /// Вычисляет расстояние между двумя объектами Geography.
        /// Метод может использоваться только в запросе к LINQProvider,
        /// в остальных случаях метод вызовет исключение NotImplementedException.
        /// </summary>
        /// <param name="geo1">Объект 1 Geography.</param>
        /// <param name="geo2">Объект 2 Geography.</param>
        /// <returns>Возвращает минимальное расстояние между двумя объектами.</returns>
        public static double GeoDistance(this Geography geo1, Geography geo2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Вычисляет расстояние между двумя объектами Geometry.
        /// Метод может использоваться только в запросе к LINQProvider,
        /// в остальных случаях метод вызовет исключение NotImplementedException.
        /// </summary>
        /// <param name="geo1">Объект 1 Geometry.</param>
        /// <param name="geo2">Объект 2 Geometry.</param>
        /// <returns>Возвращает минимальное расстояние между двумя объектами.</returns>
        public static double GeomDistance(this Geometry geo1, Geometry geo2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Тестирует пересечение двух объектов Geography.
        /// Метод может использоваться только в запросе к LINQProvider,
        /// в остальных случаях метод вызовет исключение NotImplementedException.
        /// </summary>
        /// <param name="geo1">Объект 1 Geography.</param>
        /// <param name="geo2">Объект 2 Geography.</param>
        /// <returns>Если существует пересечение двух объектов, то возвращает true, иначе false.</returns>
        public static bool GeoIntersects(this Geography geo1, Geography geo2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Тестирует пересечение двух объектов Geometry.
        /// Метод может использоваться только в запросе к LINQProvider,
        /// в остальных случаях метод вызовет исключение NotImplementedException.
        /// </summary>
        /// <param name="geo1">Объект 1 Geometry.</param>
        /// <param name="geo2">Объект 2 Geometry.</param>
        /// <returns>Если существует пересечение двух объектов, то возвращает true, иначе false.</returns>
        public static bool GeomIntersects(this Geometry geo1, Geometry geo2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создаёт объект Geography.
        /// </summary>
        /// <param name="wkt">Строка в формате WKT или EWKT.</param>
        /// <returns>Объект Geography.</returns>
        public static Geography CreateGeography(this string wkt)
        {
            WellKnownTextSqlFormatter wktFormatter = WellKnownTextSqlFormatter.Create();
            return wktFormatter.Read<Geography>(new StringReader(wkt));
        }

        /// <summary>
        /// Создаёт объект Geometry.
        /// </summary>
        /// <param name="wkt">Строка в формате WKT или EWKT.</param>
        /// <returns>Объект Geometry.</returns>
        public static Geometry CreateGeometry(this string wkt)
        {
            WellKnownTextSqlFormatter wktFormatter = WellKnownTextSqlFormatter.Create();
            return wktFormatter.Read<Geometry>(new StringReader(wkt));
        }

        /// <summary>
        /// Получает строку в формате EWKT из объекта Geography.
        /// </summary>
        /// <param name="geo">Объект Geography.</param>
        /// <returns>Cтрока в формате EWKT.</returns>
        public static string GetEWKT(this Geography geo)
        {
            WellKnownTextSqlFormatter wktFormatter = WellKnownTextSqlFormatter.Create();
            StringWriter wr = new StringWriter();
            wktFormatter.Write(geo, wr);
            return wr.ToString();
        }

        /// <summary>
        /// Получает строку в формате EWKT из объекта Geometry.
        /// </summary>
        /// <param name="geo">Объект Geometry.</param>
        /// <returns>Cтрока в формате EWKT.</returns>
        public static string GetEWKT(this Geometry geo)
        {
            WellKnownTextSqlFormatter wktFormatter = WellKnownTextSqlFormatter.Create();
            StringWriter wr = new StringWriter();
            wktFormatter.Write(geo, wr);
            return wr.ToString();
        }

        /// <summary>
        /// Получает строку в формате WKT из объекта Geography.
        /// </summary>
        /// <param name="geo">Объект Geography.</param>
        /// <returns>Cтрока в формате WKT.</returns>
        public static string GetWKT(this Geography geo)
        {
            return geo.GetEWKT().Replace($"SRID={geo.CoordinateSystem.Id};", string.Empty);
        }

        /// <summary>
        /// Получает строку в формате WKT из объекта Geometry.
        /// </summary>
        /// <param name="geo">Объект Geometry.</param>
        /// <returns>Cтрока в формате WKT.</returns>
        public static string GetWKT(this Geometry geo)
        {
            return geo.GetEWKT().Replace($"SRID={geo.CoordinateSystem.Id};", string.Empty);
        }

        /// <summary>
        /// Получает SRID из объекта Geography.
        /// </summary>
        /// <param name="geo">Объект Geography.</param>
        /// <returns>Cтрока SRID.</returns>
        public static string GetSRID(this Geography geo)
        {
            return geo.CoordinateSystem.Id;
        }

        /// <summary>
        /// Получает SRID из объекта Geometry.
        /// </summary>
        /// <param name="geo">Объект Geometry.</param>
        /// <returns>Cтрока SRID.</returns>
        public static string GetSRID(this Geometry geo)
        {
            return geo.CoordinateSystem.Id;
        }
    }
}
