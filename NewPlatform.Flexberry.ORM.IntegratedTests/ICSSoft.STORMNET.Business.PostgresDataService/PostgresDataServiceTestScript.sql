




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
 Attr1 VARCHAR(255) NULL,
 Attr10 UUID NULL,
 Attr11 BIGINT NULL,
 Attr12 TIMESTAMP(3) NULL,
 Attr13 DECIMAL NULL,
 Attr14 INT NULL,
 Attr15 BYTEA NULL,
 Attr16 SMALLINT NULL,
 Attr17 SMALLINT NULL,
 Attr18 INT NULL,
 Attr19 BIGINT NULL,
 Attr2 INT NULL,
 Attr20 SMALLINT NULL,
 Attr21 TEXT NULL,
 Attr4 SMALLINT NULL,
 Attr5 SMALLINT NULL,
 Attr6 TIMESTAMP(3) NULL,
 Attr7 DECIMAL NULL,
 Attr8 DOUBLE PRECISION NULL,
 Attr9 REAL NULL,
 Атрибут3 BOOLEAN NULL,
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
 Attr1 VARCHAR(255) NULL,
 Attr10 UUID NULL,
 Attr11 BIGINT NULL,
 Attr12 TIMESTAMP(3) NULL,
 Attr13 DECIMAL NULL,
 Attr14 INT NULL,
 Attr15 BYTEA NULL,
 Attr16 SMALLINT NULL,
 Attr17 SMALLINT NULL,
 Attr18 INT NULL,
 Attr19 BIGINT NULL,
 Attr2 INT NULL,
 Attr20 SMALLINT NULL,
 Attr21 TEXT NULL,
 Attr4 SMALLINT NULL,
 Attr5 SMALLINT NULL,
 Attr6 TIMESTAMP(3) NULL,
 Attr7 DECIMAL NULL,
 Attr8 DOUBLE PRECISION NULL,
 Attr9 REAL NULL,
 Атрибут3 BOOLEAN NULL,
 MyClass2_m0 UUID NULL,
 MyClass2_m1 UUID NULL,
 MyClass2_m2 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE MyClass (
 primaryKey UUID NOT NULL,
 Attr1 VARCHAR(255) NULL,
 Attr10 UUID NULL,
 Attr11 BIGINT NULL,
 Attr12 TIMESTAMP(3) NULL,
 Attr13 DECIMAL NULL,
 Attr14 INT NULL,
 Attr15 BYTEA NULL,
 Attr16 SMALLINT NULL,
 Attr17 SMALLINT NULL,
 Attr18 INT NULL,
 Attr19 BIGINT NULL,
 Attr2 INT NULL,
 Attr20 SMALLINT NULL,
 Attr21 TEXT NULL,
 Attr4 SMALLINT NULL,
 Attr5 SMALLINT NULL,
 Attr6 TIMESTAMP(3) NULL,
 Attr7 DECIMAL NULL,
 Attr8 DOUBLE PRECISION NULL,
 Attr9 REAL NULL,
 Атрибут3 BOOLEAN NULL,
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
 Attr1 VARCHAR(255) NULL,
 Attr10 UUID NULL,
 Attr11 BIGINT NULL,
 Attr12 TIMESTAMP(3) NULL,
 Attr13 DECIMAL NULL,
 Attr14 INT NULL,
 Attr15 BYTEA NULL,
 Attr16 SMALLINT NULL,
 Attr17 SMALLINT NULL,
 Attr18 INT NULL,
 Attr19 BIGINT NULL,
 Attr2 INT NULL,
 Attr20 SMALLINT NULL,
 Attr21 TEXT NULL,
 Attr4 SMALLINT NULL,
 Attr5 SMALLINT NULL,
 Attr6 TIMESTAMP(3) NULL,
 Attr7 DECIMAL NULL,
 Attr8 DOUBLE PRECISION NULL,
 Attr9 REAL NULL,
 Атрибут3 BOOLEAN NULL,
 МастерКлассДлинноеИмя01_m0 UUID NULL,
 МастерКлассДлинноеИмя01_m1 UUID NULL,
 МастерКлассДлинноеИмя02 UUID NOT NULL,
 MyClass2_m0 UUID NULL,
 MyClass2_m1 UUID NULL,
 MyClass2_m2 UUID NULL,
 PRIMARY KEY (primaryKey));


