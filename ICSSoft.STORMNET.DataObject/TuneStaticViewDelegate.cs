namespace ICSSoft.STORMNET
{
    using System;

    /// <summary>
    /// Делегат для изменения донастройки статических представлений.
    /// </summary>
    /// <param name="viewName">Имя представления.</param>
    /// <param name="type">Тип для которого, запрашивается это представление.</param>
    /// <param name="view">Представление, которое можно настроить.</param>
    /// <returns>Настроенное представление.</returns>
    public delegate View TuneStaticViewDelegate(string viewName, Type type, View view);
}
