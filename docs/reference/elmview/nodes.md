# Generated-shaped nodes

`GeneratedViewNode<'RootView, 'NodeView, 'Msg>` is the base class for nested
bindable objects in an ElmView tree. Avalonia binds to deep paths in immutable
F# view records and still receives change notifications.

Despite the type name, this package does not currently ship a generator. The
sample nodes are handwritten in a regular, mechanical shape that a future
generator could emit.

## API contracts

- <a href="/api/Elmish.Avalonia.Glue.ElmView.GeneratedViewNode`3.html">`Elmish.Avalonia.Glue.ElmView.GeneratedViewNode`3`</a>

## Implementation

- [ElmViewHosts.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue.ElmView/ElmViewHosts.fs)
