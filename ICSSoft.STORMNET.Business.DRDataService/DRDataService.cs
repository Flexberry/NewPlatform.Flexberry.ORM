namespace ICSSoft.STORMNET.Business
{
    /// <summary>
    /// Сервис данных для грязного чтения (MSSQLServer).
    /// </summary>
    public class DRDataService : ICSSoft.STORMNET.Business.MSSQLDataService
    {
        public override string GetJoinTableModifierExpression()
        {
            return " WITH (NOLOCK) ";
        }
    }
}
