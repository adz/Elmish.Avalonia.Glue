using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Elmish.Avalonia.Glue;
using ExampleMatrixSample.ElmView.Core;
using ExampleMatrixSample.ElmView.UI.Views;

namespace ExampleMatrixSample.ElmView.UI;

public class App : Application
{
    private IDisposable? _host;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var view = new ExampleMatrixSample.ElmView.UI.Views.AppView();

            _host = ElmishHost.startAndBind(
                Core.App.program,
                view.Host.Update,
                view.Host.SetDispatch);

            desktop.MainWindow = view;
            desktop.Exit += (_, _) => _host?.Dispose();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
