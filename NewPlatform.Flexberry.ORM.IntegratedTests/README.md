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

Tests projects use objects from project `NewPlatform.Flexberry.ORM.Tests.Objects` and business logic from `NewPlatform.Flexberry.ORM.Tests.BusinessServers`. Both projects created by flexberry code generator from metadata file `NewPlatform.Flexberry.ORM.IntegratedTests/CodeGen/NewPlatform.Flexberry.ORM.Tests.crp`.  
Modification test objects made in Flexberry Designer and save into file `NewPlatform.Flexberry.ORM.Tests.crp` and run code generation by command:

```
.\CodeGen\start-code-gen.cmd
```
