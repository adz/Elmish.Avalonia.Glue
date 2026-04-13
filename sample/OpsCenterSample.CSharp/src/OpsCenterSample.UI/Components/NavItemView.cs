using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using OpsCenterSample.UI.Pages.Shell;

namespace OpsCenterSample.UI.Components;

public partial class NavItemView : UserControl
{
    public NavItemView()
    {
        Content = Build();
    }

    private static Control Build()
    {
        return new Button
        {
            Padding = new Thickness(14, 12),
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            Background = Brushes.Transparent,
            BorderBrush = null,
            [!Button.CommandProperty] = new Binding(nameof(NavigationItemProjection.SelectCommand)),
            Content = new Border
            {
                CornerRadius = new CornerRadius(14),
                Padding = new Thickness(12, 10),
                [!Border.BackgroundProperty] = new Binding(nameof(NavigationItemProjection.CardBackground)),
                Child = new StackPanel
                {
                    Spacing = 2,
                    Children =
                    {
                        new TextBlock
                        {
                            FontSize = 15,
                            FontWeight = FontWeight.SemiBold,
                            Foreground = Brush.Parse("#F8F3EA"),
                            [!TextBlock.TextProperty] = new Binding(nameof(NavigationItemProjection.Title))
                        },
                        new TextBlock
                        {
                            FontSize = 12,
                            Foreground = Brush.Parse("#C5D2CA"),
                            [!TextBlock.TextProperty] = new Binding(nameof(NavigationItemProjection.Caption))
                        }
                    }
                }
            }
        };
    }
}
