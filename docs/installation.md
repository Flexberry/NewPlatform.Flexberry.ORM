# Установка и настройка

## Требования
- .NET 6.0+
- Поддерживаемые СУБД:
  - Microsoft SQL Server 2012+
  - Oracle Database 11g+
  - PostgreSQL 9.4+

## Установка через NuGet
```powershell
Install-Package NewPlatform.Flexberry.ORM
Install-Package NewPlatform.Flexberry.ORM.MSSQLDataService
# или для других СУБД:
Install-Package NewPlatform.Flexberry.ORM.OracleDataService
Install-Package NewPlatform.Flexberry.ORM.PostgresDataService
```

## Базовая конфигурация
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddFlexberryORM(config =>
    {
        config.SetDataServiceProvider(() => new MSSQLDataService());
        config.UseUnityFactory();
        config.EnableAudit();
    });
}
```

## Проверка установки
```csharp
var dataService = DataServiceProvider.DataService;
var version = dataService.GetServerVersion();
Console.WriteLine($"Connected to {version}");
```

[Далее: Быстрый старт →](quickstart.md)
