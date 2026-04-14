using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Elmish.Avalonia.Glue;
using ExampleMatrixSample.Core;
using ExampleMatrixSample.UI.Views;

namespace ExampleMatrixSample.UI;

public class App : Application
{
    private IDisposable? _host;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var view = new AppView();
            view.Projection.AttachFilePicker(() => OpenFilePreviewAsync(view));

            _host = ElmishHost.startAndBind(
                Core.App.program,
                model => view.Projection.Update(model),
                view.Projection.SetDispatch);

            desktop.MainWindow = view;
            desktop.Exit += (_, _) => _host?.Dispose();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static async Task<FilesPage.SelectedFile?> OpenFilePreviewAsync(Window owner)
    {
        var files = await owner.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false,
            Title = "Open a text-like sample file"
        });

        if (files.Count == 0)
        {
            return null;
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

        return new FilesPage.SelectedFile(file.Name, kind, size, preview, location);
    }
}
