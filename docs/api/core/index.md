---
sidebar_position: 1
---

# `Elmish.Glue.Core`

This is the shared, framework-neutral substrate.

## What this shows

The core package owns the pieces both authoring families need before Avalonia-specific integration enters the picture.

## Core shape

- host lifetime and dispatch control
- bindable snapshots and nested nodes
- keyed collection patching
- explicit projection contracts

## What you can do

- run an Elmish loop and keep a stable dispatcher
- expose immutable snapshots through `INotifyPropertyChanged`
- patch keyed collections without replacing stable identity
- build CLR-facing projection surfaces when that is the right fit

## Member map

- [Host lifetime](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/core/host-lifetime)
- [Snapshots](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/core/snapshots)
- [Collections](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/core/collections)
- [Projection contracts](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/core/projections)

## Read next

- [Avalonia package hub](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/avalonia)
- [Projection package hub](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/projection)
- [ElmView package hub](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/elmview)

## Source

- [ElmishHost.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/ElmishHost.fs)
- [SnapshotSubstrate.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/SnapshotSubstrate.fs)
- [Projections.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/Projections.fs)
