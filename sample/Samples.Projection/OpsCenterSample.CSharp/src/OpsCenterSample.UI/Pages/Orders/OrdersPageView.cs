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
        var summary = new ItemsControl
        {
            Margin = new Thickness(0, 20, 0, 0),
            ItemsPanel = new FuncTemplate<Panel?>(() => new UniformGrid { Columns = 3 }),
            ItemTemplate = new FuncDataTemplate<MetricCardProjection>((_, _) => new MetricCardView(), true),
            [!ItemsControl.ItemsSourceProperty] = new Binding(nameof(OrdersPageProjection.Summary)),
            [Grid.RowProperty] = 1
        };

        var items = new ItemsControl
        {
            Margin = new Thickness(0, 12, 0, 0),
            ItemTemplate = new FuncDataTemplate<OrderRowProjection>((_, _) => BuildRow(), true),
            [!ItemsControl.ItemsSourceProperty] = new Binding(nameof(OrdersPageProjection.Orders)),
            [Grid.RowProperty] = 1
        };

        return new Grid
        {
            RowDefinitions = RowDefinitions.Parse("Auto,Auto,*"),
            Children =
            {
                new Grid
                {
                    ColumnDefinitions = ColumnDefinitions.Parse("*,Auto,Auto"),
                    Children =
                    {
                        new TextBlock { Text = "Dispatch queue", FontSize = 20, FontWeight = FontWeight.SemiBold },
                        BoundButton("Toggle active only", nameof(OrdersPageProjection.ToggleActiveOnlyCommand), 1),
                        BoundButton("Add rush order", nameof(OrdersPageProjection.AddRushOrderCommand), 2)
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
                    [Grid.RowProperty] = 2,
                    Child = new Grid
                    {
                        RowDefinitions = RowDefinitions.Parse("Auto,*"),
                        Children =
                        {
                            new TextBlock
                            {
                                Foreground = Brush.Parse("#6B6A67"),
                                [!TextBlock.TextProperty] = new Binding(nameof(OrdersPageProjection.ShowActiveOnly)) { StringFormat = "Active only: {0}" }
                            },
                            items
                        }
                    }
                }
            }
        };
    }

    private static Button BoundButton(string content, string commandPath, int column)
    {
        return new Button
        {
            Content = content,
            Margin = new Thickness(12, 0, 0, 0),
            [!Button.CommandProperty] = new Binding(commandPath),
            [Grid.ColumnProperty] = column
        };
    }

    private static Control BuildRow()
    {
        return new Grid
        {
            ColumnDefinitions = ColumnDefinitions.Parse("2*,*,Auto,Auto"),
            Margin = new Thickness(0, 0, 0, 12),
            Children =
            {
                new StackPanel
                {
                    Spacing = 2,
                    Children =
                    {
                        new TextBlock
                        {
                            FontWeight = FontWeight.SemiBold,
                            [!TextBlock.TextProperty] = new Binding(nameof(OrderRowProjection.Customer))
                        },
                        new TextBlock
                        {
                            Foreground = Brush.Parse("#6B6A67"),
                            [!TextBlock.TextProperty] = new Binding(nameof(OrderRowProjection.Route))
                        }
                    }
                },
                new TextBlock
                {
                    Margin = new Thickness(12, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    [!TextBlock.TextProperty] = new Binding(nameof(OrderRowProjection.Total)),
                    [Grid.ColumnProperty] = 1
                },
                new Border
                {
                    Margin = new Thickness(12, 0, 0, 0),
                    Padding = new Thickness(10, 6),
                    Background = Brush.Parse("#E5F0E9"),
                    CornerRadius = new CornerRadius(999),
                    [Grid.ColumnProperty] = 2,
                    Child = new TextBlock
                    {
                        Foreground = Brush.Parse("#23543E"),
                        [!TextBlock.TextProperty] = new Binding(nameof(OrderRowProjection.Status))
                    }
                },
                new Button
                {
                    Margin = new Thickness(12, 0, 0, 0),
                    [!Button.ContentProperty] = new Binding(nameof(OrderRowProjection.AdvanceLabel)),
                    [!Button.CommandProperty] = new Binding(nameof(OrderRowProjection.AdvanceCommand)),
                    [Grid.ColumnProperty] = 3
                }
            }
        };
    }
}
