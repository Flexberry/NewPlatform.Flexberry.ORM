namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Xunit;

    public class BusinessServerOrderTest
    {
        [Fact]
        public void TestMethod1()
        {
            var lst = new List<OrderedCls>();

            lst.Add(new OrderedCls { Order = 1, Name = "A" });
            lst.Add(new OrderedCls { Order = 1, Name = "B" });
            lst.Add(new OrderedCls { Order = 2, Name = "C" });
            lst.Add(new OrderedCls { Order = 1, Name = "D" });
            lst.Add(new OrderedCls { Order = 1, Name = "E" });

            var usedOrderInd = new List<int>();
            var step = 0;
            foreach (var cls in lst)
            {
                if (usedOrderInd.Contains(cls.Order))
                {
                    step++;
                }
                else
                {
                    usedOrderInd.Add(cls.Order);
                }
                cls.Order += step;
            }

            lst.Sort(new OrderedClsComparerG());

            foreach (var cls in lst)
            {
                Console.WriteLine(cls.Name + cls.Order);
            }

            Console.WriteLine("");

            var bss = new ArrayList();

            bss.Add(new OrderedCls { Order = 1, Name = "A" });
            bss.Add(new OrderedCls { Order = 1, Name = "B" });
            bss.Add(new OrderedCls { Order = 2, Name = "C" });
            bss.Add(new OrderedCls { Order = 10, Name = "D" });
            bss.Add(new OrderedCls { Order = 2, Name = "E" });

            bss.Sort(new OrderedClsComparer());

            foreach (OrderedCls cls in bss)
            {
                Console.WriteLine(cls.Name);
            }

            Console.WriteLine("");

            var sl = new SortedList();

            sl.Add(1, new OrderedCls { Order = 1, Name = "A" });
            sl.Add(2, new OrderedCls { Order = 1, Name = "B" });
            sl.Add(3, new OrderedCls { Order = 2, Name = "C" });
            sl.Add(4, new OrderedCls { Order = 1, Name = "D" });
            sl.Add(5, new OrderedCls { Order = 1, Name = "E" });

            foreach (var cls in sl)
            {
                Console.WriteLine(((OrderedCls)((DictionaryEntry)cls).Value).Name);
            }
        }

        [Fact]
        public void TestMethod2()
        {
            Console.WriteLine("");

            var bss = new ArrayList();

            bss.Add(new OrderedCls { Order = 1, Name = "A" });
            bss.Add(new OrderedCls { Order = 1, Name = "B" });
            bss.Add(new OrderedCls { Order = 2, Name = "C" });
            bss.Add(new OrderedCls { Order = 10, Name = "D" });
            bss.Add(new OrderedCls { Order = 2, Name = "E" });

            bss.Sort(new OrderedClsComparer());

            foreach (OrderedCls cls in bss)
            {
                Console.WriteLine(cls.Name);
            }
        }

        [Fact]
        public void TestMethod3()
        {
            Console.WriteLine("");

            var bss = new ArrayList();

            var cls1 = new OrderedCls { Order = 1, Name = "A" };
            bss.Add(cls1);
            var cls2 = new OrderedCls { Order = 1, Name = "B" };
            bss.Add(cls2);
            var cls3 = new OrderedCls { Order = 2, Name = "C" };
            bss.Add(cls3);
            var cls4 = new OrderedCls { Order = 1, Name = "D" };
            bss.Add(cls4);
            var cls5 = new OrderedCls { Order = 1, Name = "E" };
            bss.Add(cls5);

            //bss.Sort(new OrderedClsComparer());
            var sortedArList = new ArrayList();
            var sl = new SortedList();
            foreach (OrderedCls cls in bss)
            {
                if (!sl.ContainsKey(cls.Order))
                {
                    sl.Add(cls.Order, new ArrayList());
                }
                ((ArrayList)sl[cls.Order]).Add(cls);
            }

            foreach (DictionaryEntry entry in sl)
            {
                var arl = (ArrayList)entry.Value;
                sortedArList.AddRange(arl);
            }
            bss = sortedArList;
            foreach (OrderedCls cls in bss)
            {
                Console.WriteLine(cls.Name);
            }
        }

        public class OrderedCls
        {
            public string Name;

            public int Order;
        }

        /// <summary>
        ///     Класс для сортировки бизнес-серверов
        /// </summary>
        public class OrderedClsComparer : IComparer
        {
            /// <summary>
            ///     Сравнить
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(object x, object y)
            {
                var compareTo = ((OrderedCls)x).Order.CompareTo(((OrderedCls)y).Order);
                return compareTo;
            }
        }

        /// <summary>
        ///     Класс для сортировки бизнес-серверов
        /// </summary>
        public class OrderedClsComparerG : IComparer<OrderedCls>
        {
            public int Compare(OrderedCls x, OrderedCls y)
            {
                var compareTo = x.Order.CompareTo(y.Order);
                return compareTo;
            }
        }
    }
}