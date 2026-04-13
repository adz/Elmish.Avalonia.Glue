using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elmish.Avalonia.Glue;
using OpsCenterSample.UI.Components;
using TeamCore = OpsCenterSample.Core.TeamPage;

namespace OpsCenterSample.UI.Pages.Team;

public partial class TeamPageProjection : ObservableObject, IProjection<TeamCore.Model, TeamCore.Msg>
{
    private Action<TeamCore.Msg> _dispatch = _ => { };

    public ObservableCollection<MetricCardProjection> Summary { get; } = new();
    public ObservableCollection<TeamMemberProjection> Members { get; } = new();

    public void Update(TeamCore.Model model)
    {
        var view = TeamCore.toView(model);

        Summary.SyncWith(
            models: view.Summary,
            modelKey: card => card.Key,
            vmKey: vm => vm.Label,
            create: ProjectionFactory.MetricCard,
            update: (vm, card) => vm.Update(card));

        Members.SyncWith(view.Members, m => m.Id, vm => vm.Id, _ => new TeamMemberProjection(), _dispatch);
    }

    public void SetDispatch(Action<TeamCore.Msg> dispatch) => _dispatch = dispatch;

    [RelayCommand]
    private void RotateOnCall() => _dispatch(TeamCore.Msg.RotateOnCall);
}
