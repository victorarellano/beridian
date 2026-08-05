# <Domain Event Name>

## Purpose

Describe the business fact represented by the event.

Explain why the event exists and what it means from the business perspective.

---

## Lifecycle

Describe when the event is raised within the lifecycle of the originating domain
concept.

```text
<Simple lifecycle diagram>
```

---

## Raised By

Identify the Aggregate, Entity or Domain Service responsible for raising the
event.

### Origin

- Aggregate / Entity / Domain Service

### Operation

```text
Origin.Operation()
```

Describe the conditions that must be satisfied before the event is raised.

---

## Event Contract

Describe the information carried by the event.

| Field | Description |
|--------|-------------|
| ... | ... |

The event should contain only the information required by downstream business
processes.

---

## Interaction

Describe the business reaction triggered by the event.

```text
Origin
    │
    ▼
Domain Event
    │
    ▼
Application
    ├── ...
    ├── ...
    └── ...
```

The interaction diagram focuses on business flow rather than technical
implementation.

---

## Sequence Diagram

```plantuml
@startuml

actor User

participant Application
participant <Origin>
participant EventHandler

...

@enduml
```

The sequence diagram illustrates how the event participates in the use case.

---

## Business Rules

List the business rules related to the event.

- BR-...
- BR-...

---

## Domain Responsibilities

### Domain

Describe the responsibilities owned by the Domain Model.

### Application

Describe the responsibilities owned by the Application layer after receiving the
event.

---

## Future Consumers

Potential consumers of the event.

Examples:

- Audit logging
- Notifications
- Reporting
- Integration events
- Synchronization
- Analytics

This section documents possible future consumers without creating
implementation dependencies.

---

## Notes

Additional observations, assumptions or implementation considerations.

The Notes section should not redefine business rules already documented
elsewhere.