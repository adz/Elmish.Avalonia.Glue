# Dispatcher bridge

`Elmish.Avalonia.Glue.ElmishHost` starts an Elmish program, binds it to an Avalonia view, and marshals UI updates to Avalonia's UI thread.

## `ElmishHost`

### Static Methods

| Method | Description |
| :--- | :--- |
| `start(program, onUpdate)` | Starts an Elmish program and calls `onUpdate` on the Avalonia UI thread whenever the model changes. Returns an `IDisposable` to stop the program. |
| `startAndBind(program, onUpdate, setDispatch)` | Similar to `start`, but also calls `setDispatch` with the Elmish `dispatch` function. This is the most common way to start a program and connect it to a view host. |

## Threading and lifetime

1. **Threading**: The `start` methods run callbacks inline when `Dispatcher.UIThread.CheckAccess()` succeeds and otherwise schedule them with `Dispatcher.UIThread.Post` to run on the UI thread, which is a requirement for updating Avalonia controls.
2. **Lifetime**: Both methods return `ElmishHostConnection<'Msg>` implementing `IDisposable`. Disposing sends a termination message through the queue; subscriptions are cleaned up when that message drains. Subsequent user `Dispatch` calls are ignored.
3. **Binding**: By calling `setDispatch`, it allows your view host (whether it's an ElmView host or a Projection host) to receive the `dispatch` function and start sending messages back to Elmish.

## Connect an application

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

## Implementation

- [AvaloniaHost.fs](https://github.com/adz/Elmish.Avalonia.Glue/blob/main/src/Elmish.Avalonia.Glue/AvaloniaHost.fs)
