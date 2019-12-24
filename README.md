# Flexberry ORM

[![Build Status Master](https://travis-ci.org/Flexberry/NewPlatform.Flexberry.ORM.svg?branch=master)](https://travis-ci.org/Flexberry/NewPlatform.Flexberry.ORM)

[![Build Status Develop](https://travis-ci.org/Flexberry/NewPlatform.Flexberry.ORM.svg?branch=develop)](https://travis-ci.org/Flexberry/NewPlatform.Flexberry.ORM)

В этом репозитории располагается исходный код Flexberry ORM - фреймворка для [объектно-реляционного отображения](https://ru.wikipedia.org/wiki/ORM) для Microsoft .NET Framework.

## Ключевые особенности

* Концепция представлений (проекций).
* Поддержка различных СУБД «из коробки».
* Полная настройка названий таблиц, полей и т.п. в БД.
* Первичные ключи произвольного типа.
* Отображение в БД полей произвольных типов.
* Перехват момента сохранения в БД и выполнение дополнительных действий.
* Широкие возможности по кастомизации, включая возможность управления запросами.
* Поддержка Mono (отсутствие неуправляемого кода).

## Использование

Для работы с Flexberry ORM требуется определение классов объектов данных и готовая БД. Данные классы можно реализовать вручную, но более удобный способ - использовать подход [Model Driven Architecture](https://ru.wikipedia.org/wiki/%D0%90%D1%80%D1%85%D0%B8%D1%82%D0%B5%D0%BA%D1%82%D1%83%D1%80%D0%B0,_%D1%83%D0%BF%D1%80%D0%B0%D0%B2%D0%BB%D1%8F%D0%B5%D0%BC%D0%B0%D1%8F_%D0%BC%D0%BE%D0%B4%D0%B5%D0%BB%D1%8C%D1%8E) и проектировать приложение в UML-редакторе [Flexberry Designer](http://flexberry.ru/Flexberry/ForDevelopers/FlexberryDesigner) с последующей генерацией полноценного кода на C# и SQL-скриптов для создания или модификации структуры таблиц БД. Данный подход не накладывает ограничений на разрабатываемые приложения, а напротив позволяет иметь хотя бы минимальное описание архитектуры приложения в виде UML-диаграмм.
Для установки `Flexberry ORM` в проект следует воспользоваться [NuGet-пакетом](https://www.nuget.org/packages/NewPlatform.Flexberry.ORM/).

## Структура проекта

Данное решение содержит несколько проектов, которые можно условно разделить на следующие категории:

* Ядро ORM - базовые проекты, которые позволяют реализовывать объектно-реляционное отображение
  * `ICSSoft.STORMNET.DataObject` - основной проект, в котором располагаются классы для работы с объектами данных, их связями, проекциями и пр..
  * `ICSSoft.STORMNET.Collections` - реализация дополнительных типов коллекций, которые применяются в других проектах данного решения.
  * `ICSSoft.STORMNET.Business` - основной проект с бизнес-логикой построения SQL-запросов и интерпретации полученных от СУБД результатов.
* Проекты для поддержки конкретных СУБД
  * `ICSSoft.STORMNET.Business.MSSQLDataService` - сервис данных для Microsoft SQL Server (в т.ч. SQL Azure).
  * `ICSSoft.STORMNET.Business.DRDataService` - расширение MSSQLDataService для реализации "грязного чтения" (не блокирующее чтение).
  * `ICSSoft.STORMNET.Business.PostgresDataService` - сервис данных для Postgres.
  * `ICSSoft.STORMNET.Business.OracleDataService` - сервис данных для Oracle DB.
  * `ICSSoft.STORMNET.Business.ODBCDataService` - сервис данных для ODBC-соединений.
* Проекты для поддержки языка запросов (Functional Language, LINQ)
  * `ICSSoft.STORMNET.FunctionalLanguage` - проект с основными структурами встроенного языка запросов.
  * `ExternalLangDef` - расширения для языка запросов, поддерживающие композитную агрегацию в моделях.
  * `ICSSoft.STORMNET.Business.LINQProvider` - проект, с классами, реализующими поддержку LINQ-выражений.
* Вспомогательные проекты
  * `ICSSoft.STORMNET.Tools` - различные вспомогательные классы, например, позволяющие выполнять сериализацию-десериализацию объектов данных и пр..
  * `ICSSoft.STORMNET.UserDataTypes` - дополнительные пользовательские типы данных, расширяющие набор, предлагаемый Microsoft .NET Framework.
  * `CurrentUserService` - проект, в котором определены классы, используемые для определения контекста исполнения - указание на текущего пользователя (применяется как базовый проект в прикладных системах, используется в сервисе пессимистических блокировок и системе полномочий).
  * `ChangesToSqlBTMonitor` - проект, реализующий логику выгрузки выполняемых со стороны сервисов данных SQL-скриптов.
  * `UnityFactory` - проект, реализующий интеграцию с Unity Container - DI.
  * `ICSSoft.STORMNET.Business.LockService` - сервис пессимистических блокировок, позволяет избежать конфликтов при работе нескольких пользователей с одними и теми же данными.
* Проекты для тестов
  * `NewPlatform.Flexberry.ORM.Tests` - проект с автономными тестами.
  * `NewPlatform.Flexberry.ORM.IntegratedTests` - проект с интеграционными тестами (для их исполнения требуются различные СУБД).
  * `NewPlatform.Flexberry.ORM.Tests.Objects` - объекты для проекта с тестами
  * `NewPlatform.Flexberry.ORM.Tests.BusinessServers` - бизнес-логика объектов проекта с тестами.

### Целевая платформа

Поддеживается Microsoft .NET `4.5` и выше, mono `4.6` и выше.

## Тестирование

Тесты разделены на 2 проекта - автономные и интеграционные тесты. Для выполнения интеграционных тестов требуется наличие СУБД: Microsoft SQL, Postgres, Oracle. Соответствующие строки соединения задаются в конфигурационном файле проекта с интеграционными тестами. При выполнении тестов для каждого тестового метода создаётся временная БД (скрипты есть в проекте с интеграционными тестами). Структура данных для тестов сгенерирована при помощи Flexberry Designer, метаданные выгружены в виде crp-файла.

## Документация

Документация разработчика размещается в разделе `Flexberry ORM` на сайте [https://flexberry.github.io](https://flexberry.github.io/ru/fo_landing_page.html).
Автогенерируемая документация по API размещается в ветке `gh-pages` и доступна пользователям по адресу: https://flexberry.github.io/NewPlatform.Flexberry.ORM/autodoc/develop/

## Сообщество

Основным способом распространения `Flexberry ORM` является [NuGet-пакет](https://www.nuget.org/packages/NewPlatform.Flexberry.ORM/). Если во время использования этого фреймворка вы обнаружили ошибку или проблему, то можно завести Issue или исправить ошибку и отправить в этот репозиторий соответствующий Pool Request.

### Доработка

Исправление ошибок приветствуется, технические детали можно выяснить в [чате](https://gitter.im/Flexberry/PlatformDevelopment) или непосредственно в описании Issue.
Добавление новой функциональности рекомендуется согласовывать с авторами, поскольку принятие Pool Request в этом случае может быть затруднено.

### Техническая поддержка

Авторы оставляют за собой право выполнять доработки и исправление ошибок самостоятельно без каких-либо гарантий по срокам. В случае необходимости получения приоритетной технической поддержки с фиксированными сроками, то условия проведения данной работы можно обговорить в частном порядке по [E-Mail](mailto:mail@flexberry.net).

## Ссылки

* [Информация на официальном сайте](http://flexberry.ru/FlexberryORM)
* [Документация](https://flexberry.github.io/ru/fo_landing_page.html)
* [Лицензия (MIT)](LICENSE.md)
* [Лог изменений](CHANGELOG.md)
* [Установить через NuGet](https://www.nuget.org/packages/NewPlatform.Flexberry.ORM/)
* [Gitter чат](https://gitter.im/Flexberry/PlatformDevelopment)
* [E-Mail](mailto:mail@flexberry.net)
