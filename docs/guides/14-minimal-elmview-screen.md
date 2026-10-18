# Minimal ElmView screen

A complete, copyable ElmView screen: one editable `TextBox`, one derived
`Greeting`, and the application lifetime that wires them together. It is a
trimmed version of the form and host in
[`sample/Samples.ElmView/ExampleMatrixSample`](https://github.com/adz/Elmish.Avalonia.Glue/tree/main/sample/Samples.ElmView/ExampleMatrixSample);
run the full sample to see nested nodes, design-time snapshots, and several
screens at once.

## F# view record

In ElmView the view record is the Elmish model. The docs block omits the
file's `module` header so the tooling can compile it in place; in a project,
put this in a `.fs` file under `namespace MinimalElmView.Core` with
`module App`, then reference the types from C# as `App.View`, `App.Msg`,
`App.designView`, and `App.program`.

```fsharp
open Elmish

type View =
    { Name: string
      Greeting: string }

type Msg =
    | NameChanged of string

let designView =
    { Name = "Ada"; Greeting = "Hello, Ada!" }

let init () =
    designView, Cmd.none

let update msg view =
    match msg with
    | NameChanged name ->
        { view with
            Name = name
            Greeting = $"Hello, {name}!" },
        Cmd.none

let program =
    Program.mkProgram init update (fun _ _ -> ())
```

This block is compiled by the docs build. The [runnable ElmView
example](../examples/minimal-elmview-screen.html) executes the same host and
write-back round trip and prints the notifications.

## Host

The host is a stable CLR facade over the immutable record. It registers the
writable path once, exposes `Name` and `Greeting`, and declares both in
`GeneratedPropertyNames` so snapshot updates notify them.

```csharp
using System;
using System.Collections.Generic;
using Elmish.Avalonia.Glue.ElmView;
using MinimalElmView.Core;

public sealed class AppHost : RuntimeGeneratedViewHost<App.View, App.Msg>
{
    private static readonly Action<WriteBackBindings<App.View, App.Msg>> ConfigureBindings =
        bindings => bindings.For(x => x.Name).Dispatch(App.Msg.NewNameChanged);

    public AppHost() : base(App.designView, ConfigureBindings)
    {
    }

    public override IEnumerable<string> GeneratedPropertyNames => ["Name", "Greeting"];

    public string Name
    {
        get => View.Name;
        set => TryDispatchWriteBack("Name", value);
    }

    public string Greeting => View.Greeting;
}
```

The `Name` setter never mutates the snapshot. It calls
`TryDispatchWriteBack("Name", value)`, which looks up the route registered in
`ConfigureBindings` and dispatches `NameChanged`.

## AXAML

Standard Avalonia, compiled against the host type. The design `DataContext`
uses the same host shape without starting Elmish.

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:local="using:MinimalElmView.UI"
        x:Class="MinimalElmView.UI.MainView"
        x:DataType="local:AppHost"
        x:CompileBindings="True"
        Title="Minimal ElmView screen"
        Width="360"
        Height="160">
  <Design.DataContext>
    <local:AppHost />
  </Design.DataContext>
  <StackPanel Spacing="8" Margin="16">
    <TextBlock Text="Name" />
    <TextBox Text="{Binding Name, Mode=TwoWay}" Watermark="Type a name" />
    <TextBlock Text="{Binding Greeting}" FontWeight="SemiBold" />
  </StackPanel>
</Window>
```

The AXAML block is not F# and is not compiled by the docs tooling.

## Application lifetime

The code-behind assigns the host as `DataContext`; the application starts
Elmish and binds the host's update and dispatch callbacks.

```csharp
using Avalonia.Controls;

namespace MinimalElmView.UI;

public partial class MainView : Window
{
    public AppHost Host { get; } = new();

    public MainView()
    {
        DataContext = Host;
        InitializeComponent();
    }
}
```

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="MinimalElmView.UI.App">
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
using MinimalElmView.Core;

namespace MinimalElmView.UI;

public class App : Application
{
    private IDisposable? _host;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var view = new MainView();

            _host = ElmishHost.startAndBind(
                App.program,
                view.Host.Update,
                view.Host.SetDispatch);

            desktop.MainWindow = view;
            desktop.Exit += (_, _) => _host?.Dispose();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
```

```csharp
using Avalonia;
using MinimalElmView.UI;

AppBuilder
    .Configure<App>()
    .UsePlatformDetect()
    .StartWithClassicDesktopLifetime(args);
```

## TwoWay edit and notification

Typing `Grace` follows one loop:

```text
TextBox writes Name = "Grace"
→ AppHost.Name calls TryDispatchWriteBack("Name", "Grace")
→ the registered route dispatches NameChanged "Grace"
→ Elmish update returns { Name = "Grace"; Greeting = "Hello, Grace!" }
→ ElmishHost posts host.Update on the UI thread
→ host raises PropertyChanged for View, then Name, then Greeting
→ the TextBox and greeting TextBlock refresh
```

The setter itself only dispatches; the notification comes from the snapshot
update. A route that is not registered returns `false` and dispatches nothing.

Next, see the same screen written [as Projection](minimal-projection-screen.html),
or the [error-handling recipes](error-handling.html) for what can go wrong at
each step.
