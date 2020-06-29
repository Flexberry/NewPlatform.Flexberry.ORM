namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using Xunit;

    /// <summary>
    /// Тесты класса <see cref="BusinessServerProvider" />.
    /// <remarks>
    /// Подтверждение к https://flexberry.github.io/ru/fo_order-calls-bs.html#content
    /// </remarks>
    /// </summary>
    public class BusinessServerDefaultOrderTests
    {
        [Fact]
        public void GetBusinessServerOrderForClass3()
        {
            // Arrange.
            var type = typeof(Class3);

            // Act.
            var bss = BusinessServerProvider.GetBusinessServer(type, DataServiceObjectEvents.OnInsertToStorage, null);

            // Assert.
            Assert.Single(bss);
            Assert.Equal(typeof(Class3BS), bss[0].GetType());
        }

        [Fact]
        public void GetBusinessServerOrderForClass2()
        {
            // Arrange.
            var type = typeof(Class2);

            // Act.
            var bss = BusinessServerProvider.GetBusinessServer(type, DataServiceObjectEvents.OnInsertToStorage, null);

            // Assert.
            Assert.Equal(3, bss.Length);
            Assert.Equal(typeof(Class3BS), bss[0].GetType());
            Assert.Equal(typeof(Interface3BS), bss[1].GetType());
            Assert.Equal(typeof(Class2BS), bss[2].GetType());
        }

        [Fact]
        public void GetBusinessServerOrderForClass1()
        {
            // Arrange.
            var type = typeof(Class1);

            // Act.
            var bss = BusinessServerProvider.GetBusinessServer(type, DataServiceObjectEvents.OnInsertToStorage, null);

            // Assert.
            Assert.Equal(6, bss.Length);
            Assert.Equal(typeof(Class3BS), bss[0].GetType());
            Assert.Equal(typeof(Interface3BS), bss[1].GetType());
            Assert.Equal(typeof(Class2BS), bss[2].GetType());
            Assert.Equal(typeof(Interface2BS), bss[3].GetType());
            Assert.Equal(typeof(Interface1BS), bss[4].GetType());
            Assert.Equal(typeof(Class1BS), bss[5].GetType());
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

        [BusinessServer(typeof(Class3BS), DataServiceObjectEvents.OnAllEvents)]
        public class Class3 : DataObject { }

        [BusinessServer(typeof(Class2BS), DataServiceObjectEvents.OnAllEvents)]
        public class Class2 : Class3, Interface3 { }

        [BusinessServer(typeof(Class1BS), DataServiceObjectEvents.OnAllEvents)]
        public class Class1 : Class2, Interface1, Interface2 { }

        [BusinessServer(typeof(Interface3BS), DataServiceObjectEvents.OnAllEvents)]
        public interface Interface3 { }

        [BusinessServer(typeof(Interface2BS), DataServiceObjectEvents.OnAllEvents)]
        public interface Interface2 { }

        [BusinessServer(typeof(Interface1BS), DataServiceObjectEvents.OnAllEvents)]
        public interface Interface1 { }
    }
}
