## Scenario SC-001 — First Access Without Financial History

### Purpose

Describe what Beridian must do when the user accesses the application for the first time.

### Trigger

The user opens Beridian.

### Preconditions

- No financial periods exist.
- No financial history has been imported.

### Main Flow

1. The user opens Beridian.
2. The system verifies that no financial periods exist.
3. The system determines the current month and year.
4. The system creates an empty financial period.
5. The system marks the new period as active.
6. The system displays the active financial period.

### Alternative Flows

- If financial history exists, the system must not create an initial period.
- If the period cannot be created, the system informs the user that Beridian could not be initialized.

### Postconditions

- A financial period exists for the current month.
- The period is active.
- The user can begin planning without completing an initial configuration.

### Business Rules

- Only one active financial period may exist.
- Initial financial values start at zero.

### Open Questions

- How should a later spreadsheet import handle the automatically created period?