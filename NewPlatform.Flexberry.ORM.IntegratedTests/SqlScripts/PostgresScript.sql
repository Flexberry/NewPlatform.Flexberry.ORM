



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


CREATE TABLE Parcel (

 primaryKey UUID NOT NULL,

 Address VARCHAR(255) NULL,

 Weight DOUBLE PRECISION NULL,

 DeliveredByHomer UUID NULL,

 DeliveredByMailman UUID NULL,

 PRIMARY KEY (primaryKey));


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

 Photo TEXT NULL,

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




 ALTER TABLE Apparatus2 ADD CONSTRAINT FK9ca2924815894735aa5e0d6be0892ddb FOREIGN KEY (Maker_m0) REFERENCES Country2; 
CREATE INDEX Index3772cf966ee24a12b4ae6e117da33e07 on Apparatus2 (Maker_m0); 

 ALTER TABLE Apparatus2 ADD CONSTRAINT FK12dc2546656e4b9aaa09fa75552780f9 FOREIGN KEY (Exporter_m0) REFERENCES Country2; 
CREATE INDEX Indexbf1dcdeab10246deacdf6217fa395491 on Apparatus2 (Exporter_m0); 

 ALTER TABLE ComputedDetail ADD CONSTRAINT FKa157f7c209114bc7951e192d0a5310ac FOREIGN KEY (ComputedMaster) REFERENCES ComputedMaster; 
CREATE INDEX Index325ea74332104338a9bbaeb361b88754 on ComputedDetail (ComputedMaster); 

 ALTER TABLE Выплаты ADD CONSTRAINT FK8ff5ffdee3f14c9d96f063c95640f0fb FOREIGN KEY (Кредит1) REFERENCES Кредит; 
CREATE INDEX Indexcf0c415ccbf948c4be6867a0aa44ce1e on Выплаты (Кредит1); 

 ALTER TABLE Блоха ADD CONSTRAINT FK3c26684644d54fdd8f134ccb3a58896e FOREIGN KEY (МедведьОбитания) REFERENCES Медведь; 
CREATE INDEX Index1bc41e2e84da41d0860a87e00ba7554c on Блоха (МедведьОбитания); 

 ALTER TABLE Этап ADD CONSTRAINT FK86dd2c3c73b94fba9306ffc348d194cb FOREIGN KEY (КонфигурацияЭтапа_m0) REFERENCES КонфигурацияЭтапа; 
CREATE INDEX Index5b819cdc6ffd46de9a02316a96bbe8fa on Этап (КонфигурацияЭтапа_m0); 

 ALTER TABLE Этап ADD CONSTRAINT FK3556f853d4ec4e3db4856b6fe026a3af FOREIGN KEY (Запрос) REFERENCES Запрос; 
CREATE INDEX Index6bde22bd9bca426d8ece98538eaef923 on Этап (Запрос); 

 ALTER TABLE AuditMasterObject ADD CONSTRAINT FK84ebf2a64ce94d69a2e2674b83478ed4 FOREIGN KEY (MasterObject) REFERENCES AuditMasterMasterObject; 
CREATE INDEX Indexed33b96db1224e1c84d33ada055eddb3 on AuditMasterObject (MasterObject); 

 ALTER TABLE Берлога ADD CONSTRAINT FK9262a42d86d84c409300756501d61574 FOREIGN KEY (ЛесРасположения) REFERENCES Лес; 
CREATE INDEX Indexc9ec6c1c96594b449f7055b3004cc5b1 on Берлога (ЛесРасположения); 

 ALTER TABLE Берлога ADD CONSTRAINT FKcf14b992d6664811b40d95ab39361332 FOREIGN KEY (Медведь) REFERENCES Медведь; 
CREATE INDEX Indexde0c106501d944be8b8854ec8ca2ffe0 on Берлога (Медведь); 

 ALTER TABLE ДокККонкурсу ADD CONSTRAINT FK8b7ba6896d1d44d58392ac3d6dc38ca7 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Indexbdbaa2875d094effbfe77a98bde15924 on ДокККонкурсу (Конкурс_m0); 

 ALTER TABLE clb ADD CONSTRAINT FK6569aca4ed5d4f4a80cef64a556b537d FOREIGN KEY (ref2) REFERENCES cla; 
