# Elmish.Avalonia.Glue

Avalonia is good at normal `.axaml`, bindings, design-time preview, and
DevTools. Elmish is good at explicit state transitions and immutable models.
This repository explores how to keep Avalonia normal while moving more UI shape
and interaction state into F#.

It does that with a small shared core and two authoring families:

- `Projection` for explicit CLR-facing viewmodels
- `ElmView` for immutable F# view records and a thin generated bindable surface

`Projection` keeps named viewmodels for teams that want a familiar XAML-facing
surface. `ElmView` makes plain F# records the UI schema, then exposes them to
Avalonia through normal bindable hosts, ordinary `OneWay` / `TwoWay` bindings,
and centralized write-back mapping.

## Read next

- [Docs source](docs/intro.mdx)
- [Docs site](https://adz.github.io/Elmish.Avalonia.Glue/docs/intro)
- [Projection samples](sample/Samples.Projection)
- [ElmView samples](sample/Samples.ElmView)

## Packages

- `Elmish.Glue.Core`
- `Elmish.Avalonia.Glue`
- `Elmish.Avalonia.Glue.Projection`
- `Elmish.Avalonia.Glue.ElmView`
