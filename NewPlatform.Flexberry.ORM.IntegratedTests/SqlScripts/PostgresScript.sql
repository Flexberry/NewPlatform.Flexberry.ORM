




CREATE TABLE ФайлИдеи (
 primaryKey UUID NOT NULL,
 Файл TEXT NULL,
 Владелец_m0 UUID NOT NULL,
 Идея_m0 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Apparatus2 (
 primaryKey UUID NOT NULL,
 ApparatusName VARCHAR(255) NULL,
 Exporter_m0 UUID NOT NULL,
 Maker_m0 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Котенок (
 primaryKey UUID NOT NULL,
 КличкаКотенка VARCHAR(255) NULL,
 Глупость INT NULL,
 Кошка UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Выплаты (
 primaryKey UUID NOT NULL,
 ДатаВыплаты TIMESTAMP(3) NULL,
 СуммаВыплаты DOUBLE PRECISION NULL,
 Кредит1 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Salad2 (
 primaryKey UUID NOT NULL,
 SaladName VARCHAR(255) NULL,
 Ingridient2_m0 UUID NULL,
 Ingridient2_m1 UUID NULL,
 Ingridient1_m0 UUID NULL,
 Ingridient1_m1 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Dish2 (
 primaryKey UUID NOT NULL,
 DishName VARCHAR(255) NULL,
 MainIngridient_m0 UUID NULL,
 MainIngridient_m1 UUID NULL,
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


CREATE TABLE Plant2 (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Country2 (
 primaryKey UUID NOT NULL,
 CountryName VARCHAR(255) NULL,
 XCoordinate INT NULL,
 YCoordinate INT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass (
 primaryKey UUID NOT NULL,
 PublicStringProperty VARCHAR(255) NULL,
 StringPropertyForInfTestClass VARCHAR(255) NULL,
 IntPropertyForInfTestClass INT NULL,
 BoolPropertyForInfTestClass BOOLEAN NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Soup2 (
 primaryKey UUID NOT NULL,
 SoupName VARCHAR(255) NULL,
 CabbageType UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Лес (
 primaryKey UUID NOT NULL,
 Название VARCHAR(255) NULL,
 Площадь INT NULL,
 Заповедник BOOLEAN NULL,
 ДатаПослОсмотр TIMESTAMP(3) NULL,
 Страна UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Конкурс (
 primaryKey UUID NOT NULL,
 Название VARCHAR(255) NULL,
 Описание VARCHAR(255) NULL,
 ДатаНачала TIMESTAMP(3) NULL,
 ДатаОкончания TIMESTAMP(3) NULL,
 НачалоОценки TIMESTAMP(3) NULL,
 ОкончаниеОценки TIMESTAMP(3) NULL,
 Состояние VARCHAR(16) NULL,
 Организатор_m0 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass2 (
 primaryKey UUID NOT NULL,
 StringPropertyForInfTestClass2 VARCHAR(255) NULL,
 InformationTestClass_m0 UUID NULL,
 InformationTestClass_m1 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE HierarchyClassWithIRCD (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 Parent UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE StoredClass (
 primaryKey UUID NOT NULL,
 StoredProperty VARCHAR(255) NULL,
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


CREATE TABLE AuditClassWithDisabledAudit (
 primaryKey UUID NOT NULL,
 CreateTime TIMESTAMP(3) NULL,
 Creator VARCHAR(255) NULL,
 EditTime TIMESTAMP(3) NULL,
 Editor VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ИФХозДоговора (
 primaryKey UUID NOT NULL,
 НомерИФХозДогов INT NULL,
 ИсточникФинан UUID NOT NULL,
 ХозДоговор_m0 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Блоха (
 primaryKey UUID NOT NULL,
 Кличка VARCHAR(255) NULL,
 МедведьОбитания UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE CabbagePart2 (
 primaryKey UUID NOT NULL,
 PartName VARCHAR(255) NULL,
 Cabbage UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Place2 (
 primaryKey UUID NOT NULL,
 PlaceName VARCHAR(255) NULL,
 TomorrowTeritory_m0 UUID NULL,
 TomorrowTeritory_m1 UUID NULL,
 TodayTerritory_m0 UUID NULL,
 TodayTerritory_m1 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Пользователь (
 primaryKey UUID NOT NULL,
 Логин VARCHAR(255) NULL,
 ФИО VARCHAR(255) NULL,
 EMail VARCHAR(255) NULL,
 ДатаРегистрации TIMESTAMP(3) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE AuditMasterMasterObject (
 primaryKey UUID NOT NULL,
 Login VARCHAR(255) NULL,
 Name VARCHAR(255) NULL,
 Surname VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE clb (
 primaryKey UUID NOT NULL,
 ref2 UUID NULL,
 ref1 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE НаследникМ2 (
 primaryKey UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass4 (
 primaryKey UUID NOT NULL,
 StringPropForInfTestClass4 VARCHAR(255) NULL,
 MasterOfInformationTestClass3 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE TypeUsageProviderTestClassChil (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 DataObjectForTest_m0 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Клиент (
 primaryKey UUID NOT NULL,
 ФИО VARCHAR(255) NULL,
 Прописка VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE SomeMasterClass (
 primaryKey UUID NOT NULL,
 FieldA VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE КритерийОценки (
 primaryKey UUID NOT NULL,
 ПорядковыйНомер INT NULL,
 Описание VARCHAR(255) NULL,
 Вес DOUBLE PRECISION NULL,
 Обязательный BOOLEAN NULL,
 Конкурс_m0 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Parcel (
 primaryKey UUID NOT NULL,
 Address VARCHAR(255) NULL,
 Weight DOUBLE PRECISION NULL,
 DeliveredByHomer UUID NULL,
 DeliveredByMailman UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ИнспПоКредиту (
 primaryKey UUID NOT NULL,
 ФИО VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE cla (
 primaryKey UUID NOT NULL,
 info VARCHAR(255) NULL,
 parent UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ComputedMaster (
 primaryKey UUID NOT NULL,
 MasterField1 VARCHAR(255) NULL,
 MasterField2 VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ClassToTestIRND (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 CanBeNull UUID NULL,
 CanNotBeNull UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE DataObjectForTest (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 Height INT NULL,
 BirthDate TIMESTAMP(3) NULL,
 Gender BOOLEAN NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE УчастникХозДог (
 primaryKey UUID NOT NULL,
 НомУчастнХозДог INT NULL,
 Статус VARCHAR(12) NULL,
 Личность_m0 UUID NOT NULL,
 ХозДоговор_m0 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Страна (
 primaryKey UUID NOT NULL,
 Название VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE CombinedTypesUsageProviderTest (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 DataObjectForTest_m0 UUID NULL,
 TypeUsageProviderTestClass UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE AuditAgregatorObject (
 primaryKey UUID NOT NULL,
 Login VARCHAR(255) NULL,
 Name VARCHAR(255) NULL,
 Surname VARCHAR(255) NULL,
 MasterObject UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE DetailForIRCD (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 HierarchyClassWithIRCD UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE NullFileField (
 primaryKey UUID NOT NULL,
 FileField TEXT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE CabbageSalad (
 primaryKey UUID NOT NULL,
 CabbageSaladName VARCHAR(255) NULL,
 Cabbage2 UUID NOT NULL,
 Cabbage1 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Личность (
 primaryKey UUID NOT NULL,
 ФИО VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Кредит (
 primaryKey UUID NOT NULL,
 ДатаВыдачи TIMESTAMP(3) NULL,
 СуммаКредита DOUBLE PRECISION NULL,
 СрокКредита INT NULL,
 ВидКредита VARCHAR(15) NULL,
 ИнспекторПоКред UUID NULL,
 Клиент UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ClassToTestIRCD (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 CanNotBeNull UUID NOT NULL,
 CanBeNull UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ЗначениеКритер (
 primaryKey UUID NOT NULL,
 Значение VARCHAR(255) NULL,
 СредОценкаЭксп DOUBLE PRECISION NULL,
 Критерий_m0 UUID NOT NULL,
 Идея_m0 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Cabbage2 (
 primaryKey UUID NOT NULL,
 Type VARCHAR(255) NULL,
 Name VARCHAR(255) NULL,
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


CREATE TABLE Медведь (
 primaryKey UUID NOT NULL,
 ПорядковыйНомер INT NULL,
 Вес INT NULL,
 ЦветГлаз VARCHAR(255) NULL,
 Пол VARCHAR(7) NULL,
 ДатаРождения TIMESTAMP(3) NULL,
 ЛесОбитания UUID NULL,
 Папа UUID NULL,
 Мама UUID NULL,
 Друг_m0 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE AggregatorUpdateObjectTest (
 primaryKey UUID NOT NULL,
 AggregatorName VARCHAR(255) NULL,
 Detail UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE MasterUpdateObjectTest (
 primaryKey UUID NOT NULL,
 MasterName VARCHAR(255) NULL,
 Detail UUID NULL,
 AggregatorUpdateObjectTest UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE SomeDetailClass (
 primaryKey UUID NOT NULL,
 FieldB VARCHAR(255) NULL,
 ClassA UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE DataObjectWithKeyGuid (
 primaryKey UUID NOT NULL,
 LinkToMaster1 UUID NULL,
 LinkToMaster2 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ForKeyStorageTest (
 StorageForKey VARCHAR(255) NOT NULL,
 PRIMARY KEY (StorageForKey));


CREATE TABLE DetailClass (
 primaryKey UUID NOT NULL,
 Detailproperty VARCHAR(255) NULL,
 MasterClass_m0 UUID NULL,
 MasterClass_m1 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE TestClassA (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 Value INT NULL,
 Мастер_m0 UUID NULL,
 Мастер_m1 UUID NULL,
 Мастер_m2 UUID NULL,
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


CREATE TABLE ClassWithCaptions (
 primaryKey UUID NOT NULL,
 InformationTestClass4 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ТипЛапы (
 primaryKey UUID NOT NULL,
 Название VARCHAR(255) NULL,
 Актуально BOOLEAN NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE TypeNameUsageProviderTestClass (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Region (
 primaryKey UUID NOT NULL,
 RegionName VARCHAR(255) NULL,
 Country2_m0 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass6 (
 primaryKey UUID NOT NULL,
 StringPropForInfTestClass6 VARCHAR(255) NULL,
 ExampleOfClassWithCaptions UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Human2 (
 primaryKey UUID NOT NULL,
 HumanName VARCHAR(255) NULL,
 TodayHome_m0 UUID NULL,
 TodayHome_m1 UUID NULL,
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


CREATE TABLE InformationTestClass3 (
 primaryKey UUID NOT NULL,
 StringPropForInfTestClass3 VARCHAR(255) NULL,
 InformationTestClass2 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE AuditMasterObject (
 primaryKey UUID NOT NULL,
 Login VARCHAR(255) NULL,
 Name VARCHAR(255) NULL,
 Surname VARCHAR(255) NULL,
 MasterObject UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE HierarchyClassWithIRND (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 Parent UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ХозДоговор (
 primaryKey UUID NOT NULL,
 НомХозДоговора INT NULL,
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


CREATE TABLE DateField (
 primaryKey UUID NOT NULL,
 Date TIMESTAMP(3) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE МастерМ (
 primaryKey UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ИсточникФинанс (
 primaryKey UUID NOT NULL,
 НомИсточникаФин INT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ОценкаЭксперта (
 primaryKey UUID NOT NULL,
 ЗначениеОценки DOUBLE PRECISION NULL,
 Комментарий VARCHAR(255) NULL,
 Эксперт_m0 UUID NOT NULL,
 ЗначениеКритер UUID NOT NULL,
 Идея_m0 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ДокККонкурсу (
 primaryKey UUID NOT NULL,
 Файл TEXT NULL,
 Конкурс_m0 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE MasterClass (
 primaryKey UUID NOT NULL,
 StringMasterProperty VARCHAR(255) NULL,
 InformationTestClass_m0 UUID NULL,
 InformationTestClass_m1 UUID NULL,
 InformationTestClass2 UUID NULL,
 InformationTestClass3_m0 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE SimpleDataObject (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 Age INT NULL,
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


CREATE TABLE DetailForIRND (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 HierarchyClassWithIRND UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Порода (
 primaryKey UUID NOT NULL,
 Название VARCHAR(255) NULL,
 Ключ UUID NULL,
 Иерархия UUID NULL,
 ТипПороды UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE TypeUsageProviderTestClass (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 DataObjectForTest_m0 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ТипПороды (
 primaryKey UUID NOT NULL,
 Название VARCHAR(255) NULL,
 ДатаРегистрации TIMESTAMP(3) NULL,
 Актуально BOOLEAN NULL,
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


CREATE TABLE Mailman (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 Photo TEXT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Homer (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE ClassWithMaster (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
 First UUID NOT NULL,
 Second UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Territory2 (
 primaryKey UUID NOT NULL,
 XCoordinate INT NULL,
 YCoordinate INT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Adress2 (
 primaryKey UUID NOT NULL,
 HomeNumber INT NULL,
 Country_m0 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE AuditClassWithSettings (
 primaryKey UUID NOT NULL,
 Name VARCHAR(255) NULL,
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


CREATE TABLE Идея (
 primaryKey UUID NOT NULL,
 Заголовок VARCHAR(255) NULL,
 Описание VARCHAR(255) NULL,
 СуммаБаллов DOUBLE PRECISION NULL,
 Автор_m0 UUID NOT NULL,
 Конкурс_m0 UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE DetailUpdateObjectTest (
 primaryKey UUID NOT NULL,
 DetailName VARCHAR(255) NULL,
 Master UUID NULL,
 AggregatorUpdateObjectTest UUID NOT NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE Берлога (
 primaryKey UUID NOT NULL,
 Наименование VARCHAR(255) NULL,
 Комфортность INT NULL,
 Заброшена BOOLEAN NULL,
 ЛесРасположения UUID NULL,
 Медведь UUID NOT NULL,
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



 ALTER TABLE ФайлИдеи ADD CONSTRAINT FKaa0510843b4ca8c3eb9c4dd11da8157ea2254200 FOREIGN KEY (Владелец_m0) REFERENCES Пользователь; 
CREATE INDEX Indexaa0510843b4ca8c3eb9c4dd11da8157ea2254200 on ФайлИдеи (Владелец_m0); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FK045c526ec04360174b920046d6feb9a520b31b68 FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index045c526ec04360174b920046d6feb9a520b31b68 on ФайлИдеи (Идея_m0); 

 ALTER TABLE Apparatus2 ADD CONSTRAINT FK48136e101964ff29a6f514497819cda14e5f6b51 FOREIGN KEY (Exporter_m0) REFERENCES Country2; 
CREATE INDEX Index48136e101964ff29a6f514497819cda14e5f6b51 on Apparatus2 (Exporter_m0); 

 ALTER TABLE Apparatus2 ADD CONSTRAINT FK35750ac51213de1be5ab184697de7c7ab073eb77 FOREIGN KEY (Maker_m0) REFERENCES Country2; 
CREATE INDEX Index35750ac51213de1be5ab184697de7c7ab073eb77 on Apparatus2 (Maker_m0); 

 ALTER TABLE Котенок ADD CONSTRAINT FK395d6a8c96bb7d466f36283a32a806c766b39cd9 FOREIGN KEY (Кошка) REFERENCES Кошка; 
CREATE INDEX Index395d6a8c96bb7d466f36283a32a806c766b39cd9 on Котенок (Кошка); 

 ALTER TABLE Выплаты ADD CONSTRAINT FK4a9733d75c7ba5bd6412de23e7eec6ad52a5395c FOREIGN KEY (Кредит1) REFERENCES Кредит; 
CREATE INDEX Index4a9733d75c7ba5bd6412de23e7eec6ad52a5395c on Выплаты (Кредит1); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKd71b48f1367d1c08efc34d325bf6a66a35ff2250 FOREIGN KEY (Ingridient2_m0) REFERENCES Cabbage2; 
CREATE INDEX Indexd71b48f1367d1c08efc34d325bf6a66a35ff2250 on Salad2 (Ingridient2_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK946af0be90eeb3f3b9ba96926aa0f2b884b668be FOREIGN KEY (Ingridient2_m1) REFERENCES Plant2; 
CREATE INDEX Index946af0be90eeb3f3b9ba96926aa0f2b884b668be on Salad2 (Ingridient2_m1); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK3da17578763f6e86591f8c53606a8f3498024c67 FOREIGN KEY (Ingridient1_m0) REFERENCES Cabbage2; 
CREATE INDEX Index3da17578763f6e86591f8c53606a8f3498024c67 on Salad2 (Ingridient1_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKdbf98f9df9acb3a6281836b09feb22ec8ec2a2f9 FOREIGN KEY (Ingridient1_m1) REFERENCES Plant2; 
CREATE INDEX Indexdbf98f9df9acb3a6281836b09feb22ec8ec2a2f9 on Salad2 (Ingridient1_m1); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK9cf8c7b96277372908778a6508cad80d0bfdd650 FOREIGN KEY (MainIngridient_m0) REFERENCES Cabbage2; 
CREATE INDEX Index9cf8c7b96277372908778a6508cad80d0bfdd650 on Dish2 (MainIngridient_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK83ce2a1cdaf2cebf77ec27fdce2a8fed23a26442 FOREIGN KEY (MainIngridient_m1) REFERENCES Plant2; 
CREATE INDEX Index83ce2a1cdaf2cebf77ec27fdce2a8fed23a26442 on Dish2 (MainIngridient_m1); 

 ALTER TABLE FullTypesDetail2 ADD CONSTRAINT FK3b3670ba30333ec02d824f8993b24047869b5ee2 FOREIGN KEY (FullTypesMainAgregator) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Index3b3670ba30333ec02d824f8993b24047869b5ee2 on FullTypesDetail2 (FullTypesMainAgregator); 

 ALTER TABLE Soup2 ADD CONSTRAINT FK1a91e9537d259709dd0db8364e9c8b72aa879d06 FOREIGN KEY (CabbageType) REFERENCES Cabbage2; 
CREATE INDEX Index1a91e9537d259709dd0db8364e9c8b72aa879d06 on Soup2 (CabbageType); 

 ALTER TABLE Лес ADD CONSTRAINT FKd3bd1222072f531605e73e66656fe58296c8bfd2 FOREIGN KEY (Страна) REFERENCES Страна; 
CREATE INDEX Indexd3bd1222072f531605e73e66656fe58296c8bfd2 on Лес (Страна); 

 ALTER TABLE Конкурс ADD CONSTRAINT FK4a189ab324a2e7fbc137f4eac37c5d1e7ef04608 FOREIGN KEY (Организатор_m0) REFERENCES Пользователь; 
CREATE INDEX Index4a189ab324a2e7fbc137f4eac37c5d1e7ef04608 on Конкурс (Организатор_m0); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FK4e2e3ebe8c46c4e1f82bcff42da2047f66a1dc98 FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Index4e2e3ebe8c46c4e1f82bcff42da2047f66a1dc98 on InformationTestClass2 (InformationTestClass_m0); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FKc3f7f56049d5920eec5220f53f50d0e39188c4ed FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Indexc3f7f56049d5920eec5220f53f50d0e39188c4ed on InformationTestClass2 (InformationTestClass_m1); 

 ALTER TABLE HierarchyClassWithIRCD ADD CONSTRAINT FKb00e3833681189fa34ddbbcbb5dd544cdb7b946a FOREIGN KEY (Parent) REFERENCES HierarchyClassWithIRCD; 
CREATE INDEX Indexb00e3833681189fa34ddbbcbb5dd544cdb7b946a on HierarchyClassWithIRCD (Parent); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FKb9670d9e779cc0cf7d313a9e42a490acaa703d72 FOREIGN KEY (ИсточникФинан) REFERENCES ИсточникФинанс; 
CREATE INDEX Indexb9670d9e779cc0cf7d313a9e42a490acaa703d72 on ИФХозДоговора (ИсточникФинан); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FK8c9c0e681d532cc0bd411073bd4cd93886d65b02 FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Index8c9c0e681d532cc0bd411073bd4cd93886d65b02 on ИФХозДоговора (ХозДоговор_m0); 

 ALTER TABLE Блоха ADD CONSTRAINT FKb43131b348ee335105dd990a690720791b5dcba6 FOREIGN KEY (МедведьОбитания) REFERENCES Медведь; 
CREATE INDEX Indexb43131b348ee335105dd990a690720791b5dcba6 on Блоха (МедведьОбитания); 

 ALTER TABLE CabbagePart2 ADD CONSTRAINT FKb02e91b19ac52ded91471042d82970669ed8e5ce FOREIGN KEY (Cabbage) REFERENCES Cabbage2; 
CREATE INDEX Indexb02e91b19ac52ded91471042d82970669ed8e5ce on CabbagePart2 (Cabbage); 

 ALTER TABLE Place2 ADD CONSTRAINT FK3588ff97bc9aa6a194dd602ba138fa1fad14ddb6 FOREIGN KEY (TomorrowTeritory_m0) REFERENCES Country2; 
CREATE INDEX Index3588ff97bc9aa6a194dd602ba138fa1fad14ddb6 on Place2 (TomorrowTeritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FK6cfa5841e6ad0ea2110903b06c929f4d83297df9 FOREIGN KEY (TomorrowTeritory_m1) REFERENCES Territory2; 
CREATE INDEX Index6cfa5841e6ad0ea2110903b06c929f4d83297df9 on Place2 (TomorrowTeritory_m1); 

 ALTER TABLE Place2 ADD CONSTRAINT FKb745d0926354afce7e618a4b945ee21a87a6fdff FOREIGN KEY (TodayTerritory_m0) REFERENCES Country2; 
CREATE INDEX Indexb745d0926354afce7e618a4b945ee21a87a6fdff on Place2 (TodayTerritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FKa2f38eab7c8ae80209d5cdec48687f8adb0d7838 FOREIGN KEY (TodayTerritory_m1) REFERENCES Territory2; 
CREATE INDEX Indexa2f38eab7c8ae80209d5cdec48687f8adb0d7838 on Place2 (TodayTerritory_m1); 

 ALTER TABLE clb ADD CONSTRAINT FK65282eb80fc8ac171c4f6ddab4983f4fef052be2 FOREIGN KEY (ref2) REFERENCES cla; 
CREATE INDEX Index65282eb80fc8ac171c4f6ddab4983f4fef052be2 on clb (ref2); 

 ALTER TABLE clb ADD CONSTRAINT FK939febd47f70df107d01e36e5978953d46347859 FOREIGN KEY (ref1) REFERENCES cla; 
CREATE INDEX Index939febd47f70df107d01e36e5978953d46347859 on clb (ref1); 

 ALTER TABLE InformationTestClass4 ADD CONSTRAINT FK2c53821672c1f2cc584eaacda367e0f61ac6aeb1 FOREIGN KEY (MasterOfInformationTestClass3) REFERENCES InformationTestClass3; 
CREATE INDEX Index2c53821672c1f2cc584eaacda367e0f61ac6aeb1 on InformationTestClass4 (MasterOfInformationTestClass3); 

 ALTER TABLE TypeUsageProviderTestClassChil ADD CONSTRAINT FK6d9de4e6f84f5c042b0392baeb49e47d9e745129 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Index6d9de4e6f84f5c042b0392baeb49e47d9e745129 on TypeUsageProviderTestClassChil (DataObjectForTest_m0); 

 ALTER TABLE КритерийОценки ADD CONSTRAINT FK6ba6498f985d4ffaa25dfd3423f40ddf86af9c17 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index6ba6498f985d4ffaa25dfd3423f40ddf86af9c17 on КритерийОценки (Конкурс_m0); 

 ALTER TABLE Parcel ADD CONSTRAINT FKa206bc710e01a5485b7a1ea2d3f5f23578d9ff08 FOREIGN KEY (DeliveredByHomer) REFERENCES Homer; 
CREATE INDEX Indexa206bc710e01a5485b7a1ea2d3f5f23578d9ff08 on Parcel (DeliveredByHomer); 

 ALTER TABLE Parcel ADD CONSTRAINT FK76f3f1d9639fc16f946486ab1002a6cd30adf7de FOREIGN KEY (DeliveredByMailman) REFERENCES Mailman; 
CREATE INDEX Index76f3f1d9639fc16f946486ab1002a6cd30adf7de on Parcel (DeliveredByMailman); 

 ALTER TABLE cla ADD CONSTRAINT FK28e75d67293b21fe3d940a1dc21bfe39a54a4088 FOREIGN KEY (parent) REFERENCES clb; 
CREATE INDEX Index28e75d67293b21fe3d940a1dc21bfe39a54a4088 on cla (parent); 

 ALTER TABLE ClassToTestIRND ADD CONSTRAINT FK52de1446f201034ce17318c5702baa8d78ffe9db FOREIGN KEY (CanBeNull) REFERENCES HierarchyClassWithIRND; 
CREATE INDEX Index52de1446f201034ce17318c5702baa8d78ffe9db on ClassToTestIRND (CanBeNull); 

 ALTER TABLE ClassToTestIRND ADD CONSTRAINT FK97dc2aeebbea1f97a261f2711e4d5189e0952a7c FOREIGN KEY (CanNotBeNull) REFERENCES HierarchyClassWithIRND; 
CREATE INDEX Index97dc2aeebbea1f97a261f2711e4d5189e0952a7c on ClassToTestIRND (CanNotBeNull); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FKfbd26cb4174bee5b42b9a9fe9b35d92bb0e49238 FOREIGN KEY (Личность_m0) REFERENCES Личность; 
CREATE INDEX Indexfbd26cb4174bee5b42b9a9fe9b35d92bb0e49238 on УчастникХозДог (Личность_m0); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FK64c4d7f373306fcadd288cb82db01555a9a77ec8 FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Index64c4d7f373306fcadd288cb82db01555a9a77ec8 on УчастникХозДог (ХозДоговор_m0); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FKde6e262862f9efd6b5e98e22868b2fe6f0f91a15 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Indexde6e262862f9efd6b5e98e22868b2fe6f0f91a15 on CombinedTypesUsageProviderTest (DataObjectForTest_m0); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FK81b7d818ee11d19ad9eba608575903af8880d735 FOREIGN KEY (TypeUsageProviderTestClass) REFERENCES TypeUsageProviderTestClass; 
CREATE INDEX Index81b7d818ee11d19ad9eba608575903af8880d735 on CombinedTypesUsageProviderTest (TypeUsageProviderTestClass); 

 ALTER TABLE AuditAgregatorObject ADD CONSTRAINT FKa995b2c8187d3e87fe6c1a7d846ef88fa19e24ec FOREIGN KEY (MasterObject) REFERENCES AuditMasterObject; 
CREATE INDEX Indexa995b2c8187d3e87fe6c1a7d846ef88fa19e24ec on AuditAgregatorObject (MasterObject); 

 ALTER TABLE DetailForIRCD ADD CONSTRAINT FKbe096cfa34ade245555dd368391e4a154b4257cc FOREIGN KEY (HierarchyClassWithIRCD) REFERENCES HierarchyClassWithIRCD; 
CREATE INDEX Indexbe096cfa34ade245555dd368391e4a154b4257cc on DetailForIRCD (HierarchyClassWithIRCD); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FKff90a249ca15863a3216143637cd6d4449a767b9 FOREIGN KEY (Cabbage2) REFERENCES Cabbage2; 
CREATE INDEX Indexff90a249ca15863a3216143637cd6d4449a767b9 on CabbageSalad (Cabbage2); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FK57f965a51e2867e91670995088f493561e4e6b0c FOREIGN KEY (Cabbage1) REFERENCES Cabbage2; 
CREATE INDEX Index57f965a51e2867e91670995088f493561e4e6b0c on CabbageSalad (Cabbage1); 

 ALTER TABLE Кредит ADD CONSTRAINT FK4526cd7c533134f0a0fa9054a8a1bd32f682bcca FOREIGN KEY (ИнспекторПоКред) REFERENCES ИнспПоКредиту; 
CREATE INDEX Index4526cd7c533134f0a0fa9054a8a1bd32f682bcca on Кредит (ИнспекторПоКред); 

 ALTER TABLE Кредит ADD CONSTRAINT FK55eae5c0d31ae4b663f88a9d4b5f53c8cbd5f090 FOREIGN KEY (Клиент) REFERENCES Клиент; 
CREATE INDEX Index55eae5c0d31ae4b663f88a9d4b5f53c8cbd5f090 on Кредит (Клиент); 

 ALTER TABLE ClassToTestIRCD ADD CONSTRAINT FK51aff63681ebcd2e0047eba15f59c0bbc8b9c140 FOREIGN KEY (CanNotBeNull) REFERENCES HierarchyClassWithIRCD; 
CREATE INDEX Index51aff63681ebcd2e0047eba15f59c0bbc8b9c140 on ClassToTestIRCD (CanNotBeNull); 

 ALTER TABLE ClassToTestIRCD ADD CONSTRAINT FK5e60335d7e9e080cb3023e5e63498fd8f04bcb8d FOREIGN KEY (CanBeNull) REFERENCES HierarchyClassWithIRCD; 
CREATE INDEX Index5e60335d7e9e080cb3023e5e63498fd8f04bcb8d on ClassToTestIRCD (CanBeNull); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FKc0a1cb9e30279f45b46a9644515b1ecb27d7fe2a FOREIGN KEY (Критерий_m0) REFERENCES КритерийОценки; 
CREATE INDEX Indexc0a1cb9e30279f45b46a9644515b1ecb27d7fe2a on ЗначениеКритер (Критерий_m0); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FK35e6c82b6dfdaa086957e0558be8ca0dca3c0cdc FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index35e6c82b6dfdaa086957e0558be8ca0dca3c0cdc on ЗначениеКритер (Идея_m0); 

 ALTER TABLE FullTypesMainAgregator ADD CONSTRAINT FKe73d2bf2a09f105942d78f9c1254df90ffd76a45 FOREIGN KEY (FullTypesMaster1_m0) REFERENCES FullTypesMaster1; 
CREATE INDEX Indexe73d2bf2a09f105942d78f9c1254df90ffd76a45 on FullTypesMainAgregator (FullTypesMaster1_m0); 

 ALTER TABLE Медведь ADD CONSTRAINT FK93be01a32cae64dc4b18705ade6683f41a32c367 FOREIGN KEY (ЛесОбитания) REFERENCES Лес; 
CREATE INDEX Index93be01a32cae64dc4b18705ade6683f41a32c367 on Медведь (ЛесОбитания); 

 ALTER TABLE Медведь ADD CONSTRAINT FK0ca403a899ac5a709a19bbb9ada47b0060e5b819 FOREIGN KEY (Папа) REFERENCES Медведь; 
CREATE INDEX Index0ca403a899ac5a709a19bbb9ada47b0060e5b819 on Медведь (Папа); 

 ALTER TABLE Медведь ADD CONSTRAINT FK0b9f6ad0caded1971696ef6602e8a2831fa941b1 FOREIGN KEY (Мама) REFERENCES Медведь; 
CREATE INDEX Index0b9f6ad0caded1971696ef6602e8a2831fa941b1 on Медведь (Мама); 

 ALTER TABLE Медведь ADD CONSTRAINT FKb5b93133978ff74ce234de641e9c512ddae023f2 FOREIGN KEY (Друг_m0) REFERENCES Медведь; 
CREATE INDEX Indexb5b93133978ff74ce234de641e9c512ddae023f2 on Медведь (Друг_m0); 

 ALTER TABLE AggregatorUpdateObjectTest ADD CONSTRAINT FK56d63dd74c42dee4c3809b8b894732112e1fbcf3 FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Index56d63dd74c42dee4c3809b8b894732112e1fbcf3 on AggregatorUpdateObjectTest (Detail); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FK79863c7df1af266525ff7b96ee1e63a01a7b9fe8 FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Index79863c7df1af266525ff7b96ee1e63a01a7b9fe8 on MasterUpdateObjectTest (Detail); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FK7695e86f46c8d012754f241e87233b3eb7bf9bcc FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Index7695e86f46c8d012754f241e87233b3eb7bf9bcc on MasterUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE SomeDetailClass ADD CONSTRAINT FK52690599bb911f792389788796c47b8227a955aa FOREIGN KEY (ClassA) REFERENCES SomeMasterClass; 
CREATE INDEX Index52690599bb911f792389788796c47b8227a955aa on SomeDetailClass (ClassA); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK3b66f4eb9b4585e86a13855a4a8c4658cb7ac8fd FOREIGN KEY (MasterClass_m0) REFERENCES MasterClass; 
CREATE INDEX Index3b66f4eb9b4585e86a13855a4a8c4658cb7ac8fd on DetailClass (MasterClass_m0); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK8f42b7ac29a50600ae8e7c07e98ba486d4f08b13 FOREIGN KEY (MasterClass_m1) REFERENCES MasterClass; 
CREATE INDEX Index8f42b7ac29a50600ae8e7c07e98ba486d4f08b13 on DetailClass (MasterClass_m1); 

 ALTER TABLE TestClassA ADD CONSTRAINT FKf29e5695cae4d500c66b6f50f5b3aca66e5a13c9 FOREIGN KEY (Мастер_m0) REFERENCES МастерМ; 
CREATE INDEX Indexf29e5695cae4d500c66b6f50f5b3aca66e5a13c9 on TestClassA (Мастер_m0); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK837ca60e4c4b0d4b8a3494326fcb0df72bdeccc6 FOREIGN KEY (Мастер_m1) REFERENCES НаследникМ1; 
CREATE INDEX Index837ca60e4c4b0d4b8a3494326fcb0df72bdeccc6 on TestClassA (Мастер_m1); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK2cece259bbf05997f093c9c9273e9afa1f0360b0 FOREIGN KEY (Мастер_m2) REFERENCES НаследникМ2; 
CREATE INDEX Index2cece259bbf05997f093c9c9273e9afa1f0360b0 on TestClassA (Мастер_m2); 

 ALTER TABLE FullTypesDetail1 ADD CONSTRAINT FK960c1233e52fdb4f21902e7cc229288cd2db0302 FOREIGN KEY (FullTypesMainAgregator_m0) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Index960c1233e52fdb4f21902e7cc229288cd2db0302 on FullTypesDetail1 (FullTypesMainAgregator_m0); 

 ALTER TABLE ClassWithCaptions ADD CONSTRAINT FK81ef5c62d6b79d7f5a5c3184bc86d43a356b9b72 FOREIGN KEY (InformationTestClass4) REFERENCES InformationTestClass4; 
CREATE INDEX Index81ef5c62d6b79d7f5a5c3184bc86d43a356b9b72 on ClassWithCaptions (InformationTestClass4); 

 ALTER TABLE Region ADD CONSTRAINT FKd936eda3e8ce36555b5d9cab6e27e5b275946799 FOREIGN KEY (Country2_m0) REFERENCES Country2; 
CREATE INDEX Indexd936eda3e8ce36555b5d9cab6e27e5b275946799 on Region (Country2_m0); 

 ALTER TABLE InformationTestClass6 ADD CONSTRAINT FK2163de91dc2558edb3686163b1766289b15297d9 FOREIGN KEY (ExampleOfClassWithCaptions) REFERENCES ClassWithCaptions; 
CREATE INDEX Index2163de91dc2558edb3686163b1766289b15297d9 on InformationTestClass6 (ExampleOfClassWithCaptions); 

 ALTER TABLE Human2 ADD CONSTRAINT FK0ce238a8f0af8d2c85032ebc4303082beee8ee83 FOREIGN KEY (TodayHome_m0) REFERENCES Country2; 
CREATE INDEX Index0ce238a8f0af8d2c85032ebc4303082beee8ee83 on Human2 (TodayHome_m0); 

 ALTER TABLE Human2 ADD CONSTRAINT FKc72802751389cbb63d70a7862dbe4c0c3590feed FOREIGN KEY (TodayHome_m1) REFERENCES Territory2; 
CREATE INDEX Indexc72802751389cbb63d70a7862dbe4c0c3590feed on Human2 (TodayHome_m1); 

 ALTER TABLE Лапа ADD CONSTRAINT FK801cdef07db8852f60bd68a5a1fc42341cd641fa FOREIGN KEY (ТипЛапы_m0) REFERENCES ТипЛапы; 
CREATE INDEX Index801cdef07db8852f60bd68a5a1fc42341cd641fa on Лапа (ТипЛапы_m0); 

 ALTER TABLE Лапа ADD CONSTRAINT FKd2c2995f4deb3767b25fa4ca17d61bf9bff3d562 FOREIGN KEY (Кошка_m0) REFERENCES Кошка; 
CREATE INDEX Indexd2c2995f4deb3767b25fa4ca17d61bf9bff3d562 on Лапа (Кошка_m0); 

 ALTER TABLE InformationTestClass3 ADD CONSTRAINT FKa52b893b549bee4813cd1c14b010f3a079bbd2e1 FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Indexa52b893b549bee4813cd1c14b010f3a079bbd2e1 on InformationTestClass3 (InformationTestClass2); 

 ALTER TABLE AuditMasterObject ADD CONSTRAINT FKe3f9a4bea6d459a2954deb3fcf817ebec12ab30e FOREIGN KEY (MasterObject) REFERENCES AuditMasterMasterObject; 
CREATE INDEX Indexe3f9a4bea6d459a2954deb3fcf817ebec12ab30e on AuditMasterObject (MasterObject); 

 ALTER TABLE HierarchyClassWithIRND ADD CONSTRAINT FKf0535f00c9a2384b0a707e754a0668661366f1d4 FOREIGN KEY (Parent) REFERENCES HierarchyClassWithIRND; 
CREATE INDEX Indexf0535f00c9a2384b0a707e754a0668661366f1d4 on HierarchyClassWithIRND (Parent); 

 ALTER TABLE Кошка ADD CONSTRAINT FK622a9f641a5308f2d104e0139d3cfb877b83e7f1 FOREIGN KEY (Порода) REFERENCES Порода; 
CREATE INDEX Index622a9f641a5308f2d104e0139d3cfb877b83e7f1 on Кошка (Порода); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FKe7dc8033d2bc2f27017151b89905a95c985005d8 FOREIGN KEY (Эксперт_m0) REFERENCES Пользователь; 
CREATE INDEX Indexe7dc8033d2bc2f27017151b89905a95c985005d8 on ОценкаЭксперта (Эксперт_m0); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK9de9103fd2eda179954d0971f8ba860d1e344207 FOREIGN KEY (ЗначениеКритер) REFERENCES ЗначениеКритер; 
CREATE INDEX Index9de9103fd2eda179954d0971f8ba860d1e344207 on ОценкаЭксперта (ЗначениеКритер); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK8c5b7bd87c655ac406b1b34087216af840912cea FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index8c5b7bd87c655ac406b1b34087216af840912cea on ОценкаЭксперта (Идея_m0); 

 ALTER TABLE ДокККонкурсу ADD CONSTRAINT FKc71c6a5439f7ae7d47606e0534f0e462f414f22c FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Indexc71c6a5439f7ae7d47606e0534f0e462f414f22c on ДокККонкурсу (Конкурс_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FKb207dd773e093b192d9ec87dfaec1471ce07a3f8 FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Indexb207dd773e093b192d9ec87dfaec1471ce07a3f8 on MasterClass (InformationTestClass_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FKa83ab52b4ff2bfe25ce889ea09500664dc591c37 FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Indexa83ab52b4ff2bfe25ce889ea09500664dc591c37 on MasterClass (InformationTestClass_m1); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK23e6698f08fbc8406ea5692a723cc2123467a9a0 FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Index23e6698f08fbc8406ea5692a723cc2123467a9a0 on MasterClass (InformationTestClass2); 

 ALTER TABLE MasterClass ADD CONSTRAINT FKea1c30326407ae0c0c754bd0a2b816ea12239a72 FOREIGN KEY (InformationTestClass3_m0) REFERENCES InformationTestClass3; 
CREATE INDEX Indexea1c30326407ae0c0c754bd0a2b816ea12239a72 on MasterClass (InformationTestClass3_m0); 

 ALTER TABLE Перелом ADD CONSTRAINT FK6dee404d2bb9702d8d72537c5ae42a7c97dfb5fa FOREIGN KEY (Лапа_m0) REFERENCES Лапа; 
CREATE INDEX Index6dee404d2bb9702d8d72537c5ae42a7c97dfb5fa on Перелом (Лапа_m0); 

 ALTER TABLE DetailForIRND ADD CONSTRAINT FK55a742a58df9209f59067df58b12c1ab1785174b FOREIGN KEY (HierarchyClassWithIRND) REFERENCES HierarchyClassWithIRND; 
CREATE INDEX Index55a742a58df9209f59067df58b12c1ab1785174b on DetailForIRND (HierarchyClassWithIRND); 

 ALTER TABLE Порода ADD CONSTRAINT FKebd959b46f07cafcc01d6ec3e2830022d7e5a758 FOREIGN KEY (Иерархия) REFERENCES Порода; 
CREATE INDEX Indexebd959b46f07cafcc01d6ec3e2830022d7e5a758 on Порода (Иерархия); 

 ALTER TABLE Порода ADD CONSTRAINT FKa49e836c5356ea3e7bf40891601f94f9e901f626 FOREIGN KEY (ТипПороды) REFERENCES ТипПороды; 
CREATE INDEX Indexa49e836c5356ea3e7bf40891601f94f9e901f626 on Порода (ТипПороды); 

 ALTER TABLE TypeUsageProviderTestClass ADD CONSTRAINT FK43a10ebc1b4b5fc5f0761a2b068aa1f9dbdc2298 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Index43a10ebc1b4b5fc5f0761a2b068aa1f9dbdc2298 on TypeUsageProviderTestClass (DataObjectForTest_m0); 

 ALTER TABLE ComputedDetail ADD CONSTRAINT FK3514ed0e8b3649679c9f9e2d3480fd506ed34d99 FOREIGN KEY (ComputedMaster) REFERENCES ComputedMaster; 
CREATE INDEX Index3514ed0e8b3649679c9f9e2d3480fd506ed34d99 on ComputedDetail (ComputedMaster); 

 ALTER TABLE ClassWithMaster ADD CONSTRAINT FKf5c3a7c63365601454b969d66a94ae74d39f6899 FOREIGN KEY (First) REFERENCES ClassToTestIRND; 
CREATE INDEX Indexf5c3a7c63365601454b969d66a94ae74d39f6899 on ClassWithMaster (First); 

 ALTER TABLE ClassWithMaster ADD CONSTRAINT FKc585902f8ffba04f4c86e6de5bfcdf11dd2c48e6 FOREIGN KEY (Second) REFERENCES ClassToTestIRCD; 
CREATE INDEX Indexc585902f8ffba04f4c86e6de5bfcdf11dd2c48e6 on ClassWithMaster (Second); 

 ALTER TABLE Adress2 ADD CONSTRAINT FK02576e7567345d16728ba37f23f2daaf93394730 FOREIGN KEY (Country_m0) REFERENCES Country2; 
CREATE INDEX Index02576e7567345d16728ba37f23f2daaf93394730 on Adress2 (Country_m0); 

 ALTER TABLE Идея ADD CONSTRAINT FKcd2b2659b0769c3011c4bebc1ae441dec84e11ec FOREIGN KEY (Автор_m0) REFERENCES Пользователь; 
CREATE INDEX Indexcd2b2659b0769c3011c4bebc1ae441dec84e11ec on Идея (Автор_m0); 

 ALTER TABLE Идея ADD CONSTRAINT FK1f0afae273e89503114f99931573e0ec1f2de5ef FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index1f0afae273e89503114f99931573e0ec1f2de5ef on Идея (Конкурс_m0); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FKe7fe3ef70dac31b888e5765248e08e95682e218f FOREIGN KEY (Master) REFERENCES MasterUpdateObjectTest; 
CREATE INDEX Indexe7fe3ef70dac31b888e5765248e08e95682e218f on DetailUpdateObjectTest (Master); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FK4dbe09c60e1023ebd2e5d8ba52138aeef8cb3110 FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Index4dbe09c60e1023ebd2e5d8ba52138aeef8cb3110 on DetailUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE Берлога ADD CONSTRAINT FKa74603e81cb82d318a92d5d3e374895fe242d80e FOREIGN KEY (ЛесРасположения) REFERENCES Лес; 
CREATE INDEX Indexa74603e81cb82d318a92d5d3e374895fe242d80e on Берлога (ЛесРасположения); 

 ALTER TABLE Берлога ADD CONSTRAINT FK838e30a686c4f1dcfbb02e55d47218e48ddbe7a2 FOREIGN KEY (Медведь) REFERENCES Медведь; 
CREATE INDEX Index838e30a686c4f1dcfbb02e55d47218e48ddbe7a2 on Берлога (Медведь); 

 ALTER TABLE STORMWEBSEARCH ADD CONSTRAINT FKc4378e39870eb056aec84088683297a01d2a6200 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERDETAIL ADD CONSTRAINT FK921d16269835017e2a0d0e29ad6fb175454a70d0 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERLOOKUP ADD CONSTRAINT FKce38ef0db3f01a53acaa49fed8853fb941ad47ba FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 
CREATE INDEX Indexaa1a3fec50499d204c389473163d0d8f55d4aed9 on STORMAuEntity (ObjectPrimaryKey); 
CREATE INDEX Indexa06334f170abdcbe9ebbf9a1c97a105e31caac8d on STORMAuEntity (upper(ObjectPrimaryKey)); 
CREATE INDEX Index969964c4b120bd7eebed319d77e182a5adf20816 on STORMAuEntity (OperationTime); 
CREATE INDEX Index44feded66c1cee358e0db313bcaa06e5f8d8e549 on STORMAuEntity (User_m0); 

 ALTER TABLE STORMAuEntity ADD CONSTRAINT FKb5725f55e665c6b660aff02c558b5ba413523eaa FOREIGN KEY (ObjectType_m0) REFERENCES STORMAuObjType; 
CREATE INDEX Indexb5725f55e665c6b660aff02c558b5ba413523eaa on STORMAuEntity (ObjectType_m0); 

 ALTER TABLE STORMAuField ADD CONSTRAINT FKf2cc121c707b1bf4290f2bb625d1d112b4919288 FOREIGN KEY (MainChange_m0) REFERENCES STORMAuField; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FK680dbb7e20a2404a7439d4de2d06d669f165bafe FOREIGN KEY (AuditEntity_m0) REFERENCES STORMAuEntity; 
CREATE INDEX Index680dbb7e20a2404a7439d4de2d06d669f165bafe on STORMAuField (AuditEntity_m0); 

