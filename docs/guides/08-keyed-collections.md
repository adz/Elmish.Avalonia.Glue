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

Key by identity (for example a path or persistent ID), never by display text.
Duplicate keys in an incoming `SyncWith` snapshot throw `ArgumentException` (parameter `targets`) naming the key, before any collection changes. Duplicate keys already in the target collection throw `InvalidOperationException`; unlike incoming-key validation, this can happen after obsolete items have been removed.

## Understand the patch

Given the next immutable list, the adapter:

1. matches existing items by key;
2. updates retained items;
3. creates items for new keys;
4. moves retained items into the requested order;
5. removes keys that disappeared.

The result has the same keys and order as the snapshot. `SyncWith` retains matching row viewmodel references; `KeyedSnapshotCollection` replaces a matching item's reference when its incoming snapshot differs. Selection by object is therefore not guaranteed for snapshot rows.

## Choose the collection API

Choose the adapter by whether rows need stable viewmodel identity or only collection identity; see [collection API selection](../api/core/collections.html#choose-the-collection-api).

See the runnable [keyed patching example](../examples/keyed-collection-sync.html)
and the generated [API reference](../api.html).
