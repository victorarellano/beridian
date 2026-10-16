# Beridian — Frontend Models and Services

**Version:** 0.1 — UF-001 design baseline  
**Status:** In progress — UF-001 approved; remaining user flows pending  
**Sprint:** 3 — Financial Period Experience  
**Session:** 003 — Frontend Architecture and API Contract Review  
**Task:** Define frontend models and services

## Documents

- [Shared models, API DTOs and mappers](shared-models.md) — reusable types, enums, mappings, and conventions in code.
- [UF-001 — Application Entry and Period Synchronization](user-flows/uf-001-application-entry.md) — orchestration, API service, facade, errors, and traceability.
- **UF-002 — Monthly Financial Period Management:** pending analysis; document not created yet.

## 2. Approved design conventions

1. API transport contracts use TypeScript `interface` declarations, with names ending in `Dto`.
2. Frontend-facing models use TypeScript `interface` declarations. Classes are reserved for cases requiring behavior.
3. API DTOs and frontend models are separate when the frontend model has a justified application/presentation responsibility. Do not create mirror models and mappers automatically for every DTO.
4. Closed sets of API values use string-valued TypeScript enums matching the **actual serialized API values**, not the numeric backing values of C# domain enums.
5. Frontend enums remain distinct from transport enums. Convert between them using explicit, exhaustive `Record<SourceEnum, TargetEnum>` lookup tables, with runtime rejection of unrecognized values.
6. Mappers are pure functions, with no HTTP access, Angular dependencies, input mutation, or business calculations. Composite mappers reuse smaller mappers.
7. Unexpected API contract values must fail explicitly; do not silently substitute default enum values.
8. C# `DateOnly` values are transported as `YYYY-MM-DD` strings (or `null` when nullable). Do not automatically parse them with JavaScript `Date` or apply timezone conversion. Format calendar dates without changing the underlying day.
9. Backend/domain owns financial calculations, invariants, persistence, and duplicate-period prevention. The frontend owns orchestration, input feedback, and presentation.
10. Mapping automation and OpenAPI-generated client tooling may be evaluated later, after the manually designed contracts and transformations are understood.

## 4. Pending user flows and design decisions

| Area | Status |
|---|---|
| UF-001 — Application Entry and Period Synchronization | **Reviewed and approved** |
| UF-002 — Monthly Financial Period Management | **Next**: navigation and mutation request/response DTOs, service operations, orchestration |
| UF-003 — Financial Period Closing | Pending |
| UF-004 — Financial Period Continuity | Pending; respect MVP deferrals |
| Angular state ownership / mechanisms | Separate design task |
| Timezone determining the current financial month | Open decision |
| Precise TypeScript folder paths / exports | Pending review against domain/feature architecture |
| DTO response runtime validation | Not selected; evaluate if needed |
| Mapping/client code generation | Future evaluation |

## 5. Relationship to frontend architecture

This document refines the approved **Presentation → Application / Facade → Data Access** responsibilities in `frontend-architecture.md`. It does not supersede the approved component hierarchy, specify final Angular folder structure, or authorize frontend implementation. The API remains authoritative for financial calculations, financial-period continuity constraints, and closed-period modification rules.
