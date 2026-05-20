# Integration Tests

## Tests run

Before start test run please fill connection string `ConnectionStringPostgres` in `App.config` like this:

```xml
    <add name="ConnectionStringPostgres" connectionString="SERVER=localhost;User ID=postgres;Password=p@ssw0rd;Port=5432;" />
```
Install [Docker](https://docker.com) if it not installed yet and start Docker.

Then start Docker container with PostgreSQL by command from root folder this repository:

```sh
docker-compose up
```

Tests ready to run. Do it now.

When the database is no longer needed for tests, run the command:

```sh
docker-compose down
```

Also perform undo in App.config before commit changes.

## Test objects development

Tests projects use objects from project `NewPlatform.Flexberry.ORM.Tests.Objects` and business logic from `NewPlatform.Flexberry.ORM.Tests.BusinessServers`. Both projects created by flexberry code generator from metadata file `NewPlatform.Flexberry.ORM.IntegratedTests/SqlScripts/NewPlatform.Flexberry.ORM.Tests.crp`.  
Modification test objects made in Flexberry Designer and save into file `NewPlatform.Flexberry.ORM.Tests.crp` and run code generation by command:

```
.\CodeGen\start-code-gen.cmd
```

Important: Manual SQL correction for the DateOnly type
This rule applies if you are updating the CRP for the ICSSoft.STORMNET.Business.PostgresDataService test project. After regenerating the CRP.
Flexberry Designer currently does not support the DateOnly type in .NET 6+. After updating the .crp file and running the code generator, the generated PostgreSQL SQL script will contain incorrect data types for the DateOnly table (for example, TIMESTAMP(3) or an invalid user-defined type instead of DATE).

You must manually replace the DateOnly table creation script in the generated .sql file to ensure that the Attr column uses the correct DATE type. The table definition should look like this:

```
sql

CREATE TABLE DateOnly (
 primaryKey UUID NOT NULL,
 Attr DATE NULL,
 AttrDate DATE NULL,
 AttrString STRING NULL,
 ```