# Dependency Injection с Unity Container

## Обзор
Flexberry ORM предоставляет встроенную поддержку Dependency Injection через Unity Container. Это позволяет:
- Упростить управление зависимостями
- Повысить тестируемость кода
- Упростить конфигурацию приложения
- Централизовать управление жизненным циклом объектов

## Базовая настройка
1. Установите пакет `NewPlatform.Flexberry.ORM.UnityFactory`
2. Добавьте в Startup.cs:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddUnityFactory(container =>
    {
        // Регистрация сервисов
        container.RegisterType<IDataService, MSSQLDataService>("MSSQL");
        container.RegisterType<IBusinessServer, OrderBS>();
    });
}
```

## Ключевые сценарии использования

### Внедрение DataService
```csharp
public class OrderRepository
{
    private readonly IDataService _dataService;

    public OrderRepository(IDataService dataService)
    {
        _dataService = dataService;
    }
    
    public DataObject[] GetOrders()
    {
        var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Order));
        return _dataService.LoadObjects(lcs);
    }
}
```

### Конфигурация бизнес-серверов
```csharp
container.RegisterType<IBusinessServer, OrderBS>("OrderBS", 
    new ContainerControlledLifetimeManager(),
    new InjectionProperty("AuditService"),
    new InjectionConstructor(typeof(IDataService)));
```

### Работа с несколькими БД
```csharp
container.RegisterType<IDataService, MSSQLDataService>("MainDB");
container.RegisterType<IDataService, OracleDataService>("ArchiveDB");

// Использование
var mainDataService = container.Resolve<IDataService>("MainDB");
var archiveDataService = container.Resolve<IDataService>("ArchiveDB");
```

## Расширенные возможности

### Переопределение зависимостей
```csharp
public class CustomDataService : MSSQLDataService
{
    public CustomDataService(ILogger logger) : base(logger) { }
}

// Регистрация
container.RegisterType<IDataService, CustomDataService>();
```

### Жизненные циклы объектов
```csharp
container.RegisterType<ICacheService, MemoryCacheService>(
    new ContainerControlledLifetimeManager()); // Singleton
container.RegisterType<IValidator, RequestValidator>(
    new TransientLifetimeManager()); // Transient
```

## Лучшие практики
1. Используйте именованные регистрации для разных реализаций
2. Избегайте Service Locator антипаттерна
3. Выносите сложную конфигурацию в отдельные методы
4. Используйте фабрики для создания сложных объектов
5. Регулярно проверяйте граф зависимостей

## Пример полной конфигурации
```csharp
public static void ConfigureUnity(IUnityContainer container)
{
    container
        .RegisterType<IDataService, MSSQLDataService>("MainDB")
        .RegisterType<IBusinessServer, OrderBS>()
        .RegisterType<IAuditService, DatabaseAuditService>(
            new ContainerControlledLifetimeManager())
        .RegisterType<ILogger, FileLogger>(
            new InjectionConstructor("app.log"))
        .RegisterFactory<ICryptoService>(c => new AesCryptoService("secret-key"));
}
```

## Устранение неполадок
**Проблема**: Циклические зависимости  
**Решение**: Используйте интерфейсы и Lazy-инициализацию
```csharp
container.RegisterType<IServiceA>(new InjectionFactory(c => 
    new Lazy<ServiceA>(c.Resolve<ServiceA>()).Value));
```

**Проблема**: Неразрешенные зависимости  
**Решение**: Включите диагностику
```csharp
container.AddExtension(new Diagnostic());
Debug.WriteLine(container.Diagnose());
```

## Ссылки
- [Официальная документация Unity](https://github.com/unitycontainer/unity)
- [Паттерны DI](https://martinfowler.com/articles/injection.html)
- [Тестирование с DI](../testing/unit-tests.md)
