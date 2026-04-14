# Elmish.Avalonia.Glue

Avalonia-first Elmish integration built around a shared substrate and two UI
authoring families:

- `Projection`: explicit CLR-facing projections for teams that want named
  viewmodels and normal compiled bindings.
- `ElmView`: immutable F# view records as the authored UI schema, with a thin
  bindable host instead of a handwritten projection tree.

The repository keeps Avalonia normal in both paths: standard `.axaml`,
standard bindings, standard design-time preview, and standard DevTools.

## Repository Shape

The split is now:

- `src/Elmish.Glue.Core`
  framework-neutral host lifetime, projection contracts, and keyed collection
  patching primitives.
- `src/Elmish.Avalonia.Glue`
  Avalonia-specific host wiring that posts core updates onto Avalonia's UI
  thread and preserves the compatibility surface for existing projection-based
  apps.
- `src/Elmish.Avalonia.Glue.Projection`
  Avalonia-facing projection helpers such as `SnapshotHost` and
  `KeyedSnapshotCollection` for the explicit viewmodel family.
- `src/Elmish.Avalonia.Glue.ElmView`
  bindable runtime and design-time hosts for immutable F# view records.
- `sample/Samples.Projection`
  sample suite for the projection family, including the full example matrix.
- `sample/Samples.ElmView`
  sample suite for the ElmView family, implementing the same example matrix.

The root solution, `Elmish.Avalonia.Glue.sln`, includes both sample suites so
they stay visible in the IDE and can be formatted together. For core-only CLI
builds that should exclude the samples, use `Elmish.Avalonia.Glue.Build.slnf`.

## Shared Substrate

`Elmish.Glue.Core` is the common substrate used by both architecture families.
It contains the pieces that are genuinely framework-neutral:

- `ElmishHostConnection<'Msg>` and `ElmishHost.start*` for owning a running
  Elmish loop and exposing a stable dispatcher.
- `IProjection<'Model>`, `IDispatchTarget<'Msg>`, and `IProjection<'Model,'Msg>`
  for mechanical projection wiring where a CLR-facing surface is still useful.
- `ObservableCollectionExtensions.SyncWith` for keyed collection patching that
  preserves selection, scroll position, and stable item identity.

Anything Avalonia-specific stays above this layer. The core is moving toward
`Elmish.Glue.Core`, but the implementation order and sample guidance remain
Avalonia-first.

## Architecture Families

### Projection

Use `Projection` when the UI needs explicit named viewmodels or when the team
wants the clearest possible CLR-facing surface for XAML, commands, and
tooling.

This family keeps:

- normal `DataContext` objects with explicit properties
- compiled bindings against projection types
- shallow C# or F# glue over Elmish-owned state
- identity-aware collection sync via `SyncWith`
- optional snapshot-host style helpers for immutable snapshot data with stable
  mutable shells

The projection samples show both the minimal path and the larger
feature-foldered path.

### ElmView

Use `ElmView` when the team wants immutable F# records to be the authored UI
schema and wants to minimize handwritten projection boilerplate.

This family keeps:

- F# view records as the UI-shaped data model
- normal AXAML bindings through a single bindable host
- a tiny host class for commands or event forwarding where Avalonia needs an
  imperative seam
- the same shape for runtime updates and design-time preview data

The ElmView samples show the closest-to-Elm path currently in the repository.

## Design-Time Workflow

Design-time support is a hard requirement for both families.

The common workflow is:

1. Define UI-shaped F# data in the core/sample layer.
2. Provide a realistic design snapshot from F#.
3. Wrap that snapshot in the bindable surface the view expects.
4. Point AXAML at that normal bindable surface.
5. Start the real Elmish host at runtime and keep the preview shape as close to
   runtime as possible.

In the current samples:

- `Samples.Projection` calls `Projection.Update(Core.App.getDesignModel())` in
  the view constructor so the window renders useful preview data before the
  runtime host is attached.
- `Samples.ElmView` constructs `RuntimeViewHost` with
  `Core.App.getDesignView()` so the same `View` shape feeds both preview and
  live updates.

That keeps AXAML previewable without booting the full Elmish runtime.

## When To Choose Which Path

Choose `Projection` when:

- the team wants explicit CLR-facing viewmodels
- view-specific command and interaction seams are easier to express as concrete
  properties and methods
- the app already leans on projection-style organization and you want the split
  with minimal disruption
- a feature benefits from stable mutable shells around immutable model data

Choose `ElmView` when:

- the team wants most authored UI shaping logic to stay in F#
- immutable view snapshots should be the main review surface
- reducing handwritten projection boilerplate matters more than exposing many
  named CLR projection types
- preview and runtime should share one bindable view shape as directly as
  possible

You can mix biases at the repository level, but each feature path should stay
internally consistent. Both families are meant to keep Avalonia ordinary.

## Packages And APIs

Use `Elmish.Avalonia.Glue` when you want the Avalonia dispatcher integration:

```fsharp
_host <- ElmishHost.startAndBind(program, viewModel.Update, viewModel.SetDispatch)
```

Use `Elmish.Glue.Core` when you only need the framework-neutral hosting or
projection primitives.

Use `Elmish.Avalonia.Glue.Projection` for explicit projection helpers such as:

- `SnapshotHost<'T>`
- `KeyedSnapshotCollection<'T,'Key>`

Use `Elmish.Avalonia.Glue.ElmView` for bindable F# view hosts such as:

- `RuntimeViewHost<'View>`
- `DesignViewHost<'View>`
- `ElmView.runtime`
- `ElmView.design`

## Samples

The sample matrix is implemented in both suites:

- static layout / HTML-equivalent
- user input / forms
- random / dice
- HTTP
- time / clock
- files
- SVG-equivalent composition

See:

- `sample/Samples.Projection`
- `sample/Samples.ElmView`
- `docs/INTENDED_USAGE.md`

## Why

This repository is not trying to replace Avalonia with a custom UI DSL. The
goal is to compare two Elmish-friendly authoring styles while keeping Avalonia
tooling, previews, bindings, and debugging workflows intact.

## License

MIT
