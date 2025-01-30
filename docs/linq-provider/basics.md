# LINQ-провайдер: Основы

## Архитектура преобразования запросов
```mermaid
flowchart LR
    LINQ[LINQ-запрос] --> AST[Абстрактное синтаксическое дерево]
    AST --> LCS[LoadingCustomizationStruct]
    LCS --> SQL[SQL-запрос]
```

## Базовый пример
```csharp
var ds = DataServiceProvider.DataService;
var query = ds.Query<Клиент>()
    .Where(k => k.Возраст > 25)
    .OrderBy(k => k.Фамилия)
    .Select(k => new { k.Фамилия, k.Имя });

var result = query.ToList();
```

## Поддерживаемые операции
- Фильтрация (Where)
- Сортировка (OrderBy/ThenBy)
- Проекции (Select)
- Объединения (Join)
- Группировка (GroupBy)
- Постраничная выборка (Skip/Take)

## Ограничения
1. Нет поддержки локальных коллекций в запросах
2. Ограниченная поддержка сложных математических операций
3. Требуется точное соответствие типов данных

[Подробнее о расширенных возможностях →](../linq-provider/advanced-queries.md)
