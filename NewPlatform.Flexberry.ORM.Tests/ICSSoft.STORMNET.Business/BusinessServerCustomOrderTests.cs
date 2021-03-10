﻿namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using Xunit;

    /// <summary>
    /// Тесты класса <see cref="BusinessServerProvider" /> для проверки порядка выполнения бизнес-серверов, если их порядок задается разработчиком. 
    /// </summary>
    public class BusinessServerCustomOrderTests
    {
        /// <summary>
        /// Проверка установленного разработчиком порядка выполнения бизнес-серверов для класса Class1.
        /// </summary>
        [Fact]
        public void GetBusinessServerOrderForClass1()
        {
            // Arrange.
            var type = typeof(Class1);

            // Act.
            BusinessServer[] bss = BusinessServerProvider.GetBusinessServer(type, DataServiceObjectEvents.OnInsertToStorage, null);

            // Assert.
            Assert.Equal(6, bss.Length);
            Assert.Equal(typeof(Interface1BS), bss[0].GetType());
            Assert.Equal(nameof(Interface1BS.OnUpdateInterface1), bss[0].Method.Name);
            Assert.Equal(typeof(Interface2BS), bss[1].GetType());
            Assert.Equal(nameof(Interface2BS.OnUpdateInterface2), bss[1].Method.Name);
            Assert.Equal(typeof(Interface3BS), bss[2].GetType());
            Assert.Equal(nameof(Interface3BS.OnUpdateInterface3), bss[2].Method.Name);
            Assert.Equal(typeof(Class1BS), bss[3].GetType());
            Assert.Equal(nameof(Class1BS.OnUpdateClass1), bss[3].Method.Name);
            Assert.Equal(typeof(Class2BS), bss[4].GetType());
            Assert.Equal(nameof(Class2BS.OnUpdateClass2), bss[4].Method.Name);
            Assert.Equal(typeof(Class3BS), bss[5].GetType());
            Assert.Equal(nameof(Class3BS.OnUpdateClass3), bss[5].Method.Name);
        }

        public class Class3BS : BusinessServer
        {
            public virtual DataObject[] OnUpdateClass3(Class3 UpdatedObject)
            {
                return new DataObject[0];
            }
        }

        public class Class2BS : BusinessServer
        {
            public virtual DataObject[] OnUpdateClass2(Class2 UpdatedObject)
            {
                return new DataObject[0];
            }
        }

        public class Class1BS : BusinessServer
        {
            public virtual DataObject[] OnUpdateClass1(Class1 UpdatedObject)
            {
                return new DataObject[0];
            }
        }

        public class Interface3BS : BusinessServer
        {
            public virtual DataObject[] OnUpdateInterface3(Interface3 UpdatedObject)
            {
                return new DataObject[0];
            }
        }

        public class Interface2BS : BusinessServer
        {
            public virtual DataObject[] OnUpdateInterface2(Interface2 UpdatedObject)
            {
                return new DataObject[0];
            }
        }

        public class Interface1BS : BusinessServer
        {
            public virtual DataObject[] OnUpdateInterface1(Interface1 UpdatedObject)
            {
                return new DataObject[0];
            }
        }

        [BusinessServer(typeof(Class3BS), DataServiceObjectEvents.OnAllEvents, 6)]
        public class Class3 : DataObject
        {
        }

        [BusinessServer(typeof(Class2BS), DataServiceObjectEvents.OnAllEvents, 5)]
        public class Class2 : Class3, Interface3
        {
        }

        [BusinessServer(typeof(Class1BS), DataServiceObjectEvents.OnAllEvents, 4)]
        public class Class1 : Class2, Interface1, Interface2
        {
        }

        [BusinessServer(typeof(Interface3BS), DataServiceObjectEvents.OnAllEvents, 3)]
        public interface Interface3
        {
        }

        [BusinessServer(typeof(Interface2BS), DataServiceObjectEvents.OnAllEvents, 2)]
        public interface Interface2
        {
        }

        [BusinessServer(typeof(Interface1BS), DataServiceObjectEvents.OnAllEvents, 1)]
        public interface Interface1
        {
        }
    }
}
