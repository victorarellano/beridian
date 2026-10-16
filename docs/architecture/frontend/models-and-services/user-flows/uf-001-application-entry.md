# UF-001 — Application Entry and Period Synchronization

**Status:** Reviewed and approved for models-and-services design.

Shared contracts, types, and mappers are defined in [Shared Frontend Models, DTOs and Mappers](../shared-models.md); they are not duplicated here.

## User-flow objective

On entry, resolve the financial period for the current month and load its complete statement. The backend synchronization operation handles an existing current period or creation of the initial period when no financial history exists. Automatic chronological catch-up for multiple missing periods (**BC-005**) is **deferred beyond the MVP**; the frontend must not simulate that capability.

## Operations and orchestration

| Step | Layer | Operation | Purpose |
|---|---|---|---|
| 1 | `FinancialPeriodPage` / application entry | Trigger loading | Start the entry experience |
| 2 | `FinancialPeriodFacade` | `loadCurrentPeriod()` | Resolve current year/month and coordinate requests |
| 3 | `FinancialPeriodApiService` | `synchronize(request)` | `POST /api/v1/financial-periods/synchronize` |
| 4 | `FinancialPeriodApiService` | `getById(financialPeriodId)` | `GET /api/v1/financial-periods/{financialPeriodId}` |
| 5 | Mapper functions | `mapFinancialPeriod(dto)` | Convert API statement into frontend model |
| 6 | Presentation | Render `FinancialPeriodPage` | Show the resolved period or appropriate feedback |

The synchronization response is an **operation result**, not a frontend entity. The Facade uses its `financialPeriodId` directly to request the statement; no additional synchronization-result model is necessary.

## Angular services and responsibilities

**Data Access — `FinancialPeriodApiService`**

```typescript
@Injectable({ providedIn: 'root' })
export class FinancialPeriodApiService {
  constructor(private readonly http: HttpClient) {}

  synchronize(
    request: SynchronizeFinancialPeriodRequestDto
  ): Observable<SynchronizeCurrentFinancialPeriodResponseDto> {
    return this.http.post<SynchronizeCurrentFinancialPeriodResponseDto>(
      '/api/v1/financial-periods/synchronize', request
    );
  }

  getById(financialPeriodId: string): Observable<FinancialPeriodResponseDto> {
    return this.http.get<FinancialPeriodResponseDto>(
      `/api/v1/financial-periods/${financialPeriodId}`
    );
  }
}
```

Responsibilities: encapsulate HTTP requests and return API contracts. No UI state, business calculations, or application-entry orchestration. The URL configuration shown is conceptual; environment/base URL and HTTP infrastructure are implementation details.

**Application / Facade — `FinancialPeriodFacade`**

```typescript
@Injectable({ providedIn: 'root' })
export class FinancialPeriodFacade {
  constructor(private readonly api: FinancialPeriodApiService) {}

  loadCurrentPeriod(): Observable<FinancialPeriod> {
    const today = new Date();
    return this.api.synchronize({
      year: today.getFullYear(),
      month: today.getMonth() + 1
    }).pipe(
      switchMap(result => this.api.getById(result.financialPeriodId)),
      map(mapFinancialPeriod)
    );
  }
}
```

Responsibilities: resolve the target month/year, orchestrate synchronization and retrieval, and return a frontend `FinancialPeriod`. The browser-local clock in this example is **provisional**: the authoritative timezone for selecting the current financial month remains an explicit open decision. Loading-state ownership and reactive state mechanisms are outside this task's approved scope.

## Error and interface behavior

| Outcome | Expected behavior |
|---|---|
| Synchronization and retrieval succeed | Render the resolved period in `FinancialPeriodPage` |
| Synchronization cannot resolve continuity (`HTTP 409`) | Distinguish the business conflict; explain that automatic synchronization is unavailable |
| Network/server/unexpected-contract failure | Treat as technical failure; show suitable feedback and allow retry when appropriate |

The Facade must not collapse all failures into one undifferentiated error. Detailed error representation and ownership of loading/error state remain for the dedicated state-design task. The presentation must not display an unconfirmed period as ready.

## UF-001 traceability

| Backend capability / UX need | API or frontend responsibility | Design status |
|---|---|---|
| BC-001 — Determine whether history exists | Synchronization API | Covered |
| BC-002 — Resolve current period | `synchronize()` | Covered |
| BC-003 — Create initial period without history | Backend synchronization; `wasCreated` response | Covered |
| BC-004 — Check latest period when relevant | Backend synchronization | Covered |
| BC-005 — Chronological catch-up | Deferred beyond MVP; no frontend implementation | Deferred |
| BC-008 — Prevent duplicate periods | Backend/persistence invariant | Covered by backend |
| Retrieve statement for dashboard | `getById()` and `mapFinancialPeriod()` | Covered |
| Initial loading and error feedback | Presentation + Facade responsibilities | Behavior agreed; state implementation pending |
| Current month timezone reference | Month/year selection in Facade | Open decision |

**UF-001 design review:** closed for the current models-and-services task, with explicit deferred/open items recorded above. This does not mean all frontend implementation or state-management work is complete.
