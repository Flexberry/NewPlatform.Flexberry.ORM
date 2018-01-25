namespace ICSSoft.STORMNET.Business.Interfaces
{
    using ICSSoft.STORMNET.Business;
    /// <summary>
    /// Интерфейс, к которому привязан бизнес-сервер, выполняющий каскадное удаление объектов.
    /// </summary>
    [BusinessServer(typeof(InterfaceBusinessServer), DataServiceObjectEvents.OnDeleteFromStorage)]
    public interface IReferencesCascadeDelete
    {
    }
}
