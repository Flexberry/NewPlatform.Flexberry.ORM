



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




 ALTER TABLE Apparatus2 ADD CONSTRAINT FKbf9319a4bd154bc9b9804752b0501bfa FOREIGN KEY (Maker_m0) REFERENCES Country2; 
CREATE INDEX Index50307ef1530e49abbffadf3650caa571 on Apparatus2 (Maker_m0); 

 ALTER TABLE Apparatus2 ADD CONSTRAINT FK1c91202c30b54169845fbaf0cc100ad9 FOREIGN KEY (Exporter_m0) REFERENCES Country2; 
CREATE INDEX Index0feea3c0fffe480285f02bab11648c43 on Apparatus2 (Exporter_m0); 

 ALTER TABLE ComputedDetail ADD CONSTRAINT FK4f7280b356dd443fb2bcd4adccef72f4 FOREIGN KEY (ComputedMaster) REFERENCES ComputedMaster; 
CREATE INDEX Indexe1c44184fa2e4c05b2c5a8bcb50ed3a3 on ComputedDetail (ComputedMaster); 

 ALTER TABLE Выплаты ADD CONSTRAINT FKe0e942f81d2e425287bb789ea42e2815 FOREIGN KEY (Кредит1) REFERENCES Кредит; 
CREATE INDEX Indexd9cd9fd841f54269b7ed87ed6cb373ce on Выплаты (Кредит1); 

 ALTER TABLE Блоха ADD CONSTRAINT FK853ed79eea694504bfcba293539d60e7 FOREIGN KEY (МедведьОбитания) REFERENCES Медведь; 
CREATE INDEX Index4094f21efe8743549a75707e16e11137 on Блоха (МедведьОбитания); 

 ALTER TABLE Этап ADD CONSTRAINT FKa38702a67bd54caa96e11361c9d11e56 FOREIGN KEY (КонфигурацияЭтапа_m0) REFERENCES КонфигурацияЭтапа; 
CREATE INDEX Index0dbb4608c9844d088e5bc94caf65719b on Этап (КонфигурацияЭтапа_m0); 

 ALTER TABLE Этап ADD CONSTRAINT FK6dde97f0feb3470dbff2e3a877560ff2 FOREIGN KEY (Запрос) REFERENCES Запрос; 
CREATE INDEX Index7cd7fce7028d41b09d7df7b05558b7da on Этап (Запрос); 

 ALTER TABLE AuditMasterObject ADD CONSTRAINT FKaa22588352f14286b33edd7dc2ef1fef FOREIGN KEY (MasterObject) REFERENCES AuditMasterMasterObject; 
CREATE INDEX Indexd8bfc8c5bb1c4e46ae0e9bd51b8a8b29 on AuditMasterObject (MasterObject); 

 ALTER TABLE Берлога ADD CONSTRAINT FKf7df2befa45045acb408cf06967c1005 FOREIGN KEY (ЛесРасположения) REFERENCES Лес; 
CREATE INDEX Indexa19d455fcef24a1d9e24836e07d56592 on Берлога (ЛесРасположения); 

 ALTER TABLE Берлога ADD CONSTRAINT FK492d99e0786a4a3eac091e48261f7070 FOREIGN KEY (Медведь) REFERENCES Медведь; 
CREATE INDEX Indexaffedb93e8a7410dadc0f23810446274 on Берлога (Медведь); 

 ALTER TABLE ДокККонкурсу ADD CONSTRAINT FK307f12a4a85842d29a9791422df437fb FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index9c25142a82dc4677bfb79c1d007101c6 on ДокККонкурсу (Конкурс_m0); 

 ALTER TABLE clb ADD CONSTRAINT FK29567279632d4cca9c5c41045b69e6c2 FOREIGN KEY (ref2) REFERENCES cla; 
CREATE INDEX Index1df6933e4bfc4d74be0d183d0a6e15f4 on clb (ref2); 

 ALTER TABLE clb ADD CONSTRAINT FK9cde053231734f0aa8fe9dc1bcf275a0 FOREIGN KEY (ref1) REFERENCES cla; 
