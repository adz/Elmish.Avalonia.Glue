---
sidebar_position: 2
---

# `Elmish.Avalonia.Glue`

This package is the Avalonia-facing bridge over the core host.

## Scope

The package keeps Avalonia threading and bindings in the Avalonia layer while
delegating host mechanics to the shared core.

## Core shape

- dispatcher marshalling to Avalonia's UI thread
- compatibility aliases that preserve the existing public shape
- the same `ElmishHost` entry points at a higher integration layer

## What you can do

- attach Elmish to an Avalonia app
- keep viewmodels and AXAML conventional
- preserve existing `Elmish.Avalonia.Glue` call sites while the core evolves

## Member map

- [Dispatcher bridge](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/avalonia/dispatcher)
- [Compatibility aliases](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/avalonia/aliases)

## Read next

- [Core package hub](https://adz.github.io/Elmish.Avalonia.Glue/docs/api/core)
- [Avalonia host bridge](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue/AvaloniaHost.fs)
