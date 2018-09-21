namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Обертка для работы с путем до мастерового свойства.
    /// </summary>
    public class MasterPropertyPath
    {
        /// <summary>
        /// Разделитель по умолчанию между частями пути к мастеровому свойству.
        /// </summary>
        public const string DefaultSeparator = ".";

        /// <summary>
        /// Разделитель между частями пути к мастеровому свойству.
        /// </summary>
        public string Separator { get; set; }

        /// <summary>
        /// Путь до мастерового свойства.
        /// </summary>
        public string Value
        {
            get
            {
                return _masterPropertyPath;
            }
        }

        /// <summary>
        /// Путь до мастерового свойства.
        /// </summary>
        private string _masterPropertyPath;

        /// <summary>
        /// Создать экземпляр обертки для работы с путем до мастерового свойства.
        /// </summary>
        /// <param name="value">
        /// Путь до мастерового поля, с которым будет идти в последующем работа.
        /// </param>
        public MasterPropertyPath(string value)
        {
            Separator = DefaultSeparator;

            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value", @"Необходимо указать непустой путь до мастерового свойства.");

            _masterPropertyPath = value;

            if (GetMasterLevel() < 1)
                throw new ArgumentException("Необходимо указать корректный путь до мастерового свойства.");
        }

        /// <summary>
        /// Получить путь до мастерового свойства из его частей.
        /// </summary>
        /// <param name="propertyPathParts">
        /// Части пути до мастерового свойства.
        /// <example>"MasterClass", "MasterClass2", "Attribute" = MasterClass.MasterClass2.Attribute</example>
        /// </param>
        /// <returns>Путь до мастерового свойства.</returns>
        public static string GetMasterPropertyPath(params string[] propertyPathParts)
        {
            return string.Join(DefaultSeparator, propertyPathParts);
        }

        /// <summary>
        /// Проверяет, является ли переданный путь мастеровым.
        /// При проверке используется разделитель по умолчанию.
        /// </summary>
        /// <param name="propertyPath">Путь, который необходимо проверить.</param>
        /// <returns>Является ли переданный путь мастеровым.</returns>
        public static bool IsMasterPath(string propertyPath)
        {
            return !string.IsNullOrEmpty(propertyPath) && propertyPath.Contains(DefaultSeparator);
        }

        /// <summary>
        /// Получить уровень мастера из пути до его свойства.
        /// </summary>
        /// <param name="masterName">
        /// Наименование мастера в пути, уровень которого необходимо получить.
        /// Если не указан, то метод возвращает максимальный уровень мастера.
        /// </param>
        /// <returns>
        /// Уровень мастера из пути до его свойства.
        /// Вернет -1, если указанное имя мастера не было найдено в пути.
        /// </returns>
        public int GetMasterLevel(string masterName = null)
        {
            string[] parts = GetMasterPropertyPathParts();

            if (masterName == null)
                return parts.Length - 1;

            // Просматриваем все части пути кроме последней. Так как последняя часть не является мастером.
            for (int i = parts.Length - 2; i >= 0; i--)
            {
                if (parts[i] == masterName)
                    return i + 1;
            }

            return -1;
        }

        /// <summary>
        /// Получить наименование свойства мастера из его пути.
        /// </summary>
        /// <returns>Наименование свойства мастера из его пути.</returns>
        public string GetMasterPropertyName()
        {
            string[] parts = GetMasterPropertyPathParts();
            return parts[parts.Length - 1];
        }

        /// <summary>
        /// Заменить определенную часть пути.
        /// </summary>
        /// <param name="partLevel">Номер части пути. Начинается с 1.</param>
        /// <param name="replacementString">Строка для замены части пути.</param>
        /// <returns>Путь до мастерового свойства с замененной частью.</returns>
        public string ReplacePart(int partLevel, string replacementString)
        {
            if (partLevel < 1)
                throw new ArgumentException("Части пути начинают считаться с 1.");

            string[] parts = GetMasterPropertyPathParts();
            string replacedString = parts[partLevel - 1];

            // Добавим разделители в начале и в конце строк, если это необходимо, чтобы попытаться однозначно определить часть пути.
            if (partLevel > 1)
            {
                replacedString = Separator + replacedString;

                // Если мы хотим заменить не на пустую строку, то добавим разделитель и в начале подменной строки, чтобы все разделители сохранились.
                if (!string.IsNullOrEmpty(replacementString))
                    replacementString = Separator + replacementString;
            }

            if (partLevel < parts.Length - 1)
            {
                replacedString += Separator;

                // Если мы хотим заменить на не пустую строку в середине или конце пути, то добавим разделитель и в конце подменной строки, чтобы все разделители сохранились.
                if (!string.IsNullOrEmpty(replacementString) || partLevel != 1)
                    replacementString += Separator;
            }

            _masterPropertyPath = _masterPropertyPath.Replace(replacedString, replacementString);

            return _masterPropertyPath;
        }

        /// <summary>
        /// Заменить разделитель частей пути до мастерового свойства.
        /// </summary>
        /// <param name="newSeparator">Новый разделитель для частей пути.</param>
        /// <returns>Путь до мастерового свойства с замененным разделителем.</returns>
        public string ChangeSeparator(string newSeparator)
        {
            if (string.IsNullOrEmpty(newSeparator))
                throw new ArgumentNullException("newSeparator", @"Разделитель не может быть пустым.");

            _masterPropertyPath = _masterPropertyPath.Replace(Separator, newSeparator);
            Separator = newSeparator;
            return _masterPropertyPath;
        }

        /// <summary>
        /// Получить мастера из пути до его свойства. Путь без самого свойства.
        /// </summary>
        /// <param name="level">
        /// Уровень мастера из пути, путь которого необходимо получить.
        /// По умолчанию берется мастер максимального уровня.
        /// Уровень считается с 1.
        /// </param>
        /// <returns>
        /// Мастер полученный из пути до его свойства.
        /// </returns>
        public string GetMaster(int level = -1)
        {
            List<string> parts = GetMasterPropertyPathParts().ToList();

            if (level > parts.Count - 1)
                throw new ArgumentException("Переданный уровень мастера не может превышать максимального уровня.");

            if (level < 1)
                parts.RemoveAt(parts.Count - 1);
            else
                parts.RemoveRange(level, parts.Count - level);

            return string.Join(Separator, parts.ToArray());
        }

        /// <summary>
        /// Получить наименование мастера из пути до его свойства. Часть пути до мастера.
        /// </summary>
        /// <param name="level">
        /// Уровень мастера из пути, наименование которого необходимо получить.
        /// По умолчанию берется мастер максимального уровня.
        /// Уровень считается с 1.
        /// </param>
        /// <returns>
        /// Наименование мастера полученное из пути до его свойства.
        /// </returns>
        public string GetMasterName(int level = -1)
        {
            List<string> parts = GetMasterPropertyPathParts().ToList();

            if (level > parts.Count - 1)
                throw new ArgumentException("Переданный уровень мастера не может превышать максимального уровня.");

            if (level < 1)
                level = parts.Count - 1;

            return parts[level - 1];
        }

        /// <summary>
        /// Получить части пути до мастерового свойства.
        /// </summary>
        /// <returns>Части пути до мастерового свойства.</returns>
        protected string[] GetMasterPropertyPathParts()
        {
            return _masterPropertyPath.Split(new[] { Separator }, StringSplitOptions.None);
        }
    }
}
