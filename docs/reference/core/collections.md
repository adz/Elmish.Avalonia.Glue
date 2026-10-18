# Collections

Collection utilities sync mutable `ObservableCollection`s with immutable lists while preserving item identity.

## API contracts

- [`Elmish.Glue.Core.ObservableCollectionExtensions`](/api/Elmish.Glue.Core.ObservableCollectionExtensions.html)
- <a href="/api/Elmish.Glue.Core.KeyedSnapshotCollection`2.html">`Elmish.Glue.Core.KeyedSnapshotCollection`2`</a>

## Choose the collection API

Use `ObservableCollectionExtensions.SyncWith` when each row is a projection
that must update in place.

Use `KeyedSnapshotCollection<T,TKey>` when the collection can expose snapshot
items directly and only the collection identity needs to remain stable.


## Identity preserved by patching

Patching keeps the `ObservableCollection` instance. `SyncWith` retains the same row viewmodel for matching keys and calls `update` on it. `KeyedSnapshotCollection` instead replaces a retained key's item when the incoming snapshot is a different reference; it does **not** preserve row object identity. Avalonia container reuse, selection, and animations depend on the control and its selection model; they are not guaranteed by a matching key. For selection across replaced snapshots, track the selected key and resolve it against the new items.


## Implementations

- [ObservableCollectionExtensions.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/ObservableCollectionExtensions.fs)
- [KeyedCollectionPatching.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/KeyedCollectionPatching.fs)
- [KeyedSnapshotCollection source](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Glue.Core/SnapshotSubstrate.fs)
