namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.KeyGen;

    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Тесты для класса <see cref="KeyGenerator"/>.
    /// </summary>
    public class KeyGeneratorTests
    {
        private readonly ITestOutputHelper Output;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="output">Поток вывода теста.</param>
        public KeyGeneratorTests(ITestOutputHelper output)
        {
            Output = output;
        }

        /// <summary>
        /// Конкурентный тест получения типа ключа <see cref="KeyGenerator.KeyType(DataObject)" />.
        /// </summary>
        /// <remarks>Падает крайне редко.</remarks>
        [Fact]
        public void KeyTypeConcurrentTest()
        {
            // Arrange.
            const int length = 100;
            var multiThreadingTestTool = new MultiThreadingTestTool(KeyTypeMultiThreadMethod);
            multiThreadingTestTool.StartThreads(length);

            // Assert.
            var exception = multiThreadingTestTool.GetExceptions();
            if (exception != null)
            {
                foreach (var item in exception.InnerExceptions)
                {
                    Output.WriteLine($"Thread {item.Key}: {item.Value}");
                }

                // Пусть так.
                Assert.Empty(exception.InnerExceptions);
            }
        }

        private void KeyTypeMultiThreadMethod(object sender)
        {
            var parametersDictionary = sender as Dictionary<string, object>;
            ConcurrentDictionary<string, Exception> exceptions = parametersDictionary[MultiThreadingTestTool.ParamNameExceptions] as ConcurrentDictionary<string, Exception>;

            try
            {
                // Assert.
                var dobjType = typeof(Медведь);
                var keyType = KeyGenerator.KeyType(dobjType);
                Assert.Equal(typeof(KeyGuid), keyType);
            }
            catch (Exception exception)
            {
                exceptions.TryAdd(Thread.CurrentThread.Name, exception);
                parametersDictionary[MultiThreadingTestTool.ParamNameWorking] = false;
            }
        }
    }
}
