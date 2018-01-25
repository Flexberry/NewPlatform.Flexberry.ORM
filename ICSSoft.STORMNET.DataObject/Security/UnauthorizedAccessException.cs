namespace ICSSoft.STORMNET
{
    using System;

    using ICSSoft.STORMNET.Collections;

    /// <summary>
    /// Исключение неавторизованного доступа
    /// </summary>
    public sealed class UnauthorizedAccessException : Exception
    {
        /// <summary>
        /// конвертер имён ошибок
        /// </summary>
        public static IAccessErrorNameConverter ErrorNameConverter { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public Type Tp { get; set; }

        /// <summary>
        /// Имя операции
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// конструктор с параметрами
        /// </summary>
        /// <param name="sOperationName"></param>
        /// <param name="tp"></param>
        public UnauthorizedAccessException(string sOperationName, Type tp)
        {
            OperationName = sOperationName;
            Tp = tp;
        }

        /// <summary>
        /// Сообщение
        /// </summary>
        public override string Message
        {
            get
            {
                NameObjectCollection coll = new NameObjectCollection
                {
                    { "Start", "запуска приложения" },
                    { "Open", "открытия формы" },
                    { "Read", "чтения" },
                    { "Update", "обновления" },
                    { "Delete", "удаления" },
                    { "Insert", "вставки" },
                    { "Print", "печати" }
                };

                string sop = OperationName;
                if (ErrorNameConverter != null)
                    sop = ErrorNameConverter.Convert(sop);

                if (coll.ContainsKey(sop))
                    sop = (string)coll[sop];

                return "Недостаточно полномочий для выполнения операции " + sop + (sop == "открытия формы" ? " " : " над объектом ") + Information.GetClassCaption(Tp);
            }
        }
    }
}