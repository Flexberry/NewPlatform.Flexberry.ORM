namespace ICSSoft.STORMNET.Tests.TestClasses.ExternalLDef
{
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Тесты для ExternalLangDef.
    /// </summary>
    public class ExternalLangDefTest
    {
        /// <summary>
        /// Экземпляр ExternalLangDef для тестов.
        /// </summary>
        private static readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Тест свойства <see cref="ExternalLangDef.paramYearDIFF"/>.
        /// </summary>
        [Fact]
        public void GetparamYearDiffTest()
        {
            Assert.Equal(ldef.paramYearDIFF,  "YearDIFF");
        }

        /// <summary>
        /// Тест свойства <see cref="ExternalLangDef.paramMonthDIFF"/>.
        /// </summary>
        [Fact]
        public void GetparamMonthDiffTest()
        {
            Assert.Equal(ldef.paramMonthDIFF, "MonthDIFF");
        }

        /// <summary>
        /// Тест свойства <see cref="ExternalLangDef.paramWeekDIFF"/>.
        /// </summary>
        [Fact]
        public void GetparamWeekDiffTest()
        {
            Assert.Equal(ldef.paramWeekDIFF, "WeekDIFF");
        }

        /// <summary>
        /// Тест свойства <see cref="ExternalLangDef.paramQuarterDIFF"/>.
        /// </summary>
        [Fact]
        public void GetparamquarterDiffTest()
        {
            Assert.Equal(ldef.paramQuarterDIFF, "quarterDIFF");
        }

        /// <summary>
        /// Тест свойства <see cref="ExternalLangDef.paramDayDIFF"/>.
        /// </summary>
        [Fact]
        public void GetparamDayDiffTest()
        {
            Assert.Equal(ldef.paramDayDIFF, "DayDIFF");
        }

        /// <summary>
        /// Тест свойства <see cref="ExternalLangDef.funcMonthPart"/>.
        /// </summary>
        [Fact]
        public void GetfuncMonthPartTest()
        {
            Assert.Equal(ldef.funcMonthPart, "MonthPart");
        }

        /// <summary>
        /// Тест свойства <see cref="ExternalLangDef.funcHHPart"/>.
        /// </summary>
        [Fact]
        public void GetfuncHhPartTest()
        {
            Assert.Equal(ldef.funcHHPart, "hhPart");
        }

        /// <summary>
        /// Тест свойства <see cref="ExternalLangDef.funcMIPart"/>.
        /// </summary>
        [Fact]
        public void GetfuncMiPartTest()
        {
            Assert.Equal(ldef.funcMIPart, "miPart");
        }

        /// <summary>
        /// Тест свойства <see cref="ExternalLangDef.funcDATEDIFF"/>.
        /// </summary>
        [Fact]
        public void GetfuncDatediffTest()
        {
            Assert.Equal(ldef.funcDATEDIFF, "DATEDIFF");
        }

        /// <summary>
        /// Тест свойства <see cref="ExternalLangDef.funcDayOfWeek"/>.
        /// </summary>
        [Fact]
        public void GetfuncDayOfWeekTest()
        {
            Assert.Equal(ldef.funcDayOfWeek, "DayOfWeek");
        }

        /// <summary>
        ///  Тест перегруженного метода <see cref="ExternalLangDef.GetObjectTypeForNetType"/>
        /// </summary>
        [Fact]
        public void GetObjectTypeForNetTypeTest()
        {
            // Вариент, когда тип - массив детейлов.
            Assert.Equal(ldef.GetObjectTypeForNetType(typeof(DetailArrayOfDetailClass)).Caption, "Зависимые объекты");

            // Вариант, когда тип - мастеровой тип.
            Assert.Equal(ldef.GetObjectTypeForNetType(typeof(MasterClass)).Caption, "Сущность");
        }

        #region этот тест лучше отнести к тестам для FunctionalLanguageDef
        /// <summary>
        /// Тест выпадения NotFoundFunctionParametersException в методе <see cref="ExternalLangDef.GetExistingVariableNames"/>,
        /// в том случае, когда при формировании функции берётся неподходящий параметр.
        /// </summary>
        [Fact]
        public void GetExistingVariableNamesNotFoundFunctionParametersExceptionTest()
        {
            var exception = Record.Exception(() =>
            {
                var obj = new MasterClass() { StringMasterProperty = "prop", IntMasterProperty = 666 };
                SQLWhereLanguageDef langdef = SQLWhereLanguageDef.LanguageDef;
                Function lf2 = langdef.GetFunction(
                    langdef.funcNOT,
                    langdef.GetFunction(langdef.funcEQ, new VariableDef(langdef.GuidType, "MasterClass"), obj.DetailClass));
            });
            Assert.IsType(typeof(FunctionalLanguageDef.NotFoundFunctionParametersException), exception);
        }
        #endregion
    }
}
