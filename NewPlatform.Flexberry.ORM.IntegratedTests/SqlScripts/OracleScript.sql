



CREATE TABLE "InformationTestClass"
(

	"primaryKey" RAW(16) NOT NULL,

	"PublicStringProperty" NVARCHAR2(255) NULL,

	"StringPropertyForInfTestClass" NVARCHAR2(255) NULL,

	"IntPropertyForInfTestClass" NUMBER(10) NULL,

	"BoolPropertyForInfTestClass" NUMBER(1) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ТипЛапы"
(

	"primaryKey" RAW(16) NOT NULL,

	"Название" NVARCHAR2(255) NULL,

	"Актуально" NUMBER(1) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Идея"
(

	"primaryKey" RAW(16) NOT NULL,

	"Заголовок" NVARCHAR2(255) NULL,

	"Описание" NVARCHAR2(255) NULL,

	"СуммаБаллов" FLOAT(126) NULL,

	"Конкурс_m0" RAW(16) NOT NULL,

	"Автор_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "DateField"
(

	"primaryKey" RAW(16) NOT NULL,

	"Date" DATE NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "AuditMasterObject"
(

	"primaryKey" RAW(16) NOT NULL,

	"Login" NVARCHAR2(255) NULL,

	"Name" NVARCHAR2(255) NULL,

	"Surname" NVARCHAR2(255) NULL,

	"MasterObject" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "AuditClassWithDisabledAudit"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	"CreateTime" DATE NULL,

	"Creator" NVARCHAR2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "InformationTestClass2"
(

	"primaryKey" RAW(16) NOT NULL,

	"StringPropertyForInfTestClass2" NVARCHAR2(255) NULL,

	"InformationTestClass_m0" RAW(16) NULL,

	"InformationTestClass_m1" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Country2"
(

	"primaryKey" RAW(16) NOT NULL,

	"CountryName" NVARCHAR2(255) NULL,

	"XCoordinate" NUMBER(10) NULL,

	"YCoordinate" NUMBER(10) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "TestClassA"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	"Value" NUMBER(10) NULL,

	"Мастер_m0" RAW(16) NULL,

	"Мастер_m1" RAW(16) NULL,

	"Мастер_m2" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "clb"
(

	"primaryKey" RAW(16) NOT NULL,

	"ref2" RAW(16) NULL,

	"ref1" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "AggregatorUpdateObjectTest"
(

	"primaryKey" RAW(16) NOT NULL,

	"AggregatorName" NVARCHAR2(255) NULL,

	"Detail" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Лапа"
(

	"primaryKey" RAW(16) NOT NULL,

	"Цвет" NVARCHAR2(255) NULL,

	"Размер" NUMBER(10) NULL,

	"ДатаРождения" DATE NULL,

	"БылиЛиПереломы" NUMBER(1) NULL,

	"Сторона" NVARCHAR2(11) NULL,

	"Номер" NUMBER(10) NULL,

	"РазмерDouble" FLOAT(126) NULL,

	"РазмерFloat" FLOAT(53) NULL,

	"РазмерNullableInt" NUMBER(10) NULL,

	"РазмерDecimal" NUMBER(38) NULL,

	"РазмерNullableDecimal" NUMBER(38) NULL,

	"РазмерNullableChar" NUMBER(3) NULL,

	"ТипЛапы_m0" RAW(16) NULL,

	"Кошка_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ИФХозДоговора"
(

	"primaryKey" RAW(16) NOT NULL,

	"НомерИФХозДогов" NUMBER(10) NULL,

	"ИсточникФинан" RAW(16) NOT NULL,

	"ХозДоговор_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Котенок"
(

	"primaryKey" RAW(16) NOT NULL,

	"КличкаКотенка" NVARCHAR2(255) NULL,

	"Глупость" NUMBER(10) NULL,

	"Кошка" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ЗначениеКритер"
(

	"primaryKey" RAW(16) NOT NULL,

	"Значение" NVARCHAR2(255) NULL,

	"СредОценкаЭксп" FLOAT(126) NULL,

	"Критерий_m0" RAW(16) NOT NULL,

	"Идея_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ДокККонкурсу"
(

	"primaryKey" RAW(16) NOT NULL,

	"Файл" NCLOB NULL,

	"Конкурс_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Личность"
(

	"primaryKey" RAW(16) NOT NULL,

	"ФИО" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Лес"
(

	"primaryKey" RAW(16) NOT NULL,

	"Название" NVARCHAR2(255) NULL,

	"Площадь" NUMBER(10) NULL,

	"Заповедник" NUMBER(1) NULL,

	"ДатаПослОсмотр" DATE NULL,

	"Страна" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "FullTypesDetail1"
(

	"primaryKey" RAW(16) NOT NULL,

	"PoleInt" NUMBER(10) NULL,

	"PoleDateTime" DATE NULL,

	"PoleString" NVARCHAR2(255) NULL,

	"PoleFloat" FLOAT(53) NULL,

	"PoleDouble" FLOAT(126) NULL,

	"PoleDecimal" NUMBER(38) NULL,

	"PoleBool" NUMBER(1) NULL,

	"PoleNullableInt" NUMBER(10) NULL,

	"PoleNullableDecimal" NUMBER(38) NULL,

	"PoleNullableDateTime" DATE NULL,

	"PoleNullInt" NUMBER(10) NULL,

	"PoleNullDateTime" DATE NULL,

	"PoleNullFloat" FLOAT(53) NULL,

	"PoleNullDouble" FLOAT(126) NULL,

	"PoleNullDecimal" NUMBER(38) NULL,

	"PoleGuid" RAW(16) NULL,

	"PoleNullGuid" RAW(16) NULL,

	"PoleEnum" NVARCHAR2(15) NULL,

	"PoleChar" NUMBER(3) NULL,

	"PoleNullChar" NUMBER(3) NULL,

	"FullTypesMainAgregator_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "DetailUpdateObjectTest"
(

	"primaryKey" RAW(16) NOT NULL,

	"DetailName" NVARCHAR2(255) NULL,

	"Master" RAW(16) NULL,

	"AggregatorUpdateObjectTest" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "SomeMasterClass"
(

	"primaryKey" RAW(16) NOT NULL,

	"FieldA" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Порода"
(

	"primaryKey" RAW(16) NOT NULL,

	"Название" NVARCHAR2(255) NULL,

	"Ключ" RAW(16) NULL,

	"ТипПороды" RAW(16) NULL,

	"Иерархия" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "МастерМ"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	"Value" NUMBER(10) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Блоха"
(

	"primaryKey" RAW(16) NOT NULL,

	"Кличка" NVARCHAR2(255) NULL,

	"МедведьОбитания" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Страна"
(

	"primaryKey" RAW(16) NOT NULL,

	"Название" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "TypeNameUsageProviderTestClass"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "DetailClass"
(

	"primaryKey" RAW(16) NOT NULL,

	"Detailproperty" NVARCHAR2(255) NULL,

	"MasterClass" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Кредит"
(

	"primaryKey" RAW(16) NOT NULL,

	"ДатаВыдачи" DATE NULL,

	"СуммаКредита" FLOAT(126) NULL,

	"СрокКредита" NUMBER(10) NULL,

	"ВидКредита" NVARCHAR2(15) NULL,

	"Клиент" RAW(16) NULL,

	"ИнспекторПоКред" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "MasterUpdateObjectTest"
(

	"primaryKey" RAW(16) NOT NULL,

	"MasterName" NVARCHAR2(255) NULL,

	"Detail" RAW(16) NULL,

	"AggregatorUpdateObjectTest" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ТипПороды"
(

	"primaryKey" RAW(16) NOT NULL,

	"Название" NVARCHAR2(255) NULL,

	"ДатаРегистрации" DATE NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "FullTypesMainAgregator"
(

	"primaryKey" RAW(16) NOT NULL,

	"PoleInt" NUMBER(10) NULL,

	"PoleDateTime" DATE NULL,

	"PoleString" NVARCHAR2(255) NULL,

	"PoleFloat" FLOAT(53) NULL,

	"PoleDouble" FLOAT(126) NULL,

	"PoleDecimal" NUMBER(38) NULL,

	"PoleBool" NUMBER(1) NULL,

	"PoleNullableInt" NUMBER(10) NULL,

	"PoleNullableDecimal" NUMBER(38) NULL,

	"PoleNullableDateTime" DATE NULL,

	"PoleNullInt" NUMBER(10) NULL,

	"PoleNullDateTime" DATE NULL,

	"PoleNullFloat" FLOAT(53) NULL,

	"PoleNullDouble" FLOAT(126) NULL,

	"PoleNullDecimal" NUMBER(38) NULL,

	"PoleGuid" RAW(16) NULL,

	"PoleNullGuid" RAW(16) NULL,

	"PoleEnum" NVARCHAR2(15) NULL,

	"PoleChar" NUMBER(3) NULL,

	"PoleNullChar" NUMBER(3) NULL,

	"FullTypesMaster1_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "CombinedTypesUsageProviderTest"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	"DataObjectForTest_m0" RAW(16) NULL,

	"TypeUsageProviderTestClass" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "AuditClassWithoutSettings"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Кошка"
(

	"primaryKey" RAW(16) NOT NULL,

	"Кличка" NVARCHAR2(255) NULL,

	"ДатаРождения" DATE NOT NULL,

	"Тип" NVARCHAR2(8) NOT NULL,

	"ПородаСтрокой" NVARCHAR2(255) NULL,

	"Агрессивная" NUMBER(1) NULL,

	"КолвоУсовСлева" NUMBER(10) NULL,

	"КолвоУсовСправа" NUMBER(10) NULL,

	"Ключ" RAW(16) NULL,

	"Порода" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "SomeDetailClass"
(

	"primaryKey" RAW(16) NOT NULL,

	"FieldB" NVARCHAR2(255) NULL,

	"ClassA" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "FullTypesDetail2"
(

	"primaryKey" RAW(16) NOT NULL,

	"PoleInt" NUMBER(10) NULL,

	"PoleDateTime" DATE NULL,

	"PoleString" NVARCHAR2(255) NULL,

	"PoleFloat" FLOAT(53) NULL,

	"PoleDouble" FLOAT(126) NULL,

	"PoleDecimal" NUMBER(38) NULL,

	"PoleBool" NUMBER(1) NULL,

	"PoleNullableInt" NUMBER(10) NULL,

	"PoleNullableDecimal" NUMBER(38) NULL,

	"PoleNullableDateTime" DATE NULL,

	"PoleNullInt" NUMBER(10) NULL,

	"PoleNullDateTime" DATE NULL,

	"PoleNullFloat" FLOAT(53) NULL,

	"PoleNullDouble" FLOAT(126) NULL,

	"PoleNullDecimal" NUMBER(38) NULL,

	"PoleGuid" RAW(16) NULL,

	"PoleNullGuid" RAW(16) NULL,

	"PoleEnum" NVARCHAR2(15) NULL,

	"PoleChar" NUMBER(3) NULL,

	"PoleNullChar" NUMBER(3) NULL,

	"FullTypesMainAgregator" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Territory2"
(

	"primaryKey" RAW(16) NOT NULL,

	"XCoordinate" NUMBER(10) NULL,

	"YCoordinate" NUMBER(10) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Salad2"
(

	"primaryKey" RAW(16) NOT NULL,

	"SaladName" NVARCHAR2(255) NULL,

	"Ingridient2_m0" RAW(16) NULL,

	"Ingridient2_m1" RAW(16) NULL,

	"Ingridient1_m0" RAW(16) NULL,

	"Ingridient1_m1" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ForKeyStorageTest"
(

	"StorageForKey" NVARCHAR2(255) NOT NULL,

	 PRIMARY KEY ("StorageForKey")
) ;


CREATE TABLE "Выплаты"
(

	"primaryKey" RAW(16) NOT NULL,

	"ДатаВыплаты" DATE NULL,

	"СуммаВыплаты" FLOAT(126) NULL,

	"Кредит1" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ИсточникФинанс"
(

	"primaryKey" RAW(16) NOT NULL,

	"НомИсточникаФин" NUMBER(10) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "УчастникХозДог"
(

	"primaryKey" RAW(16) NOT NULL,

	"НомУчастнХозДог" NUMBER(10) NULL,

	"Статус" NVARCHAR2(12) NULL,

	"Личность_m0" RAW(16) NOT NULL,

	"ХозДоговор_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Перелом"
(

	"primaryKey" RAW(16) NOT NULL,

	"Дата" DATE NULL,

	"Тип" NVARCHAR2(8) NULL,

	"Лапа_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Human2"
(

	"primaryKey" RAW(16) NOT NULL,

	"HumanName" NVARCHAR2(255) NULL,

	"TodayHome_m0" RAW(16) NULL,

	"TodayHome_m1" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ОценкаЭксперта"
(

	"primaryKey" RAW(16) NOT NULL,

	"ЗначениеОценки" FLOAT(126) NULL,

	"Комментарий" NVARCHAR2(255) NULL,

	"ЗначениеКритер" RAW(16) NOT NULL,

	"Эксперт_m0" RAW(16) NOT NULL,

	"Идея_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Медведь"
(

	"primaryKey" RAW(16) NOT NULL,

	"ПорядковыйНомер" NUMBER(10) NULL,

	"Вес" NUMBER(10) NULL,

	"ЦветГлаз" NVARCHAR2(255) NULL,

	"Пол" NVARCHAR2(7) NULL,

	"ДатаРождения" DATE NULL,

	"Мама" RAW(16) NULL,

	"Папа" RAW(16) NULL,

	"ЛесОбитания" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "InformationTestClass6"
(

	"primaryKey" RAW(16) NOT NULL,

	"StringPropForInfTestClass6" NVARCHAR2(255) NULL,

	"ExampleOfClassWithCaptions" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Берлога"
(

	"primaryKey" RAW(16) NOT NULL,

	"Наименование" NVARCHAR2(255) NULL,

	"Комфортность" NUMBER(10) NULL,

	"Заброшена" NUMBER(1) NULL,

	"ЛесРасположения" RAW(16) NULL,

	"Медведь" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "AuditMasterMasterObject"
(

	"primaryKey" RAW(16) NOT NULL,

	"Login" NVARCHAR2(255) NULL,

	"Name" NVARCHAR2(255) NULL,

	"Surname" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Клиент"
(

	"primaryKey" RAW(16) NOT NULL,

	"ФИО" NVARCHAR2(255) NULL,

	"Прописка" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Apparatus2"
(

	"primaryKey" RAW(16) NOT NULL,

	"ApparatusName" NVARCHAR2(255) NULL,

	"Maker_m0" RAW(16) NULL,

	"Exporter_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "НаследникМ2"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	"Value" NUMBER(10) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Пользователь"
(

	"primaryKey" RAW(16) NOT NULL,

	"Логин" NVARCHAR2(255) NULL,

	"ФИО" NVARCHAR2(255) NULL,

	"EMail" NVARCHAR2(255) NULL,

	"ДатаРегистрации" DATE NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "НаследникМ1"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	"Value" NUMBER(10) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "AuditClassWithSettings"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	"CreateTime" DATE NULL,

	"Creator" NVARCHAR2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Dish2"
(

	"primaryKey" RAW(16) NOT NULL,

	"DishName" NVARCHAR2(255) NULL,

	"MainIngridient_m0" RAW(16) NULL,

	"MainIngridient_m1" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Cabbage2"
(

	"primaryKey" RAW(16) NOT NULL,

	"Type" NVARCHAR2(255) NULL,

	"Name" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "DataObjectForTest"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	"Height" NUMBER(10) NULL,

	"BirthDate" DATE NULL,

	"Gender" NUMBER(1) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "FullTypesMaster1"
(

	"primaryKey" RAW(16) NOT NULL,

	"PoleInt" NUMBER(10) NULL,

	"PoleUInt" NUMBER(10) NULL,

	"PoleDateTime" DATE NULL,

	"PoleString" NVARCHAR2(255) NULL,

	"PoleFloat" FLOAT(53) NULL,

	"PoleDouble" FLOAT(126) NULL,

	"PoleDecimal" NUMBER(38) NULL,

	"PoleBool" NUMBER(1) NULL,

	"PoleNullableInt" NUMBER(10) NULL,

	"PoleNullableDecimal" NUMBER(38) NULL,

	"PoleNullableDateTime" DATE NULL,

	"PoleNullInt" NUMBER(10) NULL,

	"PoleNullDateTime" DATE NULL,

	"PoleNullFloat" FLOAT(53) NULL,

	"PoleNullDouble" FLOAT(126) NULL,

	"PoleNullDecimal" NUMBER(38) NULL,

	"PoleGuid" RAW(16) NULL,

	"PoleNullGuid" RAW(16) NULL,

	"PoleEnum" NVARCHAR2(15) NULL,

	"PoleChar" NUMBER(3) NULL,

	"PoleNullChar" NUMBER(3) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Plant2"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "КритерийОценки"
(

	"primaryKey" RAW(16) NOT NULL,

	"ПорядковыйНомер" NUMBER(10) NULL,

	"Описание" NVARCHAR2(255) NULL,

	"Вес" FLOAT(126) NULL,

	"Обязательный" NUMBER(1) NULL,

	"Конкурс_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "InformationTestClass4"
(

	"primaryKey" RAW(16) NOT NULL,

	"StringPropForInfTestClass4" NVARCHAR2(255) NULL,

	"MasterOfInformationTestClass3" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "InformationTestClassChild"
(

	"primaryKey" RAW(16) NOT NULL,

	"PublicStringProperty" NVARCHAR2(255) NULL,

	"StringPropertyForInfTestClass" NVARCHAR2(255) NULL,

	"IntPropertyForInfTestClass" NUMBER(10) NULL,

	"BoolPropertyForInfTestClass" NUMBER(1) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ClassWithCaptions"
(

	"primaryKey" RAW(16) NOT NULL,

	"InformationTestClass4" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Adress2"
(

	"primaryKey" RAW(16) NOT NULL,

	"HomeNumber" NUMBER(10) NULL,

	"Country_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ИнспПоКредиту"
(

	"primaryKey" RAW(16) NOT NULL,

	"ФИО" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Конкурс"
(

	"primaryKey" RAW(16) NOT NULL,

	"Название" NVARCHAR2(255) NULL,

	"Описание" NVARCHAR2(255) NULL,

	"ДатаНачала" DATE NULL,

	"ДатаОкончания" DATE NULL,

	"НачалоОценки" DATE NULL,

	"ОкончаниеОценки" DATE NULL,

	"Состоятие" NVARCHAR2(16) NULL,

	"Организатор_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "TypeUsageProviderTestClassChil"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	"DataObjectForTest_m0" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "CabbageSalad"
(

	"primaryKey" RAW(16) NOT NULL,

	"CabbageSaladName" NVARCHAR2(255) NULL,

	"Cabbage1" RAW(16) NULL,

	"Cabbage2" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "NullFileField"
(

	"primaryKey" RAW(16) NOT NULL,

	"FileField" NCLOB NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "CabbagePart2"
(

	"primaryKey" RAW(16) NOT NULL,

	"PartName" NVARCHAR2(255) NULL,

	"Cabbage" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "cla"
(

	"primaryKey" RAW(16) NOT NULL,

	"info" NVARCHAR2(255) NULL,

	"parent" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Place2"
(

	"primaryKey" RAW(16) NOT NULL,

	"PlaceName" NVARCHAR2(255) NULL,

	"TodayTerritory_m0" RAW(16) NULL,

	"TodayTerritory_m1" RAW(16) NULL,

	"TomorrowTeritory_m0" RAW(16) NULL,

	"TomorrowTeritory_m1" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "SimpleDataObject"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	"Age" NUMBER(10) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ХозДоговор"
(

	"primaryKey" RAW(16) NOT NULL,

	"НомХозДоговора" NUMBER(10) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "DataObjectWithKeyGuid"
(

	"primaryKey" RAW(16) NOT NULL,

	"LinkToMaster1" RAW(16) NULL,

	"LinkToMaster2" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "StoredClass"
(

	"primaryKey" RAW(16) NOT NULL,

	"StoredProperty" NVARCHAR2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "TypeUsageProviderTestClass"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" NVARCHAR2(255) NULL,

	"DataObjectForTest_m0" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "AuditAgregatorObject"
(

	"primaryKey" RAW(16) NOT NULL,

	"Login" NVARCHAR2(255) NULL,

	"Name" NVARCHAR2(255) NULL,

	"Surname" NVARCHAR2(255) NULL,

	"MasterObject" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Region"
(

	"primaryKey" RAW(16) NOT NULL,

	"RegionName" NVARCHAR2(255) NULL,

	"Country2_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ФайлИдеи"
(

	"primaryKey" RAW(16) NOT NULL,

	"Файл" NCLOB NULL,

	"Владелец_m0" RAW(16) NOT NULL,

	"Идея_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "InformationTestClass3"
(

	"primaryKey" RAW(16) NOT NULL,

	"StringPropForInfTestClass3" NVARCHAR2(255) NULL,

	"InformationTestClass2" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "Soup2"
(

	"primaryKey" RAW(16) NOT NULL,

	"SoupName" NVARCHAR2(255) NULL,

	"CabbageType" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "MasterClass"
(

	"primaryKey" RAW(16) NOT NULL,

	"StringMasterProperty" NVARCHAR2(255) NULL,

	"InformationTestClass2" RAW(16) NULL,

	"InformationTestClass3_m0" RAW(16) NULL,

	"InformationTestClass_m0" RAW(16) NULL,

	"InformationTestClass_m1" RAW(16) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMNETLOCKDATA"
(

	"LockKey" NVARCHAR2(300) NOT NULL,

	"UserName" NVARCHAR2(300) NOT NULL,

	"LockDate" DATE NULL,

	 PRIMARY KEY ("LockKey")
) ;


CREATE TABLE "STORMSETTINGS"
(

	"primaryKey" RAW(16) NOT NULL,

	"Module" nvarchar2(1000) NULL,

	"Name" nvarchar2(255) NULL,

	"Value" CLOB NULL,

	"User" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMAdvLimit"
(

	"primaryKey" RAW(16) NOT NULL,

	"User" nvarchar2(255) NULL,

	"Published" NUMBER(1) NULL,

	"Module" nvarchar2(255) NULL,

	"Name" nvarchar2(255) NULL,

	"Value" CLOB NULL,

	"HotKeyData" NUMBER(10) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMFILTERSETTING"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" nvarchar2(255) NOT NULL,

	"DataObjectView" nvarchar2(255) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMWEBSEARCH"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" nvarchar2(255) NOT NULL,

	"Order" NUMBER(10) NOT NULL,

	"PresentView" nvarchar2(255) NOT NULL,

	"DetailedView" nvarchar2(255) NOT NULL,

	"FilterSetting_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMFILTERDETAIL"
(

	"primaryKey" RAW(16) NOT NULL,

	"Caption" nvarchar2(255) NOT NULL,

	"DataObjectView" nvarchar2(255) NOT NULL,

	"ConnectMasterProp" nvarchar2(255) NOT NULL,

	"OwnerConnectProp" nvarchar2(255) NULL,

	"FilterSetting_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMFILTERLOOKUP"
(

	"primaryKey" RAW(16) NOT NULL,

	"DataObjectType" nvarchar2(255) NOT NULL,

	"Container" nvarchar2(255) NULL,

	"ContainerTag" nvarchar2(255) NULL,

	"FieldsToView" nvarchar2(255) NULL,

	"FilterSetting_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "UserSetting"
(

	"primaryKey" RAW(16) NOT NULL,

	"AppName" nvarchar2(256) NULL,

	"UserName" nvarchar2(512) NULL,

	"UserGuid" RAW(16) NULL,

	"ModuleName" nvarchar2(1024) NULL,

	"ModuleGuid" RAW(16) NULL,

	"SettName" nvarchar2(256) NULL,

	"SettGuid" RAW(16) NULL,

	"SettLastAccessTime" DATE NULL,

	"StrVal" nvarchar2(256) NULL,

	"TxtVal" CLOB NULL,

	"IntVal" NUMBER(10) NULL,

	"BoolVal" NUMBER(1) NULL,

	"GuidVal" RAW(16) NULL,

	"DecimalVal" NUMBER(20,10) NULL,

	"DateTimeVal" DATE NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "ApplicationLog"
(

	"primaryKey" RAW(16) NOT NULL,

	"Category" nvarchar2(64) NULL,

	"EventId" NUMBER(10) NULL,

	"Priority" NUMBER(10) NULL,

	"Severity" nvarchar2(32) NULL,

	"Title" nvarchar2(256) NULL,

	"Timestamp" DATE NULL,

	"MachineName" nvarchar2(32) NULL,

	"AppDomainName" nvarchar2(512) NULL,

	"ProcessId" nvarchar2(256) NULL,

	"ProcessName" nvarchar2(512) NULL,

	"ThreadName" nvarchar2(512) NULL,

	"Win32ThreadId" nvarchar2(128) NULL,

	"Message" nvarchar2(2000) NULL,

	"FormattedMessage" nvarchar2(2000) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMAG"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" nvarchar2(80) NOT NULL,

	"Login" nvarchar2(50) NULL,

	"Pwd" nvarchar2(50) NULL,

	"IsUser" NUMBER(1) NOT NULL,

	"IsGroup" NUMBER(1) NOT NULL,

	"IsRole" NUMBER(1) NOT NULL,

	"ConnString" nvarchar2(255) NULL,

	"Enabled" NUMBER(1) NULL,

	"Email" nvarchar2(80) NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMLG"
(

	"primaryKey" RAW(16) NOT NULL,

	"Group_m0" RAW(16) NOT NULL,

	"User_m0" RAW(16) NOT NULL,

	"CreateTime" DATE NULL,

	"Creator" nvarchar2(255) NULL,

	"EditTime" DATE NULL,

	"Editor" nvarchar2(255) NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMAuObjType"
(

	"primaryKey" RAW(16) NOT NULL,

	"Name" nvarchar2(255) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMAuEntity"
(

	"primaryKey" RAW(16) NOT NULL,

	"ObjectPrimaryKey" nvarchar2(38) NOT NULL,

	"OperationTime" DATE NOT NULL,

	"OperationType" nvarchar2(100) NOT NULL,

	"ExecutionResult" nvarchar2(12) NOT NULL,

	"Source" nvarchar2(255) NOT NULL,

	"SerializedField" nvarchar2(2000) NULL,

	"User_m0" RAW(16) NOT NULL,

	"ObjectType_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;


CREATE TABLE "STORMAuField"
(

	"primaryKey" RAW(16) NOT NULL,

	"Field" nvarchar2(100) NOT NULL,

	"OldValue" nvarchar2(2000) NULL,

	"NewValue" nvarchar2(2000) NULL,

	"MainChange_m0" RAW(16) NULL,

	"AuditEntity_m0" RAW(16) NOT NULL,

	 PRIMARY KEY ("primaryKey")
) ;



ALTER TABLE "Идея"
	ADD CONSTRAINT "Идея_FКонкурс_0" FOREIGN KEY ("Конкурс_m0") REFERENCES "Конкурс" ("primaryKey");

CREATE INDEX "Идея_IКонкурс_m0" on "Идея" ("Конкурс_m0");

ALTER TABLE "Идея"
	ADD CONSTRAINT "Идея_FПользов_2243" FOREIGN KEY ("Автор_m0") REFERENCES "Пользователь" ("primaryKey");

CREATE INDEX "Идея_IАвтор_m0" on "Идея" ("Автор_m0");

ALTER TABLE "AuditMasterObject"
	ADD CONSTRAINT "AuditMasterObject_FAuditM_1960" FOREIGN KEY ("MasterObject") REFERENCES "AuditMasterMasterObject" ("primaryKey");

CREATE INDEX "AuditMasterObject_IMaster_4520" on "AuditMasterObject" ("MasterObject");

ALTER TABLE "InformationTestClass2"
	ADD CONSTRAINT "InformationTestClass2_FIn_3393" FOREIGN KEY ("InformationTestClass_m0") REFERENCES "InformationTestClass" ("primaryKey");

CREATE INDEX "InformationTestClass2_IIn_2146" on "InformationTestClass2" ("InformationTestClass_m0");

ALTER TABLE "InformationTestClass2"
	ADD CONSTRAINT "InformationTestClass2_FIn_6440" FOREIGN KEY ("InformationTestClass_m1") REFERENCES "InformationTestClassChild" ("primaryKey");

CREATE INDEX "InformationTestClass2_IIn_2147" on "InformationTestClass2" ("InformationTestClass_m1");

ALTER TABLE "TestClassA"
	ADD CONSTRAINT "TestClassA_FМастерМ_0" FOREIGN KEY ("Мастер_m0") REFERENCES "МастерМ" ("primaryKey");

CREATE INDEX "TestClassA_IМастер_m0" on "TestClassA" ("Мастер_m0");

ALTER TABLE "TestClassA"
	ADD CONSTRAINT "TestClassA_FНаслед_6107" FOREIGN KEY ("Мастер_m1") REFERENCES "НаследникМ1" ("primaryKey");

CREATE INDEX "TestClassA_IМастер_m1" on "TestClassA" ("Мастер_m1");

ALTER TABLE "TestClassA"
	ADD CONSTRAINT "TestClassA_FНаслед_6948" FOREIGN KEY ("Мастер_m2") REFERENCES "НаследникМ2" ("primaryKey");

CREATE INDEX "TestClassA_IМастер_m2" on "TestClassA" ("Мастер_m2");

ALTER TABLE "clb"
	ADD CONSTRAINT "clb_Fcla_0" FOREIGN KEY ("ref2") REFERENCES "cla" ("primaryKey");

CREATE INDEX "clb_Iref2" on "clb" ("ref2");

ALTER TABLE "clb"
	ADD CONSTRAINT "clb_Fcla_1" FOREIGN KEY ("ref1") REFERENCES "cla" ("primaryKey");

CREATE INDEX "clb_Iref1" on "clb" ("ref1");

ALTER TABLE "AggregatorUpdateObjectTest"
	ADD CONSTRAINT "AggregatorUpdateObjectTes_8986" FOREIGN KEY ("Detail") REFERENCES "DetailUpdateObjectTest" ("primaryKey");

CREATE INDEX "AggregatorUpdateObjectTes_6775" on "AggregatorUpdateObjectTest" ("Detail");

ALTER TABLE "Лапа"
	ADD CONSTRAINT "Лапа_FТипЛапы_0" FOREIGN KEY ("ТипЛапы_m0") REFERENCES "ТипЛапы" ("primaryKey");

CREATE INDEX "Лапа_IТипЛапы_m0" on "Лапа" ("ТипЛапы_m0");

ALTER TABLE "Лапа"
	ADD CONSTRAINT "Лапа_FКошка_0" FOREIGN KEY ("Кошка_m0") REFERENCES "Кошка" ("primaryKey");

CREATE INDEX "Лапа_IКошка_m0" on "Лапа" ("Кошка_m0");

ALTER TABLE "ИФХозДоговора"
	ADD CONSTRAINT "ИФХозДоговор_1682" FOREIGN KEY ("ИсточникФинан") REFERENCES "ИсточникФинанс" ("primaryKey");

CREATE INDEX "ИФХозДоговор_9587" on "ИФХозДоговора" ("ИсточникФинан");

ALTER TABLE "ИФХозДоговора"
	ADD CONSTRAINT "ИФХозДоговора_766" FOREIGN KEY ("ХозДоговор_m0") REFERENCES "ХозДоговор" ("primaryKey");

CREATE INDEX "ИФХозДоговор_4184" on "ИФХозДоговора" ("ХозДоговор_m0");

ALTER TABLE "Котенок"
	ADD CONSTRAINT "Котенок_FКошка_0" FOREIGN KEY ("Кошка") REFERENCES "Кошка" ("primaryKey");

CREATE INDEX "Котенок_IКошка" on "Котенок" ("Кошка");

ALTER TABLE "ЗначениеКритер"
	ADD CONSTRAINT "ЗначениеКрит_2079" FOREIGN KEY ("Критерий_m0") REFERENCES "КритерийОценки" ("primaryKey");

CREATE INDEX "ЗначениеКрит_5626" on "ЗначениеКритер" ("Критерий_m0");

ALTER TABLE "ЗначениеКритер"
	ADD CONSTRAINT "ЗначениеКрит_1697" FOREIGN KEY ("Идея_m0") REFERENCES "Идея" ("primaryKey");

CREATE INDEX "ЗначениеКрите_521" on "ЗначениеКритер" ("Идея_m0");

ALTER TABLE "ДокККонкурсу"
	ADD CONSTRAINT "ДокККонкурсу_F_411" FOREIGN KEY ("Конкурс_m0") REFERENCES "Конкурс" ("primaryKey");

CREATE INDEX "ДокККонкурсу__1428" on "ДокККонкурсу" ("Конкурс_m0");

ALTER TABLE "Лес"
	ADD CONSTRAINT "Лес_FСтрана_0" FOREIGN KEY ("Страна") REFERENCES "Страна" ("primaryKey");

CREATE INDEX "Лес_IСтрана" on "Лес" ("Страна");

ALTER TABLE "FullTypesDetail1"
	ADD CONSTRAINT "FullTypesDetail1_FFullTyp_9626" FOREIGN KEY ("FullTypesMainAgregator_m0") REFERENCES "FullTypesMainAgregator" ("primaryKey");

CREATE INDEX "FullTypesDetail1_IFullTyp_3281" on "FullTypesDetail1" ("FullTypesMainAgregator_m0");

ALTER TABLE "DetailUpdateObjectTest"
	ADD CONSTRAINT "DetailUpdateObjectTest_FM_9061" FOREIGN KEY ("Master") REFERENCES "MasterUpdateObjectTest" ("primaryKey");

CREATE INDEX "DetailUpdateObjectTest_IMaster" on "DetailUpdateObjectTest" ("Master");

ALTER TABLE "DetailUpdateObjectTest"
	ADD CONSTRAINT "DetailUpdateObjectTest_FA_6291" FOREIGN KEY ("AggregatorUpdateObjectTest") REFERENCES "AggregatorUpdateObjectTest" ("primaryKey");

CREATE INDEX "DetailUpdateObjectTest_IA_9828" on "DetailUpdateObjectTest" ("AggregatorUpdateObjectTest");

ALTER TABLE "Порода"
	ADD CONSTRAINT "Порода_FТипПо_7829" FOREIGN KEY ("ТипПороды") REFERENCES "ТипПороды" ("primaryKey");

CREATE INDEX "Порода_IТипПо_6947" on "Порода" ("ТипПороды");

ALTER TABLE "Порода"
	ADD CONSTRAINT "Порода_FПорода_0" FOREIGN KEY ("Иерархия") REFERENCES "Порода" ("primaryKey");

CREATE INDEX "Порода_IИерархия" on "Порода" ("Иерархия");

ALTER TABLE "Блоха"
	ADD CONSTRAINT "Блоха_FМедведь_0" FOREIGN KEY ("МедведьОбитания") REFERENCES "Медведь" ("primaryKey");

CREATE INDEX "Блоха_IМедвед_6073" on "Блоха" ("МедведьОбитания");

ALTER TABLE "DetailClass"
	ADD CONSTRAINT "DetailClass_FMasterClass_0" FOREIGN KEY ("MasterClass") REFERENCES "MasterClass" ("primaryKey");

CREATE INDEX "DetailClass_IMasterClass" on "DetailClass" ("MasterClass");

ALTER TABLE "Кредит"
	ADD CONSTRAINT "Кредит_FКлиент_0" FOREIGN KEY ("Клиент") REFERENCES "Клиент" ("primaryKey");

CREATE INDEX "Кредит_IКлиент" on "Кредит" ("Клиент");

ALTER TABLE "Кредит"
	ADD CONSTRAINT "Кредит_FИнспП_8484" FOREIGN KEY ("ИнспекторПоКред") REFERENCES "ИнспПоКредиту" ("primaryKey");

CREATE INDEX "Кредит_IИнспе_9849" on "Кредит" ("ИнспекторПоКред");

ALTER TABLE "MasterUpdateObjectTest"
	ADD CONSTRAINT "MasterUpdateObjectTest_FD_2387" FOREIGN KEY ("Detail") REFERENCES "DetailUpdateObjectTest" ("primaryKey");

CREATE INDEX "MasterUpdateObjectTest_IDetail" on "MasterUpdateObjectTest" ("Detail");

ALTER TABLE "MasterUpdateObjectTest"
	ADD CONSTRAINT "MasterUpdateObjectTest_FA_3311" FOREIGN KEY ("AggregatorUpdateObjectTest") REFERENCES "AggregatorUpdateObjectTest" ("primaryKey");

CREATE INDEX "MasterUpdateObjectTest_IA_8230" on "MasterUpdateObjectTest" ("AggregatorUpdateObjectTest");

ALTER TABLE "FullTypesMainAgregator"
	ADD CONSTRAINT "FullTypesMainAgregator_FF_4629" FOREIGN KEY ("FullTypesMaster1_m0") REFERENCES "FullTypesMaster1" ("primaryKey");

CREATE INDEX "FullTypesMainAgregator_IF_4367" on "FullTypesMainAgregator" ("FullTypesMaster1_m0");

ALTER TABLE "CombinedTypesUsageProviderTest"
	ADD CONSTRAINT "CombinedTypesUsageProvide_4085" FOREIGN KEY ("DataObjectForTest_m0") REFERENCES "DataObjectForTest" ("primaryKey");

CREATE INDEX "CombinedTypesUsageProvide_3833" on "CombinedTypesUsageProviderTest" ("DataObjectForTest_m0");

ALTER TABLE "CombinedTypesUsageProviderTest"
	ADD CONSTRAINT "CombinedTypesUsageProvide_1832" FOREIGN KEY ("TypeUsageProviderTestClass") REFERENCES "TypeUsageProviderTestClass" ("primaryKey");

CREATE INDEX "CombinedTypesUsageProvide_2017" on "CombinedTypesUsageProviderTest" ("TypeUsageProviderTestClass");

ALTER TABLE "Кошка"
	ADD CONSTRAINT "Кошка_FПорода_0" FOREIGN KEY ("Порода") REFERENCES "Порода" ("primaryKey");

CREATE INDEX "Кошка_IПорода" on "Кошка" ("Порода");

ALTER TABLE "SomeDetailClass"
	ADD CONSTRAINT "SomeDetailClass_FSomeMast_9095" FOREIGN KEY ("ClassA") REFERENCES "SomeMasterClass" ("primaryKey");

CREATE INDEX "SomeDetailClass_IClassA" on "SomeDetailClass" ("ClassA");

ALTER TABLE "FullTypesDetail2"
	ADD CONSTRAINT "FullTypesDetail2_FFullType_494" FOREIGN KEY ("FullTypesMainAgregator") REFERENCES "FullTypesMainAgregator" ("primaryKey");

CREATE INDEX "FullTypesDetail2_IFullTyp_3279" on "FullTypesDetail2" ("FullTypesMainAgregator");

ALTER TABLE "Salad2"
	ADD CONSTRAINT "Salad2_FCabbage2_0" FOREIGN KEY ("Ingridient2_m0") REFERENCES "Cabbage2" ("primaryKey");

CREATE INDEX "Salad2_IIngridient2_m0" on "Salad2" ("Ingridient2_m0");

ALTER TABLE "Salad2"
	ADD CONSTRAINT "Salad2_FPlant2_0" FOREIGN KEY ("Ingridient2_m1") REFERENCES "Plant2" ("primaryKey");

CREATE INDEX "Salad2_IIngridient2_m1" on "Salad2" ("Ingridient2_m1");

ALTER TABLE "Salad2"
	ADD CONSTRAINT "Salad2_FCabbage2_1" FOREIGN KEY ("Ingridient1_m0") REFERENCES "Cabbage2" ("primaryKey");

CREATE INDEX "Salad2_IIngridient1_m0" on "Salad2" ("Ingridient1_m0");

ALTER TABLE "Salad2"
	ADD CONSTRAINT "Salad2_FPlant2_1" FOREIGN KEY ("Ingridient1_m1") REFERENCES "Plant2" ("primaryKey");

CREATE INDEX "Salad2_IIngridient1_m1" on "Salad2" ("Ingridient1_m1");

ALTER TABLE "Выплаты"
	ADD CONSTRAINT "Выплаты_FКредит_0" FOREIGN KEY ("Кредит1") REFERENCES "Кредит" ("primaryKey");

CREATE INDEX "Выплаты_IКредит1" on "Выплаты" ("Кредит1");

ALTER TABLE "УчастникХозДог"
	ADD CONSTRAINT "УчастникХозД_4757" FOREIGN KEY ("Личность_m0") REFERENCES "Личность" ("primaryKey");

CREATE INDEX "УчастникХозД_2992" on "УчастникХозДог" ("Личность_m0");

ALTER TABLE "УчастникХозДог"
	ADD CONSTRAINT "УчастникХозД_7733" FOREIGN KEY ("ХозДоговор_m0") REFERENCES "ХозДоговор" ("primaryKey");

CREATE INDEX "УчастникХозД_6087" on "УчастникХозДог" ("ХозДоговор_m0");

ALTER TABLE "Перелом"
	ADD CONSTRAINT "Перелом_FЛапа_0" FOREIGN KEY ("Лапа_m0") REFERENCES "Лапа" ("primaryKey");

CREATE INDEX "Перелом_IЛапа_m0" on "Перелом" ("Лапа_m0");

ALTER TABLE "Human2"
	ADD CONSTRAINT "Human2_FCountry2_0" FOREIGN KEY ("TodayHome_m0") REFERENCES "Country2" ("primaryKey");

CREATE INDEX "Human2_ITodayHome_m0" on "Human2" ("TodayHome_m0");

ALTER TABLE "Human2"
	ADD CONSTRAINT "Human2_FTerritory2_0" FOREIGN KEY ("TodayHome_m1") REFERENCES "Territory2" ("primaryKey");

CREATE INDEX "Human2_ITodayHome_m1" on "Human2" ("TodayHome_m1");

ALTER TABLE "ОценкаЭксперта"
	ADD CONSTRAINT "ОценкаЭкспер_8419" FOREIGN KEY ("ЗначениеКритер") REFERENCES "ЗначениеКритер" ("primaryKey");

CREATE INDEX "ОценкаЭкспер_9050" on "ОценкаЭксперта" ("ЗначениеКритер");

ALTER TABLE "ОценкаЭксперта"
	ADD CONSTRAINT "ОценкаЭкспер_9101" FOREIGN KEY ("Эксперт_m0") REFERENCES "Пользователь" ("primaryKey");

CREATE INDEX "ОценкаЭкспер_4518" on "ОценкаЭксперта" ("Эксперт_m0");

ALTER TABLE "ОценкаЭксперта"
	ADD CONSTRAINT "ОценкаЭкспер_6875" FOREIGN KEY ("Идея_m0") REFERENCES "Идея" ("primaryKey");

CREATE INDEX "ОценкаЭксперт_578" on "ОценкаЭксперта" ("Идея_m0");

ALTER TABLE "Медведь"
	ADD CONSTRAINT "Медведь_FМедв_4334" FOREIGN KEY ("Мама") REFERENCES "Медведь" ("primaryKey");

CREATE INDEX "Медведь_IМама" on "Медведь" ("Мама");

ALTER TABLE "Медведь"
	ADD CONSTRAINT "Медведь_FМедв_4335" FOREIGN KEY ("Папа") REFERENCES "Медведь" ("primaryKey");

CREATE INDEX "Медведь_IПапа" on "Медведь" ("Папа");

ALTER TABLE "Медведь"
	ADD CONSTRAINT "Медведь_FЛес_0" FOREIGN KEY ("ЛесОбитания") REFERENCES "Лес" ("primaryKey");

CREATE INDEX "Медведь_IЛесО_5757" on "Медведь" ("ЛесОбитания");

ALTER TABLE "InformationTestClass6"
	ADD CONSTRAINT "InformationTestClass6_FCl_6040" FOREIGN KEY ("ExampleOfClassWithCaptions") REFERENCES "ClassWithCaptions" ("primaryKey");

CREATE INDEX "InformationTestClass6_IEx_9155" on "InformationTestClass6" ("ExampleOfClassWithCaptions");

ALTER TABLE "Берлога"
	ADD CONSTRAINT "Берлога_FЛес_0" FOREIGN KEY ("ЛесРасположения") REFERENCES "Лес" ("primaryKey");

CREATE INDEX "Берлога_IЛесР_1411" on "Берлога" ("ЛесРасположения");

ALTER TABLE "Берлога"
	ADD CONSTRAINT "Берлога_FМедв_5600" FOREIGN KEY ("Медведь") REFERENCES "Медведь" ("primaryKey");

CREATE INDEX "Берлога_IМедведь" on "Берлога" ("Медведь");

ALTER TABLE "Apparatus2"
	ADD CONSTRAINT "Apparatus2_FCountry2_0" FOREIGN KEY ("Maker_m0") REFERENCES "Country2" ("primaryKey");

CREATE INDEX "Apparatus2_IMaker_m0" on "Apparatus2" ("Maker_m0");

ALTER TABLE "Apparatus2"
	ADD CONSTRAINT "Apparatus2_FCountry2_1" FOREIGN KEY ("Exporter_m0") REFERENCES "Country2" ("primaryKey");

CREATE INDEX "Apparatus2_IExporter_m0" on "Apparatus2" ("Exporter_m0");

ALTER TABLE "Dish2"
	ADD CONSTRAINT "Dish2_FCabbage2_0" FOREIGN KEY ("MainIngridient_m0") REFERENCES "Cabbage2" ("primaryKey");

CREATE INDEX "Dish2_IMainIngridient_m0" on "Dish2" ("MainIngridient_m0");

ALTER TABLE "Dish2"
	ADD CONSTRAINT "Dish2_FPlant2_0" FOREIGN KEY ("MainIngridient_m1") REFERENCES "Plant2" ("primaryKey");

CREATE INDEX "Dish2_IMainIngridient_m1" on "Dish2" ("MainIngridient_m1");

ALTER TABLE "КритерийОценки"
	ADD CONSTRAINT "КритерийОцен_4993" FOREIGN KEY ("Конкурс_m0") REFERENCES "Конкурс" ("primaryKey");

CREATE INDEX "КритерийОцен_4215" on "КритерийОценки" ("Конкурс_m0");

ALTER TABLE "InformationTestClass4"
	ADD CONSTRAINT "InformationTestClass4_FIn_1097" FOREIGN KEY ("MasterOfInformationTestClass3") REFERENCES "InformationTestClass3" ("primaryKey");

CREATE INDEX "InformationTestClass4_IMa_9124" on "InformationTestClass4" ("MasterOfInformationTestClass3");

ALTER TABLE "ClassWithCaptions"
	ADD CONSTRAINT "ClassWithCaptions_FInform_3101" FOREIGN KEY ("InformationTestClass4") REFERENCES "InformationTestClass4" ("primaryKey");

CREATE INDEX "ClassWithCaptions_IInform_1502" on "ClassWithCaptions" ("InformationTestClass4");

ALTER TABLE "Adress2"
	ADD CONSTRAINT "Adress2_FCountry2_0" FOREIGN KEY ("Country_m0") REFERENCES "Country2" ("primaryKey");

CREATE INDEX "Adress2_ICountry_m0" on "Adress2" ("Country_m0");

ALTER TABLE "Конкурс"
	ADD CONSTRAINT "Конкурс_FПоль_5817" FOREIGN KEY ("Организатор_m0") REFERENCES "Пользователь" ("primaryKey");

CREATE INDEX "Конкурс_IОрга_2722" on "Конкурс" ("Организатор_m0");

ALTER TABLE "TypeUsageProviderTestClassChil"
	ADD CONSTRAINT "TypeUsageProviderTestClass_812" FOREIGN KEY ("DataObjectForTest_m0") REFERENCES "DataObjectForTest" ("primaryKey");

CREATE INDEX "TypeUsageProviderTestClas_8986" on "TypeUsageProviderTestClassChil" ("DataObjectForTest_m0");

ALTER TABLE "CabbageSalad"
	ADD CONSTRAINT "CabbageSalad_FCabbage2_0" FOREIGN KEY ("Cabbage1") REFERENCES "Cabbage2" ("primaryKey");

CREATE INDEX "CabbageSalad_ICabbage1" on "CabbageSalad" ("Cabbage1");

ALTER TABLE "CabbageSalad"
	ADD CONSTRAINT "CabbageSalad_FCabbage2_1" FOREIGN KEY ("Cabbage2") REFERENCES "Cabbage2" ("primaryKey");

CREATE INDEX "CabbageSalad_ICabbage2" on "CabbageSalad" ("Cabbage2");

ALTER TABLE "CabbagePart2"
	ADD CONSTRAINT "CabbagePart2_FCabbage2_0" FOREIGN KEY ("Cabbage") REFERENCES "Cabbage2" ("primaryKey");

CREATE INDEX "CabbagePart2_ICabbage" on "CabbagePart2" ("Cabbage");

ALTER TABLE "cla"
	ADD CONSTRAINT "cla_Fclb_0" FOREIGN KEY ("parent") REFERENCES "clb" ("primaryKey");

CREATE INDEX "cla_Iparent" on "cla" ("parent");

ALTER TABLE "Place2"
	ADD CONSTRAINT "Place2_FCountry2_0" FOREIGN KEY ("TodayTerritory_m0") REFERENCES "Country2" ("primaryKey");

CREATE INDEX "Place2_ITodayTerritory_m0" on "Place2" ("TodayTerritory_m0");

ALTER TABLE "Place2"
	ADD CONSTRAINT "Place2_FTerritory2_0" FOREIGN KEY ("TodayTerritory_m1") REFERENCES "Territory2" ("primaryKey");

CREATE INDEX "Place2_ITodayTerritory_m1" on "Place2" ("TodayTerritory_m1");

ALTER TABLE "Place2"
	ADD CONSTRAINT "Place2_FCountry2_1" FOREIGN KEY ("TomorrowTeritory_m0") REFERENCES "Country2" ("primaryKey");

CREATE INDEX "Place2_ITomorrowTeritory_m0" on "Place2" ("TomorrowTeritory_m0");

ALTER TABLE "Place2"
	ADD CONSTRAINT "Place2_FTerritory2_1" FOREIGN KEY ("TomorrowTeritory_m1") REFERENCES "Territory2" ("primaryKey");

CREATE INDEX "Place2_ITomorrowTeritory_m1" on "Place2" ("TomorrowTeritory_m1");

ALTER TABLE "TypeUsageProviderTestClass"
	ADD CONSTRAINT "TypeUsageProviderTestClas_3383" FOREIGN KEY ("DataObjectForTest_m0") REFERENCES "DataObjectForTest" ("primaryKey");

CREATE INDEX "TypeUsageProviderTestClas_3489" on "TypeUsageProviderTestClass" ("DataObjectForTest_m0");

ALTER TABLE "AuditAgregatorObject"
	ADD CONSTRAINT "AuditAgregatorObject_FAud_2275" FOREIGN KEY ("MasterObject") REFERENCES "AuditMasterObject" ("primaryKey");

CREATE INDEX "AuditAgregatorObject_IMas_3563" on "AuditAgregatorObject" ("MasterObject");

ALTER TABLE "Region"
	ADD CONSTRAINT "Region_FCountry2_0" FOREIGN KEY ("Country2_m0") REFERENCES "Country2" ("primaryKey");

CREATE INDEX "Region_ICountry2_m0" on "Region" ("Country2_m0");

ALTER TABLE "ФайлИдеи"
	ADD CONSTRAINT "ФайлИдеи_FПол_7245" FOREIGN KEY ("Владелец_m0") REFERENCES "Пользователь" ("primaryKey");

CREATE INDEX "ФайлИдеи_IВлад_739" on "ФайлИдеи" ("Владелец_m0");

ALTER TABLE "ФайлИдеи"
	ADD CONSTRAINT "ФайлИдеи_FИдея_0" FOREIGN KEY ("Идея_m0") REFERENCES "Идея" ("primaryKey");

CREATE INDEX "ФайлИдеи_IИдея_m0" on "ФайлИдеи" ("Идея_m0");

ALTER TABLE "InformationTestClass3"
	ADD CONSTRAINT "InformationTestClass3_FIn_6092" FOREIGN KEY ("InformationTestClass2") REFERENCES "InformationTestClass2" ("primaryKey");

CREATE INDEX "InformationTestClass3_IIn_7693" on "InformationTestClass3" ("InformationTestClass2");

ALTER TABLE "Soup2"
	ADD CONSTRAINT "Soup2_FCabbage2_0" FOREIGN KEY ("CabbageType") REFERENCES "Cabbage2" ("primaryKey");

CREATE INDEX "Soup2_ICabbageType" on "Soup2" ("CabbageType");

ALTER TABLE "MasterClass"
	ADD CONSTRAINT "MasterClass_FInformationT_6917" FOREIGN KEY ("InformationTestClass2") REFERENCES "InformationTestClass2" ("primaryKey");

CREATE INDEX "MasterClass_IInformationT_1923" on "MasterClass" ("InformationTestClass2");

ALTER TABLE "MasterClass"
	ADD CONSTRAINT "MasterClass_FInformationT_7758" FOREIGN KEY ("InformationTestClass3_m0") REFERENCES "InformationTestClass3" ("primaryKey");

CREATE INDEX "MasterClass_IInformationT_4661" on "MasterClass" ("InformationTestClass3_m0");

ALTER TABLE "MasterClass"
	ADD CONSTRAINT "MasterClass_FInformationT_2185" FOREIGN KEY ("InformationTestClass_m0") REFERENCES "InformationTestClass" ("primaryKey");

CREATE INDEX "MasterClass_IInformationT_7142" on "MasterClass" ("InformationTestClass_m0");

ALTER TABLE "MasterClass"
	ADD CONSTRAINT "MasterClass_FInformationTe_539" FOREIGN KEY ("InformationTestClass_m1") REFERENCES "InformationTestClassChild" ("primaryKey");

CREATE INDEX "MasterClass_IInformationT_7143" on "MasterClass" ("InformationTestClass_m1");

ALTER TABLE "STORMWEBSEARCH"
	ADD CONSTRAINT "STORMWEBSEARCH_FSTORMFILT_6521" FOREIGN KEY ("FilterSetting_m0") REFERENCES "STORMFILTERSETTING" ("primaryKey");

ALTER TABLE "STORMFILTERDETAIL"
	ADD CONSTRAINT "STORMFILTERDETAIL_FSTORMF_2900" FOREIGN KEY ("FilterSetting_m0") REFERENCES "STORMFILTERSETTING" ("primaryKey");

ALTER TABLE "STORMFILTERLOOKUP"
	ADD CONSTRAINT "STORMFILTERLOOKUP_FSTORMF_1583" FOREIGN KEY ("FilterSetting_m0") REFERENCES "STORMFILTERSETTING" ("primaryKey");

ALTER TABLE "STORMLG"
	ADD CONSTRAINT "STORMLG_FSTORMAG_0" FOREIGN KEY ("Group_m0") REFERENCES "STORMAG" ("primaryKey");

ALTER TABLE "STORMLG"
	ADD CONSTRAINT "STORMLG_FSTORMAG_1" FOREIGN KEY ("User_m0") REFERENCES "STORMAG" ("primaryKey");

ALTER TABLE "STORMAuEntity"
	ADD CONSTRAINT "STORMAuEntity_FSTORMAG_0" FOREIGN KEY ("User_m0") REFERENCES "STORMAG" ("primaryKey");

ALTER TABLE "STORMAuEntity"
	ADD CONSTRAINT "STORMAuEntity_FSTORMAuObj_3287" FOREIGN KEY ("ObjectType_m0") REFERENCES "STORMAuObjType" ("primaryKey");

ALTER TABLE "STORMAuField"
	ADD CONSTRAINT "STORMAuField_FSTORMAuField_0" FOREIGN KEY ("MainChange_m0") REFERENCES "STORMAuField" ("primaryKey");

ALTER TABLE "STORMAuField"
	ADD CONSTRAINT "STORMAuField_FSTORMAuEntity_0" FOREIGN KEY ("AuditEntity_m0") REFERENCES "STORMAuEntity" ("primaryKey");


