---
title: Architecture - Domain Construction and Naming - Guidelines
shortTitle: Domain Construction and Naming
author: Joshua Ramirez
status: Canonical
version: v1.1.0
category: Architecture
subcategory: Standards
tags:
  - Naming
  - Construction
  - Architecture
  - Signals
  - Update Events
created: 2025-04-28
updated: 2025-05-01
summary: Defines standards for domain factory creation, facade exposure, signal event surfaces, and naming conventions.
architectureModel: Domain-Driven UI - Causal-Dimensional Model
architecturePhilosophy: Causality, update-driven state mutation, signal-driven binding readiness, and declarative rendering separate architectural concerns cleanly.
context: Provides structural standards for wiring, naming, event surface separation, and binding exposure within domain modules.
intendedUse: Standardize domain assembly, facade contract enforcement, and internal vs external event modeling.
audience: ChatGPT project for generating UI code
priority: Important
stability: Canonical
maturity: Refined
relatedDocuments:
  - Architecture - Invariants - Manifest
reviewedBy: Joshua Ramirez
codeIncluded: yes
compliance:
  - R37
  - R38
  - R40
openIssues: []
relatedPatterns:
  - Factory Pattern
  - Signal Surface Composition
requires: []
supersedes: []
weight: 0
visibility: private
license: MIT
---

# 📘 Domain Structure and Composition Reference

---

## 📘 Purpose

This document defines the standards for:

- Domain entrypoint structure
- Naming conventions for core architectural artifacts
- Manual object graph construction (without dependency injection)
- Instance-based **Signal** event integration for UI readiness

It ensures every domain is **predictable**, **causally coherent**, and **safe to construct** in any runtime context.

---

## 📦 Naming Convention

Every domain must declare the following components:

|Role|Naming|Description|
|:--|:--|:--|
|Factory Root|`[DomainName]Domain`|Static `Create()` method wiring dependencies and returning the domain instance|
|UI Gateway|`[DomainName]Facade`|UI-facing contract for gestures, Signals, and Binding tree exposure|
|Event Surface|Signals emitted by `[DomainName]Facade`|Instance-based events authorizing UI binding — no global buses|

---

## 🧱 `[DomainName]Domain` — Composition Root

Each domain’s static `Create()` method must:

- Construct all triadic Presentation components
- Construct Feature aggregates
- Construct Interaction regions
- Wire internal Update Event and external Signal flows
- Return the `[DomainName]Domain` instance

✅ Explicit, causal, fully layered construction.

---

## 📡 Instance-Based Signal Events

Signals are defined as **instance fields on the Facade** — not static buses.

This allows:

- Feature → Facade event translation at instance scope
    
- UI → Facade subscription cleanly scoped to domain instance
    
- No global event surfaces or cross-instance contamination
    
- Full domain testability and portability
    

✅ Facade events exist **per domain instance**.  
✅ UI binds and listens **only to active domain instances**.

---

## 🎛 Facade Wiring Responsibilities

The Facade must:

- Accept fully constructed Presentation, Interaction, Feature, and Operational domains
    
- Expose root triadic `Bindings` and `Behaviors`
    
- Delegate all gestures to Interaction region methods
    
- Emit UI-facing **Signals** after internal stability is guaranteed
    
- Maintain no mutable domain state
    

The Facade must **not**:

- Interpret gestures itself
    
- Construct or own domain logic
    
- Return semantic state from gesture methods
    

---

## 📚 Manual Construction Philosophy

This architecture prefers **explicit causal construction** to dependency injection frameworks.

|Principle|Description|
|:--|:--|
|Declarative Construction|Domain object graph wired in `Create()` method|
|Causal Layering|Presentation → Interaction → Feature → Operational|
|Parameterless Defaults|Constructors favor no arguments unless structural constraints exist|
|Domain Return|`Create()` returns the full, composed domain instance|
|Composable Layers|Each domain owns only its own immediate structures|
|DI Agnostic|Compatible with production and test scenarios|

---

## 🔐 Composition Safety

By following this model:

- Domains are portable, modular, and easily testable
    
- No framework-specific behavior is embedded
    
- Signals correctly separate internal and external event surfaces
    
- UI binding remains passive, event-driven, and structurally declarative
    

---

## 📏 Structural Ruleset

|ID|Rule|
|:--|:--|
|DC1|Each domain must expose `[DomainName]Domain.Create()`|
|DC2|All triadic components must be explicitly constructed|
|DC3|Signals must be instance events on the Facade|
|DC4|No static event buses are allowed|
|DC5|No event surfaces defined inside domain model classes|
|DC6|Creation order: Presentation → Interaction → Feature → Operational|
|DC7|Facade exposes root Bindings and Behaviors only|
|DC8|Facade delegates all gestures via Interaction, never interpreting|
|DC9|Gesture methods return only `void` or `Task`, never state|
|DC10|Signals must authorize UI binding after domain stability|

---

## ✅ Outcome

Following this model ensures:

- Predictable, testable, modular domain graphs
    
- Facades act as passive membranes, not logic holders
    
- UI binding occurs declaratively and safely after Signal authorization
    
- Domain modules are free from framework lock-in
    

---

# 🔥 Summary

✅ **Correct causal domain construction**  
✅ **Instance-based Signal event surfaces**  
✅ **Strict domain modularity and purity**  
✅ **Declarative, testable UI binding architecture**  
✅ **Facades as membranes, not controllers**

---
