namespace IIS.University.PKHelper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestObjects.DataObject;

    [TestClass]
    public class PerformanceTests
    {
        private static double[] Measure(int reps, Action action)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // warm up.
            action();

            var results = new double[reps];
            for (int i = 0; i < reps; i++)
            {
                var sw = Stopwatch.StartNew();
                action();
                results[i] = sw.Elapsed.TotalMilliseconds;
            }

            return results;
        }

        private static List<double[]> MeasureMethods(int reps, int its, params Action[] actions)
        {
            return actions.Select(
                    x => Measure(
                        reps,
                        () =>
                        {
                            for (int i = 0; i < its; i++)
                            {
                                x();
                            }
                        }))
                .ToList();
        }

        private static T NewTrivial<T>() where T : new()
        {
            return new T();
        }

        private static T NewReflectGen<T>() where T : new()
        {
            return Activator.CreateInstance<T>();
        }

        private static T NewReflect<T>() where T : new()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

        private static void AllocTypeTest<T>(int reps, int its) where T : new()
        {
            var testMethods = new Action[]
            {
                () =>
                {
                    var obj = NewTrivial<T>();
                },
                () =>
                {
                    var obj = NewReflectGen<T>();
                },
                () =>
                {
                    var obj = NewReflect<T>();
                },
                () =>
                {
                    var obj = FastAllocator<T>.New();
                }
            };
            var res = MeasureMethods(reps, its, testMethods);

            var res0 = res.Last();

            Assert.IsTrue(
                res.All(x => x.Average() >= res0.Average()),
                string.Join(", ", res.Select(x => x.Average())));
        }

        [TestMethod]
        public void PerformanceTest0()
        {
            const int reps = 100;
            const int its = 10000;
            AllocTypeTest<DataObjectForTest>(reps, its);
            AllocTypeTest<DataObjectWithKeyGuid>(reps, its);
            AllocTypeTest<SimpleDataObject>(reps, its);
        }
    }
}
