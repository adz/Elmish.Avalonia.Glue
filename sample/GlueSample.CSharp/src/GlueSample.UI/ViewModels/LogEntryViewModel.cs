using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Elmish.Avalonia.Glue;
using GlueSample.Core;

namespace GlueSample.UI.ViewModels;

public partial class LogEntryViewModel : ObservableObject, IProjection<CounterPage.LogEntry, object>
{
    public Guid Id { get; }

    [ObservableProperty] public partial string Message { get; set; } = "";
    [ObservableProperty] public partial string Time { get; set; } = "";

    public LogEntryViewModel(CounterPage.LogEntry e)
    {
        Id = e.Id;
        Update(e);
    }

    public void Update(CounterPage.LogEntry e)
    {
        Message = e.Message;
        Time = e.Time.ToString("HH:mm:ss");
    }

    public void SetDispatch(Action<object> dispatch) { }
}
