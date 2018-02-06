



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

 MasterClass UUID NOT NULL,

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




 ALTER TABLE Идея ADD CONSTRAINT FK3415e1a46b75493e80000670f1c86168 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index7eef1c85491c4145a50c96cc2576b870 on Идея (Конкурс_m0); 

 ALTER TABLE Идея ADD CONSTRAINT FK1380abb4d7354f27857eeaf0954d280c FOREIGN KEY (Автор_m0) REFERENCES Пользователь; 
CREATE INDEX Index5bf01084fdcc4e53b0b5de573420a2ae on Идея (Автор_m0); 

 ALTER TABLE AuditMasterObject ADD CONSTRAINT FK73c13db8bf844daf858d429617798f3a FOREIGN KEY (MasterObject) REFERENCES AuditMasterMasterObject; 
CREATE INDEX Index5468a17620cc41c7b4868b5345f18609 on AuditMasterObject (MasterObject); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FKa24fadcdd0bf40eca05a1f798966dd9f FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Index00e6aff305b84e9f9fa061e6b95f012d on InformationTestClass2 (InformationTestClass_m0); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FKd9c4f43ab8b940948ac24860a094c0a1 FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Index4853c0d3527b4b23aa7cfd59b65cf1d3 on InformationTestClass2 (InformationTestClass_m1); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK00ab7a052c464107896e5c2aecf2ec61 FOREIGN KEY (Мастер_m0) REFERENCES МастерМ; 
CREATE INDEX Index79ad8d44f6ca46989af24a5ab370d29f on TestClassA (Мастер_m0); 

 ALTER TABLE TestClassA ADD CONSTRAINT FKbbac32ca9bd341ba8b5f6081b4e0d517 FOREIGN KEY (Мастер_m1) REFERENCES НаследникМ1; 
CREATE INDEX Index0515bd91e5014dd58ca97b539571fb15 on TestClassA (Мастер_m1); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK022393d0191545d18944d0ed6d319da5 FOREIGN KEY (Мастер_m2) REFERENCES НаследникМ2; 
CREATE INDEX Indexd7ecb9cc5bc84910803df3999c9e0306 on TestClassA (Мастер_m2); 

 ALTER TABLE clb ADD CONSTRAINT FK6c36984435534e5a9b1af65e1909a852 FOREIGN KEY (ref2) REFERENCES cla; 
CREATE INDEX Index30598a3f76c746cfaf10f30aa47e0e22 on clb (ref2); 

 ALTER TABLE clb ADD CONSTRAINT FK41575bb9571d467bb6a00bf21f64ab94 FOREIGN KEY (ref1) REFERENCES cla; 
CREATE INDEX Index47589e4b470046ef85ad70430f72c7f2 on clb (ref1); 

 ALTER TABLE AggregatorUpdateObjectTest ADD CONSTRAINT FK66c1d8f8320f48638751e82d542dc64c FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Index1c6224b24d9a4add92b79d1d5086ae77 on AggregatorUpdateObjectTest (Detail); 

 ALTER TABLE Лапа ADD CONSTRAINT FKe4e341b7f66b41a680a98a400ca0d496 FOREIGN KEY (ТипЛапы_m0) REFERENCES ТипЛапы; 
CREATE INDEX Index31fdcf14ba814c68b199a129ac830abc on Лапа (ТипЛапы_m0); 

 ALTER TABLE Лапа ADD CONSTRAINT FKa56520424ba24b8b9215d574733e323f FOREIGN KEY (Кошка_m0) REFERENCES Кошка; 
CREATE INDEX Index5ca66a64a07d410dbd5b3509c021c870 on Лапа (Кошка_m0); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FK272811d765f549fdb3108b76f7ff41fa FOREIGN KEY (ИсточникФинан) REFERENCES ИсточникФинанс; 
CREATE INDEX Index57c00e208d1e469ab839326f43b53df7 on ИФХозДоговора (ИсточникФинан); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FK8ae0bc6f3776440eaf6a68b042c25e8e FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Index9b1658d4a684404aba300090eb9d3adf on ИФХозДоговора (ХозДоговор_m0); 

 ALTER TABLE Котенок ADD CONSTRAINT FK2fa020cadaee458b9016e45398ac0dd5 FOREIGN KEY (Кошка) REFERENCES Кошка; 
