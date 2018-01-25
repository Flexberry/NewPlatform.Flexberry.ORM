namespace ICSSoft.STORMNET.Tools
{
    using System;
    using System.Globalization;
    
    /// <summary>
    /// Вспомагательный класс для работы <see cref="XMLManager"/>.
    /// Содержит методы для работы с типами данных.
    /// </summary>
    public class TypeManager
    {
        /// <summary>
        /// Строковый формат числа с запятой.
        /// </summary>
        private static NumberFormatInfo _formatComma = new NumberFormatInfo { NumberDecimalSeparator = "," };

        /// <summary>
        /// Строковый формат числа с точкой.
        /// </summary>
        private static NumberFormatInfo _formatPoint = new NumberFormatInfo { NumberDecimalSeparator = "." };
        
        /// <summary>
        /// Попробовать распарсить <see cref="Double"/> из строки без учета культуры.
        /// </summary>
        /// <param name="value">Строковое значение, которое следует распарсить.</param>
        /// <param name="result">Распарсеное значение. Результат аналогичен при работе метода <see cref="double.TryParse(string, out double)"/>.</param>
        /// <returns>Удалось ли распарсить значение из строки.</returns>
        public static bool TryParseDouble(string value, out double result)
        {
            NumberFormatInfo format = GetFormatForString(value);
            return double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, format, out result);
        }

        /// <summary>
        /// Попробовать распарсить <see cref="Decimal"/> из строки без учета культуры.
        /// </summary>
        /// <param name="value">Строковое значение, которое следует распарсить.</param>
        /// <param name="result">Распарсеное значение. Результат аналогичен при работе метода <see cref="decimal.TryParse(string, out decimal)"/>.</param>
        /// <returns>Удалось ли распарсить значение из строки.</returns>
        public static bool TryParseDecimal(string value, out decimal result)
        {
            NumberFormatInfo format = GetFormatForString(value);
            return decimal.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, format, out result);
        }
        
        /// <summary>
        /// Поллучить актуальный формат для парсинга числа из строки.
        /// </summary>
        /// <param name="stringValue">Строка для которой будет получен формат.</param>
        /// <returns>Формат для парсинга числа из строки.</returns>
        private static NumberFormatInfo GetFormatForString(string stringValue)
        {
            return stringValue.Contains(",") ? _formatComma : _formatPoint;
        }
    }
}
