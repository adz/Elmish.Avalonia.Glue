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
        var button = new Button
        {
            Padding = new Thickness(14, 12),
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            Background = Brushes.Transparent,
            BorderBrush = null
        };
        button.Bind(Button.CommandProperty, new Binding(nameof(NavigationItemProjection.SelectCommand)));

        var card = new Border
        {
            CornerRadius = new CornerRadius(14),
            Padding = new Thickness(12, 10)
        };
        card.Bind(Border.BackgroundProperty, new Binding(nameof(NavigationItemProjection.CardBackground)));

        var stack = new StackPanel { Spacing = 2 };
        var title = new TextBlock
        {
            FontSize = 15,
            FontWeight = FontWeight.SemiBold,
            Foreground = Brush.Parse("#F8F3EA")
        };
        title.Bind(TextBlock.TextProperty, new Binding(nameof(NavigationItemProjection.Title)));
        stack.Children.Add(title);

        var caption = new TextBlock
        {
            FontSize = 12,
            Foreground = Brush.Parse("#C5D2CA")
        };
        caption.Bind(TextBlock.TextProperty, new Binding(nameof(NavigationItemProjection.Caption)));
        stack.Children.Add(caption);

        card.Child = stack;
        button.Content = card;
        return button;
    }
}
