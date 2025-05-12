using Avalonia.Controls;
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