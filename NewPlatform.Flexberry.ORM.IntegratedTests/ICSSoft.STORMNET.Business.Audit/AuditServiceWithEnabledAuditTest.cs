namespace ICSSoft.STORMNET.Business.Audit.IntegratedTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using ICSSoft.STORMNET.Business.Audit.Objects;

    using Xunit;

    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// ORM-integrated unit test class for <see cref="AuditService"/> with enabled audit.
    /// </summary>
    public class AuditServiceWithEnabledAuditTest : BaseAuditServiceTest
    {
        /// <summary>
        /// Unit test for <see cref="AuditService.AddCreateAuditInformation"/> and <see cref="AuditService.AddEditAuditInformation"/>.
        /// Tests correctness of setting of audit fields from <see cref="IDataObjectWithAuditFields"/> for the type with enabled audit.
        /// </summary>
        [Fact]
        public void TestSettingAuditFieldsForTypeWithEnabledAudit()
        {
            foreach (var dataService in DataServices)
            {
                var d1 = new AuditClassWithSettings();

                // Audit fields are empty after creating object.
                Assert.Null(d1.CreateTime);
                Assert.Null(d1.Creator);
                Assert.Null(d1.Editor);
                Assert.Null(d1.EditTime);

                dataService.UpdateObject(d1);

                // Audit fields are set after persisting.
                Assert.NotNull(d1.CreateTime);
                Assert.NotNull(d1.Creator);
                Assert.Null(d1.Editor);
                Assert.Null(d1.EditTime);

                var d2 = new AuditClassWithSettings();
                d2.SetExistObjectPrimaryKey(d1.__PrimaryKey);
                dataService.LoadObject(d2);

                // Audit fields are persisted.
                Assert.NotNull(d2.CreateTime);
                Assert.NotNull(d2.Creator);
                Assert.Null(d2.Editor);
                Assert.Null(d2.EditTime);

                d2.Name = "42";
                dataService.UpdateObject(d2);

                // Audit fields are set after persisting.
                Assert.NotNull(d2.CreateTime);
                Assert.NotNull(d2.Creator);
                Assert.NotNull(d2.Editor);
                Assert.NotNull(d2.EditTime);

                var d3 = new AuditClassWithSettings();
                d3.SetExistObjectPrimaryKey(d2.__PrimaryKey);
                dataService.LoadObject(d3);

                // Audit fields are persisted.
                Assert.NotNull(d3.CreateTime);
                Assert.NotNull(d3.Creator);
                Assert.NotNull(d3.Editor);
                Assert.NotNull(d3.EditTime);
            }
        }

        /// <summary>
        /// Тест скорости обновления объектов при включенном аудите.
        /// </summary>
        [Fact]
        public void UpdateManyObjectsWithEnabledAuditTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                Stopwatch stopwatch = new Stopwatch();

                dataService.AuditService.DisableAudit();

                // Шаг 1. Создание объектов и сохранение в БД.
                const int count = 15000;
                var list = new List<AuditClassWithSettings>();
                for (int i = 0; i < count; i++)
                {
                    var obj = new AuditClassWithSettings { Name = $"Name{i}" };
                    list.Add(obj);
                }

                DataObject[] arrList = list.ToArray();
                dataService.UpdateObjects(ref arrList);

                // Шаг 2. Помечаем все объекты как удаляемые.
                foreach (var obj in list)
                {
                    obj.SetStatus(ObjectStatus.Deleted);
                }

                // Шаг 3. Первую половину объектов удаляем с выключенным аудитом.
                arrList = list.Take(count / 2).ToArray();

                stopwatch.Restart();
                dataService.UpdateObjects(ref arrList);
                stopwatch.Stop();

                var timeWithoutAudit = stopwatch.ElapsedMilliseconds;

                // Шаг 4. Вторую половину объектов удаляем с включенным аудитом.
                arrList = list.Skip(count / 2).Take(count / 2).ToArray();

                stopwatch.Restart();
                dataService.AuditService.EnableAudit(false);
                dataService.UpdateObjects(ref arrList);
                stopwatch.Stop();

                var timeWithAudit = stopwatch.ElapsedMilliseconds;

                if (timeWithAudit > 180000)
                {
                    throw new Exception("Operation is timed out.");
                }
            }
        }

        /// <summary>
        /// Gets the enabled audit service for the test.
        /// </summary>
        /// <returns>Returns instance of the <see cref="AuditService" /> class that will be used for the test.</returns>
        protected override AuditService GetAuditServiceForTest()
        {
            return new AuditService
            {
                AppSetting = new AuditAppSetting { AuditEnabled = true },
                ApplicationMode = AppMode.Win,
                Audit = new EmptyAudit(),
            };
        }
    }
}
