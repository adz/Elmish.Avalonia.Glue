# Elmish.Avalonia.Glue

Minimal glue between an Elmish program and Avalonia: run the update loop on the UI thread and keep normal
AXAML, bindings, the designer and DevTools. No Rx, no DynamicData, no F# UI DSL.

Pick an approach on top of it:

- [Elmish.Avalonia.Glue.Projection](https://www.nuget.org/packages/Elmish.Avalonia.Glue.Projection) —
  project the model into ordinary CLR viewmodels.
- [Elmish.Avalonia.Glue.ElmView](https://www.nuget.org/packages/Elmish.Avalonia.Glue.ElmView) —
  bind immutable F# view records through thin, design-time-friendly hosts.

Pre-1.0: the API may still change between minor versions.

- Docs: https://adz.github.io/Elmish.Avalonia.Glue/docs/intro
- Source: https://github.com/adz/Elmish.Avalonia.Glue
