



CREATE TABLE [AuditMasterMasterObject] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Login] VARCHAR(255)  NULL,

	 [Name] VARCHAR(255)  NULL,

	 [Surname] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ИсточникФинанс] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [НомИсточникаФин] INT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Apparatus2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ApparatusName] VARCHAR(255)  NULL,

	 [Maker_m0] UNIQUEIDENTIFIER  NULL,

	 [Exporter_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ComputedDetail] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [DetailField1] VARCHAR(255)  NULL,

	 [DetailField2] VARCHAR(255)  NULL,

	 [ComputedMaster] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [AuditClassWithoutSettings] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Выплаты] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ДатаВыплаты] DATETIME  NULL,

	 [СуммаВыплаты] FLOAT  NULL,

	 [Кредит1] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Блоха] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Кличка] VARCHAR(255)  NULL,

	 [МедведьОбитания] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Этап] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Статус] VARCHAR(78)  NULL,

	 [КонфигурацияЭтапа_m0] UNIQUEIDENTIFIER  NOT NULL,

	 [Запрос] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ИсходящийЗапрос] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ПервоеДлинноеПолеДляПроверки] BIT  NULL,

	 [ВтороеДлинноеПолеДляПроверки] VARCHAR(78)  NULL,

	 [ПятоеДлинноеПолеДляПроверки] INT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [AuditMasterObject] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Login] VARCHAR(255)  NULL,

	 [Name] VARCHAR(255)  NULL,

	 [Surname] VARCHAR(255)  NULL,

	 [MasterObject] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Берлога] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Наименование] VARCHAR(255)  NULL,

	 [Комфортность] INT  NULL,

	 [Заброшена] BIT  NULL,

	 [ЛесРасположения] UNIQUEIDENTIFIER  NULL,

	 [Медведь] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Cabbage2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Type] VARCHAR(255)  NULL,

	 [Name] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [TypeNameUsageProviderTestClass] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [НаследникМ2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ИнспПоКредиту] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ФИО] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ДокККонкурсу] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Файл] NVARCHAR(MAX)  NULL,

	 [Конкурс_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [clb] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ref2] UNIQUEIDENTIFIER  NULL,

	 [ref1] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [DataObjectWithKeyGuid] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [LinkToMaster1] uniqueidentifier  NULL,

	 [LinkToMaster2] uniqueidentifier  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ОценкаЭксперта] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ЗначениеОценки] FLOAT  NULL,

	 [Комментарий] VARCHAR(255)  NULL,

	 [Эксперт_m0] UNIQUEIDENTIFIER  NOT NULL,

	 [ЗначениеКритер] UNIQUEIDENTIFIER  NOT NULL,

	 [Идея_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Dish2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [DishName] VARCHAR(255)  NULL,

	 [MainIngridient_m0] UNIQUEIDENTIFIER  NULL,

	 [MainIngridient_m1] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Медведь] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ПорядковыйНомер] INT  NULL,

	 [Вес] INT  NULL,

	 [ЦветГлаз] VARCHAR(255)  NULL,

	 [Пол] VARCHAR(7)  NULL,

	 [ДатаРождения] DATETIME  NULL,

	 [Мама] UNIQUEIDENTIFIER  NULL,

	 [ЛесОбитания] UNIQUEIDENTIFIER  NULL,

	 [Папа] UNIQUEIDENTIFIER  NULL,

	 [Друг_m0] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [TypeUsageProviderTestClassChil] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 [DataObjectForTest_m0] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Soup2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [SoupName] VARCHAR(255)  NULL,

	 [CabbageType] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [StoredClass] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [StoredProperty] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Клиент] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ФИО] VARCHAR(255)  NULL,

	 [Прописка] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ForKeyStorageTest] (

	 [StorageForKey] VARCHAR(255)  NOT NULL,

	 PRIMARY KEY ([StorageForKey]))


