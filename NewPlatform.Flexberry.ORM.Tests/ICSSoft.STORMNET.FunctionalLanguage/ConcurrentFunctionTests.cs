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

        /// <summary>
        /// Конкурентный тест сравнения функций.
        /// </summary>
        [Fact]
        public void GetFunctionConcurrentTest()
        {
            // Arrange.
            const int length = 100;
            var multiThreadingTestTool = new MultiThreadingTestTool(GetFunctionMultiThreadMethod);
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

        private static void GetFunctionMultiThreadMethod(object sender)
        {
            var parametersDictionary = sender as Dictionary<string, object>;
            ConcurrentDictionary<string, Exception> exceptions = parametersDictionary[MultiThreadingTestTool.ParamNameExceptions] as ConcurrentDictionary<string, Exception>;

            try
            {
                // Assert.
                Assert.Equal(
                    LangDef.GetFunction(LangDef.funcEQ, IntGenVarDef, 152),
                    FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, 152));
            }
            catch (Exception exception)
            {
                exceptions.TryAdd(Thread.CurrentThread.Name, exception);
                parametersDictionary[MultiThreadingTestTool.ParamNameWorking] = false;
            }
        }
    }
}
