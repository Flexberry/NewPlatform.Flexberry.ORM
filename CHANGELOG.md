# Flexberry ORM Changelog
All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/).

## [Unreleased]

### Added
- `IComparableType` implementation to `WebFile` class.
- Feature GeoDistance is implemented.

### Changed

### Deprecated

### Removed

### Fixed
- Resolve hierarchy references for `Created` master only (removed excess `UPDATE`).
- Update data objects with static properties inherited from DataObject.
- Remove an external context from PutIdentifierIntoBrackets() method implementation in PostgresDataService.
- Fix the return value type of GisExtensions GeoDistance, GeomDistance LINQ gis-functions prototypes.
- Nullable bool isnull limit.
    
### Security

### Performance
- Optimize query (select, update, delete) generation.

## [6.0.0] - 2021-05-25

### Added
- .NET Standard 2.0 implementation. NuGet package contains `net45` and `netstandard2.0` targets.

### Changed
- MSSQLDataService, PostgresDataService, OracleDataService in it's own NuGet packages.
- `ToolXML.DataObject2XMLDocument` uses `ToolBinarySerializer` for serialize `DynamicProperties`.
- `CurrentUserService` support only windows users.
- `DataServiceProvider.IsWebApp` now always return `false`.
- `ChangesToSqlBTMonitor` class moved to `ICSSoft.STORMNET.Business` assembly.
- `DRDataService` class moved to `ICSSoft.STORMNET.Business.MSSQLDataService` assembly.
- Assembly `ExternalLangDef` renamed to `ICSSoft.STORMNET.Business.ExternalLangDef`.
- Assembly `CurrentUserService` renamed to `NewPlatform.Flexberry.ORM.CurrentUserService`.
- Assembly `UnityFactory` renamed to `NewPlatform.Flexberry.ORM.UnityFactory`.

### Removed
- `Information.GetPropertyDataFormat` method.
- `CurrentWebHttpUser` class.
- `ServiceContract` for `IAudit`, `IAuditWcfService` interfaces.
- `RemoteAuditController` class.
- `CurrentUserFromLockService` class.
- `ICSSoft.STORMNET.Business.ODBCDataService` assembly.

### Fixed
- `ControlProviderAttribute(string)` constructor ignores type loading error.
- Multiple concurrency fixes.

## [5.1.3] - 2021-06-25

### Fixed

- Update data objects with static properties inherited from DataObject.

## [5.1.2] - 2021-06-07

### Added

- IComparableType implementation to WebFile class.

## [5.1.1] - 2021-05-17

### Fixed

- Delete agregator with not loaded details.

## [5.1.0] - 2021-04-06

### Added
- `FunctionBuilder` implements `BuildLike(VariableDef, string)` functions.
- `DbTransactionWrapper` class to wrap `IDbConnection` and `IDbTransaction`.
- Interfaces `IConverterToQueryValueString` and `IConvertibleToQueryValueString` to control the conversion of objects to a query string.
- Interfaces `INotifyUpdateObjects`, `INotifyUpdateObject`, `INotifyUpdateProperty` and `INotifyUpdatePropertyByType` for notify when data is updates.
- Support of postgres table suffix and prefix modifiers.
- Differ table modifiers `from` and `join` expressions.
- Property AuditService.DetailedLogEnabled to disable audit log info.
- Interface `IExportStringedObjectViewService` for fast export service based on ObjectStringDataView type.

### Changed
- ChangesToSqlBTMonitor now split queries by ';'.
- Signatures of the method `GenerateQueriesForUpdateObjects` and its overloads.
- Upgraded Npgsql version to 3.2.6.
- Optimize left join with SQLDataService.GenerateSQL methods for some cases.
- Moved group audit from SQLDataService to AuditService.

