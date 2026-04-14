using Avalonia.Controls;

namespace ExampleMatrixSample.ElmView.UI.Views;

public partial class UserInputPrototypeView : UserControl
{
    public AppHost Host { get; } = new();

    public UserInputPrototypeView()
    {
        Host.Update(ExampleMatrixSample.ElmView.Core.App.getDesignView());
        DataContext = Host;
        InitializeComponent();
    }
}
