





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

 ColorPropertyForInfTestClass VARCHAR(255) NULL,

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




 ALTER TABLE Идея ADD CONSTRAINT FK7807bdaf568846a6bf25a7f52f43ef7f FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index9e5a8937cbb84a0291ccf063b6e48462 on Идея (Конкурс_m0); 

 ALTER TABLE Идея ADD CONSTRAINT FK197fd835ac0944eb8c9dc466dd7ab6a0 FOREIGN KEY (Автор_m0) REFERENCES Пользователь; 
CREATE INDEX Indexca247c2039ac4f6ea1629d92fed25611 on Идея (Автор_m0); 

 ALTER TABLE AuditMasterObject ADD CONSTRAINT FK3de856ea5f2641f39321e740dfaa9a94 FOREIGN KEY (MasterObject) REFERENCES AuditMasterMasterObject; 
CREATE INDEX Indexce4d5bf3f674455188a7d464fed994ae on AuditMasterObject (MasterObject); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FKdf73f712cff84ba5973f0712e2593395 FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Index02e8e5921b934a009f9303357405210b on InformationTestClass2 (InformationTestClass_m0); 

 ALTER TABLE InformationTestClass2 ADD CONSTRAINT FK8845b1abefc94ffaa420a3b4a671facd FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Index47a05cb7362f46748049607972a654dc on InformationTestClass2 (InformationTestClass_m1); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK730866d9382c4c429c7f9a3a597b1542 FOREIGN KEY (Мастер_m0) REFERENCES МастерМ; 
CREATE INDEX Index8e6c8f84960c413c8ac17141a3baaeb7 on TestClassA (Мастер_m0); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK15a405c4f0514fd4a2476763824b094c FOREIGN KEY (Мастер_m1) REFERENCES НаследникМ1; 
CREATE INDEX Indexb8af48e20e254b9c98550e032f0e87d0 on TestClassA (Мастер_m1); 

 ALTER TABLE TestClassA ADD CONSTRAINT FK54a4be93531b4606985d4fd25d08efa1 FOREIGN KEY (Мастер_m2) REFERENCES НаследникМ2; 
CREATE INDEX Index7696d2e1c5174b00aba51d1c8389a901 on TestClassA (Мастер_m2); 

 ALTER TABLE clb ADD CONSTRAINT FK47984ab8c72e47d3b799fcc9da853a07 FOREIGN KEY (ref2) REFERENCES cla; 
CREATE INDEX Index381b45a038f74f5d8ea4605ff42ca68f on clb (ref2); 

 ALTER TABLE clb ADD CONSTRAINT FK27115b6ca3974f0298e25969472aba17 FOREIGN KEY (ref1) REFERENCES cla; 
CREATE INDEX Indexf93460cfaf534234955740db157082a5 on clb (ref1); 

 ALTER TABLE AggregatorUpdateObjectTest ADD CONSTRAINT FK85f245f87bc144c3826cf2bbe0cf3d26 FOREIGN KEY (Detail) REFERENCES DetailUpdateObjectTest; 
CREATE INDEX Indexbfe387591cfc4368b3fa9ab1b67fd88e on AggregatorUpdateObjectTest (Detail); 

 ALTER TABLE Лапа ADD CONSTRAINT FK5ff52c1e082743478b62be0108b48312 FOREIGN KEY (ТипЛапы_m0) REFERENCES ТипЛапы; 
CREATE INDEX Indexc035b610aca242ff8545128c00281181 on Лапа (ТипЛапы_m0); 

 ALTER TABLE Лапа ADD CONSTRAINT FK15efc440f691400a8111565c6706c296 FOREIGN KEY (Кошка_m0) REFERENCES Кошка; 
CREATE INDEX Index58c16cb1a29147bd8fd8aced67e5bc55 on Лапа (Кошка_m0); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FKe75a71432a304e09a95d39371d4e06a0 FOREIGN KEY (ИсточникФинан) REFERENCES ИсточникФинанс; 
CREATE INDEX Indexf024e484283a40ef9486e3ab937aebf9 on ИФХозДоговора (ИсточникФинан); 

 ALTER TABLE ИФХозДоговора ADD CONSTRAINT FK343dbe47950049fd865979c2e80310a2 FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Index252fc6aa764241c1b2b0551bee712278 on ИФХозДоговора (ХозДоговор_m0); 

 ALTER TABLE Котенок ADD CONSTRAINT FKb78c76dae4354906b0b5e40627df15fa FOREIGN KEY (Кошка) REFERENCES Кошка; 
