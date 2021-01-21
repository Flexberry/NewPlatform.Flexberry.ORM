namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;

    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Тесты конкурентного получения функций ограничения.
    /// </summary>
    public class ConcurrentFunctionTests
    {
        protected static readonly ExternalLangDef LangDef = ExternalLangDef.LanguageDef;

        protected static readonly VariableDef IntGenVarDef = new VariableDef(LangDef.NumericType, Information.ExtractPropertyPath<TestDataObject>(x => x.Height));

        private readonly ITestOutputHelper Output;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ConcurrentFunctionTests(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact]
        public void GetFunctionDefConcurrentTest()
        {
            // Arrange.
            const int length = 100;
            var multiThreadingTestTool = new MultiThreadingTestTool(MultiThreadMethod);
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

        private static void MultiThreadMethod(object sender)
        {
            var parametersDictionary = sender as Dictionary<string, object>;
            ConcurrentDictionary<string, Exception> exceptions = parametersDictionary[MultiThreadingTestTool.ParamNameExceptions] as ConcurrentDictionary<string, Exception>;

            try
            {
                // Assert.
                Assert.Equal(
                    LangDef.GetFunction(LangDef.funcIsNull, IntGenVarDef),
                    FunctionBuilder.BuildIsNull<TestDataObject>(x => x.Height));
            }
            catch (Exception exception)
            {
                exceptions.TryAdd(Thread.CurrentThread.Name, exception);
                parametersDictionary[MultiThreadingTestTool.ParamNameWorking] = false;
            }
        }
    }
}
