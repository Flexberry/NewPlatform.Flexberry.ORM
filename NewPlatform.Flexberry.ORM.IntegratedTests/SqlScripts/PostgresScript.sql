



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

 Name VARCHAR(255) NULL,

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




 ALTER TABLE Apparatus2 ADD CONSTRAINT FK2fa1d88a7c93422498f3a5e2e39d38c0 FOREIGN KEY (Maker_m0) REFERENCES Country2; 
CREATE INDEX Indexa16986368d164d62a0a484313020745c on Apparatus2 (Maker_m0); 

 ALTER TABLE Apparatus2 ADD CONSTRAINT FK8e4ba475631643099caad09c500547c7 FOREIGN KEY (Exporter_m0) REFERENCES Country2; 
CREATE INDEX Index21750952a6184f4f806bb9cf7d0b1759 on Apparatus2 (Exporter_m0); 

 ALTER TABLE ComputedDetail ADD CONSTRAINT FKfd2e0d6bea6f4fadb5816d0e891c6567 FOREIGN KEY (ComputedMaster) REFERENCES ComputedMaster; 
CREATE INDEX Index7daa3a9d8be340c69185466ac677e4d0 on ComputedDetail (ComputedMaster); 

 ALTER TABLE Выплаты ADD CONSTRAINT FK5dee2cb5e0294baeabaccf67449957db FOREIGN KEY (Кредит1) REFERENCES Кредит; 
CREATE INDEX Index07f05761e60340718f09d9982b5cacfd on Выплаты (Кредит1); 

 ALTER TABLE Блоха ADD CONSTRAINT FK4bbcf229aa7146ee8d5dda3d458939cd FOREIGN KEY (МедведьОбитания) REFERENCES Медведь; 
CREATE INDEX Index4ed3ae4710cc472c8cfe0794a969dba4 on Блоха (МедведьОбитания); 

 ALTER TABLE Этап ADD CONSTRAINT FK9c1ff19bca7c46cdbfb3fdad23172707 FOREIGN KEY (КонфигурацияЭтапа_m0) REFERENCES КонфигурацияЭтапа; 
CREATE INDEX Index7e30538a959b47c48ebc7ed1012aa626 on Этап (КонфигурацияЭтапа_m0); 

 ALTER TABLE Этап ADD CONSTRAINT FKcd9c7b0d819640d098d7455c5f8aaa82 FOREIGN KEY (Запрос) REFERENCES Запрос; 
CREATE INDEX Index45de60a7d8e7461c9e2057a776ac7c0f on Этап (Запрос); 

 ALTER TABLE AuditMasterObject ADD CONSTRAINT FK32941e821f354e0d8b71f6ad61fe6cf9 FOREIGN KEY (MasterObject) REFERENCES AuditMasterMasterObject; 
CREATE INDEX Indexe8f1347e0f9140f7916236f59aeab928 on AuditMasterObject (MasterObject); 

 ALTER TABLE Берлога ADD CONSTRAINT FKeafff6ff15cb4594a54cfe6f91ab8ac9 FOREIGN KEY (ЛесРасположения) REFERENCES Лес; 
CREATE INDEX Indexb08c8ab4a74143c2b23d4026f0705e79 on Берлога (ЛесРасположения); 

 ALTER TABLE Берлога ADD CONSTRAINT FKfced157382144474b4e730ce258fe4a8 FOREIGN KEY (Медведь) REFERENCES Медведь; 
CREATE INDEX Indexbc0c429a47e946679e38b7ce572c306f on Берлога (Медведь); 

 ALTER TABLE ДокККонкурсу ADD CONSTRAINT FK270526c6660640eead6f87862c48d622 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index795ca2ce8a954a3986dfd8b4be32197f on ДокККонкурсу (Конкурс_m0); 

 ALTER TABLE clb ADD CONSTRAINT FKf01603c9e47d467e87f488a1f9de9147 FOREIGN KEY (ref2) REFERENCES cla; 
