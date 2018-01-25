namespace ICSSoft.STORMNET.Business.Interfaces
{
    /// <summary>
    /// Интерфейс, к которому привязан бизнес-сервер, выполняющий зануление ссылок на удаляемый объект.
    /// </summary>
    [BusinessServer(typeof(InterfaceBusinessServer), DataServiceObjectEvents.OnDeleteFromStorage)]
    public interface IReferencesNullDelete
    {
    }
}
