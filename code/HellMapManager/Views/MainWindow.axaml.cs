using Avalonia.Controls;
using HellMapManager.Services;
using HellMapManager.Helpers;
using HellMapManager.Models;
using HellMapManager.Misc;
using HellMapManager.Windows.PatchWindow;
using HellMapManager.Windows.AboutWindow;
using HellMapManager.Windows.APIConfigWindow;
using Avalonia.Interactivity;
using System;
using HellMapManager.Cores;
using System.Threading.Tasks;
using HellMapManager.Services.API;
using Microsoft.AspNetCore.Authentication;

namespace HellMapManager.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    public async void OnOpenRecent(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is MenuItem mi && mi.DataContext is RecentFile rf)
        {
            await AppUI.Main.OnOpenRecent(rf.Path);
        }
    }
    public async void OnDiffMapFile(object? sender, RoutedEventArgs args)
    {
        if (AppKernel.MapDatabase.Current == null)
        {
            return;
        }
        var diffs = await AppUI.Main.DiffMapFile();
        if (diffs != null)
        {
            var result = await ShowDiffWindow(AppKernel.MapDatabase.Current, diffs);
            if (result != null && result.Items.Count > 0)
            {
                DiffHelper.Apply(result, AppKernel.MapDatabase.Current);
                AppKernel.MapDatabase.Current.MarkAsModified();
                AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public async void OnDiffOriginal(object? sender, RoutedEventArgs args)
    {
        if (AppKernel.MapDatabase.Current == null)
        {
            return;
        }
        var diffs = await AppUI.Main.DiffOriginal();
        if (diffs != null)
        {
            var result = await ShowDiffWindow(AppKernel.MapDatabase.Current, diffs);
            if (result != null && result.Items.Count > 0)
            {
                DiffHelper.Apply(result, AppKernel.MapDatabase.Current);
                AppKernel.MapDatabase.Current.MarkAsModified();
                AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
            }
        }
    }


    public async Task<Diffs?> ShowDiffWindow(MapFile mf, Diffs diffs)
    {
        var window = new PatchWindow();
        window.DataContext = new PatchWindowViewModel(mf, diffs);
        var result = await window.ShowDialog<Diffs?>(this);
        return result;
    }
    public async void OnOpenPatch(object? sender, RoutedEventArgs args)
    {
        if (AppKernel.MapDatabase.Current == null)
        {
            return;
        }
        var file = await AppUI.Main.AskOpenPatch();
        if (file == null || file == "")
        {
            return;
        }
        try
        {
            var diffs = HMPFile.Open(file);
            if (diffs != null)
            {
                var result = await ShowDiffWindow(AppKernel.MapDatabase.Current, diffs);
                if (result != null && result.Items.Count > 0)
                {
                    DiffHelper.Apply(result, AppKernel.MapDatabase.Current);
                    AppKernel.MapDatabase.Current.MarkAsModified();
                    AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
                }

            }
        }
        catch (Exception ex)
        {
            AppUI.Alert("打开变更补丁文件失败", ex.Message);
        }
    }
    public void OnAbout(object? sender, RoutedEventArgs args)
    {
        var w = new AboutWindow();
        w.DataContext = new AboutWindowViewModel();
        w.Show();
    }
    public void OpenURLHomepage(object? sender, RoutedEventArgs args)
    {
        TopLevel.GetTopLevel(this)!.Launcher.LaunchUriAsync(new Uri(Links.Homepage));
    }
    public void OpenURLTerm(object? sender, RoutedEventArgs args)
    {
        TopLevel.GetTopLevel(this)!.Launcher.LaunchUriAsync(new Uri(Links.Term));
    }
    public void OpenURLScriptIntro(object? sender, RoutedEventArgs args)
    {
        TopLevel.GetTopLevel(this)!.Launcher.LaunchUriAsync(new Uri(Links.ScriptInro));
    }
    public void OpenURLBestPractices(object? sender, RoutedEventArgs args)
    {
        TopLevel.GetTopLevel(this)!.Launcher.LaunchUriAsync(new Uri(Links.BestPractices));
    }

    public void OpenURLAPI(object? sender, RoutedEventArgs args)
    {
        TopLevel.GetTopLevel(this)!.Launcher.LaunchUriAsync(new Uri(Links.API));
    }


    public async void StartServer(object? sender, RoutedEventArgs args)
    {
        APIServer.Instance.Start();
    }
    public async void StopServer(object? sender, RoutedEventArgs args)
    {
        await APIServer.Instance.Stop();
    }
    public async void ConfigServer(object? sender, RoutedEventArgs args)
    {
        var w = new APIConfigWindow();
        w.DataContext = new APIConfigWindowViewModel(APIConfig.From(AppKernel.MapDatabase.Settings));
        var result = await w.ShowDialog<APIConfig>(this);
        if (result is not null)
        {
            result.Apply(AppKernel.MapDatabase.Settings);
            AppKernel.MapDatabase.RaiseSettingsUpdatedEvent(this);
        }
    }

}
