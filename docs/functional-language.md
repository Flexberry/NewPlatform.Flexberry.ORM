# Язык ограничений FunctionalLanguage

## Обзор
Класс `SQLWhereLanguageDef` предоставляет функционал для построения SQL-запросов через объектную модель. 

## Поддерживаемые типы атрибутов
```csharp
BoolType       // Логический (bool)
NumericType    // Числовой (int, long, decimal)
StringType     // Строковый (string)
DateTimeType   // Дата и время (DateTime)
GuidType       // Уникальный идентификатор (Guid)
QueryType      // Произвольное SQL выражение
```

## Основные функции

### Логические операции
```csharp
funcAND  // И (AND)
funcOR   // ИЛИ (OR)
funcNOT  // НЕ (NOT)
```

### Операции сравнения
```csharp
funcEQ    // Равно (=)
funcNEQ   // Не равно (<>)
funcL     // Меньше (<)
funcLEQ   // Меньше или равно (<=)
funcG     // Больше (>)
funcGEQ   // Больше или равно (>=)
```

### Специальные операторы
```csharp
funcLIKE     // Поиск по маске
funcIN       // Вхождение в список
funcBETWEEN  // Диапазон значений
funcSQL      // Произвольный SQL
```

## Примеры запросов

### Простое сравнение
```csharp
var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Клиент), Клиент.Views.КлиентE);
lcs.LimitFunction = SQLWhereLanguageDef.LanguageDef.GetFunction(
    SQLWhereLanguageDef.funcEQ,
    new VariableDef(SQLWhereLanguageDef.StringType, Information.ExtractPropertyPath<Клиент>(x => x.Фамилия)),
    "Иванов");
```

### Поиск по маске
```csharp
var func = SQLWhereLanguageDef.LanguageDef.GetFunction(
    SQLWhereLanguageDef.funcLIKE,
    new VariableDef(SQLWhereLanguageDef.StringType, Information.ExtractPropertyPath<Клиент>(x => x.Телефон)),
    "+7-912-*");
```

### Комбинированные условия
```csharp
var ageCondition = SQLWhereLanguageDef.LanguageDef.GetFunction(
    SQLWhereLanguageDef.funcBETWEEN,
    new VariableDef(SQLWhereLanguageDef.NumericType, Information.ExtractPropertyPath<Клиент>(x => x.Возраст)),
    18,
    30);

var cityCondition = SQLWhereLanguageDef.LanguageDef.GetFunction(
    SQLWhereLanguageDef.funcIN,
    new VariableDef(SQLWhereLanguageDef.StringType, Information.ExtractPropertyPath<Клиент>(x => x.Город)),
    new[] { "Москва", "Санкт-Петербург" });

var finalFunc = SQLWhereLanguageDef.LanguageDef.GetFunction(
    SQLWhereLanguageDef.funcAND,
    ageCondition,
    cityCondition);
```

## Особенности работы

### Регистронезависимый поиск
```csharp
var langDef = new SQLWhereLanguageDef { CaseInsensitive = true };
// Автоматически добавляет UPPER к строковым полям
```

### Настройка масок LIKE
```csharp
// По умолчанию:
// * -> %
// _ -> _
langDef.UserLikeAnyStringSymbol = "*";
langDef.QueryLikeAnyStringSymbol = "%";
```

### Оптимизация IN
```csharp
// При одном элементе в списке преобразуется в =
SQLWhereLanguageDef.OptimizeINOperator = true;
```

## Генерация SQL
Для преобразования функции в SQL-строку:
```csharp
string sql = SQLWhereLanguageDef.ToSQLString(
    function, 
    value => $"'{value}'", // Конвертер значений
    identifier => $"[{identifier}]"); // Обрамление идентификаторов
```

## Расширенные возможности
- Поддержка пользовательских SQL-выражений через `funcSQL`
- Работа с NULL-значениями через `funcIsNull`
- Комбинирование условий любой сложности
- Поддержка параметризованных запросов
