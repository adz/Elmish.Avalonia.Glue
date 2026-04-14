using Avalonia;
using ExampleMatrixSample.UI;

AppBuilder
    .Configure<App>()
    .UsePlatformDetect()
    .StartWithClassicDesktopLifetime(args);
