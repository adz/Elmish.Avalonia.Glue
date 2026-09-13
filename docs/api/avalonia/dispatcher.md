---
sidebar_position: 2
---

# Dispatcher bridge

The `ElmishHost` class in the `Elmish.Avalonia.Glue` package provides the primary entry points for starting an Elmish program and binding it to an Avalonia view. It ensures that all UI updates are correctly marshaled to Avalonia's UI thread.

## `ElmishHost`

### Static Methods

| Method | Description |
| :--- | :--- |
| `start(program, onUpdate)` | Starts an Elmish program and calls `onUpdate` on the Avalonia UI thread whenever the model changes. Returns an `IDisposable` to stop the program. |
| `startAndBind(program, onUpdate, setDispatch)` | Similar to `start`, but also calls `setDispatch` with the Elmish `dispatch` function. This is the most common way to start a program and connect it to a view host. |

## How it works

1. **Threading**: The `start` methods use `Avalonia.Threading.Dispatcher.UIThread.Post` to ensure that the `onUpdate` callback is always executed on the UI thread, which is a requirement for updating Avalonia controls.
2. **Lifetime**: It returns an `IDisposable` (the `ElmishHostConnection`). Disposing of this object will stop the Elmish loop and clean up any resources.
3. **Binding**: By calling `setDispatch`, it allows your view host (whether it's an ElmView host or a Projection host) to receive the `dispatch` function and start sending messages back to Elmish.

## Usage Example

```csharp
public override void OnFrameworkInitializationCompleted()
{
    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        var view = new MainView();

        // Start Elmish and bind it to the view's host
        _host = ElmishHost.startAndBind(
            App.program,
            view.Host.Update,      // Update the host snapshot
            view.Host.SetDispatch  // Provide the dispatcher
        );

        desktop.MainWindow = view;
    }
}
```

## Source

- [AvaloniaHost.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue/AvaloniaHost.fs)
