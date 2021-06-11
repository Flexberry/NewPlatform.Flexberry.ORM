namespace NewPlatform.Flexberry.ORM.IntegratedTests.Postgres
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.UserDataTypes;

    using IIS.TestClassesForPostgres;

    using Xunit;

    /// <summary>
    /// Юнит-тесты для PostgresDataService.
    /// </summary>
    public class PostgresDataServiceTest : BaseIntegratedTest
    {
        /// <summary>
        /// Сервис данных Postgres.
        /// </summary>
        protected PostgresDataService DataService;

        public PostgresDataServiceTest()
            : base("test")
        {
            foreach (var ds in DataServices)
            {
                if (ds is PostgresDataService)
                    DataService = ds as PostgresDataService;
            }
        }

        /// <inheritdoc />
        protected override string MssqlScript
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override string PostgresScript
        {
            get
            {
                return Resources.PostgresDataServiceTestScript;
            }
        }

        /// <inheritdoc />
        protected override string OracleScript
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc />
        protected override void AssertWatchdog(bool notEmpty)
        {
        }

        /// <summary>
        /// Выполняется тестирование сохранения и сравнения строки, если в ней присутствует символ '\'.
        /// </summary>
        [Fact]
        public void TestForwardSlash()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_string { Attr = "abc\\de" };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_string { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.Equal(clazz.Attr, clazz2.Attr);
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - string.
        /// </summary>
        [Fact]
        public void TestString()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_string { Attr = "abc" };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_string { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = "123";
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_string { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_string { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - char.
        /// </summary>
        [Fact]
        public void TestChar()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_char { Attr = 'a' };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_char { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = 'b';
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_char { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_char { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - DateTime.
        /// </summary>
        [Fact]
        public void TestDateTime()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_DateTime { Attr = DateTime.Now };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_DateTime { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr.ToString("d MMM yyyy HH:mm:ss.fff") == clazz2.Attr.ToString("d MMM yyyy HH:mm:ss.fff"));
            clazz2.Attr = clazz2.Attr.AddMonths(1);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_DateTime { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr.ToString("d MMM yyyy HH:mm:ss.fff") != clazz2.Attr.ToString("d MMM yyyy HH:mm:ss.fff"));
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_DateTime { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - NullableDateTime.
        /// </summary>
        [Fact]
        public void TestNullableDateTime()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_NullableDateTime { Attr = (NullableDateTime)DateTime.Now };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_NullableDateTime { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr.Value.ToString("d MMM yyyy HH:mm:ss.fff") == clazz2.Attr.Value.ToString("d MMM yyyy HH:mm:ss.fff"));
            clazz2.Attr = (NullableDateTime)clazz2.Attr.Value.AddMonths(1);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_NullableDateTime { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr.Value.ToString("d MMM yyyy HH:mm:ss.fff") != clazz2.Attr.Value.ToString("d MMM yyyy HH:mm:ss.fff"));
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_NullableDateTime { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - NullableDecimal.
        /// </summary>
        [Fact]
        public void TestNullableDecimal()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_NullableDecimal { Attr = (NullableDecimal)new decimal(12345.6789) };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_NullableDecimal { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = (NullableDecimal)(clazz2.Attr.Value + 1);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_NullableDecimal { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_NullableDecimal { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - NullableInt.
        /// </summary>
        [Fact]
        public void TestNullableInt()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_NullableInt { Attr = 1111 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_NullableInt { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = clazz2.Attr.Value + 1;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_NullableInt { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_NullableInt { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - WebFile.
        /// </summary>
        [Fact]
        public void TestWebFile()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_WebFile { Attr = new WebFile { Name = "Test1" } };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_WebFile { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True((string)clazz.Attr == (string)clazz2.Attr);
            clazz2.Attr = new WebFile { Name = "Test2" };
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_WebFile { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True((string)clazz.Attr != (string)clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_WebFile { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - bool.
        /// </summary>
        [Fact]
        public void TestBool()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_bool { Attr = true };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_bool { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = false;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_bool { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_bool { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - byte.
        /// </summary>
        [Fact]
        public void TestByte()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_byte { Attr = 11 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_byte { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = 12;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_byte { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_byte { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - decimal.
        /// </summary>
        [Fact]
        public void TestDecimal()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_decimal { Attr = 11 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_decimal { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = 12;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_decimal { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_decimal { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - double.
        /// </summary>
        [Fact]
        public void TestDouble()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_double { Attr = 11 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_double { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True((int)clazz.Attr == (int)clazz2.Attr);
            clazz2.Attr = 12;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_double { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True((int)clazz.Attr != (int)clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_double { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - float.
        /// </summary>
        [Fact]
        public void TestFloat()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_float { Attr = 11 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_float { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True((int)clazz.Attr == (int)clazz2.Attr);
            clazz2.Attr = 12;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_float { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True((int)clazz.Attr != (int)clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_float { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - guid.
        /// </summary>
        [Fact]
        public void TestGuid()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_guid { Attr = Guid.NewGuid() };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_guid { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = Guid.NewGuid();
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_guid { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_guid { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - int.
        /// </summary>
        [Fact]
        public void TestInt()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_int { Attr = 11 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_int { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = 12;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_int { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_int { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - long.
        /// </summary>
        [Fact]
        public void TestLong()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_long { Attr = 11 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_long { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = 12;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_long { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_long { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - object. Object должен быть представлен как массив байт (byte[]).
        /// </summary>
        [Fact]
        public void TestObject()
        {
            if (DataService == null)
            {
                return;
            }

            var b1 = new byte[] { 1, 2, 3 };
            var b2 = new byte[] { 3, 2, 1 };
            var clazz = new Class_object { Attr = b1 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_object { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            byte[] b = (byte[])clazz2.Attr;
            Assert.True(b1[0] == b[0] && b1[1] == b[1] && b1[2] == b[2]);
            clazz2.Attr = b2;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_object { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            b = (byte[])clazz2.Attr;
            Assert.True(!(b2[0] == b[0] && b2[1] == b[1] && b2[2] == b[2]));
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_object { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - sbyte.
        /// </summary>
        [Fact]
        public void TestSbyte()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_sbyte { Attr = 11 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_sbyte { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = 12;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_sbyte { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_sbyte { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - short.
        /// </summary>
        [Fact]
        public void TestShort()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_short { Attr = 11 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_short { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = 12;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_short { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_short { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - uint.
        /// </summary>
        [Fact]
        public void TestUint()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_uint { Attr = 11 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_uint { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = 12;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_uint { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_uint { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - ulong.
        /// </summary>
        [Fact]
        public void TestUlong()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_ulong { Attr = 11 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_ulong { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = 12;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_ulong { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_ulong { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование операций с типом Flexberry - ushort.
        /// </summary>
        [Fact]
        public void TestUshort()
        {
            if (DataService == null)
            {
                return;
            }

            var clazz = new Class_ushort { Attr = 11 };
            DataService.UpdateObject(clazz);
            var clazz2 = new Class_ushort { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr == clazz2.Attr);
            clazz2.Attr = 12;
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_ushort { __PrimaryKey = clazz.__PrimaryKey };
            DataService.LoadObject(clazz2);
            Assert.True(clazz.Attr != clazz2.Attr);
            clazz2.SetStatus(ObjectStatus.Deleted);
            DataService.UpdateObject(clazz2);
            clazz2 = new Class_ushort { __PrimaryKey = clazz.__PrimaryKey };

            // Assert.
            Assert.Throws<CantFindDataObjectException>(() => DataService.LoadObject(clazz2));
        }

        /// <summary>
        /// Выполняется тестирование длинных имён.
        /// </summary>
        [Fact]
        public void LongNamesTest()
        {
            if (DataService == null)
            {
                return;
            }

            var masterRoot = new MasterRoot { MasterAttr = 234 };

            var мастерКласс01 = new МастерКлассДлинноеИмя { MasterAttr2 = true, АтрибутМастерКласса01 = "АтрибутМастерКласса01", MasterRoot = masterRoot };
            var мастерКласс02 = new МастерКлассДлинноеИмя { MasterAttr2 = false, АтрибутМастерКласса01 = "АтрибутМастерКласса01", MasterRoot = masterRoot };
            var мастерКласс2 = new МастерКлассДлинноеИмя2 { MasterAttr2 = true, АтрибутМастерКласса01 = "АтрибутМастерКласса01", MasterRoot = masterRoot };
            var класс = new ДочернийКлассДлинноеИмя { MasterClass = мастерКласс01, МастерКлассДлинноеИмя01 = мастерКласс01, МастерКлассДлинноеИмя02 = мастерКласс2, Attr1 = "123", Attr2 = 55, Атрибут3 = true };

            ////var класс2 = new ДочернийКлассДлинноеИмя2 { MasterClass = мастерКласс, МастерКлассДлинноеИмя = мастерКласс, МастерКлассДлинноеИмя2 = мастерКласс2, Attr1 = "abc", Attr2 = 55, Атрибут3 = true };

            var objsToUpdate = new DataObject[] { мастерКласс01, класс, мастерКласс02, masterRoot };
            DataService.UpdateObjects(ref objsToUpdate, new DataObjectCache(), true);

            var languageDef = SQLWhereLanguageDef.LanguageDef;
            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(ДочернийКлассДлинноеИмя), ДочернийКлассДлинноеИмя.Views.TestView1);
            lcs.LimitFunction = languageDef.GetFunction(languageDef.funcEQ, new VariableDef(languageDef.GuidType, "МастерКлассДлинноеИмя01"), new Guid(мастерКласс01.__PrimaryKey.ToString()));
            var classes = DataService.LoadObjects(new[] { lcs });

            var clazz2 = new ДочернийКлассДлинноеИмя { __PrimaryKey = класс.__PrimaryKey };
            DataService.LoadObject(clazz2);
        }

        /// <summary>
        /// Выполняется тестирование наследования, использования master-класса и detail-класса при сохранении в БД.
        /// </summary>
        [Fact]
        public void InheritanceTest()
        {
            if (DataService == null)
            {
                return;
            }

            var masterRoot = new MasterRoot { MasterAttr = 234 };
            var master = new MasterClass { MasterAttr2 = true, MasterAttr1 = DateTime.Now, MasterRoot = masterRoot };
            var clazz = new MyClass { MasterClass = master, Attr1 = "abc", Attr2 = 1 };
            var derived = new Класс { MasterClass = master, Attr1 = "123", Attr2 = 55, Атрибут3 = true };
            var detail1 = new DetailClass { MyClass = clazz, DetailAttr = "def1" };
            var detail2 = new DetailClass { MyClass = clazz, DetailAttr = "def2" };
            var detailChild = new DetailClass2 { DetailClass = detail2, DetailAttr2 = "cbd" };
            var objsToUpdate = new DataObject[] { master, clazz, detail1, detail2, derived, masterRoot, detailChild };
            DataService.UpdateObjects(ref objsToUpdate, new DataObjectCache(), true);
            try
            {
                DataService.LoadObject(new MasterRoot { __PrimaryKey = masterRoot.__PrimaryKey });
            }
            catch (CantFindDataObjectException)
            {
                Assert.True(false, "Object not saved.");
            }

            try
            {
                DataService.LoadObject(new MasterClass { __PrimaryKey = master.__PrimaryKey });
            }
            catch (CantFindDataObjectException)
            {
                Assert.True(false, "Object not saved.");
            }

            try
            {
                DataService.LoadObject(new MyClass { __PrimaryKey = clazz.__PrimaryKey });
            }
            catch (CantFindDataObjectException)
            {
                Assert.True(false, "Object not saved.");
            }

            try
            {
                DataService.LoadObject(new Класс { __PrimaryKey = derived.__PrimaryKey });
            }
            catch (CantFindDataObjectException)
            {
                Assert.True(false, "Object not saved.");
            }

            try
            {
                DataService.LoadObject(new DetailClass { __PrimaryKey = detail1.__PrimaryKey });
            }
            catch (CantFindDataObjectException)
            {
                Assert.True(false, "Object not saved.");
            }

            try
            {
                DataService.LoadObject(new DetailClass { __PrimaryKey = detail2.__PrimaryKey });
            }
            catch (CantFindDataObjectException)
            {
                Assert.True(false, "Object not saved.");
            }

            try
            {
                DataService.LoadObject(new DetailClass2 { __PrimaryKey = detailChild.__PrimaryKey });
            }
            catch (CantFindDataObjectException)
            {
                Assert.True(false, "Object not saved.");
            }
        }
    }
}
