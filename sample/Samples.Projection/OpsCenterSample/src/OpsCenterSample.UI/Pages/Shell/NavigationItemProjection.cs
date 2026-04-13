using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpsApp = OpsCenterSample.Core.App;

namespace OpsCenterSample.UI.Pages.Shell;

public partial class NavigationItemProjection : ObservableObject
{
    private Action<OpsApp.Page> _navigate = _ => { };

    public NavigationItemProjection(OpsApp.Page page, string title, string caption)
    {
        Page = page;
        Title = title;
        Caption = caption;
    }

    public OpsApp.Page Page { get; }
    public string Title { get; }
    public string Caption { get; }

    [ObservableProperty] public partial bool IsSelected { get; set; }
    [ObservableProperty] public partial string CardBackground { get; set; } = "Transparent";

    public void SetNavigate(Action<OpsApp.Page> navigate) => _navigate = navigate;

    partial void OnIsSelectedChanged(bool value) =>
        CardBackground = value ? "#2A4D3F" : "Transparent";

    [RelayCommand]
    private void Select() => _navigate(Page);
}