CREATE INDEX Indexef5f992c299e4eb2ab9815a3feb0e5dc on clb (ref1); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK347e218695594a5680ce2b43995f6eed FOREIGN KEY (Эксперт_m0) REFERENCES Пользователь; 
CREATE INDEX Index3ec75b415bd6496ab3231faccf5eda95 on ОценкаЭксперта (Эксперт_m0); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK5c4a6fc24bdb457299083755d34c6b5a FOREIGN KEY (ЗначениеКритер) REFERENCES ЗначениеКритер; 
CREATE INDEX Indexe63612cb87664c449989b68566d3ffe1 on ОценкаЭксперта (ЗначениеКритер); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FKbab19ae05b6f486cbdb4d6c640fc4adc FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index4a1f87837a984b82a758368de95b27ab on ОценкаЭксперта (Идея_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK5b19be60c0a3462b936837952a46e3e5 FOREIGN KEY (MainIngridient_m0) REFERENCES Cabbage2; 
CREATE INDEX Index87091a6c15084b18ac479a97e302efd6 on Dish2 (MainIngridient_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK4419ca1d49344f9b874dc32f3a32d9df FOREIGN KEY (MainIngridient_m1) REFERENCES Plant2; 
CREATE INDEX Indexa14704d0866b42988e02f788fb8e0ecd on Dish2 (MainIngridient_m1); 

 ALTER TABLE Медведь ADD CONSTRAINT FKcc377c3ceeaa4a15b8f6f8b2a99ff825 FOREIGN KEY (Мама) REFERENCES Медведь; 
CREATE INDEX Indexe83571e0fb8146c78c2adf6e0c731473 on Медведь (Мама); 

 ALTER TABLE Медведь ADD CONSTRAINT FKe8f43727be064a928b0238a160a3d7f7 FOREIGN KEY (ЛесОбитания) REFERENCES Лес; 
CREATE INDEX Indexb1d76dacd0d445ab9c4f09dd839e4030 on Медведь (ЛесОбитания); 

 ALTER TABLE Медведь ADD CONSTRAINT FK4464c2a03bae41d18824ad33edb77fc4 FOREIGN KEY (Папа) REFERENCES Медведь; 
CREATE INDEX Indexa639eab3e52040809e8a12429bee093c on Медведь (Папа); 

 ALTER TABLE Медведь ADD CONSTRAINT FK0443c6b7423744a489ac3cd5052ced1f FOREIGN KEY (Друг_m0) REFERENCES Медведь; 
CREATE INDEX Indexd9bc76723f9642f392ee852790d56f7d on Медведь (Друг_m0); 

 ALTER TABLE TypeUsageProviderTestClassChil ADD CONSTRAINT FKeafb7d83193248b2900be87a0a511c3b FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Index45605d3ac0e249e6ababd7e0f1c9a1ad on TypeUsageProviderTestClassChil (DataObjectForTest_m0); 

 ALTER TABLE Soup2 ADD CONSTRAINT FK2ef943576175443ebfd14a8566fbf85e FOREIGN KEY (CabbageType) REFERENCES Cabbage2; 
CREATE INDEX Index2cbf2d29364b4a8c898c8552e18eb3f5 on Soup2 (CabbageType); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK3e4f41572c4c44708f26960df36c9161 FOREIGN KEY (Мастер_m0) REFERENCES МастерМ; 
CREATE INDEX Index026474f066e04f339452931a2d7437e2 on TestClassA (Мастер_m0); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK70863f34a13f4b87ac9927ed3e7290c8 FOREIGN KEY (Мастер_m1) REFERENCES НаследникМ1; 
CREATE INDEX Index0d1fbae8eeb14ca69482775e79eae71e on TestClassA (Мастер_m1); 

 ALTER TABLE TestClassA ADD CONSTRAINT FKeec4225622824a4fb5d15b340e5d3d8f FOREIGN KEY (Мастер_m2) REFERENCES НаследникМ2; 
CREATE INDEX Index7f068d9ab07d49a687a1b4e142aef93f on TestClassA (Мастер_m2); 

 ALTER TABLE Перелом ADD CONSTRAINT FK8cdcab55b5724c51815ce16d9010e322 FOREIGN KEY (Лапа_m0) REFERENCES Лапа; 
CREATE INDEX Indexc07f2215a5ee4e108cfde4332ab9276f on Перелом (Лапа_m0); 

 ALTER TABLE Лес ADD CONSTRAINT FKcef9e55fc1184758bf879f0fc42cc2e1 FOREIGN KEY (Страна) REFERENCES Страна; 
CREATE INDEX Indexbbf7a3fb373b410fbc4a3906a1414dee on Лес (Страна); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK6a9adb3b23f247a797c7061e8ab3a555 FOREIGN KEY (MasterClass_m0) REFERENCES MasterClass; 
CREATE INDEX Index62ebe0cb18f54eb7ae43dec3adc875b5 on DetailClass (MasterClass_m0); 

 ALTER TABLE DetailClass ADD CONSTRAINT FKa2069337dfc04ec2a4c7ee96b550d7cf FOREIGN KEY (MasterClass_m1) REFERENCES MasterClass; 
CREATE INDEX Index9bb7d5d3b74a41738e854ea818f7b989 on DetailClass (MasterClass_m1); 

 ALTER TABLE AggregatorUpdateObjectTest ADD CONSTRAINT FK8a12bd37202142d0b848626ec6558f7a FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Indexd9465d1098fd43649816e1613da0c016 on AggregatorUpdateObjectTest (Detail); 

 ALTER TABLE Adress2 ADD CONSTRAINT FK8a6efaf60f5949deaac94bbfbd7dd985 FOREIGN KEY (Country_m0) REFERENCES Country2; 
CREATE INDEX Indexa499001216334115bcf8cb322993ff8c on Adress2 (Country_m0); 

 ALTER TABLE FullTypesMainAgregator ADD CONSTRAINT FK3afb42f616034bef80e78b7ae325dbb1 FOREIGN KEY (FullTypesMaster1_m0) REFERENCES FullTypesMaster1; 
CREATE INDEX Index7e8104bb4eca4acb8d7e5083d5cea50a on FullTypesMainAgregator (FullTypesMaster1_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK4d6016db75ad48598fdc31526c5241ea FOREIGN KEY (Ingridient2_m0) REFERENCES Cabbage2; 
CREATE INDEX Indexe8b245977a1b461b8295b087fcfc7991 on Salad2 (Ingridient2_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKef64d40078e84554b216865302257a4a FOREIGN KEY (Ingridient2_m1) REFERENCES Plant2; 
CREATE INDEX Index81c61ad370034106a4fd475f4790d989 on Salad2 (Ingridient2_m1); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK5af01d54a3644a17929189ce2d544c09 FOREIGN KEY (Ingridient1_m0) REFERENCES Cabbage2; 
CREATE INDEX Indexe6f057638d2840cd949c754a66df3dc3 on Salad2 (Ingridient1_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKcbe3f76004c84b82acc0123f56a2c811 FOREIGN KEY (Ingridient1_m1) REFERENCES Plant2; 
CREATE INDEX Indexe87887a49d024bf2be23b6d8e4695a81 on Salad2 (Ingridient1_m1); 

 ALTER TABLE InformationTestClass3 ADD CONSTRAINT FK96f1f095d8af43008c18e641c3776e86 FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Index7c7a06144776412a884ab4f63b6c77a6 on InformationTestClass3 (InformationTestClass2); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FK22e08f5712e5434b898b5ce82166f383 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Indexe01292ac4e044b06b1498c285799e2c5 on CombinedTypesUsageProviderTest (DataObjectForTest_m0); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FK25ef33d4a27e403197183629b511bf9d FOREIGN KEY (TypeUsageProviderTestClass) REFERENCES TypeUsageProviderTestClass; 
CREATE INDEX Indexd943c6fca7bd490d84dcfc7c12838482 on CombinedTypesUsageProviderTest (TypeUsageProviderTestClass); 

 ALTER TABLE ClassWithCaptions ADD CONSTRAINT FK478ddd2a91794808be104f329d4e033c FOREIGN KEY (InformationTestClass4) REFERENCES InformationTestClass4; 
CREATE INDEX Indexec48cc78aba842d0bc49102feaef016f on ClassWithCaptions (InformationTestClass4); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FKa67d2c23470e4969bf8d80bc3b5fae60 FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Index6c13f807d611437caaf49a95e7e5e650 on MasterUpdateObjectTest (Detail); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FKd44607ca556a40449e3441e3a6049506 FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Index4dbd8afcd2e841ecbdb6d8783bea5ac3 on MasterUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE AuditAgregatorObject ADD CONSTRAINT FK993c6798bcde494fa42788d52260ccce FOREIGN KEY (MasterObject) REFERENCES AuditMasterObject; 
CREATE INDEX Index5b2d6e9284ce42898c72fe4e178aa8bf on AuditAgregatorObject (MasterObject); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FK119f9851266e4b969123950efa8dd62f FOREIGN KEY (Личность_m0) REFERENCES Личность; 
CREATE INDEX Index5f68f164f967484ebcb5ad3ff015eea9 on УчастникХозДог (Личность_m0); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FK118af27c9e9e4aee8fee3cd773e81974 FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Index951e8908686541e09cec06347a17b36e on УчастникХозДог (ХозДоговор_m0); 

 ALTER TABLE Кредит ADD CONSTRAINT FK688db386910c4e5d820f4b91bac84ad8 FOREIGN KEY (Клиент) REFERENCES Клиент; 
CREATE INDEX Index8548ae20576e4c6a9238bf498d2676cb on Кредит (Клиент); 

 ALTER TABLE Кредит ADD CONSTRAINT FK068e4c99e1964dfeb332ae5774aeda77 FOREIGN KEY (ИнспекторПоКред) REFERENCES ИнспПоКредиту; 
CREATE INDEX Index4e9cc609073240e298ba67fde3cb4351 on Кредит (ИнспекторПоКред); 

 ALTER TABLE Кошка ADD CONSTRAINT FK0a2d85ddd6ec4c39845349245e2936cf FOREIGN KEY (Порода) REFERENCES Порода; 
CREATE INDEX Index4bfcad668ef3407d95b92beaa75d3ecf on Кошка (Порода); 

 ALTER TABLE FullTypesDetail2 ADD CONSTRAINT FK35c0cc091ec14910a5a18e2823aa0186 FOREIGN KEY (FullTypesMainAgregator) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Indexa016e1d2c44a46fd8d818560dba2a220 on FullTypesDetail2 (FullTypesMainAgregator); 

 ALTER TABLE CabbagePart2 ADD CONSTRAINT FK53612fea76bf4b5c8538da3097951f31 FOREIGN KEY (Cabbage) REFERENCES Cabbage2; 
CREATE INDEX Index183172915d3645bbaa03be831ee7e20b on CabbagePart2 (Cabbage); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FK3b56ad6a7844407681ce07c9cd176997 FOREIGN KEY (Master) REFERENCES MasterUpdateObjectTest; 
CREATE INDEX Index15fb4303d8d74dd0b4871fa4ddd8c799 on DetailUpdateObjectTest (Master); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FKccfcff3bb21441e69f15fab763939d6a FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Indexd6d59fb48597402e947fb25b7a24c8a7 on DetailUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FKd4c45cda345c4a95931965a3e98e81b9 FOREIGN KEY (Владелец_m0) REFERENCES Пользователь; 
CREATE INDEX Indexef1edc37ae2c4c8e83d216bd6b498f2b on ФайлИдеи (Владелец_m0); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FKf1f1e5701d634f59bc73badfbbc434f3 FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Indexb3c69b3532b442b381440437346c4ce0 on ФайлИдеи (Идея_m0); 

 ALTER TABLE ЭтапИсходящегоЗапроса ADD CONSTRAINT FK3a09475bfa794227bbd4ad4fbd3b2dd6 FOREIGN KEY (Конфигурация) REFERENCES КонфигурацияЗапроса; 
CREATE INDEX Indexae4e8896ffc548309ccfce7decd0bfbf on ЭтапИсходящегоЗапроса (Конфигурация); 

 ALTER TABLE ЭтапИсходящегоЗапроса ADD CONSTRAINT FK9f3a9bb4274845499d68c313c441a18d FOREIGN KEY (Запросы) REFERENCES ИсходящийЗапрос; 
CREATE INDEX Index471da097c2d1419ca4e2fcef6f03d3b7 on ЭтапИсходящегоЗапроса (Запросы); 

 ALTER TABLE cla ADD CONSTRAINT FK8ced7390e62a4a94bb4ffd02ada95c65 FOREIGN KEY (parent) REFERENCES clb; 
CREATE INDEX Indexd11ec014371342b691e58ea5d1cbd0f6 on cla (parent); 

 ALTER TABLE Конкурс ADD CONSTRAINT FK9b354451f2d843b294361d13c3fc2ac5 FOREIGN KEY (Организатор_m0) REFERENCES Пользователь; 
CREATE INDEX Index09efa93d64e943d5b662c7ae22fb81c2 on Конкурс (Организатор_m0); 

 ALTER TABLE TypeUsageProviderTestClass ADD CONSTRAINT FK258854abe2434c2991b34408f8b5c175 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Indexbdaf60d6a2e94e49ae1c33025970e884 on TypeUsageProviderTestClass (DataObjectForTest_m0); 

 ALTER TABLE InformationTestClass6 ADD CONSTRAINT FK5e971851d78c4f669061f6f737110f15 FOREIGN KEY (ExampleOfClassWithCaptions) REFERENCES ClassWithCaptions; 
CREATE INDEX Index508063c243244ca39ee37617abaedabd on InformationTestClass6 (ExampleOfClassWithCaptions); 

 ALTER TABLE Идея ADD CONSTRAINT FK4b36e82d350a4272ae1de40a9f0a39c4 FOREIGN KEY (Автор_m0) REFERENCES Пользователь; 
CREATE INDEX Indexf5d3d0d685d24198846f9bbf25aabc72 on Идея (Автор_m0); 

 ALTER TABLE Идея ADD CONSTRAINT FK05313e9b8eff451f9692a54126cb4fdc FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index880b3f571fe446a28e2fb4da8f46d576 on Идея (Конкурс_m0); 

 ALTER TABLE Лапа ADD CONSTRAINT FK2a52d113b5034de9b744ebc2066892e9 FOREIGN KEY (ТипЛапы_m0) REFERENCES ТипЛапы; 
CREATE INDEX Index6a2000e26fef49b3b69a060473a5987c on Лапа (ТипЛапы_m0); 

 ALTER TABLE Лапа ADD CONSTRAINT FK046418184b6346e583b85b244147f981 FOREIGN KEY (Кошка_m0) REFERENCES Кошка; 
CREATE INDEX Indexf2dc4e026d804921aa976144c72c4366 on Лапа (Кошка_m0); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FK81e070ed28b64773b43f9c8f77db4241 FOREIGN KEY (Критерий_m0) REFERENCES КритерийОценки; 
CREATE INDEX Indexb5dd5783195e449ba212b29cf63ca1a2 on ЗначениеКритер (Критерий_m0); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FKed163539a358458c835d25d9f4f3d6af FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index59a42aa8bc3a4c3b8414777f3e0fad59 on ЗначениеКритер (Идея_m0); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FK228e9f3466f3476e97b811fc933f9000 FOREIGN KEY (Cabbage1) REFERENCES Cabbage2; 
CREATE INDEX Index3e4e1a48775340b0995e6c309c7b5a9b on CabbageSalad (Cabbage1); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FK2dc35373aa60489abe6808da1df53867 FOREIGN KEY (Cabbage2) REFERENCES Cabbage2; 
CREATE INDEX Index36e6cead146448409590458fe6dc7e1c on CabbageSalad (Cabbage2); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FK0030d61a9cd443b79a1e43269ff89aa3 FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Indexa4c9ce55e06b4310ad6c37f8f9cbaeb4 on InformationTestClass2 (InformationTestClass_m0); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FKe3f7f10043434ea4b7c1139a7b57ec99 FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Index713b46e96c714cb89544b4ca5032def1 on InformationTestClass2 (InformationTestClass_m1); 

 ALTER TABLE Котенок ADD CONSTRAINT FK5ec05479362c4b4394f4d8bb3508a1f5 FOREIGN KEY (Кошка) REFERENCES Кошка; 
CREATE INDEX Index0df31c0dafa14660ab0c163e17856a6b on Котенок (Кошка); 

 ALTER TABLE Region ADD CONSTRAINT FKc4cc2bdb0a414c52991fe7fa4ef1ff77 FOREIGN KEY (Country2_m0) REFERENCES Country2; 
CREATE INDEX Index03655577f2a44e3383ffa8c010bcd228 on Region (Country2_m0); 

 ALTER TABLE FullTypesDetail1 ADD CONSTRAINT FK75301d6e92104e83869783dba911be60 FOREIGN KEY (FullTypesMainAgregator_m0) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Indexac60e739dd524c7a8c9c32bd93fde66d on FullTypesDetail1 (FullTypesMainAgregator_m0); 

 ALTER TABLE Порода ADD CONSTRAINT FK5565dce8a0b049ea9390250ca6cdec6e FOREIGN KEY (ТипПороды) REFERENCES ТипПороды; 
CREATE INDEX Index0aa0a62c972d49d2b91df4918c53358a on Порода (ТипПороды); 

 ALTER TABLE Порода ADD CONSTRAINT FK95057df4dc0e4c35a8806d64e298ca05 FOREIGN KEY (Иерархия) REFERENCES Порода; 
CREATE INDEX Index7355be1d300e46adb728215c284d5d47 on Порода (Иерархия); 

 ALTER TABLE InformationTestClass4 ADD CONSTRAINT FK7b37833423c84786bbc23226c1779fcd FOREIGN KEY (MasterOfInformationTestClass3) REFERENCES InformationTestClass3; 
CREATE INDEX Indexe81fd9efe84d495f863d9eda1f576109 on InformationTestClass4 (MasterOfInformationTestClass3); 

 ALTER TABLE КритерийОценки ADD CONSTRAINT FKa03c496eba0142378ac45e066ac401f9 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index0b56fdd913f84616aa3c414ad5e31215 on КритерийОценки (Конкурс_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FK371d3b0f5e3f4e4593f9a3d397dcef97 FOREIGN KEY (TomorrowTeritory_m0) REFERENCES Country2; 
CREATE INDEX Index0c316ae4c6d3461f8d2544cb2e3b89c3 on Place2 (TomorrowTeritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FKaa3b55d025944d59aef49d66db72a065 FOREIGN KEY (TomorrowTeritory_m1) REFERENCES Territory2; 
CREATE INDEX Index93a28a40d0fc40abb3190cb9d622d469 on Place2 (TomorrowTeritory_m1); 

 ALTER TABLE Place2 ADD CONSTRAINT FK7ba4ed3d1de14017892cbaa4ce4b0e90 FOREIGN KEY (TodayTerritory_m0) REFERENCES Country2; 
CREATE INDEX Indexb6cb28f4525e4d81ad364708305643d5 on Place2 (TodayTerritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FKeb41dec781eb4c8ba599da6780d7f95f FOREIGN KEY (TodayTerritory_m1) REFERENCES Territory2; 
CREATE INDEX Indexff565d3cd92b4b738d1f53bfc2b2af0b on Place2 (TodayTerritory_m1); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FK5e30cbc7096945349023fe1c5519c13d FOREIGN KEY (ИсточникФинан) REFERENCES ИсточникФинанс; 
CREATE INDEX Indexe9b300ba05e743d69618614d7ff89a2c on ИФХозДоговора (ИсточникФинан); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FK82108ccf4c374911ba04f40bfe0499c2 FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Index53c36ea94cd4496aa1a8928dc2705359 on ИФХозДоговора (ХозДоговор_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FKb57b426602534116a8189193a2066d3f FOREIGN KEY (InformationTestClass3_m0) REFERENCES InformationTestClass3; 
CREATE INDEX Indexbecf96037abe453589f3116e65d93db1 on MasterClass (InformationTestClass3_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK44f4f0950f3644f8bdbe9369e9ad7f95 FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Index565a34f409924b669676a3a6e5dbc6c0 on MasterClass (InformationTestClass2); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK783b82c58812431e9dba0dc9aac877be FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Index1d00677575b04b32a1372d18e18ef296 on MasterClass (InformationTestClass_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK327837d97ba848b5b8bd76c35d5c8c7a FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Indexcb8115f85db4468481c6f100e1e18927 on MasterClass (InformationTestClass_m1); 

 ALTER TABLE Human2 ADD CONSTRAINT FKd4a4156fcb2444eaa3ff78529839948f FOREIGN KEY (TodayHome_m0) REFERENCES Country2; 
CREATE INDEX Index2a02a7185a434fe1bcd4267b57d7d5fd on Human2 (TodayHome_m0); 

 ALTER TABLE Human2 ADD CONSTRAINT FK6ad465efc5bc4c10a56f26c7ff1dd073 FOREIGN KEY (TodayHome_m1) REFERENCES Territory2; 
CREATE INDEX Indexcff300aba5e845fab3614910cef8d0b4 on Human2 (TodayHome_m1); 

 ALTER TABLE SomeDetailClass ADD CONSTRAINT FKf8cfcc8b362142718656ffaf45d2fd8f FOREIGN KEY (ClassA) REFERENCES SomeMasterClass; 
CREATE INDEX Indexae23024e6a33473e81915b30217ed322 on SomeDetailClass (ClassA); 

 ALTER TABLE STORMWEBSEARCH ADD CONSTRAINT FK5288cae8fe1745699f0f8b1ec7be7008 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERDETAIL ADD CONSTRAINT FK61d479c7cdf44ef581e6b678760ddb75 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERLOOKUP ADD CONSTRAINT FK46ccd3f83dc84017a5c4af548c9df68f FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMAuEntity ADD CONSTRAINT FKb44274fd995b419d9311a2b9c75ffb89 FOREIGN KEY (ObjectType_m0) REFERENCES STORMAuObjType; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FKd2c7fac7b1de430da4d8d801a194a091 FOREIGN KEY (MainChange_m0) REFERENCES STORMAuField; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FK221b6f1af99a4d5bae3c946d9eca487e FOREIGN KEY (AuditEntity_m0) REFERENCES STORMAuEntity; 

