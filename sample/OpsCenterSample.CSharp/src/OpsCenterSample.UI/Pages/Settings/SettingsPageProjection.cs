using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elmish.Avalonia.Glue;
using SettingsCore = OpsCenterSample.Core.SettingsPage;

namespace OpsCenterSample.UI.Pages.Settings;

public partial class SettingsPageProjection : ObservableObject, IProjection<SettingsCore.Model, SettingsCore.Msg>
{
    private Action<SettingsCore.Msg> _dispatch = _ => { };

    [ObservableProperty] public partial bool NotificationsEnabled { get; set; }
    [ObservableProperty] public partial bool CompactLists { get; set; }
    [ObservableProperty] public partial string Accent { get; set; } = "";

    public void Update(SettingsCore.Model model)
    {
        var view = SettingsCore.toView(model);
        NotificationsEnabled = view.NotificationsEnabled;
        CompactLists = view.CompactLists;
        Accent = view.Accent;
    }

    public void SetDispatch(Action<SettingsCore.Msg> dispatch) => _dispatch = dispatch;

    [RelayCommand]
    private void ToggleNotifications() => _dispatch(SettingsCore.Msg.ToggleNotifications);

    [RelayCommand]
    private void ToggleCompactLists() => _dispatch(SettingsCore.Msg.ToggleCompactLists);

    [RelayCommand]
    private void UseMoss() => _dispatch(SettingsCore.Msg.NewSetAccent(SettingsCore.Accent.Moss));

    [RelayCommand]
    private void UseEmber() => _dispatch(SettingsCore.Msg.NewSetAccent(SettingsCore.Accent.Ember));

    [RelayCommand]
    private void UseHarbor() => _dispatch(SettingsCore.Msg.NewSetAccent(SettingsCore.Accent.Harbor));
}
