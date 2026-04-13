namespace Elmish.Avalonia.Glue

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open System.Runtime.CompilerServices

/// Interface for ViewModels that project Elmish model state.
type IProjection<'Model> =
    /// Updates the projection with the latest state from the Elmish model.
    abstract member Update : 'Model -> unit

/// Interface for projections that receive an Elmish dispatcher.
type IDispatchTarget<'Msg> =
    /// Sets the dispatcher used to send messages back to the Elmish loop.
    abstract member SetDispatch : Action<'Msg> -> unit

/// Interface for ViewModels that act as projections of an Elmish model.
type IProjection<'Model, 'Msg> =
    inherit IProjection<'Model>
    inherit IDispatchTarget<'Msg>

[<Extension>]
type ProjectionExtensions =
    /// Wires a child projection's dispatcher to a parent's dispatcher with a message map.
    [<Extension>]
    static member ForwardTo<'Msg, 'ParentMsg>
        (
            projection: IDispatchTarget<'Msg>,
            dispatch: Action<'ParentMsg>,
            map: Func<'Msg, 'ParentMsg>
        ) : unit =
        projection.SetDispatch(Action<'Msg>(fun msg -> dispatch.Invoke(map.Invoke(msg))))

    /// Synchronizes a collection of projections with a list of models.
    /// Automatically updates existing projections if they implement IProjection.
    [<Extension>]
    static member SyncWith<'ViewModel, 'Model, 'Key when 'ViewModel :> IProjection<'Model> and 'Key : equality and 'Key : not null>
        (
            collection: ObservableCollection<'ViewModel>,
            models: IReadOnlyList<'Model>,
            modelKey: Func<'Model, 'Key>,
            vmKey: Func<'ViewModel, 'Key>,
            create: Func<'Model, 'ViewModel>
        ) : unit =
        ObservableCollectionExtensions.SyncWith(
            collection,
            models,
            modelKey,
            vmKey,
            create,
            Action<'ViewModel, 'Model>(fun vm m -> vm.Update(m)))

    /// Synchronizes a collection of projections with a list of models and
    /// wires a parent dispatcher directly when the child and parent message
    /// types are the same.
    [<Extension>]
    static member SyncWith<'ViewModel, 'Model, 'Key, 'Msg when 'ViewModel :> IProjection<'Model> and 'ViewModel :> IDispatchTarget<'Msg> and 'Key : equality and 'Key : not null>
        (
            collection: ObservableCollection<'ViewModel>,
            models: IReadOnlyList<'Model>,
            modelKey: Func<'Model, 'Key>,
            vmKey: Func<'ViewModel, 'Key>,
            create: Func<'Model, 'ViewModel>,
            dispatch: Action<'Msg>
        ) : unit =

        ObservableCollectionExtensions.SyncWith(
            collection,
            models,
            modelKey,
            vmKey,
            Func<'Model, 'ViewModel>(fun m ->
                let vm = create.Invoke(m)
                vm.SetDispatch(dispatch)
                vm.Update(m)
                vm),
            Action<'ViewModel, 'Model>(fun vm m -> vm.Update(m)))

    /// Synchronizes a collection of projections AND wires their dispatchers.
    /// This eliminates manual loops in SetDispatch for collection items.
    [<Extension>]
    static member SyncWith<'ViewModel, 'Model, 'Key, 'Msg, 'ParentMsg when 'ViewModel :> IProjection<'Model> and 'ViewModel :> IDispatchTarget<'Msg> and 'Key : equality and 'Key : not null>
        (
            collection: ObservableCollection<'ViewModel>,
            models: IReadOnlyList<'Model>,
            modelKey: Func<'Model, 'Key>,
            vmKey: Func<'ViewModel, 'Key>,
            create: Func<'Model, 'ViewModel>,
            parentDispatch: Action<'ParentMsg>,
            map: Func<'Msg, 'ParentMsg>
        ) : unit =
        
        let childDispatch = Action<'Msg>(fun msg -> parentDispatch.Invoke(map.Invoke(msg)))
        
        ObservableCollectionExtensions.SyncWith(
            collection,
            models,
            modelKey,
            vmKey,
            Func<'Model, 'ViewModel>(fun m ->
                let vm = create.Invoke(m)
                vm.SetDispatch(childDispatch)
                vm.Update(m)
                vm),
            Action<'ViewModel, 'Model>(fun vm m -> vm.Update(m)))

/// Minimal base class for projections that handles dispatch.
/// Primarily intended for F# ViewModels where multiple inheritance isn't an issue.
[<AbstractClass>]
type FSharpProjectionBase<'Model, 'Msg>() =
    let mutable _dispatch : Action<'Msg> = Action<_>(ignore)
    member _.Dispatch = _dispatch
    abstract member Update : 'Model -> unit
    interface IProjection<'Model> with
        member this.Update(model) = this.Update(model)
    interface IDispatchTarget<'Msg> with
        member _.SetDispatch(dispatch) = _dispatch <- dispatch
    interface IProjection<'Model, 'Msg>

/// Helper for C# ViewModels to hold a dispatcher without manual field/method boilerplate.
type Dispatcher<'Msg>() =
    let mutable _dispatch : Action<'Msg> = Action<_>(ignore)
    member _.Connect(dispatch) = _dispatch <- dispatch
    member _.Send(msg) = _dispatch.Invoke(msg)
    interface IDispatchTarget<'Msg> with
        member this.SetDispatch(dispatch) = this.Connect(dispatch)
    interface IProjection<unit, 'Msg>
    interface IProjection<unit> with
        member _.Update(()) = ()
