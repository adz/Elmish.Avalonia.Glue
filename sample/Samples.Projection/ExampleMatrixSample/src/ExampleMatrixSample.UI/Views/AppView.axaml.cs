using Avalonia.Controls;
using ExampleMatrixSample.UI.ViewModels;

namespace ExampleMatrixSample.UI.Views;

using CoreApp = ExampleMatrixSample.Core.App;

public partial class AppView : Window
{
    public AppProjection Projection { get; } = new();

    public AppView()
    {
        InitializeComponent();
        DataContext = Projection;
        Projection.Update(CoreApp.getDesignModel());
    }
}
