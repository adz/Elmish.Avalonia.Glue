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
        var compact = Card("List density", nameof(SettingsPageProjection.CompactLists), "Compact lists: {0}", nameof(SettingsPageProjection.ToggleCompactListsCommand));
        compact.Margin = new Thickness(18, 0, 0, 0);
        compact[Grid.ColumnProperty] = 1;

        return new Grid
        {
            RowDefinitions = RowDefinitions.Parse("Auto,Auto,*"),
            Children =
            {
                new TextBlock { Text = "Operator preferences", FontSize = 20, FontWeight = FontWeight.SemiBold },
                new Grid
                {
                    Margin = new Thickness(0, 20, 0, 0),
                    ColumnDefinitions = ColumnDefinitions.Parse("*,*"),
                    [Grid.RowProperty] = 1,
                    Children =
                    {
                        Card("Notifications", nameof(SettingsPageProjection.NotificationsEnabled), "Enabled: {0}", nameof(SettingsPageProjection.ToggleNotificationsCommand)),
                        compact
                    }
                },
                new Border
                {
                    Margin = new Thickness(0, 20, 0, 0),
                    Background = Brush.Parse("#F7F4EF"),
                    BorderBrush = Brush.Parse("#E6DED3"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(16),
                    Padding = new Thickness(18),
                    [Grid.RowProperty] = 2,
                    Child = new StackPanel
                    {
                        Spacing = 14,
                        Children =
                        {
                            new TextBlock { Text = "Accent palette", FontWeight = FontWeight.SemiBold },
                            new TextBlock
                            {
                                Foreground = Brush.Parse("#6B6A67"),
                                [!TextBlock.TextProperty] = new Binding(nameof(SettingsPageProjection.Accent)) { StringFormat = "Current accent: {0}" }
                            },
                            new StackPanel
                            {
                                Orientation = Orientation.Horizontal,
                                Spacing = 10,
                                Children =
                                {
                                    ActionButton("Moss", nameof(SettingsPageProjection.UseMossCommand)),
                                    ActionButton("Ember", nameof(SettingsPageProjection.UseEmberCommand)),
                                    ActionButton("Harbor", nameof(SettingsPageProjection.UseHarborCommand))
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    private static Border Card(string title, string valuePath, string format, string commandPath)
    {
        return new Border
        {
            Background = Brush.Parse("#F7F4EF"),
            BorderBrush = Brush.Parse("#E6DED3"),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(16),
            Padding = new Thickness(18),
            Child = new StackPanel
            {
                Spacing = 10,
                Children =
                {
                    new TextBlock { Text = title, FontWeight = FontWeight.SemiBold },
                    new TextBlock
                    {
                        Foreground = Brush.Parse("#6B6A67"),
                        [!TextBlock.TextProperty] = new Binding(valuePath) { StringFormat = format }
                    },
                    ActionButton(title.StartsWith("Notifications") ? "Toggle notifications" : "Toggle compact lists", commandPath)
                }
            }
        };
    }

    private static Button ActionButton(string content, string commandPath)
    {
        return new Button
        {
            Content = content,
            [!Button.CommandProperty] = new Binding(commandPath)
        };
    }
}
