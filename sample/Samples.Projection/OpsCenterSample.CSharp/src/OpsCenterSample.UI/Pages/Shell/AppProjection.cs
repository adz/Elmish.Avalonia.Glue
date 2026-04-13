using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Elmish.Avalonia.Glue;
using Elmish.Glue.Core;
using OpsCenterSample.UI.Pages.Inventory;
using OpsCenterSample.UI.Pages.Orders;
using OpsCenterSample.UI.Pages.Overview;
using OpsCenterSample.UI.Pages.Settings;
using OpsCenterSample.UI.Pages.Team;
using OpsApp = OpsCenterSample.Core.App;

namespace OpsCenterSample.UI.Pages.Shell;

public partial class AppProjection : ObservableObject, IProjection<OpsApp.Model, OpsApp.Msg>
{
    public AppProjection()
    {
        NavigationItems = new ObservableCollection<NavigationItemProjection>();

        foreach (var info in OpsApp.pageInfos)
        {
            NavigationItems.Add(new NavigationItemProjection(info.Page, info.NavTitle, info.NavCaption));
        }
    }

    public ObservableCollection<NavigationItemProjection> NavigationItems { get; }
    public OverviewPageProjection Overview { get; } = new();
    public OrdersPageProjection Orders { get; } = new();
    public InventoryPageProjection Inventory { get; } = new();
    public TeamPageProjection Team { get; } = new();
    public SettingsPageProjection Settings { get; } = new();

    [ObservableProperty] public partial object CurrentPage { get; set; } = null!;
    [ObservableProperty] public partial string CurrentPageTitle { get; set; } = "";
    [ObservableProperty] public partial string CurrentPageSubtitle { get; set; } = "";
    [ObservableProperty] public partial string CurrentPageKey { get; set; } = "";

    public void Update(OpsApp.Model model)
    {
        Overview.Update(model.Overview);
        Orders.Update(model.Orders);
        Inventory.Update(model.Inventory);
        Team.Update(model.Team);
        Settings.Update(model.Settings);

        foreach (var item in NavigationItems)
        {
            item.IsSelected = Equals(item.Page, model.CurrentPage);
        }

        var info = OpsApp.pageInfo(model.CurrentPage);
        CurrentPageTitle = info.Title;
        CurrentPageSubtitle = info.Subtitle;
        CurrentPageKey = info.Key;

        switch (model.CurrentPage.Tag)
        {
            case 0:
                CurrentPage = Overview;
                break;
            case 1:
                CurrentPage = Orders;
                break;
            case 2:
                CurrentPage = Inventory;
                break;
            case 3:
                CurrentPage = Team;
                break;
            default:
                CurrentPage = Settings;
                break;
        }
    }

    public void SetDispatch(Action<OpsApp.Msg> dispatch)
    {
        foreach (var item in NavigationItems)
        {
            item.SetNavigate(page => dispatch(OpsApp.Msg.NewNavigate(page)));
        }

        Overview.ForwardTo(dispatch, OpsApp.Msg.NewOverviewMsg);
        Orders.ForwardTo(dispatch, OpsApp.Msg.NewOrdersMsg);
        Inventory.ForwardTo(dispatch, OpsApp.Msg.NewInventoryMsg);
        Team.ForwardTo(dispatch, OpsApp.Msg.NewTeamMsg);
        Settings.ForwardTo(dispatch, OpsApp.Msg.NewSettingsMsg);
    }
}
