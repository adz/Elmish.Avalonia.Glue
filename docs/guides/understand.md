---
title: Understand the architecture
---

# Understand the architecture

Read this after the quickstart. It names the few moving parts so the API pages
are easier to use.

## The boundary

Elmish replaces immutable values. Avalonia binds to stable objects. A **host**
is the stable object. A **snapshot** is the current immutable value behind it.

When a snapshot changes, the host raises `PropertyChanged`. When a user edits
a writable property, the host dispatches an Elmish message. The host never
mutates the snapshot in place.

## The layers

| Layer | Owns | Does not own |
| --- | --- | --- |
| `Elmish.Glue.Core` | snapshots, notifications, dispatch, keyed patching | Avalonia threading |
| `Elmish.Avalonia.Glue` | UI-thread delivery and compatibility names | UI schema |
| `Projection` | explicit CLR-facing hosts and collections | the Elmish model |
| `ElmView` | generated-shaped hosts, nodes, and write-back routes | a custom markup language |

## The two authoring families

**Projection** makes the UI-facing shape explicit in CLR viewmodels. Choose it
when commands, derived properties, or mutable row adapters deserve named code.

**ElmView** makes an immutable F# view record the UI-facing schema. Choose it
when most UI shaping belongs in F# and the host can remain mechanical.

## The important invariant

The AXAML binding path is a contract. Runtime and design-time hosts must expose
the same paths. A `TwoWay` path must dispatch exactly once for a user edit;
receiving a new snapshot must never dispatch again.

Read [the Projection guide](understand/projection-family.html) or [the ElmView
guide](understand/elmview-family.html) next.
