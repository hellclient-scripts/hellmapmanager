using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Services;
namespace HellMapManager.Windows.EditRegionItemWindow;

public partial class EditRegionItemWindow : Window
{
    public EditRegionItemWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRegionItemWindowViewModel vm)
        {
            var err = vm.Item.Validate();
            if (err != "")
            {
                AppUI.Alert("验证失败", err);
                return;
            }
            Close(vm.Item.ToRegionItem());
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRegionItemWindowViewModel vm)
        {
            Close(null);
        }
    }
}