CREATE TABLE РодительскийКлассДлинноеИмя (
 primaryKey UUID NOT NULL,
 Attr1 VARCHAR(255) NULL,
 Attr10 UUID NULL,
 Attr11 BIGINT NULL,
 Attr12 TIMESTAMP(3) NULL,
 Attr13 DECIMAL NULL,
 Attr14 INT NULL,
 Attr15 BYTEA NULL,
 Attr16 SMALLINT NULL,
 Attr17 SMALLINT NULL,
 Attr18 INT NULL,
 Attr19 BIGINT NULL,
 Attr2 INT NULL,
 Attr20 SMALLINT NULL,
 Attr21 TEXT NULL,
 Attr4 SMALLINT NULL,
 Attr5 SMALLINT NULL,
 Attr6 TIMESTAMP(3) NULL,
 Attr7 DECIMAL NULL,
 Attr8 DOUBLE PRECISION NULL,
 Attr9 REAL NULL,
 Атрибут3 BOOLEAN NULL,
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


CREATE TABLE Class_DateOnly (
 primaryKey UUID NOT NULL,
 AttrDate TIMESTAMP(3) NULL,
 AttrDateOnly DATE NULL,
 AttrString VARCHAR(255) NULL,
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



 ALTER TABLE МастерКлассДлинноеИмя ADD CONSTRAINT FKfb3ee17605a74a2f5442095f517db6998431d570 FOREIGN KEY (MasterRoot) REFERENCES MasterRoot; 
CREATE INDEX Indexfb3ee17605a74a2f5442095f517db6998431d570 on МастерКлассДлинноеИмя (MasterRoot); 

 ALTER TABLE ДочернийКлассДлинноеИмя ADD CONSTRAINT FKe17186faff3eaeb9545dd09e8bf853105d4e8fef FOREIGN KEY (МастерКлассДлинноеИмя01_m0) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Indexe17186faff3eaeb9545dd09e8bf853105d4e8fef on ДочернийКлассДлинноеИмя (МастерКлассДлинноеИмя01_m0); 

 ALTER TABLE ДочернийКлассДлинноеИмя ADD CONSTRAINT FK1f042255e425d1c8786b6b0103ae0d58d0c7c5ef FOREIGN KEY (МастерКлассДлинноеИмя01_m1) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Index1f042255e425d1c8786b6b0103ae0d58d0c7c5ef on ДочернийКлассДлинноеИмя (МастерКлассДлинноеИмя01_m1); 

 ALTER TABLE ДочернийКлассДлинноеИмя ADD CONSTRAINT FK025ab81ce67de2ca987719c3157aeec161206272 FOREIGN KEY (МастерКлассДлинноеИмя02) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Index025ab81ce67de2ca987719c3157aeec161206272 on ДочернийКлассДлинноеИмя (МастерКлассДлинноеИмя02); 

 ALTER TABLE ДочернийКлассДлинноеИмя ADD CONSTRAINT FK1818be36d46dfbb46e4c4511cf465a3dfe8fea56 FOREIGN KEY (MyClass2_m0) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Index1818be36d46dfbb46e4c4511cf465a3dfe8fea56 on ДочернийКлассДлинноеИмя (MyClass2_m0); 

 ALTER TABLE ДочернийКлассДлинноеИмя ADD CONSTRAINT FK39372e37588406730156fbfc58845420f97c106f FOREIGN KEY (MyClass2_m1) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Index39372e37588406730156fbfc58845420f97c106f on ДочернийКлассДлинноеИмя (MyClass2_m1); 

 ALTER TABLE ДочернийКлассДлинноеИмя ADD CONSTRAINT FKd2bc4348adc393a5189d3c7476c05dfd6885b469 FOREIGN KEY (MyClass2_m2) REFERENCES MasterClass; 
CREATE INDEX Indexd2bc4348adc393a5189d3c7476c05dfd6885b469 on ДочернийКлассДлинноеИмя (MyClass2_m2); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK77aa250364c053114540e0b96bccf70f47d3dd31 FOREIGN KEY (MyClass1_m0) REFERENCES ДочернийКлассДлинноеИмя; 
CREATE INDEX Index77aa250364c053114540e0b96bccf70f47d3dd31 on DetailClass (MyClass1_m0); 

 ALTER TABLE DetailClass ADD CONSTRAINT FK3ac0aa6f58aa1aff08d633343b2834bd961caac5 FOREIGN KEY (MyClass1_m1) REFERENCES ДочернийКлассДлинноеИмя2; 
CREATE INDEX Index3ac0aa6f58aa1aff08d633343b2834bd961caac5 on DetailClass (MyClass1_m1); 

 ALTER TABLE DetailClass ADD CONSTRAINT FKca27ff105b842ef6cac7b5e7eba7772c8f74d712 FOREIGN KEY (MyClass1_m2) REFERENCES Класс; 
CREATE INDEX Indexca27ff105b842ef6cac7b5e7eba7772c8f74d712 on DetailClass (MyClass1_m2); 

 ALTER TABLE DetailClass ADD CONSTRAINT FKe789f6f4069ceb440ba166a1896bc86dd8a59f39 FOREIGN KEY (MyClass1_m3) REFERENCES РодительскийКлассДлинноеИмя; 
CREATE INDEX Indexe789f6f4069ceb440ba166a1896bc86dd8a59f39 on DetailClass (MyClass1_m3); 

 ALTER TABLE DetailClass ADD CONSTRAINT FKf01ef28579d81f1402ae693b56505dcefd2521d2 FOREIGN KEY (MyClass1_m4) REFERENCES MyClass; 
CREATE INDEX Indexf01ef28579d81f1402ae693b56505dcefd2521d2 on DetailClass (MyClass1_m4); 

 ALTER TABLE Класс ADD CONSTRAINT FKfea81ad1e26c37dd1ebc8bda5dfc9f660d1305ed FOREIGN KEY (MyClass2_m0) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Indexfea81ad1e26c37dd1ebc8bda5dfc9f660d1305ed on Класс (MyClass2_m0); 

 ALTER TABLE Класс ADD CONSTRAINT FKf6d2577ba3277cd2b1e7b52daa3934ce2007177c FOREIGN KEY (MyClass2_m1) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Indexf6d2577ba3277cd2b1e7b52daa3934ce2007177c on Класс (MyClass2_m1); 

 ALTER TABLE Класс ADD CONSTRAINT FK9d5d8518937c93de853b0a3df3ac15b45c228351 FOREIGN KEY (MyClass2_m2) REFERENCES MasterClass; 
CREATE INDEX Index9d5d8518937c93de853b0a3df3ac15b45c228351 on Класс (MyClass2_m2); 

 ALTER TABLE MyClass ADD CONSTRAINT FK74816bdce3deb319484f6140003df82c77b762fd FOREIGN KEY (MyClass2_m0) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Index74816bdce3deb319484f6140003df82c77b762fd on MyClass (MyClass2_m0); 

 ALTER TABLE MyClass ADD CONSTRAINT FKd8127369ee55dc2097ed4b08708fe4afff5c089d FOREIGN KEY (MyClass2_m1) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Indexd8127369ee55dc2097ed4b08708fe4afff5c089d on MyClass (MyClass2_m1); 

 ALTER TABLE MyClass ADD CONSTRAINT FK77c89e3e96ba71c677881fcf55fb30d26ae1003b FOREIGN KEY (MyClass2_m2) REFERENCES MasterClass; 
CREATE INDEX Index77c89e3e96ba71c677881fcf55fb30d26ae1003b on MyClass (MyClass2_m2); 

 ALTER TABLE DetailClass2 ADD CONSTRAINT FK875a6d0bf601df9ef292680f286b0f7e154b1796 FOREIGN KEY (DetailClass_m0) REFERENCES ДетейлКлассДлинноеИмя; 
CREATE INDEX Index875a6d0bf601df9ef292680f286b0f7e154b1796 on DetailClass2 (DetailClass_m0); 

 ALTER TABLE DetailClass2 ADD CONSTRAINT FK649ba5bbf29aa3c4891521e2112052f310811b1c FOREIGN KEY (DetailClass_m1) REFERENCES ДетейлКлассДлинноеИмя2; 
CREATE INDEX Index649ba5bbf29aa3c4891521e2112052f310811b1c on DetailClass2 (DetailClass_m1); 

 ALTER TABLE DetailClass2 ADD CONSTRAINT FK32445981adb4dffecfc22125b646f42251196089 FOREIGN KEY (DetailClass_m2) REFERENCES DetailClass; 
CREATE INDEX Index32445981adb4dffecfc22125b646f42251196089 on DetailClass2 (DetailClass_m2); 

 ALTER TABLE ДочернийКлассДлинноеИмя2 ADD CONSTRAINT FK72ccad34380fa9a8138470858fb9168c88c13414 FOREIGN KEY (МастерКлассДлинноеИмя01_m0) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Index72ccad34380fa9a8138470858fb9168c88c13414 on ДочернийКлассДлинноеИмя2 (МастерКлассДлинноеИмя01_m0); 

 ALTER TABLE ДочернийКлассДлинноеИмя2 ADD CONSTRAINT FKe177a2dfbc86ccc50ee0b8cd62d549ed06d68d51 FOREIGN KEY (МастерКлассДлинноеИмя01_m1) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Indexe177a2dfbc86ccc50ee0b8cd62d549ed06d68d51 on ДочернийКлассДлинноеИмя2 (МастерКлассДлинноеИмя01_m1); 

 ALTER TABLE ДочернийКлассДлинноеИмя2 ADD CONSTRAINT FKa64728983324d4655fee3eb004d84b0dbe36b59a FOREIGN KEY (МастерКлассДлинноеИмя02) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Indexa64728983324d4655fee3eb004d84b0dbe36b59a on ДочернийКлассДлинноеИмя2 (МастерКлассДлинноеИмя02); 

 ALTER TABLE ДочернийКлассДлинноеИмя2 ADD CONSTRAINT FK21c052b11057689a8144f58330adb8473a76f35a FOREIGN KEY (MyClass2_m0) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Index21c052b11057689a8144f58330adb8473a76f35a on ДочернийКлассДлинноеИмя2 (MyClass2_m0); 

 ALTER TABLE ДочернийКлассДлинноеИмя2 ADD CONSTRAINT FK38d41bfdbf84f871aaed5d0b13b796f52937d75a FOREIGN KEY (MyClass2_m1) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Index38d41bfdbf84f871aaed5d0b13b796f52937d75a on ДочернийКлассДлинноеИмя2 (MyClass2_m1); 

 ALTER TABLE ДочернийКлассДлинноеИмя2 ADD CONSTRAINT FK6bbbaa4b60fe8302a828332adc8478f3aac112b4 FOREIGN KEY (MyClass2_m2) REFERENCES MasterClass; 
CREATE INDEX Index6bbbaa4b60fe8302a828332adc8478f3aac112b4 on ДочернийКлассДлинноеИмя2 (MyClass2_m2); 

 ALTER TABLE РодительскийКлассДлинноеИмя ADD CONSTRAINT FK3fbc6d2b1567480de8bfbdab07ef655d0837f3d2 FOREIGN KEY (MyClass2_m0) REFERENCES МастерКлассДлинноеИмя; 
CREATE INDEX Index3fbc6d2b1567480de8bfbdab07ef655d0837f3d2 on РодительскийКлассДлинноеИмя (MyClass2_m0); 

 ALTER TABLE РодительскийКлассДлинноеИмя ADD CONSTRAINT FK328edfb0a80d816d0740593fb35cc3bb4f377ea0 FOREIGN KEY (MyClass2_m1) REFERENCES МастерКлассДлинноеИмя2; 
CREATE INDEX Index328edfb0a80d816d0740593fb35cc3bb4f377ea0 on РодительскийКлассДлинноеИмя (MyClass2_m1); 

 ALTER TABLE РодительскийКлассДлинноеИмя ADD CONSTRAINT FKc8a01f000e6d3853df28e996be984d75574254fd FOREIGN KEY (MyClass2_m2) REFERENCES MasterClass; 
CREATE INDEX Indexc8a01f000e6d3853df28e996be984d75574254fd on РодительскийКлассДлинноеИмя (MyClass2_m2); 

 ALTER TABLE MasterClass ADD CONSTRAINT FK9b21ec79a1a584907f5995af2d116cb5105bf343 FOREIGN KEY (MasterRoot) REFERENCES MasterRoot; 
CREATE INDEX Index9b21ec79a1a584907f5995af2d116cb5105bf343 on MasterClass (MasterRoot); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FK3f67af9786b09aecd623d68960d38b2165e7a588 FOREIGN KEY (MyClass1_m0) REFERENCES ДочернийКлассДлинноеИмя; 
CREATE INDEX Index3f67af9786b09aecd623d68960d38b2165e7a588 on ДетейлКлассДлинноеИмя (MyClass1_m0); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FKcf9c6765ff8ff2b3301596f3631cad25138bd517 FOREIGN KEY (MyClass1_m1) REFERENCES ДочернийКлассДлинноеИмя2; 
CREATE INDEX Indexcf9c6765ff8ff2b3301596f3631cad25138bd517 on ДетейлКлассДлинноеИмя (MyClass1_m1); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FK45b9c7772adb0cadcf2fd93308e935e3457b6e50 FOREIGN KEY (MyClass1_m2) REFERENCES Класс; 
CREATE INDEX Index45b9c7772adb0cadcf2fd93308e935e3457b6e50 on ДетейлКлассДлинноеИмя (MyClass1_m2); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FK4f6c37452e6b24fab968e00448e5c07ab02eebce FOREIGN KEY (MyClass1_m3) REFERENCES РодительскийКлассДлинноеИмя; 
CREATE INDEX Index4f6c37452e6b24fab968e00448e5c07ab02eebce on ДетейлКлассДлинноеИмя (MyClass1_m3); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FKf3e6c33fadfae073b6cdf2263f1c8277e21d8a2e FOREIGN KEY (MyClass1_m4) REFERENCES MyClass; 
CREATE INDEX Indexf3e6c33fadfae073b6cdf2263f1c8277e21d8a2e on ДетейлКлассДлинноеИмя (MyClass1_m4); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FK718d65f8c6d546050f7a5b88cae1ddcc0f5c1aec FOREIGN KEY (ДочернийКлассДлинноеИмя_m0) REFERENCES ДочернийКлассДлинноеИмя; 
CREATE INDEX Index718d65f8c6d546050f7a5b88cae1ddcc0f5c1aec on ДетейлКлассДлинноеИмя (ДочернийКлассДлинноеИмя_m0); 

 ALTER TABLE ДетейлКлассДлинноеИмя ADD CONSTRAINT FKae1b763aa2b629c8639e395145e2d1b2dba7df8d FOREIGN KEY (ДочернийКлассДлинноеИмя_m1) REFERENCES ДочернийКлассДлинноеИмя2; 
CREATE INDEX Indexae1b763aa2b629c8639e395145e2d1b2dba7df8d on ДетейлКлассДлинноеИмя (ДочернийКлассДлинноеИмя_m1); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FKdf8e57a337fab38888526f4167082a15d0cc6028 FOREIGN KEY (MyClass1_m0) REFERENCES ДочернийКлассДлинноеИмя; 
CREATE INDEX Indexdf8e57a337fab38888526f4167082a15d0cc6028 on ДетейлКлассДлинноеИмя2 (MyClass1_m0); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FK9da909b7c08b123983085d7fdcd0ff6920a22e0e FOREIGN KEY (MyClass1_m1) REFERENCES ДочернийКлассДлинноеИмя2; 
CREATE INDEX Index9da909b7c08b123983085d7fdcd0ff6920a22e0e on ДетейлКлассДлинноеИмя2 (MyClass1_m1); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FK6af18f448fcecf4f5f00b385413b301b74d13a38 FOREIGN KEY (MyClass1_m2) REFERENCES Класс; 
CREATE INDEX Index6af18f448fcecf4f5f00b385413b301b74d13a38 on ДетейлКлассДлинноеИмя2 (MyClass1_m2); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FKe83f86f013aa00136d9d19d2fd4966f6142441cc FOREIGN KEY (MyClass1_m3) REFERENCES РодительскийКлассДлинноеИмя; 
CREATE INDEX Indexe83f86f013aa00136d9d19d2fd4966f6142441cc on ДетейлКлассДлинноеИмя2 (MyClass1_m3); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FKee0db2186ad87bca7be62195121979972f1e6387 FOREIGN KEY (MyClass1_m4) REFERENCES MyClass; 
CREATE INDEX Indexee0db2186ad87bca7be62195121979972f1e6387 on ДетейлКлассДлинноеИмя2 (MyClass1_m4); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FKddb7493d6a06284a041d7247daed16474b1ae259 FOREIGN KEY (ДочернийКлассДлинноеИмя_m0) REFERENCES ДочернийКлассДлинноеИмя; 
CREATE INDEX Indexddb7493d6a06284a041d7247daed16474b1ae259 on ДетейлКлассДлинноеИмя2 (ДочернийКлассДлинноеИмя_m0); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FKb48ea68314179f076d8f174a8e65d5836f390151 FOREIGN KEY (ДочернийКлассДлинноеИмя_m1) REFERENCES ДочернийКлассДлинноеИмя2; 
CREATE INDEX Indexb48ea68314179f076d8f174a8e65d5836f390151 on ДетейлКлассДлинноеИмя2 (ДочернийКлассДлинноеИмя_m1); 

 ALTER TABLE ДетейлКлассДлинноеИмя2 ADD CONSTRAINT FKbca885ab8eb41e4a8f9d1c16bb6de4121d0f7d84 FOREIGN KEY (ДочернийКлассДлинноеИмя2) REFERENCES ДочернийКлассДлинноеИмя2; 
CREATE INDEX Indexbca885ab8eb41e4a8f9d1c16bb6de4121d0f7d84 on ДетейлКлассДлинноеИмя2 (ДочернийКлассДлинноеИмя2); 

 ALTER TABLE МастерКлассДлинноеИмя2 ADD CONSTRAINT FK1b27d519d97e4120d322cfcd6f731f90a94599c2 FOREIGN KEY (MasterRoot) REFERENCES MasterRoot; 
CREATE INDEX Index1b27d519d97e4120d322cfcd6f731f90a94599c2 on МастерКлассДлинноеИмя2 (MasterRoot); 

 ALTER TABLE STORMWEBSEARCH ADD CONSTRAINT FKc4378e39870eb056aec84088683297a01d2a6200 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERDETAIL ADD CONSTRAINT FK921d16269835017e2a0d0e29ad6fb175454a70d0 FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

 ALTER TABLE STORMFILTERLOOKUP ADD CONSTRAINT FKce38ef0db3f01a53acaa49fed8853fb941ad47ba FOREIGN KEY (FilterSetting_m0) REFERENCES STORMFILTERSETTING; 

