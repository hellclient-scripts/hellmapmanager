using Avalonia.Controls;
using HellMapManager.Models;
using HellMapManager.Services;
using HellMapManager.ViewModels;
using System;
namespace HellMapManager.Views.Welcome;

public partial class Welcome : UserControl
{
    public Welcome()
    {
        InitializeComponent();
    }
    public void OnOpen(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.OnOpen();
        }
    }
    public void OnNew(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.OnNew();
        }
    }
    public async void OnOpenRecent(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (sender is not null && sender is Border border)
        {
            if (border.DataContext is RecentFile rf)
            {

                await AppUI.Main.OnOpenRecent(rf.Path);
            }
        }
    }
    public void OnOpenLink(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (sender is not null && sender is Border border)
        {
            if (border.DataContext is ExternalLink link)
            {
                if (link.Link != null && link.Link != "")
                {
                    var launcher = TopLevel.GetTopLevel(this)!.Launcher;
                    launcher.LaunchUriAsync(new Uri(link.Link));
                }
            }
        }
    }
}