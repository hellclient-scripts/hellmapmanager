using Avalonia;
using HellMapManager.Helpers;
using HellMapManager.Services.API;
using System;
using System.Text;

namespace HellMapManager;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        CommandLineHelper.Current.Prase(args);
        if (CommandLineHelper.Current.Headless != true)
        {
            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
            return;
        }
        APIServer.Instance.LaunchHeadless(CommandLineHelper.Current.OpenFile);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
