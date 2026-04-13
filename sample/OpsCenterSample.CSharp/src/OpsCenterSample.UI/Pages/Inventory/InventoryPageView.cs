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
        var root = new Grid { RowDefinitions = RowDefinitions.Parse("Auto,Auto,*") };

        var header = new Grid { ColumnDefinitions = ColumnDefinitions.Parse("*,Auto") };
        header.Children.Add(new TextBlock { Text = "Consumables", FontSize = 20, FontWeight = FontWeight.SemiBold });
        var toggle = new Button { Content = "Toggle low stock only", Margin = new Thickness(12, 0, 0, 0) };
        toggle.Bind(Button.CommandProperty, new Binding(nameof(InventoryPageProjection.ToggleLowOnlyCommand)));
        Grid.SetColumn(toggle, 1);
        header.Children.Add(toggle);
        root.Children.Add(header);

        var summary = new ItemsControl { Margin = new Thickness(0, 20, 0, 0) };
        summary.Bind(ItemsControl.ItemsSourceProperty, new Binding(nameof(InventoryPageProjection.Summary)));
        summary.ItemsPanel = new FuncTemplate<Panel?>(() => new UniformGrid { Columns = 3 });
        summary.ItemTemplate = new FuncDataTemplate<MetricCardProjection>((_, _) => new MetricCardView(), true);
        Grid.SetRow(summary, 1);
        root.Children.Add(summary);

        var border = new Border
        {
            Margin = new Thickness(0, 20, 0, 0),
            Background = Brush.Parse("#F7F4EF"),
            BorderBrush = Brush.Parse("#E6DED3"),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(16),
            Padding = new Thickness(18)
        };
        Grid.SetRow(border, 2);

        var items = new ItemsControl();
        items.Bind(ItemsControl.ItemsSourceProperty, new Binding(nameof(InventoryPageProjection.Items)));
        items.ItemTemplate = new FuncDataTemplate<InventoryRowProjection>((_, _) => BuildRow(), true);
        border.Child = items;
        root.Children.Add(border);

        return root;
    }

    private static Control BuildRow()
    {
        var row = new Grid { ColumnDefinitions = ColumnDefinitions.Parse("2*,Auto,*,Auto,Auto"), Margin = new Thickness(0, 0, 0, 12) };
        var name = new TextBlock { FontWeight = FontWeight.SemiBold, VerticalAlignment = VerticalAlignment.Center };
        name.Bind(TextBlock.TextProperty, new Binding(nameof(InventoryRowProjection.Name)));
        row.Children.Add(name);

        var stock = new TextBlock { Margin = new Thickness(12, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };
        stock.Bind(TextBlock.TextProperty, new Binding(nameof(InventoryRowProjection.Stock)));
        Grid.SetColumn(stock, 1);
        row.Children.Add(stock);

        var gap = new TextBlock { Margin = new Thickness(12, 0, 0, 0), Foreground = Brush.Parse("#6B6A67"), VerticalAlignment = VerticalAlignment.Center };
        gap.Bind(TextBlock.TextProperty, new Binding(nameof(InventoryRowProjection.Gap)));
        Grid.SetColumn(gap, 2);
        row.Children.Add(gap);

        var pick = new Button { Content = "Pick one", Margin = new Thickness(12, 0, 0, 0) };
        pick.Bind(Button.CommandProperty, new Binding(nameof(InventoryRowProjection.PickOneCommand)));
        Grid.SetColumn(pick, 3);
        row.Children.Add(pick);

        var restock = new Button { Content = "Restock", Margin = new Thickness(12, 0, 0, 0) };
        restock.Bind(Button.CommandProperty, new Binding(nameof(InventoryRowProjection.RestockCommand)));
        Grid.SetColumn(restock, 4);
        row.Children.Add(restock);

        return row;
    }
}
