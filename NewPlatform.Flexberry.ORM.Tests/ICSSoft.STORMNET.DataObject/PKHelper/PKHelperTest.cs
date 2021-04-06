namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.KeyGen;

    using Xunit;

    public class PKHelperTests
    {
        private static readonly KeyGuid kg1 = KeyGuid.NewGuid();

        private static readonly KeyGuid kg2 = KeyGuid.NewGuid();

        private readonly Guid? ng1 = Guid.NewGuid();

        private readonly Guid? ng2 = Guid.NewGuid();

        private readonly Guid g1 = Guid.NewGuid();

        private readonly Guid g2 = Guid.NewGuid();

        private readonly string str1 = KeyGuid.NewGuid().ToString();

        private readonly string str2 = KeyGuid.NewGuid().ToString();

        private readonly DataObjectForTest doft0 = new DataObjectForTest();

        private readonly DataObjectForTest doft1 = PKHelper.CreateDataObject<DataObjectForTest>(kg1);

        private readonly DataObjectForTest doft2 = PKHelper.CreateDataObject<DataObjectForTest>(kg2);

        #region EQPK

        [Fact]
        public void EQPKСравнениеПустыхОбъектов()
        {
            Assert.True(!PKHelper.EQPK(null, null), "Пустые объекты сравнены неверно.");
        }

        [Fact]
        public void EQPKСравнениеПустогоОбъектаИБезКлюча()
        {
            Assert.True(!PKHelper.EQPK(null, new object()), "Пустой объект и объект без ключа сравнены неверно.");
        }

        [Fact]
        public void EQPKСравнениеОбъектовБезКлючей()
        {
            Assert.True(!PKHelper.EQPK(new object(), new object()), "Объекты без ключей сравнены неверно.");
        }

        [Fact]
        public void EQPKСравнениеПустогоОбъектаИСКлючом()
        {
            Assert.True(!PKHelper.EQPK(null, doft0), "Пустой объект и объект с ключом сравнены неверно.");
        }

        [Fact]
        public void EQPKСравнениеОбъектовСКлючомИБезКлючом()
        {
            Assert.True(!PKHelper.EQPK(new object(), doft0), "Объект с ключом и объект без ключа сравнены неверно.");
        }

        [Fact]
        public void EQPKСравнениеОбъектовСОдинаковымиGuid()
        {
            var obj2 = PKHelper.CreateDataObject<DataObjectForTest>(doft0);
            Assert.True(PKHelper.EQPK(doft0, obj2), "Объекты с одинаковыми Guid сравнены неверно.");
        }

        [Fact]
        public void EQPKСравнениеОбъектовСРазнымиGuid()
        {
            Assert.True(
                !PKHelper.EQPK(doft1, doft2),
                "Объекты с разными Guid сравнены неверно.");
        }

        #endregion EQPK

        #region GetKeyByObject

        [Fact]
        public void GetKeyByObjectСравнениеKeyGuid()
        {
            Assert.True(
                PKHelper.EQPK(kg1, PKHelper.GetKeyByObject(kg1)),
                "Возвращенный KeyGuid не равен исходному.");
        }

        [Fact]
        public void GetKeyByObjectСравнениеGuid()
        {
            Assert.True(
                PKHelper.EQPK(g1, PKHelper.GetKeyByObject(g1)),
                "Возвращенный KeyGuid не равен исходному Guid.");
        }

        [Fact]
        public void GetKeyByObjectСравнениеString()
        {
            Assert.True(
                PKHelper.EQPK(str1, PKHelper.GetKeyByObject(str1)),
                "Возвращенный KeyGuid не равен исходному string.");
        }

        #endregion GetKeyByObject

        #region GetGuidByObject

        [Fact]
        public void GetGuidByObjectNull()
        {
            Assert.Equal(null, PKHelper.GetGuidByObject(null));
        }

        [Fact]
        public void GetGuidByObjectСравнениеKeyGuid()
        {
            var obj = PKHelper.CreateDataObject<DataObjectForTest>(kg1);
            Assert.True(
                PKHelper.EQPK(kg1, PKHelper.GetGuidByObject(obj)),
                "Возвращенный Guid не равен исходному.");
        }

        [Fact]
        public void GetGuidByObjectСравнениеNullableGuid()
        {
            var obj = PKHelper.CreateDataObject<DataObjectForTest>(ng1);
            Assert.True(
                PKHelper.EQPK(ng1, PKHelper.GetGuidByObject(obj)),
                "Возвращенный Guid не равен исходному.");
        }

        [Fact]
        public void GetGuidByObjectСравнениеGuid()
        {
            var obj = PKHelper.CreateDataObject<DataObjectForTest>(g1);
            Assert.True(
                PKHelper.EQPK(g1, PKHelper.GetGuidByObject(obj)),
                "Возвращенный Guid не равен исходному.");
        }

        [Fact]
        public void GetGuidByObjectСравнениеString()
        {
            Assert.True(
                PKHelper.EQPK(str1, PKHelper.GetKeyByObject(str1)),
                "Возвращенный Guid не равен исходному string.");
        }

        #endregion GetGuidByObject

        #region EQDataObject

        [Fact]
        public void EQDataObjectСравнениеПустыхОбъектов()
        {
            Assert.True(
                PKHelper.EQDataObject(null, null, true),
                "Пустые объекты сравнены неверно.");
        }

        [Fact]
        public void EQDataObjectСравнениеПустогоИНепустогоОбъектов()
        {
            Assert.True(!
                PKHelper.EQDataObject(null, doft0, true),
                "Пустой и непустой объекты сравнены неверно.");
        }

        [Fact]
        public void EQDataObjectСравнениеНепустыхОбъектовБезТипа()
        {
            Assert.True(
                PKHelper.EQDataObject(doft0, doft0, false),
                "Непустые объекты сравнены без учёта типа неверно.");
        }

        [Fact]
        public void EQDataObjectСравнениеНепустыхОбъектов()
        {
            Assert.True(
                PKHelper.EQDataObject(doft0, doft0, true),
                "Непустые объекты сравнены неверно.");
        }

        [Fact]
        public void EQDataObjectСравнениеНепустыхОбъектовРазныхТипов()
        {
            var obj1 = PKHelper.CreateDataObject<ClassWithCaptions>(kg1);
            Assert.True(!
                PKHelper.EQDataObject(doft1, obj1, true),
                "Непустые объекты разных типов сравнены неверно.");
        }

        #endregion EQDataObject

        #region EQDataObject T

        [Fact]
        public void EQDataObjectTСравнениеПустыхОбъектов()
        {
            Assert.True(
                PKHelper.EQDataObject<DataObject>(null, null),
                "Пустые объекты сравнены неверно.");
        }

        [Fact]
        public void EQDataObjectTСравнениеПустогоИНепустогоОбъектов()
        {
            Assert.True(!
                PKHelper.EQDataObject(null, doft0),
                "Пустой и непустой объекты сравнены неверно.");
        }

        [Fact]
        public void EQDataObjectTСравнениеПустогоИНепустогоОбъектов1()
        {
            Assert.True(!
                PKHelper.EQDataObject(doft0, null),
                "Пустой и непустой объекты сравнены неверно.");
        }

        [Fact]
        public void EQDataObjectTСравнениеНепустыхОбъектов()
        {
            Assert.True(
                PKHelper.EQDataObject(doft0, doft0),
                "Непустые объекты сравнены неверно.");
        }

        #endregion EQDataObject

        #region PKIn

        [Fact]
        public void PKInDataObject()
        {
            Assert.True(
                PKHelper.PKIn(kg1, new List<DataObjectForTest> { null, null, null, doft1 }),
                "Ключ существует среди перечисления.");
        }

        [Fact]
        public void PKInGuid()
        {
            Assert.True(
                PKHelper.PKIn(kg1, new List<Guid> { new Guid(), new Guid(), new Guid(), kg1.Guid }),
                "Ключ существует среди перечисления.");
        }

        [Fact]
        public void PKInKeyGuid()
        {
            Assert.True(
                PKHelper.PKIn(kg1, new List<KeyGuid> { null, null, null, kg1 }),
                "Ключ существует среди перечисления.");
        }

        [Fact]
        public void PKInString()
        {
            Assert.True(
                PKHelper.PKIn(kg1, new List<string> { null, "", "123", kg1.ToString() }),
                "Ключ существует среди перечисления.");
        }

        [Fact]
        public void PKInObjectFull()
        {
            Assert.True(
                PKHelper.PKIn(kg1, new List<object> { kg1.ToString(), kg1, kg1.Guid, doft1 }),
                "Ключ существует среди перечисления.");
        }

        [Fact]
        public void PKInObjectDataObject()
        {
            Assert.True(
                PKHelper.PKIn(kg1, new List<object> { null, null, null, doft1 }),
                "Ключ существует среди перечисления.");
        }

        [Fact]
        public void PKInObjectGuid()
        {
            Assert.True(
                PKHelper.PKIn(kg1, new List<object> { null, null, kg1.Guid, null }),
                "Ключ существует среди перечисления.");
        }

        [Fact]
        public void PKInObjectKeyGuid()
        {
            Assert.True(
                PKHelper.PKIn(kg1, new List<object> { null, kg1, null, null }),
                "Ключ существует среди перечисления.");
        }

        [Fact]
        public void PKInObjectString()
        {
            Assert.True(
                PKHelper.PKIn(kg1, new List<object> { kg1.ToString(), null, null, null }),
                "Ключ существует среди перечисления.");
        }

        [Fact]
        public void PKInParamsFull()
        {
            Assert.True(
                PKHelper.PKIn(kg1, kg1.ToString(), kg1, kg1.Guid, doft1),
                "Ключ существует среди параметров.");
        }

        [Fact]
        public void PKInParamsDataObject()
        {
            Assert.True(
                PKHelper.PKIn(kg1, null, null, null, doft1),
                "Ключ существует среди параметров.");
        }

        [Fact]
        public void PKInParamsGuid()
        {
            Assert.True(
                PKHelper.PKIn(kg1, null, null, kg1.Guid, null),
                "Ключ существует среди параметров.");
        }

        [Fact]
        public void PKInParamsKeyGuid()
        {
            Assert.True(
                PKHelper.PKIn(kg1, null, kg1, null, null),
                "Ключ существует среди параметров.");
        }

        [Fact]
        public void PKInParamsString()
        {
            Assert.True(
                PKHelper.PKIn(kg1, kg1.ToString(), null, null, null),
                "Ключ существует среди параметров.");
        }

        #endregion PKIn

        #region EQParentPK

        [Fact]
        public void EQParentPKНепустойОбъектЛинкНаСебя()
        {
            var obj = PKHelper.CreateDataObject<DataObjectWithKeyGuid>(kg2);
            obj.LinkToMaster1 = kg1;
            Assert.True(
                PKHelper.EQParentPK(
                    obj,
                    kg1,
                    nameof(DataObjectWithKeyGuid.LinkToMaster1)),
                "Сравнение ключа и родителя произведено неверно.");
        }

        [Fact]
        public void EQParentPKПустойОбъект()
        {
            Assert.True(!
                PKHelper.EQParentPK(
                    null,
                    kg1,
                    nameof(DataObjectWithKeyGuid.LinkToMaster1)),
                "Сравнение пустого объекта и ключа произведено неверно.");
        }

        #endregion EQParentPK

        #region GetKeysString

        [Fact]
        public void GetKeysStringПустойСписок()
        {
            Assert.True(
                string.IsNullOrEmpty(PKHelper.GetKeysString(new List<DataObjectForTest>())),
                "Метод вернул не пустую строку.");
        }

        [Fact]
        public void GetKeysStringСписокИзОдногоЭлемента()
        {
            var list = new List<DataObjectForTest> { doft1 };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.Equal(res, PKHelper.GetKeysString(list));
        }

        [Fact]
        public void GetKeysStringСписокИзНесколькихЭлементов()
        {
            var list = new List<DataObjectForTest>
            {
                doft0,
                doft1,
                doft2
            };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.Equal(res, PKHelper.GetKeysString(list));
        }

        [Fact]
        public void GetKeysStringСписокИзНекорректныхЭлементов()
        {
            var list = new List<string> { "123", "321", "sssssss" };
            Assert.True(
                string.IsNullOrEmpty(PKHelper.GetKeysString(list)),
                "Метод вернул не пустую строку.");
        }

        [Fact]
        public void GetKeysStringParamsFull()
        {
            var list = new List<object> { g1, kg2, doft1, str1 };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.Equal(res, PKHelper.GetKeysString(g1, kg2, doft1, str1));
        }

        [Fact]
        public void GetKeysStringParamsDataObject()
        {
            var list = new List<DataObjectForTest> { doft1, doft2 };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.Equal(res, PKHelper.GetKeysString(doft1, doft2));
        }

        [Fact]
        public void GetKeysStringParamsGuid()
        {
            var list = new List<Guid> { g1, g2 };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.Equal(res, PKHelper.GetKeysString(g1, g2));
        }

        [Fact]
        public void GetKeysStringParamsKeyGuid()
        {
            var list = new List<KeyGuid> { kg1, kg2 };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.Equal(res, PKHelper.GetKeysString(kg1, kg2));
        }

        [Fact]
        public void GetKeysStringParamsString()
        {
            var list = new List<string> { str1, str2 };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.Equal(res, PKHelper.GetKeysString(str1, str2));
        }

        [Fact]
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
            Assert.Equal(res, PKHelper.GetKeysString(list));
        }

        [Fact]
        public void GetKeysStringIEnumerableDataObject()
        {
            var list = new List<DataObjectForTest>
            {
                doft1,
                doft2
            };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.Equal(res, PKHelper.GetKeysString(list));
        }

        [Fact]
        public void GetKeysStringIEnumerableGuid()
        {
            var list = new List<Guid>
            {
                g1,
                g2
            };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.Equal(res, PKHelper.GetKeysString(list));
        }

        [Fact]
        public void GetKeysStringIEnumerableKeyGuid()
        {
            var list = new List<KeyGuid>
            {
                kg1,
                kg2
            };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.Equal(res, PKHelper.GetKeysString(list));
        }

        [Fact]
        public void GetKeysStringIEnumerableString()
        {
            var list = new List<string>
            {
                str1,
                str2
            };
            string res = string.Join(",", list.Select(o => $"'{PKHelper.GetGuidByObject(o)}'"));
            Assert.Equal(res, PKHelper.GetKeysString(list));
        }

        #endregion GetKeysString

        #region CreateDataObject

        [Fact]
        public void CreateDataObjectСравнениеТипаОбъекта()
        {
            Assert.True(doft1 is DataObjectForTest, "Метод вернул объект неверного типа.");
        }

        [Fact]
        public void CreateDataObjectСравнениеКлючей()
        {
            Assert.True(PKHelper.EQPK(kg1, doft1), "Метод вернул объект с неверным ключом.");
        }

        [Fact]
        public void CreateDataObjectСравнениеException()
        {
            Assert.Throws<ArgumentException>(() => PKHelper.CreateDataObject<DataObjectForTest>("123"));
        }

        #endregion CreateDataObject

        #region GetKeys

        [Fact]
        public void GetKeysSimpleParamsNull()
        {
            Assert.Equal(PKHelper.GetKeys(null, null, null, null).Length, 0);
        }

        [Fact]
        public void GetKeysSimpleParamsNullKeyGuid()
        {
            Assert.Equal(PKHelper.GetKeys(kg1, null, null, null).Length, 1);
        }

        [Fact]
        public void GetKeysSimpleParamsNullGuid()
        {
            Assert.Equal(PKHelper.GetKeys(null, g1, null, null).Length, 1);
        }

        [Fact]
        public void GetKeysSimpleParamsNullString()
        {
            Assert.Equal(PKHelper.GetKeys(null, null, str1, null).Length, 1);
        }

        [Fact]
        public void GetKeysSimpleParamsNullDataObject()
        {
            Assert.Equal(PKHelper.GetKeys(null, null, null, doft0).Length, 1);
        }

        [Fact]
        public void GetKeysSimpleParamsIncorrectString()
        {
            Assert.Equal(PKHelper.GetKeys("sssssssss", "123", str1, null).Length, 1);
        }

        [Fact]
        public void GetKeysSimpleParamsMixed()
        {
            Assert.Equal(PKHelper.GetKeys(kg1, g1, str1, doft0).Length, 4);
        }

        [Fact]
        public void GetKeysSimpleParamsKeyGuid()
        {
            Assert.Equal(PKHelper.GetKeys(kg1, kg2).Length, 2);
        }

        [Fact]
        public void GetKeysSimpleParamsGuid()
        {
            Assert.Equal(PKHelper.GetKeys(g1, g2).Length, 2);
        }

        [Fact]
        public void GetKeysSimpleParamsString()
        {
            Assert.Equal(PKHelper.GetKeys(str1, str2).Length, 2);
        }

        [Fact]
        public void GetKeysSimpleParamsDataObject()
        {
            Assert.Equal(PKHelper.GetKeys(doft1, doft2).Length, 2);
        }

        [Fact]
        public void GetKeysSimpleIEnumerableGuid()
        {
            var list = new List<Guid>
            {
                g1,
                g2
            };
            Assert.Equal(PKHelper.GetKeys(list).Length, 2);
        }

        [Fact]
        public void GetKeysSimpleIEnumerableNullableGuid()
        {
            var list = new List<Guid?>
            {
                ng1,
                ng2
            };
            Assert.Equal(PKHelper.GetKeys(list).Length, 2);
        }

        [Fact]
        public void GetKeysSimpleIEnumerableKeyGuid()
        {
            var list = new List<KeyGuid>
            {
                kg1,
                kg2
            };
            Assert.Equal(PKHelper.GetKeys(list).Length, 2);
        }

        [Fact]
        public void GetKeysSimpleIEnumerableString()
        {
            var list = new List<string>
            {
                str1,
                str2
            };
            Assert.Equal(PKHelper.GetKeys(list).Length, 2);
        }

        [Fact]
        public void GetKeysSimpleIEnumerableDataObject()
        {
            var list = new List<DataObjectForTest>
            {
                doft1,
                doft2
            };
            Assert.Equal(PKHelper.GetKeys(list).Length, 2);
        }

        [Fact]
        public void GetKeysSimpleIEnumerableObject()
        {
            var list = new List<object>
            {
                g1,
                kg1,
                doft0,
                str1
            };
            Assert.Equal(PKHelper.GetKeys(list).Length, 4);
        }

        [Fact]
        public void GetKeysParamsIEnumerableGuid()
        {
            var list = new List<Guid>
            {
                g1,
                g2
            };
            Assert.Equal(PKHelper.GetKeys(list, list).Length, 2);
        }

        [Fact]
        public void GetKeysParamsIEnumerableKeyGuid()
        {
            var list = new List<KeyGuid>
            {
                kg1,
                kg2
            };
            Assert.Equal(PKHelper.GetKeys(list, list).Length, 2);
        }

        [Fact]
        public void GetKeysParamsIEnumerableString()
        {
            var list = new List<string>
            {
                str1,
                str2
            };
            Assert.Equal(PKHelper.GetKeys(list, list).Length, 2);
        }

        [Fact]
        public void GetKeysParamsIEnumerableObject()
        {
            var list = new List<object>
            {
                g1,
                kg1,
                doft0,
                str1
            };
            Assert.Equal(PKHelper.GetKeys(list, list).Length, 4);
        }

        [Fact]
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
            Assert.Equal(PKHelper.GetKeys(list, listg, listkg, liststr).Length, 7);
        }

        [Fact]
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
            Assert.Equal(PKHelper.GetKeys(listlist).Length, 2);
        }

        [Fact]
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
            Assert.Equal(PKHelper.GetKeys(listlist).Length, 2);
        }

        [Fact]
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
            Assert.Equal(PKHelper.GetKeys(listlist).Length, 2);
        }

        [Fact]
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
            Assert.Equal(PKHelper.GetKeys(listlist).Length, 4);
        }

        [Fact]
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
            Assert.Equal(PKHelper.GetKeys(listlist, listlistg, listlistkg, listliststr).Length, 7);
        }

        [Fact]
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
            Assert.Equal(PKHelper.GetKeys(listlist, listlistg, listlistkg, listliststr, kg1, g1, str1, doft0).Length, 7);
        }

        [Fact]
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

            var listng = new List<Guid?>
            {
                ng1,
                ng2
            };
            var listlistng = new List<List<Guid?>>
            {
                listng,
                listng
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
            Assert.Equal(PKHelper.GetKeys(listlist, listlistg, listlistkg, listliststr, list, listg, listkg, liststr).Length, 7);
        }

        #endregion GetKeys

        #region CreateObjectsByKey

        [Fact]
        public void CreateDataObjectsСравнениеКоличества00()
        {
            Assert.Equal(0, PKHelper.CreateObjectsByKey<DataObjectForTest>("123").Length);
        }

        [Fact]
        public void CreateDataObjectsСравнениеКоличества01()
        {
            Assert.Equal(1, PKHelper.CreateObjectsByKey<DataObjectForTest>(kg1).Length);
        }

        [Fact]
        public void CreateDataObjectsСравнениеКоличества02()
        {
            Assert.Equal(2, PKHelper.CreateObjectsByKey<DataObjectForTest>(kg1, kg2).Length);
        }

        [Fact]
        public void CreateDataObjectsСравнениеКоличества03()
        {
            Assert.Equal(1, PKHelper.CreateObjectsByKey<DataObjectForTest>(kg1, kg1).Length);
        }

        [Fact]
        public void CreateDataObjectsСравнениеТипа00()
        {
            Assert.Equal(typeof(DataObjectForTest), PKHelper.CreateObjectsByKey<DataObjectForTest>(kg1).First().GetType());
        }

        [Fact]
        public void CreateDataObjectsСравнениеТипа01()
        {
            Assert.Equal(typeof(DataObjectWithKeyGuid), PKHelper.CreateObjectsByKey<DataObjectWithKeyGuid>(kg1).First().GetType());
        }

        #endregion CreateObjectsByKey
    }
}