CREATE INDEX Indexda60f57a1fb245869a54782ae0319719 on Котенок (Кошка); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FKb5180e1b88534d13a5529f6a96ebbf18 FOREIGN KEY (Критерий_m0) REFERENCES КритерийОценки; 
CREATE INDEX Indexc8340b16226442a78cb684b3b00bcdb0 on ЗначениеКритер (Критерий_m0); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FKfcfe75a0801e42808a0bf1c7932f86ac FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index81d05d025b1740d8b9541ca684ded7e1 on ЗначениеКритер (Идея_m0); 

 ALTER TABLE ДокККонкурсу ADD CONSTRAINT FKad53cdf0fa564e18ae55c79a8dd214be FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Indexf74aecf8199b4aaea1897dc34da4940f on ДокККонкурсу (Конкурс_m0); 

 ALTER TABLE Лес ADD CONSTRAINT FK3781aa50a41a4504a1845cbc4112a745 FOREIGN KEY (Страна) REFERENCES Страна; 
CREATE INDEX Index5641ad672e4f49978b48f3196a5f3ecd on Лес (Страна); 

 ALTER TABLE FullTypesDetail1 ADD CONSTRAINT FK48698018344c48d2b48e1177a4804a58 FOREIGN KEY (FullTypesMainAgregator_m0) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Index87f70928284b4ba0a1b35c13e298be06 on FullTypesDetail1 (FullTypesMainAgregator_m0); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FK68a4118702184ec79ee8cbd8a13252cd FOREIGN KEY (Master) REFERENCES MasterUpdateObjectTest; 
CREATE INDEX Index67e1102119904a0d88b3831bf08cbac7 on DetailUpdateObjectTest (Master); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FKc098a8b329564c91b36c46698d581c94 FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Index52ccd32ba5fd46bd8648f3995d84c0c8 on DetailUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE Порода ADD CONSTRAINT FK942f42ea22cb4c29a511838ef3267536 FOREIGN KEY (ТипПороды) REFERENCES ТипПороды; 
CREATE INDEX Index0645300bc73b43bab48c56d78fe4913f on Порода (ТипПороды); 

 ALTER TABLE Порода ADD CONSTRAINT FK2445d2913170423b9e81411c1a78aaf1 FOREIGN KEY (Иерархия) REFERENCES Порода; 
CREATE INDEX Indexcc1444cd87ab4752ad8e0d78a88d20cf on Порода (Иерархия); 

 ALTER TABLE Блоха ADD CONSTRAINT FK558dfeb1f86d412dad5cbdb9b3d4be85 FOREIGN KEY (МедведьОбитания) REFERENCES Медведь; 
CREATE INDEX Index8d6a97d4667e416e852fc979f05d7b43 on Блоха (МедведьОбитания); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK6d4c9634f3f748e6aaad1f6403faca4f FOREIGN KEY (MasterClass) REFERENCES MasterClass; 
CREATE INDEX Indexb099122b2e8046b5a79ae08a6304428b on DetailClass (MasterClass); 

 ALTER TABLE Кредит ADD CONSTRAINT FK89f7a3a94ed8406c8f25acbc7d412e93 FOREIGN KEY (Клиент) REFERENCES Клиент; 
CREATE INDEX Index842336f3d4b545fb91a40e1216e3a3ca on Кредит (Клиент); 

 ALTER TABLE Кредит ADD CONSTRAINT FK79b19b8b8c834652a7e7638d619526cc FOREIGN KEY (ИнспекторПоКред) REFERENCES ИнспПоКредиту; 
CREATE INDEX Index4fbd9305bf0c483d9a09cdb35169be79 on Кредит (ИнспекторПоКред); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FKe93cd799945e4675860ff4afac20a14b FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Index039fde9e6a5a4f0481c0c16d377d05b9 on MasterUpdateObjectTest (Detail); 

 ALTER TABLE MasterUpdateObjectTest ADD CONSTRAINT FKb2429df197834d64b149a69c04b2c75b FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Indexe210c13ca2fa4f4a960fe548d53dccbc on MasterUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE FullTypesMainAgregator ADD CONSTRAINT FK246db8597dc14612bf6e477bc3d497d2 FOREIGN KEY (FullTypesMaster1_m0) REFERENCES FullTypesMaster1; 
