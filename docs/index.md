# Flexberry ORM Документация

![Architecture](diagrams/architecture.png)

## Основные компоненты
```mermaid
graph TD
    A[DataService] --> B[MSSQL]
    A --> C[Oracle]
    A --> D[Postgres]
    E[LINQ Provider] --> F[Генерация LCS]
    E --> G[Псевдодетейлы]
    H[Business Servers] --> I[Валидация]
    H --> J[Бизнес-логика]
```

## Содержание
1. [Установка и настройка](installation.md)
2. [Быстрый старт](quickstart.md)
3. [Data Service](dataservice/overview.md)
4. [LINQ-провайдер](linq-provider/basics.md)
5. [Язык ограничений FunctionalLanguage](functional-language.md)
6. [Бизнес-серверы](business-servers/concepts.md)
7. [Dependency Injection (Unity)](di-unity.md)
8. [Система блокировок](locking/theory.md)
9. [Аудит и безопасность](audit/setup.md)
10. [Docker-развертывание](docker/deployment.md)
11. [Примеры использования](examples/crud-operations.md)
