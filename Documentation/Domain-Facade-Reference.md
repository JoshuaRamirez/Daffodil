---
title: Domain - Facade - Reference
shortTitle: Domain Facade
author: Joshua Ramirez
status: Canonical
version: v1.1.0
category: Domain
subcategory: Facade Layer
tags:
  - Facade
  - Gesture Routing
  - Signals
  - Domain Interface
created: 2025-04-28
updated: 2025-05-01
summary: Defines the Domain Facade as the exclusive routing membrane between UI gestures and structured domain behavior, ensuring safe signal flow and event-driven binding rehydration.
architectureModel: Domain-Driven UI - Causal-Dimensional Model
architecturePhilosophy: Causality, update-driven mutation, signal-driven readiness, and declarative rendering cleanly separate architectural concerns.
context: Provides the contract for how UI gestures are routed into the domain, signal readiness is emitted, and binding surfaces are safely exposed.
intendedUse: Guarantee a minimal, safe, and declarative gateway between flat UI actions and structured domain state transitions.
audience: ChatGPT project for generating UI code
priority: Foundational
stability: Aligned
maturity: Refined
relatedDocuments:
  - Domain - Interaction - Reference
  - Protocol - UI Signal and Binding Query - Protocol
  - Architecture - Invariants - Manifest
reviewedBy: Joshua Ramirez
codeIncluded: yes
compliance:
  - DF1
  - DF2
  - DF3
  - DF4
  - DF5
  - DF6
  - DF7
  - DF8
  - DF9
openIssues: []
relatedPatterns:
  - Facade Pattern
  - Triadic Binding Exposure
  - Signal Emission Protocol
requires: []
supersedes: []
weight: 0
visibility: private
license: MIT
revisionHistory:
  - v1.1.0 - Updated for Update/Signal separation and corrected event terminology.
---

## 🌟 Purpose

The Domain Facade is the **exclusive entry point** between UI behavior and the domain model.  
It **receives gestures**, **emits Signals** to the UI client after semantic readiness, and **exposes root Binding and Behavior surfaces** for rehydration.

It is the **membrane** separating the passive UI client from the active, causal domain system.

✅ It **holds no domain state**.  
✅ It **does not decide meaning** — it **only routes gestures** and **signals readiness**.

---

## 🧭 Naming Convention

Every domain must include:

|Element|Pattern|Purpose|
|:--|:--|:--|
|Factory|`[DomainName]Domain`|Static entrypoint building the domain|
|Facade|`[DomainName]Facade`|Public interface exposed to the UI|

> 🧹 **Bus classes are no longer used.**  
> **Signals are emitted as instance events** on the Facade itself.

---

## 🔁 Signal Flow Model

All readiness flows follow:

```plaintext
Command → Domain work → Facade emits instance Signal → UI queries Bindings → UI rebinds
````

✅ **No global static buses.**  
✅ **No premature Binding queries** — UI must wait for a Facade Signal.

---

## 🧱 Binding and Behavior Exposure

Facades must expose **root triadic component trees**:

```csharp
public interface IBatchProcessingFacade
{
    IBatchScreenBindings Bindings { get; }
    IBatchScreenBehaviors Behaviors { get; }
}
```

✅ UI traversal begins from the screen root, following structured child compositions.

---

## 🧠 Behavior Routing

Facades expose **gesture methods** that:

- Return `void` or `Task`
    
- Forward flat commands to **Interaction regions**
    
- Let Interaction delegate meaningfully to Feature (semantic) or Presentation (structural)
    

✅ Facade **never interprets gestures**.  
✅ It **never holds state** internally.

---

## 🧩 Signal Emission

Signals emitted by the Facade must:

- Be **instance `event` fields**.
    
- Be **emitted only after domain state stability** is guaranteed.
    
- **Carry only IDs or structural keys**, never domain data.
    

Example:

```csharp
public event Action<string>? RowCompleted;
public event Action<string, double>? GlobalProgressUpdated;
```

When Feature confirms semantic stability:

```csharp
_feature.RowCompleted += id => RowCompleted?.Invoke(id);
```

✅ Signals declare readiness — not state.

---

## 📦 Structural Guidelines

|Rule ID|Description|
|:--|:--|
|DF1|Facade owns and exposes all Interaction region objects|
|DF2|Gesture methods return `void` or `Task` only|
|DF3|Facade does not return domain state directly|
|DF4|Facade emits instance-based Signals after domain readiness|
|DF5|State queries occur through triadic Binding trees|
|DF6|Bindings are authoritative only after Signal emission|
|DF7|Only Interaction may call into Feature or Presentation|
|DF8|Root Bindings and Behaviors are exposed for declarative UI traversal|
|DF9|Facade never interprets or mutates domain logic|

---

## 🧪 Testing Usage

Facades are testable by:

- Emitting flat gestures.
    
- Listening for Facade-emitted Signals.
    
- Querying Binding trees post-signal for correctness assertions.
    

✅ No mocks needed.  
✅ Domain causality testable end-to-end.

---

## 📎 Example Implementation Pattern

```csharp
public class BatchProcessingFacade : IBatchProcessingFacade
{
    private readonly BatchScreenComponent _screen;
    private readonly OutputInteraction _output;
    private readonly EntryInteraction _entry;
    private readonly BatchFeature _feature;

    public event Action<string>? RowCompleted;
    public event Action<string, double>? GlobalProgressUpdated;

    public IBatchScreenBindings Bindings => _screen.Bindings;
    public IBatchScreenBehaviors Behaviors => _screen.Behaviors;

    public BatchProcessingFacade(BatchScreenComponent screen, OutputInteraction output, EntryInteraction entry, BatchFeature feature)
    {
        _screen = screen;
        _output = output;
        _entry = entry;
        _feature = feature;

        // Wire Feature semantic readiness to Facade Signals
        _feature.RowCompleted += id => RowCompleted?.Invoke(id);
        _feature.GlobalProgressUpdated += (id, percent) => GlobalProgressUpdated?.Invoke(id, percent);
    }

    public void AddRow() => _entry.AddRow();
    public void ToggleInsight(string rowId) => _output.ToggleInsight(rowId);
}
```

✅ Signal-based lifecycle.  
✅ Flat gesture routing into Interaction regions.

---

## 🎯 Closing Philosophy

> The Domain Facade is not the creator of truth — it is the **single trusted membrane** that surfaces domain readiness declaratively.

It preserves:

- Causal gesture flow
    
- Temporal readiness enforcement
    
- Declarative Binding exposure
    
- Strict separation between action, meaning, and rendering
    

✅ UI remains passive.  
✅ Domain integrity remains intact.

---