CREATE TABLE [Parcel] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Address] VARCHAR(255)  NULL,

	 [Weight] FLOAT  NULL,

	 [DeliveredByHomer] UNIQUEIDENTIFIER  NULL,

	 [DeliveredByMailman] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [КонфигурацияЭтапа] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ТретьеДлинноеПолеДляПроверки] BIT  NULL,

	 [ЧетвертоеДлинноеПолеДляПроверки] INT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [TestClassA] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 [Value] INT  NULL,

	 [Мастер_m0] UNIQUEIDENTIFIER  NULL,

	 [Мастер_m1] UNIQUEIDENTIFIER  NULL,

	 [Мастер_m2] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Перелом] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Дата] DATETIME  NULL,

	 [Тип] VARCHAR(8)  NULL,

	 [Лапа_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [НаследникМ1] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Лес] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Название] VARCHAR(255)  NULL,

	 [Площадь] INT  NULL,

	 [Заповедник] BIT  NULL,

	 [ДатаПослОсмотр] DATETIME  NULL,

	 [Страна] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [DetailClass] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Detailproperty] VARCHAR(255)  NULL,

	 [MasterClass_m0] UNIQUEIDENTIFIER  NULL,

	 [MasterClass_m1] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [AggregatorUpdateObjectTest] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [AggregatorName] VARCHAR(255)  NULL,

	 [Detail] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [КонфигурацияЗапроса] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ТретьеДлинноеПолеДляПроверки] BIT  NULL,

	 [ЧетвертоеДлинноеПолеДляПроверки] INT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Adress2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [HomeNumber] INT  NULL,

	 [Country_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [FullTypesMainAgregator] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [PoleInt] INT  NULL,

	 [PoleDateTime] DATETIME  NULL,

	 [PoleString] VARCHAR(255)  NULL,

	 [PoleFloat] REAL  NULL,

	 [PoleDouble] FLOAT  NULL,

	 [PoleDecimal] DECIMAL(18,4)  NULL,

	 [PoleBool] BIT  NULL,

	 [PoleNullableInt] INT  NULL,

	 [PoleNullableDecimal] DECIMAL(18,4)  NULL,

	 [PoleNullableDateTime] DATETIME  NULL,

	 [PoleNullInt] INT  NULL,

	 [PoleNullDateTime] DATETIME  NULL,

	 [PoleNullFloat] REAL  NULL,

	 [PoleNullDouble] FLOAT  NULL,

	 [PoleNullDecimal] DECIMAL(18,4)  NULL,

	 [PoleGuid] uniqueidentifier  NULL,

	 [PoleNullGuid] uniqueidentifier  NULL,

	 [PoleEnum] VARCHAR(15)  NULL,

	 [PoleChar] TINYINT  NULL,

	 [PoleNullChar] TINYINT  NULL,

	 [FullTypesMaster1_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Salad2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [SaladName] VARCHAR(255)  NULL,

	 [Ingridient2_m0] UNIQUEIDENTIFIER  NULL,

	 [Ingridient2_m1] UNIQUEIDENTIFIER  NULL,

	 [Ingridient1_m0] UNIQUEIDENTIFIER  NULL,

	 [Ingridient1_m1] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [InformationTestClass3] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [StringPropForInfTestClass3] VARCHAR(255)  NULL,

	 [InformationTestClass2] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [CombinedTypesUsageProviderTest] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 [DataObjectForTest_m0] UNIQUEIDENTIFIER  NULL,

	 [TypeUsageProviderTestClass] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ClassWithCaptions] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [InformationTestClass4] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Country2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [CountryName] VARCHAR(255)  NULL,

	 [XCoordinate] INT  NULL,

	 [YCoordinate] INT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [MasterUpdateObjectTest] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [MasterName] VARCHAR(255)  NULL,

	 [Detail] UNIQUEIDENTIFIER  NULL,

	 [AggregatorUpdateObjectTest] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [FullTypesMaster1] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [PoleInt] INT  NULL,

	 [PoleUInt] INT  NULL,

	 [PoleDateTime] DATETIME  NULL,

	 [PoleString] VARCHAR(255)  NULL,

	 [PoleFloat] REAL  NULL,

	 [PoleDouble] FLOAT  NULL,

	 [PoleDecimal] DECIMAL(18,4)  NULL,

	 [PoleBool] BIT  NULL,

	 [PoleNullableInt] INT  NULL,

	 [PoleNullableDecimal] DECIMAL(18,4)  NULL,

	 [PoleNullableDateTime] DATETIME  NULL,

	 [PoleNullInt] INT  NULL,

	 [PoleNullDateTime] DATETIME  NULL,

	 [PoleNullFloat] REAL  NULL,

	 [PoleNullDouble] FLOAT  NULL,

	 [PoleNullDecimal] DECIMAL(18,4)  NULL,

	 [PoleGuid] uniqueidentifier  NULL,

	 [PoleNullGuid] uniqueidentifier  NULL,

	 [PoleEnum] VARCHAR(15)  NULL,

	 [PoleChar] TINYINT  NULL,

	 [PoleNullChar] TINYINT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [AuditAgregatorObject] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Login] VARCHAR(255)  NULL,

	 [Name] VARCHAR(255)  NULL,

	 [Surname] VARCHAR(255)  NULL,

	 [MasterObject] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Territory2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [XCoordinate] INT  NULL,

	 [YCoordinate] INT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [DateField] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Date] DATETIME  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [УчастникХозДог] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [НомУчастнХозДог] INT  NULL,

	 [Статус] VARCHAR(12)  NULL,

	 [Личность_m0] UNIQUEIDENTIFIER  NOT NULL,

	 [ХозДоговор_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Кредит] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ДатаВыдачи] DATETIME  NULL,

	 [СуммаКредита] FLOAT  NULL,

	 [СрокКредита] INT  NULL,

	 [ВидКредита] VARCHAR(15)  NULL,

	 [Клиент] UNIQUEIDENTIFIER  NULL,

	 [ИнспекторПоКред] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Кошка] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Кличка] VARCHAR(255)  NULL,

	 [ДатаРождения] DATETIME  NOT NULL,

	 [Тип] VARCHAR(8)  NOT NULL,

	 [ПородаСтрокой] VARCHAR(255)  NULL,

	 [Агрессивная] BIT  NULL,

	 [КолвоУсовСлева] INT  NULL,

	 [КолвоУсовСправа] INT  NULL,

	 [Ключ] uniqueidentifier  NULL,

	 [Порода] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [FullTypesDetail2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [PoleInt] INT  NULL,

	 [PoleDateTime] DATETIME  NULL,

	 [PoleString] VARCHAR(255)  NULL,

	 [PoleFloat] REAL  NULL,

	 [PoleDouble] FLOAT  NULL,

	 [PoleDecimal] DECIMAL(18,4)  NULL,

	 [PoleBool] BIT  NULL,

	 [PoleNullableInt] INT  NULL,

	 [PoleNullableDecimal] DECIMAL(18,4)  NULL,

	 [PoleNullableDateTime] DATETIME  NULL,

	 [PoleNullInt] INT  NULL,

	 [PoleNullDateTime] DATETIME  NULL,

	 [PoleNullFloat] REAL  NULL,

	 [PoleNullDouble] FLOAT  NULL,

	 [PoleNullDecimal] DECIMAL(18,4)  NULL,

	 [PoleGuid] uniqueidentifier  NULL,

	 [PoleNullGuid] uniqueidentifier  NULL,

	 [PoleEnum] VARCHAR(15)  NULL,

	 [PoleChar] TINYINT  NULL,

	 [PoleNullChar] TINYINT  NULL,

	 [FullTypesMainAgregator] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [CabbagePart2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [PartName] VARCHAR(255)  NULL,

	 [Cabbage] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [DetailUpdateObjectTest] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [DetailName] VARCHAR(255)  NULL,

	 [Master] UNIQUEIDENTIFIER  NULL,

	 [AggregatorUpdateObjectTest] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [SimpleDataObject] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 [Age] INT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ХозДоговор] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [НомХозДоговора] INT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ФайлИдеи] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Файл] NVARCHAR(MAX)  NULL,

	 [Владелец_m0] UNIQUEIDENTIFIER  NOT NULL,

	 [Идея_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [InformationTestClass] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [PublicStringProperty] VARCHAR(255)  NULL,

	 [StringPropertyForInfTestClass] VARCHAR(255)  NULL,

	 [IntPropertyForInfTestClass] INT  NULL,

	 [BoolPropertyForInfTestClass] BIT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ЭтапИсходящегоЗапроса] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Статус] VARCHAR(78)  NULL,

	 [Конфигурация] UNIQUEIDENTIFIER  NOT NULL,

	 [Запросы] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [cla] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [info] VARCHAR(255)  NULL,

	 [parent] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Mailman] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 [Photo] NVARCHAR(MAX)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Страна] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Название] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Конкурс] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Название] VARCHAR(255)  NULL,

	 [Описание] VARCHAR(255)  NULL,

	 [ДатаНачала] DATETIME  NULL,

	 [ДатаОкончания] DATETIME  NULL,

	 [НачалоОценки] DATETIME  NULL,

	 [ОкончаниеОценки] DATETIME  NULL,

	 [Состоятие] VARCHAR(16)  NULL,

	 [Организатор_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ТипПороды] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Название] VARCHAR(255)  NULL,

	 [ДатаРегистрации] DATETIME  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [TypeUsageProviderTestClass] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 [DataObjectForTest_m0] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [InformationTestClass6] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [StringPropForInfTestClass6] VARCHAR(255)  NULL,

	 [ExampleOfClassWithCaptions] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [SomeMasterClass] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [FieldA] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Идея] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Заголовок] VARCHAR(255)  NULL,

	 [Описание] VARCHAR(255)  NULL,

	 [СуммаБаллов] FLOAT  NULL,

	 [Автор_m0] UNIQUEIDENTIFIER  NOT NULL,

	 [Конкурс_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Лапа] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Цвет] VARCHAR(255)  NULL,

	 [Размер] INT  NULL,

	 [ДатаРождения] DATETIME  NULL,

	 [БылиЛиПереломы] BIT  NULL,

	 [Сторона] VARCHAR(11)  NULL,

	 [Номер] INT  NULL,

	 [РазмерDouble] FLOAT  NULL,

	 [РазмерFloat] REAL  NULL,

	 [РазмерNullableInt] INT  NULL,

	 [РазмерDecimal] DECIMAL(18,4)  NULL,

	 [РазмерNullableDecimal] DECIMAL(18,4)  NULL,

	 [РазмерNullableChar] TINYINT  NULL,

	 [ТипЛапы_m0] UNIQUEIDENTIFIER  NULL,

	 [Кошка_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ЗначениеКритер] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Значение] VARCHAR(255)  NULL,

	 [СредОценкаЭксп] FLOAT  NULL,

	 [Критерий_m0] UNIQUEIDENTIFIER  NOT NULL,

	 [Идея_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [CabbageSalad] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [CabbageSaladName] VARCHAR(255)  NULL,

	 [Cabbage1] UNIQUEIDENTIFIER  NULL,

	 [Cabbage2] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [InformationTestClass2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [StringPropertyForInfTestClass2] VARCHAR(255)  NULL,

	 [InformationTestClass_m0] UNIQUEIDENTIFIER  NULL,

	 [InformationTestClass_m1] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Котенок] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [КличкаКотенка] VARCHAR(255)  NULL,

	 [Глупость] INT  NULL,

	 [Кошка] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Region] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [RegionName] VARCHAR(255)  NULL,

	 [Country2_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [AuditClassWithSettings] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [CreateTime] DATETIME  NULL,

	 [Creator] VARCHAR(255)  NULL,

	 [EditTime] DATETIME  NULL,

	 [Editor] VARCHAR(255)  NULL,

	 [Name] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [InformationTestClassChild] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [PublicStringProperty] VARCHAR(255)  NULL,

	 [StringPropertyForInfTestClass] VARCHAR(255)  NULL,

	 [IntPropertyForInfTestClass] INT  NULL,

	 [BoolPropertyForInfTestClass] BIT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [МастерМ] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [FullTypesDetail1] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [PoleInt] INT  NULL,

	 [PoleDateTime] DATETIME  NULL,

	 [PoleString] VARCHAR(255)  NULL,

	 [PoleFloat] REAL  NULL,

	 [PoleDouble] FLOAT  NULL,

	 [PoleDecimal] DECIMAL(18,4)  NULL,

	 [PoleBool] BIT  NULL,

	 [PoleNullableInt] INT  NULL,

	 [PoleNullableDecimal] DECIMAL(18,4)  NULL,

	 [PoleNullableDateTime] DATETIME  NULL,

	 [PoleNullInt] INT  NULL,

	 [PoleNullDateTime] DATETIME  NULL,

	 [PoleNullFloat] REAL  NULL,

	 [PoleNullDouble] FLOAT  NULL,

	 [PoleNullDecimal] DECIMAL(18,4)  NULL,

	 [PoleGuid] uniqueidentifier  NULL,

	 [PoleNullGuid] uniqueidentifier  NULL,

	 [PoleEnum] VARCHAR(15)  NULL,

	 [PoleChar] TINYINT  NULL,

	 [PoleNullChar] TINYINT  NULL,

	 [FullTypesMainAgregator_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Порода] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Название] VARCHAR(255)  NULL,

	 [Ключ] uniqueidentifier  NULL,

	 [ТипПороды] UNIQUEIDENTIFIER  NULL,

	 [Иерархия] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Запрос] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ПервоеДлинноеПолеДляПроверки] BIT  NULL,

	 [ВтороеДлинноеПолеДляПроверки] VARCHAR(78)  NULL,

	 [ПятоеДлинноеПолеДляПроверки] INT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [InformationTestClass4] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [StringPropForInfTestClass4] VARCHAR(255)  NULL,

	 [MasterOfInformationTestClass3] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [DataObjectForTest] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 [Height] INT  NULL,

	 [BirthDate] DATETIME  NULL,

	 [Gender] BIT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ComputedMaster] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [MasterField1] VARCHAR(255)  NULL,

	 [MasterField2] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Личность] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ФИО] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [AuditClassWithDisabledAudit] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [CreateTime] DATETIME  NULL,

	 [Creator] VARCHAR(255)  NULL,

	 [EditTime] DATETIME  NULL,

	 [Editor] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [КритерийОценки] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ПорядковыйНомер] INT  NULL,

	 [Описание] VARCHAR(255)  NULL,

	 [Вес] FLOAT  NULL,

	 [Обязательный] BIT  NULL,

	 [Конкурс_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Place2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [PlaceName] VARCHAR(255)  NULL,

	 [TomorrowTeritory_m0] UNIQUEIDENTIFIER  NULL,

	 [TomorrowTeritory_m1] UNIQUEIDENTIFIER  NULL,

	 [TodayTerritory_m0] UNIQUEIDENTIFIER  NULL,

	 [TodayTerritory_m1] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ТипЛапы] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Название] VARCHAR(255)  NULL,

	 [Актуально] BIT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ИФХозДоговора] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [НомерИФХозДогов] INT  NULL,

	 [ИсточникФинан] UNIQUEIDENTIFIER  NOT NULL,

	 [ХозДоговор_m0] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Homer] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [MasterClass] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [StringMasterProperty] VARCHAR(255)  NULL,

	 [InformationTestClass3_m0] UNIQUEIDENTIFIER  NULL,

	 [InformationTestClass2] UNIQUEIDENTIFIER  NULL,

	 [InformationTestClass_m0] UNIQUEIDENTIFIER  NULL,

	 [InformationTestClass_m1] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Plant2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [NullFileField] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [FileField] NVARCHAR(MAX)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Пользователь] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Логин] VARCHAR(255)  NULL,

	 [ФИО] VARCHAR(255)  NULL,

	 [EMail] VARCHAR(255)  NULL,

	 [ДатаРегистрации] DATETIME  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Human2] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [HumanName] VARCHAR(255)  NULL,

	 [TodayHome_m0] UNIQUEIDENTIFIER  NULL,

	 [TodayHome_m1] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [SomeDetailClass] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [FieldB] VARCHAR(255)  NULL,

	 [ClassA] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMNETLOCKDATA] (

	 [LockKey] VARCHAR(300)  NOT NULL,

	 [UserName] VARCHAR(300)  NOT NULL,

	 [LockDate] DATETIME  NULL,

	 PRIMARY KEY ([LockKey]))


