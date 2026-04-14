namespace Elmish.Avalonia.Glue.ElmView

open System
open System.Collections.Generic
open System.ComponentModel
open System.Linq.Expressions

module private WriteBackPath =
    let rec private collectSegments segments (expression: Expression | null) =
        match expression with
        | null ->
            invalidArg
                (nameof expression)
                "Write-back bindings only support simple property-access paths such as x => x.UserInput.Name."
        | :? MemberExpression as memberExpression ->
            collectSegments (memberExpression.Member.Name :: segments) memberExpression.Expression
        | :? ParameterExpression -> segments
        | :? UnaryExpression as unaryExpression
            when unaryExpression.NodeType = ExpressionType.Convert
                 || unaryExpression.NodeType = ExpressionType.ConvertChecked ->
            collectSegments segments unaryExpression.Operand
        | _ ->
            invalidArg
                (nameof expression)
                "Write-back bindings only support simple property-access paths such as x => x.UserInput.Name."

    let fromSelector (selector: Expression<Func<'View, 'Value>>) =
        if isNull (box selector) then
            nullArg (nameof selector)

        let segments = collectSegments [] selector.Body

        match segments with
        | [] ->
            invalidArg
                (nameof selector)
                "Write-back bindings require at least one readable property in the selector."
        | _ -> String.Join(".", segments)

type WriteBackBindingRegistration<'Owner, 'Msg, 'Value> internal
    (
        owner: 'Owner,
        propertyPath: string,
        register: (obj | null -> 'Msg) -> unit
    ) =
    member _.PropertyPath = propertyPath

    member _.Dispatch(map: Func<'Value, 'Msg>) =
        if isNull (box map) then
            nullArg (nameof map)

        register (unbox<'Value> >> map.Invoke)
        owner

    member _.Dispatch(map: 'Value -> 'Msg) =
        register (unbox<'Value> >> map)
        owner

type WriteBackBindings<'View, 'Msg>() as this =
    let routes = Dictionary<string, obj | null -> 'Msg>(StringComparer.Ordinal)

    let registerDispatcher (propertyPath: string) (map: obj | null -> 'Msg) =
        if String.IsNullOrWhiteSpace propertyPath then
            invalidArg (nameof propertyPath) "Write-back bindings require a non-empty property path."

        if routes.ContainsKey propertyPath then
            invalidOp $"A write-back binding for '{propertyPath}' is already registered."

        routes.Add(propertyPath, map)

    member _.For<'Value>(selector: Expression<Func<'View, 'Value>>) =
        let propertyPath = WriteBackPath.fromSelector selector
        WriteBackBindingRegistration<WriteBackBindings<'View, 'Msg>, 'Msg, 'Value>(
            this,
            propertyPath,
            registerDispatcher propertyPath)

    member _.Paths = routes.Keys :> seq<string>

    member internal _.TryDispatch(propertyPath: string, value: obj | null, dispatch: 'Msg -> unit) =
        match routes.TryGetValue propertyPath with
        | true, map ->
            dispatch (map value)
            true
        | false, _ -> false

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
type GeneratedViewHost<'View, 'Msg when 'View : not struct>
    (
        initialView: 'View,
        configureBindings: Action<WriteBackBindings<'View, 'Msg>>
    ) =
    inherit BindableViewHost<'View>(initialView)

    let children = ResizeArray<IGeneratedViewNode>()
    let mutable dispatch = ignore<'Msg>
    let writeBackBindings = WriteBackBindings<'View, 'Msg>()

    do
        if isNull (box configureBindings) |> not then
            configureBindings.Invoke(writeBackBindings)

    new(initialView: 'View) =
        GeneratedViewHost(initialView, Action<WriteBackBindings<'View, 'Msg>>(ignore))

    member _.SetDispatch(dispatcher: 'Msg -> unit) =
        dispatch <- dispatcher

    member _.SetDispatch(dispatcher: Action<'Msg>) =
        dispatch <- dispatcher.Invoke

    member _.Dispatch(message: 'Msg) =
        dispatch message

    member _.WriteBackBindings = writeBackBindings

    member _.TryDispatchWriteBack<'Value>(propertyPath: string, value: 'Value) =
        writeBackBindings.TryDispatch(propertyPath, box value, dispatch)

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

type RuntimeGeneratedViewHost<'View, 'Msg when 'View : not struct>
    (
        initialView: 'View,
        configureBindings: Action<WriteBackBindings<'View, 'Msg>>
    ) =
    inherit GeneratedViewHost<'View, 'Msg>(initialView, configureBindings)

    new(initialView: 'View) =
        RuntimeGeneratedViewHost(initialView, Action<WriteBackBindings<'View, 'Msg>>(ignore))

type DesignGeneratedViewHost<'View, 'Msg when 'View : not struct>
    (
        sampleView: 'View,
        configureBindings: Action<WriteBackBindings<'View, 'Msg>>
    ) =
    inherit GeneratedViewHost<'View, 'Msg>(sampleView, configureBindings)

    new(sampleView: 'View) =
        DesignGeneratedViewHost(sampleView, Action<WriteBackBindings<'View, 'Msg>>(ignore))

module ElmView =
    let runtime initialView = RuntimeViewHost(initialView)
    let design sampleView = DesignViewHost(sampleView)
