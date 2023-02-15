namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET;

    using Xunit;

    public class PKComparerTest
    {
        [Fact]
        public void EqualsСравнениеПустыхОбъектов()
        {
            Assert.True(new PKComparer<DataObjectForTest>().Equals(null, null), "Пустые объекты сравнены неверно.");
        }

        [Fact]
        public void EqualsСравнениеПустогоИНепустогоОбъектов()
        {
            var obj = new DataObjectForTest();
            Assert.True(!new PKComparer<DataObjectForTest>().Equals(null, obj), "Пустой и непустой объекты сравнены неверно.");
        }

        [Fact]
        public void EqualsСравнениеРавныхОбъектов()
        {
            var obj = new DataObjectForTest();
            Assert.True(new PKComparer<DataObjectForTest>().Equals(obj, obj), "Равные объекты сравнены неверно.");
        }

        [Fact]
        public void GetHashCodeСравнениеХэшОбъекта()
        {
            var obj = new DataObjectForTest();
            Assert.Equal(
                PKHelper.GetKeyByObject(obj).ToString().ToLower().GetHashCode(),
                new PKComparer<DataObjectForTest>().GetHashCode(obj));
        }

        [Fact]
        public void GetHashCodeСравнениеХэшПустогоОбъекта()
        {
            Assert.Throws<NullReferenceException>(() => new PKComparer<DataObjectForTest>().GetHashCode(null));
        }

        [Fact]
        public void Distinct01()
        {
            var list = new List<DataObjectForTest>
            {
                new DataObjectForTest(),
            };
            Assert.True(list.Distinct(new PKComparer<DataObjectForTest>()).Count() == 1);
        }

        [Fact]
        public void Distinct02()
        {
            var list = new List<DataObjectForTest>
            {
                new DataObjectForTest(),
                new DataObjectForTest(),
            };
            Assert.True(list.Distinct(new PKComparer<DataObjectForTest>()).Count() == 2);
        }

        [Fact]
        public void Distinct03()
        {
            var list = new List<DataObjectForTest>
            {
                new DataObjectForTest(),
                new DataObjectForTest(),
                null,
            };
            Assert.True(list.Distinct(new PKComparer<DataObjectForTest>()).Count() == 3);
        }

        [Fact]
        public void Distinct04()
        {
            var obj = new DataObjectForTest();
            var list = new List<DataObjectForTest>
            {
                obj,
                obj,
            };
            Assert.True(list.Distinct(new PKComparer<DataObjectForTest>()).Count() == 1);
        }

        [Fact]
        public void Distinct05()
        {
            var obj = new DataObjectForTest();
            var list = new List<DataObjectForTest>
            {
                obj,
                obj,
                null,
                null,
            };
            Assert.True(list.Distinct(new PKComparer<DataObjectForTest>()).Count() == 2);
        }

        [Fact]
        public void Distinct06()
        {
            var obj = new DataObjectForTest();
            var list = new List<DataObjectForTest>
            {
                obj,
                obj,
                new DataObjectForTest(),
                null,
                null,
            };
            Assert.True(list.Distinct(new PKComparer<DataObjectForTest>()).Count() == 3);
        }
    }
}
