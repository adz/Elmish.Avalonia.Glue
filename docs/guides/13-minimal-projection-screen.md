# Minimal Projection screen

A complete, copyable Projection screen: one editable `TextBox`, one derived
`Greeting`, and the application lifetime that wires them together. It is a
trimmed version of the form and startup code in
[`sample/Samples.Projection/ExampleMatrixSample`](https://github.com/adz/Elmish.Avalonia.Glue/tree/main/sample/Samples.Projection/ExampleMatrixSample);
run the full sample to see several screens, design-time data, and row
projections at once.

## F# core

Model the state and the user intent. The docs block omits the file's `module`
header so the tooling can compile it in place; in a project, put this in a
`.fs` file under `namespace MinimalProjection.Core` with `module App`, then
reference the types from C# as `App.Model`, `App.Msg`, and `App.program`.

```fsharp
open Elmish

type Model =
    { Name: string
      Greeting: string }

type Msg =
    | NameChanged of string

let init () =
    { Name = "Ada"; Greeting = "Hello, Ada!" },
    Cmd.none

let update msg model =
    match msg with
    | NameChanged name ->
        { model with
            Name = name
            Greeting = $"Hello, {name}!" },
        Cmd.none

let program =
    Program.mkProgram init update (fun _ _ -> ())
```

This block is compiled by the docs build. The [runnable projection
example](../examples/minimal-projection-screen.html) executes the same
transition against the core projection base and prints the notifications.

## Viewmodel

The projection is the stable `DataContext`. `Name` accepts a TwoWay write and
dispatches `NameChanged`; `Greeting` is read-only and only published by
`Update`. The `_isUpdating` guard stops a model-driven write from looking like
another user edit.

```csharp
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MinimalProjection.Core;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private Action<App.Msg> _dispatch = _ => { };
    private bool _isUpdating;

    private string _name = "";
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value) return;
            _name = value;
            OnPropertyChanged();
            if (!_isUpdating) _dispatch(App.Msg.NewNameChanged(value));
        }
    }

    private string _greeting = "";
    public string Greeting
    {
        get => _greeting;
        private set
        {
            if (_greeting == value) return;
            _greeting = value;
            OnPropertyChanged();
        }
    }

    public void Update(App.Model model)
    {
        _isUpdating = true;
        Name = model.Name;
        Greeting = model.Greeting;
        _isUpdating = false;
    }

    public void SetDispatch(Action<App.Msg> dispatch) => _dispatch = dispatch;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
```

The sample's `FormPageProjection` reaches the same effect with
CommunityToolkit.Mvvm `[ObservableProperty]` partial properties and
`OnNameChanged` partial methods. This hand-written version keeps the minimal
screen dependency-free.

## AXAML

Standard Avalonia, with compiled bindings against the viewmodel type.

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:vm="using:MinimalProjection.UI"
        x:Class="MinimalProjection.UI.MainView"
        x:DataType="vm:MainViewModel"
        x:CompileBindings="True"
        Title="Minimal Projection screen"
        Width="360"
        Height="160">
  <StackPanel Spacing="8" Margin="16">
    <TextBlock Text="Name" />
    <TextBox Text="{Binding Name, Mode=TwoWay}" Watermark="Type a name" />
    <TextBlock Text="{Binding Greeting}" FontWeight="SemiBold" />
  </StackPanel>
</Window>
```

The AXAML block is not F# and is not compiled by the docs tooling.

## Application lifetime

The code-behind only loads the view; the application assigns the viewmodel and
starts Elmish.

```csharp
using Avalonia.Controls;

namespace MinimalProjection.UI;

public partial class MainView : Window
{
    public MainView() => InitializeComponent();
}
```

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="MinimalProjection.UI.App">
  <Application.Styles>
    <FluentTheme />
  </Application.Styles>
</Application>
```

```csharp
using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Elmish.Avalonia.Glue;
using MinimalProjection.Core;

namespace MinimalProjection.UI;

public class App : Application
{
    private IDisposable? _host;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = new MainViewModel();

            _host = ElmishHost.startAndBind(
                App.program,
                model => vm.Update(model),
                vm.SetDispatch);

            desktop.MainWindow = new MainView { DataContext = vm };
            desktop.Exit += (_, _) => _host?.Dispose();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
```

```csharp
using Avalonia;
using MinimalProjection.UI;

AppBuilder
    .Configure<App>()
    .UsePlatformDetect()
    .StartWithClassicDesktopLifetime(args);
```

## TwoWay edit and notification

Typing `Grace` follows one loop:

```text
TextBox writes Name = "Grace"
→ MainViewModel.Name raises PropertyChanged("Name") and dispatches NameChanged
→ Elmish update returns { Name = "Grace"; Greeting = "Hello, Grace!" }
→ ElmishHost posts vm.Update on the UI thread
→ Update publishes Name (unchanged) and Greeting (PropertyChanged)
→ the greeting TextBlock refreshes
```

One edit dispatches one message; one snapshot update dispatches none. The
`_isUpdating` guard is what keeps `Update` from re-dispatching.

Next, see the same screen written [as ElmView](minimal-elmview-screen.html), or
the [error-handling recipes](error-handling.html) for what can go wrong at each
step.
