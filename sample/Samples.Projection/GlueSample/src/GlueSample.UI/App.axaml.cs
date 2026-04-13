using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Elmish.Avalonia.Glue;
using GlueSample.UI.ViewModels;
using GlueSample.UI.Views;
using GlueSample.Core;

namespace GlueSample.UI;

public class App : Application
{
    private IDisposable? _host;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = new CounterViewModel();

            _host = ElmishHost.startAndBind(
                CounterPage.program,
                model => vm.Update(model),
                vm.SetDispatch);

            desktop.MainWindow = new AppView { DataContext = vm };
            desktop.Exit += (_, _) => _host?.Dispose();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
