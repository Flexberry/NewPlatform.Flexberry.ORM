namespace ICSSoft.STORMNET.Business.Audit.Exceptions
{
    /// <summary>
    /// Исключение, сообщающее, что переданные на дозапись в аудит данные некорректны.
    /// </summary>
    public sealed class WrongAdditionalInfoAuditException : AuditException
    {
        private const string WrongAdditionalInfoErrorMessage = "Попытка дописать значения аудита для поля, который не имеет атрибута DisableInsertPropertyAttribute.";

        /// <summary>
        /// Конструктор для инициализации исключения с сообщением
        /// "Попытка дописать значения аудита для поля, который не имеет атрибута DisableInsertPropertyAttribute".
        /// </summary>
        public WrongAdditionalInfoAuditException()
            : base(WrongAdditionalInfoErrorMessage)
        {
        }
    }
}
