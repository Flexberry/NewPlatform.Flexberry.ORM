namespace ICSSoft.STORMNET.Convertors
{
    using System.Globalization;

    /// <summary>
    /// Позволяет использовать в строке форматирования параметр вида {* ,}.
    /// Пример: 
    /// "Результат: {*, }" c параметрами object[]{1,2,3} ->
    /// "Результат: 1, 2, 3",
    /// а формат вида: "Преобразование: {*->}" ->
    /// "Преобразование: 1->2->3".
    /// </summary>
    public sealed class Formatter
    {
        /// <summary>
        /// Кеш форматов.
        /// </summary>
        private static System.Collections.Specialized.StringDictionary formats = new System.Collections.Specialized.StringDictionary();

        /// <summary>
        /// Запрет создания экземпляров данного класса.
        /// </summary>
        private Formatter()
        {
        }

        /// <summary>
        /// Отформатировать параметры <see cref="Formatter"/>.
        /// </summary>
        /// <param name="format">Формат параметров.</param>
        /// <param name="parameters">Параметры для форматирования.</param>
        /// <returns>Отформатированные параметры.</returns>
        public static string Format(string format, params object[] parameters)
        {
            return string.Format(transfertformat(format, parameters.Length), parameters);
        }

        /// <summary>
        /// Преобразовать строку форматирования к стандартному виду.
        /// Пример:
        /// "Результат: {*, }" 
        /// с parameterscount=3 дает
        /// "Результат: {0}, {1}, {2}".
        /// </summary>
        /// <param name="format">Формат для преобразования.</param>
        /// <param name="parameterscount">Количество параметров.</param>
        /// <returns>Преобразованная строка.</returns>
        public static string transfertformat(string format, int parameterscount)
        {
            string key = parameterscount + "," + format;
            if (formats.ContainsKey(key))
                return formats[key];

            if (formats.ContainsKey(format))
                return format;

            lock (formats)
            {
                if (formats.ContainsKey(key))
                    return formats[key];

                if (formats.ContainsKey(format))
                    return format;


                int index = format.IndexOf("{* ");
                if (index == -1)
                {
                    formats.Add(format, string.Empty);
                    return format;
                }

                string pref = format.Substring(0, index);
                string and = string.Empty;
                index += 3;
                while (format[index] != '}')
                    and += format[index++].ToString(CultureInfo.InvariantCulture);
                and = " " + and + " ";
                string suf = format.Substring(index + 1);
                int ni = 0;
                while (format.IndexOf("{" + ni + "}") >= 0) ni++;
                string newformat = pref;
                if (parameterscount > ni)
                {
                    int i = ni;
                    for (; i < parameterscount - 1; i++)
                        newformat += "{" + i + "}" + and;
                    newformat += "{" + i + "}";
                }

                newformat += suf;
                formats.Add(key, newformat);

                return newformat;
            }
        }
    }
}