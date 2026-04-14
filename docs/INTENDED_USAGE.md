# Intended Usage

## Positioning

This repository now documents two Avalonia-first authoring families built on a
shared substrate:

- `Projection` for explicit CLR-facing viewmodels
- `ElmView` for immutable F# view records as the authored UI schema

Both paths assume Elmish owns application state and behavior. Both paths keep
Avalonia normal: ordinary `.axaml`, ordinary bindings, normal design-time
preview, and standard DevTools workflows.

## Shared Substrate

`Elmish.Glue.Core` is the common infrastructure layer.

Use it for:

- `ElmishHost.start`, `startWithPost`, `startAndBind`, and `startAndBindWithPost`
- `ElmishHostConnection<'Msg>` lifetime ownership
- projection contracts:
  `IProjection<'Model>`, `IDispatchTarget<'Msg>`, `IProjection<'Model,'Msg>`
- keyed collection patching through
  `ObservableCollectionExtensions.SyncWith`

This layer is intentionally mechanical. It should hold only concepts that are
framework-neutral and based on standard .NET primitives.

`Elmish.Avalonia.Glue` remains the Avalonia-facing package that routes host
updates onto Avalonia's UI thread.

## Projection Family

The `Projection` family is the explicit viewmodel path.

Use it when you want:

- named CLR-facing viewmodels as the primary AXAML surface
- compiled bindings directly against those projection types
- explicit command properties, setter-based interactions, and familiar MVVM
  organization
- stable mutable shells around immutable Elmish-owned state

The intended ownership split is:

- Elmish model: real application state
- projection objects: bindable surface, stable collections, event forwarding,
  and other UI mechanics

Projection code should stay shallow and patterned. It should not become a
second domain model.

### Projection-specific helpers

`Elmish.Avalonia.Glue.Projection` adds helpers for the projection family:

- `SnapshotHost<'T>` for a single immutable snapshot exposed through a bindable
  shell
- `KeyedSnapshotCollection<'T,'Key>` for identity-aware snapshot list updates
  without rebuilding the entire collection

These helpers are useful when a feature wants explicit CLR-facing hosts but the
underlying data should remain immutable and identity-aware.

## ElmView Family

The `ElmView` family is the F#-first path.

Use it when you want:

- immutable F# records to be the authored UI schema
- most UI shaping logic to live in F# view data and AXAML
- generated bindable root and child nodes rather than a handwritten projection
  tree
- normal Avalonia `Mode=TwoWay` bindings for editable controls
- centralized write-back mapping instead of F# binding metadata
- runtime and design-time to share the same view shape as directly as possible

The host layer stays deliberately small. It usually owns:

- the current immutable `View` snapshot
- generated CLR-facing properties that read from that snapshot
- generated writable setters for explicitly registered editable paths
- only the small imperative seams that cannot be expressed as normal bindings

The host should not become a second projection tree.

### ElmView-specific helpers

`Elmish.Avalonia.Glue.ElmView` provides:

- `BindableViewHost<'View>`
- `RuntimeViewHost<'View>`
- `DesignViewHost<'View>`
- `GeneratedViewHost<'View,'Msg>`
- `GeneratedViewNode<'RootView,'NodeView,'Msg>`
- `RuntimeGeneratedViewHost<'View,'Msg>`
- `DesignGeneratedViewHost<'View,'Msg>`
- `ElmView.runtime`
- `ElmView.design`

These types keep the bindable surface consistent between runtime and preview
snapshots. `GeneratedViewHost` and `GeneratedViewNode` define the bindable host
shape that ElmView V2 generation should target for nested nodes and writable
properties over immutable snapshots.

### ElmView V2 authoring contract

ElmView V2 is intended to feel like normal Avalonia authoring even though the
runtime model stays immutable.

The authored surface is:

- plain immutable F# view records
- ordinary `.axaml`
- explicit Elmish messages and update functions

The generated/runtime surface is:

- generated root and nested bindable nodes
- writable CLR setters that call centralized write-back mappings
- `PropertyChanged` propagation across the generated node graph when the
  immutable snapshot changes

That means:

- `Mode=OneWay` stays display-only
- `Mode=TwoWay` is the standard editable-field contract
- binding paths stay ordinary, such as `UserInput.Name`
- F# records do not carry binding annotations, write-back attributes, or other
  metadata
