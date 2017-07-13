



CREATE TABLE МастерКлассДлинноеИмя (

 primaryKey UUID NOT NULL,

 АтрибутМастерКласса01 VARCHAR(255) NULL,

 АтрибутМастерКласса02 VARCHAR(255) NULL,

 MasterAttr1 TIMESTAMP(3) NULL,

 MasterAttr2 BOOLEAN NULL,

 MasterRoot UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_ulong (

 primaryKey UUID NOT NULL,

 Attr BIGINT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ДочернийКлассДлинноеИмя (

 primaryKey UUID NOT NULL,

 Attr21 TEXT NULL,

 Attr20 SMALLINT NULL,

 Attr19 BIGINT NULL,

 Attr18 INT NULL,

 Attr17 SMALLINT NULL,

 Attr16 SMALLINT NULL,

 Attr15 BYTEA NULL,

 Attr14 INT NULL,

 Attr13 DECIMAL NULL,

 Attr12 TIMESTAMP(3) NULL,

 Attr11 BIGINT NULL,

 Attr10 UUID NULL,

 Attr9 REAL NULL,

 Attr8 DOUBLE PRECISION NULL,

 Attr7 DECIMAL NULL,

 Attr6 TIMESTAMP(3) NULL,

 Attr5 SMALLINT NULL,

 Attr4 SMALLINT NULL,

 Атрибут3 BOOLEAN NULL,

 Attr1 VARCHAR(255) NULL,

 Attr2 INT NULL,

 МастерКлассДлинноеИмя01_m0 UUID NULL,

 МастерКлассДлинноеИмя01_m1 UUID NULL,

 МастерКлассДлинноеИмя02 UUID NOT NULL,

 MyClass2_m0 UUID NULL,

 MyClass2_m1 UUID NULL,

 MyClass2_m2 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_guid (

 primaryKey UUID NOT NULL,

 Attr UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_char (

 primaryKey UUID NOT NULL,

 Attr SMALLINT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_int (

 primaryKey UUID NOT NULL,

 Attr INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_NullableInt (

 primaryKey UUID NOT NULL,

 Attr INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_NullableDateTime (

 primaryKey UUID NOT NULL,

 Attr TIMESTAMP(3) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_WebFile (

 primaryKey UUID NOT NULL,

 Attr TEXT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_ushort (

 primaryKey UUID NOT NULL,

 Attr SMALLINT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE DetailClass (

 primaryKey UUID NOT NULL,

 DetailAttr VARCHAR(255) NULL,

 MyClass1_m0 UUID NULL,

 MyClass1_m1 UUID NULL,

 MyClass1_m2 UUID NULL,

 MyClass1_m3 UUID NULL,

 MyClass1_m4 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_uint (

 primaryKey UUID NOT NULL,

 Attr INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Класс (

 primaryKey UUID NOT NULL,

 Attr21 TEXT NULL,

 Attr20 SMALLINT NULL,

 Attr19 BIGINT NULL,

 Attr18 INT NULL,

 Attr17 SMALLINT NULL,

 Attr16 SMALLINT NULL,

 Attr15 BYTEA NULL,

 Attr14 INT NULL,

 Attr13 DECIMAL NULL,

 Attr12 TIMESTAMP(3) NULL,

 Attr11 BIGINT NULL,

 Attr10 UUID NULL,

 Attr9 REAL NULL,

 Attr8 DOUBLE PRECISION NULL,

 Attr7 DECIMAL NULL,

 Attr6 TIMESTAMP(3) NULL,

 Attr5 SMALLINT NULL,

 Attr4 SMALLINT NULL,

 Атрибут3 BOOLEAN NULL,

 Attr1 VARCHAR(255) NULL,

 Attr2 INT NULL,

 MyClass2_m0 UUID NULL,

 MyClass2_m1 UUID NULL,

 MyClass2_m2 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE MyClass (

 primaryKey UUID NOT NULL,

 Attr21 TEXT NULL,

 Attr20 SMALLINT NULL,

 Attr19 BIGINT NULL,

 Attr18 INT NULL,

 Attr17 SMALLINT NULL,

 Attr16 SMALLINT NULL,

 Attr15 BYTEA NULL,

 Attr14 INT NULL,

 Attr13 DECIMAL NULL,

 Attr12 TIMESTAMP(3) NULL,

 Attr11 BIGINT NULL,

 Attr10 UUID NULL,

 Attr9 REAL NULL,

 Attr8 DOUBLE PRECISION NULL,

 Attr7 DECIMAL NULL,

 Attr6 TIMESTAMP(3) NULL,

 Attr5 SMALLINT NULL,

 Attr4 SMALLINT NULL,

 Атрибут3 BOOLEAN NULL,

 Attr1 VARCHAR(255) NULL,

 Attr2 INT NULL,

 MyClass2_m0 UUID NULL,

 MyClass2_m1 UUID NULL,

 MyClass2_m2 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE DetailClass2 (

 primaryKey UUID NOT NULL,

 DetailAttr2 VARCHAR(255) NULL,

 DetailClass_m0 UUID NULL,

 DetailClass_m1 UUID NULL,

 DetailClass_m2 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ДочернийКлассДлинноеИмя2 (

 primaryKey UUID NOT NULL,

 Attr21 TEXT NULL,

 Attr20 SMALLINT NULL,

 Attr19 BIGINT NULL,

 Attr18 INT NULL,

 Attr17 SMALLINT NULL,

 Attr16 SMALLINT NULL,

 Attr15 BYTEA NULL,

 Attr14 INT NULL,

 Attr13 DECIMAL NULL,

 Attr12 TIMESTAMP(3) NULL,

 Attr11 BIGINT NULL,

 Attr10 UUID NULL,

 Attr9 REAL NULL,

 Attr8 DOUBLE PRECISION NULL,

 Attr7 DECIMAL NULL,

 Attr6 TIMESTAMP(3) NULL,

 Attr5 SMALLINT NULL,

 Attr4 SMALLINT NULL,

 Атрибут3 BOOLEAN NULL,

 Attr1 VARCHAR(255) NULL,

 Attr2 INT NULL,

 МастерКлассДлинноеИмя01_m0 UUID NULL,

 МастерКлассДлинноеИмя01_m1 UUID NULL,

 МастерКлассДлинноеИмя02 UUID NOT NULL,

 MyClass2_m0 UUID NULL,

 MyClass2_m1 UUID NULL,

 MyClass2_m2 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE РодительскийКлассДлинноеИмя (

 primaryKey UUID NOT NULL,

 Attr21 TEXT NULL,

 Attr20 SMALLINT NULL,

 Attr19 BIGINT NULL,

 Attr18 INT NULL,

 Attr17 SMALLINT NULL,

 Attr16 SMALLINT NULL,

 Attr15 BYTEA NULL,

 Attr14 INT NULL,

 Attr13 DECIMAL NULL,

 Attr12 TIMESTAMP(3) NULL,

 Attr11 BIGINT NULL,

 Attr10 UUID NULL,

 Attr9 REAL NULL,

 Attr8 DOUBLE PRECISION NULL,

 Attr7 DECIMAL NULL,

 Attr6 TIMESTAMP(3) NULL,

 Attr5 SMALLINT NULL,

 Attr4 SMALLINT NULL,

 Атрибут3 BOOLEAN NULL,

 Attr1 VARCHAR(255) NULL,

 Attr2 INT NULL,

 MyClass2_m0 UUID NULL,

 MyClass2_m1 UUID NULL,

 MyClass2_m2 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_byte (

 primaryKey UUID NOT NULL,

 Attr SMALLINT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_sbyte (

 primaryKey UUID NOT NULL,

 Attr SMALLINT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_double (

 primaryKey UUID NOT NULL,

 Attr DOUBLE PRECISION NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_DateTime (

 primaryKey UUID NOT NULL,

 Attr TIMESTAMP(3) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_decimal (

 primaryKey UUID NOT NULL,

 Attr DECIMAL NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_short (

 primaryKey UUID NOT NULL,

 Attr SMALLINT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_bool (

 primaryKey UUID NOT NULL,

 Attr BOOLEAN NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_object (

 primaryKey UUID NOT NULL,

 Attr BYTEA NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_float (

 primaryKey UUID NOT NULL,

 Attr REAL NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE MasterClass (

 primaryKey UUID NOT NULL,

 MasterAttr1 TIMESTAMP(3) NULL,

 MasterAttr2 BOOLEAN NULL,

 MasterRoot UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE MasterRoot (

 primaryKey UUID NOT NULL,

 MasterAttr INT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ДетейлКлассДлинноеИмя (

 primaryKey UUID NOT NULL,

 DetailAttr VARCHAR(255) NULL,

 MyClass1_m0 UUID NULL,

 MyClass1_m1 UUID NULL,

 MyClass1_m2 UUID NULL,

 MyClass1_m3 UUID NULL,

 MyClass1_m4 UUID NULL,

 ДочернийКлассДлинноеИмя_m0 UUID NULL,

 ДочернийКлассДлинноеИмя_m1 UUID NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE ДетейлКлассДлинноеИмя2 (

 primaryKey UUID NOT NULL,

 DetailAttr VARCHAR(255) NULL,

 MyClass1_m0 UUID NULL,

 MyClass1_m1 UUID NULL,

 MyClass1_m2 UUID NULL,

 MyClass1_m3 UUID NULL,

 MyClass1_m4 UUID NULL,

 ДочернийКлассДлинноеИмя_m0 UUID NULL,

 ДочернийКлассДлинноеИмя_m1 UUID NULL,

 ДочернийКлассДлинноеИмя2 UUID NOT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_NullableDecimal (

 primaryKey UUID NOT NULL,

 Attr DECIMAL NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_long (

 primaryKey UUID NOT NULL,

 Attr BIGINT NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE Class_string (

 primaryKey UUID NOT NULL,

 Attr VARCHAR(255) NULL,

 PRIMARY KEY (primaryKey));


CREATE TABLE МастерКлассДлинноеИмя2 (

 primaryKey UUID NOT NULL,

 АтрибутМастерКласса03 VARCHAR(255) NULL,

 АтрибутМастерКласса01 VARCHAR(255) NULL,

 АтрибутМастерКласса02 VARCHAR(255) NULL,

 MasterAttr1 TIMESTAMP(3) NULL,

 MasterAttr2 BOOLEAN NULL,

 MasterRoot UUID NOT NULL,

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




 ALTER TABLE МастерКлассДлинноеИмя ADD CONSTRAINT FKc0c25d4b4f6940778d0cd9ec3abfe5df FOREIGN KEY (MasterRoot) REFERENCES MasterRoot; 
CREATE INDEX Index49db02a2366046c48a3e4654c9bbb6c4 on МастерКлассДлинноеИмя (MasterRoot); 

 ALTER TABLE ДочернийКлассДлинноеИмя ADD CONSTRAINT FKae7c58b9e8a94ed9927656035f697804 FOREIGN KEY (МастерКлассДлинноеИмя01_m0) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Indexab14375422ba42979d76a8f71fdec04d on ДочернийКлассДлинноеИмя (МастерКлассДлинноеИмя01_m0); 

 ALTER TABLE ДочернийКлассДлинноеИмя ADD CONSTRAINT FK60a522b6efb94d1ba406b3e0ac523024 FOREIGN KEY (МастерКлассДлинноеИмя01_m1) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Index468c3ae0e1e44abd896b9affacf80dc6 on ДочернийКлассДлинноеИмя (МастерКлассДлинноеИмя01_m1); 

 ALTER TABLE ДочернийКлассДлинноеИмя ADD CONSTRAINT FK9a68cd57271b449b8bf129f982e804f3 FOREIGN KEY (МастерКлассДлинноеИмя02) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Indexfcccacaf5d7c4c95b59d3b3733fc0f0f on ДочернийКлассДлинноеИмя (МастерКлассДлинноеИмя02); 

 ALTER TABLE ДочернийКлассДлинноеИмя ADD CONSTRAINT FKdc0380f0d8c942c9bae5b945e83ee2f4 FOREIGN KEY (MyClass2_m0) REFERENCES MasterClass; 
CREATE INDEX Index41aa5cddf0e8483f890ed54d26eb9769 on ДочернийКлассДлинноеИмя (MyClass2_m0); 

 ALTER TABLE ДочернийКлассДлинноеИмя ADD CONSTRAINT FKaa63279b07834e21b22562d7f235da8f FOREIGN KEY (MyClass2_m1) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Index2bf195ce1d1d42a998d0578dc0400b8e on ДочернийКлассДлинноеИмя (MyClass2_m1); 

 ALTER TABLE ДочернийКлассДлинноеИмя ADD CONSTRAINT FKce66bdef93d14ece9109efad92ba61a0 FOREIGN KEY (MyClass2_m2) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Index02d137590ac3422f9dabc2a2c9874d20 on ДочернийКлассДлинноеИмя (MyClass2_m2); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK545bd34b54624ab1880b6573d007f8c4 FOREIGN KEY (MyClass1_m0) REFERENCES MyClass; 
CREATE INDEX Index4598a0b614c04d00be7d9fba14943830 on DetailClass (MyClass1_m0); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK000184e7906a4b468b72d371e77719ba FOREIGN KEY (MyClass1_m1) REFERENCES ДочернийКлассДлинноеИмя; 
CREATE INDEX Index92ff28ca701a4464b46b83837f37e237 on DetailClass (MyClass1_m1); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK0088f7a366ad4199b051ec03828bcef8 FOREIGN KEY (MyClass1_m2) REFERENCES ДочернийКлассДлинноеИмя2; 
CREATE INDEX Indexe2ca459d10f54cec9ccb8fa380ba160c on DetailClass (MyClass1_m2); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK9242187eac7044a980f19a7bbbb4302c FOREIGN KEY (MyClass1_m3) REFERENCES Класс; 
CREATE INDEX Index987c9f16757049cb9c1effcc32f17cf4 on DetailClass (MyClass1_m3); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK33147e667b8b4ca7bc6fbcc69eb2167b FOREIGN KEY (MyClass1_m4) REFERENCES РодительскийКлассДлинноеИмя; 
CREATE INDEX Index40b02a678bfb42279f2a36a42e8c9c01 on DetailClass (MyClass1_m4); 

 ALTER TABLE Класс ADD CONSTRAINT FKb7746fe216974e1d8dd1c041d44bd26e FOREIGN KEY (MyClass2_m0) REFERENCES MasterClass; 
CREATE INDEX Index1ba401122ede4965951749cf1f3dca12 on Класс (MyClass2_m0); 

 ALTER TABLE Класс ADD CONSTRAINT FKf364826d54d14f458f3cff42fa833603 FOREIGN KEY (MyClass2_m1) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Index9d8a18699120456d94fea56109ca225b on Класс (MyClass2_m1); 

 ALTER TABLE Класс ADD CONSTRAINT FK4823e2be3e25459691c922d868aa0da9 FOREIGN KEY (MyClass2_m2) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Index59e0be910fb24cd9b179f6aa3dc71ac1 on Класс (MyClass2_m2); 

 ALTER TABLE MyClass ADD CONSTRAINT FKdbf850d74a6e47e6b40ff23fbdebf6e9 FOREIGN KEY (MyClass2_m0) REFERENCES MasterClass; 
CREATE INDEX Indexca335b2f706f4d8bab638a0bc18acefc on MyClass (MyClass2_m0); 

 ALTER TABLE MyClass ADD CONSTRAINT FK5e94f7f5f64543a98e4e56c8b0aa8d92 FOREIGN KEY (MyClass2_m1) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Indexbfef7c7fdc064cfa83e425f7b4c5346a on MyClass (MyClass2_m1); 

 ALTER TABLE MyClass ADD CONSTRAINT FKa436a33343124565ab46358e6017a506 FOREIGN KEY (MyClass2_m2) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Indexe3471615865c41f18069c02a3da73e7a on MyClass (MyClass2_m2); 

 ALTER TABLE DetailClass2 ADD CONSTRAINT FK4cfd2b36dbf3475b9af53d127bd8c6f2 FOREIGN KEY (DetailClass_m0) REFERENCES DetailClass; 
CREATE INDEX Index76dd999ffba34f5695175e9a2391d70d on DetailClass2 (DetailClass_m0); 

 ALTER TABLE DetailClass2 ADD CONSTRAINT FK70bfd8a1cc714b7cbdebe83a679582fc FOREIGN KEY (DetailClass_m1) REFERENCES ДетейлКлассДлинноеИмя; 
CREATE INDEX Indexfd5be72ed7594d279a226d0fc4beb945 on DetailClass2 (DetailClass_m1); 

 ALTER TABLE DetailClass2 ADD CONSTRAINT FKe79c0ba62d0648bb9a70b97246189727 FOREIGN KEY (DetailClass_m2) REFERENCES ДетейлКлассДлинноеИмя2; 
CREATE INDEX Indexe709f8044ca44c24a46b690eee51c8d8 on DetailClass2 (DetailClass_m2); 

 ALTER TABLE ДочернийКлассДлинноеИмя2 ADD CONSTRAINT FK0a3337c709844e74b32f5a15d07f833c FOREIGN KEY (МастерКлассДлинноеИмя01_m0) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Index7c3ad07dcf62404d9e9f57eb0faacf97 on ДочернийКлассДлинноеИмя2 (МастерКлассДлинноеИмя01_m0); 

 ALTER TABLE ДочернийКлассДлинноеИмя2 ADD CONSTRAINT FK6526295378e64739adbf1609d6fff853 FOREIGN KEY (МастерКлассДлинноеИмя01_m1) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Indexc1de980e25fb4aec93a0db383a32397a on ДочернийКлассДлинноеИмя2 (МастерКлассДлинноеИмя01_m1); 

 ALTER TABLE ДочернийКлассДлинноеИмя2 ADD CONSTRAINT FK97d521a074a84a6c9a7155ab454a45a2 FOREIGN KEY (МастерКлассДлинноеИмя02) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Index0b0e6338cc7e46e4b26b8f587a309e5b on ДочернийКлассДлинноеИмя2 (МастерКлассДлинноеИмя02); 

 ALTER TABLE ДочернийКлассДлинноеИмя2 ADD CONSTRAINT FK9a4c25bd3a174ce3b715c151bc20de1a FOREIGN KEY (MyClass2_m0) REFERENCES MasterClass; 
CREATE INDEX Index8c162ee85dfd4bb2a55517dd8f51dbb8 on ДочернийКлассДлинноеИмя2 (MyClass2_m0); 

 ALTER TABLE ДочернийКлассДлинноеИмя2 ADD CONSTRAINT FKa983d708fc7c46bf8ea9c7e282726986 FOREIGN KEY (MyClass2_m1) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Index7c770fce0a214c13ae24122a31a4afcc on ДочернийКлассДлинноеИмя2 (MyClass2_m1); 

 ALTER TABLE ДочернийКлассДлинноеИмя2 ADD CONSTRAINT FK0f6fd828c2f0498c8f04df6be4168209 FOREIGN KEY (MyClass2_m2) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Index18e8d5282d1d41e69bfb362551a69dcf on ДочернийКлассДлинноеИмя2 (MyClass2_m2); 

 ALTER TABLE РодительскийКлассДлинноеИмя ADD CONSTRAINT FK03bed0504309437da50e79212878646a FOREIGN KEY (MyClass2_m0) REFERENCES MasterClass; 
CREATE INDEX Index17ac3d7baae74296b5d0cbeb97095866 on РодительскийКлассДлинноеИмя (MyClass2_m0); 

 ALTER TABLE РодительскийКлассДлинноеИмя ADD CONSTRAINT FK36062a52a08b442e858b944982845db9 FOREIGN KEY (MyClass2_m1) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Index52f87829c1174bb8ad0de49a945fc668 on РодительскийКлассДлинноеИмя (MyClass2_m1); 

 ALTER TABLE РодительскийКлассДлинноеИмя ADD CONSTRAINT FK241659426f454c0ca0086b468be014f3 FOREIGN KEY (MyClass2_m2) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Indexf6a073a1abec4d4b9f361cfaea796327 on РодительскийКлассДлинноеИмя (MyClass2_m2); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK783e2926514046e29d3a8874e33b9294 FOREIGN KEY (MasterRoot) REFERENCES MasterRoot; 
CREATE INDEX Index8485fec9d3ce4a24b65517a8fe4a934a on MasterClass (MasterRoot); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FK839aef34ac774a04a5b06d8278eab7b6 FOREIGN KEY (MyClass1_m0) REFERENCES MyClass; 
CREATE INDEX Indexe5ad5ac29d834e419d4ba41859ebf113 on ДетейлКлассДлинноеИмя (MyClass1_m0); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FK145d708a13014ba3890fbc872a7d8960 FOREIGN KEY (MyClass1_m1) REFERENCES ДочернийКлассДлинноеИмя; 
CREATE INDEX Index55414b276eae4032974bca1ec6b1739c on ДетейлКлассДлинноеИмя (MyClass1_m1); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FKccd4a4e831ec406f9423c44e42ebcc3b FOREIGN KEY (MyClass1_m2) REFERENCES ДочернийКлассДлинноеИмя2; 
CREATE INDEX Index416d34e736be4aa9aa40791f6c213750 on ДетейлКлассДлинноеИмя (MyClass1_m2); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FKeba3cd1ccc0146a595c2d47650bea888 FOREIGN KEY (MyClass1_m3) REFERENCES Класс; 
CREATE INDEX Index6ca1fd4af2b5443783f9f7274e4ea131 on ДетейлКлассДлинноеИмя (MyClass1_m3); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FK93e60ad6b658429ab63c2e821dcfa852 FOREIGN KEY (MyClass1_m4) REFERENCES РодительскийКлассДлинноеИмя; 
CREATE INDEX Indexeb72c815f294406c94c5719736757816 on ДетейлКлассДлинноеИмя (MyClass1_m4); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FK3155c13ba3ba416cb5c2b4df92f13d71 FOREIGN KEY (ДочернийКлассДлинноеИмя_m0) REFERENCES ДочернийКлассДлинноеИмя; 
CREATE INDEX Indexb84da3943b214228a534b33fdbec8ffe on ДетейлКлассДлинноеИмя (ДочернийКлассДлинноеИмя_m0); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FK955ab31b48564e358d9d0e5773f2c1a5 FOREIGN KEY (ДочернийКлассДлинноеИмя_m1) REFERENCES ДочернийКлассДлинноеИмя2; 
CREATE INDEX Indexe485acb081344d94a74216102950a485 on ДетейлКлассДлинноеИмя (ДочернийКлассДлинноеИмя_m1); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FKa67cb37a884548a0ba3ed4048442b3a2 FOREIGN KEY (MyClass1_m0) REFERENCES MyClass; 
CREATE INDEX Indexb18bf7f73e4d482a89be914019856166 on ДетейлКлассДлинноеИмя2 (MyClass1_m0); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FK8dcc02637b3f438f98ed49aab084bdf4 FOREIGN KEY (MyClass1_m1) REFERENCES ДочернийКлассДлинноеИмя; 
CREATE INDEX Index701cbaaf397d41919a09e006af356e5d on ДетейлКлассДлинноеИмя2 (MyClass1_m1); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FKf5910ee2230a439fa3b1dce6c4b667c7 FOREIGN KEY (MyClass1_m2) REFERENCES ДочернийКлассДлинноеИмя2; 
CREATE INDEX Index8ba68ca8b2a84a9396ee5439e1f27443 on ДетейлКлассДлинноеИмя2 (MyClass1_m2); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FK8dbd09cc27a54c62a42dff42a64c5e0b FOREIGN KEY (MyClass1_m3) REFERENCES Класс; 
CREATE INDEX Index5e92542cafa04010960099b17f4847c6 on ДетейлКлассДлинноеИмя2 (MyClass1_m3); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FK74c206826022433d8d33df79bf117456 FOREIGN KEY (MyClass1_m4) REFERENCES РодительскийКлассДлинноеИмя; 
CREATE INDEX Index2fb1300a9ccd4ffc89f7e982015f7460 on ДетейлКлассДлинноеИмя2 (MyClass1_m4); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FK54c1dcc4914341d5b3c1529d3378b417 FOREIGN KEY (ДочернийКлассДлинноеИмя_m0) REFERENCES ДочернийКлассДлинноеИмя; 
CREATE INDEX Index011bb659715846f597562ee1f091ee78 on ДетейлКлассДлинноеИмя2 (ДочернийКлассДлинноеИмя_m0); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FKccb04ed397454caa84714306810597e2 FOREIGN KEY (ДочернийКлассДлинноеИмя_m1) REFERENCES ДочернийКлассДлинноеИмя2; 
CREATE INDEX Indexa0525fed4ac64b64be7868f8b7be6040 on ДетейлКлассДлинноеИмя2 (ДочернийКлассДлинноеИмя_m1); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FK7b9121bafe704ad9b0d21b9916cff56d FOREIGN KEY (ДочернийКлассДлинноеИмя2) REFERENCES ДочернийКлассДлинноеИмя2; 
CREATE INDEX Indexeede17b09ef8463da6b572362e2574a1 on ДетейлКлассДлинноеИмя2 (ДочернийКлассДлинноеИмя2); 

 ALTER TABLE МастерКлассДлинноеИмя2 ADD CONSTRAINT FKcca46282e6d143aba815b2ccf4290df0 FOREIGN KEY (MasterRoot) REFERENCES MasterRoot; 
CREATE INDEX Indexda14751abc0040ad893b993ea54ae92d on МастерКлассДлинноеИмя2 (MasterRoot); 

 ALTER TABLE STORMWEBSEARCH ADD CONSTRAINT FK195ba302e91b4ab497038ac16798f98d FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERDETAIL ADD CONSTRAINT FKea5afa3177a146fbba122b281bc1d733 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERLOOKUP ADD CONSTRAINT FKe1477004682144b29fe4840c2ec8b74c FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 
