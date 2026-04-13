using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using OpsCenterSample.UI.Components;

namespace OpsCenterSample.UI.Pages.Inventory;

public partial class InventoryPageView : UserControl
{
    public InventoryPageView()
    {
        Content = Build();
    }

    private static Control Build()
    {
        var summary = new ItemsControl
        {
            Margin = new Thickness(0, 20, 0, 0),
            ItemsPanel = new FuncTemplate<Panel?>(() => new UniformGrid { Columns = 3 }),
            ItemTemplate = new FuncDataTemplate<MetricCardProjection>((_, _) => new MetricCardView(), true),
            [!ItemsControl.ItemsSourceProperty] = new Binding(nameof(InventoryPageProjection.Summary)),
            [Grid.RowProperty] = 1
        };

        var items = new ItemsControl
        {
            ItemTemplate = new FuncDataTemplate<InventoryRowProjection>((_, _) => BuildRow(), true),
            [!ItemsControl.ItemsSourceProperty] = new Binding(nameof(InventoryPageProjection.Items))
        };

        return new Grid
        {
            RowDefinitions = RowDefinitions.Parse("Auto,Auto,*"),
            Children =
            {
                new Grid
                {
                    ColumnDefinitions = ColumnDefinitions.Parse("*,Auto"),
                    Children =
                    {
                        new TextBlock { Text = "Consumables", FontSize = 20, FontWeight = FontWeight.SemiBold },
                        new Button
                        {
                            Content = "Toggle low stock only",
                            Margin = new Thickness(12, 0, 0, 0),
                            [!Button.CommandProperty] = new Binding(nameof(InventoryPageProjection.ToggleLowOnlyCommand)),
                            [Grid.ColumnProperty] = 1
                        }
                    }
                },
                summary,
                new Border
                {
                    Margin = new Thickness(0, 20, 0, 0),
                    Background = Brush.Parse("#F7F4EF"),
                    BorderBrush = Brush.Parse("#E6DED3"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(16),
                    Padding = new Thickness(18),
                    Child = items,
                    [Grid.RowProperty] = 2
                }
            }
        };
    }

    private static Control BuildRow()
    {
        return new Grid
        {
            ColumnDefinitions = ColumnDefinitions.Parse("2*,Auto,*,Auto,Auto"),
            Margin = new Thickness(0, 0, 0, 12),
            Children =
            {
                new TextBlock
                {
                    FontWeight = FontWeight.SemiBold,
                    VerticalAlignment = VerticalAlignment.Center,
                    [!TextBlock.TextProperty] = new Binding(nameof(InventoryRowProjection.Name))
                },
                new TextBlock
                {
                    Margin = new Thickness(12, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    [!TextBlock.TextProperty] = new Binding(nameof(InventoryRowProjection.Stock)),
                    [Grid.ColumnProperty] = 1
                },
                new TextBlock
                {
                    Margin = new Thickness(12, 0, 0, 0),
                    Foreground = Brush.Parse("#6B6A67"),
                    VerticalAlignment = VerticalAlignment.Center,
                    [!TextBlock.TextProperty] = new Binding(nameof(InventoryRowProjection.Gap)),
                    [Grid.ColumnProperty] = 2
                },
                new Button
                {
                    Content = "Pick one",
                    Margin = new Thickness(12, 0, 0, 0),
                    [!Button.CommandProperty] = new Binding(nameof(InventoryRowProjection.PickOneCommand)),
                    [Grid.ColumnProperty] = 3
                },
                new Button
                {
                    Content = "Restock",
                    Margin = new Thickness(12, 0, 0, 0),
                    [!Button.CommandProperty] = new Binding(nameof(InventoryRowProjection.RestockCommand)),
                    [Grid.ColumnProperty] = 4
                }
            }
        };
    }
}
