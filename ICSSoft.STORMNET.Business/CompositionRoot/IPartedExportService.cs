namespace NewPlatform.Flexberry
{
    /// <summary>
    /// Сервис экспорта данных из ORM, поддерживающий экспорт по частям.
    /// Применяется в случае, если количество экспортируемых данных столь велико,
    /// что одновременное их удержание в памяти пагубно сказывается на производительности.
    /// </summary>
    public interface IPartedExportService
    {
        /// <summary>
        /// Получение пустого параметра экспорта необходимого типа.
        /// </summary>
        /// <returns>Пустой параметр экспорта необходимого типа.</returns>
        BasePartedExportParam GetEmptyBasePartedExportParam();

        /// <summary>
        /// Инициализация файла экспорта данных из ORM.
        /// </summary>
        /// <param name="exportParameter">Параметры, необходимые для полноценного экспорта.</param>
        /// <returns>Сведения о файле экспорта данных из ORM, позволяющие выполнять добавление следующей порции данных для экспорта.</returns>
        BasePartedExportParam InitExportStream(BasePartedExportParam exportParameter);

        /// <summary>
        /// Добавление в файл экспорта данных из ORM новой порции данных.
        /// </summary>
        /// <param name="exportParameter">Параметры, необходимые для полноценного экспорта.</param>
        /// <returns>Сведения об файле экспорта данных из ORM.</returns>
        BasePartedExportParam AddPartedDataToExportStream(BasePartedExportParam exportParameter);
    }
}
