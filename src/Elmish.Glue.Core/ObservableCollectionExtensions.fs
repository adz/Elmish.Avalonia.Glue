namespace Elmish.Glue.Core

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open System.Runtime.CompilerServices

[<Extension>]
type ObservableCollectionExtensions =
    [<Extension>]
    static member SyncWith<'ViewModel, 'Model, 'Key when 'Key: equality and 'Key: not null>
        (
            collection: ObservableCollection<'ViewModel>,
            models: IReadOnlyList<'Model>,
            modelKey: Func<'Model, 'Key>,
            vmKey: Func<'ViewModel, 'Key>,
            create: Func<'Model, 'ViewModel>,
            update: Action<'ViewModel, 'Model>
        ) : unit =
        KeyedCollectionPatching.patch(
            collection,
            (fun vm -> vmKey.Invoke(vm)),
            models,
            (fun model -> modelKey.Invoke(model)),
            ignore,
            (fun vm model ->
                update.Invoke(vm, model)
                vm),
            (fun model -> create.Invoke(model)),
            (fun key -> $"SyncWith requires unique model keys. Duplicate key '{key}' was found."),
            (fun key -> $"SyncWith requires unique view-model keys. Duplicate key '{key}' was found in the target collection."))
