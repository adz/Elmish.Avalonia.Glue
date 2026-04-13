using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elmish.Avalonia.Glue;
using OpsCenterSample.UI.Components;
using InventoryCore = OpsCenterSample.Core.InventoryPage;

namespace OpsCenterSample.UI.Pages.Inventory;

public partial class InventoryPageProjection : ObservableObject, IProjection<InventoryCore.Model, InventoryCore.Msg>
{
    private Action<InventoryCore.Msg> _dispatch = _ => { };

    [ObservableProperty] public partial bool ShowLowOnly { get; set; }

    public ObservableCollection<MetricCardProjection> Summary { get; } = new();
    public ObservableCollection<InventoryRowProjection> Items { get; } = new();

    public void Update(InventoryCore.Model model)
    {
        var view = InventoryCore.toView(model);

        ShowLowOnly = view.ShowLowOnly;

        Summary.SyncWith(
            models: view.Summary,
            modelKey: card => card.Key,
            vmKey: vm => vm.Label,
            create: ProjectionFactory.MetricCard,
            update: (vm, card) => vm.Update(card));

        Items.SyncWith(
            models: view.Items,
            modelKey: item => item.Id,
            vmKey: vm => vm.Id,
            create: _ => new InventoryRowProjection(),
            dispatch: _dispatch);
    }

    public void SetDispatch(Action<InventoryCore.Msg> dispatch) => _dispatch = dispatch;

    [RelayCommand]
    private void ToggleLowOnly() => _dispatch(InventoryCore.Msg.ToggleLowOnly);
}
