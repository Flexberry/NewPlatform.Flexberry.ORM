namespace ICSSoft.STORMNET.Business.Audit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Xunit;

    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    ///     Тесты класса <see cref="AuditService" />.
    /// </summary>
    public partial class AuditServiceTest
    {
        /// <summary>
        ///     Значение поля для "загруженного" объекта.
        /// </summary>
        private const string LoadedValue = "loadedValue";

        /// <summary>
        ///     Список свойств мастера мастера объекта.
        /// </summary>
        private readonly List<string> masterMasterProperties = new List<string>
        {
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject.MasterObject),
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject.MasterObject.Login),
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject.MasterObject.Name),
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject.MasterObject.Surname),
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject.MasterObject.NameSurname),
        };

        /// <summary>
        ///     Список свойств мастера объекта.
        /// </summary>
        private readonly List<string> masterProperties = new List<string>
        {
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject),
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject.Login),
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject.Name),
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject.Surname),
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject.NameSurname),
        };

        /// <summary>
        ///     Список собственных свойств объекта.
        /// </summary>
        private readonly List<string> ownProperties = new List<string>
        {
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.Login),
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.Name),
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.Surname),
            Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.NameSurname),
        };

        /// <summary>
        ///     Список свойств мастера мастера объекта для тестирования.
        /// </summary>
        private readonly List<string> testMasterMasterOwnProperties = new List<string> { Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject.MasterObject.Name) };

        /// <summary>
        ///     Список свойств мастера объекта для тестирования.
        /// </summary>
        private readonly List<string> testMasterOwnProperties = new List<string> { Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject.Name) };

        /// <summary>
        ///     Список собственных свойств объекта для тестирования.
        /// </summary>
        private readonly List<string> testOwnProperties = new List<string> { Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.Name) };

        /// <summary>
        ///     Тест метода <see cref="AuditService.CopyAlteredNotSavedDataObject" />.
        ///     Передаём только собственные свойства.
        /// </summary>
        [Fact]
        public void TestCopyAlteredNotSavedDataObjectOwnProperties()
        {
            // Arrange.
            var oldValue = "OldValue";
            var newValue = "NewValue";
            var auditView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            auditView.AddProperties(ownProperties.ToArray());
            var auditViewPropertyNames = auditView.Properties.Select(x => x.Name).ToList();

            var testView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            testView.AddProperties(testOwnProperties.ToArray());
            var testViewPropertyNames = testView.Properties.Select(x => x.Name).ToList();

            var expectedList = auditViewPropertyNames.Union(testViewPropertyNames).Distinct().ToList();
            var expectedPropertyCount = expectedList.Count;

            var oldObject = new AuditAgregatorObject();
            oldObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            SetViewProperties(oldObject, auditView, oldValue);
            oldObject.SetLoadedProperties(ownProperties.ToArray());

            var newObject = new AuditAgregatorObject();
            newObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            SetViewProperties(newObject, testView, newValue);
            newObject.SetLoadedProperties(testViewPropertyNames.ToArray());

            // Act.
            var loadedProperties = AuditService.CopyAlteredNotSavedDataObject(oldObject, newObject, auditView, null, null);

            // Assert.
            foreach (var auditProperty in auditView.Properties)
            {
                var expectedValue = testViewPropertyNames.Contains(auditProperty.Name) ? newValue : oldValue;
                var actualValue = Information.GetPropValueByName(newObject, auditProperty.Name);
                if (Information.IsStoredProperty(typeof(AuditAgregatorObject), auditProperty.Name))
                {
                    Assert.Equal(expectedValue, actualValue);
                }
            }

            Assert.Equal(expectedPropertyCount, loadedProperties.Count);
            Assert.Equal(expectedPropertyCount, expectedList.Union(loadedProperties).Distinct().Count());
        }

        /// <summary>
        ///     Тест метода <see cref="AuditService.CopyAlteredNotSavedDataObject" />.
        ///     Передаём мастеровые свойства, у нового объекта мастер инициализирован, сам мастер не изменился.
        /// </summary>
        [Fact]
        public void TestCopyAlteredNotSavedDataObjectMasterProperties()
        {
            // Arrange.
            var oldValue = "OldValue";
            var newValue = "NewValue";
            var auditView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            auditView.AddProperties(ownProperties.ToArray());
            auditView.AddProperties(masterProperties.ToArray());
            var auditViewPropertyNames = auditView.Properties.Select(x => x.Name).ToList();

            var testView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            testView.AddProperties(testOwnProperties.ToArray());
            testView.AddProperties(testMasterOwnProperties.ToArray());
            var testViewPropertyNames = testView.Properties.Select(x => x.Name).ToList();

            var expectedList = auditViewPropertyNames.Union(testViewPropertyNames).Distinct().ToList();
            var expectedPropertyCount = expectedList.Count;

            var masterObject = new AuditMasterObject();
            var oldObject = new AuditAgregatorObject();
            oldObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            oldObject.MasterObject = masterObject;
            SetViewProperties(oldObject, auditView, oldValue);
            oldObject.SetLoadedProperties(ownProperties.ToArray());
            oldObject.MasterObject.SetLoadedProperties(ownProperties.ToArray());

            var newObject = new AuditAgregatorObject();
            newObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            newObject.MasterObject = new AuditMasterObject { __PrimaryKey = masterObject.__PrimaryKey };
            SetViewProperties(newObject, testView, newValue);
            newObject.SetLoadedProperties(testOwnProperties.ToArray());
            newObject.AddLoadedProperties(Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject));
            newObject.MasterObject.SetLoadedProperties(testOwnProperties.ToArray());
            newObject.SetDataCopy(newObject);

            // Act.
            var loadedProperties = AuditService.CopyAlteredNotSavedDataObject(oldObject, newObject, auditView, null, null);

            // Assert.
            foreach (var auditProperty in auditView.Properties)
            {
                var expectedValue = testViewPropertyNames.Contains(auditProperty.Name) ? newValue : oldValue;
                var actualValue = Information.GetPropValueByName(newObject, auditProperty.Name);

                if (!(actualValue is DataObject) && Information.IsStoredProperty(typeof(AuditAgregatorObject), auditProperty.Name))
                {
                    Assert.Equal(expectedValue, actualValue);
                }
            }

            Assert.Equal(expectedPropertyCount, loadedProperties.Count);
            Assert.Equal(expectedPropertyCount, expectedList.Union(loadedProperties).Distinct().Count());
        }

        /// <summary>
        ///     Тест метода <see cref="AuditService.CopyAlteredNotSavedDataObject" />.
        ///     Передаём мастеровые свойства, у нового объекта мастер инициализирован, у старого - null.
        /// </summary>
        [Fact]
        public void TestCopyAlteredNotSavedDataObjectMasterPropertiesNullOld()
        {
            // Arrange.
            var oldValue = "OldValue";
            var newValue = "NewValue";
            var auditView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            auditView.AddProperties(ownProperties.ToArray());
            auditView.AddProperties(masterProperties.ToArray());
            var auditViewPropertyNames = auditView.Properties.Select(x => x.Name).ToList();

            var testView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            testView.AddProperties(testOwnProperties.ToArray());
            testView.AddProperties(testMasterOwnProperties.ToArray());
            var testViewPropertyNames = testView.Properties.Select(x => x.Name).ToList();

            var expectedList = auditViewPropertyNames.Union(testViewPropertyNames).Distinct().ToList();
            var expectedPropertyCount = expectedList.Count;

            var masterObject = new AuditMasterObject();
            var oldObject = new AuditAgregatorObject();
            oldObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            oldObject.MasterObject = masterObject;
            SetViewProperties(oldObject, auditView, oldValue);
            oldObject.SetLoadedProperties(ownProperties.ToArray());
            oldObject.AddLoadedProperties(Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject));
            oldObject.MasterObject = null;

            var newObject = new AuditAgregatorObject();
            newObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            newObject.MasterObject = new AuditMasterObject { __PrimaryKey = masterObject.__PrimaryKey };
            SetViewProperties(newObject, testView, newValue);
            newObject.SetLoadedProperties(testOwnProperties.ToArray());
            newObject.AddLoadedProperties(Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject));
            newObject.MasterObject.SetLoadedProperties(testOwnProperties.ToArray());
            var dataCopyObject = new AuditAgregatorObject();
            newObject.CopyTo(dataCopyObject, true, true, false);
            dataCopyObject.MasterObject = null;
            newObject.SetDataCopy(dataCopyObject);
            var ds = new LocalDataService();

            // Act.
            var loadedProperties = AuditService.CopyAlteredNotSavedDataObject(oldObject, newObject, auditView, ds, null);

            // Assert.
            Assert.Equal(1, ds.Counter);
            foreach (var auditProperty in auditView.Properties)
            {
                var expectedValue = testViewPropertyNames.Contains(auditProperty.Name)
                    ? newValue
                    : (auditProperty.Name.StartsWith(Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject) + ".") ? LoadedValue : oldValue);

                var actualValue = Information.GetPropValueByName(newObject, auditProperty.Name);

                if (!(actualValue is DataObject) && Information.IsStoredProperty(typeof(AuditAgregatorObject), auditProperty.Name))
                {
                    Assert.Equal(expectedValue, actualValue);
                }
            }

            Assert.Equal(expectedPropertyCount, loadedProperties.Count);
            Assert.Equal(expectedPropertyCount, expectedList.Union(loadedProperties).Distinct().Count());
        }

        /// <summary>
        ///     Тест метода <see cref="AuditService.CopyAlteredNotSavedDataObject" />.
        ///     Передаём мастеровые свойства, у нового объекта мастер не инициализирован.
        /// </summary>
        [Fact]
        public void TestCopyAlteredNotSavedDataObjectMasterPropertiesNull()
        {
            // Arrange.
            var oldValue = "OldValue";
            var newValue = "NewValue";
            var auditView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            auditView.AddProperties(ownProperties.ToArray());
            auditView.AddProperties(masterProperties.ToArray());
            var auditViewPropertyNames = auditView.Properties.Select(x => x.Name).ToList();

            var testView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            testView.AddProperties(testOwnProperties.ToArray());
            var testViewPropertyNames = testView.Properties.Select(x => x.Name).ToList();

            var expectedList = auditViewPropertyNames.Union(testViewPropertyNames).Distinct().ToList();
            var expectedPropertyCount = expectedList.Count;

            var masterObject = new AuditMasterObject();
            var oldObject = new AuditAgregatorObject();
            oldObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            oldObject.MasterObject = masterObject;
            SetViewProperties(oldObject, auditView, oldValue);
            oldObject.SetLoadedProperties(ownProperties.ToArray());
            oldObject.MasterObject.SetLoadedProperties(ownProperties.ToArray());

            var newObject = new AuditAgregatorObject();
            newObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            SetViewProperties(newObject, testView, newValue);
            newObject.SetLoadedProperties(testOwnProperties.ToArray());

            // Act.
            var loadedProperties = AuditService.CopyAlteredNotSavedDataObject(oldObject, newObject, auditView, null, null);

            // Assert.
            foreach (var auditProperty in auditView.Properties)
            {
                var expectedValue = testViewPropertyNames.Contains(auditProperty.Name) ? newValue : oldValue;
                var actualValue = Information.GetPropValueByName(newObject, auditProperty.Name);

                if (!(actualValue is DataObject) && Information.IsStoredProperty(typeof(AuditAgregatorObject), auditProperty.Name))
                {
                    Assert.Equal(expectedValue, actualValue);
                }
            }

            Assert.Equal(expectedPropertyCount, loadedProperties.Count);
            Assert.Equal(expectedPropertyCount, expectedList.Union(loadedProperties).Distinct().Count());
        }

        /// <summary>
        ///     Тест метода <see cref="AuditService.CopyAlteredNotSavedDataObject" />.
        ///     Передаём мастеровые свойства, у нового объекта мастер инициализирован и равен null.
        /// </summary>
        [Fact]
        public void TestCopyAlteredNotSavedDataObjectMasterPropertiesNullLoaded()
        {
            // Arrange.
            var oldValue = "OldValue";
            var newValue = "NewValue";
            var auditView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            auditView.AddProperties(ownProperties.ToArray());
            auditView.AddProperties(masterProperties.ToArray());
            var auditViewPropertyNames = auditView.Properties.Select(x => x.Name).ToList();

            var testView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            testView.AddProperties(testOwnProperties.ToArray());
            var testViewPropertyNames = testView.Properties.Select(x => x.Name).ToList();

            var expectedList = auditViewPropertyNames.Union(testViewPropertyNames).Distinct().ToList();
            var expectedPropertyCount = expectedList.Count;

            var masterObject = new AuditMasterObject();
            var oldObject = new AuditAgregatorObject();
            oldObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            oldObject.MasterObject = masterObject;
            SetViewProperties(oldObject, auditView, oldValue);
            oldObject.SetLoadedProperties(ownProperties.ToArray());
            oldObject.MasterObject.SetLoadedProperties(ownProperties.ToArray());

            var newObject = new AuditAgregatorObject();
            newObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            SetViewProperties(newObject, testView, newValue);
            newObject.SetLoadedProperties(testOwnProperties.ToArray());
            newObject.AddLoadedProperties(Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject));

            // Act.
            var loadedProperties = AuditService.CopyAlteredNotSavedDataObject(oldObject, newObject, auditView, null, null);

            // Assert.
            foreach (var auditProperty in auditView.Properties)
            {
                var expectedValue = testViewPropertyNames.Contains(auditProperty.Name)
                    ? newValue
                    : (auditProperty.Name.StartsWith(Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject) + ".")
                        || auditProperty.Name == Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject)
                        ? null
                        : oldValue);
                var actualValue = Information.GetPropValueByName(newObject, auditProperty.Name);

                if (!(actualValue is DataObject) && Information.IsStoredProperty(typeof(AuditAgregatorObject), auditProperty.Name))
                {
                    Assert.Equal(expectedValue, actualValue);
                }
            }

            Assert.Equal(expectedPropertyCount, loadedProperties.Count);
            Assert.Equal(expectedPropertyCount, expectedList.Union(loadedProperties).Distinct().Count());
        }

        /// <summary>
        ///     Тест метода <see cref="AuditService.CopyAlteredNotSavedDataObject" />.
        ///     Передаём мастеровые свойства мастера, у нового объекта мастер инициализирован, сам мастер не изменился.
        /// </summary>
        [Fact]
        public void TestCopyAlteredNotSavedDataObjectMasterMasterProperties()
        {
            // Arrange.
            var oldValue = "OldValue";
            var newValue = "NewValue";
            var auditView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            auditView.AddProperties(ownProperties.ToArray());
            auditView.AddProperties(masterProperties.ToArray());
            auditView.AddProperties(masterMasterProperties.ToArray());
            var auditViewPropertyNames = auditView.Properties.Select(x => x.Name).ToList();

            var testView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            testView.AddProperties(testOwnProperties.ToArray());
            testView.AddProperties(testMasterOwnProperties.ToArray());
            testView.AddProperties(testMasterMasterOwnProperties.ToArray());
            var testViewPropertyNames = testView.Properties.Select(x => x.Name).ToList();

            var expectedList = auditViewPropertyNames.Union(testViewPropertyNames).Distinct().ToList();
            var expectedPropertyCount = expectedList.Count;

            var masterObject = new AuditMasterObject();
            var masterMasterObject = new AuditMasterMasterObject();
            var oldObject = new AuditAgregatorObject();
            oldObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            oldObject.MasterObject = masterObject;
            oldObject.MasterObject.MasterObject = masterMasterObject;
            SetViewProperties(oldObject, auditView, oldValue);
            oldObject.SetLoadedProperties(ownProperties.ToArray());
            oldObject.MasterObject.SetLoadedProperties(ownProperties.ToArray());
            oldObject.MasterObject.MasterObject.SetLoadedProperties(ownProperties.ToArray());

            var newObject = new AuditAgregatorObject();
            newObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            newObject.MasterObject = new AuditMasterObject { __PrimaryKey = masterObject.__PrimaryKey };
            newObject.MasterObject.MasterObject = new AuditMasterMasterObject { __PrimaryKey = masterMasterObject.__PrimaryKey };
            SetViewProperties(newObject, testView, newValue);
            newObject.SetLoadedProperties(testOwnProperties.ToArray());
            newObject.AddLoadedProperties(Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject));
            newObject.MasterObject.SetLoadedProperties(testOwnProperties.ToArray());
            newObject.MasterObject.AddLoadedProperties(Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject));
            newObject.MasterObject.MasterObject.SetLoadedProperties(testOwnProperties.ToArray());
            newObject.SetDataCopy(newObject);
            newObject.MasterObject.SetDataCopy(newObject.MasterObject);

            // Act.
            var loadedProperties = AuditService.CopyAlteredNotSavedDataObject(oldObject, newObject, auditView, null, null);

            // Assert.
            foreach (var auditProperty in auditView.Properties)
            {
                var expectedValue = testViewPropertyNames.Contains(auditProperty.Name) ? newValue : oldValue;
                var actualValue = Information.GetPropValueByName(newObject, auditProperty.Name);

                if (!(actualValue is DataObject) && Information.IsStoredProperty(typeof(AuditAgregatorObject), auditProperty.Name))
                {
                    Assert.Equal(expectedValue, actualValue);
                }
            }

            Assert.Equal(expectedPropertyCount, loadedProperties.Count);
            Assert.Equal(expectedPropertyCount, expectedList.Union(loadedProperties).Distinct().Count());
        }

        /// <summary>
        ///     Тест метода <see cref="AuditService.CopyAlteredNotSavedDataObject" />.
        ///     Передаём мастеровые свойства мастера, у нового объекта мастер не инициализирован.
        /// </summary>
        [Fact]
        public void TestCopyAlteredNotSavedDataObjectMasterPropertiesNullMaster()
        {
            // Arrange.
            var oldValue = "OldValue";
            var newValue = "NewValue";
            var auditView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            auditView.AddProperties(ownProperties.ToArray());
            auditView.AddProperties(masterProperties.ToArray());
            auditView.AddProperties(masterMasterProperties.ToArray());
            var auditViewPropertyNames = auditView.Properties.Select(x => x.Name).ToList();

            var testView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            testView.AddProperties(testOwnProperties.ToArray());
            var testViewPropertyNames = testView.Properties.Select(x => x.Name).ToList();

            var expectedList = auditViewPropertyNames.Union(testViewPropertyNames).Distinct().ToList();
            var expectedPropertyCount = expectedList.Count;

            var masterObject = new AuditMasterObject();
            var masterMasterObject = new AuditMasterMasterObject();
            var oldObject = new AuditAgregatorObject();
            oldObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            oldObject.MasterObject = masterObject;
            oldObject.MasterObject.MasterObject = masterMasterObject;
            SetViewProperties(oldObject, auditView, oldValue);
            oldObject.SetLoadedProperties(ownProperties.ToArray());
            oldObject.MasterObject.SetLoadedProperties(ownProperties.ToArray());
            oldObject.MasterObject.MasterObject.SetLoadedProperties(ownProperties.ToArray());

            var newObject = new AuditAgregatorObject();
            newObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            SetViewProperties(newObject, testView, newValue);
            newObject.SetLoadedProperties(testOwnProperties.ToArray());

            // Act.
            var loadedProperties = AuditService.CopyAlteredNotSavedDataObject(oldObject, newObject, auditView, null, null);

            // Assert.
            foreach (var auditProperty in auditView.Properties)
            {
                var expectedValue = testViewPropertyNames.Contains(auditProperty.Name) ? newValue : oldValue;
                var actualValue = Information.GetPropValueByName(newObject, auditProperty.Name);

                if (!(actualValue is DataObject) && Information.IsStoredProperty(typeof(AuditAgregatorObject), auditProperty.Name))
                {
                    Assert.Equal(expectedValue, actualValue);
                }
            }

            Assert.Equal(expectedPropertyCount, loadedProperties.Count);
            Assert.Equal(expectedPropertyCount, expectedList.Union(loadedProperties).Distinct().Count());
        }

        /// <summary>
        ///     Тест метода <see cref="AuditService.CopyAlteredNotSavedDataObject" />.
        ///     Передаём мастеровые свойства мастера, у нового объекта мастер мастера не инициализирован.
        /// </summary>
        [Fact]
        public void TestCopyAlteredNotSavedDataObjectMasterMasterPropertiesNull()
        {
            // Arrange.
            var oldValue = "OldValue";
            var newValue = "NewValue";
            var auditView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            auditView.AddProperties(ownProperties.ToArray());
            auditView.AddProperties(masterProperties.ToArray());
            auditView.AddProperties(masterMasterProperties.ToArray());
            var auditViewPropertyNames = auditView.Properties.Select(x => x.Name).ToList();

            var testView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            testView.AddProperties(testOwnProperties.ToArray());
            testView.AddProperties(testMasterOwnProperties.ToArray());
            var testViewPropertyNames = testView.Properties.Select(x => x.Name).ToList();

            var expectedList = auditViewPropertyNames.Union(testViewPropertyNames).Distinct().ToList();
            var expectedPropertyCount = expectedList.Count;

            var masterObject = new AuditMasterObject();
            var masterMasterObject = new AuditMasterMasterObject();
            var oldObject = new AuditAgregatorObject();
            oldObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            oldObject.MasterObject = masterObject;
            oldObject.MasterObject.MasterObject = masterMasterObject;
            SetViewProperties(oldObject, auditView, oldValue);
            oldObject.SetLoadedProperties(ownProperties.ToArray());
            oldObject.MasterObject.SetLoadedProperties(ownProperties.ToArray());
            oldObject.MasterObject.MasterObject.SetLoadedProperties(ownProperties.ToArray());

            var newObject = new AuditAgregatorObject();
            newObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            newObject.MasterObject = new AuditMasterObject { __PrimaryKey = masterObject.__PrimaryKey };
            SetViewProperties(newObject, testView, newValue);
            newObject.SetLoadedProperties(testOwnProperties.ToArray());
            newObject.AddLoadedProperties(Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject));
            newObject.MasterObject.SetLoadedProperties(testOwnProperties.ToArray());
            newObject.SetDataCopy(newObject);
            newObject.MasterObject.SetDataCopy(newObject.MasterObject);

            // Act.
            var loadedProperties = AuditService.CopyAlteredNotSavedDataObject(oldObject, newObject, auditView, null, null);

            // Assert.
            foreach (var auditProperty in auditView.Properties)
            {
                var expectedValue = testViewPropertyNames.Contains(auditProperty.Name) ? newValue : oldValue;
                var actualValue = Information.GetPropValueByName(newObject, auditProperty.Name);

                if (!(actualValue is DataObject) && Information.IsStoredProperty(typeof(AuditAgregatorObject), auditProperty.Name))
                {
                    Assert.Equal(expectedValue, actualValue);
                }
            }

            Assert.Equal(expectedPropertyCount, loadedProperties.Count);
            Assert.Equal(expectedPropertyCount, expectedList.Union(loadedProperties).Distinct().Count());
        }

        /// <summary>
        ///     Тест метода <see cref="AuditService.CopyAlteredNotSavedDataObject" />.
        ///     Передаём мастеровые свойства, у нового объекта мастер инициализирован, сам мастер изменился и потребуется его
        ///     подгрузить.
        /// </summary>
        [Fact]
        public void TestCopyAlteredNotSavedDataObjectMasterPropertiesWithLoad()
        {
            // Arrange.
            var oldValue = "OldValue";
            var newValue = "NewValue";
            var auditView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            auditView.AddProperties(ownProperties.ToArray());
            auditView.AddProperties(masterProperties.ToArray());
            var auditViewPropertyNames = auditView.Properties.Select(x => x.Name).ToList();

            var testView = new View { DefineClassType = typeof(AuditAgregatorObject) };
            testView.AddProperties(testOwnProperties.ToArray());
            testView.AddProperties(testMasterOwnProperties.ToArray());
            var testViewPropertyNames = testView.Properties.Select(x => x.Name).ToList();

            var expectedList = auditViewPropertyNames.Union(testViewPropertyNames).Distinct().ToList();
            var expectedPropertyCount = expectedList.Count;

            var oldObject = new AuditAgregatorObject();
            oldObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            oldObject.MasterObject = new AuditMasterObject();
            SetViewProperties(oldObject, auditView, oldValue);
            oldObject.SetLoadedProperties(ownProperties.ToArray());
            oldObject.MasterObject.SetLoadedProperties(ownProperties.ToArray());

            var newObject = new AuditAgregatorObject();
            newObject.SetExistObjectPrimaryKey(Guid.NewGuid());
            newObject.MasterObject = new AuditMasterObject();
            SetViewProperties(newObject, testView, newValue);
            newObject.SetLoadedProperties(testOwnProperties.ToArray());
            newObject.AddLoadedProperties(Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject));
            newObject.MasterObject.SetLoadedProperties(testOwnProperties.ToArray());
            var dataCopyObject = new AuditAgregatorObject();
            newObject.CopyTo(dataCopyObject, true, true, false);
            dataCopyObject.MasterObject = null;
            newObject.SetDataCopy(dataCopyObject);

            var ds = new LocalDataService();

            // Act.
            var loadedProperties = AuditService.CopyAlteredNotSavedDataObject(oldObject, newObject, auditView, ds, null);

            // Assert.
            Assert.Equal(1, ds.Counter);
            foreach (var auditProperty in auditView.Properties)
            {
                var expectedValue = testViewPropertyNames.Contains(auditProperty.Name)
                    ? newValue
                    : (auditProperty.Name.StartsWith(Information.ExtractPropertyPath<AuditAgregatorObject>(x => x.MasterObject) + ".") ? LoadedValue : oldValue);
                var actualValue = Information.GetPropValueByName(newObject, auditProperty.Name);

                if (!(actualValue is DataObject) && Information.IsStoredProperty(typeof(AuditAgregatorObject), auditProperty.Name))
                {
                    Assert.Equal(expectedValue, actualValue);
                }
            }

            Assert.Equal(expectedPropertyCount, loadedProperties.Count);
            Assert.Equal(expectedPropertyCount, expectedList.Union(loadedProperties).Distinct().Count());
        }

        /// <summary>
        ///     Установка в объект свойств по представлению.
        /// </summary>
        /// <param name="dataObject">Объект данных, в который нужно установить свойства.</param>
        /// <param name="setView">Представление, по которому нужно установить свойства.</param>
        /// <param name="setValue">Устанавливаемое значение.</param>
        private static void SetViewProperties(DataObject dataObject, View setView, string setValue)
        {
            foreach (var propertyInView in setView.Properties)
            {
                if (Information.GetPropertyType(dataObject.GetType(), propertyInView.Name) == typeof(string))
                {
                    Information.SetPropValueByName(dataObject, propertyInView.Name, setValue);
                }
            }
        }

        /// <summary>
        ///     Вспомогательный сервис данных, через который организуется имитация обновления объекта.
        /// </summary>
        private class LocalDataService : MSSQLDataService
        {
            /// <summary>
            ///     Количество обращений к инстанции данного класса.
            /// </summary>
            public int Counter;

            /// <summary>
            ///     Имитация загрузки объекта.
            /// </summary>
            /// <param name="dataObjectView">Представление для загрузки.</param>
            /// <param name="dobject">Объект для загрузки.</param>
            /// <param name="clearDataObject">Следует ли очистить объект перед загрузкой.</param>
            /// <param name="checkExistingObject">Проводить проверку существования ссылочных объектов.</param>
            public override void LoadObject(View dataObjectView, DataObject dobject, bool clearDataObject, bool checkExistingObject)
            {
                SetViewProperties(dobject, dataObjectView, LoadedValue);
                Counter++;
            }

            /// <summary>
            ///     Имитация загрузки объекта с транзакцией.
            /// </summary>
            /// <param name="dataObjectView">Представление для загрузки.</param>
            /// <param name="dobject">Объект для загрузки.</param>
            /// <param name="сlearDataObject">Следует ли очистить объект перед загрузкой.</param>
            /// <param name="сheckExistingObject">Проводить проверку существования ссылочных объектов.</param>
            /// <param name="dataObjectCache">Кэш загрузки объектов.</param>
            /// <param name="connection">Соединение для обновления.</param>
            /// <param name="transaction">Транзакция для обновления.</param>
            public override void LoadObjectByExtConn(
                View dataObjectView,
                DataObject dobject,
                bool сlearDataObject,
                bool сheckExistingObject,
                DataObjectCache dataObjectCache,
                IDbConnection connection,
                IDbTransaction transaction)
            {
                SetViewProperties(dobject, dataObjectView, LoadedValue);
                Counter++;
            }
        }
    }
}