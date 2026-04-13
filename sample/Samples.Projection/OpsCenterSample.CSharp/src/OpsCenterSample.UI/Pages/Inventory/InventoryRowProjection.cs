using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elmish.Avalonia.Glue;
using Elmish.Glue.Core;
using InventoryCore = OpsCenterSample.Core.InventoryPage;

namespace OpsCenterSample.UI.Pages.Inventory;

public partial class InventoryRowProjection : ObservableObject, IProjection<InventoryCore.ItemView, InventoryCore.Msg>
{
    private Action<InventoryCore.Msg> _dispatch = _ => { };

    public Guid Id { get; private set; }

    [ObservableProperty] public partial string Name { get; set; } = "";
    [ObservableProperty] public partial string Stock { get; set; } = "";
    [ObservableProperty] public partial string Gap { get; set; } = "";

    public void SetDispatch(Action<InventoryCore.Msg> dispatch) => _dispatch = dispatch;

    public void Update(InventoryCore.ItemView item)
    {
        Id = item.Id;
        Name = item.Name;
        Stock = item.Stock;
        Gap = item.Gap;
    }

    [RelayCommand]
    private void Restock() => _dispatch(InventoryCore.Msg.NewRestock(Id));

    [RelayCommand]
    private void PickOne() => _dispatch(InventoryCore.Msg.NewPickOne(Id));
}