CREATE TABLE [STORMSETTINGS] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Module] varchar(1000)  NULL,

	 [Name] varchar(255)  NULL,

	 [Value] text  NULL,

	 [User] varchar(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMAdvLimit] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [User] varchar(255)  NULL,

	 [Published] bit  NULL,

	 [Module] varchar(255)  NULL,

	 [Name] varchar(255)  NULL,

	 [Value] text  NULL,

	 [HotKeyData] int  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMFILTERSETTING] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Name] varchar(255)  NOT NULL,

	 [DataObjectView] varchar(255)  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMWEBSEARCH] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Name] varchar(255)  NOT NULL,

	 [Order] INT  NOT NULL,

	 [PresentView] varchar(255)  NOT NULL,

	 [DetailedView] varchar(255)  NOT NULL,

	 [FilterSetting_m0] uniqueidentifier  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMFILTERDETAIL] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Caption] varchar(255)  NOT NULL,

	 [DataObjectView] varchar(255)  NOT NULL,

	 [ConnectMasterProp] varchar(255)  NOT NULL,

	 [OwnerConnectProp] varchar(255)  NULL,

	 [FilterSetting_m0] uniqueidentifier  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMFILTERLOOKUP] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [DataObjectType] varchar(255)  NOT NULL,

	 [Container] varchar(255)  NULL,

	 [ContainerTag] varchar(255)  NULL,

	 [FieldsToView] varchar(255)  NULL,

	 [FilterSetting_m0] uniqueidentifier  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [UserSetting] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [AppName] varchar(256)  NULL,

	 [UserName] varchar(512)  NULL,

	 [UserGuid] uniqueidentifier  NULL,

	 [ModuleName] varchar(1024)  NULL,

	 [ModuleGuid] uniqueidentifier  NULL,

	 [SettName] varchar(256)  NULL,

	 [SettGuid] uniqueidentifier  NULL,

	 [SettLastAccessTime] DATETIME  NULL,

	 [StrVal] varchar(256)  NULL,

	 [TxtVal] varchar(max)  NULL,

	 [IntVal] int  NULL,

	 [BoolVal] bit  NULL,

	 [GuidVal] uniqueidentifier  NULL,

	 [DecimalVal] decimal(20,10)  NULL,

	 [DateTimeVal] DATETIME  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ApplicationLog] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Category] varchar(64)  NULL,

	 [EventId] INT  NULL,

	 [Priority] INT  NULL,

	 [Severity] varchar(32)  NULL,

	 [Title] varchar(256)  NULL,

	 [Timestamp] DATETIME  NULL,

	 [MachineName] varchar(32)  NULL,

	 [AppDomainName] varchar(512)  NULL,

	 [ProcessId] varchar(256)  NULL,

	 [ProcessName] varchar(512)  NULL,

	 [ThreadName] varchar(512)  NULL,

	 [Win32ThreadId] varchar(128)  NULL,

	 [Message] varchar(2500)  NULL,

	 [FormattedMessage] varchar(max)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMAG] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Name] varchar(80)  NOT NULL,

	 [Login] varchar(50)  NULL,

	 [Pwd] varchar(50)  NULL,

	 [IsUser] bit  NOT NULL,

	 [IsGroup] bit  NOT NULL,

	 [IsRole] bit  NOT NULL,

	 [ConnString] varchar(255)  NULL,

	 [Enabled] bit  NULL,

	 [Email] varchar(80)  NULL,

	 [Comment] varchar(MAX)  NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMLG] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Group_m0] uniqueidentifier  NOT NULL,

	 [User_m0] uniqueidentifier  NOT NULL,

	 [CreateTime] datetime  NULL,

	 [Creator] varchar(255)  NULL,

	 [EditTime] datetime  NULL,

	 [Editor] varchar(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMAuObjType] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Name] nvarchar(255)  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMAuEntity] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [ObjectPrimaryKey] nvarchar(38)  NOT NULL,

	 [OperationTime] DATETIME  NOT NULL,

	 [OperationType] nvarchar(100)  NOT NULL,

	 [ExecutionResult] nvarchar(12)  NOT NULL,

	 [Source] nvarchar(255)  NOT NULL,

	 [SerializedField] nvarchar(max)  NULL,

	 [User_m0] uniqueidentifier  NOT NULL,

	 [ObjectType_m0] uniqueidentifier  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMAuField] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Field] nvarchar(100)  NOT NULL,

	 [OldValue] nvarchar(max)  NULL,

	 [NewValue] nvarchar(max)  NULL,

	 [MainChange_m0] uniqueidentifier  NULL,

	 [AuditEntity_m0] uniqueidentifier  NOT NULL,

	 PRIMARY KEY ([primaryKey]))




 ALTER TABLE [Apparatus2] ADD CONSTRAINT [Apparatus2_FCountry2_0] FOREIGN KEY ([Maker_m0]) REFERENCES [Country2]
