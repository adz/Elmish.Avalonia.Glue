# Keyed collections

Immutable lists are easy to reason about. Desktop controls often benefit from
stable mutable item identity. Keyed patching bridges those needs.

## Notice when replacement is harmful

Replacing the whole collection can reset selection, recreate containers,
interrupt inline editing, and reduce virtualization reuse.

For a short static list, replacement may still be the clearest choice. Add a
mutable adapter only when the control benefits from it.

## Choose a stable key

A key must identify the same logical item across snapshots. Database IDs and
domain identifiers work well. List positions and display names usually do not.

Duplicate keys are invalid because the patcher cannot decide which item to
retain.

## Understand the patch

Given the next immutable list, the adapter:

1. matches existing items by key;
2. updates retained items;
3. creates items for new keys;
4. moves retained items into the requested order;
5. removes keys that disappeared.

The result has the same keys and order as the snapshot while retained items
keep their CLR identity.

## Choose the collection API

Use `ObservableCollectionExtensions.SyncWith` when each row is a projection
that must update in place.

Use `KeyedSnapshotCollection<T,TKey>` when the collection can expose snapshot
items directly and only the collection identity needs to remain stable.

See the runnable [keyed patching example](../examples/keyed-collection-sync.html)
and the generated [API reference](../api.html).
