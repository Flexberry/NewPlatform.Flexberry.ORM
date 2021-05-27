namespace ICSSoft.STORMNET.Tests.TestClasses.DataObject
{
    using System.Diagnostics;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Тест для проверки <see cref="ObjectCreator"/>.
    /// </summary>
    public class ObjectCreatorTest
    {
        /// <summary>
        /// Метод для проверки функции по созданию новых объектов данных по указанному типу <see cref="ObjectCreator.CreateObject"/>.
        /// </summary>
        [Fact]
        public void CreateObjectTest()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var objectCreator1 = new ObjectCreator();
            object obj1 = objectCreator1.CreateObject(typeof(Страна));
            stopwatch.Stop();
            Debug.WriteLine(stopwatch.ElapsedTicks + "ticks without cache");
            Assert.True(obj1 is Страна);
            stopwatch.Restart();
            var objectCreator2 = new ObjectCreator();
            object obj2 = objectCreator2.CreateObject(typeof(Страна));
            stopwatch.Stop();
            Debug.WriteLine(stopwatch.ElapsedTicks + "ticks with cache");

            Assert.True(obj2 is Страна);
            int key = typeof(Страна).GetHashCode();
            Assert.True(ObjectCreator.CacheInstantiateObjectHandler.ContainsKey(key));
        }
    }
}
