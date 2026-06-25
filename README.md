# Elmish.Avalonia.Glue

**Can an Avalonia app keep normal AXAML and designer tooling while moving UI
state and UI-shaped data into Elmish-style F#?**

Avalonia has `.axaml`, bindings, design-time preview, and DevTools.

Elmish has immutable state, explicit messages, and one update loop.

This repository leaves each of these to what they do best via two projects:

- `Projection` for normal, explicit CLR XAML-facing viewmodels
- `ElmView` for immutable F# view records and a thin generated bindable surface

`ElmView` makes plain F# records the UI schema, then exposes them through
bindable hosts, standard `OneWay` / `TwoWay` bindings, and centralized
write-back mapping.

The project does not replace AXAML with an F# UI DSL in either case.

## Read next

- [Docs site](https://adz.github.io/Elmish.Avalonia.Glue/docs/intro): start here if you want the intent before the APIs.
- [Getting started](docs/guides/start.mdx): compare the two families from the same Avalonia binding.
- [Projection samples](sample/Samples.Projection): explicit CLR viewmodel path.
- [ElmView samples](sample/Samples.ElmView): F# view-record path.

## Packages

- `Elmish.Glue.Core`
- `Elmish.Avalonia.Glue`
- `Elmish.Avalonia.Glue.Projection`
- `Elmish.Avalonia.Glue.ElmView`
