# ADR-004: Adopt Explicit API Versioning

- **Status:** Accepted
- **Date:** 2026-08-17

---

## Context

Beridian exposes its application use cases through an HTTP API.

Although the initial MVP has no external consumers, the API contract will evolve as new capabilities are introduced. Some future changes may be backward-compatible, while others may modify routes, request structures, response structures, validation rules, or endpoint behavior.

Introducing endpoints without an explicit version would make future breaking changes harder to manage. Existing clients could be affected without a clear migration path, and multiple contract versions could not be represented consistently in routing or OpenAPI documentation.

API versioning must therefore be established before the first public endpoint is implemented.

## Decision

Beridian will use explicit API versioning from the first MVP version.

The API will use the `Asp.Versioning` libraries for ASP.NET Core:

* `Asp.Versioning.Http` for Minimal API versioning.
* `Asp.Versioning.Mvc.ApiExplorer` for version-aware API discovery and OpenAPI documentation.

Versions will be represented as URL path segments.

The initial API version will be:

```text
v1
```

Versioned routes will follow this format:

```text
/api/v{version}/resource
```

For example:

```text
POST /api/v1/financial-periods
```

Endpoint route templates will use the API version constraint:

```text
/api/v{version:apiVersion}/financial-periods
```

Each endpoint group must declare:

* Its supported API versions.
* The version implemented by each endpoint.
* Its version-aware route.
* Its OpenAPI group.

Clients must provide the API version explicitly in the URL. Beridian will not assume a version when the version is omitted.

The API will report supported and deprecated versions through HTTP response headers.

OpenAPI documents and Swagger UI entries will be generated separately for each supported API version.

## Versioning Policy

### Compatible Changes

Backward-compatible changes will remain within the current API version.

Examples include:

* Adding an optional request property.
* Adding a response property that existing clients can ignore.
* Adding a new endpoint.
* Adding a new optional query parameter.
* Correcting an internal implementation without changing the HTTP contract.
* Improving performance, logging, security, or persistence behavior without changing observable API behavior.

### Breaking Changes

A new API version will be created when a change is incompatible with existing clients.

Examples include:

* Removing or renaming a request property.
* Removing or renaming a response property.
* Changing the type or meaning of an existing property.
* Changing a route or HTTP method.
* Making an optional property mandatory.
* Changing status-code behavior in a way that affects existing clients.
* Changing validation rules in a way that rejects previously valid requests.
* Replacing an existing response structure.

### Version Coexistence

Multiple API versions may remain active simultaneously.

When a new breaking version is introduced:

1. The new version will be added without immediately removing the previous version.
2. Existing clients may continue using the previous version during a migration period.
3. The previous version may be marked as deprecated.
4. Supported and deprecated versions will be reported in HTTP headers.
5. Each version will retain separate OpenAPI documentation.
6. Removal will occur only after the migration period has been documented and completed.

### Deprecation

Deprecation does not mean immediate removal.

A deprecated version must:

* Continue operating during its announced transition period.
* Be identified as deprecated through API version metadata.
* Appear in the reported deprecated-version headers.
* Have a documented replacement version.
* Have a documented removal or review date before deletion is considered.

## Version Number Format

Beridian will represent API versions using major version numbers in public URLs:

```text
v1
v2
v3
```

Internally, the versioning library may represent these as:

```text
1.0
2.0
3.0
```

Minor implementation releases will not create new public API versions unless they introduce a breaking contract change.

API versions will not be created for ordinary feature releases, bug fixes, or internal refactoring.

## OpenAPI and Swagger

Each supported API version will have an independent OpenAPI document.

Expected document groups include:

```text
v1
v2
```

Swagger UI will expose one selectable document per supported version.

Version substitution will replace the version placeholder in generated routes so that OpenAPI displays concrete paths such as:

```text
/api/v1/financial-periods
```

rather than:

```text
/api/v{version}/financial-periods
```

## Alternatives Considered

### No API Versioning

The API could initially expose unversioned routes and introduce versioning only when a breaking change appears.

This option was rejected because it would require changing every existing route after clients had already started consuming the API.

### Manually Hard-Coded Version Routes

Routes could include `/api/v1` without using a versioning library.

This option was rejected because a hard-coded route identifies a version but does not provide version metadata, deprecation management, version-aware endpoint selection, supported-version reporting, or versioned API exploration.

### Query String Versioning

Example:

```text
/api/financial-periods?api-version=1.0
```

This option was rejected because the API version is less visible in route structure and generated documentation.

### HTTP Header Versioning

Example:

```http
api-version: 1.0
```

This option was rejected because version discovery and manual testing are less straightforward, and the selected version is not visible in the resource URL.

### Media Type Versioning

Example:

```http
Accept: application/vnd.beridian.v1+json
```

This option was rejected because it introduces unnecessary complexity for the current API and its expected clients.

## Consequences