CREATE INDEX Index0b420c4974e342e387f349f706f72d0e on clb (ref2); 

 ALTER TABLE clb ADD CONSTRAINT FKec741e285c274940a213b92e6231e96a FOREIGN KEY (ref1) REFERENCES cla; 
CREATE INDEX Index9bff05ddcff34f0280d9efd8fed7d826 on clb (ref1); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK81f80c39c9534b86890f5aaccc6e88e4 FOREIGN KEY (Эксперт_m0) REFERENCES Пользователь; 
CREATE INDEX Indexabc0e26db03b4eb4835bf5a00f9b865f on ОценкаЭксперта (Эксперт_m0); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK1e8475ccaf9545a89c76db9e2c30c116 FOREIGN KEY (ЗначениеКритер) REFERENCES ЗначениеКритер; 
CREATE INDEX Indexbc1c70dbc62947269eb635a536602ca7 on ОценкаЭксперта (ЗначениеКритер); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK815253272fbe4bfe930a29f33aa998f3 FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index3c2c9e49661b4dc0b0aa8fb6d4c0a881 on ОценкаЭксперта (Идея_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FKa1aab0d0d7264a49a60422dcfc966982 FOREIGN KEY (MainIngridient_m0) REFERENCES Cabbage2; 
CREATE INDEX Index08d71992cb9e400582e9bc0c8d03a14a on Dish2 (MainIngridient_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK768846bba38747c98f29aab8cb8a15ca FOREIGN KEY (MainIngridient_m1) REFERENCES Plant2; 
CREATE INDEX Index8a4069864cf04f029d70a552de7a31cd on Dish2 (MainIngridient_m1); 

 ALTER TABLE Медведь ADD CONSTRAINT FKd4417b6023b744a89cbf8f210bac7f88 FOREIGN KEY (Мама) REFERENCES Медведь; 
CREATE INDEX Index634e23831bec490c85d87425fb427043 on Медведь (Мама); 

 ALTER TABLE Медведь ADD CONSTRAINT FK0e883fdb212b400ca632fa7b701f034c FOREIGN KEY (ЛесОбитания) REFERENCES Лес; 
CREATE INDEX Index5f17dfb1bcb040bbbe0d2b378c37b59f on Медведь (ЛесОбитания); 

 ALTER TABLE Медведь ADD CONSTRAINT FK35e68316706b461a9b7a76214ef9f21c FOREIGN KEY (Папа) REFERENCES Медведь; 
CREATE INDEX Indexf496bf5dca7a4ed88a9d66300a53d265 on Медведь (Папа); 

 ALTER TABLE Медведь ADD CONSTRAINT FK8e2540028cab4523a69858b1afc1268d FOREIGN KEY (Друг_m0) REFERENCES Медведь; 
CREATE INDEX Index8c429c8e4b3c4dc8be6de8a795631a68 on Медведь (Друг_m0); 

 ALTER TABLE TypeUsageProviderTestClassChil ADD CONSTRAINT FKdb1831137bf4439c96ac1e9316005b26 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Index40e52175b47d425d8de6aef80d1fb41f on TypeUsageProviderTestClassChil (DataObjectForTest_m0); 

 ALTER TABLE Soup2 ADD CONSTRAINT FKc8f7aca42b84410c81d4bfbdab369928 FOREIGN KEY (CabbageType) REFERENCES Cabbage2; 
CREATE INDEX Index06909b32953a44f89d016d41af527f88 on Soup2 (CabbageType); 

 ALTER TABLE TestClassA ADD CONSTRAINT FKda4dbc86282f4dacbeeec4f2ac7a6491 FOREIGN KEY (Мастер_m0) REFERENCES МастерМ; 
CREATE INDEX Index0a0dc645fa4a43b1b58df5353d03b602 on TestClassA (Мастер_m0); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK10c6ad8ca01e48f5a718fbc396a214d0 FOREIGN KEY (Мастер_m1) REFERENCES НаследникМ1; 
CREATE INDEX Index2e3c319aac9448459d4f0e74c05887d3 on TestClassA (Мастер_m1); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK1e8f377a5f3340988338a8aa112a4c92 FOREIGN KEY (Мастер_m2) REFERENCES НаследникМ2; 
CREATE INDEX Indexbcd0fb53b69a49d4ac2ded1e33827f2c on TestClassA (Мастер_m2); 

 ALTER TABLE Перелом ADD CONSTRAINT FKd0f1d879dbcb437f8484aa8278efce4f FOREIGN KEY (Лапа_m0) REFERENCES Лапа; 
CREATE INDEX Indexe506f7aaaa244b7b8bf0c506094c0d20 on Перелом (Лапа_m0); 

 ALTER TABLE Лес ADD CONSTRAINT FK8f1150648fa7421aa1fb52a15ae97b97 FOREIGN KEY (Страна) REFERENCES Страна; 
CREATE INDEX Index09f755feff83494e91ad59c96f98596e on Лес (Страна); 

 ALTER TABLE DetailClass ADD CONSTRAINT FKe6f102bb705b4f498056826721083904 FOREIGN KEY (MasterClass_m0) REFERENCES MasterClass; 
CREATE INDEX Indexbd1eae539aa5409389af5fe3e1236547 on DetailClass (MasterClass_m0); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK4d0ab5b56ec24d89b6105f8d717af6f9 FOREIGN KEY (MasterClass_m1) REFERENCES MasterClass; 
CREATE INDEX Index8fad5a8489bf4b1e87a8d3723de2fb18 on DetailClass (MasterClass_m1); 

 ALTER TABLE AggregatorUpdateObjectTest ADD CONSTRAINT FKead89e2c231644de884e1dae3d763ad3 FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Index2d57b3f06d2c46a89e5e7de4a079cefa on AggregatorUpdateObjectTest (Detail); 

 ALTER TABLE Adress2 ADD CONSTRAINT FKcb44f85a56d44bd1b099171e9aa40bc7 FOREIGN KEY (Country_m0) REFERENCES Country2; 
CREATE INDEX Indexbb7c4547205142b7a2f6dd3f5b2f0e40 on Adress2 (Country_m0); 

 ALTER TABLE FullTypesMainAgregator ADD CONSTRAINT FK006d8a5a75124f98b91184aa5de5666d FOREIGN KEY (FullTypesMaster1_m0) REFERENCES FullTypesMaster1; 
CREATE INDEX Indexfba67e2bbe7e4ff28eadb05c418c0687 on FullTypesMainAgregator (FullTypesMaster1_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK9664fe921a804f10a1733a92a456a94f FOREIGN KEY (Ingridient2_m0) REFERENCES Cabbage2; 
CREATE INDEX Index2609a883441e497b8bab55c025d7e181 on Salad2 (Ingridient2_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKf3cd21cc7254439b8f8e105204892c93 FOREIGN KEY (Ingridient2_m1) REFERENCES Plant2; 
CREATE INDEX Indexc932139f50144a4c886f265ee78825fa on Salad2 (Ingridient2_m1); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKc4e94c26851b475cb6930cad5916ac7e FOREIGN KEY (Ingridient1_m0) REFERENCES Cabbage2; 
CREATE INDEX Indexf91e727f98b74d518dd73b4cc53ebf71 on Salad2 (Ingridient1_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK4244083d439b440f9667297dd4487836 FOREIGN KEY (Ingridient1_m1) REFERENCES Plant2; 
CREATE INDEX Index825a28eab6ed4a94a3dbe2bb684d5156 on Salad2 (Ingridient1_m1); 

 ALTER TABLE InformationTestClass3 ADD CONSTRAINT FK2b567bab7dea4f838f2a89ca46fb40af FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Indexeacb84340e6246b4abf9a79415fd3cbf on InformationTestClass3 (InformationTestClass2); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FK392c891f75e34fa78f85b5e658a7639f FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Indexc97af92568c246d58bc5dc44db224d58 on CombinedTypesUsageProviderTest (DataObjectForTest_m0); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FK36872fdd99fe42389f690b78b6362c94 FOREIGN KEY (TypeUsageProviderTestClass) REFERENCES TypeUsageProviderTestClass; 
CREATE INDEX Index465b18576da9497db4ba02464f832057 on CombinedTypesUsageProviderTest (TypeUsageProviderTestClass); 

 ALTER TABLE ClassWithCaptions ADD CONSTRAINT FK45cca9464cf94369abdfcf9472369020 FOREIGN KEY (InformationTestClass4) REFERENCES InformationTestClass4; 
CREATE INDEX Indexfc615269a866493c834e3da077b0152c on ClassWithCaptions (InformationTestClass4); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FKe0ce7ea43df144d4b43642aa49963a4a FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Indexd0cdfabaed7c4be591c3fef4e13268f1 on MasterUpdateObjectTest (Detail); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FK60f3c7003fd94c2b89e52291c92a6013 FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Index37549545453946f39540b07e86c95b9d on MasterUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE AuditAgregatorObject ADD CONSTRAINT FKc22120d66ecb4bc5b09b024ec14be11c FOREIGN KEY (MasterObject) REFERENCES AuditMasterObject; 
CREATE INDEX Index7438a52c715c419dacbc58eff49e6cdc on AuditAgregatorObject (MasterObject); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FK8712d52898d3416480ed206bb47b2d44 FOREIGN KEY (Личность_m0) REFERENCES Личность; 
CREATE INDEX Index5e832fb401264aacb53c1daf740e6220 on УчастникХозДог (Личность_m0); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FK0312c89330a1469d912d8e2aaae505ac FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Indexa18907ec46184232add7ccd6785a1147 on УчастникХозДог (ХозДоговор_m0); 

 ALTER TABLE Кредит ADD CONSTRAINT FK41365b926942477b8918b290a0d97b35 FOREIGN KEY (Клиент) REFERENCES Клиент; 
CREATE INDEX Index123519603d73419495a55e55bcb92bff on Кредит (Клиент); 

 ALTER TABLE Кредит ADD CONSTRAINT FKb3faa0a5c3c344428c52370be940e5ab FOREIGN KEY (ИнспекторПоКред) REFERENCES ИнспПоКредиту; 
CREATE INDEX Index5912810dba734656b0c641f9edab496e on Кредит (ИнспекторПоКред); 

 ALTER TABLE Кошка ADD CONSTRAINT FK0c846368bff14155b5e42433ebdecc8b FOREIGN KEY (Порода) REFERENCES Порода; 
CREATE INDEX Indexb2ec3c3de74c4952a1e6549212f04268 on Кошка (Порода); 

 ALTER TABLE FullTypesDetail2 ADD CONSTRAINT FK48f7b35ccb95451e920203c86af6b0e5 FOREIGN KEY (FullTypesMainAgregator) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Index88ae90050edb4193bdaf80914f049fc0 on FullTypesDetail2 (FullTypesMainAgregator); 

 ALTER TABLE CabbagePart2 ADD CONSTRAINT FK6346d1ac8e9c4aea98d5b8a39b0ea3f8 FOREIGN KEY (Cabbage) REFERENCES Cabbage2; 
CREATE INDEX Indexde7c312f3186446ca95ddfb8856732d2 on CabbagePart2 (Cabbage); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FKf5261894a7e64e44920c73a426bd797b FOREIGN KEY (Master) REFERENCES MasterUpdateObjectTest; 
CREATE INDEX Index7ef8a75c64434991a62343320829ed40 on DetailUpdateObjectTest (Master); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FK94001c8d1af142d69ca8d946f55a31db FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Index4152c722dba04f319abdcf88b93c6800 on DetailUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FKf9f20f60a69f44588f277602d8553c1c FOREIGN KEY (Владелец_m0) REFERENCES Пользователь; 
CREATE INDEX Index88352efcf16740eb95968a13f314a3bd on ФайлИдеи (Владелец_m0); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FK50b613dccaf64df28bef0bfd6fa433a7 FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index2bd52920739a43e8bc03c2d1b2b32565 on ФайлИдеи (Идея_m0); 

 ALTER TABLE ЭтапИсходящегоЗапроса ADD CONSTRAINT FK8cfad30b3a9e4b33a780a6a046950077 FOREIGN KEY (Конфигурация) REFERENCES КонфигурацияЗапроса; 
CREATE INDEX Index426dbda34641433682abb801ca755a71 on ЭтапИсходящегоЗапроса (Конфигурация); 

 ALTER TABLE ЭтапИсходящегоЗапроса ADD CONSTRAINT FKedd05645606047288aee30159d20fe99 FOREIGN KEY (Запросы) REFERENCES ИсходящийЗапрос; 
CREATE INDEX Index3020fdf0c5c742f6ad72ef48c48d2526 on ЭтапИсходящегоЗапроса (Запросы); 

 ALTER TABLE cla ADD CONSTRAINT FKe1a5129c34bd4830a4121552ca98feff FOREIGN KEY (parent) REFERENCES clb; 
CREATE INDEX Index1d174215a68e4ec4ad331249650fa950 on cla (parent); 

 ALTER TABLE Конкурс ADD CONSTRAINT FK00efdeedfffe40f195f431261a015973 FOREIGN KEY (Организатор_m0) REFERENCES Пользователь; 
CREATE INDEX Index0d9ff9acba6942359e616109e5790514 on Конкурс (Организатор_m0); 

 ALTER TABLE TypeUsageProviderTestClass ADD CONSTRAINT FKf34bbf2a051b4e8bb2e36d4c4d7c44d3 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Indexaa7fa12e0ae041f1b01614ca4c961ad1 on TypeUsageProviderTestClass (DataObjectForTest_m0); 

 ALTER TABLE InformationTestClass6 ADD CONSTRAINT FKe1813eb3d7a141c096c00f8915759b3a FOREIGN KEY (ExampleOfClassWithCaptions) REFERENCES ClassWithCaptions; 
CREATE INDEX Index0fc3bc56ccd04f758981d56761bdb0bc on InformationTestClass6 (ExampleOfClassWithCaptions); 

 ALTER TABLE Идея ADD CONSTRAINT FK7dac1f9af8344b36825dfa01a1c96b96 FOREIGN KEY (Автор_m0) REFERENCES Пользователь; 
CREATE INDEX Indexea483e84596d4da598c4987c911281f5 on Идея (Автор_m0); 

 ALTER TABLE Идея ADD CONSTRAINT FK2e41286341974c25bdc5708faf42f8f8 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index385d7cfed6b84596941bb5f242795019 on Идея (Конкурс_m0); 

 ALTER TABLE Лапа ADD CONSTRAINT FKe1b4b1f971574883a4595f4030fc3dd9 FOREIGN KEY (ТипЛапы_m0) REFERENCES ТипЛапы; 
CREATE INDEX Indexfc4e75d7e16441c685cdf27d3d838076 on Лапа (ТипЛапы_m0); 

 ALTER TABLE Лапа ADD CONSTRAINT FKff10ca0536cc49f1863f591fef68fa55 FOREIGN KEY (Кошка_m0) REFERENCES Кошка; 
CREATE INDEX Index1c7b5873a3fc464aa06bb1fe7386f805 on Лапа (Кошка_m0); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FKe411c453245c4a91a0509473e9851e96 FOREIGN KEY (Критерий_m0) REFERENCES КритерийОценки; 
CREATE INDEX Index7a17c6563c6143e3b21585a2d2ccfa08 on ЗначениеКритер (Критерий_m0); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FK2ee98c4e524846d5822c6e38ff708cb9 FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index4a2c5d4823be4f118bcbb20ae2e12bf9 on ЗначениеКритер (Идея_m0); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FK3112553ab45b4ceb8400e9c5bd9aadfd FOREIGN KEY (Cabbage1) REFERENCES Cabbage2; 
CREATE INDEX Indexa4d2271383ca43578639b1aeb698f829 on CabbageSalad (Cabbage1); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FK4752e362a4bb4a03a73c83e02ffff27a FOREIGN KEY (Cabbage2) REFERENCES Cabbage2; 
CREATE INDEX Index77624afbe7fc466fa0e940269da3fb63 on CabbageSalad (Cabbage2); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FKd546de7baef74bec8a800c128cc82ec3 FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Index62f42ac492fc4a58a6f61c1bb5f63400 on InformationTestClass2 (InformationTestClass_m0); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FK1ef6af9c69c14dcaa0c130c8db6dcf21 FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Indexc9a56c8f2e84450cb5319829175d89f3 on InformationTestClass2 (InformationTestClass_m1); 

 ALTER TABLE Котенок ADD CONSTRAINT FKb327230024ca46a193f6dd7640b15462 FOREIGN KEY (Кошка) REFERENCES Кошка; 
CREATE INDEX Index0d49cbe4dbbd4f1b800daa58eabd75f0 on Котенок (Кошка); 

 ALTER TABLE Region ADD CONSTRAINT FK7acdeed6127b4cf5a58ef64af0bf1560 FOREIGN KEY (Country2_m0) REFERENCES Country2; 
CREATE INDEX Indexbfbb8055c5e34d13a7d04da4f0ebe696 on Region (Country2_m0); 

 ALTER TABLE FullTypesDetail1 ADD CONSTRAINT FK1c11a387fd554fcc8fbea4eb84bcc39a FOREIGN KEY (FullTypesMainAgregator_m0) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Indexd1f86a73552a4b84b38269f6836cfe57 on FullTypesDetail1 (FullTypesMainAgregator_m0); 

 ALTER TABLE Порода ADD CONSTRAINT FKb94665a8d7d14b09ba340cb4c63ab00d FOREIGN KEY (ТипПороды) REFERENCES ТипПороды; 
CREATE INDEX Index1a00d1d989494a209d9365582191df60 on Порода (ТипПороды); 

 ALTER TABLE Порода ADD CONSTRAINT FKc40b3fdc3b5744ebbc188e88f587d9ca FOREIGN KEY (Иерархия) REFERENCES Порода; 
CREATE INDEX Indexa2af6ca913d2464899b748f1ebb01ced on Порода (Иерархия); 

 ALTER TABLE InformationTestClass4 ADD CONSTRAINT FK1f62e9e8a9ee4c50b0e71c51089a1ccb FOREIGN KEY (MasterOfInformationTestClass3) REFERENCES InformationTestClass3; 
CREATE INDEX Indexdc755dc48afe47e7a8de6608fb5bb270 on InformationTestClass4 (MasterOfInformationTestClass3); 

 ALTER TABLE КритерийОценки ADD CONSTRAINT FK357aaeb394654ad19690af7d2d71607b FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Indexd36c8852a11144049214f8423068d73a on КритерийОценки (Конкурс_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FKdad83897a7c34ff68fd4cb1cd5a11ba8 FOREIGN KEY (TomorrowTeritory_m0) REFERENCES Country2; 
CREATE INDEX Index2cc1e628fbc341ee8273b2c59fdcc3dc on Place2 (TomorrowTeritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FK2408b5e648844e08bfb529dab9ddf800 FOREIGN KEY (TomorrowTeritory_m1) REFERENCES Territory2; 
CREATE INDEX Indexb5deab03602b4bb3b61bb07ee19ace23 on Place2 (TomorrowTeritory_m1); 

 ALTER TABLE Place2 ADD CONSTRAINT FK2b98ecf3fffc4cd9abbfeb9eafec562a FOREIGN KEY (TodayTerritory_m0) REFERENCES Country2; 
CREATE INDEX Indexb5e7a1c283cd4b779eda2ac0b3eb15a2 on Place2 (TodayTerritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FK3434995d077d4a63bbd476c918b0d643 FOREIGN KEY (TodayTerritory_m1) REFERENCES Territory2; 
CREATE INDEX Indexda5cd1a9fdb342d2aa72b311ab6bb6f1 on Place2 (TodayTerritory_m1); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FKbe7d4d8e49f84e9c82b9cfa0d3ba7a2e FOREIGN KEY (ИсточникФинан) REFERENCES ИсточникФинанс; 
CREATE INDEX Indexe6b73e445a1d419fbe67283a3fc8acd6 on ИФХозДоговора (ИсточникФинан); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FK8a16278aac1444558ecfd6becb20e528 FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Indexa136db99cec3461891f2f1b7f3af1693 on ИФХозДоговора (ХозДоговор_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FKcfbcc6a767cb4a41930a44713552ca33 FOREIGN KEY (InformationTestClass3_m0) REFERENCES InformationTestClass3; 
CREATE INDEX Index38f0dc519208469888a4194ca76eade1 on MasterClass (InformationTestClass3_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FKec664f0fb0dc42cd9a37e6f9715ea26f FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Index3c4f6f51719244419ff7645b6e65e7ab on MasterClass (InformationTestClass2); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK3fcb05bcabe9467c8fee65fe664961cd FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Indexcb6fe40201124ed29bf3b67946be58b5 on MasterClass (InformationTestClass_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK54de7aaa8bd04b06aa3f8960e0a7ddc7 FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Index5b50849a048042f0846f272bb3593ec0 on MasterClass (InformationTestClass_m1); 

 ALTER TABLE Human2 ADD CONSTRAINT FKb4e6df0804f9421d8cd6d5b6f7f7a8f8 FOREIGN KEY (TodayHome_m0) REFERENCES Country2; 
CREATE INDEX Index8dba805be9824975bef11f5a8c63e74b on Human2 (TodayHome_m0); 

 ALTER TABLE Human2 ADD CONSTRAINT FKd9ee5d8dff2245feb9315a00ec808f8d FOREIGN KEY (TodayHome_m1) REFERENCES Territory2; 
CREATE INDEX Index0e2f2b1dae80477394d38fa39bc83474 on Human2 (TodayHome_m1); 

 ALTER TABLE SomeDetailClass ADD CONSTRAINT FK4be4c314d8d14848b16addede00bd562 FOREIGN KEY (ClassA) REFERENCES SomeMasterClass; 
CREATE INDEX Index82a148cb723f47e3bcc45d21a724da96 on SomeDetailClass (ClassA); 

 ALTER TABLE STORMWEBSEARCH ADD CONSTRAINT FK1d730c9570124340b71a1e01faf4a877 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERDETAIL ADD CONSTRAINT FK70a16110b14d4bc89edee90f48b22eb1 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERLOOKUP ADD CONSTRAINT FK66d6286bbb044b64ab9adda69782ee55 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMAuEntity ADD CONSTRAINT FKd4391b2c4cb045f3814703e8bcefeebc FOREIGN KEY (ObjectType_m0) REFERENCES STORMAuObjType; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FK18c695248431434d84329e3a7a657440 FOREIGN KEY (MainChange_m0) REFERENCES STORMAuField; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FKea1e46179f714ea3b01cb070799c2a4e FOREIGN KEY (AuditEntity_m0) REFERENCES STORMAuEntity; 

