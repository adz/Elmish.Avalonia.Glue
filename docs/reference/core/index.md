# Elmish.Glue.Core

Core contains the framework-neutral mechanics shared by Projection and
ElmView. It depends on standard .NET notification, collection, and lifetime
concepts rather than Avalonia controls.

## Use Core to build adapters

- `ElmishHost` runs a program with direct or caller-provided posting.
- `ElmishHostConnection` owns dispatch and disposal.
- `BindableNode` implements the notification base.
- `BindableSnapshotHost` holds a root immutable snapshot.
- `BindableSnapshotNode` exposes one nested snapshot.
- `KeyedSnapshotCollection` patches immutable lists by identity.
- Projection contracts compose explicit model-to-viewmodel adapters.

Most applications consume these types through an Avalonia package. Use them
directly for tests, custom framework adapters, or lower-level composition.
