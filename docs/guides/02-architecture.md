# Architecture

The library has one job: reconcile immutable values with stable bindable
objects. Everything else follows from that boundary.

## Model

The Elmish model is application state. `update` is the only place that decides
how a message changes it. Avalonia never mutates this value directly.

## View snapshot

A view snapshot is immutable data shaped for a screen. It can be the model
itself, but it is often better to derive display text, flags, and nested view
records in F#.

For example, derive `CanSubmit` from validation state once. Do not repeat the
same rule in a button, a projection, and code-behind.

## Host

The host is the stable CLR object used as `DataContext`. It stores the current
snapshot, raises `PropertyChanged`, and holds the dispatch function supplied
when the Elmish program starts.

The host is mutable infrastructure around immutable application data. It must
not become a second source of truth.

## Node

A node is a stable bindable object for one nested record. If AXAML binds to
`Profile.Name`, the root host exposes `Profile` and the profile node exposes
`Name`.

Nodes let Avalonia retain object identity while F# replaces nested records.

## Write-back route

A `TwoWay` setter cannot change an immutable snapshot. A write-back route maps
the new property value to an Elmish message.

```text
Profile.Name = "Grace"
→ NameChanged "Grace"
→ update
→ next snapshot
→ PropertyChanged("Name")
```

Snapshot refresh must never call the setter. One user edit should dispatch one
message, and one snapshot update should dispatch none.

## Keyed collection

Replacing a list can discard selection, virtualization state, or row identity.
A keyed collection patches the mutable Avalonia-facing list to match the next
immutable list.

Use it only when stable item identity materially helps the control.

## Package boundaries

| Package | Responsibility |
| --- | --- |
| `Elmish.Glue.Core` | framework-neutral hosts, notifications, dispatch, and keyed patching |
| `Elmish.Avalonia.Glue` | Avalonia UI-thread delivery and compatibility surface |
| `Elmish.Avalonia.Glue.Projection` | explicit projection and snapshot-host helpers |
| `Elmish.Avalonia.Glue.ElmView` | view-record hosts, nodes, and write-back routing |

The next decision is the authoring surface. Read [Projection](projection.html)
or [ElmView](elmview.html).
