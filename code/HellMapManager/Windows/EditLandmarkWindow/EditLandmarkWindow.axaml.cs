using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Services;
namespace HellMapManager.Windows.EditLandmarkWindow;

public partial class EditLandmarkWindow : Window
{
    public EditLandmarkWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditLandmarkWindowViewModel vm)
        {
            var err = vm.Item.Validate();
            if (err != "")
            {
                AppUI.Alert("验证失败", err);
                return;
            }
            Close(vm.Item.ToLandmark());
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditLandmarkWindowViewModel vm)
        {
            Close(null);
        }
    }

    public void OnEnterEditing(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditLandmarkWindowViewModel vm && vm.Editable)
        {
            vm.EnterEdit();
        }
    }
    public void OnCancel(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditLandmarkWindowViewModel vm && vm.Editing)
        {
            vm.CancelEdit();
        }
    }
}