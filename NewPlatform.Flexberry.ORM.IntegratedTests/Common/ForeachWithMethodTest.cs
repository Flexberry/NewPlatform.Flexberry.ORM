namespace ICSSoft.STORMNET.Tests.TestClasses.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Xunit;

    /// <summary>
    /// Summary description for ForeachWithMethodTest.
    /// </summary>
    public class ForeachWithMethodTest
    {
        public ForeachWithMethodTest()
        {
            // TODO: Add constructor logic here
        }

        [Fact]
        public void ForeachWithMethodTesting()
        {
            string str = string.Empty;
            foreach (string s in GetArray())
            {
                str += (string.IsNullOrEmpty(str) ? string.Empty : ",") + s;
            }

            Console.WriteLine(str);
            Assert.True(str != string.Empty);
        }

        private int count = 0;

        Random rnd = new Random();

        private string[] GetArray()
        {
            List<string> lst = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                lst.Add(rnd.Next(10).ToString());
            }

            Console.WriteLine(string.Join(",", lst.ToArray()));
            return lst.ToArray();
        }

        [Fact]
        public void ForeachWithPropertyTesting()
        {
            string str = string.Empty;
            ForeachClass foreachClass = new ForeachClass();
            foreach (string s in foreachClass.Array)
            {
                str += (string.IsNullOrEmpty(str) ? string.Empty : ",") + s;
            }

            Console.WriteLine(str);
            Assert.True(str != string.Empty);
        }

        [Fact]
        public void ForeachWithPropAndMethodTesting()
        {
            string str = string.Empty;
            foreach (string s in new ForeachClass1().GetFC().Array)
            {
                str += (string.IsNullOrEmpty(str) ? string.Empty : ",") + s;
            }

            Console.WriteLine(str);
            Assert.True(str != string.Empty);
        }

        [Fact]
        public void ForWithPropAndMethodTesting()
        {
            string str = string.Empty;
            for (int i = 0; i < new ForeachClass1().GetFC().Array.Length; i++)
            {
                str += (string.IsNullOrEmpty(str) ? string.Empty : ",") + "(new ForeachClass1()).GetFC().Array[i]";
            }

            Console.WriteLine(str);
            Assert.NotEqual(str, string.Empty);
        }

        private class ForeachClass1
        {
            public ForeachClass GetFC()
            {
                Console.WriteLine("GetFC");
                return new ForeachClass();
            }
        }

        private class ForeachClass
        {
            public Random rnd = new Random(new Random().Next());

            public string[] Array
            {
                get
                {
                    List<string> lst = new List<string>();
                    for (int i = 0; i < 10; i++)
                    {
                        lst.Add(rnd.Next(10).ToString());
                    }

                    Console.WriteLine(string.Join(",", lst.ToArray()));
                    return lst.ToArray();
                }
            }

            public int count = 0;
        }
    }
}
