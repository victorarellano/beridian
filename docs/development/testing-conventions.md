# Unit Test Naming Conventions

## Purpose

This document defines the naming convention for unit tests in Beridian. The goal is to make each test name readable as a behavioral specification and to keep the test suite consistent across the project.

The guide is based on two complementary conventions:

| Convention or pattern | Purpose in the test |
| --- | --- |
| **AAA pattern — Arrange, Act, Assert** | Organizes the internal structure of the test method. |
| **Beridian adaptation of the Osherove test naming pattern** | Defines how the test method is named. |

They address different concerns: AAA structures the test implementation, while the Osherove-based convention describes the tested behavior in the method name.

## Test Structure: AAA Pattern

Beridian unit tests follow the **AAA pattern**, which divides each test into three sections:

1. **Arrange:** creates the objects, inputs, and preconditions required by the scenario.
2. **Act:** executes the method or behavior under test.
3. **Assert:** verifies the expected observable result.

```csharp
[Fact]
public void Close_WhenAllItemsAreEntered_ShouldRegisterFinancialPeriodClosedEvent()
{
    // Arrange
    var financialPeriod = CreateFinancialPeriodWithAllItemsEntered();

    // Act
    financialPeriod.Close();

    // Assert
    Assert.Contains(
        financialPeriod.DomainEvents,
        domainEvent => domainEvent is FinancialPeriodClosedEvent);
}
```

AAA is the pattern used to structure the body of a test; it is not the test naming convention.

## Convention Name and Origin

This guide uses a **Beridian adaptation of the Osherove test naming pattern**.

Roy Osherove's original pattern is:

```text
UnitOfWork_StateUnderTest_ExpectedBehavior
```

The Beridian adaptation is:

```text
Method_WhenCondition_ShouldExpectedBehavior
```

The adaptation preserves the same three-part meaning while making the condition and expected behavior read more explicitly:

| Osherove pattern | Beridian adaptation |
| --- | --- |
| `UnitOfWork` | `Method` or behavior under test |
| `StateUnderTest` | `WhenCondition` |
| `ExpectedBehavior` | `ShouldExpectedBehavior` |

Therefore, `Method_WhenCondition_ShouldExpectedBehavior` should not be presented as the formal name of an independent pattern. It is the project-specific form of the Osherove naming pattern.

