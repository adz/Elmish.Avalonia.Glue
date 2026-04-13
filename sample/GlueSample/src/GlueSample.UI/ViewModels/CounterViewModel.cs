using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elmish.Avalonia.Glue;
using Elmish.Glue.Core;
using GlueSample.Core;

namespace GlueSample.UI.ViewModels;

public partial class CounterViewModel : ObservableObject
{
    private Action<CounterPage.Msg> _dispatch = _ => { };

    [ObservableProperty] public partial int Count { get; set; }

    public ObservableCollection<LogEntryViewModel> Log { get; } = new();

    public void Update(CounterPage.Model model)
    {
        Count = model.Count;
        Log.SyncWith(
            models:   model.Log,
            modelKey: e => e.Id,
            vmKey:    vm => vm.Id,
            create:   e => new LogEntryViewModel(e),
            update:   (vm, e) => vm.Update(e));
    }

    public void SetDispatch(Action<CounterPage.Msg> dispatch) => _dispatch = dispatch;

    private void Dispatch(CounterPage.Msg msg) => _dispatch(msg);

    [RelayCommand]
    private void Increment() => Dispatch(CounterPage.Msg.Increment);

    [RelayCommand]
    private void Decrement() => Dispatch(CounterPage.Msg.Decrement);

    [RelayCommand]
    private void Reset() => Dispatch(CounterPage.Msg.Reset);
}
