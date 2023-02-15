namespace ICSSoft.STORMNET.Tests.TestClasses.DataObject
{
    using System;
    using ICSSoft.STORMNET.KeyGen;
    using Xunit;

    /// <summary>
    /// Тестовый класс для KeyGuid.
    /// </summary>
    public class KeyGuidTest
    {
        /// <summary>
        /// Проверка оператора "&lt;".
        /// </summary>
        [Fact]

        public void KeyGuidOperatorLessTest()
        {
            KeyGuid xNull = null;
            KeyGuid yNull = null;

            var result = xNull < yNull;
            Assert.True(false == result, "null < null");

            KeyGuid x = new KeyGuid("00000000-0000-0000-0000-000000000005");

            try
            {
                result = x < null;
                Assert.True(false, "notNull < null");
            }
            catch (ArgumentException)
            {
            }

            KeyGuid y = new KeyGuid("00000000-0000-0000-0000-000000000004");
            result = xNull < y;
            Assert.True(true == result, "null < notNull");

            result = x < y;
            Assert.True(false == result, "5 < 4");

            y = new KeyGuid("00000000-0000-0000-0000-000000000005");
            result = x < y;
            Assert.True(false == result, "5 < 5");

            y = new KeyGuid("00000000-0000-0000-0000-000000000006");
            result = x < y;
            Assert.True(true == result, "5 < 6");
        }

        /// <summary>
        /// Проверка оператора "&gt;".
        /// </summary>
        [Fact]

        public void KeyGuidOperatorMoreTest()
        {
            KeyGuid xNull = null;
            KeyGuid yNull = null;

            var result = xNull > yNull;
            Assert.True(false == result, "null > null");

            KeyGuid x = new KeyGuid("00000000-0000-0000-0000-000000000005");
            try
            {
                result = x > null;
                Assert.True(false, "notNull > null");
            }
            catch (ArgumentException)
            {
            }

            KeyGuid y = new KeyGuid("00000000-0000-0000-0000-000000000004");
            result = xNull > y;
            Assert.True(false == result, "null > notNull");

            result = x > y;
            Assert.True(true == result, "5 > 4");

            y = new KeyGuid("00000000-0000-0000-0000-000000000005");
            result = x > y;
            Assert.True(false == result, "5 > 5");

            y = new KeyGuid("00000000-0000-0000-0000-000000000006");
            result = x > y;
            Assert.True(false == result, "5 > 6");
        }

        /// <summary>
        /// Проверка оператора "&lt;=".
        /// </summary>
        [Fact]

        public void KeyGuidOperatorLessAndEqualTest()
        {
            KeyGuid xNull = null;
            KeyGuid yNull = null;

            var result = xNull <= yNull;
            Assert.True(true == result, "null <= null");

            KeyGuid x = new KeyGuid("00000000-0000-0000-0000-000000000005");
            try
            {
                result = x <= null;
                Assert.True(false, "notNull <= null");
            }
            catch (ArgumentException)
            {
            }

            KeyGuid y = new KeyGuid("00000000-0000-0000-0000-000000000004");
            result = xNull <= y;
            Assert.True(true == result, "null <= notNull");

            result = x <= y;
            Assert.True(false == result, "5 <= 4");

            y = new KeyGuid("00000000-0000-0000-0000-000000000005");
            result = x <= y;
            Assert.True(true == result, "5 <= 5");

            y = new KeyGuid("00000000-0000-0000-0000-000000000006");
            result = x <= y;
            Assert.True(true == result, "5 <= 6");
        }

        /// <summary>
        /// Проверка оператора "&gt;=".
        /// </summary>
        [Fact]

        public void KeyGuidOperatorMoreAndEqualTest()
        {
            KeyGuid xNull = null;
            KeyGuid yNull = null;

            var result = xNull >= yNull;
            Assert.True(true == result, "null >= null");

            KeyGuid x = new KeyGuid("00000000-0000-0000-0000-000000000005");
            try
            {
                result = x >= null;
                Assert.True(false, "notNull >= null");
            }
            catch (ArgumentException)
            {
            }

            KeyGuid y = new KeyGuid("00000000-0000-0000-0000-000000000004");
            result = null >= y;
            Assert.True(false == result, "null >= notNull");

            result = x >= y;
            Assert.True(true == result, "5 >= 4");

            y = new KeyGuid("00000000-0000-0000-0000-000000000005");
            result = x >= y;
            Assert.True(true == result, "5 >= 5");

            y = new KeyGuid("00000000-0000-0000-0000-000000000006");
            result = x >= y;
            Assert.True(false == result, "5 >= 6");
        }

        /// <summary>
        /// Проверка на гуидность.
        /// </summary>
        [Fact]

        public void KeyGuidIsGuidTest()
        {
            string guid = null;
            bool result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid);

            guid = string.Empty;
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid);

            guid = "  ";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid);

            guid = "00000000-0000-0000-0000-000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(true == result, guid);

            guid = "00000000-0000-0000-0000-00000000000f";
            result = KeyGuid.IsGuid(guid);
            Assert.True(true == result, guid);

            guid = "00000000-0000-0000-0000-00000000000x";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid);

            guid = "00000000-0000-0000-0000-00000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " -1 символ");

            guid = "00000000-0000-0000-0000-00000000000x";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " +1 символ");

            guid = "0000000-0000-0000-0000-000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " -1 символ");

            guid = "000000000-0000-0000-0000-000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " +1 символ");

            guid = "00000000-000-0000-0000-000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " -1 символ");

            guid = "00000000-00000-0000-0000-000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " +1 символ");

            guid = "00000000-0000-000-0000-000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " -1 символ");

            guid = "00000000-0000-00000-0000-000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " +1 символ");

            guid = "00000000-0000-0000-000-000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " -1 символ");

            guid = "00000000-0000-0000-00000-000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " +1 символ");

            guid = "0000000-00000-0000-0000-000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " смещение дефиса");

            guid = "00000000-000-00000-0000-000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " смещение дефиса");

            guid = "00000000-0000-000-00000-000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " смещение дефиса");

            guid = "00000000-0000-0000-000-0000000000000";
            result = KeyGuid.IsGuid(guid);
            Assert.True(false == result, guid + " смещение дефиса");
        }

        /// <summary>
        /// Проверка неявного преобразования из Guid в KeyGuid.
        /// </summary>
        [Fact]

        public void KeyGuidImplicitOperatorKeyGuidTest()
        {
            Guid? guid = null;
            KeyGuid keyGuid = guid;

            Assert.Null(keyGuid);

            guid = new Guid();
            keyGuid = guid;
            Assert.NotNull(keyGuid);
        }

        /// <summary>
        /// Проверка неявного преобразования из KeyGuid в Guid.
        /// </summary>
        [Fact]

        public void KeyGuidImplicitOperatorGuid()
        {
            KeyGuid keyGuid = null;
            Guid? guid = keyGuid;

            Assert.Null(guid);

            keyGuid = new KeyGuid();
            guid = keyGuid;
            Assert.NotNull(guid);
        }

        /// <summary>
        /// Проверка неявного преобразования из bytes[] в KeyGuid.
        /// </summary>
        [Fact]

        public void KeyGuidImplicitOperatorByteArrayTest()
        {
            Guid guid = new Guid("00000000-0000-0000-0000-000000000005");
            var bytes = guid.ToByteArray();

            KeyGuid keyGuid = bytes;
            Assert.NotNull(keyGuid);
        }

        /// <summary>
        /// Проверка неявного преобразования из KeyGuid в bytes[].
        /// </summary>
        [Fact]

        public void KeyGuidImplicitOperatorByteArray1Test()
        {
            KeyGuid keyGuid = new KeyGuid();
            byte[] bytes = keyGuid;

            Assert.NotNull(bytes);
            Assert.Equal(16, bytes.Length);
        }

        /// <summary>
        /// Проверка оператора "==".
        /// </summary>
        [Fact]

        public void KeyGuidOperatorEqualTest()
        {
            KeyGuid xNull = null;
            KeyGuid yNull = null;

            var result = xNull == yNull;
            Assert.True(true == result, "null == null");

            KeyGuid x = new KeyGuid("00000000-0000-0000-0000-000000000005");
            result = x == yNull;
            Assert.True(false == result, "notNull == null");

            KeyGuid y = new KeyGuid("00000000-0000-0000-0000-000000000004");
            result = xNull == y;
            Assert.True(false == result, "null == notNull");

            result = x == y;
            Assert.True(false == result, "5 == 4");

            y = new KeyGuid("00000000-0000-0000-0000-000000000005");
            result = x == y;
            Assert.True(true == result, "5 == 5");

            y = new KeyGuid("00000000-0000-0000-0000-000000000006");
            result = x == y;
            Assert.True(false == result, "5 == 6");
        }

        /// <summary>
        /// Проверка оператора "!=".
        /// </summary>
        [Fact]

        public void KeyGuidOperatorNotEqualTest()
        {
            KeyGuid xNull = null;
            KeyGuid yNull = null;

            var result = xNull != yNull;
            Assert.True(false == result, "null != null");

            KeyGuid x = new KeyGuid("00000000-0000-0000-0000-000000000005");
            result = x != yNull;
            Assert.True(true == result, "notNull != null");

            KeyGuid y = new KeyGuid("00000000-0000-0000-0000-000000000004");
            result = xNull != y;
            Assert.True(true == result, "null != notNull");

            result = x != y;
            Assert.True(true == result, "5 != 4");

            y = new KeyGuid("00000000-0000-0000-0000-000000000005");
            result = x != y;
            Assert.True(false == result, "5 != 5");

            y = new KeyGuid("00000000-0000-0000-0000-000000000006");
            result = x != y;
            Assert.True(true == result, "5 != 6");
        }
    }
}
