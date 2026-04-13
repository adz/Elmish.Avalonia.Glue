namespace Elmish.Avalonia.Glue

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open System.Runtime.CompilerServices

type IProjection<'Model> =
    abstract member Update : 'Model -> unit

type IDispatchTarget<'Msg> =
    abstract member SetDispatch : Action<'Msg> -> unit

type IProjection<'Model, 'Msg> =
    inherit IProjection<'Model>
    inherit IDispatchTarget<'Msg>

[<Extension>]
type ProjectionExtensions =
    [<Extension>]
    static member ForwardTo<'Msg, 'ParentMsg>
        (
            projection: IDispatchTarget<'Msg>,
            dispatch: Action<'ParentMsg>,
            map: Func<'Msg, 'ParentMsg>
        ) : unit =
        projection.SetDispatch(Action<'Msg>(fun msg -> dispatch.Invoke(map.Invoke(msg))))

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

type Dispatcher<'Msg>() =
    let mutable _dispatch : Action<'Msg> = Action<_>(ignore)
    member _.Connect(dispatch) = _dispatch <- dispatch
    member _.Send(msg) = _dispatch.Invoke(msg)
    interface IDispatchTarget<'Msg> with
        member this.SetDispatch(dispatch) = this.Connect(dispatch)
    interface IProjection<unit, 'Msg>
    interface IProjection<unit> with
        member _.Update(()) = ()
