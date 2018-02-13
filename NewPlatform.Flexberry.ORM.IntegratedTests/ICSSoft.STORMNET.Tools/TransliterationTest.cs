namespace ICSSoft.STORMNET.Tests.TestClasses.Tools
{
    using ICSSoft.STORMNET.Tools;

    using Xunit;

    /// <summary>
    /// Тесты по транслитерации с русского на английский и наоборот.
    /// </summary>
    
    public class TransliterationTest
    {
        /// <summary>
        /// Тест транслитерации с русского на английский.
        /// </summary>
        [Fact]
        
        public void TestTransliterateFromRussianToEnglish()
        {
            string rusLetters = "абвгдеёжзиклмнопрстуфхцчшщъыьэюя";

            // ISO
            Assert.Equal("abvgdeyozhziklmnoprstufxcchshshh'yeyuya", Transliteration.Front(rusLetters));
            Assert.Equal("abcdefgijklmnoprstuvxyz", Transliteration.Front("абцдефгийклмнопрстувхыз"));
            Assert.Equal(Transliteration.Front(rusLetters), Transliteration.Front(rusLetters.ToUpper()).ToLower());

            // GOST
            Assert.Equal("abvgdejozhziklmnoprstufkhcchshshh'yehyuya", Transliteration.Front(rusLetters, Transliteration.TransliterationType.Gost));
            Assert.Equal("abcdefgiklmnoprstuvyz", Transliteration.Front("абцдефгиклмнопрстувыз", Transliteration.TransliterationType.Gost));
            Assert.Equal(
                Transliteration.Front(rusLetters, Transliteration.TransliterationType.Gost),
                Transliteration.Front(rusLetters.ToUpper(), Transliteration.TransliterationType.Gost).ToLower());

        }

        /// <summary>
        /// Тест транслитерации с английского на русский.
        /// </summary>
        [Fact]
        
        public void TestTransliterateFromEnglishToRussian()
        {
            string engLetters = "abcdefghijklmnopqrstuvwxyz";

            // ISO
            Assert.Equal("абцдефгийклмнопрстувхыз", Transliteration.Back(engLetters));
            Assert.Equal("абвгдеёжзиклмнопрстуфхцчшщъыеюя", Transliteration.Back("abvgdeyozhziklmnoprstufxcchshshh'yeyuya"));
            Assert.Equal(Transliteration.Back(engLetters), Transliteration.Back(engLetters.ToUpper()).ToLower());

            // GOST
            Assert.Equal("абцдефгиклмнопрстувыз", Transliteration.Back(engLetters, Transliteration.TransliterationType.Gost));
            Assert.Equal("абвгдеёжзиклмнопрстуфхцчшщЪыеыуыа", Transliteration.Back("abvgdejozhziklmnoprstufkhcchshshh'yehyuya", Transliteration.TransliterationType.Gost));
            Assert.Equal(
                Transliteration.Back(engLetters, Transliteration.TransliterationType.Gost).ToLower(),
                Transliteration.Back(engLetters.ToUpper(), Transliteration.TransliterationType.Gost).ToLower());
        }
    }
}
