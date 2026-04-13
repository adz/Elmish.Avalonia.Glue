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
        var root = new Grid { RowDefinitions = RowDefinitions.Parse("Auto,Auto,*") };

        var header = new Grid { ColumnDefinitions = ColumnDefinitions.Parse("*,Auto") };
        header.Children.Add(new TextBlock { Text = "Shift crew", FontSize = 20, FontWeight = FontWeight.SemiBold });
        var rotate = new Button { Content = "Rotate on call", Margin = new Thickness(12, 0, 0, 0) };
        rotate.Bind(Button.CommandProperty, new Binding(nameof(TeamPageProjection.RotateOnCallCommand)));
        Grid.SetColumn(rotate, 1);
        header.Children.Add(rotate);
        root.Children.Add(header);

        var summary = new ItemsControl { Margin = new Thickness(0, 20, 0, 0) };
        summary.Bind(ItemsControl.ItemsSourceProperty, new Binding(nameof(TeamPageProjection.Summary)));
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
        items.Bind(ItemsControl.ItemsSourceProperty, new Binding(nameof(TeamPageProjection.Members)));
        items.ItemTemplate = new FuncDataTemplate<TeamMemberProjection>((_, _) => BuildRow(), true);
        border.Child = items;
        root.Children.Add(border);

        return root;
    }

    private static Control BuildRow()
    {
        var row = new Grid { ColumnDefinitions = ColumnDefinitions.Parse("2*,*,Auto,Auto,Auto"), Margin = new Thickness(0, 0, 0, 12) };
        var left = new StackPanel { Spacing = 2 };
        var name = new TextBlock { FontWeight = FontWeight.SemiBold };
        name.Bind(TextBlock.TextProperty, new Binding(nameof(TeamMemberProjection.Name)));
        left.Children.Add(name);
        var focus = new TextBlock { Foreground = Brush.Parse("#6B6A67") };
        focus.Bind(TextBlock.TextProperty, new Binding(nameof(TeamMemberProjection.Focus)));
        left.Children.Add(focus);
        row.Children.Add(left);

        var load = new TextBlock { Margin = new Thickness(12, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };
        load.Bind(TextBlock.TextProperty, new Binding(nameof(TeamMemberProjection.Load)));
        Grid.SetColumn(load, 1);
        row.Children.Add(load);

        var badge = new Border { Margin = new Thickness(12, 0, 0, 0), Padding = new Thickness(10, 6), Background = Brush.Parse("#F0E7D8"), CornerRadius = new CornerRadius(999) };
        var onCall = new TextBlock { Foreground = Brush.Parse("#765739") };
        onCall.Bind(TextBlock.TextProperty, new Binding(nameof(TeamMemberProjection.OnCall)));
        badge.Child = onCall;
        Grid.SetColumn(badge, 2);
        row.Children.Add(badge);

        var ease = new Button { Content = "Ease", Margin = new Thickness(12, 0, 0, 0) };
        ease.Bind(Button.CommandProperty, new Binding(nameof(TeamMemberProjection.EaseLoadCommand)));
        Grid.SetColumn(ease, 3);
        row.Children.Add(ease);

        var assign = new Button { Content = "Assign", Margin = new Thickness(12, 0, 0, 0) };
        assign.Bind(Button.CommandProperty, new Binding(nameof(TeamMemberProjection.AddLoadCommand)));
        Grid.SetColumn(assign, 4);
        row.Children.Add(assign);

        return row;
    }
}
