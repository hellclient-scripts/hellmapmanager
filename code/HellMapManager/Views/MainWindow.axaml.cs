using Avalonia.Controls;
using HellMapManager.Services;
using HellMapManager.Models;
using HellMapManager.Windows.RoomsHExportWindow;
using Avalonia.Interactivity;
using System;
using HellMapManager.States;
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
        if (AppState.Main.Current == null)
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
                    AppState.Main.ExportRoomsH(path, option);
                }
                catch (Exception ex)
                {
                    AppUI.Alert("导出失败", ex.Message);
                }
            }
        }
    }

}
