# Flexberry ORM Changelog
All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/).

## [Unreleased]

### Added
- `FunctionBuilder` implements `BuildLike(VariableDef, string)` functions.
- `DbTransactionWrapper` class to wrap `IDbConnection` and `IDbTransaction`.
- Interfaces `IConverterToQueryValueString` and `IConvertibleToQueryValueString` to control the conversion of objects to a query string.

### Changed
- ChangesToSqlBTMonitor now split queries by ';'.
- Signatures of the method `GenerateQueriesForUpdateObjects` and its overloads.
- Upgraded Npgsql version to 3.2.6.

### Deprecated

### Removed

### Fixed

- Parsing nullable guids with PKHelper.GetKeys method.
- Getting property storage name when resolving circular dependencies.
- Getting Unity container by replace UnityFactory.CreateContainer to UnityFactory.GetContainer.
- Loading details to delete on deleting aggregator object (using single transaction).
- Appending view properties from not stored prop expression.
- Updating empty array via `SQLDataService.UpdateObjects` (connections remain opened).

### Security


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
