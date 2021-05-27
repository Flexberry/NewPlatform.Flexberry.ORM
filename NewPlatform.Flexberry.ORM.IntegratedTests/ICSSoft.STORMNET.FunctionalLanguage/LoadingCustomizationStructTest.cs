namespace ICSSoft.STORMNET.Tests.TestClasses.FunctionalLanguage
{
#if !NETCOREAPP
    using System;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Tools;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;
#endif

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LoadingCustomizationStructTest
    {
        private STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef _ldef =
            STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;

#if !NETCOREAPP
        [Fact]
        public void LcsSerializationTest()
        {
            // TODO: починить тест
            var lcs = new LoadingCustomizationStruct(1)
            {
                ColumnsSort = new[] { new ColumnsSortDef("Колонка1", SortOrder.Desc) },
                LimitFunction =
                    _ldef.GetFunction(_ldef.funcEQ, new VariableDef(_ldef.GuidType, "STORMMainObjectKey"), Guid.NewGuid().ToString()),
                LoadingTypes = new[] { typeof(DataObjectForTest) },
                View = new View(typeof(DataObjectForTest), View.ReadType.OnlyThatObject),
                ColumnsOrder = new[] { "Фамилия", "Name" },
                AdvansedColumns =
                    new[] { new AdvansedColumn("Колонка", "myExpression", "myStorageSourceModification") },
                InitDataCopy = true,
                ReturnTop = 100,
                LoadingBufferSize = 1000,
                RowNumber = new RowNumberDef(9, 89),
                Distinct = true,
                ReturnType = LcsReturnType.Object,
            };

            var lcsStr = ToolBinarySerializer.ObjectToString(lcs);
            var newLcs = (LoadingCustomizationStruct)ToolBinarySerializer.ObjectFromString(lcsStr);
            Assert.True(newLcs.Equals(lcs));
        }
#endif
    }
}
