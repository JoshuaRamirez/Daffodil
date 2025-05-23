---
title: Domain - Presentation - Reference
shortTitle: Presentation Domain
author: Joshua Ramirez
status: Canonical
version: v1.1.0
category: Domain
subcategory: Presentation
tags:
  - Declarative UI
  - Bindings
  - UX Local State
  - Update Events
  - Signals
created: 2025-04-28
updated: 2025-04-30
summary: Defines the Presentation domain as a declarative surface for structural UI binding, separated from semantic domain logic, reacting to domain Update Events internally and UI Signals externally.
architectureModel: Domain-Driven UI - Causal-Dimensional Model
architecturePhilosophy: Causality, semantic projection, and declarative rendering separate architectural concerns cleanly.
context: Models renderable state and UX-local workflows in a purely declarative, passive way without semantic processing, updating via internal domain events and UI signals.
intendedUse: Expose safe, framework-agnostic binding surfaces for UI rendering.
audience: ChatGPT project for generating UI code
priority: Foundational
stability: Evolving
maturity: Conceptual
relatedDocuments:
  - Protocol - UI Projection and Event Translation - Protocol
  - Domain - Feature - Reference
  - Protocol - Temporal Projection - Protocol
  - Architecture - Invariants - Manifest
reviewedBy: Joshua Ramirez
codeIncluded: yes
compliance:
  - R5
  - R18
  - R20
  - R28
  - NEW-UpdateEvents
  - NEW-Signals
openIssues: []
relatedPatterns:
  - Triadic Component Model
requires: []
supersedes: []
weight: 0
visibility: private
license: MIT
---

**Author**: Joshua Ramirez  
**Document**: Presentation Domain Reference Ruleset  
**Context**: Domain-Driven UI Architecture — Four-Domain + Facade Model

---

## 📘 Purpose

The Presentation domain defines the structure, lifecycle, and binding behavior of all **UI-renderable state**.  
It operates **purely declaratively**, **updates internal UX-local state privately in response to Update Events**, and **never emits outward signals**.

It reacts to:

- **Update Events** emitted internally by the Feature domain (through the `FeatureEvents` bus).
- **Signals** emitted externally by the Facade to authorize UI rebinds.

✅ **Presentation adapts; it does not originate semantic change.**  
✅ **Presentation reflects domain updates; it does not project meaning.**

---

## 🧀 Domain Role

The Presentation domain:

- Owns **all renderable UI structure and UX-local state**.  
- Exposes **Bindings** for the UI to query after Signals authorize binding.  
- Exposes **Behaviors** for safe, local structural transitions (triggered by the Interaction domain).  
- **Subscribes to FeatureEvents Update Events** to internally mutate private state.  
- **Listens to Facade Signals** externally to allow the UI to query Bindings safely.
- Maintains a **framework-agnostic, declarative, testable model** of the UI.

---

## 🔱 Triad Component Structure

Every Presentation component must implement a **triad** of interfaces:

```csharp
public interface IExampleComponent
{
    IExampleBindings Bindings { get; }
    IExampleBehaviors Behaviors { get; }
}
````

```csharp
public interface IExampleBindings
{
    string Title { get; }
    bool IsVisible { get; }
}
```

```csharp
public interface IExampleBehaviors
{
    void ToggleVisibility();
}
```

Components must expose `Bindings` and `Behaviors` via `this`.

✅ Bindings are readonly, declarative surfaces.  
✅ Behaviors are safe UX-local transitions.

---

## 🧹 Composition and Hierarchy

Bindings and Behaviors are structured as **composable trees**:

- Static composition: child properties.
    
- Dynamic keyed composition: dictionaries by ID.
    

---

## 🔁 Event Handling Pathways

### 1. **Update Event Pathway (Domain-Internal)**

- **Feature** emits an **Update Event** through `FeatureEvents`.
    
- **Presentation** subscribes privately to the Update Event.
    
- Updates internal UX-local state.
    
- No outward emission from Presentation.
    

Example:

```csharp
_featureEvents.SubscribeRowProgressUpdated(OnRowProgressUpdated);

private void OnRowProgressUpdated(string rowId, double percent)
{
    if (_rows.TryGetValue(rowId, out var row))
        row.UpdateProgress(percent);
}
```

✅ Update Events may carry **data**.  
✅ Handlers are **private** inside Presentation components.

---

### 2. **Signal Pathway (UI-Facing)**

- After internal state is stable, **Facade emits a Signal**.
    
- **UI listens to the Signal**.
    
- **UI queries** Bindings and rebinds render state.
    

Example:

```csharp
facade.RowCompleted += rowId => {
    var row = facade.Bindings.Rows[rowId];
    RenderRow(row);
};
```

✅ Signals **do not carry data**.  
✅ Signals **authorize** rebinds.

---

## 🗱 State Management Rules

|Rule|Description|
|:--|:--|
|**P1**|Presentation owns internal renderable and UX-local state.|
|**P2**|State updates via FeatureEvents Update Events and Behavior invocations.|
|**P3**|Public interface = Bindings and Behaviors only.|
|**P4**|UI queries Presentation only after Signal emission by Facade.|
|**P5**|No Presentation→UI event emissions allowed.|
|**P6**|No direct Feature references; subscription is via FeatureEvents only.|

---

## 🛡 Constraints

- ❌ No UI frameworks (`ViewModel`, `INotifyPropertyChanged`, etc.)
    
- ❌ No projection event terminology.
    
- ✅ Update Events are internal and may carry data.
    
- ✅ Signals are external and carry no data.
    
- ✅ Binding structure must be fully declarative and composable.
    

---

## 🧪 Example Usage (Corrected)

```csharp
// Internal domain Update Event handling
_featureEvents.SubscribeRowCompleted(OnRowCompleted);

private void OnRowCompleted(string rowId)
{
    if (_rows.TryGetValue(rowId, out var row))
        row.MarkCompleted();
}

// External UI Signal handling (through Facade)
facade.RowCompleted += rowId => {
    var row = facade.Bindings.Rows[rowId];
    RenderRow(row);
};
```

---

## 🧐 Closing Principle

> **Presentation reflects domain truth internally via Update Events, and enables UI rendering externally after Signals.**

✅ It mirrors, not invents.  
✅ It adapts structurally, not causally.  
✅ It protects causal and temporal integrity.

---
