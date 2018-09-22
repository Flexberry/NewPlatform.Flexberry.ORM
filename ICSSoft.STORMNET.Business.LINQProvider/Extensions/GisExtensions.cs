namespace ICSSoft.STORMNET.Business.LINQProvider.Extensions
{
#if NETFX_45
    using Microsoft.Spatial;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Методы расширения Gis для LINQProvider.
    /// </summary>
    public static class GisExtensions
    {
        /// <summary>
        /// Тестирует пересечение двух объектов Geography.
        /// Метод может использоваться только в запросе к LINQProvider. В остальных случаях всегда будет возвращать false.
        /// </summary>
        /// <param name="geo1">Объект 1 Geography.</param>
        /// <param name="geo2">Объект 2 Geography.</param>
        /// <returns>Если существует пересечение двух объектов, то возвращает true, иначе false.</returns>
        public static bool GeoIntersects(this Geography geo1, Geography geo2)
        {
            return false;
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
        /// Получает строку в формате WKT из объекта Geography.
        /// </summary>
        /// <param name="geo">Объект Geography.</param>
        /// <returns>Cтрока в формате WKT.</returns>
        public static string GetWKT(this Geography geo)
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
    }
#endif
}
