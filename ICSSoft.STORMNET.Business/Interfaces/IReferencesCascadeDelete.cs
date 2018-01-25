namespace ICSSoft.STORMNET.Business.Interfaces
{
    /// <summary>
    /// Интерфейс, к которому привязан бизнес-сервер, выполняющий каскадное удаление объектов.
    /// </summary>
    [BusinessServer(typeof(InterfaceBusinessServer), DataServiceObjectEvents.OnDeleteFromStorage)]
    public interface IReferencesCascadeDelete
    {
    }
}