CREATE INDEX Index6bbc467cfac1414288b4f19ea9d1bdb1 on FullTypesMainAgregator (FullTypesMaster1_m0); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FKfac2b151681d4418919f16eff7334a02 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Index085074ceb37c4f95b643d0b0eaf88b0a on CombinedTypesUsageProviderTest (DataObjectForTest_m0); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FK1f0b77e8581c4c36bcaa70bcfc3bba55 FOREIGN KEY (TypeUsageProviderTestClass) REFERENCES TypeUsageProviderTestClass; 
CREATE INDEX Indexc5af599da7bc40f8877d2f07704e072c on CombinedTypesUsageProviderTest (TypeUsageProviderTestClass); 

 ALTER TABLE Кошка ADD CONSTRAINT FK1ad3f332561b445fbf0d157f58a5b2fc FOREIGN KEY (Порода) REFERENCES Порода; 
CREATE INDEX Index6311ee388a75499fb20adee425c7a4b6 on Кошка (Порода); 

 ALTER TABLE SomeDetailClass ADD CONSTRAINT FKa0283b96234e4395b88c632589b2ffb1 FOREIGN KEY (ClassA) REFERENCES SomeMasterClass; 
CREATE INDEX Indexacd8923cd78149ad94032beccac4b2a7 on SomeDetailClass (ClassA); 

 ALTER TABLE FullTypesDetail2 ADD CONSTRAINT FK433a9243e37840b4bacb3b57f8ef721f FOREIGN KEY (FullTypesMainAgregator) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Index15c7310a3e7f4aba8e077c570adee632 on FullTypesDetail2 (FullTypesMainAgregator); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKbdad9dd915d3487faf4820cc96601fc1 FOREIGN KEY (Ingridient2_m0) REFERENCES Cabbage2; 
