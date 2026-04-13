namespace Elmish.Avalonia.Glue

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open System.Runtime.CompilerServices

/// Extension methods for keeping Avalonia-facing observable collections in sync
/// with Elmish model lists.
[<Extension>]
type ObservableCollectionExtensions =
    /// Synchronizes an <see cref="ObservableCollection{T}" /> with a keyed model
    /// list while preserving existing ViewModel instances where keys match.
    ///
    /// The method updates existing items, removes items whose keys are no longer
    /// present, inserts new items, and reorders the target collection to match
    /// the source sequence.
    ///
    /// This is intended for UI projection layers that want to keep an
    /// `ObservableCollection` instance stable for bindings, selection, and
    /// scroll position.
    ///
    /// Both the model list and the target collection must contain unique keys.
    /// Duplicate keys are treated as invalid input and fail fast.
    ///
    /// <param name="collection">The target collection to update in place.</param>
    /// <param name="models">The source model list in the desired final order.</param>
    /// <param name="modelKey">Extracts the stable identity key from a model item.</param>
    /// <param name="vmKey">Extracts the stable identity key from a ViewModel item.</param>
    /// <param name="create">Creates a new ViewModel for a model item that does not already exist in the target collection.</param>
    /// <param name="update">Updates an existing ViewModel from the latest model item.</param>
    /// <exception cref="System.ArgumentException">
    /// Thrown when the source model list contains duplicate keys.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when the target collection contains duplicate ViewModel keys.
    /// </exception>
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
