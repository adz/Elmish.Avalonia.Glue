# Elmish.Avalonia.Glue

**Can an Avalonia app keep normal AXAML and designer tooling while moving UI
state and UI-shaped data into Elmish-style F#?**

- Avalonia has `.axaml`, bindings, design-time preview, and great DevTools.
- Elmish has immutable state, explicit messages, and one update loop.

Elmish.Avalonia.Glue does exactly this in two flavours in different projects:

- `Projection` where you create nomral CLR XAML-facing viewmodels, and 'project' into it
- `ElmView` with less C# - immutable F# view records bind via a thin api

The project does not replace AXAML with an F# UI DSL in either case. It's just glue.

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