CREATE INDEX Index71a33d2a29074553be239fcc26bd1d41 on Salad2 (Ingridient2_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKc2e7ab87273641b9bb4aa9a5240dfc87 FOREIGN KEY (Ingridient2_m1) REFERENCES Plant2; 
CREATE INDEX Indexc461ef433f4d494ba690cff12129faa7 on Salad2 (Ingridient2_m1); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK4764287579454a1eadef97b0864621ed FOREIGN KEY (Ingridient1_m0) REFERENCES Cabbage2; 
CREATE INDEX Index076bd84b1ff546e7bb30043182bd63e8 on Salad2 (Ingridient1_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK56fe5707f137455190de8a3ff7da259b FOREIGN KEY (Ingridient1_m1) REFERENCES Plant2; 
CREATE INDEX Index5cd5997105334fbbb8cf914924a42e87 on Salad2 (Ingridient1_m1); 

 ALTER TABLE Выплаты ADD CONSTRAINT FK4055f9fec4464bb5a4ae326033dc4d5a FOREIGN KEY (Кредит1) REFERENCES Кредит; 
CREATE INDEX Indexd2c18dd677b245bab2a1a9e1fdeb6755 on Выплаты (Кредит1); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FKcc4ff95a8b1e4957b85aaf69d26aa5a4 FOREIGN KEY (Личность_m0) REFERENCES Личность; 
CREATE INDEX Indexcd1c9663cacf43908ed920579f65dc2e on УчастникХозДог (Личность_m0); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FK40281466694e400ebd7820f53335f63c FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Indexbf1e3198e421493e966efe858197dee1 on УчастникХозДог (ХозДоговор_m0); 

 ALTER TABLE Перелом ADD CONSTRAINT FKca48f3826bee4850b688feffbc55ae51 FOREIGN KEY (Лапа_m0) REFERENCES Лапа; 
CREATE INDEX Indexfefd9ebb743840a2978cc02cd4ca622f on Перелом (Лапа_m0); 

 ALTER TABLE Human2 ADD CONSTRAINT FK6b1a430823ae4c14bae119fd7604a5a0 FOREIGN KEY (TodayHome_m0) REFERENCES Country2; 
CREATE INDEX Indexfc3b528848434ceca79fbd58e57bf892 on Human2 (TodayHome_m0); 

 ALTER TABLE Human2 ADD CONSTRAINT FKb9a0548a38da487495a87bb7d2144a57 FOREIGN KEY (TodayHome_m1) REFERENCES Territory2; 
CREATE INDEX Index38b50038e4c84e7bba0d96c54c0956c3 on Human2 (TodayHome_m1); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK045d5a82c52249edbfbe8a07696657bc FOREIGN KEY (ЗначениеКритер) REFERENCES ЗначениеКритер; 
CREATE INDEX Indexd1f5aacf8c82441da3ee48102c355e6f on ОценкаЭксперта (ЗначениеКритер); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK02ab033c0e744fe7b1bf3ea864f57fff FOREIGN KEY (Эксперт_m0) REFERENCES Пользователь; 
CREATE INDEX Index0c5724f3e2c04602aeaa71448946ebd5 on ОценкаЭксперта (Эксперт_m0); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK4e62bf3d9aeb4a06aea5b84d0ff78c93 FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index972056254e7f4f74a685b08563d0123d on ОценкаЭксперта (Идея_m0); 

 ALTER TABLE Медведь ADD CONSTRAINT FK57d07ab6e8a1459da2ec32e8150f9d6f FOREIGN KEY (Мама) REFERENCES Медведь; 
CREATE INDEX Index74f99f0280904722a864827b234f948a on Медведь (Мама); 

 ALTER TABLE Медведь ADD CONSTRAINT FK77b2429ff05b4fab99305b89520fddd7 FOREIGN KEY (Папа) REFERENCES Медведь; 
CREATE INDEX Index63f854c36ebe433987ecbeb348518bf8 on Медведь (Папа); 

 ALTER TABLE Медведь ADD CONSTRAINT FK6ea3e68b68f2471ba6aefb44a64b42d3 FOREIGN KEY (ЛесОбитания) REFERENCES Лес; 
CREATE INDEX Indexe4d21487fcac4d8faa160e0b90d2a5b7 on Медведь (ЛесОбитания); 

 ALTER TABLE InformationTestClass6 ADD CONSTRAINT FK947a03b059cc4fceb3a3aed2b6243281 FOREIGN KEY (ExampleOfClassWithCaptions) REFERENCES ClassWithCaptions; 
CREATE INDEX Indexc215a92511e54ddfa4e9ae6c5580db3a on InformationTestClass6 (ExampleOfClassWithCaptions); 

 ALTER TABLE Берлога ADD CONSTRAINT FK5a6a46c558eb4bfc8834b34309d35cde FOREIGN KEY (ЛесРасположения) REFERENCES Лес; 
CREATE INDEX Indexf694045b866b42838d99220a16539143 on Берлога (ЛесРасположения); 

 ALTER TABLE Берлога ADD CONSTRAINT FK6a21a04aae0e4152824cb8ee91f32883 FOREIGN KEY (Медведь) REFERENCES Медведь; 
CREATE INDEX Indexebfb8100a0524d9da0caa91af935115e on Берлога (Медведь); 

 ALTER TABLE Apparatus2 ADD CONSTRAINT FK63b9ad2e5d864499a0b01f20fe6757cf FOREIGN KEY (Maker_m0) REFERENCES Country2; 
CREATE INDEX Index2687f3ce48c7470fa0f205c47edc161a on Apparatus2 (Maker_m0); 

 ALTER TABLE Apparatus2 ADD CONSTRAINT FK5f7fd9f51ae948c3ab44561525d4e9b0 FOREIGN KEY (Exporter_m0) REFERENCES Country2; 
CREATE INDEX Index3136cc18b9034e4fb6f601df78ec7b2b on Apparatus2 (Exporter_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK4aee8d1c91304866a914702848e41b60 FOREIGN KEY (MainIngridient_m0) REFERENCES Cabbage2; 
CREATE INDEX Indexfe55b8bf91164483b1aa8b8906290861 on Dish2 (MainIngridient_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK506071996f8548509b85db86e130bb32 FOREIGN KEY (MainIngridient_m1) REFERENCES Plant2; 
CREATE INDEX Indexff152207fe9e47d784962b7a2ae56a59 on Dish2 (MainIngridient_m1); 

 ALTER TABLE КритерийОценки ADD CONSTRAINT FKb71dd30ed98044c18c7ef19058adf27d FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Indexa9e64b8d4e764725a47dbdda851bc8be on КритерийОценки (Конкурс_m0); 

 ALTER TABLE InformationTestClass4 ADD CONSTRAINT FK8e5791e9d8ce4ce989bc94fb85d00e6d FOREIGN KEY (MasterOfInformationTestClass3) REFERENCES InformationTestClass3; 
CREATE INDEX Index8e26fd791e20418286c476edf28f2dfd on InformationTestClass4 (MasterOfInformationTestClass3); 

 ALTER TABLE ClassWithCaptions ADD CONSTRAINT FK20f858ca0d684d13bce746e05c359a0d FOREIGN KEY (InformationTestClass4) REFERENCES InformationTestClass4; 
CREATE INDEX Index4badb9b3bcd648f69eb9f224829c5d9b on ClassWithCaptions (InformationTestClass4); 

 ALTER TABLE Adress2 ADD CONSTRAINT FK5c2fc0f0cdb14fa6b01b3af342b8472b FOREIGN KEY (Country_m0) REFERENCES Country2; 
CREATE INDEX Index34a75ddf1d364867a6dd42bb63c1d929 on Adress2 (Country_m0); 

 ALTER TABLE Конкурс ADD CONSTRAINT FKe02bc8a825034f4b815417a5167afb32 FOREIGN KEY (Организатор_m0) REFERENCES Пользователь; 
CREATE INDEX Index112fa8c1d936483a8951ae978d137ac0 on Конкурс (Организатор_m0); 

 ALTER TABLE TypeUsageProviderTestClassChil ADD CONSTRAINT FK81680bf57f204cd7b97641e3e0dd1803 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Indexf5aeeb49e16b4fb0a06f9e73935dde94 on TypeUsageProviderTestClassChil (DataObjectForTest_m0); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FK1d03cd407cd04bf78c2e3d3ffc5026a3 FOREIGN KEY (Cabbage1) REFERENCES Cabbage2; 
CREATE INDEX Indexf68faaac50814a06940fe2f98237c210 on CabbageSalad (Cabbage1); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FK4ac891cd348f4ae09f39406e57e8e6cf FOREIGN KEY (Cabbage2) REFERENCES Cabbage2; 
CREATE INDEX Index2a523f8b6b6444a1924c0c38fd2faaf1 on CabbageSalad (Cabbage2); 

 ALTER TABLE CabbagePart2 ADD CONSTRAINT FKf62902fab7684b908617351ea2c67ce3 FOREIGN KEY (Cabbage) REFERENCES Cabbage2; 
CREATE INDEX Index7207f14528fe469b9c2549241ef0e8ed on CabbagePart2 (Cabbage); 

 ALTER TABLE cla ADD CONSTRAINT FK6c418a1844e841a68f5d0bde4255efb7 FOREIGN KEY (parent) REFERENCES clb; 
CREATE INDEX Indexc912fa6315424bbda4690c6dd26eb7c0 on cla (parent); 

 ALTER TABLE Place2 ADD CONSTRAINT FK11cffce67fd142389f60ddb4429fa935 FOREIGN KEY (TodayTerritory_m0) REFERENCES Country2; 
CREATE INDEX Index612144d87c204832be02460f17e32b4e on Place2 (TodayTerritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FK580628a1633846fb92e5d3b52899eebb FOREIGN KEY (TodayTerritory_m1) REFERENCES Territory2; 
CREATE INDEX Indexb128eaeed88c429a80ae361f57fa7c61 on Place2 (TodayTerritory_m1); 

 ALTER TABLE Place2 ADD CONSTRAINT FK9f4974cd4c1c4ca8a243052764b0d688 FOREIGN KEY (TomorrowTeritory_m0) REFERENCES Country2; 
CREATE INDEX Index1c72ef0df5534f58a3506c012c2ca606 on Place2 (TomorrowTeritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FK6af20e11259048eea020c5853b2ad572 FOREIGN KEY (TomorrowTeritory_m1) REFERENCES Territory2; 
CREATE INDEX Index7a6ef56df9164749908c59850dec3acd on Place2 (TomorrowTeritory_m1); 

 ALTER TABLE TypeUsageProviderTestClass ADD CONSTRAINT FK99878a819e39459d8b20f995b91bfb4b FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Index72f1e8ca0a174fba93974baf2013e397 on TypeUsageProviderTestClass (DataObjectForTest_m0); 

 ALTER TABLE AuditAgregatorObject ADD CONSTRAINT FK4e352c3693ec43889f3782ec5c7b942e FOREIGN KEY (MasterObject) REFERENCES AuditMasterObject; 
CREATE INDEX Index6b8a86ad445748cf88d2c3433175f99f on AuditAgregatorObject (MasterObject); 

 ALTER TABLE Region ADD CONSTRAINT FK989d712ef6dd4e8bb0b7aa77af2eb031 FOREIGN KEY (Country2_m0) REFERENCES Country2; 
CREATE INDEX Index978bc68bb2d14f039e7b8f1b76dae202 on Region (Country2_m0); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FK30c45f0325f34a1b89a868b02f2eea1b FOREIGN KEY (Владелец_m0) REFERENCES Пользователь; 
CREATE INDEX Indexbe1f9db38f2841d6b89d52d4a10e5b7c on ФайлИдеи (Владелец_m0); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FK5b833ae1b5d84022bcbcbecbe6a5df3e FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index37e2c6aa25c842d4a9a4cdc2e8f86973 on ФайлИдеи (Идея_m0); 

 ALTER TABLE InformationTestClass3 ADD CONSTRAINT FK4204a7c42cb94bfdaaf90d33d750e2ea FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Indexe254f4ec074f43b585206e0a5f9cc2a1 on InformationTestClass3 (InformationTestClass2); 

 ALTER TABLE Soup2 ADD CONSTRAINT FK21f9e0651c8f45ada9960ee401a27c40 FOREIGN KEY (CabbageType) REFERENCES Cabbage2; 
CREATE INDEX Indexd422fff81b634bf88e5295a48538596d on Soup2 (CabbageType); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK9e34d8bbbc05420ca8ad97fa567250ad FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Index8fb21ef73c3042d697e256cbbe95ea6e on MasterClass (InformationTestClass2); 

 ALTER TABLE MasterClass ADD CONSTRAINT FKbe30b5bfb2084f3f8279106bea392e08 FOREIGN KEY (InformationTestClass3_m0) REFERENCES InformationTestClass3; 
CREATE INDEX Index31c38e9fe1fd4ab5a33d71c8b82beae6 on MasterClass (InformationTestClass3_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK753a673eec0140f69e27de5dfcaa6ebf FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Index26880d1bea4f4a3fa5b3cd3df0a36f28 on MasterClass (InformationTestClass_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK0314f0f229ec4e7eacb244af835876b7 FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Indexcdc824d4e8c24cad96f34476f95b6688 on MasterClass (InformationTestClass_m1); 

 ALTER TABLE STORMWEBSEARCH ADD CONSTRAINT FK46977912166f45a8b033152f6b2026e7 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERDETAIL ADD CONSTRAINT FK0e81dbc28ad640869dd84f7058fb6205 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERLOOKUP ADD CONSTRAINT FK4a7dedffe83e4ccd9653bf1ff19bdcd4 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMAuEntity ADD CONSTRAINT FK990bcf0012d94a93a21c1b8098dde42e FOREIGN KEY (ObjectType_m0) REFERENCES STORMAuObjType; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FK2f3e9bb19f71487aa4d018870d7da7e9 FOREIGN KEY (MainChange_m0) REFERENCES STORMAuField; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FKd9c2c88c9e144f8fa868ae7a55d766b2 FOREIGN KEY (AuditEntity_m0) REFERENCES STORMAuEntity; 

