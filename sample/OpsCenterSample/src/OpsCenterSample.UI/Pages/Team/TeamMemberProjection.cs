using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TeamCore = OpsCenterSample.Core.TeamPage;

namespace OpsCenterSample.UI.Pages.Team;

public partial class TeamMemberProjection : ObservableObject
{
    private Action<TeamCore.Msg> _dispatch = _ => { };

    public Guid Id { get; private set; }

    [ObservableProperty] public partial string Name { get; set; } = "";
    [ObservableProperty] public partial string Focus { get; set; } = "";
    [ObservableProperty] public partial string Load { get; set; } = "";
    [ObservableProperty] public partial string OnCall { get; set; } = "";

    public void SetDispatch(Action<TeamCore.Msg> dispatch) => _dispatch = dispatch;

    public void Update(TeamCore.MemberView member)
    {
        Id = member.Id;
        Name = member.Name;
        Focus = member.Focus;
        Load = member.Load;
        OnCall = member.Availability;
    }

    [RelayCommand]
    private void EaseLoad() => _dispatch(TeamCore.Msg.NewEaseLoad(Id));

    [RelayCommand]
    private void AddLoad() => _dispatch(TeamCore.Msg.NewAddLoad(Id));
}
