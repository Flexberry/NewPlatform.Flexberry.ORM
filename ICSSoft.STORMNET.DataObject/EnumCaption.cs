namespace ICSSoft.STORMNET
{
    using System;
    using System.Linq;
    using System.Reflection;
    using ICSSoft.STORMNET.Exceptions;

    /// <summary>
    /// Класс для работы с заголовками перечислений.
    /// </summary>
    public sealed class EnumCaption
    {
        /// <summary>
        /// Кеш заголовков для перечислений.
        /// </summary>
        private static System.Collections.Hashtable captions = new System.Collections.Hashtable();

        /// <summary>
        /// Кеш значений перечислений, которые соответствуют заголовкам.
        /// </summary>
        private static System.Collections.Hashtable values = new System.Collections.Hashtable();

        /// <summary>
        /// Создание инстанций данного класса запрещаем.
        /// </summary>
        private EnumCaption()
        {
        }
        
        /// <summary>
        /// Получить заголовок по значению перечислимого типа из CaptionAttribute.
        /// </summary>
        /// <param name="value">Значение перечислимого типа.</param>
        /// <returns>Соответствующий заголовок.</returns>
        public static string GetCaptionFor(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            lock (captions)
            {
                if (captions.ContainsKey(value))
                    return (string)captions[value];
                string res;
                Type enumType = value.GetType();
                if (enumType.IsEnum)
                {
                    FieldInfo fi = enumType.GetField(value.ToString());
                    if (fi == null)
                    {
                        throw new Exception("Не найдено значение " + value + " в перечислении " + enumType.Name + ". Обратитесь к разработчикам.");
                    }

                    object[] atrs = fi.GetCustomAttributes(typeof(CaptionAttribute), true);
                    res = atrs.Length == 0 ? value.ToString() : ((CaptionAttribute)atrs[0]).Value;
                }
                else
                {
                    throw new NotEnumTypeException(enumType.Name);
                }

                captions.Add(value, res);
                return res;
            }
        }

        /// <summary>
        /// Получить enum-значение по заголовку.
        /// </summary>
        /// <param name="caption">Заголовок значения перечисления.</param>
        /// <param name="enumType">Тип перечисления.</param>
        /// <returns>Значение перечислимого типа, соответствующее заголовку.</returns>
        public static object GetValueFor(string caption, Type enumType)
        {
            lock (values)
            {
                if (caption == null) caption = string.Empty;
                caption = caption.Trim();
                string valueKey = enumType.FullName + "%" + caption;
                if (values.ContainsKey(valueKey))
                {
                    return values[valueKey];
                }

                if (enumType.IsEnum)
                {
                    FieldInfo[] fis = enumType.GetFields();
                    string caseInsensitiveVal = null;
                    for (int i = 0; i < fis.Length; i++)
                    {
                        if (fis[i].IsSpecialName)
                        {
                            continue;
                        }

                        object[] atrs = fis[i].GetCustomAttributes(typeof(CaptionAttribute), false);
                        if (atrs.Length > 0)
                        {
                            string val = ((CaptionAttribute)atrs[0]).Value ?? string.Empty;

                            if (string.Equals(val.Trim(), caption))
                            {
                                object res = Enum.Parse(enumType, fis[i].Name);
                                values.Add(valueKey, res);
                                return res;
                            }

                            if (string.Equals(val.Trim(), caption, StringComparison.OrdinalIgnoreCase))
                            {
                                caseInsensitiveVal = fis[i].Name;
                            }
                        }
                    }

                    if (caseInsensitiveVal != null)
                    {
                        object res = Enum.Parse(enumType, caseInsensitiveVal, true);
                        values.Add(valueKey, res);
                        return res;
                    }

                    // Явная проверка, т.к. Enum.TryParse появился в .NET 4.
                    if (Enum.IsDefined(enumType, caption))
                    {
                        object res = Enum.Parse(enumType, caption);
                        values.Add(valueKey, res);
                        return res;
                    }

                    var allNames = Enum.GetNames(enumType);

                    // Case insensitive для обратной совместимости.
                    if (allNames.Any(x => string.Equals(x, caption, StringComparison.OrdinalIgnoreCase)))
                    {
                        object res = Enum.Parse(enumType, caption, true);
                        values.Add(valueKey, res);
                        return res;
                    }

                    throw new ArgumentException("Can't parse caption for enum");
                }

                return null;
            }
        }

        /// <summary>
        /// Метод для получения значения перечисления по заголовку без генерации исключений.
        /// </summary>
        /// <typeparam name="TEnum">Перечислимый тип.</typeparam>
        /// <param name="caption">Заголовок значения перечислимого типа.</param>
        /// <param name="enumValue">Значение перечислимого типа, соответствующее заголовку <paramref name="caption"/>.</param>
        /// <returns>Если удалось найти соответствие, то <c>true</c>, иначе - <c>false</c>.</returns>
        public static bool TryGetValueFor<TEnum>(string caption, out TEnum enumValue)
        {
            try
            {
                enumValue = (TEnum)GetValueFor(caption, typeof(TEnum));
                return true;
            }
            catch (Exception)
            {
                enumValue = default(TEnum);
                return false;
            }
        }
    }
}