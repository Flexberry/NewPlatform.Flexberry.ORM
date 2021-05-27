namespace ICSSoft.STORMNET.Tests.TestClasses.Tools
{
    using System;
    using ICSSoft.STORMNET.Tools;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Различные тесты на получение заголовков и работу с ними.
    /// </summary>
    public class CaptionToolTests
    {
        /// <summary>
        /// Тестирование получения имени по заголовку.
        /// </summary>
        [Fact]

        public void TestGetNameByCaption()
        {
            Assert.Equal("Абвгдеёжзиклмнопрстуфхцчшщъыьэюя", CaptionTool.TransformCaptionToName("абвгдеёжзиклмнопрстуфхцчшщъыьэюя"));
            Assert.Equal("Abcdefghijklmnopqrstuvwxyz", CaptionTool.TransformCaptionToName("abcdefghijklmnopqrstuvwxyz"));
            Assert.Equal("МамаМылаРаму", CaptionTool.TransformCaptionToName("Мама мыла раму"));
            Assert.Equal("МаМаМыЛаРаму", CaptionTool.TransformCaptionToName("Ма;;ма _' мы#####ла раму"));
            Assert.Equal("ФеиФееричноФеяли121Фен", CaptionTool.TransformCaptionToName("123 Феи феерично феяли 121 фен."));
        }

        /// <summary>
        /// Тестирование получения имени по пустому заголовку.
        /// </summary>
        [Fact]
        public void TestGetNameByNullCaption()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                CaptionTool.TransformCaptionToName(null);
            });
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        /// <summary>
        /// Тестирование того, какой заголовок для поля возвращается из представления по имени этого поля.
        /// </summary>
        [Fact]

        public void TestGetAttrCaptionByView()
        {
            Assert.Equal("Ириска", CaptionTool.GetAttrCaptionByView(null, "Ириска"));
            Assert.Equal("Ириска", CaptionTool.GetAttrCaptionByView(Кредит.Views.C__КредитE, "Ириска"));
            Assert.Equal("Синяя гладь", CaptionTool.GetAttrCaptionByView(Кредит.Views.C__КредитE, "СиняяГладь"));
            Assert.Equal("Срок кредита", CaptionTool.GetAttrCaptionByView(Кредит.Views.C__КредитE, "СрокКредита"));
            Assert.Equal("СуммаДляCaptionКредита", CaptionTool.GetAttrCaptionByView(Кредит.Views.C__КредитE, "СуммаКредита"));
        }

        /// <summary>
        /// Тестирование отбрасывания невалидных символов из имени.
        /// </summary>
        [Fact]

        public void TestGetValidName()
        {
            Assert.Equal("Азбука", CaptionTool.GetValidName("А з б у к а"));
            Assert.Equal("Кошка", CaptionTool.GetValidName("1234567890 Кошка"));
            Assert.Equal("абвгдеёжзиклмнопрстуфхцчшщъыьэюя", CaptionTool.GetValidName("абвгдеёжзиклмнопрстуфхцчшщъыьэюя"));
            Assert.Equal("qwRt_цшщ", CaptionTool.GetValidName("qwRt_цшщ"));
            Assert.Equal("С__л_о__в_о", CaptionTool.GetValidName("С!_\"№ %_л;_о: ?_*_(){}[]!@#$%^&*()-=+ в _ о"));
            Assert.Equal("你好吗", CaptionTool.GetValidName("你 好 吗"));
        }

        /// <summary>
        /// Тестирование метода TransformTitle.
        /// </summary>
        [Fact]

        public void CaptionToolTransformTitleTest()
        {
            Assert.Equal(CaptionTool.TransformTitle("Property.Name", false, true), "Name");
            Assert.Equal(CaptionTool.TransformTitle("Property.NAme", true, true), "N ame");
            Assert.Equal(CaptionTool.TransformTitle("Property.NewNAme", true, true), "New n ame");
            Assert.Equal(CaptionTool.TransformTitle("Property.NAME", true, false), "NAME property");
        }

        /// <summary>
        /// Генерация exception в метода GetValidName.
        /// </summary>
        [Fact]
        public void CaptionToolGetValidNameTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                CaptionTool.GetValidName(String.Empty);
            });
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        //// TODO: отсутствует проверка на пробелы.
        ///// <summary>
        ///// Генерация exception в метода GetValidName.
        ///// </summary>
        // [Fact]
        //
        // [ExpectedException(typeof(ArgumentNullException))]
        // public void CaptionToolGetValidNameTest()
        // {
        //    CaptionTool.GetValidName(" ");
        // }
    }
}
