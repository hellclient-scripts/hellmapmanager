using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Services;
namespace HellMapManager.Windows.EditSnapshotWindow;

public partial class EditSnapshotWindow : Window
{
    public EditSnapshotWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditSnapshotWindowViewModel vm)
        {
            var err = vm.Item.Validate();
            if (err != "")
            {
                AppUI.Alert("验证失败", err);
                return;
            }
            Close(vm.Item.ToSnapshot());
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditSnapshotWindowViewModel vm)
        {
            Close(null);
        }
    }

    // public void OnEnterEditing(object? sender, RoutedEventArgs args)
    // {
    //     if (DataContext is EditSnapshotWindowViewModel vm && vm.Editable)
    //     {
    //         vm.EnterEdit();
    //     }
    // }
    public void OnCancel(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditSnapshotWindowViewModel vm && vm.Editing)
        {
            vm.CancelEdit();
        }
    }
}