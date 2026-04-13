namespace Elmish.Avalonia.Glue.ElmView

open System.ComponentModel

[<AbstractClass>]
type BindableViewHost<'View when 'View : not struct>(initialView: 'View) =
    let propertyChanged = Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()
    let mutable currentView = initialView

    [<CLIEvent>]
    member _.PropertyChanged = propertyChanged.Publish

    member _.View = currentView

    member this.Update(nextView: 'View) =
        if not (obj.ReferenceEquals(currentView, nextView)) then
            currentView <- nextView
            propertyChanged.Trigger(this, PropertyChangedEventArgs(nameof this.View))

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member _.PropertyChanged = propertyChanged.Publish

type RuntimeViewHost<'View when 'View : not struct>(initialView: 'View) =
    inherit BindableViewHost<'View>(initialView)

type DesignViewHost<'View when 'View : not struct>(sampleView: 'View) =
    inherit BindableViewHost<'View>(sampleView)

module ElmView =
    let runtime initialView = RuntimeViewHost(initialView)
    let design sampleView = DesignViewHost(sampleView)
