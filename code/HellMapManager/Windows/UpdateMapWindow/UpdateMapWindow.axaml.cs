using Avalonia.Controls;
using Avalonia.Interactivity;

namespace HellMapManager.Windows.UpdateMapWindow;

public partial class UpdateMapWindow : Window
{
    public UpdateMapWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is UpdateMapWindowViewModel vm)
        {
            Close(vm.Settings);
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is UpdateMapWindowViewModel vm)
        {
            Close(null);
        }
    }
}