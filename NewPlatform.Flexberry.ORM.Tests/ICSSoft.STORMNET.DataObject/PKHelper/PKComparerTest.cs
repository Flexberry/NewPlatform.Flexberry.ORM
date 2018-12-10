namespace IIS.University.PKHelper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestObjects.DataObject;

    [TestClass]
    public class PKComparerTest
    {
        [TestMethod]
        public void EqualsСравнениеПустыхОбъектов()
        {
            Assert.IsTrue(new PKComparer<DataObjectForTest>().Equals(null, null), "Пустые объекты сравнены неверно.");
        }

        [TestMethod]
        public void EqualsСравнениеПустогоИНепустогоОбъектов()
        {
            var obj = new DataObjectForTest();
            Assert.IsFalse(new PKComparer<DataObjectForTest>().Equals(null, obj), "Пустой и непустой объекты сравнены неверно.");
        }

        [TestMethod]
        public void EqualsСравнениеРавныхОбъектов()
        {
            var obj = new DataObjectForTest();
            Assert.IsTrue(new PKComparer<DataObjectForTest>().Equals(obj, obj), "Равные объекты сравнены неверно.");
        }

        [TestMethod]
        public void GetHashCodeСравнениеХэшОбъекта()
        {
            var obj = new DataObjectForTest();
            Assert.AreEqual(
                PKHelper.GetKeyByObject(obj).ToString().ToLower().GetHashCode(),
                new PKComparer<DataObjectForTest>().GetHashCode(obj));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetHashCodeСравнениеХэшПустогоОбъекта()
        {
            new PKComparer<DataObjectForTest>().GetHashCode(null);
        }
        
        [TestMethod]
        public void Distinct01()
        {
            var list = new List<DataObjectForTest>
            {
                new DataObjectForTest()
            };
            Assert.IsTrue(list.Distinct(new PKComparer<DataObjectForTest>()).Count() == 1);
        }

        [TestMethod]
        public void Distinct02()
        {
            var list = new List<DataObjectForTest>
            {
                new DataObjectForTest(),
                new DataObjectForTest()
            };
            Assert.IsTrue(list.Distinct(new PKComparer<DataObjectForTest>()).Count() == 2);
        }
        
        [TestMethod]
        public void Distinct03()
        {
            var list = new List<DataObjectForTest>
            {
                new DataObjectForTest(),
                new DataObjectForTest(),
                null
            };
            Assert.IsTrue(list.Distinct(new PKComparer<DataObjectForTest>()).Count() == 3);
        }

        [TestMethod]
        public void Distinct04()
        {
            var obj = new DataObjectForTest();
            var list = new List<DataObjectForTest>
            {
                obj,
                obj
            };
            Assert.IsTrue(list.Distinct(new PKComparer<DataObjectForTest>()).Count() == 1);
        }

        [TestMethod]
        public void Distinct05()
        {
            var obj = new DataObjectForTest();
            var list = new List<DataObjectForTest>
            {
                obj,
                obj,
                null,
                null
            };
            Assert.IsTrue(list.Distinct(new PKComparer<DataObjectForTest>()).Count() == 2);
        }

        [TestMethod]
        public void Distinct06()
        {
            var obj = new DataObjectForTest();
            var list = new List<DataObjectForTest>
            {
                obj,
                obj,
                new DataObjectForTest(),
                null,
                null
            };
            Assert.IsTrue(list.Distinct(new PKComparer<DataObjectForTest>()).Count() == 3);
        }
    }
}
