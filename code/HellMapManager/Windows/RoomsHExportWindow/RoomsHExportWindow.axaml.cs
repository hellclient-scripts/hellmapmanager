using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using HellMapManager.Helpers;
namespace HellMapManager.Windows.RoomsHExportWindow;

public partial class RoomsHExportWindow : Window
{
    public RoomsHExportWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is RoomsHExportOption vm)
        {
            Close(vm);
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is RoomsHExportOption vm)
        {
            Close(null);
        }
    }

}