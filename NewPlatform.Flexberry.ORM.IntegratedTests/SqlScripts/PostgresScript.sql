



CREATE TABLE AuditMasterMasterObject (

 primaryKey UUID NOT NULL,

 Login VARCHAR(255) NULL,

 Name VARCHAR(255) NULL,

 Surname VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ИсточникФинанс (

 primaryKey UUID NOT NULL,

 НомИсточникаФин INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Apparatus2 (

 primaryKey UUID NOT NULL,

 ApparatusName VARCHAR(255) NULL,

 Maker_m0 UUID NULL,

 Exporter_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ComputedDetail (

 primaryKey UUID NOT NULL,

 DetailField1 VARCHAR(255) NULL,

 DetailField2 VARCHAR(255) NULL,

 ComputedMaster UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AuditClassWithoutSettings (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Выплаты (

 primaryKey UUID NOT NULL,

 ДатаВыплаты TIMESTAMP(3) NULL,

 СуммаВыплаты DOUBLE PRECISION NULL,

 Кредит1 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Блоха (

 primaryKey UUID NOT NULL,

 Кличка VARCHAR(255) NULL,

 МедведьОбитания UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Этап (

 primaryKey UUID NOT NULL,

 Статус VARCHAR(78) NULL,

 КонфигурацияЭтапа_m0 UUID NOT NULL,

 Запрос UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ИсходящийЗапрос (

 primaryKey UUID NOT NULL,

 ПервоеДлинноеПолеДляПроверки BOOLEAN NULL,

 ВтороеДлинноеПолеДляПроверки VARCHAR(78) NULL,

 ПятоеДлинноеПолеДляПроверки INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AuditMasterObject (

 primaryKey UUID NOT NULL,

 Login VARCHAR(255) NULL,

 Name VARCHAR(255) NULL,

 Surname VARCHAR(255) NULL,

 MasterObject UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Берлога (

 primaryKey UUID NOT NULL,

 Наименование VARCHAR(255) NULL,

 Комфортность INT NULL,

 Заброшена BOOLEAN NULL,

 ЛесРасположения UUID NULL,

 Медведь UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Cabbage2 (

 primaryKey UUID NOT NULL,

 Type VARCHAR(255) NULL,

 Name VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE TypeNameUsageProviderTestClass (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE НаследникМ2 (

 primaryKey UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ИнспПоКредиту (

 primaryKey UUID NOT NULL,

 ФИО VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ДокККонкурсу (

 primaryKey UUID NOT NULL,

 Файл TEXT NULL,

 Конкурс_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE clb (

 primaryKey UUID NOT NULL,

 ref2 UUID NULL,

 ref1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE DataObjectWithKeyGuid (

 primaryKey UUID NOT NULL,

 LinkToMaster1 UUID NULL,

 LinkToMaster2 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ОценкаЭксперта (

 primaryKey UUID NOT NULL,

 ЗначениеОценки DOUBLE PRECISION NULL,

 Комментарий VARCHAR(255) NULL,

 Эксперт_m0 UUID NOT NULL,

 ЗначениеКритер UUID NOT NULL,

 Идея_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Dish2 (

 primaryKey UUID NOT NULL,

 DishName VARCHAR(255) NULL,

 MainIngridient_m0 UUID NULL,

 MainIngridient_m1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Медведь (

 primaryKey UUID NOT NULL,

 ПорядковыйНомер INT NULL,

 Вес INT NULL,

 ЦветГлаз VARCHAR(255) NULL,

 Пол VARCHAR(7) NULL,

 ДатаРождения TIMESTAMP(3) NULL,

 Мама UUID NULL,

 ЛесОбитания UUID NULL,

 Папа UUID NULL,

 Друг_m0 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE TypeUsageProviderTestClassChil (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 DataObjectForTest_m0 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Soup2 (

 primaryKey UUID NOT NULL,

 SoupName VARCHAR(255) NULL,

 CabbageType UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE StoredClass (

 primaryKey UUID NOT NULL,

 StoredProperty VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Клиент (

 primaryKey UUID NOT NULL,

 ФИО VARCHAR(255) NULL,

 Прописка VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ForKeyStorageTest (

 StorageForKey VARCHAR(255) NOT NULL,

 PRIMARY KEY (StorageForKey));


CREATE TABLE КонфигурацияЭтапа (

 primaryKey UUID NOT NULL,

 ТретьеДлинноеПолеДляПроверки BOOLEAN NULL,

 ЧетвертоеДлинноеПолеДляПроверки INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE TestClassA (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 Value INT NULL,

 Мастер_m0 UUID NULL,

 Мастер_m1 UUID NULL,

 Мастер_m2 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Перелом (

 primaryKey UUID NOT NULL,

 Дата TIMESTAMP(3) NULL,

 Тип VARCHAR(8) NULL,

 Лапа_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE НаследникМ1 (

 primaryKey UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Лес (

 primaryKey UUID NOT NULL,

 Название VARCHAR(255) NULL,

 Площадь INT NULL,

 Заповедник BOOLEAN NULL,

 ДатаПослОсмотр TIMESTAMP(3) NULL,

 Страна UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE DetailClass (

 primaryKey UUID NOT NULL,

 Detailproperty VARCHAR(255) NULL,

 MasterClass_m0 UUID NULL,

 MasterClass_m1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AggregatorUpdateObjectTest (

 primaryKey UUID NOT NULL,

 AggregatorName VARCHAR(255) NULL,

 Detail UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE КонфигурацияЗапроса (

 primaryKey UUID NOT NULL,

 ТретьеДлинноеПолеДляПроверки BOOLEAN NULL,

 ЧетвертоеДлинноеПолеДляПроверки INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Adress2 (

 primaryKey UUID NOT NULL,

 HomeNumber INT NULL,

 Country_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE FullTypesMainAgregator (

 primaryKey UUID NOT NULL,

 PoleInt INT NULL,

 PoleDateTime TIMESTAMP(3) NULL,

 PoleString VARCHAR(255) NULL,

 PoleFloat REAL NULL,

 PoleDouble DOUBLE PRECISION NULL,

 PoleDecimal DECIMAL NULL,

 PoleBool BOOLEAN NULL,

 PoleNullableInt INT NULL,

 PoleNullableDecimal DECIMAL NULL,

 PoleNullableDateTime TIMESTAMP(3) NULL,

 PoleNullInt INT NULL,

 PoleNullDateTime TIMESTAMP(3) NULL,

 PoleNullFloat REAL NULL,

 PoleNullDouble DOUBLE PRECISION NULL,

 PoleNullDecimal DECIMAL NULL,

 PoleGuid UUID NULL,

 PoleNullGuid UUID NULL,

 PoleEnum VARCHAR(15) NULL,

 PoleChar SMALLINT NULL,

 PoleNullChar SMALLINT NULL,

 FullTypesMaster1_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Salad2 (

 primaryKey UUID NOT NULL,

 SaladName VARCHAR(255) NULL,

 Ingridient2_m0 UUID NULL,

 Ingridient2_m1 UUID NULL,

 Ingridient1_m0 UUID NULL,

 Ingridient1_m1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass3 (

 primaryKey UUID NOT NULL,

 StringPropForInfTestClass3 VARCHAR(255) NULL,

 InformationTestClass2 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE CombinedTypesUsageProviderTest (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 DataObjectForTest_m0 UUID NULL,

 TypeUsageProviderTestClass UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ClassWithCaptions (

 primaryKey UUID NOT NULL,

 InformationTestClass4 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Country2 (

 primaryKey UUID NOT NULL,

 CountryName VARCHAR(255) NULL,

 XCoordinate INT NULL,

 YCoordinate INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE MasterUpdateObjectTest (

 primaryKey UUID NOT NULL,

 MasterName VARCHAR(255) NULL,

 Detail UUID NULL,

 AggregatorUpdateObjectTest UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE FullTypesMaster1 (

 primaryKey UUID NOT NULL,

 PoleInt INT NULL,

 PoleUInt INT NULL,

 PoleDateTime TIMESTAMP(3) NULL,

 PoleString VARCHAR(255) NULL,

 PoleFloat REAL NULL,

 PoleDouble DOUBLE PRECISION NULL,

 PoleDecimal DECIMAL NULL,

 PoleBool BOOLEAN NULL,

 PoleNullableInt INT NULL,

 PoleNullableDecimal DECIMAL NULL,

 PoleNullableDateTime TIMESTAMP(3) NULL,

 PoleNullInt INT NULL,

 PoleNullDateTime TIMESTAMP(3) NULL,

 PoleNullFloat REAL NULL,

 PoleNullDouble DOUBLE PRECISION NULL,

 PoleNullDecimal DECIMAL NULL,

 PoleGuid UUID NULL,

 PoleNullGuid UUID NULL,

 PoleEnum VARCHAR(15) NULL,

 PoleChar SMALLINT NULL,

 PoleNullChar SMALLINT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AuditAgregatorObject (

 primaryKey UUID NOT NULL,

 Login VARCHAR(255) NULL,

 Name VARCHAR(255) NULL,

 Surname VARCHAR(255) NULL,

 MasterObject UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Territory2 (

 primaryKey UUID NOT NULL,

 XCoordinate INT NULL,

 YCoordinate INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE DateField (

 primaryKey UUID NOT NULL,

 Date TIMESTAMP(3) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE УчастникХозДог (

 primaryKey UUID NOT NULL,

 НомУчастнХозДог INT NULL,

 Статус VARCHAR(12) NULL,

 Личность_m0 UUID NOT NULL,

 ХозДоговор_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Кредит (

 primaryKey UUID NOT NULL,

 ДатаВыдачи TIMESTAMP(3) NULL,

 СуммаКредита DOUBLE PRECISION NULL,

 СрокКредита INT NULL,

 ВидКредита VARCHAR(15) NULL,

 Клиент UUID NULL,

 ИнспекторПоКред UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Кошка (

 primaryKey UUID NOT NULL,

 Кличка VARCHAR(255) NULL,

 ДатаРождения TIMESTAMP(3) NOT NULL,

 Тип VARCHAR(8) NOT NULL,

 ПородаСтрокой VARCHAR(255) NULL,

 Агрессивная BOOLEAN NULL,

 КолвоУсовСлева INT NULL,

 КолвоУсовСправа INT NULL,

 Ключ UUID NULL,

 Порода UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE FullTypesDetail2 (

 primaryKey UUID NOT NULL,

 PoleInt INT NULL,

 PoleDateTime TIMESTAMP(3) NULL,

 PoleString VARCHAR(255) NULL,

 PoleFloat REAL NULL,

 PoleDouble DOUBLE PRECISION NULL,

 PoleDecimal DECIMAL NULL,

 PoleBool BOOLEAN NULL,

 PoleNullableInt INT NULL,

 PoleNullableDecimal DECIMAL NULL,

 PoleNullableDateTime TIMESTAMP(3) NULL,

 PoleNullInt INT NULL,

 PoleNullDateTime TIMESTAMP(3) NULL,

 PoleNullFloat REAL NULL,

 PoleNullDouble DOUBLE PRECISION NULL,

 PoleNullDecimal DECIMAL NULL,

 PoleGuid UUID NULL,

 PoleNullGuid UUID NULL,

 PoleEnum VARCHAR(15) NULL,

 PoleChar SMALLINT NULL,

 PoleNullChar SMALLINT NULL,

 FullTypesMainAgregator UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE CabbagePart2 (

 primaryKey UUID NOT NULL,

 PartName VARCHAR(255) NULL,

 Cabbage UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE DetailUpdateObjectTest (

 primaryKey UUID NOT NULL,

 DetailName VARCHAR(255) NULL,

 Master UUID NULL,

 AggregatorUpdateObjectTest UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE SimpleDataObject (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 Age INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ХозДоговор (

 primaryKey UUID NOT NULL,

 НомХозДоговора INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ФайлИдеи (

 primaryKey UUID NOT NULL,

 Файл TEXT NULL,

 Владелец_m0 UUID NOT NULL,

 Идея_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass (

 primaryKey UUID NOT NULL,

 PublicStringProperty VARCHAR(255) NULL,

 StringPropertyForInfTestClass VARCHAR(255) NULL,

 IntPropertyForInfTestClass INT NULL,

 BoolPropertyForInfTestClass BOOLEAN NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ЭтапИсходящегоЗапроса (

 primaryKey UUID NOT NULL,

 Статус VARCHAR(78) NULL,

 Конфигурация UUID NOT NULL,

 Запросы UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE cla (

 primaryKey UUID NOT NULL,

 info VARCHAR(255) NULL,

 parent UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Mailman (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 Photo NEWPLATFORM.FLEXBERRY.ORM.TESTS.FILEFORTESTS NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Страна (

 primaryKey UUID NOT NULL,

 Название VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Конкурс (

 primaryKey UUID NOT NULL,

 Название VARCHAR(255) NULL,

 Описание VARCHAR(255) NULL,

 ДатаНачала TIMESTAMP(3) NULL,

 ДатаОкончания TIMESTAMP(3) NULL,

 НачалоОценки TIMESTAMP(3) NULL,

 ОкончаниеОценки TIMESTAMP(3) NULL,

 Состоятие VARCHAR(16) NULL,

 Организатор_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ТипПороды (

 primaryKey UUID NOT NULL,

 Название VARCHAR(255) NULL,

 ДатаРегистрации TIMESTAMP(3) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE TypeUsageProviderTestClass (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 DataObjectForTest_m0 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass6 (

 primaryKey UUID NOT NULL,

 StringPropForInfTestClass6 VARCHAR(255) NULL,

 ExampleOfClassWithCaptions UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE SomeMasterClass (

 primaryKey UUID NOT NULL,

 FieldA VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Идея (

 primaryKey UUID NOT NULL,

 Заголовок VARCHAR(255) NULL,

 Описание VARCHAR(255) NULL,

 СуммаБаллов DOUBLE PRECISION NULL,

 Автор_m0 UUID NOT NULL,

 Конкурс_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Лапа (

 primaryKey UUID NOT NULL,

 Цвет VARCHAR(255) NULL,

 Размер INT NULL,

 ДатаРождения TIMESTAMP(3) NULL,

 БылиЛиПереломы BOOLEAN NULL,

 Сторона VARCHAR(11) NULL,

 Номер INT NULL,

 РазмерDouble DOUBLE PRECISION NULL,

 РазмерFloat REAL NULL,

 РазмерNullableInt INT NULL,

 РазмерDecimal DECIMAL NULL,

 РазмерNullableDecimal DECIMAL NULL,

 РазмерNullableChar SMALLINT NULL,

 ТипЛапы_m0 UUID NULL,

 Кошка_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ЗначениеКритер (

 primaryKey UUID NOT NULL,

 Значение VARCHAR(255) NULL,

 СредОценкаЭксп DOUBLE PRECISION NULL,

 Критерий_m0 UUID NOT NULL,

 Идея_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE CabbageSalad (

 primaryKey UUID NOT NULL,

 CabbageSaladName VARCHAR(255) NULL,

 Cabbage1 UUID NULL,

 Cabbage2 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass2 (

 primaryKey UUID NOT NULL,

 StringPropertyForInfTestClass2 VARCHAR(255) NULL,

 InformationTestClass_m0 UUID NULL,

 InformationTestClass_m1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Котенок (

 primaryKey UUID NOT NULL,

 КличкаКотенка VARCHAR(255) NULL,

 Глупость INT NULL,

 Кошка UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Region (

 primaryKey UUID NOT NULL,

 RegionName VARCHAR(255) NULL,

 Country2_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AuditClassWithSettings (

 primaryKey UUID NOT NULL,

 CreateTime TIMESTAMP(3) NULL,

 Creator VARCHAR(255) NULL,

 EditTime TIMESTAMP(3) NULL,

 Editor VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClassChild (

 primaryKey UUID NOT NULL,

 PublicStringProperty VARCHAR(255) NULL,

 StringPropertyForInfTestClass VARCHAR(255) NULL,

 IntPropertyForInfTestClass INT NULL,

 BoolPropertyForInfTestClass BOOLEAN NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE МастерМ (

 primaryKey UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE FullTypesDetail1 (

 primaryKey UUID NOT NULL,

 PoleInt INT NULL,

 PoleDateTime TIMESTAMP(3) NULL,

 PoleString VARCHAR(255) NULL,

 PoleFloat REAL NULL,

 PoleDouble DOUBLE PRECISION NULL,

 PoleDecimal DECIMAL NULL,

 PoleBool BOOLEAN NULL,

 PoleNullableInt INT NULL,

 PoleNullableDecimal DECIMAL NULL,

 PoleNullableDateTime TIMESTAMP(3) NULL,

 PoleNullInt INT NULL,

 PoleNullDateTime TIMESTAMP(3) NULL,

 PoleNullFloat REAL NULL,

 PoleNullDouble DOUBLE PRECISION NULL,

 PoleNullDecimal DECIMAL NULL,

 PoleGuid UUID NULL,

 PoleNullGuid UUID NULL,

 PoleEnum VARCHAR(15) NULL,

 PoleChar SMALLINT NULL,

 PoleNullChar SMALLINT NULL,

 FullTypesMainAgregator_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Порода (

 primaryKey UUID NOT NULL,

 Название VARCHAR(255) NULL,

 Ключ UUID NULL,

 ТипПороды UUID NULL,

 Иерархия UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Запрос (

 primaryKey UUID NOT NULL,

 ПервоеДлинноеПолеДляПроверки BOOLEAN NULL,

 ВтороеДлинноеПолеДляПроверки VARCHAR(78) NULL,

 ПятоеДлинноеПолеДляПроверки INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass4 (

 primaryKey UUID NOT NULL,

 StringPropForInfTestClass4 VARCHAR(255) NULL,

 MasterOfInformationTestClass3 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE DataObjectForTest (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 Height INT NULL,

 BirthDate TIMESTAMP(3) NULL,

 Gender BOOLEAN NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ComputedMaster (

 primaryKey UUID NOT NULL,

 MasterField1 VARCHAR(255) NULL,

 MasterField2 VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Личность (

 primaryKey UUID NOT NULL,

 ФИО VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AuditClassWithDisabledAudit (

 primaryKey UUID NOT NULL,

 CreateTime TIMESTAMP(3) NULL,

 Creator VARCHAR(255) NULL,

 EditTime TIMESTAMP(3) NULL,

 Editor VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE КритерийОценки (

 primaryKey UUID NOT NULL,

 ПорядковыйНомер INT NULL,

 Описание VARCHAR(255) NULL,

 Вес DOUBLE PRECISION NULL,

 Обязательный BOOLEAN NULL,

 Конкурс_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Place2 (

 primaryKey UUID NOT NULL,

 PlaceName VARCHAR(255) NULL,

 TomorrowTeritory_m0 UUID NULL,

 TomorrowTeritory_m1 UUID NULL,

 TodayTerritory_m0 UUID NULL,

 TodayTerritory_m1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ТипЛапы (

 primaryKey UUID NOT NULL,

 Название VARCHAR(255) NULL,

 Актуально BOOLEAN NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ИФХозДоговора (

 primaryKey UUID NOT NULL,

 НомерИФХозДогов INT NULL,

 ИсточникФинан UUID NOT NULL,

 ХозДоговор_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Homer (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE MasterClass (

 primaryKey UUID NOT NULL,

 StringMasterProperty VARCHAR(255) NULL,

 InformationTestClass3_m0 UUID NULL,

 InformationTestClass2 UUID NULL,

 InformationTestClass_m0 UUID NULL,

 InformationTestClass_m1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Plant2 (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE NullFileField (

 primaryKey UUID NOT NULL,

 FileField TEXT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Пользователь (

 primaryKey UUID NOT NULL,

 Логин VARCHAR(255) NULL,

 ФИО VARCHAR(255) NULL,

 EMail VARCHAR(255) NULL,

 ДатаРегистрации TIMESTAMP(3) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Human2 (

 primaryKey UUID NOT NULL,

 HumanName VARCHAR(255) NULL,

 TodayHome_m0 UUID NULL,

 TodayHome_m1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE SomeDetailClass (

 primaryKey UUID NOT NULL,

 FieldB VARCHAR(255) NULL,

 ClassA UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE STORMNETLOCKDATA (

 LockKey VARCHAR(300) NOT NULL,

 UserName VARCHAR(300) NOT NULL,

 LockDate TIMESTAMP(3) NULL,

 PRIMARY KEY (LockKey));


CREATE TABLE STORMSETTINGS (

 primaryKey UUID NOT NULL,

 Module VARCHAR(1000) NULL,

 Name VARCHAR(255) NULL,

 Value TEXT NULL,

 "User" VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE STORMAdvLimit (

 primaryKey UUID NOT NULL,

 "User" VARCHAR(255) NULL,

 Published BOOLEAN NULL,

 Module VARCHAR(255) NULL,

 Name VARCHAR(255) NULL,

 Value TEXT NULL,

 HotKeyData INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE STORMFILTERSETTING (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NOT NULL,

 DataObjectView VARCHAR(255) NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE STORMWEBSEARCH (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NOT NULL,

 "Order" INT NOT NULL,

 PresentView VARCHAR(255) NOT NULL,

 DetailedView VARCHAR(255) NOT NULL,

 FilterSetting_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE STORMFILTERDETAIL (

 primaryKey UUID NOT NULL,

 Caption VARCHAR(255) NOT NULL,

 DataObjectView VARCHAR(255) NOT NULL,

 ConnectMasterProp VARCHAR(255) NOT NULL,

 OwnerConnectProp VARCHAR(255) NULL,

 FilterSetting_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE STORMFILTERLOOKUP (

 primaryKey UUID NOT NULL,

 DataObjectType VARCHAR(255) NOT NULL,

 Container VARCHAR(255) NULL,

 ContainerTag VARCHAR(255) NULL,

 FieldsToView VARCHAR(255) NULL,

 FilterSetting_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE UserSetting (

 primaryKey UUID NOT NULL,

 AppName VARCHAR(256) NULL,

 UserName VARCHAR(512) NULL,

 UserGuid UUID NULL,

 ModuleName VARCHAR(1024) NULL,

 ModuleGuid UUID NULL,

 SettName VARCHAR(256) NULL,

 SettGuid UUID NULL,

 SettLastAccessTime TIMESTAMP(3) NULL,

 StrVal VARCHAR(256) NULL,

 TxtVal TEXT NULL,

 IntVal INT NULL,

 BoolVal BOOLEAN NULL,

 GuidVal UUID NULL,

 DecimalVal DECIMAL(20,10) NULL,

 DateTimeVal TIMESTAMP(3) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ApplicationLog (

 primaryKey UUID NOT NULL,

 Category VARCHAR(64) NULL,

 EventId INT NULL,

 Priority INT NULL,

 Severity VARCHAR(32) NULL,

 Title VARCHAR(256) NULL,

 Timestamp TIMESTAMP(3) NULL,

 MachineName VARCHAR(32) NULL,

 AppDomainName VARCHAR(512) NULL,

 ProcessId VARCHAR(256) NULL,

 ProcessName VARCHAR(512) NULL,

 ThreadName VARCHAR(512) NULL,

 Win32ThreadId VARCHAR(128) NULL,

 Message VARCHAR(2500) NULL,

 FormattedMessage TEXT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE STORMAuObjType (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE STORMAuEntity (

 primaryKey UUID NOT NULL,

 ObjectPrimaryKey VARCHAR(38) NOT NULL,

 OperationTime TIMESTAMP(3) NOT NULL,

 OperationType VARCHAR(100) NOT NULL,

 ExecutionResult VARCHAR(12) NOT NULL,

 Source VARCHAR(255) NOT NULL,

 SerializedField TEXT NULL,

 User_m0 UUID NOT NULL,

 ObjectType_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE STORMAuField (

 primaryKey UUID NOT NULL,

 Field VARCHAR(100) NOT NULL,

 OldValue TEXT NULL,

 NewValue TEXT NULL,

 MainChange_m0 UUID NULL,

 AuditEntity_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));




 ALTER TABLE Apparatus2 ADD CONSTRAINT FKe66f7e4d6b27402c8f23a3099d1753fa FOREIGN KEY (Maker_m0) REFERENCES Country2; 
CREATE INDEX Indexa271eee7812040e0b0c8d10d0e79bf11 on Apparatus2 (Maker_m0); 

 ALTER TABLE Apparatus2 ADD CONSTRAINT FK2b3d3ebbc6a949d18927d890ce8a285e FOREIGN KEY (Exporter_m0) REFERENCES Country2; 
CREATE INDEX Index034df8d59af44902aadd3ff886c99946 on Apparatus2 (Exporter_m0); 

 ALTER TABLE ComputedDetail ADD CONSTRAINT FK9d2bf7e313da4c718bab215b069f1009 FOREIGN KEY (ComputedMaster) REFERENCES ComputedMaster; 
CREATE INDEX Index8b12f34ba6614fc2a0465dc103823e60 on ComputedDetail (ComputedMaster); 

 ALTER TABLE Выплаты ADD CONSTRAINT FK7f0ace17e120440caec57cbb32f611f8 FOREIGN KEY (Кредит1) REFERENCES Кредит; 
CREATE INDEX Indexf32afec141bb436d915b8048c78aac1b on Выплаты (Кредит1); 

 ALTER TABLE Блоха ADD CONSTRAINT FK46ae738483924e6f943cc2c2db501c91 FOREIGN KEY (МедведьОбитания) REFERENCES Медведь; 
CREATE INDEX Indexc24a1744a9f64ad18b2271556694b140 on Блоха (МедведьОбитания); 

 ALTER TABLE Этап ADD CONSTRAINT FK6a73def98f0c412692d1f1f942bbdcf0 FOREIGN KEY (КонфигурацияЭтапа_m0) REFERENCES КонфигурацияЭтапа; 
CREATE INDEX Index8870c9c31de243e7ace445e234a1e703 on Этап (КонфигурацияЭтапа_m0); 

 ALTER TABLE Этап ADD CONSTRAINT FKc517eecb352b4014adf0674130f5cc35 FOREIGN KEY (Запрос) REFERENCES Запрос; 
CREATE INDEX Indexba72c29ab3ca4bb7b848c5c593d50107 on Этап (Запрос); 

 ALTER TABLE AuditMasterObject ADD CONSTRAINT FK4a0687b52438492fa4cc73d9523e0a5d FOREIGN KEY (MasterObject) REFERENCES AuditMasterMasterObject; 
CREATE INDEX Index9f2fa46ec1684ed59be29e5c015827be on AuditMasterObject (MasterObject); 

 ALTER TABLE Берлога ADD CONSTRAINT FKabd515fd0147450aae747d49a8f6081b FOREIGN KEY (ЛесРасположения) REFERENCES Лес; 
CREATE INDEX Index7814091c7fdf49e4bb62da09ae5c9f2d on Берлога (ЛесРасположения); 

 ALTER TABLE Берлога ADD CONSTRAINT FK29c21a41e964459d963e70dccaa0614b FOREIGN KEY (Медведь) REFERENCES Медведь; 
CREATE INDEX Index2ce87fe875dc476594ffa82332f5913b on Берлога (Медведь); 

 ALTER TABLE ДокККонкурсу ADD CONSTRAINT FK0436e1f1f5f14bb5b902eec0cdf28404 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index08212530f45240f692c3988c2a8dcc9a on ДокККонкурсу (Конкурс_m0); 

 ALTER TABLE clb ADD CONSTRAINT FKca7079bd18e742d59234a4f9628e8cef FOREIGN KEY (ref2) REFERENCES cla; 
CREATE INDEX Indexe95de89b02284e3dab7f9b12a9dc4cc4 on clb (ref2); 

 ALTER TABLE clb ADD CONSTRAINT FK5c373a8ba6174be58370b90ab7f0ef21 FOREIGN KEY (ref1) REFERENCES cla; 
CREATE INDEX Index04a459d316494caaab6b98d8489f4681 on clb (ref1); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK0624502c566b4a3baf5932a9d7d3362a FOREIGN KEY (Эксперт_m0) REFERENCES Пользователь; 
CREATE INDEX Index0bc95827cc7d4b3da12343d02761d394 on ОценкаЭксперта (Эксперт_m0); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FKe78f296cf6d24d2299c41f3367c39d35 FOREIGN KEY (ЗначениеКритер) REFERENCES ЗначениеКритер; 
CREATE INDEX Index903a87ed28724374ba089fb2990f8e47 on ОценкаЭксперта (ЗначениеКритер); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK486d7334a80e4b0d929b114e12381eff FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Indexa4721c6c20654ddf8329536183ebf969 on ОценкаЭксперта (Идея_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK02270170ad0e4e0a843bfe5dcd66aa04 FOREIGN KEY (MainIngridient_m0) REFERENCES Cabbage2; 
CREATE INDEX Index3baa3f1f42ed4899bf24918fec8c78c8 on Dish2 (MainIngridient_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK0a36161d6f924b9795bd3a692600c364 FOREIGN KEY (MainIngridient_m1) REFERENCES Plant2; 
CREATE INDEX Index00a299ee424e43579d0f904a804fea4c on Dish2 (MainIngridient_m1); 

 ALTER TABLE Медведь ADD CONSTRAINT FKd0189e1167e7431e8c0069b0d1f97ce4 FOREIGN KEY (Мама) REFERENCES Медведь; 
CREATE INDEX Indexf92175d51ac74a3db78fa2dfeb6c4ef3 on Медведь (Мама); 

 ALTER TABLE Медведь ADD CONSTRAINT FKda1fc22665a44948a4d55bfcaafa99e0 FOREIGN KEY (ЛесОбитания) REFERENCES Лес; 
CREATE INDEX Index66a69abd75f347c68c01574173d71899 on Медведь (ЛесОбитания); 

 ALTER TABLE Медведь ADD CONSTRAINT FKc5e870af12cf4d4fbffd46034d92b4da FOREIGN KEY (Папа) REFERENCES Медведь; 
CREATE INDEX Indexe1032e0389464be7a173eee47bc0d43f on Медведь (Папа); 

 ALTER TABLE Медведь ADD CONSTRAINT FK16b1afe033f946a4a4da71f6eb49283f FOREIGN KEY (Друг_m0) REFERENCES Медведь; 
CREATE INDEX Index993084020dc541c6a9769307ff376a2d on Медведь (Друг_m0); 

 ALTER TABLE TypeUsageProviderTestClassChil ADD CONSTRAINT FK21bb626b01c143549ecab40c77b4ba6e FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Indexe03f131bfe2d42de85bc613b7c37cb1b on TypeUsageProviderTestClassChil (DataObjectForTest_m0); 

 ALTER TABLE Soup2 ADD CONSTRAINT FKecd29842b0744167a0e6af7476bfd2b4 FOREIGN KEY (CabbageType) REFERENCES Cabbage2; 
CREATE INDEX Index394f7e087ea44e9d9d2f5ab5cbc6c2fa on Soup2 (CabbageType); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK28f3b2c950f541e4909411fa46911c1f FOREIGN KEY (Мастер_m0) REFERENCES МастерМ; 
CREATE INDEX Indexea41a6ace81c4ec5bb1855324e447011 on TestClassA (Мастер_m0); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK68b4ff6ab2b64e518a55e33c9b0d35cc FOREIGN KEY (Мастер_m1) REFERENCES НаследникМ1; 
CREATE INDEX Indexcccf7b56b5b1492cb5ab451ce4d090fa on TestClassA (Мастер_m1); 

 ALTER TABLE TestClassA ADD CONSTRAINT FKf0880c6eabd149dc8bb9f47b67cbaccf FOREIGN KEY (Мастер_m2) REFERENCES НаследникМ2; 
CREATE INDEX Index8c4bd43566804d568fd3d6536b027579 on TestClassA (Мастер_m2); 

 ALTER TABLE Перелом ADD CONSTRAINT FKeddbc1a773bf4f7a914ae5966d1fd75d FOREIGN KEY (Лапа_m0) REFERENCES Лапа; 
CREATE INDEX Indexc27829052709404395708d27810a14e9 on Перелом (Лапа_m0); 

 ALTER TABLE Лес ADD CONSTRAINT FK8293c8c4cc0b4fa8adb48e68015373a5 FOREIGN KEY (Страна) REFERENCES Страна; 
CREATE INDEX Indexf647032c11ea48ad88cf4bb2ce57e6c1 on Лес (Страна); 

 ALTER TABLE DetailClass ADD CONSTRAINT FKb5ec895b7f744388829faeed61f543fe FOREIGN KEY (MasterClass_m0) REFERENCES MasterClass; 
CREATE INDEX Indexbecfc937ce2248679125958fd225d632 on DetailClass (MasterClass_m0); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK110f1ce871484ffcaf456c82b0d3a4df FOREIGN KEY (MasterClass_m1) REFERENCES MasterClass; 
CREATE INDEX Indexd6252b48cac34f1c811ab5d6a0e44645 on DetailClass (MasterClass_m1); 

 ALTER TABLE AggregatorUpdateObjectTest ADD CONSTRAINT FK24f9632e1bed42b2afe90d85daf31738 FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Index9f0a249ed5214c97994c0d0ce885b1c9 on AggregatorUpdateObjectTest (Detail); 

 ALTER TABLE Adress2 ADD CONSTRAINT FK741b8a4bdfdf43f284f983a7dd6553c6 FOREIGN KEY (Country_m0) REFERENCES Country2; 
CREATE INDEX Index01005b6409af428abd02eb566a8dbe37 on Adress2 (Country_m0); 

 ALTER TABLE FullTypesMainAgregator ADD CONSTRAINT FKb1d83d83e1594ead9c1f175f0f1c261e FOREIGN KEY (FullTypesMaster1_m0) REFERENCES FullTypesMaster1; 
CREATE INDEX Index736cd8563bd24edda5e4ec3a8d10c848 on FullTypesMainAgregator (FullTypesMaster1_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKa39234d80f3941b5a9261cb9fe9d6937 FOREIGN KEY (Ingridient2_m0) REFERENCES Cabbage2; 
CREATE INDEX Indexdf9a16cdebb54c2dabffbd17e99a2080 on Salad2 (Ingridient2_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK9cd45a31973049df8d1177f62b2b2c14 FOREIGN KEY (Ingridient2_m1) REFERENCES Plant2; 
CREATE INDEX Index994045d2f77b4f7384463d0a01133338 on Salad2 (Ingridient2_m1); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKbef49932a86c41d4962e3a4f255b6c3b FOREIGN KEY (Ingridient1_m0) REFERENCES Cabbage2; 
CREATE INDEX Indexc25c7a8bbdf842c998a51ba67efebb29 on Salad2 (Ingridient1_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK57e27d9725de444fa9dea6cf00f25ad5 FOREIGN KEY (Ingridient1_m1) REFERENCES Plant2; 
CREATE INDEX Index7128d6885a354b6daf716c8dd8a5a3e9 on Salad2 (Ingridient1_m1); 

 ALTER TABLE InformationTestClass3 ADD CONSTRAINT FK88bd4ca2280846289972861d5b000d4b FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Indexa1c47900c027495ebedc932e2c274c7a on InformationTestClass3 (InformationTestClass2); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FK8dc2bf579912418c81a9e443df1da5c4 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Index2edaf3498cdd416db97640ea3c44d877 on CombinedTypesUsageProviderTest (DataObjectForTest_m0); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FKd65fb01e51e34d30906c5e48e2af1178 FOREIGN KEY (TypeUsageProviderTestClass) REFERENCES TypeUsageProviderTestClass; 
CREATE INDEX Indexf0cb66ec257846508ee6b1d1acdf0e4e on CombinedTypesUsageProviderTest (TypeUsageProviderTestClass); 

 ALTER TABLE ClassWithCaptions ADD CONSTRAINT FK7e8e098d2ea04525a121b161aae32fb7 FOREIGN KEY (InformationTestClass4) REFERENCES InformationTestClass4; 
CREATE INDEX Index046ffceb9c8b48ffa7195cf07d94fd22 on ClassWithCaptions (InformationTestClass4); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FK435e353239774d47b3c6a8e041429ebc FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Indexcf6d0e8b5eb54a8ebd1c30d2f3393f0b on MasterUpdateObjectTest (Detail); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FKfb122658882b4e6fbe2f682df038440b FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Indexbeace0b7eba24e17a300a29f31f53563 on MasterUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE AuditAgregatorObject ADD CONSTRAINT FKcc768bdeb3a34ce78103c6fd45de0852 FOREIGN KEY (MasterObject) REFERENCES AuditMasterObject; 
CREATE INDEX Index2883b82a69de4a079280ad036ca7b2b1 on AuditAgregatorObject (MasterObject); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FK08586b5fb74d4f39a3cd9da1cbb47a5f FOREIGN KEY (Личность_m0) REFERENCES Личность; 
CREATE INDEX Index5d0b608f235845d2ab14be3b7b57c628 on УчастникХозДог (Личность_m0); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FKd40139530236485f90c94e311ef4dbf6 FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Indexe00dcc14c3fe4d519b7f8d027df34d64 on УчастникХозДог (ХозДоговор_m0); 

 ALTER TABLE Кредит ADD CONSTRAINT FK67b97a4fd6a34c24989cb8cc4fc71917 FOREIGN KEY (Клиент) REFERENCES Клиент; 
CREATE INDEX Index03c386fae0c94252bbac208527fb1454 on Кредит (Клиент); 

 ALTER TABLE Кредит ADD CONSTRAINT FK1dfd67c6a0ce41c885cf9c7631b5d8da FOREIGN KEY (ИнспекторПоКред) REFERENCES ИнспПоКредиту; 
CREATE INDEX Index0f856a8573bf4573becb4f9d39a701de on Кредит (ИнспекторПоКред); 

 ALTER TABLE Кошка ADD CONSTRAINT FKbb30d6979c6f4231bdfed047776334b7 FOREIGN KEY (Порода) REFERENCES Порода; 
CREATE INDEX Indexd2c0cf93cf3e4e1ab11e4787ad72af83 on Кошка (Порода); 

 ALTER TABLE FullTypesDetail2 ADD CONSTRAINT FKf73540a0ca034b21ac3ae2c323dc35f7 FOREIGN KEY (FullTypesMainAgregator) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Index9ddbfa2dcc2946feacee90d6837562df on FullTypesDetail2 (FullTypesMainAgregator); 

 ALTER TABLE CabbagePart2 ADD CONSTRAINT FK20f77de4b0e14b3cbb4c01f7d7813d83 FOREIGN KEY (Cabbage) REFERENCES Cabbage2; 
CREATE INDEX Index3d8157f43e1e4344bedf02688ae75d99 on CabbagePart2 (Cabbage); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FK26842619884845e7ac55d9c98b8491ca FOREIGN KEY (Master) REFERENCES MasterUpdateObjectTest; 
CREATE INDEX Indexd97c2699ce8848d1b0d655c78cf6e70d on DetailUpdateObjectTest (Master); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FK7255424c517c40e3abde7c1edff5606f FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Index87b42c64f33e4c88b9261ab0737f3287 on DetailUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FK0e9e7c1166344367b5c506e987b64b85 FOREIGN KEY (Владелец_m0) REFERENCES Пользователь; 
CREATE INDEX Index7104664babc14167a589274b5c0bf64d on ФайлИдеи (Владелец_m0); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FK52d5adfdef55453fb8e5cbd6fcee48e1 FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Indexea8ac859e5694180a2856209d89c6d3d on ФайлИдеи (Идея_m0); 

 ALTER TABLE ЭтапИсходящегоЗапроса ADD CONSTRAINT FKcdb736245c764184bfbb9c118f5c5f42 FOREIGN KEY (Конфигурация) REFERENCES КонфигурацияЗапроса; 
CREATE INDEX Index82ca98450fb74b3eaed143dfb650c95c on ЭтапИсходящегоЗапроса (Конфигурация); 

 ALTER TABLE ЭтапИсходящегоЗапроса ADD CONSTRAINT FK581e423bd8754eeb99022f99f075772e FOREIGN KEY (Запросы) REFERENCES ИсходящийЗапрос; 
CREATE INDEX Indexe7ff8c5cd43e4818be91c7ed495fdaf3 on ЭтапИсходящегоЗапроса (Запросы); 

 ALTER TABLE cla ADD CONSTRAINT FKabf1204507d84f8789b92c0d13cec1f5 FOREIGN KEY (parent) REFERENCES clb; 
CREATE INDEX Index8afe8aac1dc94e60966e5b2f96343782 on cla (parent); 

 ALTER TABLE Конкурс ADD CONSTRAINT FK6ca2becd08334a9aaa1fea67a2c144f4 FOREIGN KEY (Организатор_m0) REFERENCES Пользователь; 
CREATE INDEX Index88cd603215184677b959adb8b51d0639 on Конкурс (Организатор_m0); 

 ALTER TABLE TypeUsageProviderTestClass ADD CONSTRAINT FKdab41959e12b4ab6aba5fb27690f32da FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Index4fbabdde97e54c969da15079fdb29a90 on TypeUsageProviderTestClass (DataObjectForTest_m0); 

 ALTER TABLE InformationTestClass6 ADD CONSTRAINT FK337aab92433047a796e4e364081441cc FOREIGN KEY (ExampleOfClassWithCaptions) REFERENCES ClassWithCaptions; 
CREATE INDEX Indexcda5e8ff036b487aaf25753212466224 on InformationTestClass6 (ExampleOfClassWithCaptions); 

 ALTER TABLE Идея ADD CONSTRAINT FK98d7908fab544695992632eadfa88f45 FOREIGN KEY (Автор_m0) REFERENCES Пользователь; 
CREATE INDEX Index7ef0922f78ac4b2398579ce7267e7755 on Идея (Автор_m0); 

 ALTER TABLE Идея ADD CONSTRAINT FK186c0b41aded40609bb96ffaf2787655 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Indexfd5c64b278d043339650a84f1bf3aa22 on Идея (Конкурс_m0); 

 ALTER TABLE Лапа ADD CONSTRAINT FK175d02b68c2a4a4389d90f5c514cafce FOREIGN KEY (ТипЛапы_m0) REFERENCES ТипЛапы; 
CREATE INDEX Index32b03a0effd8441391e474aeb2f3a105 on Лапа (ТипЛапы_m0); 

 ALTER TABLE Лапа ADD CONSTRAINT FKd62a77aae51f47e3b5941c86a13886f0 FOREIGN KEY (Кошка_m0) REFERENCES Кошка; 
CREATE INDEX Index44aacd3f622c40c1980cf3dc42df4596 on Лапа (Кошка_m0); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FKb25781d643814a7792fb5fe8e13fa538 FOREIGN KEY (Критерий_m0) REFERENCES КритерийОценки; 
CREATE INDEX Index6d7ca3589d20449fb362c9dd36c569fe on ЗначениеКритер (Критерий_m0); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FKd05d49d02d7b41e48203c80a59c3f02e FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Indexfc47906be53d4c6d8eba47e3cc0fdc2b on ЗначениеКритер (Идея_m0); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FKc50946af5627486799e8ff2058825818 FOREIGN KEY (Cabbage1) REFERENCES Cabbage2; 
CREATE INDEX Index448ea356e96f4b328ac0c85ef93a3d63 on CabbageSalad (Cabbage1); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FK165ffa753f434ecab97d02aa7e917903 FOREIGN KEY (Cabbage2) REFERENCES Cabbage2; 
CREATE INDEX Index31d610b9fed34b97a6504880d9dc88ce on CabbageSalad (Cabbage2); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FKef9a5dc76c29443ba8ee5f724583627f FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Indexcdf0fb861b204961884cce7f3e203b39 on InformationTestClass2 (InformationTestClass_m0); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FK6e0196065ea541d88ee92a413f48f67b FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Indexe62f9f469c1a49828d33ad35f996c11e on InformationTestClass2 (InformationTestClass_m1); 

 ALTER TABLE Котенок ADD CONSTRAINT FK06d84a8c945b4993adadbe82094bb992 FOREIGN KEY (Кошка) REFERENCES Кошка; 
CREATE INDEX Index250a749b862541cdb427322ed8f820ea on Котенок (Кошка); 

 ALTER TABLE Region ADD CONSTRAINT FK14e11ac16ee74d23924af09fb264f701 FOREIGN KEY (Country2_m0) REFERENCES Country2; 
CREATE INDEX Index07ebc26ffce44e4cbf3f56b8bb0d71b6 on Region (Country2_m0); 

 ALTER TABLE FullTypesDetail1 ADD CONSTRAINT FK24a73d1962d740cab5ff584d4610ad11 FOREIGN KEY (FullTypesMainAgregator_m0) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Index6af2e6736d5146a7bb35fa7438b39857 on FullTypesDetail1 (FullTypesMainAgregator_m0); 

 ALTER TABLE Порода ADD CONSTRAINT FKd249277455b2425a82d38e6e080f5513 FOREIGN KEY (ТипПороды) REFERENCES ТипПороды; 
CREATE INDEX Index6d0f0d477913498483c9c347e2c8a9e4 on Порода (ТипПороды); 

 ALTER TABLE Порода ADD CONSTRAINT FK17093c39315d44c39f06e9175efc64a2 FOREIGN KEY (Иерархия) REFERENCES Порода; 
CREATE INDEX Indexc7037ae8b5b044e4adc566a830be7728 on Порода (Иерархия); 

 ALTER TABLE InformationTestClass4 ADD CONSTRAINT FK469ec543e16047f78492e4ff04a99d93 FOREIGN KEY (MasterOfInformationTestClass3) REFERENCES InformationTestClass3; 
CREATE INDEX Indexa057a03309fe42c49ef852a4a8e6d084 on InformationTestClass4 (MasterOfInformationTestClass3); 

 ALTER TABLE КритерийОценки ADD CONSTRAINT FK3934641f178b4f34a0a79e0dadeab667 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index5997a8ea9dda402c8865bb1f636e98ab on КритерийОценки (Конкурс_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FKc6716f2b9d9549e4bb8774243ed6675a FOREIGN KEY (TomorrowTeritory_m0) REFERENCES Country2; 
CREATE INDEX Indexd52c59042ca14ad7bc2131f3c2f9199a on Place2 (TomorrowTeritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FKbe7ff557beb14702aad32aa9ce8fe550 FOREIGN KEY (TomorrowTeritory_m1) REFERENCES Territory2; 
CREATE INDEX Index43476cb352b94ee9a00a5fd47dfcfec2 on Place2 (TomorrowTeritory_m1); 

 ALTER TABLE Place2 ADD CONSTRAINT FK41197716392543b5890c8d7d765e2bd5 FOREIGN KEY (TodayTerritory_m0) REFERENCES Country2; 
CREATE INDEX Indexdcf9ba4c43df434fb854af8b334645c6 on Place2 (TodayTerritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FK919036fb7ce5437da00d20fee1e470a3 FOREIGN KEY (TodayTerritory_m1) REFERENCES Territory2; 
CREATE INDEX Index8c3926edc42c48b89332d71dab962da3 on Place2 (TodayTerritory_m1); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FK60127d0a7adf4a27b14b117646a31ff2 FOREIGN KEY (ИсточникФинан) REFERENCES ИсточникФинанс; 
CREATE INDEX Indexca158c4d336144ed91891aa003e438c4 on ИФХозДоговора (ИсточникФинан); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FK16b3e01b297a48ec8acea5f2d98c9ab9 FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Index8befe3de0afe4f7da3a49fbe650dc074 on ИФХозДоговора (ХозДоговор_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK05f5540dd94e40cfb55c13b8e802d718 FOREIGN KEY (InformationTestClass3_m0) REFERENCES InformationTestClass3; 
CREATE INDEX Indexc02e93a18a8f4da0b8f1f9d92554c28d on MasterClass (InformationTestClass3_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK4c4ec28bc47f43ef9018321abc700879 FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Index7d03fffd21dd4330ae7af018df8d8df4 on MasterClass (InformationTestClass2); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK762baab773a04807829e6e8e9778d9d9 FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Index45ffdad33f034423bc90496eb7708667 on MasterClass (InformationTestClass_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK9e360a41ce664e5e8af3d4f6c2bf3218 FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Indexd47c499415b14183a2a0145ba5077b43 on MasterClass (InformationTestClass_m1); 

 ALTER TABLE Human2 ADD CONSTRAINT FK4e622c598c1f4cc3a90473547f2db214 FOREIGN KEY (TodayHome_m0) REFERENCES Country2; 
CREATE INDEX Indexe6028f474d1545fe8214fdb5488c8273 on Human2 (TodayHome_m0); 

 ALTER TABLE Human2 ADD CONSTRAINT FKb8686e03cae7480f813cc5125da2b5a5 FOREIGN KEY (TodayHome_m1) REFERENCES Territory2; 
CREATE INDEX Index9d956c3fbf1c4d8ebc97a7d5f72fd409 on Human2 (TodayHome_m1); 

 ALTER TABLE SomeDetailClass ADD CONSTRAINT FKc01508c9c5a1453c876edccfa186433a FOREIGN KEY (ClassA) REFERENCES SomeMasterClass; 
CREATE INDEX Index27f208be12014246b3d2e52f4192cdd4 on SomeDetailClass (ClassA); 

 ALTER TABLE STORMWEBSEARCH ADD CONSTRAINT FK5ad34c2dec4e4a5785fd1abdf5538c0e FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERDETAIL ADD CONSTRAINT FKf56fae7aada3401ca774cad45ed66eb8 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERLOOKUP ADD CONSTRAINT FK929a0d421cc74de893c3d9fb3edbdb29 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMAuEntity ADD CONSTRAINT FK26ddccfbe6cb4e0788822cbda01e3a37 FOREIGN KEY (ObjectType_m0) REFERENCES STORMAuObjType; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FK1d62a48daf44422aabe7e77901acbba2 FOREIGN KEY (MainChange_m0) REFERENCES STORMAuField; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FK1c44c8347a1b4d8188ce9e4f8fbda4c7 FOREIGN KEY (AuditEntity_m0) REFERENCES STORMAuEntity; 

