using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elmish.Avalonia.Glue;
using OpsCenterSample.UI.Components;
using OverviewCore = OpsCenterSample.Core.OverviewPage;

namespace OpsCenterSample.UI.Pages.Overview;

public partial class OverviewPageProjection : ObservableObject, IProjection<OverviewCore.Model, OverviewCore.Msg>
{
    private Action<OverviewCore.Msg> _dispatch = _ => { };

    public ObservableCollection<MetricCardProjection> Highlights { get; } = new();
    public ObservableCollection<ActivityItemProjection> Activity { get; } = new();

    public void Update(OverviewCore.Model model)
    {
        var view = OverviewCore.toView(model);

        Highlights.SyncWith(
            models: view.Highlights,
            modelKey: card => card.Key,
            vmKey: vm => vm.Label,
            create: ProjectionFactory.MetricCard,
            update: (vm, card) => vm.Update(card));

        Activity.SyncWith(
            models: view.Activity,
            modelKey: item => item.Id,
            vmKey: vm => vm.Id,
            create: _ => new ActivityItemProjection(),
            dispatch: _dispatch);
    }

    public void SetDispatch(Action<OverviewCore.Msg> dispatch) => _dispatch = dispatch;

    [RelayCommand]
    private void RefreshSnapshot() => _dispatch(OverviewCore.Msg.RefreshSnapshot);
}
