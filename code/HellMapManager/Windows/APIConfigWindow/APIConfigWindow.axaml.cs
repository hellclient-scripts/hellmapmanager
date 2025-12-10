using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace HellMapManager.Windows.APIConfigWindow;

public partial class APIConfigWindow : Window
{
    public APIConfigWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is APIConfigWindowViewModel vm)
        {
            Close(vm.Config);
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        Close();
    }
}