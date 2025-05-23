---
title: Protocol - Gesture Action Binding - Triangle
shortTitle: Gesture Action Binding Triangle
author: Joshua Ramirez
status: Canonical
version: v1.1.0
category: Protocol
subcategory: Interaction Model
tags:
  - Gesture
  - Action
  - Binding
  - Signals
  - Update Events
  - UI Flow
created: 2025-04-28
updated: 2025-05-02
summary: Defines the causal sequence that maps user gestures into domain actions and ultimately into safe, signal-authorized binding queries.
architectureModel: Domain-Driven UI - Causal-Dimensional Model
architecturePhilosophy: Causality, update-driven state mutation, signal-driven binding readiness, and declarative rendering separate architectural concerns cleanly.
context: Articulates the structural model connecting UI gestures to domain-driven state changes, update event handling, and UI signal-based binding access.
intendedUse: Align gesture interpretation, domain action dispatch, internal update event handling, and binding structure under a unified causal model.
audience: ChatGPT project for generating UI code
priority: Important
stability: Refined
maturity: Conceptual
relatedDocuments:
  - Protocol - Temporal Projection - Protocol
  - Protocol - UI Signal and Binding Query - Protocol
reviewedBy: Joshua Ramirez
codeIncluded: yes
compliance:
  - R4
  - R5
  - R23
  - NEW-UpdateEvents
  - NEW-Signals
openIssues: []
relatedPatterns:
  - Gesture–Action–Binding Triangle
  - Update-Signal Separation
requires: []
supersedes:
  - v1.0.0
weight: 0
visibility: private
license: MIT
---

# 📙 **Gesture–Action–Binding Triangle**

---

## 📘 Purpose

This document defines the **Gesture–Action–Binding Triangle**, the causal and structural model governing how UI behavior is interpreted, fulfilled by domain actions, updated internally through domain events, and ultimately **bound safely** after **UI Signals** authorize rehydration.

It describes the three critical phases:

```plaintext
[Gesture]
   ↓
[Action]
   ↓
[Binding Query after Signal]
````

Each phase is governed by strict domain boundaries and mediated by the Domain Facade.

---

## 🔺 Triangle Overview

|Vertex|Description|Driven By|
|:--|:--|:--|
|**Gesture**|The user's flat input event — a click, input, or toggle.|UI Client|
|**Action**|The resulting domain behavior — executing semantic logic or structural behaviors.|Feature Domain or Presentation Behaviors|
|**Binding Query (post-Signal)**|UI safely queries Binding trees **only after** receiving a readiness Signal from the Facade.|UI Client|

---

## 🧠 Role of the Interaction Domain

The **Interaction domain** traverses the Gesture–Action–Binding triangle by:

- Accepting gestures from the Facade
    
- Structuring them as meaningful intent
    
- Delegating to either Feature (semantic progression) or Presentation (structural behaviors)
    
- Never executing or reflecting UI state directly
    

✅ Interaction ensures **coherent, non-speculative flow**, without mutating state or emitting signals.

---

## 🧩 Example — Full Causal Flow

```plaintext
[User clicks "Run Prompt"]
 → Facade.StartBatch()
   → Routed to: BatchControlInteraction.StartBatchExecution()
     → Calls Feature.BatchExecutionAggregate.StartExecution()
       → Feature performs computation and invokes Operational tasks
         → Feature emits Update Events (RowProgressUpdated, RowCompleted) internally
           → Presentation subscribes and mutates internal state
             → Facade emits a Signal (RowCompleted)
               → UI listens, receives the Signal
                 → UI queries: facade.Bindings.Rows[rowId]
                   → UI renders updated output
```

✅ No data carried in Signals.  
✅ Update Events mutate Presentation privately.  
✅ UI remains passive, declarative, and causally correct.

---

## 🔁 Mapping Across the Architecture

|Phase|Domain|
|:--|:--|
|**Gesture**|UI → Facade → Interaction|
|**Action**|Interaction → Feature (or Presentation Behavior)|
|**Internal Update**|Feature publishes Update Events → Presentation subscribes and updates internal state|
|**Signal**|Facade emits Signal → UI listens and authorizes binding query|
|**Binding Query**|UI queries Binding tree after Signal received|

---

## 🧠 Architectural Benefits

- ✅ **Causal Purity** — Gestures, Actions, and Bindings are dimensionally separate.
    
- ✅ **Temporal Safety** — UI rendering is gated behind explicit Signals.
    
- ✅ **Declarative UI Reflection** — UI reflects domain structure without speculative binding.
    
- ✅ **Clear Testing Path** — Every phase (Gesture → Action → Update → Signal → Query) is independently testable.
    

---

## 🧭 Enforcement Rules

|Rule|Enforcement|
|:--|:--|
|**G1**|UI must never query Bindings before receiving a Signal.|
|**G2**|Update Events are internal, domain-only — they may carry data for state updates.|
|**G3**|Signals are external, UI-facing — they must carry only minimal identifiers, not data.|
|**G4**|Interaction domain routes all gestures without interpreting or mutating state itself.|
|**G5**|Feature fulfills semantic meaning and emits Update Events internally.|
|**G6**|Presentation updates internal state via Update Event subscriptions, not external commands.|

---

## ✨ Closing Principle

> **The Gesture–Action–Binding Triangle models how imperatives become meaning, meaning becomes stable structure, and structure becomes safe reflection.**

✅ Gestures are raw intent.  
✅ Actions structure domain meaning.  
✅ Update Events evolve domain surfaces internally.  
✅ Signals authorize UI rebinding passively.

Thus, **speculative rendering dies**, and **causal, semantic-driven UI emerges**.

---