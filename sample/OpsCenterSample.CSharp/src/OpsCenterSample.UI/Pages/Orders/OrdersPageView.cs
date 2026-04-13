using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using OpsCenterSample.UI.Components;

namespace OpsCenterSample.UI.Pages.Orders;

public partial class OrdersPageView : UserControl
{
    public OrdersPageView()
    {
        Content = Build();
    }

    private static Control Build()
    {
        var root = new Grid { RowDefinitions = RowDefinitions.Parse("Auto,Auto,*") };

        var header = new Grid { ColumnDefinitions = ColumnDefinitions.Parse("*,Auto,Auto") };
        header.Children.Add(new TextBlock { Text = "Dispatch queue", FontSize = 20, FontWeight = FontWeight.SemiBold });
        header.Children.Add(BoundButton("Toggle active only", nameof(OrdersPageProjection.ToggleActiveOnlyCommand), 1));
        header.Children.Add(BoundButton("Add rush order", nameof(OrdersPageProjection.AddRushOrderCommand), 2));
        root.Children.Add(header);

        var summary = new ItemsControl { Margin = new Thickness(0, 20, 0, 0) };
        summary.Bind(ItemsControl.ItemsSourceProperty, new Binding(nameof(OrdersPageProjection.Summary)));
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

        var inner = new Grid { RowDefinitions = RowDefinitions.Parse("Auto,*") };
        var active = new TextBlock { Foreground = Brush.Parse("#6B6A67") };
        active.Bind(TextBlock.TextProperty, new Binding(nameof(OrdersPageProjection.ShowActiveOnly)) { StringFormat = "Active only: {0}" });
        inner.Children.Add(active);

        var items = new ItemsControl { Margin = new Thickness(0, 12, 0, 0) };
        items.Bind(ItemsControl.ItemsSourceProperty, new Binding(nameof(OrdersPageProjection.Orders)));
        items.ItemTemplate = new FuncDataTemplate<OrderRowProjection>((_, _) => BuildRow(), true);
        Grid.SetRow(items, 1);
        inner.Children.Add(items);

        border.Child = inner;
        root.Children.Add(border);
        return root;
    }

    private static Button BoundButton(string content, string commandPath, int column)
    {
        var button = new Button { Content = content, Margin = new Thickness(12, 0, 0, 0) };
        button.Bind(Button.CommandProperty, new Binding(commandPath));
        Grid.SetColumn(button, column);
        return button;
    }

    private static Control BuildRow()
    {
        var row = new Grid { ColumnDefinitions = ColumnDefinitions.Parse("2*,*,Auto,Auto"), Margin = new Thickness(0, 0, 0, 12) };

        var left = new StackPanel { Spacing = 2 };
        var customer = new TextBlock { FontWeight = FontWeight.SemiBold };
        customer.Bind(TextBlock.TextProperty, new Binding(nameof(OrderRowProjection.Customer)));
        left.Children.Add(customer);
        var route = new TextBlock { Foreground = Brush.Parse("#6B6A67") };
        route.Bind(TextBlock.TextProperty, new Binding(nameof(OrderRowProjection.Route)));
        left.Children.Add(route);
        row.Children.Add(left);

        var total = new TextBlock { Margin = new Thickness(12, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };
        total.Bind(TextBlock.TextProperty, new Binding(nameof(OrderRowProjection.Total)));
        Grid.SetColumn(total, 1);
        row.Children.Add(total);

        var badge = new Border { Margin = new Thickness(12, 0, 0, 0), Padding = new Thickness(10, 6), Background = Brush.Parse("#E5F0E9"), CornerRadius = new CornerRadius(999) };
        var status = new TextBlock { Foreground = Brush.Parse("#23543E") };
        status.Bind(TextBlock.TextProperty, new Binding(nameof(OrderRowProjection.Status)));
        badge.Child = status;
        Grid.SetColumn(badge, 2);
        row.Children.Add(badge);

        var action = new Button { Margin = new Thickness(12, 0, 0, 0) };
        action.Bind(Button.ContentProperty, new Binding(nameof(OrderRowProjection.AdvanceLabel)));
        action.Bind(Button.CommandProperty, new Binding(nameof(OrderRowProjection.AdvanceCommand)));
        Grid.SetColumn(action, 3);
        row.Children.Add(action);

        return row;
    }
}
