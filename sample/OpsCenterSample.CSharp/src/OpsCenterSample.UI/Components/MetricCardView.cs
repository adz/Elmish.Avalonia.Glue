using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;

namespace OpsCenterSample.UI.Components;

public partial class MetricCardView : UserControl
{
    public MetricCardView()
    {
        Content = Build();
    }

    private static Control Build()
    {
        var border = new Border
        {
            Background = Brush.Parse("#F6F3EE"),
            BorderBrush = Brush.Parse("#E6DED3"),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(16),
            Padding = new Thickness(16)
        };

        var stack = new StackPanel { Spacing = 8 };

        var label = new TextBlock { FontSize = 13, Foreground = Brush.Parse("#6F6B64") };
        label.Bind(TextBlock.TextProperty, new Binding(nameof(MetricCardProjection.Label)));
        stack.Children.Add(label);

        var value = new TextBlock { FontSize = 28, FontWeight = FontWeight.SemiBold };
        value.Bind(TextBlock.TextProperty, new Binding(nameof(MetricCardProjection.Value)));
        stack.Children.Add(value);

        var hint = new TextBlock { FontSize = 12, Foreground = Brush.Parse("#2E6C53") };
        hint.Bind(TextBlock.TextProperty, new Binding(nameof(MetricCardProjection.Hint)));
        stack.Children.Add(hint);

        border.Child = stack;
        return border;
    }
}