CREATE INDEX Index8b12dfd0e1d7450ea13511c6659f743c on Котенок (Кошка); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FK64d8e6dcb619421ba740cf2ac638f679 FOREIGN KEY (Критерий_m0) REFERENCES КритерийОценки; 
CREATE INDEX Indexca22e563d7604f4fb7c7203de096d107 on ЗначениеКритер (Критерий_m0); 

 ALTER TABLE ЗначениеКритер ADD CONSTRAINT FK71c8ec3409994c1daca92dceafd7697c FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index9a663cdc6aea4b89b72d56252bffa902 on ЗначениеКритер (Идея_m0); 

 ALTER TABLE ДокККонкурсу ADD CONSTRAINT FK6a8e77c4476945b39ed6e8ef0f3e5dc9 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Indexe2482f50bb5f45bba4f012ea30da1c36 on ДокККонкурсу (Конкурс_m0); 

 ALTER TABLE Лес ADD CONSTRAINT FK7b5797ac81e340e0a28232acfa1544f1 FOREIGN KEY (Страна) REFERENCES Страна; 
CREATE INDEX Index69eb3ebd410b4a16902694f9ad389935 on Лес (Страна); 

 ALTER TABLE FullTypesDetail1 ADD CONSTRAINT FK2f6545bfb3a640cbb5c68acb31834f70 FOREIGN KEY (FullTypesMainAgregator_m0) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Index5dd67ff3c3e141bba95db1afc951230a on FullTypesDetail1 (FullTypesMainAgregator_m0); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FKf53c1f1921eb4385b390d31ce8de023d FOREIGN KEY (Master) REFERENCES MasterUpdateObjectTest; 
CREATE INDEX Indexd02ce5cc72a544f1ad16e926e4322537 on DetailUpdateObjectTest (Master); 

 ALTER TABLE DetailUpdateObjectTest ADD CONSTRAINT FK4f9e4d1f92f347ba83fcfa7b307e1548 FOREIGN KEY (AggregatorUpdateObjectTest) REFERENCES AggregatorUpdateObjectTest; 
CREATE INDEX Index35e36263e0044463b8da3df86112c710 on DetailUpdateObjectTest (AggregatorUpdateObjectTest); 

 ALTER TABLE Порода ADD CONSTRAINT FK0bacfd39492b4a42a3843a4d3e2a9047 FOREIGN KEY (ТипПороды) REFERENCES ТипПороды; 
CREATE INDEX Index3a71074b09d4436696443fc5b9f24849 on Порода (ТипПороды); 

 ALTER TABLE Порода ADD CONSTRAINT FKef70b627e2664f83b42dad9f85be4b5a FOREIGN KEY (Иерархия) REFERENCES Порода; 
CREATE INDEX Indexe91006ec87bc4b42bc0ac0086407d324 on Порода (Иерархия); 

 ALTER TABLE Блоха ADD CONSTRAINT FKba880762e89045ac83dc5f75c5897530 FOREIGN KEY (МедведьОбитания) REFERENCES Медведь; 
CREATE INDEX Index739c15dae84144629d7d22e262510999 on Блоха (МедведьОбитания); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK63b4af636854468ca78fdf71211bcf45 FOREIGN KEY (MasterClass) REFERENCES MasterClass; 
CREATE INDEX Indexd17c28313d774a64bc0b6728747350dc on DetailClass (MasterClass); 

 ALTER TABLE Кредит ADD CONSTRAINT FK600c4c41231a401486133ac8dcf73c04 FOREIGN KEY (Клиент) REFERENCES Клиент; 
CREATE INDEX Index0f3c37d9a7614305bd9fb8b7fc2a8d2f on Кредит (Клиент); 

 ALTER TABLE Кредит ADD CONSTRAINT FKca099544659348628a22879b8a0bf6ca FOREIGN KEY (ИнспекторПоКред) REFERENCES ИнспПоКредиту; 
CREATE INDEX Indexfa3d82df5a054a02a0f86b3a79fa1a1f on Кредит (ИнспекторПоКред); 

 ALTER TABLE FullTypesMainAgregator ADD CONSTRAINT FK4a82ed38469f4aeca85cbfc0cbf0e779 FOREIGN KEY (FullTypesMaster1_m0) REFERENCES FullTypesMaster1; 
