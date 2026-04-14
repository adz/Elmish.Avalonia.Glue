namespace Elmish.Avalonia.Glue.Tests

open System
open System.Collections.Generic
open System.ComponentModel
open System.Linq.Expressions
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.Primitives
open Avalonia.Data
open Elmish.Avalonia.Glue.ElmView
open Xunit

module ElmViewGeneratedHostTests =

    type private ChildView =
        {
            Name: string
            IsEnabled: bool
        }

    type private FormView =
        {
            Newsletter: bool
            FavoriteLanguage: string
            Languages: string list
            Experience: float
            Notes: string
        }

    type private RootView =
        {
            Title: string
            Child: ChildView
            Form: FormView
        }

    type private DetailView =
        {
            Label: string
            Count: int
        }

    type private NestedChildView =
        {
            Name: string
            Detail: DetailView
        }

    type private NestedRootView =
        {
            Title: string
            Child: NestedChildView
        }

    type private Msg =
        | SetName of string
        | SetNewsletter of bool
        | SetFavoriteLanguage of string
        | SetExperience of float
        | SetNotes of string

    type private NestedMsg =
        | NoOp

    let private childNameSelector () =
        let root = Expression.Parameter(typeof<RootView>, "x")
        let child = Expression.Property(root, nameof Unchecked.defaultof<RootView>.Child)
        let name = Expression.Property(child, nameof Unchecked.defaultof<ChildView>.Name)
        Expression.Lambda<Func<RootView, string>>(name, root)

    let private formNewsletterSelector () =
        let root = Expression.Parameter(typeof<RootView>, "x")
        let form = Expression.Property(root, nameof Unchecked.defaultof<RootView>.Form)
        let newsletter = Expression.Property(form, nameof Unchecked.defaultof<FormView>.Newsletter)
        Expression.Lambda<Func<RootView, bool>>(newsletter, root)

    let private formFavoriteLanguageSelector () =
        let root = Expression.Parameter(typeof<RootView>, "x")
        let form = Expression.Property(root, nameof Unchecked.defaultof<RootView>.Form)
        let favoriteLanguage = Expression.Property(form, nameof Unchecked.defaultof<FormView>.FavoriteLanguage)
        Expression.Lambda<Func<RootView, string>>(favoriteLanguage, root)

    let private formExperienceSelector () =
        let root = Expression.Parameter(typeof<RootView>, "x")
        let form = Expression.Property(root, nameof Unchecked.defaultof<RootView>.Form)
        let experience = Expression.Property(form, nameof Unchecked.defaultof<FormView>.Experience)
        Expression.Lambda<Func<RootView, float>>(experience, root)

    let private formNotesSelector () =
        let root = Expression.Parameter(typeof<RootView>, "x")
        let form = Expression.Property(root, nameof Unchecked.defaultof<RootView>.Form)
        let notes = Expression.Property(form, nameof Unchecked.defaultof<FormView>.Notes)
        Expression.Lambda<Func<RootView, string>>(notes, root)

    let private configureBindings =
        Action<WriteBackBindings<RootView, Msg>>(fun bindings ->
            bindings.For(childNameSelector()).Dispatch(Func<string, Msg>(SetName)) |> ignore
            bindings.For(formNewsletterSelector()).Dispatch(Func<bool, Msg>(SetNewsletter)) |> ignore
            bindings.For(formFavoriteLanguageSelector()).Dispatch(Func<string, Msg>(SetFavoriteLanguage)) |> ignore
            bindings.For(formExperienceSelector()).Dispatch(Func<float, Msg>(SetExperience)) |> ignore
            bindings.For(formNotesSelector()).Dispatch(Func<string, Msg>(SetNotes)) |> ignore)

    type private SampleHost(initialView: RootView) as this =
        inherit RuntimeGeneratedViewHost<RootView, Msg>(initialView, configureBindings)

        let child = SampleChildNode(this)
        let form = SampleFormNode(this)

        override _.GeneratedPropertyNames =
            [ "Title"; "Child"; "Form" ]

        member this.Title = this.View.Title
        member _.Child = child
        member _.Form = form

    and private SampleChildNode(host: SampleHost) =
        inherit GeneratedViewNode<RootView, ChildView, Msg>(
            (fun () -> host.View),
            host.Dispatch,
            host.RegisterChildNode,
            (fun root -> root.Child),
            [ "Name"; "IsEnabled" ])

        member this.Name
            with get () = this.Snapshot.Name
            and set (value: string) = host.TryDispatchWriteBack("Child.Name", value) |> ignore

        member this.IsEnabled = this.Snapshot.IsEnabled

    and private SampleFormNode(host: SampleHost) =
        inherit GeneratedViewNode<RootView, FormView, Msg>(
            (fun () -> host.View),
            host.Dispatch,
            host.RegisterChildNode,
            (fun root -> root.Form),
            [ "Newsletter"; "FavoriteLanguage"; "Languages"; "Experience"; "Notes" ])

        member this.Newsletter
            with get () = this.Snapshot.Newsletter
            and set (value: bool) = host.TryDispatchWriteBack("Form.Newsletter", value) |> ignore

        member this.FavoriteLanguage
            with get () = this.Snapshot.FavoriteLanguage
            and set (value: string) = host.TryDispatchWriteBack("Form.FavoriteLanguage", value) |> ignore

        member this.Languages = this.Snapshot.Languages

        member this.Experience
            with get () = this.Snapshot.Experience
            and set (value: float) = host.TryDispatchWriteBack("Form.Experience", value) |> ignore

        member this.Notes
            with get () = this.Snapshot.Notes
            and set (value: string) = host.TryDispatchWriteBack("Form.Notes", value) |> ignore

    let private createView title name isEnabled =
        {
            Title = title
            Child =
                {
                    Name = name
                    IsEnabled = isEnabled
                }
            Form =
                {
                    Newsletter = false
                    FavoriteLanguage = "F#"
                    Languages = [ "F#"; "C#"; "Rust" ]
                    Experience = 3.0
                    Notes = "Original notes"
                }
        }

    type private NestedHost(initialView: NestedRootView) as this =
        inherit RuntimeGeneratedViewHost<NestedRootView, NestedMsg>(initialView)

        let child = NestedChildNode(this)

        override _.GeneratedPropertyNames =
            [ "Title"; "Child" ]

        member this.Title = this.View.Title
        member _.Child = child

    and private NestedChildNode(host: NestedHost) as this =
        inherit GeneratedViewNode<NestedRootView, NestedChildView, NestedMsg>(
            (fun () -> host.View),
            host.Dispatch,
            host.RegisterChildNode,
            (fun root -> root.Child),
            [ "Name"; "Detail" ])

        let detail = NestedDetailNode(host, this)

        member this.Name = this.Snapshot.Name
        member _.Detail = detail

    and private NestedDetailNode(host: NestedHost, parent: NestedChildNode) =
        inherit GeneratedViewNode<NestedRootView, DetailView, NestedMsg>(
            (fun () -> host.View),
            host.Dispatch,
            parent.RegisterChildNode,
            (fun root -> root.Child.Detail),
            [ "Label"; "Count" ])

        member this.Label = this.Snapshot.Label
        member this.Count = this.Snapshot.Count

    let private createNestedView title name label count =
        {
            Title = title
            Child =
                {
                    Name = name
                    Detail =
                        {
                            Label = label
                            Count = count
                        }
                }
        }

    [<Fact>]
    let ``generated host getters read the latest immutable snapshot`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let child = host.Child

        Assert.Equal("Before", host.Title)
        Assert.Equal("Ada", host.Child.Name)
        Assert.True(host.Child.IsEnabled)

        host.Update(createView "After" "Linus" false)

        Assert.Equal("After", host.Title)
        Assert.Equal("Linus", host.Child.Name)
        Assert.False(host.Child.IsEnabled)
        Assert.Same(child, host.Child)

    [<Fact>]
    let ``generated writable setters dispatch messages instead of mutating snapshots`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()

        host.SetDispatch(Action<Msg>(messages.Add))
        host.Child.Name <- "Grace"

        Assert.Equal<Msg list>([ SetName "Grace" ], Seq.toList messages)
        Assert.Equal("Ada", host.Child.Name)

    [<Fact>]
    let ``two-way bindings dispatch through generated writable properties`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()
        let textBox = TextBox()
        let binding = ReflectionBinding("Child.Name")

        host.SetDispatch(Action<Msg>(messages.Add))
        binding.Source <- host
        binding.Mode <- BindingMode.TwoWay
        binding.UpdateSourceTrigger <- UpdateSourceTrigger.PropertyChanged
        textBox.Bind(TextBox.TextProperty, binding) |> ignore

        Assert.Equal("Ada", textBox.Text)

        textBox.Text <- "Grace"

        Assert.Equal<Msg list>([ SetName "Grace" ], Seq.toList messages)
        Assert.Equal("Ada", host.Child.Name)

    [<Fact>]
    let ``text input bindings dispatch through generated writable text properties`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()
        let textBox = TextBox()
        let binding = ReflectionBinding("Child.Name")

        host.SetDispatch(Action<Msg>(messages.Add))
        binding.Source <- host
        binding.Mode <- BindingMode.TwoWay
        binding.UpdateSourceTrigger <- UpdateSourceTrigger.PropertyChanged
        textBox.Bind(TextBox.TextProperty, binding) |> ignore

        Assert.Equal("Ada", textBox.Text)

        textBox.Text <- "Grace"

        Assert.Equal<Msg list>([ SetName "Grace" ], Seq.toList messages)
        Assert.Equal("Ada", host.Child.Name)

    [<Fact>]
    let ``check box bindings dispatch through generated writable boolean properties`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()
        let checkBox = CheckBox()
        let binding = ReflectionBinding("Form.Newsletter")

        host.SetDispatch(Action<Msg>(messages.Add))
        binding.Source <- host
        binding.Mode <- BindingMode.TwoWay
        checkBox.Bind(ToggleButton.IsCheckedProperty, binding) |> ignore

        Assert.False(checkBox.IsChecked.GetValueOrDefault())

        checkBox.IsChecked <- Nullable true

        Assert.Equal<Msg list>([ SetNewsletter true ], Seq.toList messages)
        Assert.False(host.Form.Newsletter)

    [<Fact>]
    let ``combo box bindings dispatch through generated writable selection properties`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()
        let comboBox = ComboBox()
        let itemsBinding = ReflectionBinding("Form.Languages")
        let selectionBinding = ReflectionBinding("Form.FavoriteLanguage")

        host.SetDispatch(Action<Msg>(messages.Add))

        itemsBinding.Source <- host
        comboBox.Bind(ItemsControl.ItemsSourceProperty, itemsBinding) |> ignore

        selectionBinding.Source <- host
        selectionBinding.Mode <- BindingMode.TwoWay
        comboBox.Bind(SelectingItemsControl.SelectedItemProperty, selectionBinding) |> ignore

        Assert.Equal("F#", comboBox.SelectedItem :?> string)

        comboBox.SelectedItem <- "Rust"

        Assert.Equal<Msg list>([ SetFavoriteLanguage "Rust" ], Seq.toList messages)
        Assert.Equal("F#", host.Form.FavoriteLanguage)

    [<Fact>]
    let ``slider bindings dispatch through generated writable range properties`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()
        let slider = Slider()
        let binding = ReflectionBinding("Form.Experience")

        host.SetDispatch(Action<Msg>(messages.Add))
        binding.Source <- host
        binding.Mode <- BindingMode.TwoWay
        slider.Bind(RangeBase.ValueProperty, binding) |> ignore

        Assert.Equal(3.0, slider.Value)

        slider.Value <- 7.0

        Assert.Equal<Msg list>([ SetExperience 7.0 ], Seq.toList messages)
        Assert.Equal(3.0, host.Form.Experience)

    [<Fact>]
    let ``multiline text bindings dispatch through generated writable note properties`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()
        let textBox = TextBox(AcceptsReturn = true, TextWrapping = Avalonia.Media.TextWrapping.Wrap)
        let binding = ReflectionBinding("Form.Notes")

        host.SetDispatch(Action<Msg>(messages.Add))
        binding.Source <- host
        binding.Mode <- BindingMode.TwoWay
        binding.UpdateSourceTrigger <- UpdateSourceTrigger.PropertyChanged
        textBox.Bind(TextBox.TextProperty, binding) |> ignore

        Assert.Equal("Original notes", textBox.Text)

        textBox.Text <- "Line one\nLine two"

        Assert.Equal<Msg list>([ SetNotes "Line one\nLine two" ], Seq.toList messages)
        Assert.Equal("Original notes", host.Form.Notes)

    [<Fact>]
    let ``snapshot-driven binding updates do not re-dispatch messages`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()
        let textBox = TextBox()
        let binding = ReflectionBinding("Child.Name")

        host.SetDispatch(Action<Msg>(messages.Add))
        binding.Source <- host
        binding.Mode <- BindingMode.TwoWay
        binding.UpdateSourceTrigger <- UpdateSourceTrigger.PropertyChanged
        textBox.Bind(TextBox.TextProperty, binding) |> ignore

        Assert.Equal("Ada", textBox.Text)

        host.Update(createView "After" "Grace" true)

        Assert.Equal("Grace", textBox.Text)
        Assert.Empty(messages)

    [<Fact>]
    let ``user edits dispatch exactly once even after the snapshot catches up`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()
        let textBox = TextBox()
        let binding = ReflectionBinding("Child.Name")

        host.SetDispatch(Action<Msg>(messages.Add))
        binding.Source <- host
        binding.Mode <- BindingMode.TwoWay
        binding.UpdateSourceTrigger <- UpdateSourceTrigger.PropertyChanged
        textBox.Bind(TextBox.TextProperty, binding) |> ignore

        textBox.Text <- "Grace"

        Assert.Equal<Msg list>([ SetName "Grace" ], Seq.toList messages)

        host.Update(createView "After" "Grace" true)

        Assert.Equal("Grace", textBox.Text)
        Assert.Equal<Msg list>([ SetName "Grace" ], Seq.toList messages)

    [<Fact>]
    let ``one-way bindings stay display-only and do not dispatch`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()
        let textBox = TextBox()
        let binding = ReflectionBinding("Child.Name")

        host.SetDispatch(Action<Msg>(messages.Add))
        binding.Source <- host
        binding.Mode <- BindingMode.OneWay
        textBox.Bind(TextBox.TextProperty, binding) |> ignore

        Assert.Equal("Ada", textBox.Text)

        textBox.Text <- "Grace"

        Assert.Empty(messages)
        Assert.Equal("Ada", host.Child.Name)

    [<Fact>]
    let ``only explicitly registered write-back paths dispatch`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let messages = List<Msg>()

        host.SetDispatch(Action<Msg>(messages.Add))

        let dispatchedMapped = host.TryDispatchWriteBack("Child.Name", "Grace")
        let dispatchedUnmapped = host.TryDispatchWriteBack("Child.IsEnabled", true)

        Assert.True(dispatchedMapped)
        Assert.False(dispatchedUnmapped)
        Assert.Equal<Msg list>([ SetName "Grace" ], Seq.toList messages)

    [<Fact>]
    let ``write-back bindings expose the configured nested property path`` () =
        let host = SampleHost(createView "Before" "Ada" true)

        Assert.Equal<string list>(
            [ "Child.Name"; "Form.Newsletter"; "Form.FavoriteLanguage"; "Form.Experience"; "Form.Notes" ],
            Seq.toList host.WriteBackBindings.Paths)

    [<Fact>]
    let ``snapshot updates notify root and nested generated properties`` () =
        let host = SampleHost(createView "Before" "Ada" true)
        let changed = ResizeArray<string>()

        (host :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add(args.PropertyName))
        (host.Child :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add($"Child.{args.PropertyName}"))
        (host.Form :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add($"Form.{args.PropertyName}"))

        host.Update(createView "After" "Grace" false)

        Assert.Equal<string list>(
            [
                "View"
                "Title"
                "Child"
                "Form"
                "Child.Name"
                "Child.IsEnabled"
                "Form.Newsletter"
                "Form.FavoriteLanguage"
                "Form.Languages"
                "Form.Experience"
                "Form.Notes"
            ],
            Seq.toList changed)

    [<Fact>]
    let ``nested generated nodes stay stable and read the latest snapshot`` () =
        let host = NestedHost(createNestedView "Before" "Ada" "One" 1)
        let child = host.Child
        let detail = host.Child.Detail

        Assert.Equal("Before", host.Title)
        Assert.Equal("Ada", host.Child.Name)
        Assert.Equal("One", host.Child.Detail.Label)
        Assert.Equal(1, host.Child.Detail.Count)

        host.Update(createNestedView "After" "Grace" "Two" 2)

        Assert.Equal("After", host.Title)
        Assert.Equal("Grace", host.Child.Name)
        Assert.Equal("Two", host.Child.Detail.Label)
        Assert.Equal(2, host.Child.Detail.Count)
        Assert.Same(child, host.Child)
        Assert.Same(detail, host.Child.Detail)

    [<Fact>]
    let ``snapshot updates propagate PropertyChanged through nested bindable nodes`` () =
        let host = NestedHost(createNestedView "Before" "Ada" "One" 1)
        let changed = ResizeArray<string>()

        (host :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add($"Host.{args.PropertyName}"))
        (host.Child :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add($"Child.{args.PropertyName}"))
        (host.Child.Detail :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add($"Detail.{args.PropertyName}"))

        host.Update(createNestedView "After" "Grace" "Two" 2)

        Assert.Equal<string list>(
            [
                "Host.View"
                "Host.Title"
                "Host.Child"
                "Child.Name"
                "Child.Detail"
                "Detail.Label"
                "Detail.Count"
            ],
            Seq.toList changed)

    [<Fact>]
    let ``updating with the same snapshot reference does not raise root or nested PropertyChanged`` () =
        let initialView = createNestedView "Before" "Ada" "One" 1
        let host = NestedHost(initialView)
        let changed = ResizeArray<string>()

        (host :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add($"Host.{args.PropertyName}"))
        (host.Child :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add($"Child.{args.PropertyName}"))
        (host.Child.Detail :> INotifyPropertyChanged).PropertyChanged.Add(fun args -> changed.Add($"Detail.{args.PropertyName}"))

        host.Update(initialView)

        Assert.Empty(changed)
