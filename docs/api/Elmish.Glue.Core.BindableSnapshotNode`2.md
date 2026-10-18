# Elmish.Glue.Core.BindableSnapshotNode`2

## `BindableSnapshotNode<'RootSnapshot, 'NodeSnapshot>`

An abstract base class for nested bindable objects. These nodes "pluck" a piece of the root snapshot and expose it to the UI.

### Members

| Member | Description |
| :--- | :--- |
| `Snapshot` | Returns the current node-specific snapshot, plucked from the root. |
| `RegisterChildNode(child: IBindableSnapshotNode)` | Registers a child node to be included in subtree refreshes. |
| `RefreshSubtree()` | Triggers `PropertyChanged` for all properties exposed by this node and recursively calls `RefreshSubtree` on all registered children. |