CREATE INDEX Apparatus2_IMaker_m0 on [Apparatus2] ([Maker_m0])

 ALTER TABLE [Apparatus2] ADD CONSTRAINT [Apparatus2_FCountry2_1] FOREIGN KEY ([Exporter_m0]) REFERENCES [Country2]
CREATE INDEX Apparatus2_IExporter_m0 on [Apparatus2] ([Exporter_m0])

 ALTER TABLE [ComputedDetail] ADD CONSTRAINT [ComputedDetail_FComputedMaster_0] FOREIGN KEY ([ComputedMaster]) REFERENCES [ComputedMaster]
CREATE INDEX ComputedDetail_IComputedMaster on [ComputedDetail] ([ComputedMaster])

 ALTER TABLE [Выплаты] ADD CONSTRAINT [Выплаты_FКредит_0] FOREIGN KEY ([Кредит1]) REFERENCES [Кредит]
CREATE INDEX Выплаты_IКредит1 on [Выплаты] ([Кредит1])

 ALTER TABLE [Блоха] ADD CONSTRAINT [Блоха_FМедведь_0] FOREIGN KEY ([МедведьОбитания]) REFERENCES [Медведь]
CREATE INDEX Блоха_IМедведьОбитания on [Блоха] ([МедведьОбитания])

 ALTER TABLE [Этап] ADD CONSTRAINT [Этап_FКонфигурацияЭтапа_0] FOREIGN KEY ([КонфигурацияЭтапа_m0]) REFERENCES [КонфигурацияЭтапа]
CREATE INDEX Этап_IКонфигурацияЭтапа_m0 on [Этап] ([КонфигурацияЭтапа_m0])

 ALTER TABLE [Этап] ADD CONSTRAINT [Этап_FЗапрос_0] FOREIGN KEY ([Запрос]) REFERENCES [Запрос]
CREATE INDEX Этап_IЗапрос on [Этап] ([Запрос])

 ALTER TABLE [AuditMasterObject] ADD CONSTRAINT [AuditMasterObject_FAuditMasterMasterObject_0] FOREIGN KEY ([MasterObject]) REFERENCES [AuditMasterMasterObject]
CREATE INDEX AuditMasterObject_IMasterObject on [AuditMasterObject] ([MasterObject])

 ALTER TABLE [Берлога] ADD CONSTRAINT [Берлога_FЛес_0] FOREIGN KEY ([ЛесРасположения]) REFERENCES [Лес]
CREATE INDEX Берлога_IЛесРасположения on [Берлога] ([ЛесРасположения])

 ALTER TABLE [Берлога] ADD CONSTRAINT [Берлога_FМедведь_0] FOREIGN KEY ([Медведь]) REFERENCES [Медведь]
CREATE INDEX Берлога_IМедведь on [Берлога] ([Медведь])

 ALTER TABLE [ДокККонкурсу] ADD CONSTRAINT [ДокККонкурсу_FКонкурс_0] FOREIGN KEY ([Конкурс_m0]) REFERENCES [Конкурс]
CREATE INDEX ДокККонкурсу_IКонкурс_m0 on [ДокККонкурсу] ([Конкурс_m0])

 ALTER TABLE [clb] ADD CONSTRAINT [clb_Fcla_0] FOREIGN KEY ([ref2]) REFERENCES [cla]
