using System;
using CommunityToolkit.Mvvm.ComponentModel;
using OverviewCore = OpsCenterSample.Core.OverviewPage;

namespace OpsCenterSample.UI.Pages.Overview;

public partial class ActivityItemProjection : ObservableObject
{
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
}
