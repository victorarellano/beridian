# Phase 5 — AI-Assisted Financial Guidance

## Objective

Introduce artificial intelligence capabilities that help the user understand
financial information, explore alternatives, and receive contextual explanations
based on the data already managed by Beridian.

Artificial intelligence acts as an assistant for analysis and decision support.
It does not replace the domain model, define business rules, or modify financial
information autonomously.

---

## Business Value

The user can interact with financial information using natural language and
receive explanations that are easier to understand than raw indicators or tables.

The assistant can help the user:

- understand current financial results;
- identify relevant deviations;
- explore possible causes of financial behavior;
- formulate and compare alternative scenarios;
- understand the impact of possible decisions;
- discover patterns that may not be immediately visible;
- navigate historical financial information more efficiently.

---

## Scope

### Natural-Language Financial Queries

The user can ask questions about financial information using natural language.

Examples:

- Why was my remaining balance lower this month?
- Which expenses exceeded their planned amount?
- How much did I spend on utilities during the last six months?
- Which expenses have increased consistently?
- How has my investment capacity evolved?
- What changed between two financial periods?

The assistant must generate answers from verified application data.

---

### Financial Explanations

The assistant can transform financial indicators into understandable
explanations.

Examples:

- explain the difference between planned and actual expenses;
- summarize the financial result of a period;
- identify the concepts with the greatest impact;
- describe relevant historical trends;
- explain why a projection differs from the current plan;
- explain the assumptions used in a simulation.

Generated explanations must distinguish facts from interpretations.

---

### Assisted Scenario Creation

The user can describe a possible financial situation in natural language.

Example:

> What would happen if my monthly income decreased by CLP 200,000 and my
> electricity expense increased by 15%?

The assistant translates the request into explicit scenario assumptions.

Before executing the simulation, the application must present those assumptions
to the user for review.

The simulation continues to be performed by the deterministic simulation
capabilities introduced in Phase 4.

---

### Financial Pattern Identification

The assistant may identify relevant patterns from historical financial data.

Examples:

- repeated overspending in a category;
- consistent differences between planned and actual amounts;
- seasonal changes in utility expenses;
- progressive reduction of investment capacity;
- completion of fixed-term expenses;
- unusual financial movements.

A detected pattern is an analytical observation, not a new business fact.

---

### Contextual Recommendations

The assistant may present possible actions for the user to evaluate.

Examples:

- review an expense that repeatedly exceeds its plan;
- evaluate reducing a discretionary expense;
- simulate the impact of an upcoming fixed-term expense completion;
- compare different income assumptions;
- reserve part of a positive remaining balance.

Recommendations must be:

- explainable;
- based on available information;
- presented as suggestions;
- subject to user evaluation;
- separated from automatic financial operations.

---

## Main Capabilities

- Query financial information using natural language.
- Generate summaries of financial periods.
- Explain planned-versus-actual deviations.
- Describe historical financial trends.
- Identify relevant financial patterns.
- Translate natural-language requests into scenario assumptions.
- Explain simulation and projection results.
- Present contextual suggestions for user evaluation.
- Reference the financial information supporting each response.
- Preserve user control over every financial decision.

---

## AI Boundaries

Artificial intelligence must not:

- define or replace domain business rules;
- calculate authoritative financial balances;
- modify a FinancialPeriod without explicit user action;
- close or generate financial periods autonomously;
- create definitive financial records from inferred information;
- hide the assumptions behind a recommendation;
- present uncertain interpretations as confirmed facts;
- execute financial transactions;
- recommend specific financial products as authoritative advice.

All authoritative calculations remain under deterministic domain and application
logic.

---

## Domain Considerations

Artificial intelligence is not part of the core financial domain.

The domain remains responsible for:

- financial rules;
- lifecycle transitions;
- invariants;
- balance calculations;
- carry-forward behavior;
- projections and deterministic simulations.

The AI capability consumes structured results produced by the application and
generates explanations or interaction support around them.

Potential application concepts include:

```text
FinancialQuestion
FinancialContext
FinancialExplanation
SuggestedScenario
FinancialObservation
```

Recommendation
