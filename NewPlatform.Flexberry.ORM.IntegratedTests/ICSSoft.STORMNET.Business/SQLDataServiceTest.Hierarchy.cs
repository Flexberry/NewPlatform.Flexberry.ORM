namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

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
                    sqlDataService.OnCreateCommand += (object sender, CreateCommandEventArgs e) =>
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

                // Assert.
                Assert.Equal(1, master.ПорядковыйНомер);
            }
        }

        /// <summary>
        /// В случае, когда объект зависимоти и основной объект создаются в одной транзакции (объекта зависимости предварительно нет в БД).
        /// </summary>
        [Fact]
        public void InsertHierarchyOneTransactionTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var parent = new Медведь { ПорядковыйНомер = 1 };
                var child = new Медведь { ПорядковыйНомер = 2, Папа = parent };

                // Act.
                dataService.UpdateObject(child);

                // Assert.
                Assert.Equal(2, child.ПорядковыйНомер);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// В случае, когда объект зависимоти и основной объект создаются в одной транзакции (объекта зависимости предварительно нет в БД).
        /// </summary>
        [Fact]
        public void InsertHierarchyOneTransactionTest2()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var motherBear = new Медведь { ПорядковыйНомер = 1 };
                dataService.UpdateObject(motherBear);

                var fatherBear = new Медведь { ПорядковыйНомер = 2 };
                var child = new Медведь { ПорядковыйНомер = 3, Папа = fatherBear, Мама = motherBear };

                // Act.
                dataService.UpdateObject(child);

                // Assert.
                Assert.Equal(3, child.ПорядковыйНомер);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// В случае, когда объект зависимоти и основной объект создаются в одной транзакции (объекта зависимости предварительно нет в БД).
        /// </summary>
        [Fact]
        public void InsertHierarchyOneTransactionTest3()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var father = new Медведь { ПорядковыйНомер = 1 };

                var child1 = new Медведь { ПорядковыйНомер = 2, Папа = father };
                var child2 = new Медведь { ПорядковыйНомер = 3, Папа = father };

                DataObject[] objsToUpdate = new DataObject[] { father, child1, child2 };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                Assert.Equal(1, father.ПорядковыйНомер);
                Assert.Equal(2, child1.ПорядковыйНомер);
                Assert.Equal(3, child2.ПорядковыйНомер);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Проверяем, что нет лишних Update-запросов в БД, в случае, когда объект зависимости предварительно есть в БД.
        /// </summary>
        [Fact]
        public void InsertHierarchyMultyTransactionTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                if (dataService is SQLDataService sqlDataService)
                {
                    sqlDataService.OnCreateCommand += (object sender, CreateCommandEventArgs e) =>
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

                // Assert.
                Assert.Equal(2, child.ПорядковыйНомер);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Проверяем, что нет лишних Update-запросов в БД.
        /// В случае, когда объект зависимости уже есть в БД и получается через SetExistObjectPrimaryKey.
        /// </summary>
        [Fact]
        public void InsertHierarchydWithSetExistTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                if (dataService is SQLDataService sqlDataService)
                {
                    sqlDataService.OnCreateCommand += (object sender, CreateCommandEventArgs e) =>
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

                // Assert.
                Assert.Equal(2, child.ПорядковыйНомер);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Множественные объекты с циклической вложенностью. В этом случае должно разрешаться через доп. команду UPDATE.
        /// </summary>
        [Fact]
        public void InsertHierarchyOneTransactionCycleTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                var parent = new Медведь { ПорядковыйНомер = 1 };
                var child1 = new Медведь { ПорядковыйНомер = 2, Папа = parent };
                var child2 = new Медведь { ПорядковыйНомер = 3, Папа = child1 };
                var child3 = new Медведь { ПорядковыйНомер = 4, Папа = child2 };
                var child4 = new Медведь { ПорядковыйНомер = 5, Папа = child3 };

                child1.Мама = child4;

                // Act.
                dataService.UpdateObject(child4);

                // Assert.
                Assert.Equal(5, child4.ПорядковыйНомер);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Множественные объекты с циклической вложенностью, когда все объекты в одной транзакции. В этом случае должно разрешаться через доп. команду UPDATE.
        /// </summary>
        [Fact]
        public void InsertHierarchyOneTransactionCycleTest2()
        {
            foreach (IDataService dataService in DataServices)
            {
                var parent = new Медведь { ПорядковыйНомер = 1 };
                var child1 = new Медведь { ПорядковыйНомер = 2, Папа = parent };
                var child2 = new Медведь { ПорядковыйНомер = 3, Папа = child1 };
                var child3 = new Медведь { ПорядковыйНомер = 4, Папа = child2 };
                var child4 = new Медведь { ПорядковыйНомер = 5, Папа = child3 };

                parent.Папа = child1;

                // Act.
                dataService.UpdateObject(child4);

                // Assert.
                Assert.Equal(5, child4.ПорядковыйНомер);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Множественные объекты с циклической вложенностью, когда все объекты в одной транзакции. В этом случае должно разрешаться через доп. команду UPDATE.
        /// </summary>
        [Fact]
        public void InsertHierarchyOneTransactionCycleTest3()
        {
            foreach (IDataService dataService in DataServices)
            {
                var father = new Медведь { ПорядковыйНомер = 1 };
                var child1 = new Медведь { ПорядковыйНомер = 2, Папа = father };
                var child2 = new Медведь { ПорядковыйНомер = 3, Папа = child1 };
                father.Папа = child2;

                DataObject[] objsToUpdate = new DataObject[] { father, child1, child2 };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                Assert.Equal(1, father.ПорядковыйНомер);
                Assert.Equal(2, child1.ПорядковыйНомер);
                Assert.Equal(3, child2.ПорядковыйНомер);
            }
        }

        /// <summary>
        /// Тест для проверки записи иерархической сущности.
        /// Множественные объекты с циклической вложенностью, когда объекты сохраняются в нескольких транзакциях. В этом случае должно разрешаться через доп. команду UPDATE.
        /// </summary>
        [Fact]
        public void InsertHierarchySeveralTransactionCycleTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                var parent = new Медведь { ПорядковыйНомер = 1 };
                dataService.UpdateObject(parent);

                var child = new Медведь { ПорядковыйНомер = 2, Папа = parent };
                dataService.UpdateObject(child);

                parent.Папа = child;

                // Act.
                dataService.UpdateObject(parent);

                // Assert.
                Assert.Equal(1, parent.ПорядковыйНомер);
            }
        }
    }
}