CREATE INDEX clb_Iref2 on [clb] ([ref2])

 ALTER TABLE [clb] ADD CONSTRAINT [clb_Fcla_1] FOREIGN KEY ([ref1]) REFERENCES [cla]
CREATE INDEX clb_Iref1 on [clb] ([ref1])

 ALTER TABLE [ОценкаЭксперта] ADD CONSTRAINT [ОценкаЭксперта_FПользователь_0] FOREIGN KEY ([Эксперт_m0]) REFERENCES [Пользователь]
CREATE INDEX ОценкаЭксперта_IЭксперт_m0 on [ОценкаЭксперта] ([Эксперт_m0])

 ALTER TABLE [ОценкаЭксперта] ADD CONSTRAINT [ОценкаЭксперта_FЗначениеКритер_0] FOREIGN KEY ([ЗначениеКритер]) REFERENCES [ЗначениеКритер]
CREATE INDEX ОценкаЭксперта_IЗначениеКритер on [ОценкаЭксперта] ([ЗначениеКритер])

 ALTER TABLE [ОценкаЭксперта] ADD CONSTRAINT [ОценкаЭксперта_FИдея_0] FOREIGN KEY ([Идея_m0]) REFERENCES [Идея]
CREATE INDEX ОценкаЭксперта_IИдея_m0 on [ОценкаЭксперта] ([Идея_m0])

 ALTER TABLE [Dish2] ADD CONSTRAINT [Dish2_FCabbage2_0] FOREIGN KEY ([MainIngridient_m0]) REFERENCES [Cabbage2]
CREATE INDEX Dish2_IMainIngridient_m0 on [Dish2] ([MainIngridient_m0])

 ALTER TABLE [Dish2] ADD CONSTRAINT [Dish2_FPlant2_0] FOREIGN KEY ([MainIngridient_m1]) REFERENCES [Plant2]
CREATE INDEX Dish2_IMainIngridient_m1 on [Dish2] ([MainIngridient_m1])

 ALTER TABLE [Медведь] ADD CONSTRAINT [Медведь_FМедведь_0] FOREIGN KEY ([Мама]) REFERENCES [Медведь]
CREATE INDEX Медведь_IМама on [Медведь] ([Мама])

 ALTER TABLE [Медведь] ADD CONSTRAINT [Медведь_FЛес_0] FOREIGN KEY ([ЛесОбитания]) REFERENCES [Лес]
CREATE INDEX Медведь_IЛесОбитания on [Медведь] ([ЛесОбитания])

 ALTER TABLE [Медведь] ADD CONSTRAINT [Медведь_FМедведь_1] FOREIGN KEY ([Папа]) REFERENCES [Медведь]
CREATE INDEX Медведь_IПапа on [Медведь] ([Папа])

 ALTER TABLE [Медведь] ADD CONSTRAINT [Медведь_FМедведь_2] FOREIGN KEY ([Друг_m0]) REFERENCES [Медведь]
CREATE INDEX Медведь_IДруг_m0 on [Медведь] ([Друг_m0])

 ALTER TABLE [TypeUsageProviderTestClassChil] ADD CONSTRAINT [TypeUsageProviderTestClassChil_FDataObjectForTest_0] FOREIGN KEY ([DataObjectForTest_m0]) REFERENCES [DataObjectForTest]
CREATE INDEX TypeUsageProviderTestClassChil_IDataObjectForTest_m0 on [TypeUsageProviderTestClassChil] ([DataObjectForTest_m0])

 ALTER TABLE [Soup2] ADD CONSTRAINT [Soup2_FCabbage2_0] FOREIGN KEY ([CabbageType]) REFERENCES [Cabbage2]
CREATE INDEX Soup2_ICabbageType on [Soup2] ([CabbageType])

 ALTER TABLE [Parcel] ADD CONSTRAINT [Parcel_FHomer_0] FOREIGN KEY ([DeliveredByHomer]) REFERENCES [Homer]
CREATE INDEX Parcel_IDeliveredByHomer on [Parcel] ([DeliveredByHomer])

 ALTER TABLE [Parcel] ADD CONSTRAINT [Parcel_FMailman_0] FOREIGN KEY ([DeliveredByMailman]) REFERENCES [Mailman]
CREATE INDEX Parcel_IDeliveredByMailman on [Parcel] ([DeliveredByMailman])

 ALTER TABLE [TestClassA] ADD CONSTRAINT [TestClassA_FМастерМ_0] FOREIGN KEY ([Мастер_m0]) REFERENCES [МастерМ]
CREATE INDEX TestClassA_IМастер_m0 on [TestClassA] ([Мастер_m0])

 ALTER TABLE [TestClassA] ADD CONSTRAINT [TestClassA_FНаследникМ1_0] FOREIGN KEY ([Мастер_m1]) REFERENCES [НаследникМ1]
CREATE INDEX TestClassA_IМастер_m1 on [TestClassA] ([Мастер_m1])

 ALTER TABLE [TestClassA] ADD CONSTRAINT [TestClassA_FНаследникМ2_0] FOREIGN KEY ([Мастер_m2]) REFERENCES [НаследникМ2]
CREATE INDEX TestClassA_IМастер_m2 on [TestClassA] ([Мастер_m2])

 ALTER TABLE [Перелом] ADD CONSTRAINT [Перелом_FЛапа_0] FOREIGN KEY ([Лапа_m0]) REFERENCES [Лапа]
CREATE INDEX Перелом_IЛапа_m0 on [Перелом] ([Лапа_m0])

 ALTER TABLE [Лес] ADD CONSTRAINT [Лес_FСтрана_0] FOREIGN KEY ([Страна]) REFERENCES [Страна]
CREATE INDEX Лес_IСтрана on [Лес] ([Страна])

 ALTER TABLE [DetailClass] ADD CONSTRAINT [DetailClass_FMasterClass_0] FOREIGN KEY ([MasterClass_m0]) REFERENCES [MasterClass]
CREATE INDEX DetailClass_IMasterClass_m0 on [DetailClass] ([MasterClass_m0])

 ALTER TABLE [DetailClass] ADD CONSTRAINT [DetailClass_FMasterClass_1] FOREIGN KEY ([MasterClass_m1]) REFERENCES [MasterClass]
CREATE INDEX DetailClass_IMasterClass_m1 on [DetailClass] ([MasterClass_m1])

 ALTER TABLE [AggregatorUpdateObjectTest] ADD CONSTRAINT [AggregatorUpdateObjectTest_FDetailUpdateObjectTest_0] FOREIGN KEY ([Detail]) REFERENCES [DetailUpdateObjectTest]
CREATE INDEX AggregatorUpdateObjectTest_IDetail on [AggregatorUpdateObjectTest] ([Detail])

 ALTER TABLE [Adress2] ADD CONSTRAINT [Adress2_FCountry2_0] FOREIGN KEY ([Country_m0]) REFERENCES [Country2]
CREATE INDEX Adress2_ICountry_m0 on [Adress2] ([Country_m0])

 ALTER TABLE [FullTypesMainAgregator] ADD CONSTRAINT [FullTypesMainAgregator_FFullTypesMaster1_0] FOREIGN KEY ([FullTypesMaster1_m0]) REFERENCES [FullTypesMaster1]
