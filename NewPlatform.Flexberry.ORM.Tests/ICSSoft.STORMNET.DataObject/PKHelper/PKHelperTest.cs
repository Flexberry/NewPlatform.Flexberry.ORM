namespace IIS.University.PKHelper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.KeyGen;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestObjects.DataObject;

    [TestClass]
    public class PKHelperTests
    {
        private static readonly KeyGuid kg1 = KeyGuid.NewGuid();

        private static readonly KeyGuid kg2 = KeyGuid.NewGuid();

        private readonly Guid? ng1 = Guid.NewGuid();

        private readonly Guid g1 = Guid.NewGuid();

        private readonly Guid g2 = Guid.NewGuid();

        private readonly string str1 = KeyGuid.NewGuid().ToString();

        private readonly string str2 = KeyGuid.NewGuid().ToString();

        private readonly DataObjectForTest doft0 = new DataObjectForTest();

        private readonly DataObjectForTest doft1 = PKHelper.CreateDataObject<DataObjectForTest>(kg1);

        private readonly DataObjectForTest doft2 = PKHelper.CreateDataObject<DataObjectForTest>(kg2);

        #region EQPK

        [TestMethod]
        public void EQPKСравнениеПустыхОбъектов()
        {
            Assert.IsFalse(PKHelper.EQPK(null, null), "Пустые объекты сравнены неверно.");
        }

        [TestMethod]
        public void EQPKСравнениеПустогоОбъектаИБезКлюча()
        {
            Assert.IsFalse(PKHelper.EQPK(null, new object()), "Пустой объект и объект без ключа сравнены неверно.");
        }

        [TestMethod]
        public void EQPKСравнениеОбъектовБезКлючей()
        {
            Assert.IsFalse(PKHelper.EQPK(new object(), new object()), "Объекты без ключей сравнены неверно.");
        }

        [TestMethod]
        public void EQPKСравнениеПустогоОбъектаИСКлючом()
        {
            Assert.IsFalse(PKHelper.EQPK(null, doft0), "Пустой объект и объект с ключом сравнены неверно.");
        }

        [TestMethod]
        public void EQPKСравнениеОбъектовСКлючомИБезКлючом()
        {
            Assert.IsFalse(PKHelper.EQPK(new object(), doft0), "Объект с ключом и объект без ключа сравнены неверно.");
        }

        [TestMethod]
        public void EQPKСравнениеОбъектовСОдинаковымиGuid()
        {
            var obj2 = PKHelper.CreateDataObject<DataObjectForTest>(doft0);
            Assert.IsTrue(PKHelper.EQPK(doft0, obj2), "Объекты с одинаковыми Guid сравнены неверно.");
        }

        [TestMethod]
        public void EQPKСравнениеОбъектовСРазнымиGuid()
        {
            Assert.IsFalse(
                PKHelper.EQPK(doft1, doft2),
                "Объекты с разными Guid сравнены неверно.");
        }

        #endregion EQPK

        #region GetKeyByObject

        [TestMethod]
        public void GetKeyByObjectСравнениеKeyGuid()
        {
            Assert.IsTrue(
                PKHelper.EQPK(kg1, PKHelper.GetKeyByObject(kg1)),
                "Возвращенный KeyGuid не равен исходному.");
        }

        [TestMethod]
        public void GetKeyByObjectСравнениеGuid()
        {
            Assert.IsTrue(
                PKHelper.EQPK(g1, PKHelper.GetKeyByObject(g1)),
                "Возвращенный KeyGuid не равен исходному Guid.");
        }

        [TestMethod]
        public void GetKeyByObjectСравнениеString()
        {
            Assert.IsTrue(
                PKHelper.EQPK(str1, PKHelper.GetKeyByObject(str1)),
                "Возвращенный KeyGuid не равен исходному string.");
        }

        #endregion GetKeyByObject

        #region GetGuidByObject

        [TestMethod]
        public void GetGuidByObjectNull()
        {
            Assert.AreEqual(null, PKHelper.GetGuidByObject(null));
        }

        [TestMethod]
        public void GetGuidByObjectСравнениеKeyGuid()
        {
            var obj = PKHelper.CreateDataObject<DataObjectForTest>(kg1);
            Assert.IsTrue(
                PKHelper.EQPK(kg1, PKHelper.GetGuidByObject(obj)),
                "Возвращенный Guid не равен исходному.");
        }

        [TestMethod]
        public void GetGuidByObjectСравнениеNullableGuid()
        {
            var obj = PKHelper.CreateDataObject<DataObjectForTest>(ng1);
            Assert.IsTrue(
                PKHelper.EQPK(ng1, PKHelper.GetGuidByObject(obj)),
                "Возвращенный Guid не равен исходному.");
        }

        [TestMethod]
        public void GetGuidByObjectСравнениеGuid()
        {
            var obj = PKHelper.CreateDataObject<DataObjectForTest>(g1);
            Assert.IsTrue(
                PKHelper.EQPK(g1, PKHelper.GetGuidByObject(obj)),
                "Возвращенный Guid не равен исходному.");
        }

        [TestMethod]
        public void GetGuidByObjectСравнениеString()
        {
            Assert.IsTrue(
                PKHelper.EQPK(str1, PKHelper.GetKeyByObject(str1)),
                "Возвращенный Guid не равен исходному string.");
        }

        #endregion GetGuidByObject

        #region EQDataObject

        [TestMethod]
        public void EQDataObjectСравнениеПустыхОбъектов()
        {
            Assert.IsTrue(
                PKHelper.EQDataObject(null, null, true),
                "Пустые объекты сравнены неверно.");
        }

        [TestMethod]
        public void EQDataObjectСравнениеПустогоИНепустогоОбъектов()
        {
            Assert.IsFalse(
                PKHelper.EQDataObject(null, doft0, true),
                "Пустой и непустой объекты сравнены неверно.");
        }

        [TestMethod]
        public void EQDataObjectСравнениеНепустыхОбъектовБезТипа()
        {
            Assert.IsTrue(
                PKHelper.EQDataObject(doft0, doft0, false),
                "Непустые объекты сравнены без учёта типа неверно.");
        }

        [TestMethod]
        public void EQDataObjectСравнениеНепустыхОбъектов()
        {
            Assert.IsTrue(
                PKHelper.EQDataObject(doft0, doft0, true),
                "Непустые объекты сравнены неверно.");
        }

        [TestMethod]
        public void EQDataObjectСравнениеНепустыхОбъектовРазныхТипов()
        {
            var obj1 = PKHelper.CreateDataObject<ClassWithCaptions>(kg1);
            Assert.IsFalse(
                PKHelper.EQDataObject(doft1, obj1, true),
                "Непустые объекты разных типов сравнены неверно.");
        }

        #endregion EQDataObject

        #region EQDataObject T

        [TestMethod]
        public void EQDataObjectTСравнениеПустыхОбъектов()
        {
            Assert.IsTrue(
                PKHelper.EQDataObject<DataObject>(null, null),
                "Пустые объекты сравнены неверно.");
        }

        [TestMethod]
        public void EQDataObjectTСравнениеПустогоИНепустогоОбъектов()
        {
            Assert.IsFalse(
                PKHelper.EQDataObject(null, doft0),
                "Пустой и непустой объекты сравнены неверно.");
        }

        [TestMethod]
        public void EQDataObjectTСравнениеПустогоИНепустогоОбъектов1()
        {
            Assert.IsFalse(
                PKHelper.EQDataObject(doft0, null),
                "Пустой и непустой объекты сравнены неверно.");
        }

        [TestMethod]
        public void EQDataObjectTСравнениеНепустыхОбъектов()
        {
            Assert.IsTrue(
                PKHelper.EQDataObject(doft0, doft0),
                "Непустые объекты сравнены неверно.");
        }

        #endregion EQDataObject

        #region PKIn

        [TestMethod]
        public void PKInDataObject()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, new List<DataObjectForTest> { null, null, null, doft1 }),
                "Ключ существует среди перечисления.");
        }

        [TestMethod]
        public void PKInGuid()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, new List<Guid> { new Guid(), new Guid(), new Guid(), kg1.Guid }),
                "Ключ существует среди перечисления.");
        }

        [TestMethod]
        public void PKInKeyGuid()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, new List<KeyGuid> { null, null, null, kg1 }),
                "Ключ существует среди перечисления.");
        }

        [TestMethod]
        public void PKInString()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, new List<string> { null, "", "123", kg1.ToString() }),
                "Ключ существует среди перечисления.");
        }

        [TestMethod]
        public void PKInObjectFull()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, new List<object> { kg1.ToString(), kg1, kg1.Guid, doft1 }),
                "Ключ существует среди перечисления.");
        }

        [TestMethod]
        public void PKInObjectDataObject()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, new List<object> { null, null, null, doft1 }),
                "Ключ существует среди перечисления.");
        }

        [TestMethod]
        public void PKInObjectGuid()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, new List<object> { null, null, kg1.Guid, null }),
                "Ключ существует среди перечисления.");
        }

        [TestMethod]
        public void PKInObjectKeyGuid()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, new List<object> { null, kg1, null, null }),
                "Ключ существует среди перечисления.");
        }

        [TestMethod]
        public void PKInObjectString()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, new List<object> { kg1.ToString(), null, null, null }),
                "Ключ существует среди перечисления.");
        }

        [TestMethod]
        public void PKInParamsFull()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, kg1.ToString(), kg1, kg1.Guid, doft1),
                "Ключ существует среди параметров.");
        }

        [TestMethod]
        public void PKInParamsDataObject()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, null, null, null, doft1),
                "Ключ существует среди параметров.");
        }

        [TestMethod]
        public void PKInParamsGuid()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, null, null, kg1.Guid, null),
                "Ключ существует среди параметров.");
        }

        [TestMethod]
        public void PKInParamsKeyGuid()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, null, kg1, null, null),
                "Ключ существует среди параметров.");
        }

        [TestMethod]
        public void PKInParamsString()
        {
            Assert.IsTrue(
                PKHelper.PKIn(kg1, kg1.ToString(), null, null, null),
                "Ключ существует среди параметров.");
        }

        #endregion PKIn

        #region EQParentPK

        [TestMethod]
        public void EQParentPKНепустойОбъектЛинкНаСебя()
        {
            var obj = PKHelper.CreateDataObject<DataObjectWithKeyGuid>(kg2);
            obj.LinkToMaster1 = kg1;
            Assert.IsTrue(
                PKHelper.EQParentPK(
                    obj,
                    kg1,
                    nameof(DataObjectWithKeyGuid.LinkToMaster1)),
                "Сравнение ключа и родителя произведено неверно.");
        }

        [TestMethod]
        public void EQParentPKПустойОбъект()
        {
            Assert.IsFalse(
                PKHelper.EQParentPK(
                    null,
                    kg1,
                    nameof(DataObjectWithKeyGuid.LinkToMaster1)),
                "Сравнение пустого объекта и ключа произведено неверно.");
        }

        #endregion EQParentPK

        #region GetKeysString

        [TestMethod]
        public void GetKeysStringПустойСписок()
        {
            Assert.IsTrue(
                string.IsNullOrEmpty(PKHelper.GetKeysString(new List<DataObjectForTest>())),
                "Метод вернул не пустую строку.");
        }

        [TestMethod]
        public void GetKeysStringСписокИзОдногоЭлемента()
        {
            var list = new List<DataObjectForTest> { doft1 };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.AreEqual(res, PKHelper.GetKeysString(list));
        }

        [TestMethod]
        public void GetKeysStringСписокИзНесколькихЭлементов()
        {
            var list = new List<DataObjectForTest>
            {
                doft0,
                doft1,
                doft2
            };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.AreEqual(res, PKHelper.GetKeysString(list));
        }

        [TestMethod]
        public void GetKeysStringСписокИзНекорректныхЭлементов()
        {
            var list = new List<string> { "123", "321", "sssssss" };
            Assert.IsTrue(
                string.IsNullOrEmpty(PKHelper.GetKeysString(list)),
                "Метод вернул не пустую строку.");
        }

        [TestMethod]
        public void GetKeysStringParamsFull()
        {
            var list = new List<object> { g1, kg2, doft1, str1 };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.AreEqual(res, PKHelper.GetKeysString(g1, kg2, doft1, str1));
        }

        [TestMethod]
        public void GetKeysStringParamsDataObject()
        {
            var list = new List<DataObjectForTest> { doft1, doft2 };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.AreEqual(res, PKHelper.GetKeysString(doft1, doft2));
        }

        [TestMethod]
        public void GetKeysStringParamsGuid()
        {
            var list = new List<Guid> { g1, g2 };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.AreEqual(res, PKHelper.GetKeysString(g1, g2));
        }

        [TestMethod]
        public void GetKeysStringParamsKeyGuid()
        {
            var list = new List<KeyGuid> { kg1, kg2 };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.AreEqual(res, PKHelper.GetKeysString(kg1, kg2));
        }

        [TestMethod]
        public void GetKeysStringParamsString()
        {
            var list = new List<string> { str1, str2 };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.AreEqual(res, PKHelper.GetKeysString(str1, str2));
        }

        [TestMethod]
        public void GetKeysStringIEnumerableFull()
        {
            var list = new List<object>
            {
                g1,
                kg1,
                doft0,
                str1
            };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.AreEqual(res, PKHelper.GetKeysString(list));
        }

        [TestMethod]
        public void GetKeysStringIEnumerableDataObject()
        {
            var list = new List<DataObjectForTest>
            {
                doft1,
                doft2
            };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.AreEqual(res, PKHelper.GetKeysString(list));
        }

        [TestMethod]
        public void GetKeysStringIEnumerableGuid()
        {
            var list = new List<Guid>
            {
                g1,
                g2
            };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.AreEqual(res, PKHelper.GetKeysString(list));
        }

        [TestMethod]
        public void GetKeysStringIEnumerableKeyGuid()
        {
            var list = new List<KeyGuid>
            {
                kg1,
                kg2
            };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.AreEqual(res, PKHelper.GetKeysString(list));
        }

        [TestMethod]
        public void GetKeysStringIEnumerableString()
        {
            var list = new List<string>
            {
                str1,
                str2
            };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.AreEqual(res, PKHelper.GetKeysString(list));
        }

        #endregion GetKeysString

        #region CreateDataObject

        [TestMethod]
        public void CreateDataObjectСравнениеТипаОбъекта()
        {
            Assert.IsTrue(doft1 is DataObjectForTest, "Метод вернул объект неверного типа.");
        }

        [TestMethod]
        public void CreateDataObjectСравнениеКлючей()
        {
            Assert.IsTrue(PKHelper.EQPK(kg1, doft1), "Метод вернул объект с неверным ключом.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateDataObjectСравнениеException()
        {
            PKHelper.CreateDataObject<DataObjectForTest>("123");
        }

        #endregion CreateDataObject

        #region GetKeys

        [TestMethod]
        public void GetKeysSimpleParamsNull()
        {
            Assert.AreEqual(PKHelper.GetKeys(null, null, null, null).Length, 0);
        }

        [TestMethod]
        public void GetKeysSimpleParamsNullKeyGuid()
        {
            Assert.AreEqual(PKHelper.GetKeys(kg1, null, null, null).Length, 1);
        }

        [TestMethod]
        public void GetKeysSimpleParamsNullGuid()
        {
            Assert.AreEqual(PKHelper.GetKeys(null, g1, null, null).Length, 1);
        }

        [TestMethod]
        public void GetKeysSimpleParamsNullString()
        {
            Assert.AreEqual(PKHelper.GetKeys(null, null, str1, null).Length, 1);
        }

        [TestMethod]
        public void GetKeysSimpleParamsNullDataObject()
        {
            Assert.AreEqual(PKHelper.GetKeys(null, null, null, doft0).Length, 1);
        }

        [TestMethod]
        public void GetKeysSimpleParamsIncorrectString()
        {
            Assert.AreEqual(PKHelper.GetKeys("sssssssss", "123", str1, null).Length, 1);
        }

        [TestMethod]
        public void GetKeysSimpleParamsMixed()
        {
            Assert.AreEqual(PKHelper.GetKeys(kg1, g1, str1, doft0).Length, 4);
        }

        [TestMethod]
        public void GetKeysSimpleParamsKeyGuid()
        {
            Assert.AreEqual(PKHelper.GetKeys(kg1, kg2).Length, 2);
        }

        [TestMethod]
        public void GetKeysSimpleParamsGuid()
        {
            Assert.AreEqual(PKHelper.GetKeys(g1, g2).Length, 2);
        }

        [TestMethod]
        public void GetKeysSimpleParamsString()
        {
            Assert.AreEqual(PKHelper.GetKeys(str1, str2).Length, 2);
        }

        [TestMethod]
        public void GetKeysSimpleParamsDataObject()
        {
            Assert.AreEqual(PKHelper.GetKeys(doft1, doft2).Length, 2);
        }

        [TestMethod]
        public void GetKeysSimpleIEnumerableGuid()
        {
            var list = new List<Guid>
            {
                g1,
                g2
            };
            Assert.AreEqual(PKHelper.GetKeys(list).Length, 2);
        }

        [TestMethod]
        public void GetKeysSimpleIEnumerableKeyGuid()
        {
            var list = new List<KeyGuid>
            {
                kg1,
                kg2
            };
            Assert.AreEqual(PKHelper.GetKeys(list).Length, 2);
        }

        [TestMethod]
        public void GetKeysSimpleIEnumerableString()
        {
            var list = new List<string>
            {
                str1,
                str2
            };
            Assert.AreEqual(PKHelper.GetKeys(list).Length, 2);
        }

        [TestMethod]
        public void GetKeysSimpleIEnumerableDataObject()
        {
            var list = new List<DataObjectForTest>
            {
                doft1,
                doft2
            };
            Assert.AreEqual(PKHelper.GetKeys(list).Length, 2);
        }

        [TestMethod]
        public void GetKeysSimpleIEnumerableObject()
        {
            var list = new List<object>
            {
                g1,
                kg1,
                doft0,
                str1
            };
            Assert.AreEqual(PKHelper.GetKeys(list).Length, 4);
        }

        [TestMethod]
        public void GetKeysParamsIEnumerableGuid()
        {
            var list = new List<Guid>
            {
                g1,
                g2
            };
            Assert.AreEqual(PKHelper.GetKeys(list, list).Length, 2);
        }

        [TestMethod]
        public void GetKeysParamsIEnumerableKeyGuid()
        {
            var list = new List<KeyGuid>
            {
                kg1,
                kg2
            };
            Assert.AreEqual(PKHelper.GetKeys(list, list).Length, 2);
        }

        [TestMethod]
        public void GetKeysParamsIEnumerableString()
        {
            var list = new List<string>
            {
                str1,
                str2
            };
            Assert.AreEqual(PKHelper.GetKeys(list, list).Length, 2);
        }

        [TestMethod]
        public void GetKeysParamsIEnumerableObject()
        {
            var list = new List<object>
            {
                g1,
                kg1,
                doft0,
                str1
            };
            Assert.AreEqual(PKHelper.GetKeys(list, list).Length, 4);
        }

        [TestMethod]
        public void GetKeysParamsIEnumerableMixed()
        {
            var listg = new List<Guid>
            {
                g1,
                g2
            };
            var listkg = new List<KeyGuid>
            {
                kg1,
                kg2
            };
            var liststr = new List<string>
            {
                str1,
                str2
            };
            var list = new List<object>
            {
                g1,
                kg1,
                doft0,
                str1
            };
            Assert.AreEqual(PKHelper.GetKeys(list, listg, listkg, liststr).Length, 7);
        }

        [TestMethod]
        public void GetKeysIEnumerableIEnumerableGuid()
        {
            var list = new List<Guid>
            {
                g1,
                g2
            };

            var listlist = new List<List<Guid>>
            {
                list,
                list
            };
            Assert.AreEqual(PKHelper.GetKeys(listlist).Length, 2);
        }

        [TestMethod]
        public void GetKeysIEnumerableIEnumerableKeyGuid()
        {
            var list = new List<KeyGuid>
            {
                kg1,
                kg2
            };

            var listlist = new List<List<KeyGuid>>
            {
                list,
                list
            };
            Assert.AreEqual(PKHelper.GetKeys(listlist).Length, 2);
        }

        [TestMethod]
        public void GetKeysIEnumerableIEnumerableString()
        {
            var list = new List<string>
            {
                str1,
                str2
            };

            var listlist = new List<List<string>>
            {
                list,
                list
            };
            Assert.AreEqual(PKHelper.GetKeys(listlist).Length, 2);
        }

        [TestMethod]
        public void GetKeysIEnumerableIEnumerableObject()
        {
            var list = new List<object>
            {
                g1,
                kg1,
                doft0,
                str1
            };

            var listlist = new List<List<object>>
            {
                list,
                list
            };
            Assert.AreEqual(PKHelper.GetKeys(listlist).Length, 4);
        }

        [TestMethod]
        public void GetKeysIEnumerableIEnumerableMixed()
        {
            var listg = new List<Guid>
            {
                g1,
                g2
            };
            var listlistg = new List<List<Guid>>
            {
                listg,
                listg
            };

            var listkg = new List<KeyGuid>
            {
                kg1,
                kg2
            };
            var listlistkg = new List<List<KeyGuid>>
            {
                listkg,
                listkg
            };

            var liststr = new List<string>
            {
                str1,
                str2
            };
            var listliststr = new List<List<string>>
            {
                liststr,
                liststr
            };

            var list = new List<object>
            {
                g1,
                kg1,
                doft0,
                str1
            };
            var listlist = new List<List<object>>
            {
                list,
                list
            };
            Assert.AreEqual(PKHelper.GetKeys(listlist, listlistg, listlistkg, listliststr).Length, 7);
        }

        [TestMethod]
        public void GetKeysIEnumerableIEnumerableMixedWithSimple()
        {
            var listg = new List<Guid>
            {
                g1,
                g2
            };
            var listlistg = new List<List<Guid>>
            {
                listg,
                listg
            };

            var listkg = new List<KeyGuid>
            {
                kg1,
                kg2
            };
            var listlistkg = new List<List<KeyGuid>>
            {
                listkg,
                listkg
            };

            var liststr = new List<string>
            {
                str1,
                str2
            };
            var listliststr = new List<List<string>>
            {
                liststr,
                liststr
            };

            var list = new List<object>
            {
                g1,
                kg1,
                doft0,
                str1
            };
            var listlist = new List<List<object>>
            {
                list,
                list
            };
            Assert.AreEqual(PKHelper.GetKeys(listlist, listlistg, listlistkg, listliststr, kg1, g1, str1, doft0).Length, 7);
        }

        [TestMethod]
        public void GetKeysIEnumerableIEnumerableMixedWithIEnumerable()
        {
            var listg = new List<Guid>
            {
                g1,
                g2
            };
            var listlistg = new List<List<Guid>>
            {
                listg,
                listg
            };

            var listkg = new List<KeyGuid>
            {
                kg1,
                kg2
            };
            var listlistkg = new List<List<KeyGuid>>
            {
                listkg,
                listkg
            };

            var liststr = new List<string>
            {
                str1,
                str2
            };
            var listliststr = new List<List<string>>
            {
                liststr,
                liststr
            };

            var list = new List<object>
            {
                g1,
                kg1,
                doft0,
                str1
            };
            var listlist = new List<List<object>>
            {
                list,
                list
            };
            Assert.AreEqual(PKHelper.GetKeys(listlist, listlistg, listlistkg, listliststr, list, listg, listkg, liststr).Length, 7);
        }

        #endregion GetKeys

        #region CreateObjectsByKey

        [TestMethod]
        public void CreateDataObjectsСравнениеКоличества00()
        {
            Assert.AreEqual(0, PKHelper.CreateObjectsByKey<DataObjectForTest>("123").Length);
        }

        [TestMethod]
        public void CreateDataObjectsСравнениеКоличества01()
        {
            Assert.AreEqual(1, PKHelper.CreateObjectsByKey<DataObjectForTest>(kg1).Length);
        }

        [TestMethod]
        public void CreateDataObjectsСравнениеКоличества02()
        {
            Assert.AreEqual(2, PKHelper.CreateObjectsByKey<DataObjectForTest>(kg1, kg2).Length);
        }

        [TestMethod]
        public void CreateDataObjectsСравнениеКоличества03()
        {
            Assert.AreEqual(1, PKHelper.CreateObjectsByKey<DataObjectForTest>(kg1, kg1).Length);
        }

        [TestMethod]
        public void CreateDataObjectsСравнениеТипа00()
        {
            Assert.AreEqual(typeof(DataObjectForTest), PKHelper.CreateObjectsByKey<DataObjectForTest>(kg1).First().GetType());
        }

        [TestMethod]
        public void CreateDataObjectsСравнениеТипа01()
        {
            Assert.AreEqual(typeof(DataObjectWithKeyGuid), PKHelper.CreateObjectsByKey<DataObjectWithKeyGuid>(kg1).First().GetType());
        }

        #endregion CreateObjectsByKey
    }
}