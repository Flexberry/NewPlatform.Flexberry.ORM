namespace ICSSoft.STORMNET.Business
{
    /// <summary>
    /// Делегат для изменения строки соединения (организация работы с несколькими базами).
    /// </summary>
    /// <param name="types">Массив типов, который получается из объектов пришедших в сервис данных.</param>
    /// <returns>Новая строка соединения, если вернётся пустое значение или null, строка не изменится.</returns>
    public delegate string ChangeCustomizationStringDelegate(System.Type[] types);
}
