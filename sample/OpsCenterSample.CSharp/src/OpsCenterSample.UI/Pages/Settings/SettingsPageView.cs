using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;

namespace OpsCenterSample.UI.Pages.Settings;

public partial class SettingsPageView : UserControl
{
    public SettingsPageView()
    {
        Content = Build();
    }

    private static Control Build()
    {
        var root = new Grid { RowDefinitions = RowDefinitions.Parse("Auto,Auto,*") };
        root.Children.Add(new TextBlock { Text = "Operator preferences", FontSize = 20, FontWeight = FontWeight.SemiBold });

        var top = new Grid { Margin = new Thickness(0, 20, 0, 0), ColumnDefinitions = ColumnDefinitions.Parse("*,*") };
        Grid.SetRow(top, 1);
        top.Children.Add(Card("Notifications", nameof(SettingsPageProjection.NotificationsEnabled), "Enabled: {0}", nameof(SettingsPageProjection.ToggleNotificationsCommand)));
        var compact = Card("List density", nameof(SettingsPageProjection.CompactLists), "Compact lists: {0}", nameof(SettingsPageProjection.ToggleCompactListsCommand));
        compact.Margin = new Thickness(18, 0, 0, 0);
        Grid.SetColumn(compact, 1);
        top.Children.Add(compact);
        root.Children.Add(top);

        var accent = new Border
        {
            Margin = new Thickness(0, 20, 0, 0),
            Background = Brush.Parse("#F7F4EF"),
            BorderBrush = Brush.Parse("#E6DED3"),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(16),
            Padding = new Thickness(18)
        };
        Grid.SetRow(accent, 2);
        var stack = new StackPanel { Spacing = 14 };
        stack.Children.Add(new TextBlock { Text = "Accent palette", FontWeight = FontWeight.SemiBold });
        var current = new TextBlock { Foreground = Brush.Parse("#6B6A67") };
        current.Bind(TextBlock.TextProperty, new Binding(nameof(SettingsPageProjection.Accent)) { StringFormat = "Current accent: {0}" });
        stack.Children.Add(current);
        var buttons = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };
        buttons.Children.Add(ActionButton("Moss", nameof(SettingsPageProjection.UseMossCommand)));
        buttons.Children.Add(ActionButton("Ember", nameof(SettingsPageProjection.UseEmberCommand)));
        buttons.Children.Add(ActionButton("Harbor", nameof(SettingsPageProjection.UseHarborCommand)));
        stack.Children.Add(buttons);
        accent.Child = stack;
        root.Children.Add(accent);
        return root;
    }

    private static Border Card(string title, string valuePath, string format, string commandPath)
    {
        var border = new Border
        {
            Background = Brush.Parse("#F7F4EF"),
            BorderBrush = Brush.Parse("#E6DED3"),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(16),
            Padding = new Thickness(18)
        };
        var stack = new StackPanel { Spacing = 10 };
        stack.Children.Add(new TextBlock { Text = title, FontWeight = FontWeight.SemiBold });
        var text = new TextBlock { Foreground = Brush.Parse("#6B6A67") };
        text.Bind(TextBlock.TextProperty, new Binding(valuePath) { StringFormat = format });
        stack.Children.Add(text);
        stack.Children.Add(ActionButton(title.StartsWith("Notifications") ? "Toggle notifications" : "Toggle compact lists", commandPath));
        border.Child = stack;
        return border;
    }

    private static Button ActionButton(string content, string commandPath)
    {
        var button = new Button { Content = content };
        button.Bind(Button.CommandProperty, new Binding(commandPath));
        return button;
    }
}