CREATE INDEX Indexbb00f7d7e9b34f6dbe1e59c8f6231e7c on FullTypesMainAgregator (FullTypesMaster1_m0); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FK5a00e32663c04b3381d87135892ca234 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Indexb123387d481a4604a1705412ed0d9ff4 on CombinedTypesUsageProviderTest (DataObjectForTest_m0); 

 ALTER TABLE CombinedTypesUsageProviderTest ADD CONSTRAINT FKec687cca1f0242988e67008f7e3d65fb FOREIGN KEY (TypeUsageProviderTestClass) REFERENCES TypeUsageProviderTestClass; 
CREATE INDEX Indexaf7b7483cba5428286fa61e12b15d0c4 on CombinedTypesUsageProviderTest (TypeUsageProviderTestClass); 

 ALTER TABLE Кошка ADD CONSTRAINT FKd8b3ee06cb474c6db409fd3159c722d0 FOREIGN KEY (Порода) REFERENCES Порода; 
CREATE INDEX Index2882929fa944449d995f0dcb8174c9b5 on Кошка (Порода); 

 ALTER TABLE SomeDetailClass ADD CONSTRAINT FK7ff457195b66442aa1b56ed58c0a5352 FOREIGN KEY (ClassA) REFERENCES SomeMasterClass; 
CREATE INDEX Indexed2ab6f00d60464dbf5533c8b0d493a5 on SomeDetailClass (ClassA); 

 ALTER TABLE FullTypesDetail2 ADD CONSTRAINT FK7ff31fbeb80b496fbba47a7e2c6f8a51 FOREIGN KEY (FullTypesMainAgregator) REFERENCES FullTypesMainAgregator; 