- editable properties dispatch only when the host registers that path

## Design-Time Workflow

Design-time support is required in both families. The intended workflow is:

1. Define UI-shaped F# types for the feature.
2. Create a realistic design snapshot in F#.
3. Wrap that snapshot in the bindable shape the AXAML view expects.
4. Bind AXAML against that normal CLR surface with compiled bindings.
5. Start the real Elmish host at runtime and swap live updates into the same or
   nearly identical shape.

### Projection design-time flow

In the projection samples:

1. F# defines a design model, for example `Core.App.getDesignModel()`.
2. The view constructs the root projection.
3. The constructor immediately calls `Projection.Update(designModel)`.
4. AXAML binds to the populated projection with normal compiled bindings.
5. At runtime, `ElmishHost.startAndBind` keeps updating that same projection.

This keeps design-time preview useful without booting the full application.

### ElmView design-time flow

In the ElmView samples:

1. F# defines a design view, for example `Core.App.getDesignView()`.
2. The root host inherits `RuntimeGeneratedViewHost<'View,'Msg>`.
3. The host starts life with the design view snapshot and registers
   write-back mappings in one place near host construction.
4. AXAML binds through generated node properties such as `UserInput.Name`.
5. `Mode=TwoWay` bindings dispatch explicit Elmish messages through the
   registered write-back routes.
6. At runtime, `ElmishHost.startAndBind` calls `Host.Update` with live view
   snapshots, and the generated node graph raises `PropertyChanged` without
   re-dispatching.

That pattern keeps preview and runtime extremely close while limiting
imperative seams to the cases Avalonia still needs, such as button clicks,
file picking, or other command-like interactions.

## Host Lifetime

The intended runtime lifetime is the same in both families:

1. Build the Elmish program in F#.
2. Construct the root Avalonia bindable surface.
3. Start the host from the app, window, or owning view.
4. Hold the returned `ElmishHostConnection<'Msg>` or `IDisposable`.
5. Dispose it when the owner shuts down so subscriptions terminate cleanly.

## Dispatch Flow

The intended flow remains:

1. the user interacts with Avalonia controls
2. a generated writable property or small host seam emits an Elmish message
3. Elmish updates immutable state
4. the host posts the update to Avalonia's UI thread
5. the generated bindable surface refreshes from the new snapshot

The difference between the families is not where behavior lives. Behavior stays
in Elmish in both. The difference is which bindable shape AXAML sees.

## Collection Synchronization

`ObservableCollectionExtensions.SyncWith` is the default keyed collection
patching tool for projection-style bindable collections.

Use it when you need to:

- preserve existing viewmodel instances
- preserve selection, scroll position, and container reuse
- update, insert, remove, and reorder items without replacing the collection

`SyncWith` requires unique keys on both the model side and the target
collection side. Duplicate keys fail fast intentionally.

Use projection collections when Avalonia materially benefits from stable
mutable identity. Prefer immutable snapshots when a feature does not need that
extra mutable shell.

## Choosing A Path

Choose `Projection` when:

- the team wants explicit CLR-facing viewmodels
- many view interactions map naturally to command properties or viewmodel
  methods
- the codebase already organizes features around projection/viewmodel types
- a screen benefits from stable mutable collection identity or snapshot hosts

Choose `ElmView` when:

- the team wants most authored UI shaping logic in F#
- the review surface should primarily be immutable view records plus AXAML
- reducing handwritten projection code is a priority
- editable controls should still look like ordinary Avalonia bindings
- runtime and design-time should share one bindable view shape with minimal
  translation

If a feature needs a lot of mutable, control-specific mechanics, prefer
`Projection`. If a feature mostly wants immutable view snapshots and a thinner
host, prefer `ElmView`.

For the post-ElmView-V2 measured comparison between the two families, see
`docs/ARCHITECTURE_COMPARISON.md`.

## What These Libraries Are Not

They are not:

- a custom Avalonia view DSL
- a nonstandard binding model
- a second application state system beside Elmish
- a reactive collection framework

The goal is to keep Avalonia ordinary while making Elmish-owned, F#-shaped UI
authoring more practical.
