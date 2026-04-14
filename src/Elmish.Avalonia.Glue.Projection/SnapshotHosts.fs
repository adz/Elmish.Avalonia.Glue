namespace Elmish.Avalonia.Glue.Projection

open System
open Elmish.Glue.Core

type ObservableHost = BindableNode

type SnapshotHost<'T when 'T : not struct>(initial: 'T) =
    inherit BindableSnapshotHost<'T>(initial, "Current")

    member this.Current = this.Snapshot

type KeyedSnapshotCollection<'T, 'Key when 'T : not struct and 'Key : equality and 'Key : not null>
    (
        keySelector: Func<'T, 'Key>
    ) =
    inherit Elmish.Glue.Core.KeyedSnapshotCollection<'T, 'Key>(keySelector)
