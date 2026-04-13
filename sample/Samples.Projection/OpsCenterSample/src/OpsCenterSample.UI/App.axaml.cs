using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Elmish.Avalonia.Glue;
using OpsCenterSample.UI.Pages.Shell;
using OpsApp = OpsCenterSample.Core.App;

namespace OpsCenterSample.UI;

public class App : Application
{
    private IDisposable? _host;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var view = new AppView();

            _host = ElmishHost.startAndBind(
                OpsApp.program,
                model => view.Projection.Update(model),
                view.Projection.SetDispatch);

            desktop.MainWindow = view;
            desktop.Exit += (_, _) => _host?.Dispose();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
