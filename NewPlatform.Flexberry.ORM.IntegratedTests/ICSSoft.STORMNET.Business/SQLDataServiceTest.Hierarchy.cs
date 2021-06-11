namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    /// <summary>
    /// Тесты иерархической вставки данных.
    /// </summary>
    public partial class SQLDataServiceTest
    {
                /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Проверяем, что нет лишних Update-запросов в БД, когда объект зависимости пустой.
        /// </summary>
        [Fact]
        public void InsertHierarchyEmptyTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                if (dataService is SQLDataService sqlDataService)
                {
                    sqlDataService.OnCreateCommand += (sender, e) =>
                    {
                        if (e.Command.CommandText.StartsWith("UPDATE"))
                        {
                            throw new Exception("Unnecessary update");
                        }
                    };
                }

                var master = new Медведь { ПорядковыйНомер = 1 };

                // Act.
                dataService.UpdateObject(master);

                var loadedMaster = PKHelper.CreateDataObject<Медведь>(master);
                dataService.LoadObject(loadedMaster);

                // Assert.
                Assert.Equal(1, loadedMaster.ПорядковыйНомер);
                Assert.Null(loadedMaster.Папа);
                Assert.Null(loadedMaster.Мама);
                Assert.Null(loadedMaster.Друг);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Проверяем, что нет лишних Update-запросов в БД.
        /// В случае, когда объект зависимоcти и основной объект создаются в одной транзакции и неправильном порядке.
        /// </summary>
        [Fact]
        public void InsertHierarchyFilledTest1()
        {
            foreach (IDataService dataService in DataServices)
            {
                var parent = new Медведь { ПорядковыйНомер = 1 };
                var child = new Медведь { ПорядковыйНомер = 2, Папа = parent };

                // Act.
                dataService.UpdateObject(child);

                var loadedParent = PKHelper.CreateDataObject<Медведь>(parent);
                dataService.LoadObject(loadedParent);

                var loadedChild = PKHelper.CreateDataObject<Медведь>(child);
                dataService.LoadObject(loadedChild);

                // Assert.
                Assert.Equal(1, loadedParent.ПорядковыйНомер);
                Assert.Null(loadedParent.Папа);
                Assert.Null(loadedParent.Мама);
                Assert.Null(loadedParent.Друг);
                Assert.Equal(2, loadedChild.ПорядковыйНомер);
                Assert.NotNull(loadedChild.Папа);
                Assert.Null(loadedChild.Мама);
                Assert.Null(loadedChild.Друг);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Проверяем, что нет лишних Update-запросов в БД.
        /// В случае, когда объект зависимоcти и основной объект создаются в одной транзакции и неправильном порядке.
        /// </summary>
        [Fact]
        public void InsertHierarchyFilledTest2()
        {
            foreach (IDataService dataService in DataServices)
            {
                var motherBear = new Медведь { ПорядковыйНомер = 1 };
                dataService.UpdateObject(motherBear);

                var fatherBear = new Медведь { ПорядковыйНомер = 2 };
                var child = new Медведь { ПорядковыйНомер = 3, Папа = fatherBear, Мама = motherBear };

                // Act.
                dataService.UpdateObject(child);

                var loadedFather = PKHelper.CreateDataObject<Медведь>(fatherBear);
                dataService.LoadObject(loadedFather);

                var loadedChild = PKHelper.CreateDataObject<Медведь>(child);
                dataService.LoadObject(loadedChild);

                // Assert.
                Assert.Equal(2, loadedFather.ПорядковыйНомер);
                Assert.Null(loadedFather.Папа);
                Assert.Null(loadedFather.Мама);
                Assert.Null(loadedFather.Друг);
                Assert.Equal(3, loadedChild.ПорядковыйНомер);
                Assert.NotNull(loadedChild.Папа);
                Assert.NotNull(loadedChild.Мама);
                Assert.Null(loadedChild.Друг);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Проверяем, что нет лишних Update-запросов в БД.
        /// В случае, когда объект зависимоcти и основной объект создаются в одной транзакции в правильном порядке.
        /// </summary>
        [Fact]
        public void InsertHierarchyFilledTest3()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                if (dataService is SQLDataService sqlDataService)
                {
                    sqlDataService.OnCreateCommand += (sender, e) =>
                    {
                        if (e.Command.CommandText.StartsWith("UPDATE"))
                        {
                            throw new Exception("Unnecessary update");
                        }
                    };
                }

                var father = new Медведь { ПорядковыйНомер = 1 };

                var child1 = new Медведь { ПорядковыйНомер = 2, Папа = father };
                var child2 = new Медведь { ПорядковыйНомер = 3, Папа = father };
                var child3 = new Медведь { ПорядковыйНомер = 4, Папа = child2 };

                DataObject[] objsToUpdate = new DataObject[] { father, child1, child2, child3 };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                var loadedFather = PKHelper.CreateDataObject<Медведь>(father);
                dataService.LoadObject(loadedFather);

                var loadedChild1 = PKHelper.CreateDataObject<Медведь>(child1);
                dataService.LoadObject(loadedChild1);

                var loadedChild2 = PKHelper.CreateDataObject<Медведь>(child2);
                dataService.LoadObject(loadedChild2);

                var loadedChild3 = PKHelper.CreateDataObject<Медведь>(child3);
                dataService.LoadObject(loadedChild3);

                // Assert.
                Assert.Equal(1, loadedFather.ПорядковыйНомер);
                Assert.Null(loadedFather.Папа);
                Assert.Null(loadedFather.Мама);
                Assert.Null(loadedFather.Друг);
                Assert.Equal(2, loadedChild1.ПорядковыйНомер);
                Assert.NotNull(loadedChild1.Папа);
                Assert.Null(loadedChild1.Мама);
                Assert.Null(loadedChild1.Друг);
                Assert.Equal(3, loadedChild2.ПорядковыйНомер);
                Assert.NotNull(loadedChild2.Папа);
                Assert.Null(loadedChild2.Мама);
                Assert.Null(loadedChild2.Друг);
                Assert.Equal(4, loadedChild3.ПорядковыйНомер);
                Assert.NotNull(loadedChild3.Папа);
                Assert.Null(loadedChild3.Мама);
                Assert.Null(loadedChild3.Друг);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Проверяем, что нет лишних Update-запросов в БД, в случае, когда объект зависимости предварительно есть в БД.
        /// </summary>
        [Fact]
        public void InsertHierarchyFilledTest4()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                if (dataService is SQLDataService sqlDataService)
                {
                    sqlDataService.OnCreateCommand += (sender, e) =>
                    {
                        if (e.Command.CommandText.StartsWith("UPDATE"))
                        {
                            throw new Exception("Unnecessary update");
                        }
                    };
                }

                var parent = new Медведь { ПорядковыйНомер = 1 };
                dataService.UpdateObject(parent);

                var child = new Медведь { ПорядковыйНомер = 2, Папа = parent };

                // Act.
                dataService.UpdateObject(child);

                var loadedChild = PKHelper.CreateDataObject<Медведь>(child);
                dataService.LoadObject(loadedChild);

                // Assert.
                Assert.Equal(2, loadedChild.ПорядковыйНомер);
                Assert.NotNull(loadedChild.Папа);
                Assert.Null(loadedChild.Мама);
                Assert.Null(loadedChild.Друг);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Проверяем, что нет лишних Update-запросов в БД.
        /// В случае, когда объект зависимости уже есть в БД и получается через SetExistObjectPrimaryKey.
        /// </summary>
        [Fact]
        public void InsertHierarchyFilledTest5()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                if (dataService is SQLDataService sqlDataService)
                {
                    sqlDataService.OnCreateCommand += (sender, e) =>
                    {
                        if (e.Command.CommandText.StartsWith("UPDATE"))
                        {
                            throw new Exception("Unnecessary update");
                        }
                    };
                }

                var parentOriginal = new Медведь { ПорядковыйНомер = 1 };
                dataService.UpdateObject(parentOriginal);

                var parent = new Медведь();
                parent.SetExistObjectPrimaryKey(parentOriginal.__PrimaryKey);
                parent.InitDataCopy();

                var child = new Медведь { ПорядковыйНомер = 2, Папа = parent };

                // Act.
                dataService.UpdateObject(child);

                var loadedChild = PKHelper.CreateDataObject<Медведь>(child);
                dataService.LoadObject(loadedChild);

                // Assert.
                Assert.Equal(2, loadedChild.ПорядковыйНомер);
                Assert.NotNull(loadedChild.Папа);
                Assert.Null(loadedChild.Мама);
                Assert.Null(loadedChild.Друг);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Проверяем, что нет лишних Update-запросов в БД.
        /// В случае, когда объект зависимоcти и основной объект создаются в одной транзакции в правильном порядке.
        /// </summary>
        [Fact]
        public void InsertHierarchyFilledTest6()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                if (dataService is SQLDataService sqlDataService)
                {
                    sqlDataService.OnCreateCommand += (sender, e) =>
                    {
                        if (e.Command.CommandText.StartsWith("UPDATE"))
                        {
                            throw new Exception("Unnecessary update");
                        }
                    };
                }

                var father = new Медведь { ПорядковыйНомер = 1 };
                dataService.UpdateObject(father);

                var child1 = new Медведь { ПорядковыйНомер = 2, Папа = father };
                var child2 = new Медведь { ПорядковыйНомер = 3, Папа = father };
                var child3 = new Медведь { ПорядковыйНомер = 4, Папа = child2 };

                DataObject[] objsToUpdate = new DataObject[] { child1, child2, child3 };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                var loadedFather = PKHelper.CreateDataObject<Медведь>(father);
                dataService.LoadObject(loadedFather);

                var loadedChild1 = PKHelper.CreateDataObject<Медведь>(child1);
                dataService.LoadObject(loadedChild1);

                var loadedChild2 = PKHelper.CreateDataObject<Медведь>(child2);
                dataService.LoadObject(loadedChild2);

                var loadedChild3 = PKHelper.CreateDataObject<Медведь>(child3);
                dataService.LoadObject(loadedChild3);

                // Assert.
                Assert.Equal(1, loadedFather.ПорядковыйНомер);
                Assert.Null(loadedFather.Папа);
                Assert.Null(loadedFather.Мама);
                Assert.Null(loadedFather.Друг);
                Assert.Equal(2, loadedChild1.ПорядковыйНомер);
                Assert.NotNull(loadedChild1.Папа);
                Assert.Null(loadedChild1.Мама);
                Assert.Null(loadedChild1.Друг);
                Assert.Equal(3, loadedChild2.ПорядковыйНомер);
                Assert.NotNull(loadedChild2.Папа);
                Assert.Null(loadedChild2.Мама);
                Assert.Null(loadedChild2.Друг);
                Assert.Equal(4, loadedChild3.ПорядковыйНомер);
                Assert.NotNull(loadedChild3.Папа);
                Assert.Null(loadedChild3.Мама);
                Assert.Null(loadedChild3.Друг);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Множественные объекты с циклической вложенностью. В этом случае должно разрешаться через доп. команду UPDATE.
        /// </summary>
        [Fact]
        public void InsertHierarchyCycleTest1()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var parent = new Медведь { ПорядковыйНомер = 1 };
                var child1 = new Медведь { ПорядковыйНомер = 2, Папа = parent };
                var child2 = new Медведь { ПорядковыйНомер = 3, Папа = child1 };
                var child3 = new Медведь { ПорядковыйНомер = 4, Папа = child2 };
                var child4 = new Медведь { ПорядковыйНомер = 5, Папа = child3 };

                child1.Мама = child4;

                // Act.
                dataService.UpdateObject(child4);

                var loadedParent = PKHelper.CreateDataObject<Медведь>(parent);
                dataService.LoadObject(loadedParent);

                var loadedChild1 = PKHelper.CreateDataObject<Медведь>(child1);
                dataService.LoadObject(loadedChild1);

                var loadedChild2 = PKHelper.CreateDataObject<Медведь>(child2);
                dataService.LoadObject(loadedChild2);

                var loadedChild3 = PKHelper.CreateDataObject<Медведь>(child3);
                dataService.LoadObject(loadedChild3);

                var loadedChild4 = PKHelper.CreateDataObject<Медведь>(child4);
                dataService.LoadObject(loadedChild4);

                // Assert.
                Assert.Equal(1, loadedParent.ПорядковыйНомер);
                Assert.Null(loadedParent.Папа);
                Assert.Null(loadedParent.Мама);
                Assert.Null(loadedParent.Друг);
                Assert.Equal(2, loadedChild1.ПорядковыйНомер);
                Assert.NotNull(loadedChild1.Папа);
                Assert.NotNull(loadedChild1.Мама);
                Assert.Null(loadedChild1.Друг);
                Assert.Equal(3, loadedChild2.ПорядковыйНомер);
                Assert.NotNull(loadedChild2.Папа);
                Assert.Null(loadedChild2.Мама);
                Assert.Null(loadedChild2.Друг);
                Assert.Equal(4, loadedChild3.ПорядковыйНомер);
                Assert.NotNull(loadedChild3.Папа);
                Assert.Null(loadedChild3.Мама);
                Assert.Null(loadedChild3.Друг);
                Assert.Equal(5, loadedChild4.ПорядковыйНомер);
                Assert.NotNull(loadedChild4.Папа);
                Assert.Null(loadedChild4.Мама);
                Assert.Null(loadedChild4.Друг);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Множественные объекты с циклической вложенностью, когда все объекты в одной транзакции. В этом случае должно разрешаться через доп. команду UPDATE.
        /// </summary>
        [Fact]
        public void InsertHierarchyCycleTest2()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var parent = new Медведь { ПорядковыйНомер = 1 };
                var child1 = new Медведь { ПорядковыйНомер = 2, Папа = parent };
                var child2 = new Медведь { ПорядковыйНомер = 3, Папа = child1 };
                var child3 = new Медведь { ПорядковыйНомер = 4, Папа = child2 };
                var child4 = new Медведь { ПорядковыйНомер = 5, Папа = child3 };

                parent.Папа = child1;

                // Act.
                dataService.UpdateObject(child4);

                var loadedParent = PKHelper.CreateDataObject<Медведь>(parent);
                dataService.LoadObject(loadedParent);

                var loadedChild1 = PKHelper.CreateDataObject<Медведь>(child1);
                dataService.LoadObject(loadedChild1);

                var loadedChild2 = PKHelper.CreateDataObject<Медведь>(child2);
                dataService.LoadObject(loadedChild2);

                var loadedChild3 = PKHelper.CreateDataObject<Медведь>(child3);
                dataService.LoadObject(loadedChild3);

                var loadedChild4 = PKHelper.CreateDataObject<Медведь>(child4);
                dataService.LoadObject(loadedChild4);

                // Assert.
                Assert.Equal(1, loadedParent.ПорядковыйНомер);
                Assert.NotNull(loadedParent.Папа);
                Assert.Null(loadedParent.Мама);
                Assert.Null(loadedParent.Друг);
                Assert.Equal(2, loadedChild1.ПорядковыйНомер);
                Assert.NotNull(loadedChild1.Папа);
                Assert.Null(loadedChild1.Мама);
                Assert.Null(loadedChild1.Друг);
                Assert.Equal(3, loadedChild2.ПорядковыйНомер);
                Assert.NotNull(loadedChild2.Папа);
                Assert.Null(loadedChild2.Мама);
                Assert.Null(loadedChild2.Друг);
                Assert.Equal(4, loadedChild3.ПорядковыйНомер);
                Assert.NotNull(loadedChild3.Папа);
                Assert.Null(loadedChild3.Мама);
                Assert.Null(loadedChild3.Друг);
                Assert.Equal(5, loadedChild4.ПорядковыйНомер);
                Assert.NotNull(loadedChild4.Папа);
                Assert.Null(loadedChild4.Мама);
                Assert.Null(loadedChild4.Друг);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Множественные объекты с циклической вложенностью, когда все объекты в одной транзакции. В этом случае должно разрешаться через доп. команду UPDATE.
        /// </summary>
        [Fact]
        public void InsertHierarchyCycleTest3()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var father = new Медведь { ПорядковыйНомер = 1 };
                var child1 = new Медведь { ПорядковыйНомер = 2, Папа = father };
                var child2 = new Медведь { ПорядковыйНомер = 3, Папа = child1 };
                father.Папа = child2;

                DataObject[] objsToUpdate = new DataObject[] { father, child1, child2 };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                var loadedFather = PKHelper.CreateDataObject<Медведь>(father);
                dataService.LoadObject(loadedFather);

                var loadedChild1 = PKHelper.CreateDataObject<Медведь>(child1);
                dataService.LoadObject(loadedChild1);

                var loadedChild2 = PKHelper.CreateDataObject<Медведь>(child2);
                dataService.LoadObject(loadedChild2);

                // Assert.
                Assert.Equal(1, loadedFather.ПорядковыйНомер);
                Assert.NotNull(loadedFather.Папа);
                Assert.Null(loadedFather.Мама);
                Assert.Null(loadedFather.Друг);
                Assert.Equal(2, loadedChild1.ПорядковыйНомер);
                Assert.NotNull(loadedChild1.Папа);
                Assert.Null(loadedChild1.Мама);
                Assert.Null(loadedChild1.Друг);
                Assert.Equal(3, loadedChild2.ПорядковыйНомер);
                Assert.NotNull(loadedChild2.Папа);
                Assert.Null(loadedChild2.Мама);
                Assert.Null(loadedChild2.Друг);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Множественные объекты с циклической вложенностью, когда все объекты в одной транзакции. В этом случае должно разрешаться через доп. команду UPDATE.
        /// </summary>
        [Fact]
        public void InsertHierarchyCycleTest4()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var father = new Медведь { ПорядковыйНомер = 1 };
                var child1 = new Медведь { ПорядковыйНомер = 2, Папа = father };
                var child2 = new Медведь { ПорядковыйНомер = 3 };
                var child3 = new Медведь { ПорядковыйНомер = 4, Папа = child2 };
                var child4 = new Медведь { ПорядковыйНомер = 5, Папа = child3 };

                child2.Папа = child4;

                DataObject[] objsToUpdate = new DataObject[] { father, child1, child2, child3, child4 };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                var loadedFather = PKHelper.CreateDataObject<Медведь>(father);
                dataService.LoadObject(loadedFather);

                var loadedChild1 = PKHelper.CreateDataObject<Медведь>(child1);
                dataService.LoadObject(loadedChild1);

                var loadedChild2 = PKHelper.CreateDataObject<Медведь>(child2);
                dataService.LoadObject(loadedChild2);

                var loadedChild3 = PKHelper.CreateDataObject<Медведь>(child3);
                dataService.LoadObject(loadedChild3);

                var loadedChild4 = PKHelper.CreateDataObject<Медведь>(child4);
                dataService.LoadObject(loadedChild4);

                // Assert.
                Assert.Equal(1, loadedFather.ПорядковыйНомер);
                Assert.Null(loadedFather.Папа);
                Assert.Null(loadedFather.Мама);
                Assert.Null(loadedFather.Друг);
                Assert.Equal(2, loadedChild1.ПорядковыйНомер);
                Assert.NotNull(loadedChild1.Папа);
                Assert.Null(loadedChild1.Мама);
                Assert.Null(loadedChild1.Друг);
                Assert.Equal(3, loadedChild2.ПорядковыйНомер);
                Assert.NotNull(loadedChild2.Папа);
                Assert.Null(loadedChild2.Мама);
                Assert.Null(loadedChild2.Друг);
                Assert.Equal(4, loadedChild3.ПорядковыйНомер);
                Assert.NotNull(loadedChild3.Папа);
                Assert.Null(loadedChild3.Мама);
                Assert.Null(loadedChild3.Друг);
                Assert.Equal(5, loadedChild4.ПорядковыйНомер);
                Assert.NotNull(loadedChild4.Папа);
                Assert.Null(loadedChild4.Мама);
                Assert.Null(loadedChild4.Друг);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Множественные объекты с циклической вложенностью, когда объекты сохраняются в нескольких транзакциях. В этом случае должно разрешаться через доп. команду UPDATE.
        /// </summary>
        [Fact]
        public void InsertHierarchyCycleTest5()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var parent = new Медведь { ПорядковыйНомер = 1 };
                dataService.UpdateObject(parent);

                var child = new Медведь { ПорядковыйНомер = 2, Папа = parent };
                dataService.UpdateObject(child);

                parent.Папа = child;

                // Act.
                dataService.UpdateObject(parent);

                var loadedParent = PKHelper.CreateDataObject<Медведь>(parent);
                dataService.LoadObject(loadedParent);

                var loadedChild = PKHelper.CreateDataObject<Медведь>(child);
                dataService.LoadObject(loadedChild);

                // Assert.
                Assert.Equal(1, loadedParent.ПорядковыйНомер);
                Assert.NotNull(loadedParent.Папа);
                Assert.Null(loadedParent.Мама);
                Assert.Null(loadedParent.Друг);
                Assert.Equal(2, loadedChild.ПорядковыйНомер);
                Assert.NotNull(loadedChild.Папа);
                Assert.Null(loadedChild.Мама);
                Assert.Null(loadedChild.Друг);
            }
        }

        /// <summary>
        /// Тест для вставки огромной цепочки иерархических объектов.
        /// </summary>
        [Fact(Skip = "Тяжелый тест, использовать при необходимости для измерения скорости.")]
        public void InsertHugeHierarchy()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                const int count = 10000;
                const string hierarchyPropName = nameof(Медведь.Папа);
                var hierarchyList = new List<Медведь>();
                foreach (int i in Enumerable.Range(0, count))
                {
                    var currentBear = new Медведь { ПорядковыйНомер = i, };
                    if (i != 0)
                    {
                        var parentBear = hierarchyList[i - 1];
                        Information.SetPropValueByName(currentBear, hierarchyPropName, parentBear);
                    }

                    hierarchyList.Add(currentBear);
                }

                DataObject[] objsToUpdate = hierarchyList.Cast<DataObject>().ToArray();

                // Act.
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                dataService.UpdateObjects(ref objsToUpdate);
                stopwatch.Stop();
                Console.WriteLine("{0} Elapsed Time is {1} ms", dataService.GetType().Name, stopwatch.ElapsedMilliseconds);

                var loadedFirstParent = PKHelper.CreateDataObject<Медведь>(hierarchyList[0]);
                dataService.LoadObject(loadedFirstParent);

                var loadedLastChild = PKHelper.CreateDataObject<Медведь>(hierarchyList[count - 1]);
                dataService.LoadObject(loadedLastChild);

                // Assert.
                Assert.Equal(0, loadedFirstParent.ПорядковыйНомер);
                Assert.Null(loadedFirstParent.Папа);
                Assert.Null(loadedFirstParent.Мама);
                Assert.Null(loadedFirstParent.Друг);
                Assert.Equal(count - 1, loadedLastChild.ПорядковыйНомер);
                Assert.NotNull(loadedLastChild.Папа);
                Assert.Null(loadedLastChild.Мама);
                Assert.Null(loadedLastChild.Друг);
            }
        }
    }
}
