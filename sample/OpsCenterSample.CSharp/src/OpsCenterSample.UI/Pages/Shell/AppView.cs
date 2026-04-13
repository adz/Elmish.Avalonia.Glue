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
        var root = new Grid
        {
            ColumnDefinitions = ColumnDefinitions.Parse("220,*"),
            Background = Brush.Parse("#F4F1EA")
        };

        root.Children.Add(BuildSidebar());

        var main = new Grid
        {
            Margin = new Thickness(24),
            RowDefinitions = RowDefinitions.Parse("Auto,*")
        };
        Grid.SetColumn(main, 1);

        main.Children.Add(BuildHeaderCard());
        var content = BuildContentCard();
        Grid.SetRow(content, 1);
        main.Children.Add(content);

        root.Children.Add(main);
        return root;
    }

    private Control BuildSidebar()
    {
        var sidebar = new Border { Background = Brush.Parse("#163227"), Padding = new Thickness(18, 22) };
        var grid = new Grid { RowDefinitions = RowDefinitions.Parse("Auto,Auto,*") };

        var intro = new StackPanel { Spacing = 6 };
        intro.Children.Add(new TextBlock { Text = "Ops Center", FontSize = 22, FontWeight = FontWeight.SemiBold, Foreground = Brush.Parse("#F8F3EA") });
        intro.Children.Add(new TextBlock { Text = "Code-only sample with explicit projections.", TextWrapping = TextWrapping.Wrap, Foreground = Brush.Parse("#C5D2CA") });
        grid.Children.Add(intro);

        var divider = new Border { Margin = new Thickness(0, 22, 0, 18), Height = 1, Background = Brush.Parse("#2D5144") };
        Grid.SetRow(divider, 1);
        grid.Children.Add(divider);

        var nav = new ItemsControl();
        nav.Bind(ItemsControl.ItemsSourceProperty, new Binding(nameof(AppProjection.NavigationItems)));
        nav.ItemTemplate = new FuncDataTemplate<NavigationItemProjection>((_, _) => new NavItemView(), true);
        Grid.SetRow(nav, 2);
        grid.Children.Add(nav);

        sidebar.Child = grid;
        return sidebar;
    }

    private Control BuildHeaderCard()
    {
        var border = new Border
        {
            CornerRadius = new CornerRadius(18),
            Background = Brush.Parse("#FFFDF9"),
            Padding = new Thickness(24, 20),
            BoxShadow = BoxShadows.Parse("0 8 32 0 #16000000")
        };
        var grid = new Grid { RowDefinitions = RowDefinitions.Parse("Auto,Auto"), ColumnDefinitions = ColumnDefinitions.Parse("*,Auto") };

        var title = new TextBlock { FontSize = 28, FontWeight = FontWeight.SemiBold };
        title.Bind(TextBlock.TextProperty, new Binding(nameof(AppProjection.CurrentPageTitle)));
        grid.Children.Add(title);

        var keyBorder = new Border { Padding = new Thickness(10, 6), Background = Brush.Parse("#E5F0E9"), CornerRadius = new CornerRadius(999) };
        var key = new TextBlock { Foreground = Brush.Parse("#23543E"), FontWeight = FontWeight.SemiBold };
        key.Bind(TextBlock.TextProperty, new Binding(nameof(AppProjection.CurrentPageKey)));
        keyBorder.Child = key;
        Grid.SetColumn(keyBorder, 1);
        grid.Children.Add(keyBorder);

        var subtitle = new TextBlock { Margin = new Thickness(0, 8, 0, 0), Foreground = Brush.Parse("#6B6A67") };
        subtitle.Bind(TextBlock.TextProperty, new Binding(nameof(AppProjection.CurrentPageSubtitle)));
        Grid.SetRow(subtitle, 1);
        grid.Children.Add(subtitle);

        border.Child = grid;
        return border;
    }

    private Control BuildContentCard()
    {
        var border = new Border
        {
            Margin = new Thickness(0, 18, 0, 0),
            CornerRadius = new CornerRadius(18),
            Background = Brush.Parse("#FFFDF9"),
            Padding = new Thickness(24),
            BoxShadow = BoxShadows.Parse("0 8 32 0 #16000000")
        };

        var content = new ContentControl();
        content.Bind(ContentControl.ContentProperty, new Binding(nameof(AppProjection.CurrentPage)));
        content.DataTemplates.Add(new FuncDataTemplate<OverviewPageProjection>((_, _) => new OverviewPageView(), true));
        content.DataTemplates.Add(new FuncDataTemplate<OrdersPageProjection>((_, _) => new OrdersPageView(), true));
        content.DataTemplates.Add(new FuncDataTemplate<InventoryPageProjection>((_, _) => new InventoryPageView(), true));
        content.DataTemplates.Add(new FuncDataTemplate<TeamPageProjection>((_, _) => new TeamPageView(), true));
        content.DataTemplates.Add(new FuncDataTemplate<SettingsPageProjection>((_, _) => new SettingsPageView(), true));
        border.Child = content;
        return border;
    }
}
