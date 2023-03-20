namespace ICSSoft.STORMNET.Tests.TestClasses.Common
{
    using System;
    using System.Collections.Generic;

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
            string str = "";
            foreach (string s in GetArray())
            {
                str += (string.IsNullOrEmpty(str) ? "" : ",") + s;
            }

            Console.WriteLine(str);
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
            string str = "";
            ForeachClass foreachClass = new ForeachClass();
            foreach (string s in foreachClass.Array)
            {
                str += (string.IsNullOrEmpty(str) ? "" : ",") + s;
            }

            Console.WriteLine(str);
        }

        [Fact]
        public void ForeachWithPropAndMethodTesting()
        {
            string str = "";
            foreach (string s in (new ForeachClass1()).GetFC().Array)
            {
                str += (string.IsNullOrEmpty(str) ? "" : ",") + s;
            }

            Console.WriteLine(str);
        }

        [Fact]
        public void ForWithPropAndMethodTesting()
        {
            string str = "";
            for (int i = 0; i < (new ForeachClass1()).GetFC().Array.Length; i++)
            {
                str += (string.IsNullOrEmpty(str) ? "" : ",") + "(new ForeachClass1()).GetFC().Array[i]";
            }

            Console.WriteLine(str);
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
