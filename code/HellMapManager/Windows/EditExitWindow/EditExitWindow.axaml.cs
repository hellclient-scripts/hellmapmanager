using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using HellMapManager.Services;
namespace HellMapManager.Windows.EditExitWindow;

public partial class EditExitWindow : Window
{
    public EditExitWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditExitWindowViewModel vm)
        {
            var err = vm.Item.Validate();
            if (err != "")
            {
                DialogHelper.Alert("验证失败", err);
                return;
            }
            Close(vm.Item.ToExit());
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditExitWindowViewModel vm)
        {
            Close(null);
        }
    }
}