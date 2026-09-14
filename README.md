# Elmish.Avalonia.Glue

- Avalonia has `.axaml`, bindings, design-time preview, and great DevTools.
- Elmish has immutable state, explicit messages, and one update loop.

**Can an Avalonia app keep normal AXAML and designer tooling while moving UI
state and UI-shaped data into Elmish-style F#?**

*Elmish.Avalonia.Glue* does exactly this in two different approaches:

- `Projection` where you create normal CLR XAML-facing viewmodels, and 'project' into it
- `ElmView` with less C# - immutable F# view records bind via a thin api

The project does not replace AXAML with an F# UI DSL or change how Elmish works in either case.

It's just the glue between them.

## Start with the guide

- [Docs site](https://adz.github.io/Elmish.Avalonia.Glue/): start here if you want the intent before the APIs.

## Packages

- [Architecture](https://adz.github.io/Elmish.Avalonia.Glue/guides/architecture.html) explains the shared substrate.
- [Projection](https://adz.github.io/Elmish.Avalonia.Glue/guides/projection.html) covers explicit CLR viewmodels.
- [ElmView](https://adz.github.io/Elmish.Avalonia.Glue/guides/elmview.html) covers immutable F# view records.

## Samples

- [Projection samples](sample/Samples.Projection): explicit CLR viewmodel path.
- [ElmView samples](sample/Samples.ElmView): F# view-record path.
