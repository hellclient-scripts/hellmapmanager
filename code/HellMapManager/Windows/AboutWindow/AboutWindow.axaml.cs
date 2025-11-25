using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HellMapManager.Windows.AboutWindow;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
    }
    public void OnOpenLink(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (DataContext is AboutWindowViewModel vm)
        {
            var launcher = TopLevel.GetTopLevel(this)!.Launcher;
            launcher.LaunchUriAsync(new Uri(vm.URL));
        }

    }
}