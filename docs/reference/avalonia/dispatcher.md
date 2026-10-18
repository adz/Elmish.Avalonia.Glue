# Dispatcher bridge

`Elmish.Avalonia.Glue.ElmishHost` starts an Elmish program, binds it to an Avalonia view, and marshals UI updates to Avalonia's UI thread.

## API contracts

- [`Elmish.Avalonia.Glue.ElmishHost`](/api/Elmish.Avalonia.Glue.ElmishHost.html)

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
