namespace ICSSoft.STORMNET.Business.Interfaces
{
    using ICSSoft.STORMNET.Business;
    /// <summary>
    /// Интерфейс, к которому привязан бизнес-сервер, выполняющий зануление ссылок на удаляемый объект.
    /// </summary>
    [BusinessServer(typeof(InterfaceBusinessServer), DataServiceObjectEvents.OnDeleteFromStorage)]
    public interface IReferencesNullDelete
    {
    }
}
