using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using ExampleMatrixSample.ElmView.Core;

namespace ExampleMatrixSample.ElmView.UI.Views;

public partial class AppView : Window
{
    public AppHost Host { get; } = new();

    public AppView()
    {
        Host.Update(Core.App.getDesignView());
        DataContext = Host;
        InitializeComponent();

        // Compiled bindings on the window content need the host in place during load.
        if (!ReferenceEquals(DataContext, Host))
        {
            DataContext = Host;
        }
    }

    private void OnNavigateClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: NavigationItemView item })
        {
            Host.Navigate(item.Example);
        }
    }

    private void OnNameChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            var next = textBox.Text ?? "";

            if (next != Host.View.UserInput.Name)
            {
                Host.SetName(next);
            }
        }
    }

    private void OnEmailChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            var next = textBox.Text ?? "";

            if (next != Host.View.UserInput.Email)
            {
                Host.SetEmail(next);
            }
        }
    }

    private void OnNewsletterClick(object? sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox)
        {
            var next = checkBox.IsChecked ?? false;

            if (next != Host.View.UserInput.Newsletter)
            {
                Host.SetNewsletter(next);
            }
        }
    }

    private void OnFavoriteLanguageChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedItem is string language)
        {
            if (language != Host.View.UserInput.FavoriteLanguage)
            {
                Host.SetFavoriteLanguage(language);
            }
        }
    }

    private void OnExperienceChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (sender is Slider slider && e.Property.Name == nameof(RangeBase.Value))
        {
            var next = (int)slider.Value;

            if (next != Host.View.UserInput.Experience)
            {
                Host.SetExperience(next);
            }
        }
    }

    private void OnNotesChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            var next = textBox.Text ?? "";

            if (next != Host.View.UserInput.Notes)
            {
                Host.SetNotes(next);
            }
        }
    }

    private void OnSubmitFormClick(object? sender, RoutedEventArgs e) => Host.SubmitForm();

    private void OnRollDiceClick(object? sender, RoutedEventArgs e) => Host.RollDice();

    private void OnRefreshHttpClick(object? sender, RoutedEventArgs e) => Host.RefreshHttp();

    private void OnToggleClockFormatClick(object? sender, RoutedEventArgs e) => Host.ToggleClockFormat();

    private async void OnOpenFileClick(object? sender, RoutedEventArgs e)
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false,
            Title = "Open a text-like sample file"
        });

        if (files.Count == 0)
        {
            return;
        }

        var file = files[0];
        await using var stream = await file.OpenReadAsync();
        using var reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true);

        var buffer = new char[480];
        var charsRead = await reader.ReadBlockAsync(buffer, 0, buffer.Length);
        var preview = new string(buffer, 0, charsRead).Replace("\r\n", "\n").Trim();

        if (string.IsNullOrWhiteSpace(preview))
        {
            preview = "(The file did not contain readable text in the preview range.)";
        }

        var properties = await file.GetBasicPropertiesAsync();
        var size = properties.Size is ulong sizeValue ? $"{sizeValue:n0} bytes" : "Unknown size";
        var extension = Path.GetExtension(file.Name);
        var kind = string.IsNullOrWhiteSpace(extension) ? "Unknown type" : extension;
        var location = file.TryGetLocalPath() ?? file.Path?.ToString() ?? "(No path available)";

        Host.FileOpened(new SelectedFileView(file.Name, kind, size, preview, location));
    }

    private void OnClearFileClick(object? sender, RoutedEventArgs e) => Host.ClearFileSelection();

    private void OnNextThemeClick(object? sender, RoutedEventArgs e) => Host.NextSvgTheme();
}
