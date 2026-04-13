using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elmish.Avalonia.Glue;
using OpsCenterSample.UI.Components;
using TeamCore = OpsCenterSample.Core.TeamPage;

namespace OpsCenterSample.UI.Pages.Team;

public partial class TeamPageProjection : ObservableObject
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

        Members.SyncWith(
            models: view.Members,
            modelKey: member => member.Id,
            vmKey: vm => vm.Id,
            create: member => CreateMember(member),
            update: (vm, member) => vm.Update(member));
    }

    public void SetDispatch(Action<TeamCore.Msg> dispatch)
    {
        _dispatch = dispatch;

        foreach (var member in Members)
        {
            member.SetDispatch(dispatch);
        }
    }

    private TeamMemberProjection CreateMember(TeamCore.MemberView member)
    {
        var projection = new TeamMemberProjection();
        projection.SetDispatch(_dispatch);
        projection.Update(member);
        return projection;
    }

    [RelayCommand]
    private void RotateOnCall() => _dispatch(TeamCore.Msg.RotateOnCall);
}
