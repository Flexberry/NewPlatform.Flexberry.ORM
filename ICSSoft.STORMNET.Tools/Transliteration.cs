namespace ICSSoft.STORMNET.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Класс, организующий транслитерацию из русского в английский и наоборот.
    /// (Класс ранее был в asp-генераторе).
    /// </summary>
    public static class Transliteration
    {
        /// <summary>
        /// Используемый при транслитерации стандарт.
        /// </summary>
        public enum TransliterationType
        {
            /// <summary>
            /// ГОСТ 16876-71
            /// </summary>
            Gost,

            /// <summary>
            /// ISO 9-95
            /// </summary>
            ISO,
        }

        /// <summary>
        /// Словарь, содержащий транслитерацию, соответствующую ГОСТ 16876-71.
        /// </summary>
        private static Dictionary<string, string> gost = new Dictionary<string, string>(); // ГОСТ 16876-71

        /// <summary>
        /// Словарь, содержащий транслитерацию, соответствующую ISO 9-95.
        /// </summary>
        private static Dictionary<string, string> iso = new Dictionary<string, string>(); // ISO 9-95

        /// <summary>
        /// Регулярное выражение для поиска английских символов.
        /// </summary>
        private static Regex regexToSearchEnglish = new Regex("[a-zA-Z]*");

        /// <summary>
        /// Перевод с русского на английский (по стандарту ISO 9-95).
        /// </summary>
        /// <param name="text">Переводимый текст на русском языке.</param>
        /// <returns>Переведённый текст на английском языке.</returns>
        public static string Front(string text)
        {
            return Front(text, TransliterationType.ISO);
        }

        /// <summary>
        /// Перевод с русского на английский по указанному стандарту.
        /// (Если в результате транслитерации получится пустая строка, то вернётся ""Transliteration" + random").
        /// </summary>
        /// <param name="text">Переводимый текст на русском языке.</param>
        /// <param name="type">Используемый стандарт.</param>
        /// <returns>Переведённый текст на английском языке.</returns>
        public static string Front(string text, TransliterationType type)
        {
            string output = text;
            Dictionary<string, string> tdict = GetDictionaryByType(type);

            foreach (KeyValuePair<string, string> key in tdict)
            {
                output = output.Replace(key.Key, key.Value);
            }

            if (string.IsNullOrEmpty(text) || text.Trim() == string.Empty)
            {
                output = "Transliteration" + DateTime.Now.Ticks;
            }

            return output;
        }

        /// <summary>
        /// Перевод с английского на русский (по стандарту ISO 9-95).
        /// </summary>
        /// <param name="text">Переводимый текст на английском языке.</param>
        /// <returns>Переведённый текст на русском языке.</returns>
        public static string Back(string text)
        {
            return Back(text, TransliterationType.ISO);
        }

        /// <summary>
        /// Перевод с английского на русский по указанному стандарту.
        /// (Если в результате транслитерации получится пустая строка, то вернётся ""Транслитерация" + random").
        /// </summary>
        /// <param name="text">Переводимый текст на английском языке.</param>
        /// <param name="type">Используемый стандарт.</param>
        /// <returns>Переведённый текст на русском языке.</returns>
        public static string Back(string text, TransliterationType type)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            string output = text;
            Dictionary<string, string> tdict = GetDictionaryByType(type);

            foreach (KeyValuePair<string, string> key in tdict.Where(x => !string.IsNullOrEmpty(x.Value)))
            {
                output = output.Replace(key.Value, key.Key);
            }

            output = ReleaseStringFromEnglishSymbols(output);

            if (string.IsNullOrEmpty(output) || text.Trim() == string.Empty)
            {
                output = "Транслитерация" + DateTime.Now.Ticks;
            }

            return output;
        }

        /// <summary>
        /// Удалить из строки оставшиеся символы английского алфавита.
        /// </summary>
        /// <param name="releaseString">Строка, из которой необходимо убрать символы.</param>
        /// <returns>Строка без английских символов.</returns>
        private static string ReleaseStringFromEnglishSymbols(string releaseString)
        {
            return regexToSearchEnglish.Replace(releaseString, string.Empty);
        }

        /// <summary>
        /// Получение словаря для словаря для указанного стандарта.
        /// </summary>
        /// <param name="type">Стандарт транслитерации.</param>
        /// <returns>Словарь стандарта.</returns>
        private static Dictionary<string, string> GetDictionaryByType(TransliterationType type)
        {
            Dictionary<string, string> tdict = iso;
            if (type == TransliterationType.Gost)
            {
                tdict = gost;
            }

            return tdict;
        }

        /// <summary>
        /// Заполнение словарей стандартов.
        /// </summary>
        static Transliteration()
        {
            // gost.Add("╙", "EH");
            // gost.Add("╡", "I");
            // gost.Add("Ё", "i");
            // gost.Add("╧", "#");
            // gost.Add("╨", "eh");
            gost.Add("Щ", "SHH"); // Сортировка идёт по количеству символов.
            gost.Add("щ", "shh");
            gost.Add("Ё", "JO");
            gost.Add("Ж", "ZH");
            gost.Add("Ч", "CH");
            gost.Add("Й", "JJ");
            gost.Add("Х", "KH");
            gost.Add("Ш", "SH");
            gost.Add("Э", "EH");
            gost.Add("Ю", "YU");
            gost.Add("Я", "YA");
            gost.Add("ё", "jo");
            gost.Add("ж", "zh");
            gost.Add("й", "jj");
            gost.Add("х", "kh");
            gost.Add("ч", "ch");
            gost.Add("ш", "sh");
            gost.Add("А", "A");
            gost.Add("Б", "B");
            gost.Add("В", "V");
            gost.Add("Г", "G");
            gost.Add("Д", "D");
            gost.Add("Е", "E");
            gost.Add("З", "Z");
            gost.Add("И", "I");
            gost.Add("К", "K");
            gost.Add("Л", "L");
            gost.Add("М", "M");
            gost.Add("Н", "N");
            gost.Add("О", "O");
            gost.Add("П", "P");
            gost.Add("Р", "R");
            gost.Add("С", "S");
            gost.Add("Т", "T");
            gost.Add("У", "U");
            gost.Add("Ф", "F");
            gost.Add("Ц", "C");
            gost.Add("Ы", "Y");
            gost.Add("а", "a");
            gost.Add("б", "b");
            gost.Add("в", "v");
            gost.Add("г", "g");
            gost.Add("д", "d");
            gost.Add("е", "e");
            gost.Add("з", "z");
            gost.Add("и", "i");
            gost.Add("к", "k");
            gost.Add("л", "l");
            gost.Add("м", "m");
            gost.Add("н", "n");
            gost.Add("о", "o");
            gost.Add("п", "p");
            gost.Add("р", "r");
            gost.Add("с", "s");
            gost.Add("т", "t");
            gost.Add("у", "u");
            gost.Add("ф", "f");
            gost.Add("ц", "c");
            gost.Add("ы", "y");
            gost.Add("э", "eh");
            gost.Add("ю", "yu");
            gost.Add("я", "ya");
            gost.Add("Ъ", "'");
            gost.Add("ъ", "'");
            gost.Add("ь", string.Empty);
            gost.Add("Ь", string.Empty);

            // gost.Add("╚", "");
            // gost.Add("╩", "");
            // gost.Add("≈", "-");

            // iso.Add("╙", "YE");
            // iso.Add("╡", "I");
            // iso.Add("│", "G");
            // iso.Add("Ё", "i");
            // iso.Add("╧", "#");
            // iso.Add("╨", "ye");
            // iso.Add("┐", "g");
            iso.Add("Щ", "SHH"); // Сортировка идёт по количеству символов.
            iso.Add("щ", "shh");
            iso.Add("Ё", "YO");
            iso.Add("Ж", "ZH");
            iso.Add("Ч", "CH");
            iso.Add("Ш", "SH");
            iso.Add("Ю", "YU");
            iso.Add("Я", "YA");
            iso.Add("ё", "yo");
            iso.Add("ж", "zh");
            iso.Add("ч", "ch");
            iso.Add("ш", "sh");
            iso.Add("ю", "yu");
            iso.Add("я", "ya");
            iso.Add("А", "A");
            iso.Add("Б", "B");
            iso.Add("В", "V");
            iso.Add("Г", "G");
            iso.Add("Д", "D");
            iso.Add("Е", "E");
            iso.Add("З", "Z");
            iso.Add("И", "I");
            iso.Add("Й", "J");
            iso.Add("К", "K");
            iso.Add("Л", "L");
            iso.Add("М", "M");
            iso.Add("Н", "N");
            iso.Add("О", "O");
            iso.Add("П", "P");
            iso.Add("Р", "R");
            iso.Add("С", "S");
            iso.Add("Т", "T");
            iso.Add("У", "U");
            iso.Add("Ф", "F");
            iso.Add("Х", "X");
            iso.Add("Ц", "C");
            iso.Add("Ы", "Y");
            iso.Add("Э", "E");
            iso.Add("а", "a");
            iso.Add("б", "b");
            iso.Add("в", "v");
            iso.Add("г", "g");
            iso.Add("д", "d");
            iso.Add("е", "e");
            iso.Add("з", "z");
            iso.Add("и", "i");
            iso.Add("й", "j");
            iso.Add("к", "k");
            iso.Add("л", "l");
            iso.Add("м", "m");
            iso.Add("н", "n");
            iso.Add("о", "o");
            iso.Add("п", "p");
            iso.Add("р", "r");
            iso.Add("с", "s");
            iso.Add("т", "t");
            iso.Add("у", "u");
            iso.Add("ф", "f");
            iso.Add("х", "x");
            iso.Add("ц", "c");
            iso.Add("ы", "y");
            iso.Add("э", "e");
            iso.Add("ъ", "'");
            iso.Add("Ъ", "'");
            iso.Add("Ь", string.Empty);
            iso.Add("ь", string.Empty);

            // iso.Add("╚", "");
            // iso.Add("╩", "");
            // iso.Add("≈", "-");
        }
    }
}