CREATE INDEX Indexd210a0aa41a744ab82cf681e80c1d5bc on FullTypesDetail2 (FullTypesMainAgregator); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK33547d2e90ee40a8bbafd484e463e922 FOREIGN KEY (Ingridient2_m0) REFERENCES Cabbage2; 
CREATE INDEX Index450bf2b1ce2e4d309156720fc4b20dad on Salad2 (Ingridient2_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK5260086ebc844361a24bb2fc1a489db9 FOREIGN KEY (Ingridient2_m1) REFERENCES Plant2; 
CREATE INDEX Index31a42c4106e84bec873738273884e45b on Salad2 (Ingridient2_m1); 

 ALTER TABLE Salad2 ADD CONSTRAINT FK8620248828a34e9ea40d71f39bfaaf45 FOREIGN KEY (Ingridient1_m0) REFERENCES Cabbage2; 
CREATE INDEX Indexebf2b39a1d854a7c8499ad4c14565897 on Salad2 (Ingridient1_m0); 

 ALTER TABLE Salad2 ADD CONSTRAINT FKadb7553b96644b7b85bd5cec619b0ad2 FOREIGN KEY (Ingridient1_m1) REFERENCES Plant2; 
CREATE INDEX Index5be6ed4ce5874b168c6e172391ca77f9 on Salad2 (Ingridient1_m1); 

 ALTER TABLE Выплаты ADD CONSTRAINT FK250b88b81da147ddac4a080b8db00968 FOREIGN KEY (Кредит1) REFERENCES Кредит; 
CREATE INDEX Indexa67d314707f74a568dafd23b8c637ef9 on Выплаты (Кредит1); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FK8126ff2bbc8c4d5c9bda05263252ff54 FOREIGN KEY (Личность_m0) REFERENCES Личность; 
CREATE INDEX Index72610b276613476eb2e9edda3ad04a4a on УчастникХозДог (Личность_m0); 

 ALTER TABLE УчастникХозДог ADD CONSTRAINT FK39699fe0041b49909e056b784c03c40b FOREIGN KEY (ХозДоговор_m0) REFERENCES ХозДоговор; 
CREATE INDEX Index00a3c568a8bf46d4a92855f172ff2dd7 on УчастникХозДог (ХозДоговор_m0); 

 ALTER TABLE Перелом ADD CONSTRAINT FK7da72ab2825e47e98fefb56369bf80e3 FOREIGN KEY (Лапа_m0) REFERENCES Лапа; 
CREATE INDEX Index14fe98b9eff542b586e68f8a3a2a2fe9 on Перелом (Лапа_m0); 

 ALTER TABLE Human2 ADD CONSTRAINT FK028762dce4c1420c97f941875ea77090 FOREIGN KEY (TodayHome_m0) REFERENCES Country2; 
CREATE INDEX Indexb4e8936b693b494d8b637860b439f14a on Human2 (TodayHome_m0); 

 ALTER TABLE Human2 ADD CONSTRAINT FK14f9de0794f64f8a899e7887f07c4518 FOREIGN KEY (TodayHome_m1) REFERENCES Territory2; 
CREATE INDEX Indexdfa20491363341a7a03ed08bc4f83575 on Human2 (TodayHome_m1); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FKccf9f960e9254be897a54f5b6781cb30 FOREIGN KEY (ЗначениеКритер) REFERENCES ЗначениеКритер; 
CREATE INDEX Indexf51611f869b24a3384a5d150ebf6f201 on ОценкаЭксперта (ЗначениеКритер); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FKe50b4cb02cbe47d68ae7b92304fa6896 FOREIGN KEY (Эксперт_m0) REFERENCES Пользователь; 
CREATE INDEX Indexb0acdd33fb8d4c8796fddc6c21903579 on ОценкаЭксперта (Эксперт_m0); 

 ALTER TABLE ОценкаЭксперта ADD CONSTRAINT FK1ff34a088d334948988757505549f8b8 FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index4740497f636246beb9519ae76b5c6e00 on ОценкаЭксперта (Идея_m0); 

 ALTER TABLE Медведь ADD CONSTRAINT FK8e2b66fd98c9443e9cd5c37277c98cd5 FOREIGN KEY (Мама) REFERENCES Медведь; 
CREATE INDEX Indexdbaf9cf7365d44d185f0ebf18363d4ba on Медведь (Мама); 

 ALTER TABLE Медведь ADD CONSTRAINT FKff57f0ea33f44191aab0943b73a5eecb FOREIGN KEY (Папа) REFERENCES Медведь; 
CREATE INDEX Indexa9b0c9ff26674071aa96cd400f9a802c on Медведь (Папа); 

 ALTER TABLE Медведь ADD CONSTRAINT FK1b2dd7bbc98e4e298c51c2541833f570 FOREIGN KEY (ЛесОбитания) REFERENCES Лес; 
CREATE INDEX Index535422e47af74040b9f2c5ebd6de8e7b on Медведь (ЛесОбитания); 

 ALTER TABLE InformationTestClass6 ADD CONSTRAINT FKd5ce53b063584aac88df14630ae4f88f FOREIGN KEY (ExampleOfClassWithCaptions) REFERENCES ClassWithCaptions; 
CREATE INDEX Index909e4941995c4a1f8c6f963284b826f4 on InformationTestClass6 (ExampleOfClassWithCaptions); 

 ALTER TABLE Берлога ADD CONSTRAINT FK124be5d3f3964ef4980a828426aaaa43 FOREIGN KEY (ЛесРасположения) REFERENCES Лес; 
CREATE INDEX Indexa6b05a4137a14a66830138e893879e79 on Берлога (ЛесРасположения); 

 ALTER TABLE Берлога ADD CONSTRAINT FK7d951171e8a046fd9a6081a8b1602f2c FOREIGN KEY (Медведь) REFERENCES Медведь; 
CREATE INDEX Indexfbe54a1723364c22bbc72bf1a3397a8b on Берлога (Медведь); 

 ALTER TABLE Apparatus2 ADD CONSTRAINT FK7cfe5e28cc184a2db9fbcc3d836db5d4 FOREIGN KEY (Maker_m0) REFERENCES Country2; 
CREATE INDEX Index59754990a24f4acc94cf9aba5cb92fb1 on Apparatus2 (Maker_m0); 

 ALTER TABLE Apparatus2 ADD CONSTRAINT FK1e3b842f97094f7fbfbd76d03a67d940 FOREIGN KEY (Exporter_m0) REFERENCES Country2; 
CREATE INDEX Index13a57dcdaaa44f78ac195e6c7ec66de8 on Apparatus2 (Exporter_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK952cbe4dc75348bcab00827f996d8711 FOREIGN KEY (MainIngridient_m0) REFERENCES Cabbage2; 
CREATE INDEX Indexe88b987344d24499957558a498b74a10 on Dish2 (MainIngridient_m0); 

 ALTER TABLE Dish2 ADD CONSTRAINT FK01a9f55139c1467d8d0988eeba3f0894 FOREIGN KEY (MainIngridient_m1) REFERENCES Plant2; 
CREATE INDEX Index34fa7c83d82e43ffad1e2a1a8b793457 on Dish2 (MainIngridient_m1); 

 ALTER TABLE КритерийОценки ADD CONSTRAINT FKfdeb7cd2447d41e6b0a62d6e96e9ea41 FOREIGN KEY (Конкурс_m0) REFERENCES Конкурс; 
CREATE INDEX Index5f9f6569f09f41c682dbded4a3ae6df4 on КритерийОценки (Конкурс_m0); 

 ALTER TABLE InformationTestClass4 ADD CONSTRAINT FK001a542139c343a8846b5b555a3bb94b FOREIGN KEY (MasterOfInformationTestClass3) REFERENCES InformationTestClass3; 
CREATE INDEX Index8438394aca2e4f0698bac3424b0a0611 on InformationTestClass4 (MasterOfInformationTestClass3); 

 ALTER TABLE ClassWithCaptions ADD CONSTRAINT FK63c00f5fc4d140dea36317748a0f0a90 FOREIGN KEY (InformationTestClass4) REFERENCES InformationTestClass4; 
CREATE INDEX Indexae41ccf3c2104825ad3c072fd0629323 on ClassWithCaptions (InformationTestClass4); 

 ALTER TABLE Adress2 ADD CONSTRAINT FKca2dfa93e9fc4e49be9363990ef1940a FOREIGN KEY (Country_m0) REFERENCES Country2; 
CREATE INDEX Indexc064eca44b614105bb4e2e4dfec8fbb5 on Adress2 (Country_m0); 

 ALTER TABLE Конкурс ADD CONSTRAINT FK7f656b735f3846ff932891534236a0e6 FOREIGN KEY (Организатор_m0) REFERENCES Пользователь; 
CREATE INDEX Index06bf18156fb241459f168a88df6582a3 on Конкурс (Организатор_m0); 

 ALTER TABLE TypeUsageProviderTestClassChil ADD CONSTRAINT FK8380985c3743417194d77b8c0698d35c FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Indexbff25b51e3fc4508bd6f55294443c984 on TypeUsageProviderTestClassChil (DataObjectForTest_m0); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FKa273f7f3b10040518323fa25146f0e9a FOREIGN KEY (Cabbage1) REFERENCES Cabbage2; 
CREATE INDEX Index49aedf36fd1943369492a176b2503dc2 on CabbageSalad (Cabbage1); 

 ALTER TABLE CabbageSalad ADD CONSTRAINT FKf2aee49004f243d596cda3a32a56448d FOREIGN KEY (Cabbage2) REFERENCES Cabbage2; 
CREATE INDEX Index6caf307f81234aef991100e7afd679ac on CabbageSalad (Cabbage2); 

 ALTER TABLE CabbagePart2 ADD CONSTRAINT FKccf1801ed82f47e1a0f7641720ea3df4 FOREIGN KEY (Cabbage) REFERENCES Cabbage2; 
CREATE INDEX Indexb790b45066f343e8af6808bb5bce4600 on CabbagePart2 (Cabbage); 

 ALTER TABLE cla ADD CONSTRAINT FK362143e7fd9e482f9068d68cdee02dbb FOREIGN KEY (parent) REFERENCES clb; 
CREATE INDEX Index43179122ba914a0b97177b25e2430148 on cla (parent); 

 ALTER TABLE Place2 ADD CONSTRAINT FK68fc3acf000646bbb0616b4f60b666f0 FOREIGN KEY (TodayTerritory_m0) REFERENCES Country2; 
CREATE INDEX Indexbbb318ec9d9844dcbf2c72d66b9dd56d on Place2 (TodayTerritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FKd2b4a620b6b84f1e8bf189c1cc331b9a FOREIGN KEY (TodayTerritory_m1) REFERENCES Territory2; 
CREATE INDEX Indexd904f3ebdb8d43efaa1dc6154bef3127 on Place2 (TodayTerritory_m1); 

 ALTER TABLE Place2 ADD CONSTRAINT FKcfccb39376b94ae0b77cfa1353f50793 FOREIGN KEY (TomorrowTeritory_m0) REFERENCES Country2; 
CREATE INDEX Index3f94f08502934cf194b06654990ecc0b on Place2 (TomorrowTeritory_m0); 

 ALTER TABLE Place2 ADD CONSTRAINT FKcebc408e34b64a8c8458381353c94a23 FOREIGN KEY (TomorrowTeritory_m1) REFERENCES Territory2; 
CREATE INDEX Index1b909a71cf5d4b63a36045291d05f992 on Place2 (TomorrowTeritory_m1); 

 ALTER TABLE TypeUsageProviderTestClass ADD CONSTRAINT FKa9ed6530fd5642218ab04c89586a20c2 FOREIGN KEY (DataObjectForTest_m0) REFERENCES DataObjectForTest; 
CREATE INDEX Index1bb2d357373c4cea9b11ed2dd1a134bf on TypeUsageProviderTestClass (DataObjectForTest_m0); 

 ALTER TABLE AuditAgregatorObject ADD CONSTRAINT FK56b86c84f7c64aebbc3a05027c9eac24 FOREIGN KEY (MasterObject) REFERENCES AuditMasterObject; 
CREATE INDEX Index733525f2f969417e825133fd110813a9 on AuditAgregatorObject (MasterObject); 

 ALTER TABLE Region ADD CONSTRAINT FK462ba7442a234346b7d5b94b372e1989 FOREIGN KEY (Country2_m0) REFERENCES Country2; 
CREATE INDEX Index8c9b49f5f9504420b21eef2302620cb8 on Region (Country2_m0); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FK54be009e97724ca98dca430c0f76347c FOREIGN KEY (Владелец_m0) REFERENCES Пользователь; 
CREATE INDEX Index193a8120d32c4e49923bd2df3af4d406 on ФайлИдеи (Владелец_m0); 

 ALTER TABLE ФайлИдеи ADD CONSTRAINT FK62700922622542c3a68736a104a4b238 FOREIGN KEY (Идея_m0) REFERENCES Идея; 
CREATE INDEX Index59e75273068d4b77acf94dde7e892c9a on ФайлИдеи (Идея_m0); 

 ALTER TABLE InformationTestClass3 ADD CONSTRAINT FKbc13608ded5644d98ff0c5f7a2271f64 FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Indexf3eea1289f46473c9aacbfb7e376bfec on InformationTestClass3 (InformationTestClass2); 

 ALTER TABLE Soup2 ADD CONSTRAINT FKc2f45d0c593f4092ba586559780322c6 FOREIGN KEY (CabbageType) REFERENCES Cabbage2; 
CREATE INDEX Indexca80ca9a66c64b9387c210fa42b15fc9 on Soup2 (CabbageType); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK2e16b4d5c86a4407ad00a3de03328790 FOREIGN KEY (InformationTestClass2) REFERENCES InformationTestClass2; 
CREATE INDEX Index6436ec3385134051ad1110aac8a5eed1 on MasterClass (InformationTestClass2); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK4d5f50e33d5941348469aeba24ad3dc4 FOREIGN KEY (InformationTestClass3_m0) REFERENCES InformationTestClass3; 
CREATE INDEX Index1afe4c788146473fba1b3be3419e2472 on MasterClass (InformationTestClass3_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FKcca21b969a2d41a7aa7d2c271e30d656 FOREIGN KEY (InformationTestClass_m0) REFERENCES InformationTestClass; 
CREATE INDEX Index5ee653e3964b4db491199664e17379a9 on MasterClass (InformationTestClass_m0); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK76abcb48a83842dc8c4b693fb5645d42 FOREIGN KEY (InformationTestClass_m1) REFERENCES InformationTestClassChild; 
CREATE INDEX Indexb7c143e2ea2b4218b41d673f6be67c3a on MasterClass (InformationTestClass_m1); 

 ALTER TABLE STORMWEBSEARCH ADD CONSTRAINT FK03c3cd9556cf46e2bd71ea48f1908df7 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERDETAIL ADD CONSTRAINT FK6fe597e3f2bf490fb013e02b185ad8f5 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERLOOKUP ADD CONSTRAINT FK22428b99174d40d7927a49c701a8f4be FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMAuEntity ADD CONSTRAINT FKa72aefa6e860461aac5a88a71009329a FOREIGN KEY (ObjectType_m0) REFERENCES STORMAuObjType; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FKb4e736a01bd34da5a46948046a952d26 FOREIGN KEY (MainChange_m0) REFERENCES STORMAuField; 

 ALTER TABLE STORMAuField ADD CONSTRAINT FK423b337fea5c4f309a1bb06c34f8181b FOREIGN KEY (AuditEntity_m0) REFERENCES STORMAuEntity; 
