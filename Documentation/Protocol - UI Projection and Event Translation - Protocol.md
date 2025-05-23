---
title: Protocol - UI Signal and Binding Query - Protocol
shortTitle: UI Signal and Binding Query
author: Joshua Ramirez
status: Canonical
version: v1.1.0
category: Protocol
subcategory: UI Signals
tags:
  - UI Signals
  - Event Translation
  - UI Bindings
created: 2025-04-28
updated: 2025-04-29
summary: Defines how flat UI gestures are routed, interpreted, and how binding queries occur via domain-emitted UI signals.
architectureModel: Domain-Driven UI - Causal-Dimensional Model
architecturePhilosophy: Causality, semantic projection, and declarative rendering separate architectural concerns cleanly.
context: Provides the formal contract for how gestures are mapped to domain actions and later trigger UI signals authorizing binding queries.
intendedUse: Support passive, event-driven UI binding via domain facade signals.
audience: ChatGPT project for generating UI code
priority: Foundational
stability: Evolving
maturity: Conceptual
relatedDocuments:
  - Protocol - Temporal Projection - Protocol
  - Architecture - Invariants - Manifest
reviewedBy: Joshua Ramirez
codeIncluded: yes
compliance:
  - R6
  - R8
  - R12
  - R15
openIssues: []
relatedPatterns:
  - Flat Gesture Forwarding
  - Event Translation Layer
requires: []
supersedes: []
weight: 0
visibility: private
license: MIT
---

# 📘 Introduction

This protocol defines how a **declarative, passive UI client** communicates with a structured, semantically layered domain. It governs:

- How UI gestures are emitted and interpreted
- How **UI signals** declare readiness for binding
- How the UI queries binding trees safely after signal receipt

✅ **Signals are emitted by Facade instance events**, not static buses.  
✅ **Signals carry no state** — only readiness.  
✅ **Binding queries occur only after explicit signal receipt.**

---

# 🔁 Core Communication Chain

```plaintext
[User Gesture]
   ↓
UI calls Facade method (e.g., AddEntryRow)
   ↓
Facade routes gesture → Interaction or Presentation
   ↓
If routed to Interaction:
   Interaction interprets → delegates to Feature
   ↓
   Feature executes logic or operational work
   ↓
   Feature emits Update Events internally
   ↓
   Facade emits a UI Signal to indicate readiness
Else if routed to Presentation:
   Presentation executes Behavior
   ↓
   Facade emits a UI Signal
   ↓
In both cases:
   UI listens to Facade instance Signal
   ↓
   UI queries Bindings tree
   ↓
   UI rebinds and renders updated state
````

---

# 🔗 Flat Gesture Emission, Deferred Interpretation

The UI client:

- Emits flat gesture calls (e.g., `StartBatch()` or `ShowModal()`)
    
- Does not know flow structure
    
- Listens only for Facade-emitted **Signals**
    
- Queries bindings _after_ receiving a Signal
    

✅ Gesture emission and flow orchestration are entirely domain internal.

---

# 🔁 Input: Flat Gesture Emission

```csharp
_domainFacade.AddEntryRow();
_domainFacade.ResetBatch();
_domainFacade.ShowSettingsScreen();
```

✅ UI emits intent, not structure.

---

# 🔁 Output: UI Signals (formerly "Projection Events")

UI listens to **UI Signals** emitted by the **Domain Facade instance**.

Signals:

- **Indicate only that readiness has been reached**
    
- **Carry no data** (no binding snapshots, no DTOs)
    
- **Authorize** the UI to query the Bindings tree
    

✅ Signals are **instance events** on the Facade, scoped per domain instance.

---

### Correct Semantic Signal Example

```csharp
_domainFacade.RowCompleted += rowId =>
{
    var row = _domainFacade.Bindings.Rows[rowId];
    RenderRow(row);
};
```

✅ Signal indicates readiness.  
✅ UI queries for data — Signal carries no data itself.

---

### Correct Structural Signal Example

```csharp
_domainFacade.SettingsScreenReady += () =>
{
    var settings = _domainFacade.Bindings.SettingsScreen;
    RenderSettings(settings);
};
```

✅ Structural readiness without embedded payload.

---

> ⚠️ **Key Enforcement:**
> 
> - The UI **must always wait** for the Signal.
>     
> - Only **after** receiving the Signal, the UI queries the Bindings tree.
>     
> - No speculative reads. No premature rendering.
>     

---

# 🧐 Gesture–Action–Binding–Signal Chain

|Stage|Role|
|:--|:--|
|**Gesture**|UI emits flat command to Facade|
|**Action**|Domain (Interaction, Feature, Presentation) fulfills|
|**Binding**|UI queries Bindings|
|**Signal**|Facade emits signal authorizing UI query|

✅ UI is passive until authorization.

---

# 📌 Temporal Readiness Guarantee

Rendering may **only** occur after:

- Semantic/structural state is stabilized
    
- A **Signal** is emitted by the Facade instance
    
- The UI traverses the triadic Bindings tree
    

✅ **Mutation is not rendering. Readiness is rendering.**

---

# 📏 Behavioral Responsibilities

|Role|Description|
|:--|:--|
|**UI**|Emits gestures, listens to Signals, queries Bindings after Signals|
|**Facade**|Routes gestures, emits instance Signals, exposes root Bindings/Behaviors|
|**Interaction**|Interprets gesture intent, dispatches to Feature/Presentation|
|**Feature**|Fulfills semantic meaning, emits Update Events (internally)|
|**Presentation**|Mutates internal render state from Update Events, exposes declarative Bindings|

---

# ✅ Summary: Correct Interaction Cycle

```plaintext
1. UI emits gesture to Facade
2a. Routed to Interaction → Feature → internal Update → Facade emits UI Signal
2b. Or routed to Presentation → Behavior → Facade emits UI Signal
2. UI listens to Signal
3. UI queries Bindings
4. UI renders updated state
```

✅ Fully layered separation.  
✅ Correct causal ordering.  
✅ Event discipline enforced.

---

# ✨ Closing Principle

> **Rendering is not a reflex to mutation.  
> Rendering is a response to a declared Signal of readiness.**

✅ UI waits passively.  
✅ Bindings reflect state declaratively.  
✅ Signals are the only gateway to rendering.

---
