namespace ICSSoft.STORMNET.Business
{
    /// <summary>
    /// Делегат для изменения View для Типа.
    /// </summary>
    /// <param name="type">Тип, для которого можно поменять представление.</param>
    /// <returns>Представление, которое было изменено.</returns>
    public delegate View ChangeViewForTypeDelegate(System.Type type);
}