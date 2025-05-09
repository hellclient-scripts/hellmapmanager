using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using HellMapManager.ViewModels;
using HellMapManager.Views;
using HellMapManager.Cores;
using System.Diagnostics.CodeAnalysis;
using HellMapManager.Services;

using HellMapManager.Helpers;
using System.IO;
namespace HellMapManager;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var path = Path.GetDirectoryName(System.Environment.ProcessPath);
        var settingspath = System.IO.Path.Join([path, AppPreset.SettingsFileName]);
        var settingsHelper = new SettingsHelper(settingspath);
        var settings = settingsHelper.Load();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            AppUI.Main.MapDatabase = AppKernel.Instance.MapDatabase;
            AppUI.Main.Desktop = desktop;
            AppKernel.Instance.MapDatabase.SettingsUpdatedEvent += (sender, args) => { settingsHelper.Save(AppKernel.Instance.MapDatabase.Settings); };
            AppKernel.Instance.MapDatabase.ExitEvent += (sender, args) => { desktop.Shutdown(0); };
            if (settings is not null)
            {
                AppKernel.Instance.MapDatabase.Settings = settings;
            }
            var mw = new MainWindow()
            {
                DataContext = new MainWindowViewModel()
            };
            desktop.MainWindow = mw;
            mw.Closing += async (s, e) =>
            {
                if (!AppUI.Main.ConfirmedExit)
                {
                    e.Cancel = true;
                    if (await AppUI.Main.ConfirmExit())
                    {
                        if (AppUI.Main.ConfirmedExit)
                            mw.Close();

                    }
                }
            }
        ;
        }
        base.OnFrameworkInitializationCompleted();
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}