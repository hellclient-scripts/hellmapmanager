using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Services;
namespace HellMapManager.Windows.NewTagWindow;

public partial class NewTagWindow : Window
{
    public NewTagWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is NewTagWindowViewModel vm)
        {
            var err = vm.Item.Validate();
            if (err != "")
            {
                AppUI.Alert("验证失败", err);
                return;
            }
            Close(vm.Item.ToTag());
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is NewTagWindowViewModel vm)
        {
            Close(null);
        }
    }
}