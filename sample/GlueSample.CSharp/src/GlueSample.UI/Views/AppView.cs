using Avalonia;
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
        var root = new Grid { Margin = new Thickness(24) };

        var stack = new StackPanel { Spacing = 16 };
        root.Children.Add(stack);

        stack.Children.Add(new TextBlock
        {
            Text = "Glue Sample",
            FontSize = 20,
            FontWeight = FontWeight.SemiBold
        });

        var count = new TextBlock
        {
            FontSize = 48,
            FontWeight = FontWeight.Bold,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        count.Bind(TextBlock.TextProperty, new Binding(nameof(CounterViewModel.Count)));
        stack.Children.Add(count);

        var actions = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 8,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        actions.Children.Add(BuildButton("-", nameof(CounterViewModel.DecrementCommand), 44));
        actions.Children.Add(BuildButton("Reset", nameof(CounterViewModel.ResetCommand)));
        actions.Children.Add(BuildButton("+", nameof(CounterViewModel.IncrementCommand), 44));
        stack.Children.Add(actions);

        stack.Children.Add(new Separator());
        stack.Children.Add(new TextBlock
        {
            Text = "Log",
            FontSize = 13,
            FontWeight = FontWeight.SemiBold
        });

        var items = new ItemsControl();
        items.Bind(ItemsControl.ItemsSourceProperty, new Binding(nameof(CounterViewModel.Log)));
        items.ItemTemplate = new FuncDataTemplate<LogEntryViewModel>(
            (entry, _) =>
            {
                var row = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 12,
                    Margin = new Thickness(0, 3)
                };

                var time = new TextBlock
                {
                    FontFamily = FontFamily.Parse("Monospace"),
                    FontSize = 11,
                    Foreground = Brushes.Gray,
                    VerticalAlignment = VerticalAlignment.Center
                };
                time.Bind(TextBlock.TextProperty, new Binding(nameof(LogEntryViewModel.Time)));
                row.Children.Add(time);

                var message = new TextBlock
                {
                    FontSize = 13,
                    VerticalAlignment = VerticalAlignment.Center
                };
                message.Bind(TextBlock.TextProperty, new Binding(nameof(LogEntryViewModel.Message)));
                row.Children.Add(message);

                return row;
            });

        stack.Children.Add(new ScrollViewer { Content = items });
        return root;
    }

    private static Button BuildButton(string content, string commandPath, double? width = null)
    {
        var button = new Button { Content = content };
        if (width is { } w) button.Width = w;
        button.Bind(Button.CommandProperty, new Binding(commandPath));
        return button;
    }
}
