# Phase 4 — Financial Insights & Simulation

## Objective

Transform the financial information accumulated by Beridian into actionable
insights that help the user understand financial behavior, anticipate future
results, and evaluate possible decisions before modifying the real financial plan.

This phase evolves the application from financial period management into a
decision-support tool.

---

## Business Value

The user can move beyond recording and reviewing financial information and begin
using historical data to:

- understand spending and income behavior;
- compare planned and actual financial results;
- identify recurring deviations;
- anticipate future balances;
- evaluate alternative financial scenarios;
- make informed decisions before committing changes to a real financial period.

---

## Scope

### Financial Insights

The application provides indicators derived from historical and current financial
periods.

Initial insights may include:

- planned income versus actual income;
- planned expenses versus actual expenses;
- planned investment versus actual investment;
- remaining balance evolution;
- expense distribution by category;
- recurring financial deviations;
- comparison between financial periods;
- historical trends by financial concept.

Insights must be generated from existing financial information without modifying
the source FinancialPeriods.

---

### Financial Projections

The application can estimate future financial results based on:

- historical financial periods;
- recurring expenses;
- active fixed-term expenses;
- expected income;
- transferred balances;
- configurable assumptions.

Projections represent expected results and must remain clearly separated from
actual financial execution.

---

### Scenario Simulation

The user can create temporary financial scenarios to evaluate possible decisions.

Examples:

- increasing or reducing an expense;
- adding a new expense;
- removing a discretionary expense;
- changing expected income;
- anticipating the completion of a fixed-term expense;
- evaluating an unexpected financial event;
- comparing different investment alternatives.

A simulation must not modify the original FinancialPeriod or its financial
concepts.

---

### Scenario Comparison

The application allows the user to compare:

- the current financial plan;
- one or more simulated scenarios;
- projected remaining balances;
- projected investment capacity;
- changes in income and expenses;
- financial impact over one or more periods.

The comparison must clearly identify the assumptions used by each scenario.

---

## Main Capabilities

- Generate financial indicators from historical periods.
- Compare planned and actual financial performance.
- Analyze financial evolution across multiple periods.
- Produce projections using historical and planned information.
- Create isolated financial simulations.
- Modify assumptions inside a simulation.
- Compare multiple scenarios.
- Identify the financial impact of each simulated decision.
- Preserve the integrity of real financial periods.

---

## Domain Considerations

This phase introduces concepts that must remain separate from the operational
FinancialPeriod aggregate.

Potential domain concepts include:

```text
FinancialInsight
FinancialProjection
FinancialScenario
ScenarioAssumption
ScenarioResult