### Positive Consequences

* API contracts are explicitly versioned from the first endpoint.
* Breaking changes can be introduced without immediately disrupting existing clients.
* Multiple versions can coexist during migration periods.
* Supported and deprecated versions can be reported consistently.
* Swagger and OpenAPI documentation can be separated by version.
* Route organization remains predictable.
* Version lifecycle rules are documented before external consumers exist.

### Negative Consequences

* API configuration becomes more complex.
* Every endpoint must declare its version metadata correctly.
* Swagger configuration must account for multiple documents.
* Multiple active versions may require duplicated endpoint mappings or translation logic.
* Deprecated versions may increase maintenance and testing effort.
* Version removal requires an explicit lifecycle decision.

## Implementation Guidelines

The API startup configuration must:

* Register API versioning services.
* Use URL segment version reading.
* Require explicit versions.
* report supported and deprecated versions.
* Register the version-aware API Explorer.
* Substitute API versions in generated URLs.
* Generate one OpenAPI document per API version.

Endpoint groups must follow this general structure:

```csharp
var versionSet = endpoints
    .NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

var group = endpoints
    .MapGroup("/api/v{version:apiVersion}/financial-periods")
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(new ApiVersion(1, 0));
```

The initial financial-period endpoint will therefore be exposed as:

```text
POST /api/v1/financial-periods
```

Versioning concerns belong to the API layer. Domain and Application projects must remain independent of HTTP API versions.

## Compliance

Future endpoint changes must be reviewed against this ADR.

Before changing an existing HTTP contract, the team must determine whether the change is:

* Backward-compatible and can remain in the current version.
* Breaking and requires a new API version.
* A deprecation requiring a documented transition period.

Any future change to the versioning strategy must be recorded in a new ADR that supersedes this decision.



## Separation of Version Types

API contract versions are independent from application releases and database schema versions.

Beridian distinguishes between three version types:

| Version Type        | Example           | Purpose                                                   |
| ------------------- | ----------------- | --------------------------------------------------------- |
| Application release | `0.2.0`           | Identifies a released version of the Beridian application |
| API contract        | `v1`              | Identifies a compatible HTTP contract                     |
| Database schema     | EF Core migration | Identifies an incremental database schema change          |

Multiple application releases may continue implementing the same API contract.

For example:

```text
Beridian 0.1.0 → API v1
Beridian 0.2.0 → API v1
Beridian 0.3.0 → API v1
Beridian 1.0.0 → API v1
Beridian 1.4.0 → API v1 and API v2
```

Deploying a new application release does not automatically create a new API version.

Database migrations also do not create API versions unless the database change results in an incompatible change to the public HTTP contract.

Supported API versions must be declared in source code and associated explicitly with their endpoint implementations. They must not be created dynamically from runtime configuration because configuration could advertise a contract that the deployed code does not implement.

Operational information, such as a future deprecation or retirement date, may be provided through configuration when required. The existence and implementation of an API version remain source-code decisions.

## Application Release Versioning

Beridian application releases will follow Semantic Versioning:

```text
MAJOR.MINOR.PATCH
```

During MVP development, releases may use the `0.x.y` range:

```text
0.1.0
0.2.0
0.2.1
```

The application release version changes independently from the API contract version.

The release components have the following meaning:

* `MAJOR`: an incompatible change to the released product or a new stable product generation.
* `MINOR`: new backward-compatible functionality.
* `PATCH`: backward-compatible corrections.

A change to the application major version does not necessarily require a new API version. A new API version is required only when the HTTP contract becomes incompatible with existing clients.

## Change Registration

Notable product and API changes will be recorded in the root `CHANGELOG.md`.

The changelog will maintain an `Unreleased` section with the following categories:

```markdown
## [Unreleased]

### Added

### Changed

### Deprecated

### Removed

### Fixed

### Security
```

Changes will be registered according to these rules:

* New compatible endpoints and optional contract additions are recorded under `Added`.
* Compatible modifications to existing behavior are recorded under `Changed`.
* Versions or contract elements scheduled for retirement are recorded under `Deprecated`.
* Contract elements removed in a new API version are recorded under `Removed`.
* Defect corrections are recorded under `Fixed`.
* Security-related corrections are recorded under `Security`.

When an application release is created, the accumulated `Unreleased` entries will move to a dated release section:

```markdown
## [0.2.0] - 2026-08-17
```

If a change introduces a new API contract version, its changelog entry must explicitly identify:

* The new API version.
* The incompatible contract changes.
* The version being replaced or deprecated.
* The expected client migration path.
* The continued availability or planned retirement of the previous version.

Git tags and release records should use the application release version, not the API contract version.

Examples:

```text
v0.1.0
v0.2.0
v1.0.0
```

OpenAPI documents provide the technical contract for each API version, while `CHANGELOG.md` records how the product and its public contract evolve between application releases.
