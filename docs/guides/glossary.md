# Glossary

- **Model**: immutable Elmish application state, changed by `update` in response to a message.
- **View snapshot**: immutable, UI-shaped value derived from a model; `Current` in a snapshot host, `View` in an ElmView host.
- **Host**: stable CLR `DataContext` object holding the latest snapshot and notifying bindings.
- **Node**: stable nested CLR binding object whose snapshot is refreshed when its root host changes.
- **Projection**: explicit bindable viewmodel that receives model updates and sends messages.
- **Generated-shaped**: handwritten host or node following the intended mechanical shape; this repository does not ship a generator.
- **Route**: a registered writable facade path mapped to a message constructor; an unmatched path does not dispatch.
- **Subscription**: Elmish-managed ongoing source of messages, owned and stopped with the program.
- **Keyed identity**: matching list items by a stable key. A stable collection does not imply stable row snapshot, selection, or container identity.

See [architecture](architecture.html) for the data flow and [collection selection](../reference/core/collections.html#choose-the-collection-api) for identity trade-offs.
