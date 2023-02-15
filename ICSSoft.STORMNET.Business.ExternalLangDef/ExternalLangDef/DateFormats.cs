namespace ICSSoft.STORMNET.Windows.Forms
{
    using System;

    public partial class ExternalLangDef
    {
        /// <summary>
        /// Класс, хранящий коды форматов строкового представления даты.
        /// Все коды перечислены здесь: http://msdn.microsoft.com/ru-ru/library/ms187928.aspx.
        /// </summary>
        public static class DateFormats
        {
            /// <summary>
            /// Дата в формате DD.MM.YY.
            /// </summary>
            public static int German
            {
                get
                {
                    return 4;
                }
            }

            /// <summary>
            /// Дата в формате DD.MM.YYYY.
            /// </summary>
            public static int GermanWithCentury
            {
                get
                {
                    return 104;
                }
            }

            /// <summary>
            /// Дата в формате DD Mon YY.
            /// </summary>
            public static int Month
            {
                get
                {
                    return 6;
                }
            }

            /// <summary>
            /// Дата в формате DD Mon YYYY.
            /// </summary>
            public static int MonthWithCentury
            {
                get
                {
                    return 106;
                }
            }

            /// <summary>
            /// Время в формате HH.MM.SS.
            /// </summary>
            public static int Time
            {
                get
                {
                    return 8;
                }
            }

            /// <summary>
            /// Возвращает маску даты для Postgres, соответстующую коду.
            /// </summary>
            /// <param name="format">Код формата MSSQL.</param>
            /// <returns>Маска Postgres.</returns>
            public static string GetPostgresDateFormat(int format)
            {
                switch (format)
                {
                    case 4:
                        return "DD.MM.YY";
                    case 104:
                        return "DD.MM.YYYY";
                    case 6:
                        return "DD MON YY";
                    case 106:
                        return "DD MON YYYY";
                    case 8:
                        return "HH24:MI:SS";
                    default:
                        throw new ArgumentException("Неизвестный код формата даты");
                }
            }

            /// <summary>
            /// Возвращает маску даты для Oracle, соответстующую коду.
            /// </summary>
            /// <param name="format">Код формата MSSQL.</param>
            /// <returns>Маска Oracle.</returns>
            public static string GetOracleDateFormat(int format)
            {
                switch (format)
                {
                    case 4:
                        return "'DD.MM.YY'";
                    case 104:
                        return "'DD.MM.YYYY'";
                    case 6:
                        return "'DD MON YY'";
                    case 106:
                        return "'DD MON YYYY'";
                    case 8:
                        return "'HH24:MI:SS'";
                    default:
                        throw new ArgumentException("Неизвестный код формата даты");
                }
            }
        }
    }
}
