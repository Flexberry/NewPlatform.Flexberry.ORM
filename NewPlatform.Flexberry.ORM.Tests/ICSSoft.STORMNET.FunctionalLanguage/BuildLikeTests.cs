namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildLikeTests : BaseFunctionTest
    {
        #region common

        [Fact]
        public void BuildLikeTest1()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLike(NullString, NullString));
        }

        [Fact]
        public void BuildLikeTest2()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLike(PropertyName, NullString));
        }

        [Fact]
        public void BuildLikeTest3()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLike, StringVarDef, String1),
                FunctionBuilder.BuildLike(PropertyName, String1));
        }

        [Fact]
        public void BuildStartsWith1()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildStartsWith(NullString, NullString));
        }

        [Fact]
        public void BuildStartsWith2()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildStartsWith(PropertyName, NullString));
        }

        [Fact]
        public void BuildStartsWith3()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLike, StringVarDef, $"{String1}%"),
                FunctionBuilder.BuildStartsWith(PropertyName, String1));
        }

        [Fact]
        public void BuildEndsWith1()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEndsWith(NullString, NullString));
        }

        [Fact]
        public void BuildEndsWith2()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEndsWith(PropertyName, NullString));
        }

        [Fact]
        public void BuildEndsWith3()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLike, StringVarDef, $"%{String1}"),
                FunctionBuilder.BuildEndsWith(PropertyName, String1));
        }

        [Fact]
        public void BuildContains1()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildContains(NullString, NullString));
        }

        [Fact]
        public void BuildContains2()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildContains(PropertyName, NullString));
        }

        [Fact]
        public void BuildContains3()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLike, StringVarDef, $"%{String1}%"),
                FunctionBuilder.BuildContains(PropertyName, String1));
        }

        #endregion

        #region generic

        [Fact]
        public void BuildGenLikeTest1()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLike<TestDataObject>(x => x.Name, NullString));
        }

        [Fact]
        public void BuildGenLikeTest2()
        {
            FunctionBuilder.BuildLike<TestDataObject>(x => x.BirthDate, String1);
        }

        [Fact]
        public void BuildGenLikeTest3()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLike, StringGenVarDef, $"{String1}"),
                FunctionBuilder.BuildLike<TestDataObject>(x => x.Name, String1));
        }

        [Fact]
        public void BuildGenStartsWithTest1()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildStartsWith<TestDataObject>(x => x.Name, NullString));
        }

        [Fact]
        public void BuildGenStartsWithTest2()
        {
            FunctionBuilder.BuildStartsWith<TestDataObject>(x => x.BirthDate, String1);
        }

        [Fact]
        public void BuildGenStartsWithTest3()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLike, StringGenVarDef, $"{String1}%"),
                FunctionBuilder.BuildStartsWith<TestDataObject>(x => x.Name, String1));
        }

        [Fact]
        public void BuildGenEndsWithTest1()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEndsWith<TestDataObject>(x => x.Name, NullString));
        }

        [Fact]
        public void BuildGenEndsWithTest2()
        {
            FunctionBuilder.BuildEndsWith<TestDataObject>(x => x.BirthDate, String1);
        }

        [Fact]
        public void BuildGenEndsWithTest3()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLike, StringGenVarDef, $"%{String1}"),
                FunctionBuilder.BuildEndsWith<TestDataObject>(x => x.Name, String1));
        }

        [Fact]
        public void BuildGenContainsTest1()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildContains<TestDataObject>(x => x.Name, NullString));
        }

        [Fact]
        public void BuildGenContainsTest2()
        {
            FunctionBuilder.BuildContains<TestDataObject>(x => x.BirthDate, String1);
        }

        [Fact]
        public void BuildGenContainsTest3()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLike, StringGenVarDef, $"%{String1}%"),
                FunctionBuilder.BuildContains<TestDataObject>(x => x.Name, String1));
        }

        #endregion
    }
}
