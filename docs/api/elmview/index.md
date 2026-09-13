---
sidebar_position: 4
---

# `Elmish.Avalonia.Glue.ElmView`

ElmView is the F#-first package family.

## Scope

This package keeps the authored UI schema in F# and exposes a mechanical
bindable surface for Avalonia markup.

The current package provides the runtime/design host base classes, node base
classes, and write-back routing. It does not yet include a source generator or
CLI that emits host code from F# records. The sample `AppHost` is handwritten in
the generated shape that future tooling is expected to produce.

## Core shape

- generated-shaped runtime and design hosts
- nested generated-shaped nodes
- centralized write-back routing
- `OneWay` and `TwoWay` bindings

## What you can do

- bind AXAML to a CLR surface while keeping the model immutable
- register editable paths in one host-side place
- share the same host shape between design-time and runtime

## Member map

- [Generated hosts](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/elmview/hosts)
- [Write-back bindings](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/elmview/write-back)
- [Generated nodes](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/elmview/nodes)

## Read next

- [Core package hub](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/core)
- [ElmView package source](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.ElmView/ElmViewHosts.fs)