### Fixed
- Parsing nullable guids with PKHelper.GetKeys method.
- Getting property storage name when resolving circular dependencies.
- Getting Unity container by replace UnityFactory.CreateContainer to UnityFactory.GetContainer.
- Loading details to delete on deleting aggregator object (using single transaction).
- Appending view properties from not stored prop expression.
- Updating empty array via `SQLDataService.UpdateObjects` (connections remain opened).
- Updating array with no changes via `SQLDataService.UpdateObjects` (connections remain opened).
- Incorrect altered state of masters after loading in some cases.
- Setting LoadingState.Loaded to DataObject after loading.
- Objects updating order if exists cycle in dependencies graph of them.
- DbTransactionWrapper commit and rollback over expired connections.
- Null GetHandler or SetHandler via cache dictionary. 
- Rethrowing exception while handling special scenario via UpdateObjects.
- Fix loading __PrimaryKey property of NotStored master.
- Fix InitDataCopy for already loaded details.
- Removed usage of DataServiceProvider.DataService for ExternalLangDef.
- Auditing objects with Unaltered status and Deleted not presented in database.
- Removed memory lock by business server (possible memory leakage).
- Removed caching business server (fix multi-threading).
- Getting new instance of audit data service on every write audit operation.
- Getting inherited business servers.
- Concurrent getting business servers.
- Ordering in PostgresDataService when used RowNumber.

## [5.0.0] - 2018-12-13

### Added

- Simply (Update/Insert style) order for write audit operation.
- Interface `IODataExportService`.
- `Geometry` type support.
- `CheckLoadedProperty` generic extension methods.
- Delegate to check compatible property storage types.
- `PersistUtcDates` property to `AuditService`.
- `PKHelper` and `FunctionBuilder` utilities.

### Changed

- **[BREAKINGCHANGE]** Upgrade `Unity` to 5.x version.
- Upgrade `Npgsql` to 3.x version.

### Removed

- **[BREAKINGCHANGE]** Support for .NET 3.5 and .NET 4.0 has been dropped - minimal version has been upgraded to .NET 4.5.

### Fixed

- Fix for Init Custom AuditService.
- Fix DataService init.
- Fix PostgresDataService wrong access to short names dictionary in multithreading app.
- Fix support multithreading in View.AddProperty method.
- Fix error for inherited aggregator type with same storage.
- Fix empty file saving in `PostgresDataService`.

## [4.1.0] - 2018-02-27
### Added
1. Add support `Microsoft.Spatial.Geography` for Net Framework 4.5.
2. Add support string conversion to `Microsoft.Spatial.Geography` in `Information`.
3. Add `PostgresDataService` method for comparing types.
4. Add support `Nullable<DateTime>` and `NullableDateTime` for `DateTime` properties (Day, Month, Year, etc.).
5. Add property for set connection string by name in DataService. **Need confugure IConfigResolver in Unity config section**
6. Add `ICSSoft.STORMNET.TuneStaticViewDelegate` for tune static Views.

### Fixed
1. Fix LINQ to LCS when sorting with types convert.
2. Fix error when use unsigned types in LINQ expression.
3. Fix using `UnityFactory.CreateContainer` method instead of `UnityFactory.GetContainer`. It should increase performance.
4. Fix update error `Geography` type.
5. Fix converting LINQ expressions with sorting to custom nullable types.
6. Fix sql query sequence for delete hasMany data objects with cyclic associations.

### Changed
1. Remove implicit dependency from `ICSSoft.STORMNET.RightManager`. Now implementation of `ISecurityManager` specified via Unity is used instead.
2. Optimized requests to Postgres if LCS has a populated `RowNumber` property.

## [4.1.1-alpha01] - 2018-03-05
### Added
Add simply(Update/Insert style) order for write audit operation.

### Fixed
Fix for Init Custom AuditService.

## [4.2.0-alpha01] - 2018-05-03
### Changed
Support for .NET 3.5 and .NET 4.0 has been dropped - minimal version has been upgraded to .NET 4.5.