CREATE INDEX FullTypesMainAgregator_IFullTypesMaster1_m0 on [FullTypesMainAgregator] ([FullTypesMaster1_m0])

 ALTER TABLE [Salad2] ADD CONSTRAINT [Salad2_FCabbage2_0] FOREIGN KEY ([Ingridient2_m0]) REFERENCES [Cabbage2]
CREATE INDEX Salad2_IIngridient2_m0 on [Salad2] ([Ingridient2_m0])

 ALTER TABLE [Salad2] ADD CONSTRAINT [Salad2_FPlant2_0] FOREIGN KEY ([Ingridient2_m1]) REFERENCES [Plant2]
CREATE INDEX Salad2_IIngridient2_m1 on [Salad2] ([Ingridient2_m1])

 ALTER TABLE [Salad2] ADD CONSTRAINT [Salad2_FCabbage2_1] FOREIGN KEY ([Ingridient1_m0]) REFERENCES [Cabbage2]
CREATE INDEX Salad2_IIngridient1_m0 on [Salad2] ([Ingridient1_m0])

 ALTER TABLE [Salad2] ADD CONSTRAINT [Salad2_FPlant2_1] FOREIGN KEY ([Ingridient1_m1]) REFERENCES [Plant2]
CREATE INDEX Salad2_IIngridient1_m1 on [Salad2] ([Ingridient1_m1])

 ALTER TABLE [InformationTestClass3] ADD CONSTRAINT [InformationTestClass3_FInformationTestClass2_0] FOREIGN KEY ([InformationTestClass2]) REFERENCES [InformationTestClass2]
CREATE INDEX InformationTestClass3_IInformationTestClass2 on [InformationTestClass3] ([InformationTestClass2])

 ALTER TABLE [CombinedTypesUsageProviderTest] ADD CONSTRAINT [CombinedTypesUsageProviderTest_FDataObjectForTest_0] FOREIGN KEY ([DataObjectForTest_m0]) REFERENCES [DataObjectForTest]
CREATE INDEX CombinedTypesUsageProviderTest_IDataObjectForTest_m0 on [CombinedTypesUsageProviderTest] ([DataObjectForTest_m0])

 ALTER TABLE [CombinedTypesUsageProviderTest] ADD CONSTRAINT [CombinedTypesUsageProviderTest_FTypeUsageProviderTestClass_0] FOREIGN KEY ([TypeUsageProviderTestClass]) REFERENCES [TypeUsageProviderTestClass]
CREATE INDEX CombinedTypesUsageProviderTest_ITypeUsageProviderTestClass on [CombinedTypesUsageProviderTest] ([TypeUsageProviderTestClass])

 ALTER TABLE [ClassWithCaptions] ADD CONSTRAINT [ClassWithCaptions_FInformationTestClass4_0] FOREIGN KEY ([InformationTestClass4]) REFERENCES [InformationTestClass4]
CREATE INDEX ClassWithCaptions_IInformationTestClass4 on [ClassWithCaptions] ([InformationTestClass4])

 ALTER TABLE [MasterUpdateObjectTest] ADD CONSTRAINT [MasterUpdateObjectTest_FDetailUpdateObjectTest_0] FOREIGN KEY ([Detail]) REFERENCES [DetailUpdateObjectTest]
CREATE INDEX MasterUpdateObjectTest_IDetail on [MasterUpdateObjectTest] ([Detail])

 ALTER TABLE [MasterUpdateObjectTest] ADD CONSTRAINT [MasterUpdateObjectTest_FAggregatorUpdateObjectTest_0] FOREIGN KEY ([AggregatorUpdateObjectTest]) REFERENCES [AggregatorUpdateObjectTest]
CREATE INDEX MasterUpdateObjectTest_IAggregatorUpdateObjectTest on [MasterUpdateObjectTest] ([AggregatorUpdateObjectTest])

 ALTER TABLE [AuditAgregatorObject] ADD CONSTRAINT [AuditAgregatorObject_FAuditMasterObject_0] FOREIGN KEY ([MasterObject]) REFERENCES [AuditMasterObject]
CREATE INDEX AuditAgregatorObject_IMasterObject on [AuditAgregatorObject] ([MasterObject])

 ALTER TABLE [УчастникХозДог] ADD CONSTRAINT [УчастникХозДог_FЛичность_0] FOREIGN KEY ([Личность_m0]) REFERENCES [Личность]
CREATE INDEX УчастникХозДог_IЛичность_m0 on [УчастникХозДог] ([Личность_m0])

 ALTER TABLE [УчастникХозДог] ADD CONSTRAINT [УчастникХозДог_FХозДоговор_0] FOREIGN KEY ([ХозДоговор_m0]) REFERENCES [ХозДоговор]
CREATE INDEX УчастникХозДог_IХозДоговор_m0 on [УчастникХозДог] ([ХозДоговор_m0])

 ALTER TABLE [Кредит] ADD CONSTRAINT [Кредит_FКлиент_0] FOREIGN KEY ([Клиент]) REFERENCES [Клиент]
CREATE INDEX Кредит_IКлиент on [Кредит] ([Клиент])

 ALTER TABLE [Кредит] ADD CONSTRAINT [Кредит_FИнспПоКредиту_0] FOREIGN KEY ([ИнспекторПоКред]) REFERENCES [ИнспПоКредиту]
CREATE INDEX Кредит_IИнспекторПоКред on [Кредит] ([ИнспекторПоКред])

 ALTER TABLE [Кошка] ADD CONSTRAINT [Кошка_FПорода_0] FOREIGN KEY ([Порода]) REFERENCES [Порода]
CREATE INDEX Кошка_IПорода on [Кошка] ([Порода])

 ALTER TABLE [FullTypesDetail2] ADD CONSTRAINT [FullTypesDetail2_FFullTypesMainAgregator_0] FOREIGN KEY ([FullTypesMainAgregator]) REFERENCES [FullTypesMainAgregator]
CREATE INDEX FullTypesDetail2_IFullTypesMainAgregator on [FullTypesDetail2] ([FullTypesMainAgregator])

 ALTER TABLE [CabbagePart2] ADD CONSTRAINT [CabbagePart2_FCabbage2_0] FOREIGN KEY ([Cabbage]) REFERENCES [Cabbage2]
CREATE INDEX CabbagePart2_ICabbage on [CabbagePart2] ([Cabbage])

 ALTER TABLE [DetailUpdateObjectTest] ADD CONSTRAINT [DetailUpdateObjectTest_FMasterUpdateObjectTest_0] FOREIGN KEY ([Master]) REFERENCES [MasterUpdateObjectTest]
CREATE INDEX DetailUpdateObjectTest_IMaster on [DetailUpdateObjectTest] ([Master])

 ALTER TABLE [DetailUpdateObjectTest] ADD CONSTRAINT [DetailUpdateObjectTest_FAggregatorUpdateObjectTest_0] FOREIGN KEY ([AggregatorUpdateObjectTest]) REFERENCES [AggregatorUpdateObjectTest]
