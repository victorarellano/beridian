# TransferredBalanceStatus

## Purpose

Represents the business validity of a transferred balance.

---

## Values

| Value | Description |
|-------|-------------|
| Provisional | The source FinancialPeriod is still Open and its remaining balance may change. |
| Definitive | The source FinancialPeriod is Closed and its remaining balance is final. |

---

## Lifecycle

```text
Provisional
      │
      ▼
Definitive
```

The reverse transition is not allowed.

---

## Related Business Rules

- BR-006
- BR-007