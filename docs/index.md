---
title: Elmish.Avalonia.Glue
weight: 1
---

# Elmish.Avalonia.Glue

Build ordinary Avalonia applications with immutable F# state. Keep AXAML,
standard bindings, design-time preview, and DevTools. Move application state
and UI-shaped data into Elmish-style F#.

## Why use this library

Avalonia bindings want stable CLR objects that raise `PropertyChanged`.
Elmish wants each state transition to create a new immutable value. This
library supplies the small, explicit bridge between those two useful models.

It does not introduce a UI DSL, a new binding language, or special controls.
Your views remain normal `.axaml` files. The question is only how their
`DataContext` is shaped.

## Choose your first path

Start with [the quickstart](guides/start.html). It walks through one small form
and helps you choose a family:

- **Projection** when you want named, explicit CLR viewmodels.
- **ElmView** when immutable F# view records should be the screen schema.

Both paths use the same Elmish loop and ordinary Avalonia binding syntax.

## Learn in order

1. [Get started](guides/start.html) with one screen and one editable field.
2. [Understand the architecture](guides/understand.html) before scaling out.
3. Follow the focused guides for [Projection](guides/understand/projection-family.html), [ElmView](guides/understand/elmview-family.html), and [design-time data](guides/start/preview-and-design.html).
4. Use the [runnable examples](examples/index.html) as working patterns.
5. Use the [package reference](api.html) when you need a specific type.

## Build the documentation

The repository uses the local FsLiveDocs tool. It compiles F# documentation
blocks and renders the authored guides together with generated API reference.

```bash
dotnet tool restore
dotnet build Elmish.Avalonia.Glue.sln
bash scripts/build-docs.sh
dotnet livedocs watch --host 127.0.0.1 --port 5000
```
