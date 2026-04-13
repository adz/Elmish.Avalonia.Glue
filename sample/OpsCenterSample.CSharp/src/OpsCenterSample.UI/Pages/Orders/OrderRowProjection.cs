using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elmish.Avalonia.Glue;
using OrdersCore = OpsCenterSample.Core.OrdersPage;

namespace OpsCenterSample.UI.Pages.Orders;

public partial class OrderRowProjection : ObservableObject, IProjection<OrdersCore.OrderView, OrdersCore.Msg>
{
    private Action<OrdersCore.Msg> _dispatch = _ => { };

    public Guid Id { get; private set; }

    [ObservableProperty] public partial string Customer { get; set; } = "";
    [ObservableProperty] public partial string Route { get; set; } = "";
    [ObservableProperty] public partial string Status { get; set; } = "";
    [ObservableProperty] public partial string Total { get; set; } = "";
    [ObservableProperty] public partial string AdvanceLabel { get; set; } = "";

    public void SetDispatch(Action<OrdersCore.Msg> dispatch) => _dispatch = dispatch;

    public void Update(OrdersCore.OrderView order)
    {
        Id = order.Id;
        Customer = order.Customer;
        Route = order.Route;
        Total = order.Total;
        Status = order.Status;
        AdvanceLabel = order.AdvanceLabel;
    }

    [RelayCommand]
    private void Advance() => _dispatch(OrdersCore.Msg.NewAdvanceOrder(Id));
}
