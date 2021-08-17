# install & tune code generation tool
nuget install NewPlatform.Flexberry.Designer.CommandLine -Version 0.1.0-beta07 -OutputDirectory gen

apt-get update
apt-get install xvfb -y

Xvfb -ac :99 -screen 0 1280x1024x16 &

export DISPLAY=:99

# copy existing files to the gen specific folder

mkdir -p /codegen/Flexberry.ORM.Tests/Objects && cp -a /src/NewPlatform.Flexberry.ORM.Test.Objects/* /codegen/Flexberry.ORM.Tests/Objects

mkdir -p /codegen/Flexberry.ORM.Tests/BusinessServers && cp -a /src/NewPlatform.Flexberry.ORM.Tests.BusinessServers/* /codegen/Flexberry.ORM.Tests/BusinessServers

# code generation (magic)

mono ./gen/NewPlatform.Flexberry.Designer.CommandLine.0.1.0-beta07/tools/flexberry.exe ./src/NewPlatform.Flexberry.ORM.IntegratedTests/CodeGen/GenConfig.fdg

# copy generated files back to the source folder

cp -a /codegen/Flexberry.ORM.Tests/Objects/*.cs /src/NewPlatform.Flexberry.ORM.Test.Objects
cp -a /codegen/Flexberry.ORM.Tests/BusinessServers/*.cs /src/NewPlatform.Flexberry.ORM.Tests.BusinessServers
cp -a /codegen/SQL/MSSql.create.sql /src/NewPlatform.Flexberry.ORM.IntegratedTests/SqlScripts/MssqlScript.sql
cp -a /codegen/SQL/Oracle.create.sql /src/NewPlatform.Flexberry.ORM.IntegratedTests/SqlScripts/OracleScript.sql
cp -a /codegen/SQL/PostgreSql.create.sql /src/NewPlatform.Flexberry.ORM.IntegratedTests/SqlScripts/PostgresScript.sql

exit
