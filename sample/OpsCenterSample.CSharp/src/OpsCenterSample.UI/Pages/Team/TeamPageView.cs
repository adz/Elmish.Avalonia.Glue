using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using OpsCenterSample.UI.Components;

namespace OpsCenterSample.UI.Pages.Team;

public partial class TeamPageView : UserControl
{
    public TeamPageView()
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
            [!ItemsControl.ItemsSourceProperty] = new Binding(nameof(TeamPageProjection.Summary)),
            [Grid.RowProperty] = 1
        };

        var items = new ItemsControl
        {
            ItemTemplate = new FuncDataTemplate<TeamMemberProjection>((_, _) => BuildRow(), true),
            [!ItemsControl.ItemsSourceProperty] = new Binding(nameof(TeamPageProjection.Members))
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
                        new TextBlock { Text = "Shift crew", FontSize = 20, FontWeight = FontWeight.SemiBold },
                        new Button
                        {
                            Content = "Rotate on call",
                            Margin = new Thickness(12, 0, 0, 0),
                            [!Button.CommandProperty] = new Binding(nameof(TeamPageProjection.RotateOnCallCommand)),
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
                    [Grid.RowProperty] = 2,
                    Child = items
                }
            }
        };
    }

    private static Control BuildRow()
    {
        return new Grid
        {
            ColumnDefinitions = ColumnDefinitions.Parse("2*,*,Auto,Auto,Auto"),
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
                            [!TextBlock.TextProperty] = new Binding(nameof(TeamMemberProjection.Name))
                        },
                        new TextBlock
                        {
                            Foreground = Brush.Parse("#6B6A67"),
                            [!TextBlock.TextProperty] = new Binding(nameof(TeamMemberProjection.Focus))
                        }
                    }
                },
                new TextBlock
                {
                    Margin = new Thickness(12, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    [!TextBlock.TextProperty] = new Binding(nameof(TeamMemberProjection.Load)),
                    [Grid.ColumnProperty] = 1
                },
                new Border
                {
                    Margin = new Thickness(12, 0, 0, 0),
                    Padding = new Thickness(10, 6),
                    Background = Brush.Parse("#F0E7D8"),
                    CornerRadius = new CornerRadius(999),
                    [Grid.ColumnProperty] = 2,
                    Child = new TextBlock
                    {
                        Foreground = Brush.Parse("#765739"),
                        [!TextBlock.TextProperty] = new Binding(nameof(TeamMemberProjection.OnCall))
                    }
                },
                new Button
                {
                    Content = "Ease",
                    Margin = new Thickness(12, 0, 0, 0),
                    [!Button.CommandProperty] = new Binding(nameof(TeamMemberProjection.EaseLoadCommand)),
                    [Grid.ColumnProperty] = 3
                },
                new Button
                {
                    Content = "Assign",
                    Margin = new Thickness(12, 0, 0, 0),
                    [!Button.CommandProperty] = new Binding(nameof(TeamMemberProjection.AddLoadCommand)),
                    [Grid.ColumnProperty] = 4
                }
            }
        };
    }
}