CREATE INDEX DetailUpdateObjectTest_IAggregatorUpdateObjectTest on [DetailUpdateObjectTest] ([AggregatorUpdateObjectTest])

 ALTER TABLE [ФайлИдеи] ADD CONSTRAINT [ФайлИдеи_FПользователь_0] FOREIGN KEY ([Владелец_m0]) REFERENCES [Пользователь]
CREATE INDEX ФайлИдеи_IВладелец_m0 on [ФайлИдеи] ([Владелец_m0])

 ALTER TABLE [ФайлИдеи] ADD CONSTRAINT [ФайлИдеи_FИдея_0] FOREIGN KEY ([Идея_m0]) REFERENCES [Идея]
CREATE INDEX ФайлИдеи_IИдея_m0 on [ФайлИдеи] ([Идея_m0])

 ALTER TABLE [ЭтапИсходящегоЗапроса] ADD CONSTRAINT [ЭтапИсходящегоЗапроса_FКонфигурацияЗапроса_0] FOREIGN KEY ([Конфигурация]) REFERENCES [КонфигурацияЗапроса]
CREATE INDEX ЭтапИсходящегоЗапроса_IКонфигурация on [ЭтапИсходящегоЗапроса] ([Конфигурация])

 ALTER TABLE [ЭтапИсходящегоЗапроса] ADD CONSTRAINT [ЭтапИсходящегоЗапроса_FИсходящийЗапрос_0] FOREIGN KEY ([Запросы]) REFERENCES [ИсходящийЗапрос]
CREATE INDEX ЭтапИсходящегоЗапроса_IЗапросы on [ЭтапИсходящегоЗапроса] ([Запросы])

 ALTER TABLE [cla] ADD CONSTRAINT [cla_Fclb_0] FOREIGN KEY ([parent]) REFERENCES [clb]
CREATE INDEX cla_Iparent on [cla] ([parent])

 ALTER TABLE [Конкурс] ADD CONSTRAINT [Конкурс_FПользователь_0] FOREIGN KEY ([Организатор_m0]) REFERENCES [Пользователь]
CREATE INDEX Конкурс_IОрганизатор_m0 on [Конкурс] ([Организатор_m0])

 ALTER TABLE [TypeUsageProviderTestClass] ADD CONSTRAINT [TypeUsageProviderTestClass_FDataObjectForTest_0] FOREIGN KEY ([DataObjectForTest_m0]) REFERENCES [DataObjectForTest]
CREATE INDEX TypeUsageProviderTestClass_IDataObjectForTest_m0 on [TypeUsageProviderTestClass] ([DataObjectForTest_m0])

 ALTER TABLE [InformationTestClass6] ADD CONSTRAINT [InformationTestClass6_FClassWithCaptions_0] FOREIGN KEY ([ExampleOfClassWithCaptions]) REFERENCES [ClassWithCaptions]
CREATE INDEX InformationTestClass6_IExampleOfClassWithCaptions on [InformationTestClass6] ([ExampleOfClassWithCaptions])

 ALTER TABLE [Идея] ADD CONSTRAINT [Идея_FПользователь_0] FOREIGN KEY ([Автор_m0]) REFERENCES [Пользователь]
CREATE INDEX Идея_IАвтор_m0 on [Идея] ([Автор_m0])

 ALTER TABLE [Идея] ADD CONSTRAINT [Идея_FКонкурс_0] FOREIGN KEY ([Конкурс_m0]) REFERENCES [Конкурс]
CREATE INDEX Идея_IКонкурс_m0 on [Идея] ([Конкурс_m0])

 ALTER TABLE [Лапа] ADD CONSTRAINT [Лапа_FТипЛапы_0] FOREIGN KEY ([ТипЛапы_m0]) REFERENCES [ТипЛапы]
CREATE INDEX Лапа_IТипЛапы_m0 on [Лапа] ([ТипЛапы_m0])

 ALTER TABLE [Лапа] ADD CONSTRAINT [Лапа_FКошка_0] FOREIGN KEY ([Кошка_m0]) REFERENCES [Кошка]
CREATE INDEX Лапа_IКошка_m0 on [Лапа] ([Кошка_m0])

 ALTER TABLE [ЗначениеКритер] ADD CONSTRAINT [ЗначениеКритер_FКритерийОценки_0] FOREIGN KEY ([Критерий_m0]) REFERENCES [КритерийОценки]
CREATE INDEX ЗначениеКритер_IКритерий_m0 on [ЗначениеКритер] ([Критерий_m0])

 ALTER TABLE [ЗначениеКритер] ADD CONSTRAINT [ЗначениеКритер_FИдея_0] FOREIGN KEY ([Идея_m0]) REFERENCES [Идея]
CREATE INDEX ЗначениеКритер_IИдея_m0 on [ЗначениеКритер] ([Идея_m0])

 ALTER TABLE [CabbageSalad] ADD CONSTRAINT [CabbageSalad_FCabbage2_0] FOREIGN KEY ([Cabbage1]) REFERENCES [Cabbage2]
CREATE INDEX CabbageSalad_ICabbage1 on [CabbageSalad] ([Cabbage1])

 ALTER TABLE [CabbageSalad] ADD CONSTRAINT [CabbageSalad_FCabbage2_1] FOREIGN KEY ([Cabbage2]) REFERENCES [Cabbage2]
CREATE INDEX CabbageSalad_ICabbage2 on [CabbageSalad] ([Cabbage2])

 ALTER TABLE [InformationTestClass2] ADD CONSTRAINT [InformationTestClass2_FInformationTestClass_0] FOREIGN KEY ([InformationTestClass_m0]) REFERENCES [InformationTestClass]
CREATE INDEX InformationTestClass2_IInformationTestClass_m0 on [InformationTestClass2] ([InformationTestClass_m0])

 ALTER TABLE [InformationTestClass2] ADD CONSTRAINT [InformationTestClass2_FInformationTestClassChild_0] FOREIGN KEY ([InformationTestClass_m1]) REFERENCES [InformationTestClassChild]
CREATE INDEX InformationTestClass2_IInformationTestClass_m1 on [InformationTestClass2] ([InformationTestClass_m1])

 ALTER TABLE [Котенок] ADD CONSTRAINT [Котенок_FКошка_0] FOREIGN KEY ([Кошка]) REFERENCES [Кошка]
CREATE INDEX Котенок_IКошка on [Котенок] ([Кошка])

 ALTER TABLE [Region] ADD CONSTRAINT [Region_FCountry2_0] FOREIGN KEY ([Country2_m0]) REFERENCES [Country2]
CREATE INDEX Region_ICountry2_m0 on [Region] ([Country2_m0])

 ALTER TABLE [FullTypesDetail1] ADD CONSTRAINT [FullTypesDetail1_FFullTypesMainAgregator_0] FOREIGN KEY ([FullTypesMainAgregator_m0]) REFERENCES [FullTypesMainAgregator]
CREATE INDEX FullTypesDetail1_IFullTypesMainAgregator_m0 on [FullTypesDetail1] ([FullTypesMainAgregator_m0])

 ALTER TABLE [Порода] ADD CONSTRAINT [Порода_FТипПороды_0] FOREIGN KEY ([ТипПороды]) REFERENCES [ТипПороды]
