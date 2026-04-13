using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using OpsCenterSample.UI.Components;

namespace OpsCenterSample.UI.Pages.Overview;

public partial class OverviewPageView : UserControl
{
    public OverviewPageView()
    {
        Content = Build();
    }

    private static Control Build()
    {
        var highlights = new ItemsControl
        {
            ItemsPanel = new FuncTemplate<Panel?>(() => new UniformGrid { Columns = 4 }),
            ItemTemplate = new FuncDataTemplate<MetricCardProjection>((_, _) => new MetricCardView(), true),
            [!ItemsControl.ItemsSourceProperty] = new Binding(nameof(OverviewPageProjection.Highlights))
        };

        var items = new ItemsControl
        {
            Margin = new Thickness(0, 14, 0, 0),
            ItemTemplate = new FuncDataTemplate<ActivityItemProjection>((_, _) => BuildActivityRow(), true),
            [!ItemsControl.ItemsSourceProperty] = new Binding(nameof(OverviewPageProjection.Activity)),
            [Grid.RowProperty] = 1
        };

        return new Grid
        {
            RowDefinitions = RowDefinitions.Parse("Auto,*"),
            Children =
            {
                new Grid
                {
                    ColumnDefinitions = ColumnDefinitions.Parse("*,Auto"),
                    Children =
                    {
                        new TextBlock
                        {
                            Text = "Morning snapshot",
                            FontSize = 20,
                            FontWeight = FontWeight.SemiBold
                        },
                        new Button
                        {
                            Content = "Refresh snapshot",
                            Margin = new Thickness(16, 0, 0, 0),
                            [!Button.CommandProperty] = new Binding(nameof(OverviewPageProjection.RefreshSnapshotCommand)),
                            [Grid.ColumnProperty] = 1
                        }
                    }
                },
                new Grid
                {
                    Margin = new Thickness(0, 24, 0, 0),
                    RowDefinitions = RowDefinitions.Parse("Auto,*"),
                    [Grid.RowProperty] = 1,
                    Children =
                    {
                        highlights,
                        new Border
                        {
                            Margin = new Thickness(0, 24, 0, 0),
                            Background = Brush.Parse("#F7F4EF"),
                            BorderBrush = Brush.Parse("#E6DED3"),
                            BorderThickness = new Thickness(1),
                            CornerRadius = new CornerRadius(16),
                            Padding = new Thickness(18),
                            [Grid.RowProperty] = 1,
                            Child = new Grid
                            {
                                RowDefinitions = RowDefinitions.Parse("Auto,*"),
                                Children =
                                {
                                    new TextBlock
                                    {
                                        Text = "Recent activity",
                                        FontSize = 16,
                                        FontWeight = FontWeight.SemiBold
                                    },
                                    items
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    private static Control BuildActivityRow()
    {
        return new Grid
        {
            ColumnDefinitions = ColumnDefinitions.Parse("64,*"),
            Margin = new Thickness(0, 0, 0, 12),
            Children =
            {
                new TextBlock
                {
                    FontFamily = FontFamily.Parse("Monospace"),
                    Foreground = Brush.Parse("#2E6C53"),
                    VerticalAlignment = VerticalAlignment.Top,
                    [!TextBlock.TextProperty] = new Binding(nameof(ActivityItemProjection.Time))
                },
                new StackPanel
                {
                    Spacing = 2,
                    [Grid.ColumnProperty] = 1,
                    Children =
                    {
                        new TextBlock
                        {
                            FontWeight = FontWeight.SemiBold,
                            [!TextBlock.TextProperty] = new Binding(nameof(ActivityItemProjection.Title))
                        },
                        new TextBlock
                        {
                            Foreground = Brush.Parse("#6B6A67"),
                            [!TextBlock.TextProperty] = new Binding(nameof(ActivityItemProjection.Detail))
                        }
                    }
                }
            }
        };
    }
}
