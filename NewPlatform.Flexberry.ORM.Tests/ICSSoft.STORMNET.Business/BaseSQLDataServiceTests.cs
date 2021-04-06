namespace NewPlatform.Flexberry.ORM.Tests
{
    using System.Collections.Generic;

    using ICSSoft.STORMNET.Business;

    public abstract class BaseSQLDataServiceTests
    {
        /// <summary>
        /// The data services for temp databases (for <see cref="DataServices"/>).
        /// </summary>
        private readonly List<SQLDataService> _dataServices = new List<SQLDataService>();

        /// <summary>
        /// Data services for temp databases.
        /// </summary>
        protected IEnumerable<SQLDataService> DataServices => _dataServices;

        protected BaseSQLDataServiceTests()
        {
            _dataServices.Add(new MSSQLDataService());
            _dataServices.Add(new PostgresDataService());
            _dataServices.Add(new OracleDataService());
        }
    }
}
