using Avalonia;
using OpsCenterSample.UI;

AppBuilder
    .Configure<App>()
    .UsePlatformDetect()
    .StartWithClassicDesktopLifetime(args);
