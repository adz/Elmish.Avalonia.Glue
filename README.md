# Elmish.Avalonia.Glue

Avalonia has `.axaml`, bindings, design-time preview, and DevTools. Elmish has
immutable state, explicit messages, and one update loop.

This repository asks how far an Avalonia app can move screen-shaped state into
F# while keeping the Avalonia workflow intact.

The experiment is split into a small shared core and two authoring families:

- `Projection` for explicit CLR-facing viewmodels
- `ElmView` for immutable F# view records and a thin generated bindable surface

`Projection` keeps named viewmodels for teams that want a CLR XAML-facing
surface.

`ElmView` makes plain F# records the UI schema, then exposes them through
bindable hosts, standard `OneWay` / `TwoWay` bindings, and centralized
write-back mapping.

The project does not replace AXAML with an F# UI DSL. It moves more of the
state and view shape behind AXAML into F#.

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