CREATE INDEX Indexf52ebbb6a9854b279199a15a6e7a160a on clb (ref2); 

 ALTER TABLE clb ADD CONSTRAINT FKaa24cc0ff45945e4a6f681e09e1af5e1 FOREIGN KEY (ref1) REFERENCES cla; 
CREATE INDEX Index6c4bdef8a23d46d7aed75f9bdffbe736 on clb (ref1); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FKcf184cc02d374df49feae3f8ec38d78e FOREIGN KEY (Эксперт_m0) REFERENCES Пользователь; 
CREATE INDEX Index5087e6dd24cc4f939ef15c4851bff563 on ОценкаЭксперта (Эксперт_m0); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FKa26650286b3a430c8f0c5fc69319454c FOREIGN KEY (ЗначениеКритер) REFERENCES ЗначениеКритер; 
CREATE INDEX Index42254b777cb74b33b87e1d49b3341648 on ОценкаЭксперта (ЗначениеКритер); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK1323bea984c544929ad5aaf08bc0d00e FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index98bf5fed4ffc42dea62897ab5b97ca10 on ОценкаЭксперта (Идея_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK76c2415928b549f6b1091efdb4b8f89b FOREIGN KEY (MainIngridient_m0) REFERENCES Cabbage2; 
CREATE INDEX Index68e29f76f54a48a2a2e77b5e61044791 on Dish2 (MainIngridient_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK5c827912acb3475c804f92ce51894e67 FOREIGN KEY (MainIngridient_m1) REFERENCES Plant2; 
CREATE INDEX Index4de91038ba8b4faab287c54cf077eb45 on Dish2 (MainIngridient_m1); 

 ALTER TABLE Медведь ADD CONSTRAINT FK8374dcc7aaf54805bae9dc5776943faa FOREIGN KEY (Мама) REFERENCES Медведь; 
CREATE INDEX Indexdab365bf9d5b41c39eba4cc3435e6ec3 on Медведь (Мама); 

 ALTER TABLE Медведь ADD CONSTRAINT FK66a311e628cf4f2b9e663f088a7f2efe FOREIGN KEY (ЛесОбитания) REFERENCES Лес; 
CREATE INDEX Index370fe80029a34b328df45eb617dd382d on Медведь (ЛесОбитания); 

 ALTER TABLE Медведь ADD CONSTRAINT FKab87559bd9254cc98f182dc46f13d2d4 FOREIGN KEY (Папа) REFERENCES Медведь; 
CREATE INDEX Indexd6369439deb24dd39c1e02c1bb32f92b on Медведь (Папа); 

 ALTER TABLE Медведь ADD CONSTRAINT FKf16852f642054f82b9ab95fe40165a65 FOREIGN KEY (Друг_m0) REFERENCES Медведь; 
CREATE INDEX Index9487b57b4f274fdc898d2b08b7cbdf62 on Медведь (Друг_m0); 

 ALTER TABLE TypeUsageProviderTestClassChil ADD CONSTRAINT FKf3a30ea26dd64b3585a0c6d641de972c FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Index58bdddd9d1a6462ca3e4c7babd953537 on TypeUsageProviderTestClassChil (DataObjectForTest_m0); 

 ALTER TABLE Soup2 ADD CONSTRAINT FK1ae118a11f6c48dfb35afd6b5ee2f2b3 FOREIGN KEY (CabbageType) REFERENCES Cabbage2; 
CREATE INDEX Indexa26bc64c7f5346cc923caa0a6449b71b on Soup2 (CabbageType); 

 ALTER TABLE Parcel ADD CONSTRAINT FK1ebdcdfff4834b9a8061f42661612534 FOREIGN KEY (DeliveredByHomer) REFERENCES Homer; 
CREATE INDEX Index76d1b1e2abe7423e82c6e3d7d4c3e81d on Parcel (DeliveredByHomer); 

 ALTER TABLE Parcel ADD CONSTRAINT FK614ccdcbe00b4cb4bd183469276cf08a FOREIGN KEY (DeliveredByMailman) REFERENCES Mailman; 
CREATE INDEX Index7769dae88d594838bb2c5600544f6c63 on Parcel (DeliveredByMailman); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK248066b703904abab2601a3b5c6b9e4e FOREIGN KEY (Мастер_m0) REFERENCES МастерМ; 
CREATE INDEX Index968452b0f7844528abdbd9f4b277037d on TestClassA (Мастер_m0); 

 ALTER TABLE TestClassA ADD CONSTRAINT FKce99f2cb0c6347d794064f7de9906756 FOREIGN KEY (Мастер_m1) REFERENCES НаследникМ1; 
CREATE INDEX Index3d024f4ddeb84ea79c6eca0b4b5ddb76 on TestClassA (Мастер_m1); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK66463863dcdb4f75b5649611752dcdef FOREIGN KEY (Мастер_m2) REFERENCES НаследникМ2; 
CREATE INDEX Index7a58f9ee27964d918e89f447b7cee9ca on TestClassA (Мастер_m2); 

 ALTER TABLE Перелом ADD CONSTRAINT FKf904a9b2faba46f6a058a9a361e30d4c FOREIGN KEY (Лапа_m0) REFERENCES Лапа; 
CREATE INDEX Indexd38294be43754d558b3c37e52f0cb8ad on Перелом (Лапа_m0); 

 ALTER TABLE Лес ADD CONSTRAINT FK08bfa25681c64ab9bea5232527f0aca2 FOREIGN KEY (Страна) REFERENCES Страна; 
CREATE INDEX Index5d95af2d2c8543a7b97e786c6ebffdb3 on Лес (Страна); 

 ALTER TABLE DetailClass ADD CONSTRAINT FKd565412e3e3943e1a099ab7d28254011 FOREIGN KEY (MasterClass_m0) REFERENCES MasterClass; 
CREATE INDEX Index7c7c3e5252e142f0a27989821fe1207c on DetailClass (MasterClass_m0); 

 ALTER TABLE DetailClass ADD CONSTRAINT FKaa98f71c30c3402191bc30c8a1f0d906 FOREIGN KEY (MasterClass_m1) REFERENCES MasterClass; 
CREATE INDEX Index2de146b5ab0e433aae24b6e371015b87 on DetailClass (MasterClass_m1); 

 ALTER TABLE AggregatorUpdateObjectTest ADD CONSTRAINT FK089dbab029154d8fafc2439e497e2b9e FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Index04d7a213321844e6b80574b1a6289798 on AggregatorUpdateObjectTest (Detail); 

 ALTER TABLE Adress2 ADD CONSTRAINT FKa8f77bc8522344b2a51d66ed4614c5ed FOREIGN KEY (Country_m0) REFERENCES Country2; 
CREATE INDEX Index47b84c7509154220a4698c25b1beeaab on Adress2 (Country_m0); 

 ALTER TABLE FullTypesMainAgregator ADD CONSTRAINT FK537b0e9a9d82436d9c2a438ddce31cc9 FOREIGN KEY (FullTypesMaster1_m0) REFERENCES FullTypesMaster1; 
CREATE INDEX Index0acd8a97bb2a49da81228dd831cde2b4 on FullTypesMainAgregator (FullTypesMaster1_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKa4c655bf91c84e5aad9999cd329e8349 FOREIGN KEY (Ingridient2_m0) REFERENCES Cabbage2; 
CREATE INDEX Indexc6229cf88b594beba3fc6fca530969ba on Salad2 (Ingridient2_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK758cdc79dfbf4e18915d8990cdaa7ee7 FOREIGN KEY (Ingridient2_m1) REFERENCES Plant2; 
CREATE INDEX Indexb700ff40974140f68afe807b9be05421 on Salad2 (Ingridient2_m1); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKa5e930a8df4348d8b2454878a9e0bdb6 FOREIGN KEY (Ingridient1_m0) REFERENCES Cabbage2; 
CREATE INDEX Index71d9d897688f47118414cea9d1cced8f on Salad2 (Ingridient1_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK63c0fd7600f84cf089981376bf9a3a9f FOREIGN KEY (Ingridient1_m1) REFERENCES Plant2; 
CREATE INDEX Index70f3435dc4d44af4b9fa0d0b3e3a7d5d on Salad2 (Ingridient1_m1); 

 ALTER TABLE InformationTestClass3 ADD CONSTRAINT FK70f8f59928f04fcd96c316967d24ba0b FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Indexec95347254c74e45bf897874b0133941 on InformationTestClass3 (InformationTestClass2); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FK43d22a459d72423d83442a2fd9a88495 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Indexf3bd88779fa045aea626e4f9f68b4c5f on CombinedTypesUsageProviderTest (DataObjectForTest_m0); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FK2c9903d04c0b4e2e9c3d95b88412c08b FOREIGN KEY (TypeUsageProviderTestClass) REFERENCES TypeUsageProviderTestClass; 
CREATE INDEX Index711f1ec93f8b49a899b8e616c3211f4c on CombinedTypesUsageProviderTest (TypeUsageProviderTestClass); 

 ALTER TABLE ClassWithCaptions ADD CONSTRAINT FKb4335930f1934cb783e72d4cbcc07e68 FOREIGN KEY (InformationTestClass4) REFERENCES InformationTestClass4; 
CREATE INDEX Indexa67449eca7514441839747ea10fe2575 on ClassWithCaptions (InformationTestClass4); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FK2b704fe9f18b4046a86ed91bab4e2a07 FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Index986ac64def84434986ac6878cbd00467 on MasterUpdateObjectTest (Detail); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FK27c3f21d71ae4624981d0492a3093a33 FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Index6a87e7eb19a04b8c9aa49411a0e1437f on MasterUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE AuditAgregatorObject ADD CONSTRAINT FK62bf11ddceaa4505a8d3a5e41c78aa15 FOREIGN KEY (MasterObject) REFERENCES AuditMasterObject; 
CREATE INDEX Index31d002cb8d52474082d51627958bcf4b on AuditAgregatorObject (MasterObject); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FK49c3610fa910425a88a5043642d32a66 FOREIGN KEY (Личность_m0) REFERENCES Личность; 
CREATE INDEX Index3dceef936cbf49df9c7cb968d4f94dd9 on УчастникХозДог (Личность_m0); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FK4bce29f1e858490f9f6e1e16f1abb645 FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Indexa6f2da465e504a8fabe791be8569e2ca on УчастникХозДог (ХозДоговор_m0); 

 ALTER TABLE Кредит ADD CONSTRAINT FK28fb9278f95140a9bbd7312e06ecbd0c FOREIGN KEY (Клиент) REFERENCES Клиент; 
CREATE INDEX Index7ae7e6f3d2b1486c869efbbf284950d1 on Кредит (Клиент); 

 ALTER TABLE Кредит ADD CONSTRAINT FK37bf2d4ccaa5422291ed28a608c8f927 FOREIGN KEY (ИнспекторПоКред) REFERENCES ИнспПоКредиту; 
CREATE INDEX Index56df8abe94db4042b2e24d406be886df on Кредит (ИнспекторПоКред); 

 ALTER TABLE Кошка ADD CONSTRAINT FKb7603c22189d48bca46512b1a7d02c2a FOREIGN KEY (Порода) REFERENCES Порода; 
CREATE INDEX Index14e224de69b14843ab7d02c2e0625b19 on Кошка (Порода); 

 ALTER TABLE FullTypesDetail2 ADD CONSTRAINT FK73744778928c4e37a897313a9749deae FOREIGN KEY (FullTypesMainAgregator) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Indexddf3a52a5d604315b0bfaed5349a1101 on FullTypesDetail2 (FullTypesMainAgregator); 

 ALTER TABLE CabbagePart2 ADD CONSTRAINT FK4d508bc6af934857b92d3a2586457e5f FOREIGN KEY (Cabbage) REFERENCES Cabbage2; 
CREATE INDEX Indexa34280f12822445980dce237d81bb1ff on CabbagePart2 (Cabbage); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FK3a277c05c6544809b02990f72b7277e6 FOREIGN KEY (Master) REFERENCES MasterUpdateObjectTest; 
CREATE INDEX Indexca8ed779461c43f390eb7dc8b6cc1e70 on DetailUpdateObjectTest (Master); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FK18e5e519f51743a48f67c91d71983e25 FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Index064c435f4e324c8da4b5f97ca19ba4d8 on DetailUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FKfc64fa01f66b40979c3eb811e2926f2f FOREIGN KEY (Владелец_m0) REFERENCES Пользователь; 
CREATE INDEX Indexe475ac220cb84b1d8f1095e6301d3d8d on ФайлИдеи (Владелец_m0); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FK12cd4fccd43d4c4cb0c7218b6872256c FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Indexd435b8b7378145ec964869c687cd8d6a on ФайлИдеи (Идея_m0); 

 ALTER TABLE ЭтапИсходящегоЗапроса ADD CONSTRAINT FK36adcce2a8e943d888c360a109d5f8c7 FOREIGN KEY (Конфигурация) REFERENCES КонфигурацияЗапроса; 
CREATE INDEX Indexc099ed3a786f471f9836c5d78edca75b on ЭтапИсходящегоЗапроса (Конфигурация); 

 ALTER TABLE ЭтапИсходящегоЗапроса ADD CONSTRAINT FKb7aa52a1d0c64dd3889ded5cbcc66f13 FOREIGN KEY (Запросы) REFERENCES ИсходящийЗапрос; 
CREATE INDEX Index9df9b4fc30464dbe8b6d9fe0d5abaf5d on ЭтапИсходящегоЗапроса (Запросы); 

 ALTER TABLE cla ADD CONSTRAINT FKac0a75d3bf054f7195b0192ce20aef0f FOREIGN KEY (parent) REFERENCES clb; 
CREATE INDEX Index8573427c23d5495fa805f42b79328e1b on cla (parent); 

 ALTER TABLE Конкурс ADD CONSTRAINT FK6b6923c316834d40a32a5af329819b9b FOREIGN KEY (Организатор_m0) REFERENCES Пользователь; 
CREATE INDEX Indexc1bb14ec2c024cfebc8079e763f71847 on Конкурс (Организатор_m0); 

 ALTER TABLE TypeUsageProviderTestClass ADD CONSTRAINT FK820da36c312549f3b92a05c877c889b5 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Index7630f38b65284281a581d26b6568d530 on TypeUsageProviderTestClass (DataObjectForTest_m0); 

 ALTER TABLE InformationTestClass6 ADD CONSTRAINT FK158eaf4a4f9a427391b968a8a7c00dc2 FOREIGN KEY (ExampleOfClassWithCaptions) REFERENCES ClassWithCaptions; 
CREATE INDEX Index8ac1e2da5c644004a2e2d74b3a8b2287 on InformationTestClass6 (ExampleOfClassWithCaptions); 

 ALTER TABLE Идея ADD CONSTRAINT FK06c1c5ee7e2348a6ad93b9975dadae7b FOREIGN KEY (Автор_m0) REFERENCES Пользователь; 
CREATE INDEX Index3a60c2d134694aa38ba6e7c02abf580f on Идея (Автор_m0); 

 ALTER TABLE Идея ADD CONSTRAINT FK096cccce4e4b4336a1886f33f93d8f51 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index91ec36068d33438683db7caafb3b62df on Идея (Конкурс_m0); 

 ALTER TABLE Лапа ADD CONSTRAINT FK75bf9e3fdf2f4252bb593e3c38b8d5f1 FOREIGN KEY (ТипЛапы_m0) REFERENCES ТипЛапы; 
CREATE INDEX Index48646e10def8434682fed962942e82f3 on Лапа (ТипЛапы_m0); 

 ALTER TABLE Лапа ADD CONSTRAINT FKb5ba0f8113f54c8eaf00536fde753981 FOREIGN KEY (Кошка_m0) REFERENCES Кошка; 
CREATE INDEX Index1b726f6e65c247cca3d69aafe854c384 on Лапа (Кошка_m0); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FK1bb53338d5dd493087eb5f702be79e6d FOREIGN KEY (Критерий_m0) REFERENCES КритерийОценки; 
CREATE INDEX Index74fcfcd50af14c61b18e3ad04d3b05e0 on ЗначениеКритер (Критерий_m0); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FKb8a4f903fc3940649ea8e1284f6748fa FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index0370e3fd8bb045bd8c63529c56b9ff39 on ЗначениеКритер (Идея_m0); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FK49bdecfe34cd47e797b8868f5602e41b FOREIGN KEY (Cabbage1) REFERENCES Cabbage2; 
CREATE INDEX Index419014ffa73342e5b0ea65921a85c1c5 on CabbageSalad (Cabbage1); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FK1b9c38a9fc8040ac9fa9e0e49ae4d075 FOREIGN KEY (Cabbage2) REFERENCES Cabbage2; 
CREATE INDEX Index185c52896b664f1abec8451ad83c23b5 on CabbageSalad (Cabbage2); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FKc092ec3e639b4221a89f6dcc78a0b59a FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Index0e9ab1280d5448b4a10f72df05a33e30 on InformationTestClass2 (InformationTestClass_m0); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FK23619536c40e40f788fb22b744d1c061 FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Indexeef9c27b406d45a78734b6dcdecf8ab2 on InformationTestClass2 (InformationTestClass_m1); 

 ALTER TABLE Котенок ADD CONSTRAINT FK49a319da404447058ae88bd41a7633a2 FOREIGN KEY (Кошка) REFERENCES Кошка; 
CREATE INDEX Indexd2b2e6dfb8624f54a7c76025a3503322 on Котенок (Кошка); 

 ALTER TABLE Region ADD CONSTRAINT FKb56815870651495d8c6d27ce6d356bc3 FOREIGN KEY (Country2_m0) REFERENCES Country2; 
CREATE INDEX Index62f175dad65547169a038fb7b4425953 on Region (Country2_m0); 

 ALTER TABLE FullTypesDetail1 ADD CONSTRAINT FK1cf74fed9daf4cd2bf708497d75b4096 FOREIGN KEY (FullTypesMainAgregator_m0) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Indexae9f547cb235403f96b65b5c977577e5 on FullTypesDetail1 (FullTypesMainAgregator_m0); 

 ALTER TABLE Порода ADD CONSTRAINT FKbdc12b85b1bb48e184a960beacaa3142 FOREIGN KEY (ТипПороды) REFERENCES ТипПороды; 
CREATE INDEX Index248c7b1be62c41ce8771718d24dcbe09 on Порода (ТипПороды); 

 ALTER TABLE Порода ADD CONSTRAINT FKaaeeeba8cb844138a62fc0161985b52f FOREIGN KEY (Иерархия) REFERENCES Порода; 
CREATE INDEX Index2b75b9a7de1c440c828c821ac8c0c82e on Порода (Иерархия); 

 ALTER TABLE InformationTestClass4 ADD CONSTRAINT FKcf834abb45c34037964b06c383762f78 FOREIGN KEY (MasterOfInformationTestClass3) REFERENCES InformationTestClass3; 
CREATE INDEX Indexd6020e9b0b8443b28ba82d5a279fcb86 on InformationTestClass4 (MasterOfInformationTestClass3); 

 ALTER TABLE КритерийОценки ADD CONSTRAINT FK2ed3d8981dc841b08b82a0384ee596a3 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Indexef979be0064b44e8a1787963929dfd7d on КритерийОценки (Конкурс_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FK43edba9cc1074d23bd2f206360ad084a FOREIGN KEY (TomorrowTeritory_m0) REFERENCES Country2; 
CREATE INDEX Index4c6f3569b535401fba08f1993620f317 on Place2 (TomorrowTeritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FKe69bcf36f8f445ab8d61e3c12f5ee9f8 FOREIGN KEY (TomorrowTeritory_m1) REFERENCES Territory2; 
CREATE INDEX Indexcd5c9a1fce6e43a8a0b547a78fabb306 on Place2 (TomorrowTeritory_m1); 

 ALTER TABLE Place2 ADD CONSTRAINT FKc6755d65aba146f38d73d9f70c5b91b2 FOREIGN KEY (TodayTerritory_m0) REFERENCES Country2; 
CREATE INDEX Index583752d68fcd413da8ff6b285ab3fc1f on Place2 (TodayTerritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FKc592d994916342d49ee90953fcaa4565 FOREIGN KEY (TodayTerritory_m1) REFERENCES Territory2; 
CREATE INDEX Indexde0b92bd0621453dbbbe82f242b6be98 on Place2 (TodayTerritory_m1); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FKe18d32644c714fb090e1af402c21efa5 FOREIGN KEY (ИсточникФинан) REFERENCES ИсточникФинанс; 
CREATE INDEX Index16546cd733c64fc79895843765329a26 on ИФХозДоговора (ИсточникФинан); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FK930cb17a50214e5c892e560384b1d010 FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Index721267d2d27a4d8bba1eeb80a574ddf5 on ИФХозДоговора (ХозДоговор_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK1c272d4df33c4592b942258be413c43d FOREIGN KEY (InformationTestClass3_m0) REFERENCES InformationTestClass3; 
CREATE INDEX Indexa1f89d3ba5154fa2898657d2a3fe7688 on MasterClass (InformationTestClass3_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK815c1e2d99a14815902cd4c83f2333f8 FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Index9e7d55de00c74cf5958c7cb685f0f551 on MasterClass (InformationTestClass2); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK18553cd3cd0147668f1bd7dd1e2867c7 FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Index239777f04711427c822a148cbb1d8de8 on MasterClass (InformationTestClass_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK7947a7a6a09646b1a37570d152741df5 FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Index8c790681a16049fa9535eadb32af3047 on MasterClass (InformationTestClass_m1); 

 ALTER TABLE Human2 ADD CONSTRAINT FKe1c330380fc644e683e0e3cdb9766e88 FOREIGN KEY (TodayHome_m0) REFERENCES Country2; 
CREATE INDEX Indexe23de9217c5e477f9e5c7eda4379a040 on Human2 (TodayHome_m0); 

 ALTER TABLE Human2 ADD CONSTRAINT FK58b2fdc382cb406890157dbe695ed8b4 FOREIGN KEY (TodayHome_m1) REFERENCES Territory2; 
CREATE INDEX Indexeaa577cd270e4c9e93b2dbea4e66cd0f on Human2 (TodayHome_m1); 

 ALTER TABLE SomeDetailClass ADD CONSTRAINT FK1a88f637af4f4e7cb5dd4bf3dcf021c9 FOREIGN KEY (ClassA) REFERENCES SomeMasterClass; 
CREATE INDEX Index324ab761b9534752aefc480cb41b3f42 on SomeDetailClass (ClassA); 

 ALTER TABLE STORMWEBSEARCH ADD CONSTRAINT FK1e7eeea73ac940a7a450eb80d674fb26 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERDETAIL ADD CONSTRAINT FK6f4b7fe6c7504b36b4912952df8623e5 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERLOOKUP ADD CONSTRAINT FK95abd17368484e308f4380abf68fd227 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMAuEntity ADD CONSTRAINT FKfd841c3369d94c96bedfead0d895c75f FOREIGN KEY (ObjectType_m0) REFERENCES STORMAuObjType; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FK90f7744da25f479ba77ee3dc53fb5c03 FOREIGN KEY (MainChange_m0) REFERENCES STORMAuField; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FK0c737ffc8791494d805e080cc825e147 FOREIGN KEY (AuditEntity_m0) REFERENCES STORMAuEntity; 

