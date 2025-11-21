using Avalonia.Controls;
using HellMapManager.Services;
using HellMapManager.Helpers;
using HellMapManager.Models;
using HellMapManager.Windows.PatchWindow;
using HellMapManager.Windows.RoomsHExportWindow;
using Avalonia.Interactivity;
using System;
using HellMapManager.Cores;
using System.Threading.Tasks;

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
    public async void OnExportRoomsH(object? sender, RoutedEventArgs args)
    {
        if (AppKernel.MapDatabase.Current == null)
        {
            return;
        }
        var window = new RoomsHExportWindow();
        window.DataContext = new RoomsHExportOption();
        var result = await window.ShowDialog<RoomsHExportOption>(this);
        if (result != null && result is RoomsHExportOption option)
        {
            var path = await AppUI.Main.AskExportRoomsH();
            if (path != null && path != "")
            {
                try
                {
                    AppKernel.MapDatabase.ExportRoomsH(path, option);
                }
                catch (Exception ex)
                {
                    AppUI.Alert("导出失败", ex.Message);
                }
            }
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
            var patch = DiffHelper.CreatePatch(AppKernel.MapDatabase.Current, diffs, true);
            var result = await ShowDiffWindow(diffs, patch);
            if (result != null)
            {
                
            }
        }
    }
    public async Task<Diffs?> ShowDiffWindow(Diffs diffs, Patch patch)
    {
        var window = new PatchWindow();
        window.DataContext = new PatchWindowViewModel(diffs, patch);
        var result = await window.ShowDialog<Diffs?>(this);
        return result;
    }

}
