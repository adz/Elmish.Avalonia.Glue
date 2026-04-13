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
        return new Border
        {
            Background = Brush.Parse("#F6F3EE"),
            BorderBrush = Brush.Parse("#E6DED3"),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(16),
            Padding = new Thickness(16),
            Child = new StackPanel
            {
                Spacing = 8,
                Children =
                {
                    new TextBlock
                    {
                        FontSize = 13,
                        Foreground = Brush.Parse("#6F6B64"),
                        [!TextBlock.TextProperty] = new Binding(nameof(MetricCardProjection.Label))
                    },
                    new TextBlock
                    {
                        FontSize = 28,
                        FontWeight = FontWeight.SemiBold,
                        [!TextBlock.TextProperty] = new Binding(nameof(MetricCardProjection.Value))
                    },
                    new TextBlock
                    {
                        FontSize = 12,
                        Foreground = Brush.Parse("#2E6C53"),
                        [!TextBlock.TextProperty] = new Binding(nameof(MetricCardProjection.Hint))
                    }
                }
            }
        };
    }
}
