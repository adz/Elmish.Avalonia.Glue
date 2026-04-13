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
        var root = new Grid
        {
            RowDefinitions = RowDefinitions.Parse("Auto,*")
        };

        var header = new Grid { ColumnDefinitions = ColumnDefinitions.Parse("*,Auto") };
        header.Children.Add(new TextBlock
        {
            Text = "Morning snapshot",
            FontSize = 20,
            FontWeight = FontWeight.SemiBold
        });
        var refresh = new Button { Content = "Refresh snapshot", Margin = new Thickness(16, 0, 0, 0) };
        refresh.Bind(Button.CommandProperty, new Binding(nameof(OverviewPageProjection.RefreshSnapshotCommand)));
        Grid.SetColumn(refresh, 1);
        header.Children.Add(refresh);
        root.Children.Add(header);

        var body = new Grid
        {
            Margin = new Thickness(0, 24, 0, 0),
            RowDefinitions = RowDefinitions.Parse("Auto,*")
        };
        Grid.SetRow(body, 1);

        var highlights = new ItemsControl();
        highlights.Bind(ItemsControl.ItemsSourceProperty, new Binding(nameof(OverviewPageProjection.Highlights)));
        highlights.ItemsPanel = new FuncTemplate<Panel?>(() => new UniformGrid { Columns = 4 });
        highlights.ItemTemplate = new FuncDataTemplate<MetricCardProjection>((_, _) => new MetricCardView(), true);
        body.Children.Add(highlights);

        var activityBorder = new Border
        {
            Margin = new Thickness(0, 24, 0, 0),
            Background = Brush.Parse("#F7F4EF"),
            BorderBrush = Brush.Parse("#E6DED3"),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(16),
            Padding = new Thickness(18)
        };
        Grid.SetRow(activityBorder, 1);

        var activityGrid = new Grid { RowDefinitions = RowDefinitions.Parse("Auto,*") };
        activityGrid.Children.Add(new TextBlock
        {
            Text = "Recent activity",
            FontSize = 16,
            FontWeight = FontWeight.SemiBold
        });

        var items = new ItemsControl { Margin = new Thickness(0, 14, 0, 0) };
        items.Bind(ItemsControl.ItemsSourceProperty, new Binding(nameof(OverviewPageProjection.Activity)));
        items.ItemTemplate = new FuncDataTemplate<ActivityItemProjection>(
            (_, _) =>
            {
                var row = new Grid
                {
                    ColumnDefinitions = ColumnDefinitions.Parse("64,*"),
                    Margin = new Thickness(0, 0, 0, 12)
                };

                var time = new TextBlock
                {
                    FontFamily = FontFamily.Parse("Monospace"),
                    Foreground = Brush.Parse("#2E6C53"),
                    VerticalAlignment = VerticalAlignment.Top
                };
                time.Bind(TextBlock.TextProperty, new Binding(nameof(ActivityItemProjection.Time)));
                row.Children.Add(time);

                var stack = new StackPanel { Spacing = 2 };
                Grid.SetColumn(stack, 1);

                var title = new TextBlock { FontWeight = FontWeight.SemiBold };
                title.Bind(TextBlock.TextProperty, new Binding(nameof(ActivityItemProjection.Title)));
                stack.Children.Add(title);

                var detail = new TextBlock { Foreground = Brush.Parse("#6B6A67") };
                detail.Bind(TextBlock.TextProperty, new Binding(nameof(ActivityItemProjection.Detail)));
                stack.Children.Add(detail);

                row.Children.Add(stack);
                return row;
            }, true);
        Grid.SetRow(items, 1);
        activityGrid.Children.Add(items);

        activityBorder.Child = activityGrid;
        body.Children.Add(activityBorder);
        root.Children.Add(body);

        return root;
    }
}
