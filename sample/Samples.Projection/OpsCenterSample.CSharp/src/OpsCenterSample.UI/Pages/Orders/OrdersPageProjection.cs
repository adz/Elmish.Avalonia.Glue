using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elmish.Avalonia.Glue;
using Elmish.Glue.Core;
using OpsCenterSample.UI.Components;
using OrdersCore = OpsCenterSample.Core.OrdersPage;

namespace OpsCenterSample.UI.Pages.Orders;

public partial class OrdersPageProjection : ObservableObject, IProjection<OrdersCore.Model, OrdersCore.Msg>
{
    private Action<OrdersCore.Msg> _dispatch = _ => { };

    [ObservableProperty] public partial bool ShowActiveOnly { get; set; }

    public ObservableCollection<MetricCardProjection> Summary { get; } = new();
    public ObservableCollection<OrderRowProjection> Orders { get; } = new();

    public void Update(OrdersCore.Model model)
    {
        var view = OrdersCore.toView(model);

        ShowActiveOnly = view.ShowActiveOnly;

        Summary.SyncWith(
            models: view.Summary,
            modelKey: card => card.Key,
            vmKey: vm => vm.Label,
            create: ProjectionFactory.MetricCard,
            update: (vm, card) => vm.Update(card));

        Orders.SyncWith(
            models: view.Orders,
            modelKey: order => order.Id,
            vmKey: vm => vm.Id,
            create: _ => new OrderRowProjection(),
            dispatch: _dispatch);
    }

    public void SetDispatch(Action<OrdersCore.Msg> dispatch) => _dispatch = dispatch;

    [RelayCommand]
    private void ToggleActiveOnly() => _dispatch(OrdersCore.Msg.ToggleActiveOnly);

    [RelayCommand]
    private void AddRushOrder() => _dispatch(OrdersCore.Msg.AddRushOrder);
}
