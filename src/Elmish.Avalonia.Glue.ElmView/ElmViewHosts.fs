namespace Elmish.Avalonia.Glue.ElmView

open System
open System.Collections.Generic
open System.ComponentModel

[<AbstractClass>]
type BindableViewHost<'View when 'View : not struct>(initialView: 'View) =
    let propertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()
    let mutable currentView = initialView

    [<CLIEvent>]
    member _.PropertyChanged = propertyChanged.Publish

    member _.View = currentView

    member this.NotifyPropertyChanged(propertyName: string) =
        propertyChanged.Trigger(this, PropertyChangedEventArgs(propertyName))

    abstract member OnViewUpdated: previousView: 'View * nextView: 'View -> unit
    default _.OnViewUpdated(_, _) = ()

    member this.Update(nextView: 'View) =
        if not (obj.ReferenceEquals(currentView, nextView)) then
            let previousView = currentView
            currentView <- nextView
            this.NotifyPropertyChanged(nameof this.View)
            this.OnViewUpdated(previousView, nextView)

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member _.PropertyChanged = propertyChanged.Publish

type IGeneratedViewNode =
    abstract RefreshSubtree: unit -> unit

[<AbstractClass>]
type GeneratedViewHost<'View, 'Msg when 'View : not struct>(initialView: 'View) =
    inherit BindableViewHost<'View>(initialView)

    let children = ResizeArray<IGeneratedViewNode>()
    let mutable dispatch = ignore<'Msg>

    member _.SetDispatch(dispatcher: 'Msg -> unit) =
        dispatch <- dispatcher

    member _.SetDispatch(dispatcher: Action<'Msg>) =
        dispatch <- dispatcher.Invoke

    member _.Dispatch(message: 'Msg) =
        dispatch message

    member _.RegisterChildNode(child: IGeneratedViewNode) =
        children.Add(child)

    abstract member GeneratedPropertyNames: seq<string>
    default _.GeneratedPropertyNames = Seq.empty

    override this.OnViewUpdated(_, _) =
        for propertyName in this.GeneratedPropertyNames do
            this.NotifyPropertyChanged(propertyName)

        for child in children do
            child.RefreshSubtree()

[<AbstractClass>]
type GeneratedViewNode<'RootView, 'NodeView, 'Msg when 'RootView : not struct>
    (
        getRootView: unit -> 'RootView,
        dispatch: 'Msg -> unit,
        registerWithParent: IGeneratedViewNode -> unit,
        getNodeView: 'RootView -> 'NodeView,
        propertyNames: seq<string>
    ) as this =
    let propertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()
    let children = ResizeArray<IGeneratedViewNode>()
    let propertyNames = propertyNames |> Seq.toArray

    do registerWithParent (this :> IGeneratedViewNode)

    member _.Snapshot = getNodeView (getRootView())

    member _.Dispatch(message: 'Msg) =
        dispatch message

    member _.RegisterChildNode(child: IGeneratedViewNode) =
        children.Add(child)

    member this.NotifyPropertyChanged(propertyName: string) =
        propertyChanged.Trigger(this, PropertyChangedEventArgs(propertyName))

    member private this.RefreshSelf() =
        for propertyName in propertyNames do
            this.NotifyPropertyChanged(propertyName)

    member private this.RefreshSubtreeCore() =
        this.RefreshSelf()

        for child in children do
            child.RefreshSubtree()

    [<CLIEvent>]
    member _.PropertyChanged = propertyChanged.Publish

    interface IGeneratedViewNode with
        member this.RefreshSubtree() =
            this.RefreshSubtreeCore()

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member _.PropertyChanged = propertyChanged.Publish

type RuntimeViewHost<'View when 'View : not struct>(initialView: 'View) =
    inherit BindableViewHost<'View>(initialView)

type DesignViewHost<'View when 'View : not struct>(sampleView: 'View) =
    inherit BindableViewHost<'View>(sampleView)

type RuntimeGeneratedViewHost<'View, 'Msg when 'View : not struct>(initialView: 'View) =
    inherit GeneratedViewHost<'View, 'Msg>(initialView)

type DesignGeneratedViewHost<'View, 'Msg when 'View : not struct>(sampleView: 'View) =
    inherit GeneratedViewHost<'View, 'Msg>(sampleView)

module ElmView =
    let runtime initialView = RuntimeViewHost(initialView)
    let design sampleView = DesignViewHost(sampleView)
