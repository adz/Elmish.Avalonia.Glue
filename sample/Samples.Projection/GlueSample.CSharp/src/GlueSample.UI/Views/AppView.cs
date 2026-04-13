using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Controls.Templates;
using GlueSample.UI.ViewModels;

namespace GlueSample.UI.Views;

public partial class AppView : Window
{
    public AppView()
    {
        Title = "Elmish.Avalonia.Glue Sample";
        Width = 420;
        Height = 520;
        Content = Build();
    }

    private static Control Build()
    {
        var items = new ItemsControl
        {
            ItemTemplate = new FuncDataTemplate<LogEntryViewModel>((_, _) => BuildLogRow()),
            [!ItemsControl.ItemsSourceProperty] = new Binding(nameof(CounterViewModel.Log))
        };

        return new Grid
        {
            Margin = new Thickness(24),
            Children =
            {
                new StackPanel
                {
                    Spacing = 16,
                    Children =
                    {
                        new TextBlock
                        {
                            Text = "Glue Sample",
                            FontSize = 20,
                            FontWeight = FontWeight.SemiBold
                        },
                        new TextBlock
                        {
                            FontSize = 48,
                            FontWeight = FontWeight.Bold,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            [!TextBlock.TextProperty] = new Binding(nameof(CounterViewModel.Count))
                        },
                        new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Spacing = 8,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Children =
                            {
                                BuildButton("-", nameof(CounterViewModel.DecrementCommand), 44),
                                BuildButton("Reset", nameof(CounterViewModel.ResetCommand)),
                                BuildButton("+", nameof(CounterViewModel.IncrementCommand), 44)
                            }
                        },
                        new Separator(),
                        new TextBlock
                        {
                            Text = "Log",
                            FontSize = 13,
                            FontWeight = FontWeight.SemiBold
                        },
                        new ScrollViewer { Content = items }
                    }
                }
            }
        };
    }

    private static Button BuildButton(string content, string commandPath, double? width = null)
    {
        var button = new Button
        {
            Content = content,
            [!Button.CommandProperty] = new Binding(commandPath)
        };
        if (width is { } w) button.Width = w;
        return button;
    }

    private static Control BuildLogRow()
    {
        return new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 12,
            Margin = new Thickness(0, 3),
            Children =
            {
                new TextBlock
                {
                    FontFamily = FontFamily.Parse("Monospace"),
                    FontSize = 11,
                    Foreground = Brushes.Gray,
                    VerticalAlignment = VerticalAlignment.Center,
                    [!TextBlock.TextProperty] = new Binding(nameof(LogEntryViewModel.Time))
                },
                new TextBlock
                {
                    FontSize = 13,
                    VerticalAlignment = VerticalAlignment.Center,
                    [!TextBlock.TextProperty] = new Binding(nameof(LogEntryViewModel.Message))
                }
            }
        };
    }
}
