namespace ICSSoft.STORMNET.Tools
{
    using System;

    /// <summary>
    /// Вспомогательный класс для обработки и получения Caption'ов и других имён.
    /// </summary>
    public class CaptionTool
    {
        /// <summary>
        /// Формирование из текста читаемого заголовка(caption)
        /// (ранее данный метод находится в STORM.NET Case Tool plugin).
        /// </summary>
        /// <param name="propertyName">Исходный текст.</param>
        /// <param name="splitWords">Разделять ли слова.</param>
        /// <param name="hideMasters">Скрывать ли имена мастеров.</param>
        public static string TransformTitle(string propertyName, bool splitWords, bool hideMasters)
        {
            string captionText = string.Empty;

            string[] arr = propertyName.Split('.');

            for (var i = arr.Length - 1; i >= 0 && (!hideMasters || i == arr.Length - 1); i--)
            {
                if (!splitWords)
                {
                    captionText += arr[i];
                }
                else
                {
                    var wordTitle = string.Empty;
                    var prevUpper = false;
                    var allYetUpper = true;
                    for (var j = arr[i].Length - 1; j >= 0; j--)
                    {
                        var c = arr[i][j];
                        var upper = char.IsUpper(c);
                        allYetUpper = allYetUpper & upper;

                        if (upper)
                        {
                            if (allYetUpper)
                            {
                                wordTitle = c.ToString() + wordTitle;
                            }
                            else if (prevUpper)
                            {
                                wordTitle = char.ToLower(c).ToString() + " " + wordTitle;
                            }
                            else
                            {
                                wordTitle = char.ToLower(c).ToString() + wordTitle;
                            }
                        }
                        else if (prevUpper)
                        {
                            wordTitle = c.ToString() + " " + wordTitle;
                        }
                        else
                        {
                            wordTitle = c.ToString() + wordTitle;
                        }

                        prevUpper = upper;
                    }

                    if (wordTitle.Length > 0)
                    {
                        wordTitle = char.ToUpper(wordTitle[0]) + wordTitle.Remove(0, 1);
                    }

                    if (!string.IsNullOrEmpty(captionText) && wordTitle.Length > 1
                        && char.IsUpper(wordTitle[0]) && char.IsLower(wordTitle[1]))
                    {
                        captionText += char.ToLower(wordTitle[0]) + wordTitle.Substring(1);
                    }
                    else
                    {
                        captionText += wordTitle;
                    }
                }

                if (i != 0)
                {
                    captionText += " ";
                }
            }

            return captionText.Trim();
        }

        /// <summary>
        /// Преобразовать заголовок в имя (имя записывается почти в camel-нотации).
        /// В имени остаются только буквы и цифры (цифра не может быть в первой позиции).
        /// </summary>
        /// <param name="captionText"> Текст, который нужно преобразовать. </param>
        /// <returns>Имя, преобразованное из заголовка.</returns>
        public static string TransformCaptionToName(string captionText)
        {
            if (captionText == null)
            {
                throw new ArgumentNullException("captionText");
            }

            string nameText = string.Empty;
            bool symbolToUpper = true;
            foreach (char currentSymbol in captionText)
            {
                if (char.IsLetter(currentSymbol) || (nameText != string.Empty && char.IsDigit(currentSymbol)))
                {
                    nameText += symbolToUpper ? char.ToUpper(currentSymbol) : currentSymbol;
                    symbolToUpper = false;
                }
                else
                {
                    symbolToUpper = true;
                }
            }

            return nameText;
        }

        /// <summary>
        /// Класс для получения Caption'a поля по его имени
        /// (если представление null или поле с таким именем не найдено, то отобразится просто
        /// имя поля, разделённое из camel-нотации).
        /// </summary>
        /// <param name="currentView">
        /// Представление, по которому будет получаться Caption.
        /// </param>
        /// <param name="fieldName">
        /// Имя поля.
        /// </param>
        /// <returns>
        /// Результат.
        /// </returns>
        public static string GetAttrCaptionByView(View currentView, string fieldName)
        {
            var resultCaption = string.Empty;
            if (currentView != null && currentView.CheckPropname(fieldName))
            {
                resultCaption = currentView.GetProperty(fieldName).Caption;
            }

            return (resultCaption != null) && (resultCaption.Trim() != string.Empty)
                       ? resultCaption
                       : TransformTitle(fieldName, true, false);
        }

        /// <summary>
        /// Получение корректного имени из набора символов (лишние символы просто отбрасываются).
        /// Корректными считаются буквы, цифры (не в первой позиции) и нижнее подчёркивание.
        /// </summary>
        /// <param name="objectName"> Исходный набор символов, которые необходимо преобразовать. </param>
        /// <returns> Строка с исключёнными недопустимыми символами. </returns>
        public static string GetValidName(string objectName)
        {
            if (string.IsNullOrEmpty(objectName))
            {
                throw new ArgumentNullException("objectName");
            }

            var result = string.Empty;

            foreach (var curSymbol in objectName)
            {
                if (char.IsLetter(curSymbol) || curSymbol == '_' || (result != string.Empty && char.IsDigit(curSymbol)))
                {
                    result += curSymbol;
                }
            }

            return result;
        }
    }
}