CREATE INDEX Порода_IТипПороды on [Порода] ([ТипПороды])

 ALTER TABLE [Порода] ADD CONSTRAINT [Порода_FПорода_0] FOREIGN KEY ([Иерархия]) REFERENCES [Порода]
CREATE INDEX Порода_IИерархия on [Порода] ([Иерархия])

 ALTER TABLE [InformationTestClass4] ADD CONSTRAINT [InformationTestClass4_FInformationTestClass3_0] FOREIGN KEY ([MasterOfInformationTestClass3]) REFERENCES [InformationTestClass3]
CREATE INDEX InformationTestClass4_IMasterOfInformationTestClass3 on [InformationTestClass4] ([MasterOfInformationTestClass3])

 ALTER TABLE [КритерийОценки] ADD CONSTRAINT [КритерийОценки_FКонкурс_0] FOREIGN KEY ([Конкурс_m0]) REFERENCES [Конкурс]
CREATE INDEX КритерийОценки_IКонкурс_m0 on [КритерийОценки] ([Конкурс_m0])

 ALTER TABLE [Place2] ADD CONSTRAINT [Place2_FCountry2_0] FOREIGN KEY ([TomorrowTeritory_m0]) REFERENCES [Country2]
CREATE INDEX Place2_ITomorrowTeritory_m0 on [Place2] ([TomorrowTeritory_m0])

 ALTER TABLE [Place2] ADD CONSTRAINT [Place2_FTerritory2_0] FOREIGN KEY ([TomorrowTeritory_m1]) REFERENCES [Territory2]
CREATE INDEX Place2_ITomorrowTeritory_m1 on [Place2] ([TomorrowTeritory_m1])

 ALTER TABLE [Place2] ADD CONSTRAINT [Place2_FCountry2_1] FOREIGN KEY ([TodayTerritory_m0]) REFERENCES [Country2]
CREATE INDEX Place2_ITodayTerritory_m0 on [Place2] ([TodayTerritory_m0])

 ALTER TABLE [Place2] ADD CONSTRAINT [Place2_FTerritory2_1] FOREIGN KEY ([TodayTerritory_m1]) REFERENCES [Territory2]
CREATE INDEX Place2_ITodayTerritory_m1 on [Place2] ([TodayTerritory_m1])

 ALTER TABLE [ИФХозДоговора] ADD CONSTRAINT [ИФХозДоговора_FИсточникФинанс_0] FOREIGN KEY ([ИсточникФинан]) REFERENCES [ИсточникФинанс]
CREATE INDEX ИФХозДоговора_IИсточникФинан on [ИФХозДоговора] ([ИсточникФинан])

 ALTER TABLE [ИФХозДоговора] ADD CONSTRAINT [ИФХозДоговора_FХозДоговор_0] FOREIGN KEY ([ХозДоговор_m0]) REFERENCES [ХозДоговор]
CREATE INDEX ИФХозДоговора_IХозДоговор_m0 on [ИФХозДоговора] ([ХозДоговор_m0])

 ALTER TABLE [MasterClass] ADD CONSTRAINT [MasterClass_FInformationTestClass3_0] FOREIGN KEY ([InformationTestClass3_m0]) REFERENCES [InformationTestClass3]
CREATE INDEX MasterClass_IInformationTestClass3_m0 on [MasterClass] ([InformationTestClass3_m0])

 ALTER TABLE [MasterClass] ADD CONSTRAINT [MasterClass_FInformationTestClass2_0] FOREIGN KEY ([InformationTestClass2]) REFERENCES [InformationTestClass2]
CREATE INDEX MasterClass_IInformationTestClass2 on [MasterClass] ([InformationTestClass2])

 ALTER TABLE [MasterClass] ADD CONSTRAINT [MasterClass_FInformationTestClass_0] FOREIGN KEY ([InformationTestClass_m0]) REFERENCES [InformationTestClass]
CREATE INDEX MasterClass_IInformationTestClass_m0 on [MasterClass] ([InformationTestClass_m0])

 ALTER TABLE [MasterClass] ADD CONSTRAINT [MasterClass_FInformationTestClassChild_0] FOREIGN KEY ([InformationTestClass_m1]) REFERENCES [InformationTestClassChild]
CREATE INDEX MasterClass_IInformationTestClass_m1 on [MasterClass] ([InformationTestClass_m1])

 ALTER TABLE [Human2] ADD CONSTRAINT [Human2_FCountry2_0] FOREIGN KEY ([TodayHome_m0]) REFERENCES [Country2]
CREATE INDEX Human2_ITodayHome_m0 on [Human2] ([TodayHome_m0])

 ALTER TABLE [Human2] ADD CONSTRAINT [Human2_FTerritory2_0] FOREIGN KEY ([TodayHome_m1]) REFERENCES [Territory2]
CREATE INDEX Human2_ITodayHome_m1 on [Human2] ([TodayHome_m1])

 ALTER TABLE [SomeDetailClass] ADD CONSTRAINT [SomeDetailClass_FSomeMasterClass_0] FOREIGN KEY ([ClassA]) REFERENCES [SomeMasterClass]
CREATE INDEX SomeDetailClass_IClassA on [SomeDetailClass] ([ClassA])

 ALTER TABLE [STORMWEBSEARCH] ADD CONSTRAINT [STORMWEBSEARCH_FSTORMFILTERSETTING_0] FOREIGN KEY ([FilterSetting_m0]) REFERENCES [STORMFILTERSETTING]

 ALTER TABLE [STORMFILTERDETAIL] ADD CONSTRAINT [STORMFILTERDETAIL_FSTORMFILTERSETTING_0] FOREIGN KEY ([FilterSetting_m0]) REFERENCES [STORMFILTERSETTING]

 ALTER TABLE [STORMFILTERLOOKUP] ADD CONSTRAINT [STORMFILTERLOOKUP_FSTORMFILTERSETTING_0] FOREIGN KEY ([FilterSetting_m0]) REFERENCES [STORMFILTERSETTING]

 ALTER TABLE [STORMLG] ADD CONSTRAINT [STORMLG_FSTORMAG_0] FOREIGN KEY ([Group_m0]) REFERENCES [STORMAG]

 ALTER TABLE [STORMLG] ADD CONSTRAINT [STORMLG_FSTORMAG_1] FOREIGN KEY ([User_m0]) REFERENCES [STORMAG]

 ALTER TABLE [STORMAuEntity] ADD CONSTRAINT [STORMAuEntity_FSTORMAG_0] FOREIGN KEY ([User_m0]) REFERENCES [STORMAG]

 ALTER TABLE [STORMAuEntity] ADD CONSTRAINT [STORMAuEntity_FSTORMAuObjType_0] FOREIGN KEY ([ObjectType_m0]) REFERENCES [STORMAuObjType]

 ALTER TABLE [STORMAuField] ADD CONSTRAINT [STORMAuField_FSTORMAuField_0] FOREIGN KEY ([MainChange_m0]) REFERENCES [STORMAuField]

 ALTER TABLE [STORMAuField] ADD CONSTRAINT [STORMAuField_FSTORMAuEntity_0] FOREIGN KEY ([AuditEntity_m0]) REFERENCES [STORMAuEntity]

