



CREATE TABLE InformationTestClass (

 primaryKey UUID NOT NULL,

 PublicStringProperty VARCHAR(255) NULL,

 StringPropertyForInfTestClass VARCHAR(255) NULL,

 IntPropertyForInfTestClass INT NULL,

 BoolPropertyForInfTestClass BOOLEAN NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ТипЛапы (

 primaryKey UUID NOT NULL,

 Название VARCHAR(255) NULL,

 Актуально BOOLEAN NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Идея (

 primaryKey UUID NOT NULL,

 Заголовок VARCHAR(255) NULL,

 Описание VARCHAR(255) NULL,

 СуммаБаллов DOUBLE PRECISION NULL,

 Конкурс_m0 UUID NOT NULL,

 Автор_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE DateField (

 primaryKey UUID NOT NULL,

 Date TIMESTAMP(3) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AuditMasterObject (

 primaryKey UUID NOT NULL,

 Login VARCHAR(255) NULL,

 Name VARCHAR(255) NULL,

 Surname VARCHAR(255) NULL,

 MasterObject UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AuditClassWithDisabledAudit (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 CreateTime TIMESTAMP(3) NULL,

 Creator VARCHAR(255) NULL,

 EditTime TIMESTAMP(3) NULL,

 Editor VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass2 (

 primaryKey UUID NOT NULL,

 StringPropertyForInfTestClass2 VARCHAR(255) NULL,

 InformationTestClass_m0 UUID NULL,

 InformationTestClass_m1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Country2 (

 primaryKey UUID NOT NULL,

 CountryName VARCHAR(255) NULL,

 XCoordinate INT NULL,

 YCoordinate INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE TestClassA (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 Value INT NULL,

 Мастер_m0 UUID NULL,

 Мастер_m1 UUID NULL,

 Мастер_m2 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE clb (

 primaryKey UUID NOT NULL,

 ref2 UUID NULL,

 ref1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AggregatorUpdateObjectTest (

 primaryKey UUID NOT NULL,

 AggregatorName VARCHAR(255) NULL,

 Detail UUID NULL,

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


CREATE TABLE ИФХозДоговора (

 primaryKey UUID NOT NULL,

 НомерИФХозДогов INT NULL,

 ИсточникФинан UUID NOT NULL,

 ХозДоговор_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Котенок (

 primaryKey UUID NOT NULL,

 КличкаКотенка VARCHAR(255) NULL,

 Глупость INT NULL,

 Кошка UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ЗначениеКритер (

 primaryKey UUID NOT NULL,

 Значение VARCHAR(255) NULL,

 СредОценкаЭксп DOUBLE PRECISION NULL,

 Критерий_m0 UUID NOT NULL,

 Идея_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ДокККонкурсу (

 primaryKey UUID NOT NULL,

 Файл TEXT NULL,

 Конкурс_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Личность (

 primaryKey UUID NOT NULL,

 ФИО VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Лес (

 primaryKey UUID NOT NULL,

 Название VARCHAR(255) NULL,

 Площадь INT NULL,

 Заповедник BOOLEAN NULL,

 ДатаПослОсмотр TIMESTAMP(3) NULL,

 Страна UUID NULL,

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


CREATE TABLE DetailUpdateObjectTest (

 primaryKey UUID NOT NULL,

 DetailName VARCHAR(255) NULL,

 Master UUID NULL,

 AggregatorUpdateObjectTest UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE SomeMasterClass (

 primaryKey UUID NOT NULL,

 FieldA VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Порода (

 primaryKey UUID NOT NULL,

 Название VARCHAR(255) NULL,

 Ключ UUID NULL,

 ТипПороды UUID NULL,

 Иерархия UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE МастерМ (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 Value INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Блоха (

 primaryKey UUID NOT NULL,

 Кличка VARCHAR(255) NULL,

 МедведьОбитания UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Страна (

 primaryKey UUID NOT NULL,

 Название VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE TypeNameUsageProviderTestClass (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE DetailClass (

 primaryKey UUID NOT NULL,

 Detailproperty VARCHAR(255) NULL,

 MasterClass_m0 UUID NULL,

 MasterClass_m1 UUID NULL,

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


CREATE TABLE MasterUpdateObjectTest (

 primaryKey UUID NOT NULL,

 MasterName VARCHAR(255) NULL,

 Detail UUID NULL,

 AggregatorUpdateObjectTest UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ТипПороды (

 primaryKey UUID NOT NULL,

 Название VARCHAR(255) NULL,

 ДатаРегистрации TIMESTAMP(3) NULL,

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


CREATE TABLE CombinedTypesUsageProviderTest (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 DataObjectForTest_m0 UUID NULL,

 TypeUsageProviderTestClass UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AuditClassWithoutSettings (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

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


CREATE TABLE SomeDetailClass (

 primaryKey UUID NOT NULL,

 FieldB VARCHAR(255) NULL,

 ClassA UUID NOT NULL,

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


CREATE TABLE Territory2 (

 primaryKey UUID NOT NULL,

 XCoordinate INT NULL,

 YCoordinate INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Salad2 (

 primaryKey UUID NOT NULL,

 SaladName VARCHAR(255) NULL,

 Ingridient2_m0 UUID NULL,

 Ingridient2_m1 UUID NULL,

 Ingridient1_m0 UUID NULL,

 Ingridient1_m1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ForKeyStorageTest (

 StorageForKey VARCHAR(255) NOT NULL,

 PRIMARY KEY (StorageForKey));


CREATE TABLE Выплаты (

 primaryKey UUID NOT NULL,

 ДатаВыплаты TIMESTAMP(3) NULL,

 СуммаВыплаты DOUBLE PRECISION NULL,

 Кредит1 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ИсточникФинанс (

 primaryKey UUID NOT NULL,

 НомИсточникаФин INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE УчастникХозДог (

 primaryKey UUID NOT NULL,

 НомУчастнХозДог INT NULL,

 Статус VARCHAR(12) NULL,

 Личность_m0 UUID NOT NULL,

 ХозДоговор_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Перелом (

 primaryKey UUID NOT NULL,

 Дата TIMESTAMP(3) NULL,

 Тип VARCHAR(8) NULL,

 Лапа_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Human2 (

 primaryKey UUID NOT NULL,

 HumanName VARCHAR(255) NULL,

 TodayHome_m0 UUID NULL,

 TodayHome_m1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ОценкаЭксперта (

 primaryKey UUID NOT NULL,

 ЗначениеОценки DOUBLE PRECISION NULL,

 Комментарий VARCHAR(255) NULL,

 ЗначениеКритер UUID NOT NULL,

 Эксперт_m0 UUID NOT NULL,

 Идея_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Медведь (

 primaryKey UUID NOT NULL,

 ПорядковыйНомер INT NULL,

 Вес INT NULL,

 ЦветГлаз VARCHAR(255) NULL,

 Пол VARCHAR(7) NULL,

 ДатаРождения TIMESTAMP(3) NULL,

 Мама UUID NULL,

 Папа UUID NULL,

 ЛесОбитания UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass6 (

 primaryKey UUID NOT NULL,

 StringPropForInfTestClass6 VARCHAR(255) NULL,

 ExampleOfClassWithCaptions UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Берлога (

 primaryKey UUID NOT NULL,

 Наименование VARCHAR(255) NULL,

 Комфортность INT NULL,

 Заброшена BOOLEAN NULL,

 ЛесРасположения UUID NULL,

 Медведь UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AuditMasterMasterObject (

 primaryKey UUID NOT NULL,

 Login VARCHAR(255) NULL,

 Name VARCHAR(255) NULL,

 Surname VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Клиент (

 primaryKey UUID NOT NULL,

 ФИО VARCHAR(255) NULL,

 Прописка VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Apparatus2 (

 primaryKey UUID NOT NULL,

 ApparatusName VARCHAR(255) NULL,

 Maker_m0 UUID NULL,

 Exporter_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE НаследникМ2 (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 Value INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Пользователь (

 primaryKey UUID NOT NULL,

 Логин VARCHAR(255) NULL,

 ФИО VARCHAR(255) NULL,

 EMail VARCHAR(255) NULL,

 ДатаРегистрации TIMESTAMP(3) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE НаследникМ1 (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 Value INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AuditClassWithSettings (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 CreateTime TIMESTAMP(3) NULL,

 Creator VARCHAR(255) NULL,

 EditTime TIMESTAMP(3) NULL,

 Editor VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Dish2 (

 primaryKey UUID NOT NULL,

 DishName VARCHAR(255) NULL,

 MainIngridient_m0 UUID NULL,

 MainIngridient_m1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Cabbage2 (

 primaryKey UUID NOT NULL,

 Type VARCHAR(255) NULL,

 Name VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE DataObjectForTest (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 Height INT NULL,

 BirthDate TIMESTAMP(3) NULL,

 Gender BOOLEAN NULL,

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


CREATE TABLE Plant2 (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE КритерийОценки (

 primaryKey UUID NOT NULL,

 ПорядковыйНомер INT NULL,

 Описание VARCHAR(255) NULL,

 Вес DOUBLE PRECISION NULL,

 Обязательный BOOLEAN NULL,

 Конкурс_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass4 (

 primaryKey UUID NOT NULL,

 StringPropForInfTestClass4 VARCHAR(255) NULL,

 MasterOfInformationTestClass3 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClassChild (

 primaryKey UUID NOT NULL,

 PublicStringProperty VARCHAR(255) NULL,

 StringPropertyForInfTestClass VARCHAR(255) NULL,

 IntPropertyForInfTestClass INT NULL,

 BoolPropertyForInfTestClass BOOLEAN NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ClassWithCaptions (

 primaryKey UUID NOT NULL,

 InformationTestClass4 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Adress2 (

 primaryKey UUID NOT NULL,

 HomeNumber INT NULL,

 Country_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ИнспПоКредиту (

 primaryKey UUID NOT NULL,

 ФИО VARCHAR(255) NULL,

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


CREATE TABLE TypeUsageProviderTestClassChil (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 DataObjectForTest_m0 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE CabbageSalad (

 primaryKey UUID NOT NULL,

 CabbageSaladName VARCHAR(255) NULL,

 Cabbage1 UUID NULL,

 Cabbage2 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE NullFileField (

 primaryKey UUID NOT NULL,

 FileField TEXT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE CabbagePart2 (

 primaryKey UUID NOT NULL,

 PartName VARCHAR(255) NULL,

 Cabbage UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE cla (

 primaryKey UUID NOT NULL,

 info VARCHAR(255) NULL,

 parent UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Place2 (

 primaryKey UUID NOT NULL,

 PlaceName VARCHAR(255) NULL,

 TodayTerritory_m0 UUID NULL,

 TodayTerritory_m1 UUID NULL,

 TomorrowTeritory_m0 UUID NULL,

 TomorrowTeritory_m1 UUID NULL,

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


CREATE TABLE DataObjectWithKeyGuid (

 primaryKey UUID NOT NULL,

 LinkToMaster1 UUID NULL,

 LinkToMaster2 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE StoredClass (

 primaryKey UUID NOT NULL,

 StoredProperty VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE TypeUsageProviderTestClass (

 primaryKey UUID NOT NULL,

 Name VARCHAR(255) NULL,

 DataObjectForTest_m0 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE AuditAgregatorObject (

 primaryKey UUID NOT NULL,

 Login VARCHAR(255) NULL,

 Name VARCHAR(255) NULL,

 Surname VARCHAR(255) NULL,

 MasterObject UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Region (

 primaryKey UUID NOT NULL,

 RegionName VARCHAR(255) NULL,

 Country2_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ФайлИдеи (

 primaryKey UUID NOT NULL,

 Файл TEXT NULL,

 Владелец_m0 UUID NOT NULL,

 Идея_m0 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE InformationTestClass3 (

 primaryKey UUID NOT NULL,

 StringPropForInfTestClass3 VARCHAR(255) NULL,

 InformationTestClass2 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Soup2 (

 primaryKey UUID NOT NULL,

 SoupName VARCHAR(255) NULL,

 CabbageType UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE MasterClass (

 primaryKey UUID NOT NULL,

 StringMasterProperty VARCHAR(255) NULL,

 InformationTestClass2 UUID NULL,

 InformationTestClass3_m0 UUID NULL,

 InformationTestClass_m0 UUID NULL,

 InformationTestClass_m1 UUID NULL,

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




 ALTER TABLE Идея ADD CONSTRAINT FKe6cffefaff994a689944703b5b32be4f FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Indexcd5b48e689484e6ab4343ebdb4c85e3f on Идея (Конкурс_m0); 

 ALTER TABLE Идея ADD CONSTRAINT FK969876bf072445edbab4ed9235616f87 FOREIGN KEY (Автор_m0) REFERENCES Пользователь; 
CREATE INDEX Index1785fd185bdf4c2085ea5f64e2944f29 on Идея (Автор_m0); 

 ALTER TABLE AuditMasterObject ADD CONSTRAINT FKcd3bb9934bef453c8d9c92a579b4d5a2 FOREIGN KEY (MasterObject) REFERENCES AuditMasterMasterObject; 
CREATE INDEX Indexad56f964d45947e6aebeb87fbd3f0ab2 on AuditMasterObject (MasterObject); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FK5d5b439626b444548a7bd4c9cf669ccf FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Index00f5939ba34d4c3f957ae3b7aeb6f36d on InformationTestClass2 (InformationTestClass_m0); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FK4fb2e455ac0843a29e455a4ae4b826d4 FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Index1f4ed47bf4c84b24846db313edcbf22f on InformationTestClass2 (InformationTestClass_m1); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK47b0f02dc6eb43e2a58da0297d37e903 FOREIGN KEY (Мастер_m0) REFERENCES МастерМ; 
CREATE INDEX Index1a000d7a15174620a9d3f464b9bac7fa on TestClassA (Мастер_m0); 

 ALTER TABLE TestClassA ADD CONSTRAINT FKd3134dfeca2849fb911e76f476fd067b FOREIGN KEY (Мастер_m1) REFERENCES НаследникМ1; 
CREATE INDEX Index1c583e2c075e42328b6c85d177cbc548 on TestClassA (Мастер_m1); 

 ALTER TABLE TestClassA ADD CONSTRAINT FKcef546deae5e458dab4f6236bc612239 FOREIGN KEY (Мастер_m2) REFERENCES НаследникМ2; 
CREATE INDEX Index6f7ede51526744daa4752c67d19bb6e3 on TestClassA (Мастер_m2); 

 ALTER TABLE clb ADD CONSTRAINT FKcfadfbca77f649a48914e28e2cde9a26 FOREIGN KEY (ref2) REFERENCES cla; 
CREATE INDEX Indexe9c57715556f4924b49c933048e1c4e4 on clb (ref2); 

 ALTER TABLE clb ADD CONSTRAINT FK75f3be6df184437c985d5b4a57f9fc0a FOREIGN KEY (ref1) REFERENCES cla; 
CREATE INDEX Index2394207fe53a413f8de0760651ed147c on clb (ref1); 

 ALTER TABLE AggregatorUpdateObjectTest ADD CONSTRAINT FK136e069e412d4990bb5df74a545f383e FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Index62df6197c9e64e2d831884d88d78944e on AggregatorUpdateObjectTest (Detail); 

 ALTER TABLE Лапа ADD CONSTRAINT FK439df1a6660148378984b70987106a56 FOREIGN KEY (ТипЛапы_m0) REFERENCES ТипЛапы; 
CREATE INDEX Index8c650c8426e943e692f35b23ec29b3d0 on Лапа (ТипЛапы_m0); 

 ALTER TABLE Лапа ADD CONSTRAINT FKe63b196a3b6f45748e0d12102daa3453 FOREIGN KEY (Кошка_m0) REFERENCES Кошка; 
CREATE INDEX Index257d99b4ad9f4f8eae955c3f35ace8e8 on Лапа (Кошка_m0); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FK77af69eda7834137b35d24da3b5ddec0 FOREIGN KEY (ИсточникФинан) REFERENCES ИсточникФинанс; 
CREATE INDEX Index4fa7b3e0b1324eed8b1464beaf65e618 on ИФХозДоговора (ИсточникФинан); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FKa8fd67a270d24e6d81acd106788cc3fe FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Index757287bf73cb4773b0507735b305ac04 on ИФХозДоговора (ХозДоговор_m0); 

 ALTER TABLE Котенок ADD CONSTRAINT FK4c330cf8c3b14d8ea32ddfc2912bbdc3 FOREIGN KEY (Кошка) REFERENCES Кошка; 
CREATE INDEX Indexb755e85a163d44ad8c0b879b1dd60481 on Котенок (Кошка); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FK389c4f044fae44cfbead7e81eb1c78a9 FOREIGN KEY (Критерий_m0) REFERENCES КритерийОценки; 
CREATE INDEX Index757139739f0147c1a4b9d775d9a49e5b on ЗначениеКритер (Критерий_m0); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FK5563d75cbd664d75898376be3c41b2ad FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Indexc18f61720dd64beca3575e18371e481d on ЗначениеКритер (Идея_m0); 

 ALTER TABLE ДокККонкурсу ADD CONSTRAINT FK540b554abf1b4ef2826d0928716d1d7b FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index838cc887f2d94f23b2ef758b279af867 on ДокККонкурсу (Конкурс_m0); 

 ALTER TABLE Лес ADD CONSTRAINT FK715c6395e878478e93c795bf6ac361eb FOREIGN KEY (Страна) REFERENCES Страна; 
CREATE INDEX Index87ca5563f40e4dce8e6a3ea82c1ff899 on Лес (Страна); 

 ALTER TABLE FullTypesDetail1 ADD CONSTRAINT FK050fccb8158746e9a43617885907feb6 FOREIGN KEY (FullTypesMainAgregator_m0) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Index833f9e2757d24c11845d9370a9159823 on FullTypesDetail1 (FullTypesMainAgregator_m0); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FK1df38dce41c34a4099f01efe3cd69a21 FOREIGN KEY (Master) REFERENCES MasterUpdateObjectTest; 
CREATE INDEX Index609761f455eb4e64a3e4a8ef1383c3c4 on DetailUpdateObjectTest (Master); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FKebc6dc5ff102475f889b2972e2dbee96 FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Index3978d0038e3f452599b9950ef7727bce on DetailUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE Порода ADD CONSTRAINT FKd31208a3cee44c969d470c139e6e7dd0 FOREIGN KEY (ТипПороды) REFERENCES ТипПороды; 
CREATE INDEX Indexef12944f1898430bacd77de42d2febcd on Порода (ТипПороды); 

 ALTER TABLE Порода ADD CONSTRAINT FKec9a788676ae40a6bafba20de6c9bf93 FOREIGN KEY (Иерархия) REFERENCES Порода; 
CREATE INDEX Index08782f40812f41dcb59f0caf0f94035c on Порода (Иерархия); 

 ALTER TABLE Блоха ADD CONSTRAINT FKed48129fa4bd4ec08d23efaef322d71f FOREIGN KEY (МедведьОбитания) REFERENCES Медведь; 
CREATE INDEX Index137022c981474fac99103a0535c5ba50 on Блоха (МедведьОбитания); 

 ALTER TABLE DetailClass ADD CONSTRAINT FKff1ea92f894840669d5d2b08b39488f3 FOREIGN KEY (MasterClass_m0) REFERENCES MasterClass; 
CREATE INDEX Indexf0298787f1624ec1adb4a063d46b3eb0 on DetailClass (MasterClass_m0); 

 ALTER TABLE DetailClass ADD CONSTRAINT FKd932daa3a0f04fa8a2b375fb83d7dcb3 FOREIGN KEY (MasterClass_m1) REFERENCES MasterClass; 
CREATE INDEX Indexa6036688196f488fa5888836512d2a04 on DetailClass (MasterClass_m1); 

 ALTER TABLE Кредит ADD CONSTRAINT FK9d2bd8b2a99747aeaeb506936500e187 FOREIGN KEY (Клиент) REFERENCES Клиент; 
CREATE INDEX Index0b4e7310ab3c49a99e2f54dc5086c59b on Кредит (Клиент); 

 ALTER TABLE Кредит ADD CONSTRAINT FK4deb281eff364a22a0a683b6d24c8d4e FOREIGN KEY (ИнспекторПоКред) REFERENCES ИнспПоКредиту; 
CREATE INDEX Index88cbd3d4e8264b0d883edb62ec106faf on Кредит (ИнспекторПоКред); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FKa079e456d4104f55981b21064e656008 FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Index1e3b90b0f4fc4a2da2b16355381f3258 on MasterUpdateObjectTest (Detail); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FKfbf7180861664161ac6d361b111d2746 FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Index718c641085d54e0bb39e028a2774552d on MasterUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE FullTypesMainAgregator ADD CONSTRAINT FKa7ca8596f889411da339446e86c0d4fd FOREIGN KEY (FullTypesMaster1_m0) REFERENCES FullTypesMaster1; 
CREATE INDEX Index7e5d4ebb52fc4220b581452d08e88e31 on FullTypesMainAgregator (FullTypesMaster1_m0); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FK3877e6278eea4a38b2ee1e2ac23e5124 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Indexd69cefb112d0433688c010a76654feee on CombinedTypesUsageProviderTest (DataObjectForTest_m0); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FKd87fc5f2aa29474786c9ae1b9f5f7ff6 FOREIGN KEY (TypeUsageProviderTestClass) REFERENCES TypeUsageProviderTestClass; 
CREATE INDEX Indexf800e39597c444e3be6a82d9c7623f72 on CombinedTypesUsageProviderTest (TypeUsageProviderTestClass); 

 ALTER TABLE Кошка ADD CONSTRAINT FK91fd8fc9e4a349ae8e1b2e558647f2d4 FOREIGN KEY (Порода) REFERENCES Порода; 
CREATE INDEX Index398fbea74cb042b5a81a0f88f43cbaee on Кошка (Порода); 

 ALTER TABLE SomeDetailClass ADD CONSTRAINT FK4416b7a0662e40a085301b5422efe9e9 FOREIGN KEY (ClassA) REFERENCES SomeMasterClass; 
CREATE INDEX Index0894789df3074d15a0cc4f4c78691069 on SomeDetailClass (ClassA); 

 ALTER TABLE FullTypesDetail2 ADD CONSTRAINT FKe6acf061e0774b7fa7ad037b2d47b21d FOREIGN KEY (FullTypesMainAgregator) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Indexaa723fdb70e04055a1592a4ca50dba23 on FullTypesDetail2 (FullTypesMainAgregator); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK4606ed7473204545864723409c4e86fa FOREIGN KEY (Ingridient2_m0) REFERENCES Cabbage2; 
CREATE INDEX Index1e7137bd903746f98daff5b9c06c501a on Salad2 (Ingridient2_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK59b51b9aa6a14d7db2896c0d48e96fc3 FOREIGN KEY (Ingridient2_m1) REFERENCES Plant2; 
CREATE INDEX Index41fae19e1daa4ef1bec48980742ba163 on Salad2 (Ingridient2_m1); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK62de64bc644e4f2f9b73748c46548bd2 FOREIGN KEY (Ingridient1_m0) REFERENCES Cabbage2; 
CREATE INDEX Index9731d337926549c8a5de8e46bf0284ec on Salad2 (Ingridient1_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKe7d2dec1c4c04868a8c1b4cbab8527e5 FOREIGN KEY (Ingridient1_m1) REFERENCES Plant2; 
CREATE INDEX Index7dd31af7454b434cad82f26494fc9748 on Salad2 (Ingridient1_m1); 

 ALTER TABLE Выплаты ADD CONSTRAINT FK372692cc7d444b0f9dee4a3475adde59 FOREIGN KEY (Кредит1) REFERENCES Кредит; 
CREATE INDEX Index4841efcdcd314a0bb01039bc276bbbb2 on Выплаты (Кредит1); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FK9d4393d139cf44a383ae50c2de03beae FOREIGN KEY (Личность_m0) REFERENCES Личность; 
CREATE INDEX Indexbccca0ddd4b94cd3886e24e7222c7993 on УчастникХозДог (Личность_m0); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FKd1f564aa3c9d41f8b388d17780d81e00 FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Index3a007c95c35c48d3b0b2a08c30be392a on УчастникХозДог (ХозДоговор_m0); 

 ALTER TABLE Перелом ADD CONSTRAINT FKc0a64938c67743138f94f6028085d908 FOREIGN KEY (Лапа_m0) REFERENCES Лапа; 
CREATE INDEX Index4a24ec78123d4bd7aadbe5fa85f7084a on Перелом (Лапа_m0); 

 ALTER TABLE Human2 ADD CONSTRAINT FK5481f462da6444c4aa5fa1a2ba948c38 FOREIGN KEY (TodayHome_m0) REFERENCES Country2; 
CREATE INDEX Indexf0533f036e69434d94e63413ddfeffb0 on Human2 (TodayHome_m0); 

 ALTER TABLE Human2 ADD CONSTRAINT FK575784db925e4041b9fa1d9317b19f25 FOREIGN KEY (TodayHome_m1) REFERENCES Territory2; 
CREATE INDEX Index3528430971d44399a79fbda5a47f5c6f on Human2 (TodayHome_m1); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FKf6c9810ff6ab4eb8b1840d7d8735b6fa FOREIGN KEY (ЗначениеКритер) REFERENCES ЗначениеКритер; 
CREATE INDEX Indexca098b1fccd245bf8af4296b581ec56e on ОценкаЭксперта (ЗначениеКритер); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FKb6b9c5697c144b49a86f7b5834bc71ec FOREIGN KEY (Эксперт_m0) REFERENCES Пользователь; 
CREATE INDEX Index34f5a038e1864d8e999688128ca88bad on ОценкаЭксперта (Эксперт_m0); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FKd70e683c5d764d43b2c8ec01fd7e8ad7 FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index38bbeb361439493581af6055bad01897 on ОценкаЭксперта (Идея_m0); 

 ALTER TABLE Медведь ADD CONSTRAINT FKc46da38ae34d43908ae5fe02a7e4295a FOREIGN KEY (Мама) REFERENCES Медведь; 
CREATE INDEX Index1bfa373ee0014fd1a547618838eae2e6 on Медведь (Мама); 

 ALTER TABLE Медведь ADD CONSTRAINT FK1a254cb88d7c49f78f225a24fa9e294c FOREIGN KEY (Папа) REFERENCES Медведь; 
CREATE INDEX Indexc09b3a2494754457b4aad7ef8605f268 on Медведь (Папа); 

 ALTER TABLE Медведь ADD CONSTRAINT FK8839c25316ee48d9b73dcc58d86be324 FOREIGN KEY (ЛесОбитания) REFERENCES Лес; 
CREATE INDEX Index8b9b044c169a45b8b0c50cc2fb49779c on Медведь (ЛесОбитания); 

 ALTER TABLE InformationTestClass6 ADD CONSTRAINT FK6be00e929f744bf0bb43362efc22bdb1 FOREIGN KEY (ExampleOfClassWithCaptions) REFERENCES ClassWithCaptions; 
CREATE INDEX Index5cda375109764b5fa00b1886d0b9304c on InformationTestClass6 (ExampleOfClassWithCaptions); 

 ALTER TABLE Берлога ADD CONSTRAINT FKe87e5c266bd44fbd89f385240ed7276a FOREIGN KEY (ЛесРасположения) REFERENCES Лес; 
CREATE INDEX Index4bdbe042cfa94b65af6cd2ff2ecd61bf on Берлога (ЛесРасположения); 

 ALTER TABLE Берлога ADD CONSTRAINT FK35e6962b785d4d899692686745cc117a FOREIGN KEY (Медведь) REFERENCES Медведь; 
CREATE INDEX Index72f690b6ded7483d90a836e3eb3c51d5 on Берлога (Медведь); 

 ALTER TABLE Apparatus2 ADD CONSTRAINT FK8865fe3641004057892fd07ca4b9ad66 FOREIGN KEY (Maker_m0) REFERENCES Country2; 
CREATE INDEX Indexbdbb9d0718704617b3c7dbd582435ae1 on Apparatus2 (Maker_m0); 

 ALTER TABLE Apparatus2 ADD CONSTRAINT FK5ed57bbbe255441b885c716d26141997 FOREIGN KEY (Exporter_m0) REFERENCES Country2; 
CREATE INDEX Indexd572999b5ec849ee977b830d22f77c92 on Apparatus2 (Exporter_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK760da294bdbf480b8efe4c2db3139e4a FOREIGN KEY (MainIngridient_m0) REFERENCES Cabbage2; 
CREATE INDEX Index066341a36b2f41bb9d672b43784c48b7 on Dish2 (MainIngridient_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK931f2574c8b744b0a3c7e6b1dfde3db3 FOREIGN KEY (MainIngridient_m1) REFERENCES Plant2; 
CREATE INDEX Index7d8c511b09d345cb9cc3d0b76abb4b70 on Dish2 (MainIngridient_m1); 

 ALTER TABLE КритерийОценки ADD CONSTRAINT FKfb3b6a1cc9ee492bab4719d16b950e44 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index151daa8cbe024869a85daefd30e52200 on КритерийОценки (Конкурс_m0); 

 ALTER TABLE InformationTestClass4 ADD CONSTRAINT FK065902a422e8482c84650cd897d66840 FOREIGN KEY (MasterOfInformationTestClass3) REFERENCES InformationTestClass3; 
CREATE INDEX Indexbda053e8498d49e3bc5336acd0dc7674 on InformationTestClass4 (MasterOfInformationTestClass3); 

 ALTER TABLE ClassWithCaptions ADD CONSTRAINT FKbbbfe22ec3774fd18ea8afa926339692 FOREIGN KEY (InformationTestClass4) REFERENCES InformationTestClass4; 
CREATE INDEX Index5c3302d7853145a4a2b91124659cb0c3 on ClassWithCaptions (InformationTestClass4); 

 ALTER TABLE Adress2 ADD CONSTRAINT FK9869fef2f9254b3ca96d7a9912f46eef FOREIGN KEY (Country_m0) REFERENCES Country2; 
CREATE INDEX Indexddf997e4f4ac49269fda6d6acac60dd4 on Adress2 (Country_m0); 

 ALTER TABLE Конкурс ADD CONSTRAINT FKcc49df3e42ca4493899b63706c63e0a5 FOREIGN KEY (Организатор_m0) REFERENCES Пользователь; 
CREATE INDEX Index666700a4a88c413bb009cce98dedf1e3 on Конкурс (Организатор_m0); 

 ALTER TABLE TypeUsageProviderTestClassChil ADD CONSTRAINT FKf93ec8d54be0444699148a9a756af4f8 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Indexaac44c20e9df4d9ebcabf56f978880e7 on TypeUsageProviderTestClassChil (DataObjectForTest_m0); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FK291c62bb9a564d98a686df7c3e5f83a6 FOREIGN KEY (Cabbage1) REFERENCES Cabbage2; 
CREATE INDEX Index7bf1b6d5e48e47519fb420ae16a14731 on CabbageSalad (Cabbage1); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FKe0f7b4e38d164b88bd03daef52f6d747 FOREIGN KEY (Cabbage2) REFERENCES Cabbage2; 
CREATE INDEX Indexc6fc2bbcc64a4fbda8ac7786c6ab6eaf on CabbageSalad (Cabbage2); 

 ALTER TABLE CabbagePart2 ADD CONSTRAINT FK2f4dd2e4124f4d5e8b04cbc3578369dc FOREIGN KEY (Cabbage) REFERENCES Cabbage2; 
CREATE INDEX Index5f785fda20bc44869e97a091de6021d0 on CabbagePart2 (Cabbage); 

 ALTER TABLE cla ADD CONSTRAINT FK6f0e3b0b26c8413392fe1286517aa44e FOREIGN KEY (parent) REFERENCES clb; 
CREATE INDEX Indexb111874b7f874149bc693e79e8309369 on cla (parent); 

 ALTER TABLE Place2 ADD CONSTRAINT FKa048b57786ac4893a6cb5ec268685406 FOREIGN KEY (TodayTerritory_m0) REFERENCES Country2; 
CREATE INDEX Indexa9aaccd4c0954369af092c9b720b6d4b on Place2 (TodayTerritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FK82ab0912d50b428aaac1b7cd618aad7d FOREIGN KEY (TodayTerritory_m1) REFERENCES Territory2; 
CREATE INDEX Index983fc7302b7d4df487471d66399dfd3a on Place2 (TodayTerritory_m1); 

 ALTER TABLE Place2 ADD CONSTRAINT FKc22869ced1944a8aa818e61c5866b350 FOREIGN KEY (TomorrowTeritory_m0) REFERENCES Country2; 
CREATE INDEX Indexc2d2844fd7fb42af807ca3b08d4c31f1 on Place2 (TomorrowTeritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FKf8267ab65e1d4cc5bbccf59630142f69 FOREIGN KEY (TomorrowTeritory_m1) REFERENCES Territory2; 
CREATE INDEX Index9cbeef30d4a04cac9388f84108033ce1 on Place2 (TomorrowTeritory_m1); 

 ALTER TABLE TypeUsageProviderTestClass ADD CONSTRAINT FK7d4b305544454a96b2b4186c6e112cc7 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Index8d88f58179114f68bb462f19bc94aba3 on TypeUsageProviderTestClass (DataObjectForTest_m0); 

 ALTER TABLE AuditAgregatorObject ADD CONSTRAINT FKbd3b34a5538f4657a7da0003aca2bb0c FOREIGN KEY (MasterObject) REFERENCES AuditMasterObject; 
CREATE INDEX Index7a17613de5ef41ceb9ac1ffdff0252ba on AuditAgregatorObject (MasterObject); 

 ALTER TABLE Region ADD CONSTRAINT FK299d3319bcce4d1a8e5c9fb5b5f7b2a0 FOREIGN KEY (Country2_m0) REFERENCES Country2; 
CREATE INDEX Index61aa6a960e52414992d2a83bfb27024c on Region (Country2_m0); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FK77f7a019c2c844a3ab372f63bf800d67 FOREIGN KEY (Владелец_m0) REFERENCES Пользователь; 
CREATE INDEX Index40a731aad2234eb4aafeefa2d8ca7f91 on ФайлИдеи (Владелец_m0); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FK50fa3d74e88647e88bd90350a9da0f92 FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Indexfa930bea3c6c4429abac29baa805dfa5 on ФайлИдеи (Идея_m0); 

 ALTER TABLE InformationTestClass3 ADD CONSTRAINT FK5a85429e3e4648d48dc100e7b8df5528 FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Index5c7cf55d438141ef8fccce637fca42bc on InformationTestClass3 (InformationTestClass2); 

 ALTER TABLE Soup2 ADD CONSTRAINT FK0e73ad511f5d4059aeb4641e900fbbb9 FOREIGN KEY (CabbageType) REFERENCES Cabbage2; 
CREATE INDEX Index1f7bfafab1e94686bac5fe713e73f908 on Soup2 (CabbageType); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK3c95444ee5244df88de7cb1d8e3f2997 FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Index42e7460a9f2641568c5cedb4b35b4092 on MasterClass (InformationTestClass2); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK6417fa63d341427c926b4c1033703e4f FOREIGN KEY (InformationTestClass3_m0) REFERENCES InformationTestClass3; 
CREATE INDEX Index91b8a7114ed54f5f99f8da8204656aff on MasterClass (InformationTestClass3_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FKbb7938ab31104e5e9c6fddb2b7ec80d4 FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Index8091a474623f4ae08e49f5d0745c51ef on MasterClass (InformationTestClass_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FKcdc7390e237941bfaac5304a41c68f27 FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Index49251b58177a43f0a9888a589a0796cb on MasterClass (InformationTestClass_m1); 

 ALTER TABLE STORMWEBSEARCH ADD CONSTRAINT FKa8e293a9668b4be5ad7f29055255df8a FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERDETAIL ADD CONSTRAINT FK743a645982ed442b890cfcc1fa33180c FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERLOOKUP ADD CONSTRAINT FKdde68fd8ae5447fd97fff714107473c1 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMAuEntity ADD CONSTRAINT FKe34def37ea9747a2bf32816562527f29 FOREIGN KEY (ObjectType_m0) REFERENCES STORMAuObjType; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FKedde9c1adc7d4ad2b38565cd6b40d5a6 FOREIGN KEY (MainChange_m0) REFERENCES STORMAuField; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FK1829e7214ba34ee2a7b3525788cd37b1 FOREIGN KEY (AuditEntity_m0) REFERENCES STORMAuEntity; 

