# Architecture

## Purpose

This section documents the architectural design of Beridian.

It describes how the system is structured, the architectural principles adopted, the responsibilities of each layer, and the organization of the main architectural components.

The documents contained here focus on **how the software is designed**, not on implementation details or business analysis.

---

## Documents

### Clean Architecture

**File**

[Clean Architecture](./clean-architecture.md) 

**Purpose**

Describes the overall architectural style adopted by the solution, including:

- project structure;
- layer responsibilities;
- dependency rules;
- dependency injection;
- composition root;
- communication flow between layers.

---

### Domain Layer

**File**

[Domain Layer](./domain-layer.md)

**Purpose**

Documents the responsibilities and internal organization of the **Domain** layer.

Topics include:

- domain responsibilities;
- dependency restrictions;
- aggregate boundaries;
- encapsulation of business rules;
- domain events;
- interaction with the Application layer;
- domain enforcement principles.

---

### Application Layer

**File**

[Application Layer](./application-layer.md)

**Purpose**

Documents the internal organization of the **Application** layer.

Topics include:

- feature-based organization;
- commands;
- handlers;
- results;
- repository abstractions;
- interaction with the Domain layer.

---

### Infrastructure Layer

**File**

[Infrastructure Layer](./infrastructure-layer.md)

**Purpose**

Documents the responsibilities and organization of the **Infrastructure** layer.

Topics include:

- persistence implementation;
- entity framework core configuration;
- repository implementations;
- database mappings;
- migrations;
- external service integrations;
- dependency injection registration.

---

### API Layer

**File**

[API Layer](./api-layer.md)

**Purpose**

Documents the responsibilities and organization of the **API** layer.

Topics include:

- HTTP endpoints;
- route organization;
- request and response handling;
- API versioning;
- exception handling;
- dependency composition;
- communication with the Application layer.

---

### Domain Model

**Folder**

[Domain Model](./domain-model/)

**Purpose**

Contains the Domain-Driven Design documentation for the Beridian business model.

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

### Project Roadmap

**Folder**

[Project Roadmap](./roadmap/)

**Purpose**

Describes the planned architectural evolution of the project.

It captures future improvements, technical milestones, and long-term architectural direction.

---

## Related Documentation

- [Analysis](../analysis/)
- [Architecture Decision Records](../adr/)
- [Development Guides](../development/)