Reference: [Naming standards for unit tests — Roy Osherove](https://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html)

## Standard Structure

The preferred naming structure is:

```text
Method_WhenCondition_ShouldExpectedBehavior
```

It answers three questions:

1. What behavior or method is being tested?
2. Under what condition is it executed?
3. What result is expected?

Example:

```csharp
Close_WhenAllItemsAreEntered_ShouldCloseFinancialPeriod
```

| Part | Meaning |
| --- | --- |
| `Close` | Method or behavior under test |
| `WhenAllItemsAreEntered` | Scenario or precondition |
| `ShouldCloseFinancialPeriod` | Expected observable behavior |

## Method or Behavior Under Test

The first segment identifies the public operation being exercised.

```csharp
Close
EnterIncome
GenerateNextPeriod
```

Use the actual method name when one method is clearly responsible for the behavior. When the test represents object creation or another behavior that is not exposed through a named method, use a concise domain-oriented term such as `Create`.

## Condition: `When<Condition>`

The condition describes the relevant state, input, or business scenario. It should read as a short sentence after `When`.

```text
When + Subject + Verb + Complement
```

Examples:

```csharp
WhenAllItemsAreEntered
WhenIncomeBelongsToPeriod
WhenIncomeDoesNotBelongToPeriod
WhenFinancialPeriodIsClosed
WhenAmountIsNegative
```

Use grammatical forms consistently:

| Form | Typical use | Example |
| --- | --- | --- |
| `Is` / `IsNot` | Singular state | `WhenAmountIsNegative` |
| `Are` / `AreNot` | Plural state | `WhenAllItemsAreEntered` |
| `Has` / `DoesNotHave` | Possession or presence | `WhenPeriodHasPendingExpenses` |
| `DoesNot` | Negative action or relationship | `WhenIncomeDoesNotBelongToPeriod` |

The condition should include only information that explains why the expected behavior occurs. Incidental setup details should remain in the test body.

## Expected Behavior: `Should<ExpectedBehavior>`

The final segment describes the observable result of the operation.

```text
Should + Base Verb + Object or Result
```

Examples:

```csharp
ShouldCloseFinancialPeriod
ShouldMarkIncomeAsEntered
ShouldTransferRemainingBalance
ShouldRegisterFinancialPeriodClosedEvent
ShouldThrowInvalidOperationException
```

The verb following `Should` must use its base form:

```csharp
ShouldClose       // Correct
ShouldTransfer    // Correct
ShouldThrow       // Correct

ShouldCloses      // Incorrect
ShouldTransfers   // Incorrect
ShouldThrows      // Incorrect
```

Prefer an observable domain behavior over an implementation detail. The name should state what the caller can observe, not how the code internally produces it.

## Is the `When` Segment Optional?

The abbreviated form is valid only when the operation has one relevant scenario and adding a condition would provide no useful distinction:

```text
Method_ShouldExpectedBehavior
```

Example:

```csharp
Create_ShouldInitializeStatusAsOpen
```

However, if the test requires a meaningful business precondition, the condition must be included even when the test is focused on a secondary effect such as a domain event.

Therefore, this abbreviated name:

```csharp
Close_ShouldRegisterFinancialPeriodClosedEvent
```

is understandable, but it hides the condition that permits the period to be closed. In Beridian, the preferred name is:

```csharp
Close_WhenAllItemsAreEntered_ShouldRegisterFinancialPeriodClosedEvent
```

This also distinguishes it from rejection scenarios such as:

```csharp
Close_WhenItemsArePending_ShouldThrowInvalidOperationException
```

## Naming Positive and Rejection Scenarios

Successful behavior:

```csharp
EnterIncome_WhenIncomeBelongsToPeriod_ShouldMarkIncomeAsEntered
```

Rejected behavior:

```csharp
EnterIncome_WhenIncomeDoesNotBelongToPeriod_ShouldThrowInvalidOperationException
```

The condition should explain the business reason for acceptance or rejection. The expected behavior should state the observable outcome.

## Relationship Between the Name and the AAA Pattern

The name maps directly to the test structure:

| Test name segment | Test section | Responsibility |
| --- | --- | --- |
| `WhenCondition` | Arrange | Establish the relevant scenario |
| `Method` | Act | Execute the behavior under test |
| `ShouldExpectedBehavior` | Assert | Verify the observable result |

This correspondence makes the test name a concise description of the same scenario represented by its AAA sections. The exact setup and assertion APIs may vary with the production model; the naming convention remains the same.

## `[Fact]` and `[Theory]`

Use `[Fact]` for one specific scenario:

```csharp
[Fact]
public void Close_WhenItemsArePending_ShouldThrowInvalidOperationException()
```

Use `[Theory]` when the same rule is verified with multiple input values:

```csharp
[Theory]
[InlineData(-1)]
[InlineData(-100)]
public void Create_WhenAmountIsNegative_ShouldThrowArgumentOutOfRangeException(
    decimal amount)
```

Changing test data does not require changing the naming structure when the business condition and expected behavior remain the same.

## Project Rules

1. Use English and `PascalCase`.
2. Prefer `Method_WhenCondition_ShouldExpectedBehavior`.
3. Omit `WhenCondition` only when there is no meaningful scenario to distinguish.
4. Describe conditions as short grammatical sentences.
5. Use the base verb form after `Should`.
6. Name the observable behavior, not the internal implementation.
7. Use domain language already established in Beridian.
8. Keep one behavioral purpose per test.
9. Use the same terminology in the test name, production code, and domain documentation.

## Review Checklist

Before accepting a test name, verify:

- Can the name be understood without reading the test body?
- Does it identify the operation under test?
- Does it include every business condition necessary to explain the outcome?
- Does the expected result start with `Should` followed by a base-form verb?
- Does it describe observable behavior?
- Does it use the same terms as the domain model?
- Can it be clearly distinguished from success and rejection scenarios for the same operation?

If these questions can be answered positively, the test name acts as an executable specification of the expected domain behavior.
