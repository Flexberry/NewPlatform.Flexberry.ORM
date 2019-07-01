namespace ICSSoft.STORMNET.Tests.TestClasses.Tools
{
    using System;
    using System.Reflection;
    using System.Xml;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.Tools;
    using IIS.TestClassesForPostgres;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    ///This is a test class for ToolXMLTest and is intended
    ///to contain all ToolXMLTest Unit Tests
    ///</summary>

    public class ToolXMLTest
    {
        /// <summary>
        ///A test for ToolXML Constructor
        ///</summary>
        [Fact]

        public void ToolXMLConstructorTest()
        {
            ToolXML target = new ToolXML();
            Assert.NotNull(target);
        }

        [Fact]

        public void DataObjectWithKeyGuid2XMLDocumentTest()
        {
            DataObjectWithKeyGuid dataObjectWithKeyGuid = new DataObjectWithKeyGuid();
            KeyGuid g = Guid.NewGuid();
            dataObjectWithKeyGuid.LinkToMaster1 = g;
            Console.WriteLine("Записали Guid " + dataObjectWithKeyGuid.LinkToMaster1);
            ICSSoft.STORMNET.DataObject dObjToXML = dataObjectWithKeyGuid;
            var serializedObj = ToolXML.DataObject2XMLDocument(ref dObjToXML);

            DataObjectWithKeyGuid dataObjectWithKeyGuidFromXML = new DataObjectWithKeyGuid();

            ICSSoft.STORMNET.DataObject dObjFromXML = dataObjectWithKeyGuidFromXML;
            ToolXML.XMLDocument2DataObject(ref dObjFromXML, serializedObj);

            Assert.Equal(((DataObjectWithKeyGuid)dObjFromXML).LinkToMaster1, g);
            Assert.Null(((DataObjectWithKeyGuid)dObjFromXML).LinkToMaster2);
        }

        [Fact]

        public void SimpleDataObject2XMLDocumentTest()
        {
            SimpleDataObject simpleDataObject = new SimpleDataObject();
            string name = "Tomcat";
            int age = 15;
            KeyGuid pk = (KeyGuid)simpleDataObject.__PrimaryKey;
            simpleDataObject.Name = name;
            simpleDataObject.Age = age;

            Console.WriteLine("Записали Name: " + simpleDataObject.Name + " Age: " + simpleDataObject.Age);
            ICSSoft.STORMNET.DataObject dObjToXML = simpleDataObject;
            var serializedObj = ToolXML.DataObject2XMLDocument(ref dObjToXML);

            ICSSoft.STORMNET.DataObject dObjFromXML = null;
            bool babah = false;
            try
            {
                ToolXML.XMLDocument2DataObject(ref dObjFromXML, serializedObj);
            }
            catch (ArgumentNullException)
            {
                babah = true;
            }
            Assert.True(babah);

            SimpleDataObject simpleDataObjectFromXML = new SimpleDataObject();
            dObjFromXML = simpleDataObjectFromXML;
            ToolXML.XMLDocument2DataObject(ref dObjFromXML, serializedObj);

            Assert.Equal(((SimpleDataObject)dObjFromXML).Name, name);
            Assert.Equal(((SimpleDataObject)dObjFromXML).Age, age);
            Assert.Equal(dObjFromXML.__PrimaryKey, pk);
        }

        [Fact]
        public void AssemblyLoadTest()
        {
            string asmName = "ICSSoft.STORMNET.DataObject, Version=1.0.0.1, Culture=neutral";//typeof(DataObject).Assembly.FullName;
            Console.WriteLine(asmName);
            Assembly asm = null;
            AssemblyName asmN = new AssemblyName(asmName);
            asm = Assembly.Load(asmN);
            Assert.NotNull(asm);
            try
            {
                //asm = Assembly.Load(asmName);
            }
            catch { }
            if (asm == null && asmName.IndexOf("PublicKeyToken=") > -1)
            {
                int ind = asmName.LastIndexOf("PublicKeyToken=") - 2;
                string shortAsmName = asmName.Substring(0, ind);
                Console.WriteLine(shortAsmName);
                try
                {
                    asm = Assembly.Load(shortAsmName);
                }
                catch { }
            }

            if (asm == null)
            {
                throw new Exception("Не могу найти сборку: " + asmName);
            }
        }

        /// <summary>
        /// Тестирование сериализации в xml объекта данных вместе с его мастерами (и его десериализации)
        /// </summary>
        [Fact]

        public void DataObjectToXmlWithMastersTest()
        {
            var credit = new Кредит() { ДатаВыдачи = DateTime.Now, СрокКредита = 300, СуммаКредита = 1000 };
            var client = new Клиент() { ФИО = "Cookie", Прописка = "Славный город печенек" };
            var inspector = new ИнспекторПоКредиту() { ФИО = "CookieInspector" };
            credit.Клиент = client;
            credit.ИнспекторПоКредиту = inspector;
            var planStroke = new Выплаты() { ДатаВыплаты = DateTime.Now, СуммаВыплаты = 100500 };
            credit.Выплаты.Add(planStroke);
            var dataObject = (ICSSoft.STORMNET.DataObject)credit;

            try
            {
                var serializedObj = ToolXML.DataObject2XMLDocument(ref dataObject, true, false, false, true);
                var credit2 = new Кредит();
                var dataObject2 = (ICSSoft.STORMNET.DataObject)credit2;
                ToolXML.XMLDocument2DataObject(ref dataObject2, serializedObj);
                Assert.Equal(credit.ДатаВыдачи.ToString(), credit2.ДатаВыдачи.ToString());
                Assert.Equal(credit.СрокКредита, credit2.СрокКредита);
                Assert.Equal(credit.СуммаКредита, credit2.СуммаКредита);
                Assert.Equal(credit.ИнспекторПоКредиту.__PrimaryKey, credit2.ИнспекторПоКредиту.__PrimaryKey);
                Assert.Equal(credit.ИнспекторПоКредиту.ФИО, credit2.ИнспекторПоКредиту.ФИО);
                Assert.Equal(credit.Клиент.__PrimaryKey, credit2.Клиент.__PrimaryKey);
                Assert.Equal(credit.Клиент.ФИО, credit2.Клиент.ФИО);
                Assert.Equal(credit.Клиент.Прописка, credit2.Клиент.Прописка);
                Assert.Equal(credit.Выплаты.Count, credit.Выплаты.Count);
                Assert.Equal(credit.Выплаты[0].__PrimaryKey, credit.Выплаты[0].__PrimaryKey);
                Assert.Equal(credit.Выплаты[0].ДатаВыплаты.ToString(), credit.Выплаты[0].ДатаВыплаты.ToString());
                Assert.Equal(credit.Выплаты[0].СуммаВыплаты, credit.Выплаты[0].СуммаВыплаты);
            }
            catch (Exception ex)
            {
                Assert.True(false, "Не удалось сериализовать/десериализовать объект вместе с его мастерами: " + ex.Message);
            }

            //Тестируем десериализацию из строки в формате xml
            string innerXml = "<STORMNETXMLDataService><IIS.TestAuditPr.Клиент CreateTime=\"\" Creator=\"\" EditTime=\"\" Editor=\"\" ФИО=\"TestWCFService\" Прописка=\"TestWCFService\" __PrimaryKey=\"{266c89c9-a3fe-4183-8d9b-79f290057f22}\" DynamicProperties=\"\" /><Assemblies /></STORMNETXMLDataService>";
            var credit3 = new Клиент();
            var dataObject3 = (ICSSoft.STORMNET.DataObject)credit3;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.InnerXml = innerXml;
            ToolXML.XMLDocument2DataObject(ref dataObject3, xmlDocument);
            Assert.Equal("TestWCFService", credit3.ФИО);
            Assert.Equal("TestWCFService", credit3.Прописка);
        }

        /// <summary>
        /// Тест десериализации наследника класса <see cref="ICSSoft.STORMNET.DataObject"/>
        /// с полем типа <see cref="ICSSoft.STORMNET.FileType.File"/> 
        /// при значении этого поля - <c>null</c>.
        /// </summary>
        [Fact(Skip = "Раскоментить после исправления ошибки 94794")]
        public void NullFileSerialization()
        {
            // Сейчас объект не десериализуется.

            // Arrange.
            STORMNET.DataObject nullFile = new NullFileField() { FileField = null };
            STORMNET.DataObject deserializedObject = new NullFileField();

            var serializedObj = ToolXML.DataObject2XMLDocument(ref nullFile);

            // Act.
            ToolXML.XMLDocument2DataObject(ref deserializedObject, serializedObj);

        }

        /// <summary>
        /// Тест десериализации наследника класса <see cref="ICSSoft.STORMNET.DataObject"/>
        /// с полем типа <see cref="ICSSoft.STORMNET.UserDataTypes.WebFile"/>
        /// при значении этого поля - <c>null</c>.
        /// </summary>
        [Fact(Skip = "Раскоментить после исправления ошибки 94794")]
        public void NullWebFileSerialization()
        {
            // Сейчас объект не десериализуется.

            // Arrange.
            STORMNET.DataObject nullWebFile = new Class_WebFile() { Attr = null };
            STORMNET.DataObject deserializedObject = new Class_WebFile();

            var serializedObj = ToolXML.DataObject2XMLDocument(ref nullWebFile);

            // Act.
            ToolXML.XMLDocument2DataObject(ref deserializedObject, serializedObj);

        }

        /// <summary>
        /// Тест десериализации наследника класса <see cref="ICSSoft.STORMNET.DataObject"/>
        /// с полем типа <see cref="ICSSoft.STORMNET.UserDataTypes.NullableDateTime"/> 
        /// при значении этого поля - <c>null</c>.
        /// </summary>
        [Fact]
        public void NullableDateTimeSerialization()
        {
            // Arrange.
            STORMNET.DataObject nullDateTime = new Class_NullableDateTime() { Attr = null };
            STORMNET.DataObject deserializedObject = new Class_NullableDateTime();

            // Act.
            var serializedObj = ToolXML.DataObject2XMLDocument(ref nullDateTime);
            ToolXML.XMLDocument2DataObject(ref deserializedObject, serializedObj);

            // Assert.
            Assert.Equal(((Class_NullableDateTime)nullDateTime).Attr, ((Class_NullableDateTime)deserializedObject).Attr);
        }

        /// <summary>
        /// Тест десериализации наследника класса <see cref="ICSSoft.STORMNET.DataObject"/>
        /// с полем типа <see cref="ICSSoft.STORMNET.UserDataTypes.NullableDecimal"/>
        /// при значении этого поля - <c>null</c>.
        /// </summary>
        [Fact]
        public void NullableDecimalSerialization()
        {
            // Arrange.
            STORMNET.DataObject nullDecimal = new Class_NullableDecimal() { Attr = null };
            STORMNET.DataObject deserializedObject = new Class_NullableDecimal();

            // Act.
            var serializedObj = ToolXML.DataObject2XMLDocument(ref nullDecimal);
            ToolXML.XMLDocument2DataObject(ref deserializedObject, serializedObj);

            // Assert.
            Assert.Equal(((Class_NullableDecimal)nullDecimal).Attr, ((Class_NullableDecimal)deserializedObject).Attr);
        }

        /// <summary>
        /// Тест десериализации наследника класса <see cref="ICSSoft.STORMNET.DataObject"/>
        /// с полем типа <see cref="ICSSoft.STORMNET.UserDataTypes.NullableInt"/>
        /// при значении этого поля - <c>null</c>.
        /// </summary>
        [Fact]
        public void NullableIntSerialization()
        {
            // Arrange.
            STORMNET.DataObject nullInt = new Class_NullableInt() { Attr = null };
            STORMNET.DataObject deserializedObject = new Class_NullableInt();

            // Act.
            var serializedObj = ToolXML.DataObject2XMLDocument(ref nullInt);
            ToolXML.XMLDocument2DataObject(ref deserializedObject, serializedObj);

            // Assert.
            Assert.Equal(((Class_NullableInt)nullInt).Attr, ((Class_NullableInt)deserializedObject).Attr);
        }
    }
}
