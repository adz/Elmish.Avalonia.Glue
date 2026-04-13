using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using OpsCenterSample.UI.Components;
using OpsCenterSample.UI.Pages.Inventory;
using OpsCenterSample.UI.Pages.Orders;
using OpsCenterSample.UI.Pages.Overview;
using OpsCenterSample.UI.Pages.Settings;
using OpsCenterSample.UI.Pages.Team;

namespace OpsCenterSample.UI.Pages.Shell;

public partial class AppView : Window
{
    public AppProjection Projection { get; } = new();

    public AppView()
    {
        Title = "Ops Center Sample";
        Width = 1120;
        Height = 760;
        MinWidth = 960;
        MinHeight = 640;
        DataContext = Projection;
        Content = Build();
    }

    private Control Build()
    {
        return new Grid
        {
            ColumnDefinitions = ColumnDefinitions.Parse("220,*"),
            Background = Brush.Parse("#F4F1EA"),
            Children =
            {
                BuildSidebar(),
                new Grid
                {
                    Margin = new Thickness(24),
                    RowDefinitions = RowDefinitions.Parse("Auto,*"),
                    [Grid.ColumnProperty] = 1,
                    Children =
                    {
                        BuildHeaderCard(),
                        new Border
                        {
                            Child = BuildContentCard(),
                            [Grid.RowProperty] = 1
                        }
                    }
                }
            }
        };
    }

    private Control BuildSidebar()
    {
        var nav = new ItemsControl
        {
            ItemTemplate = new FuncDataTemplate<NavigationItemProjection>((_, _) => new NavItemView(), true),
            [!ItemsControl.ItemsSourceProperty] = new Binding(nameof(AppProjection.NavigationItems)),
            [Grid.RowProperty] = 2
        };

        return new Border
        {
            Background = Brush.Parse("#163227"),
            Padding = new Thickness(18, 22),
            Child = new Grid
            {
                RowDefinitions = RowDefinitions.Parse("Auto,Auto,*"),
                Children =
                {
                    new StackPanel
                    {
                        Spacing = 6,
                        Children =
                        {
                            new TextBlock { Text = "Ops Center", FontSize = 22, FontWeight = FontWeight.SemiBold, Foreground = Brush.Parse("#F8F3EA") },
                            new TextBlock { Text = "Code-only sample with explicit projections.", TextWrapping = TextWrapping.Wrap, Foreground = Brush.Parse("#C5D2CA") }
                        }
                    },
                    new Border
                    {
                        Margin = new Thickness(0, 22, 0, 18),
                        Height = 1,
                        Background = Brush.Parse("#2D5144"),
                        [Grid.RowProperty] = 1
                    },
                    nav
                }
            }
        };
    }

    private Control BuildHeaderCard()
    {
        return new Border
        {
            CornerRadius = new CornerRadius(18),
            Background = Brush.Parse("#FFFDF9"),
            Padding = new Thickness(24, 20),
            BoxShadow = BoxShadows.Parse("0 8 32 0 #16000000"),
            Child = new Grid
            {
                RowDefinitions = RowDefinitions.Parse("Auto,Auto"),
                ColumnDefinitions = ColumnDefinitions.Parse("*,Auto"),
                Children =
                {
                    new TextBlock
                    {
                        FontSize = 28,
                        FontWeight = FontWeight.SemiBold,
                        [!TextBlock.TextProperty] = new Binding(nameof(AppProjection.CurrentPageTitle))
                    },
                    new Border
                    {
                        Padding = new Thickness(10, 6),
                        Background = Brush.Parse("#E5F0E9"),
                        CornerRadius = new CornerRadius(999),
                        [Grid.ColumnProperty] = 1,
                        Child = new TextBlock
                        {
                            Foreground = Brush.Parse("#23543E"),
                            FontWeight = FontWeight.SemiBold,
                            [!TextBlock.TextProperty] = new Binding(nameof(AppProjection.CurrentPageKey))
                        }
                    },
                    new TextBlock
                    {
                        Margin = new Thickness(0, 8, 0, 0),
                        Foreground = Brush.Parse("#6B6A67"),
                        [!TextBlock.TextProperty] = new Binding(nameof(AppProjection.CurrentPageSubtitle)),
                        [Grid.RowProperty] = 1
                    }
                }
            }
        };
    }

    private Control BuildContentCard()
    {
        return new Border
        {
            Margin = new Thickness(0, 18, 0, 0),
            CornerRadius = new CornerRadius(18),
            Background = Brush.Parse("#FFFDF9"),
            Padding = new Thickness(24),
            BoxShadow = BoxShadows.Parse("0 8 32 0 #16000000"),
            Child = new ContentControl
            {
                [!ContentControl.ContentProperty] = new Binding(nameof(AppProjection.CurrentPage)),
                DataTemplates =
                {
                    new FuncDataTemplate<OverviewPageProjection>((_, _) => new OverviewPageView(), true),
                    new FuncDataTemplate<OrdersPageProjection>((_, _) => new OrdersPageView(), true),
                    new FuncDataTemplate<InventoryPageProjection>((_, _) => new InventoryPageView(), true),
                    new FuncDataTemplate<TeamPageProjection>((_, _) => new TeamPageView(), true),
                    new FuncDataTemplate<SettingsPageProjection>((_, _) => new SettingsPageView(), true)
                }
            }
        };
    }
}
