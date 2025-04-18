using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using HellMapManager.Services;
namespace HellMapManager.Windows.NewConditionWindow;

public partial class NewConditionWindow : Window
{
    public NewConditionWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is NewConditionWindowViewModel vm)
        {
            var err = vm.Item.Validate();
            if (err != "")
            {
                AppUI.Alert("验证失败", err);
                return;
            }
            Close(vm.Item.ToCondition());
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is NewConditionWindowViewModel vm)
        {
            Close(null);
        }
    }
}