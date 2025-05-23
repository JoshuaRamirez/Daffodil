---
title: Architecture - Operator - Reference
shortTitle: Operator Reference
author: Joshua Ramirez
status: Canonical
version: v1.1.0
category: Architecture
subcategory: Permissions Model
tags:
  - Permissions
  - Layering
  - Domain Rules
  - Update Events
  - Signals
created: 2025-04-28
updated: 2025-05-01
summary: Defines who may perform which actions across UI, Facade, Interaction, Feature, Presentation, and Operational domains to enforce strict causal separation. Introduces separation between internal Update Events and external UI Signals.
architectureModel: Domain-Driven UI - Causal-Dimensional Model
architecturePhilosophy: Causality, semantic updates, declarative signaling, and structural binding separate architectural concerns cleanly.
context: Serves as a permissions matrix ensuring correct gesture routing, domain authority, update event handling, and signal-driven binding.
intendedUse: Prevent illegal cross-domain calls, enforce Update/Signal separation, and clarify permissible action flows.
audience: ChatGPT project for generating UI code
priority: Foundational
stability: Evolving
maturity: Conceptual
relatedDocuments:
  - Architecture - Invariants - Manifest
  - Protocol - Temporal Projection - Protocol
  - Protocol - UI Projection and Event Translation - Protocol
reviewedBy: Joshua Ramirez
codeIncluded: no
compliance:
  - R4
  - R16
  - R24
  - U1
  - S1
openIssues: []
relatedPatterns:
  - Layered Responsibility Model
  - Update-Signal Causal Split
requires: []
supersedes: []
weight: 0
visibility: private
license: MIT
---

# 🧠 Operator Reference — Domain-Driven UI Architecture (Expanded and Corrected)

**Author**: Joshua Ramirez  
**Document**: Operator Reference Cheat Sheet  
**Context**: Role-Based Contract Enforcement Across Domains

---

## 📘 Purpose

This document defines **who can do what** in the Domain-Driven UI system — and, more importantly, **who must not**.

It preserves:

- Causal integrity
- Update event correctness
- UI signal safety
- Layer separation
- Temporal sequencing

---

## 🆕 Event Types Overview

| Event Type | Purpose | May Carry Data? | Visibility |
|:--|:--|:--|:--|
| **Update Event** | Internal domain-to-domain communication (e.g., Feature ➔ Presentation) | ✅ Yes | Internal only |
| **Signal** | Outbound notification to the UI client (via Facade) | ❌ No (permission only) | Public (UI-facing) |

✅ Update Events allow Presentation to reflect Feature truth.
✅ Signals authorize UI to query Bindings safely after semantic stabilization.

---

## 📚 1. Role Summary

| Actor | Role |
|:---|:---|
| **UI** | Emits gestures and renders bindings after Signals. |
| **Domain Facade** | Routes gestures to Interaction, emits external Signals. |
| **Interaction Domain** | Interprets gestures into semantic or structural domain actions. |
| **Feature Domain** | Fulfills domain logic, publishes Update Events, triggers UI Signals via Facade. |
| **Presentation Domain** | Subscribes to internal Update Events, updates private UX-local state, exposes read-only Bindings. |
| **Operational Domain** | Executes mechanical or asynchronous tasks. |

---

## ✅ 2. May Do / Must Not Do Matrix

| Actor | May Do (Active) | May Do (Passive) | Must Not Do |
|:---|:---|:---|:---|
| **UI** | Emit gestures to Facade | Listen for Signals, traverse Bindings | Call Feature or Presentation directly, query Bindings before Signal |
| **Domain Facade** | Route gestures to Interaction, emit UI Signals | Expose root triadic Bindings | Interpret gestures, modify domain state |
| **Interaction** | Call Feature methods, call Presentation Behaviors | — | Emit Signals, own domain state |
| **Feature** | Mutate domain aggregates, publish Update Events | — | Call Presentation directly, expose mutable state |
| **Presentation** | Subscribe to Update Events, update private state, expose read-only Bindings | — | Emit Signals, expose mutable fields |
| **Operational** | Execute external work, return mechanical outputs to Feature | — | Emit Signals, access Presentation components |

---

## 🛰️ 3. Visibility Matrix

| From → To | Allowed? | Notes |
|:---|:---|:---|
| UI → Facade | ✅ | Flat gesture emission only |
| Facade → Interaction | ✅ | Gesture routing only |
| Interaction → Feature | ✅ | Semantic domain delegation |
| Interaction → Presentation (Behaviors only) | ✅ | Structural UX-local control |
| Feature → Operational | ✅ | Task delegation |
| Feature → Presentation (via FeatureEvents Update Events) | ✅ | Update events subscription only |
| UI → Presentation | ❌ | Forbidden |
| Presentation → Feature | ❌ | Forbidden |
| Operational → Presentation | ❌ | Forbidden |

✅ Only **FeatureEvents** enable legal Feature → Presentation communications.

---

## 🧩 4. Mutability Profile

| Domain | Mutable? | Scope of Mutation |
|:---|:---|:---|
| UI | ❌ | Local view state (e.g., hover, animations) |
| Facade | ❌ | Stateless router |
| Interaction | ❌ | Stateless router |
| Feature | ✅ | Domain aggregates, progress lifecycle |
| Presentation | ✅ | Private UX-local state via Update Event handlers or UX-local Behaviors |
| Operational | ✅ | External task state (e.g., background workers) |

---

## 🚫 5. Forbidden Scenarios

| Forbidden Action | Why Forbidden |
|:---|:---|
| UI calling Presentation directly | Breaks causal binding separation. |
| Feature directly mutating Presentation objects | Violates domain independence. |
| Presentation emitting Signals or Update Events itself | Violates event contract discipline. |
| Facade interpreting gestures itself | Breaks Interaction domain contract. |
| Querying Bindings before receiving Signal | Causes speculative rendering. |
| Presentation exposing mutable fields | Breaks declarative surface purity. |

---

## 🔁 6. Correct Causal and Event Flow

```plaintext
Gesture (UI)
   ↓
Route (Facade)
   ↓
Interpret (Interaction)
   ↓
Execute (Feature or Presentation Behavior)
   ↓
Publish Update Event (FeatureEvents, Feature → Presentation)
   ↓
Internal Binding Update (Presentation)
   ↓
Emit UI Signal (Facade)
   ↓
Listen (UI)
   ↓
Query Bindings (UI)
   ↓
Render (UI)
