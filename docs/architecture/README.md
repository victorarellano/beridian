# Architecture

## Purpose

This section documents the architectural design of Beridian.

It describes how the system is structured, the architectural principles adopted, the responsibilities of each layer, and the organization of the main architectural components.

The documents contained here focus on **how the software is designed**, not on implementation details or business analysis.

---

## Documents

### Clean Architecture

**File**

`clean-architecture.md`

**Purpose**

Describes the overall architectural style adopted by the solution, including:

- project structure;
- layer responsibilities;
- dependency rules;
- dependency injection;
- composition root;
- communication flow between layers.

---

### Application Layer

**File**

`application-layer.md`

**Purpose**

Documents the internal organization of the **Application** layer.

Topics include:

- feature-based organization;
- Commands;
- Handlers;
- Results;
- repository abstractions;
- interaction with the Domain layer.

---

### Domain Model

**Folder**

`domain-model/`

**Purpose**

Contains the Domain-Driven Design documentation.

Topics include:

- aggregates;
- entities;
- value objects;
- domain services;
- domain events;
- enumerations;
- business rules;
- invariants;
- modeling decisions.

---

### Roadmap

**Folder**

`roadmap/`

**Purpose**

Describes the planned architectural evolution of the project.

It captures future improvements, technical milestones, and long-term architectural direction.

---

## Related Documentation

- `../analysis/`
- `../adr/`
- `../development/`