namespace Elmish.Avalonia.Glue

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

        let modelKeys = HashSet<'Key>()
        for i in 0 .. models.Count - 1 do
            let key = modelKey.Invoke(models[i])
            if not (modelKeys.Add(key)) then
                invalidArg (nameof models) $"SyncWith requires unique model keys. Duplicate key '{key}' was found."

        let existingKeys = HashSet<'Key>()
        for i in collection.Count - 1 .. -1 .. 0 do
            let key = vmKey.Invoke(collection[i])
            if not (existingKeys.Add(key)) then
                invalidOp $"SyncWith requires unique view-model keys. Duplicate key '{key}' was found in the target collection."

            if not (modelKeys.Contains(key)) then
                collection.RemoveAt(i)

        let existing = Dictionary<'Key, 'ViewModel>()
        for vm in collection do
            existing[vmKey.Invoke(vm)] <- vm

        for i in 0 .. models.Count - 1 do
            let model = models[i]
            let key = modelKey.Invoke(model)

            match existing.TryGetValue(key) with
            | true, vm ->
                update.Invoke(vm, model)
                let currentIndex = collection.IndexOf(vm)
                if currentIndex <> i then
                    collection.Move(currentIndex, i)
            | false, _ ->
                collection.Insert(i, create.Invoke(model))
