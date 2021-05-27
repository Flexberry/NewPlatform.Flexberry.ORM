namespace ICSSoft.STORMNET.Business.OracleDataServiceTests
{
    using System.Data;

    using ICSSoft.STORMNET.Business;

    using Xunit;

    /// <summary>
    /// Класс для проверки работы <see cref="OracleDataService"/>.
    /// </summary>
    public class OracleDataServiceTest
    {
        /// <summary>
        /// Метод для создания OracleDataService для целей тестирования.
        /// </summary>
        /// <returns>Сконструированный OracleDataService.</returns>
        public static OracleDataService CreateOracleDataServiceForTests()
        {
            var ds = new OracleDataService();
            ds.CustomizationString = "Data Source=dbserver-oracle;User ID=ora_tester;Password=pwd;";
            return ds;
        }

        /// <summary>
        /// Проверка метода получения коннекции <see cref="OracleDataService.GetConnection"/>.
        /// </summary>
        [Fact]
        public void GetOraConnectionTest()
        {
            OracleDataService ds = CreateOracleDataServiceForTests();
            IDbConnection cnn = ds.GetConnection();
            Assert.NotNull(cnn);
        }
    }
}
