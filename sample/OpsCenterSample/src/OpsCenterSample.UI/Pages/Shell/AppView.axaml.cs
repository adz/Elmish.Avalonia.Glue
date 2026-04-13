using Avalonia.Controls;

namespace OpsCenterSample.UI.Pages.Shell;

public partial class AppView : Window
{
    public AppProjection Projection { get; } = new();

    public AppView()
    {
        InitializeComponent();
        DataContext = Projection;
    }
}
