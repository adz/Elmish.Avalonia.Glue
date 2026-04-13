using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Elmish.Avalonia.Glue;
using Elmish.Glue.Core;
using OverviewCore = OpsCenterSample.Core.OverviewPage;

namespace OpsCenterSample.UI.Pages.Overview;

public partial class ActivityItemProjection : ObservableObject, IProjection<OverviewCore.ActivityView, OverviewCore.Msg>
{
    private Action<OverviewCore.Msg> _dispatch = _ => { };

    public Guid Id { get; private set; }

    [ObservableProperty] public partial string Title { get; set; } = "";
    [ObservableProperty] public partial string Detail { get; set; } = "";
    [ObservableProperty] public partial string Time { get; set; } = "";

    public void Update(OverviewCore.ActivityView item)
    {
        Id = item.Id;
        Title = item.Title;
        Detail = item.Detail;
        Time = item.TimeText;
    }

    public void SetDispatch(Action<OverviewCore.Msg> dispatch) => _dispatch = dispatch;
}
