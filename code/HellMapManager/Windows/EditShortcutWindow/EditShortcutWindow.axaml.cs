using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Services;
using HellMapManager.Models;
using HellMapManager.Windows.NewConditionWindow;

namespace HellMapManager.Windows.EditShortcutWindow;

public partial class EditShortcutWindow : Window
{
    public EditShortcutWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditShortcutWindowViewModel vm)
        {
            var err = vm.Item.Validate();
            if (err != "")
            {
                AppUI.Alert("验证失败", err);
                return;
            }
            Close(vm.Item.ToShortcut());
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditShortcutWindowViewModel vm)
        {
            Close(null);
        }
    }

    public void OnEnterEditing(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditShortcutWindowViewModel vm && vm.Editable)
        {
            vm.EnterEdit();
        }
    }
    public void OnCancel(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditShortcutWindowViewModel vm && vm.Editing)
        {
            vm.CancelEdit();
        }
    }
    public async void OnNewCondition(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditShortcutWindowViewModel vm)
        {
            var wvm = new NewConditionWindowViewModel(null, vm.ConditionValidator);
            var newCondtionWindow = new NewConditionWindow.NewConditionWindow()
            {
                DataContext = wvm
            };
            var result = await newCondtionWindow.ShowDialog<ValueCondition?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                vm.Item.Conditions.Add(result);
                vm.Item.Arrange();
            }
        }
    }
    public async void OnRemoveCondition(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditShortcutWindowViewModel vm)
        {
            if (sender is not null && sender is Button bn && bn.DataContext is ValueCondition c)
            {
                if (await AppUI.Confirm("删除", "确定要删除该元素吗？") == false) return;
                vm.Item.Conditions.Remove(c);
            }
        }
    }
    public async void OnNewRoomCondition(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditShortcutWindowViewModel vm)
        {
            var wvm = new NewConditionWindowViewModel(null, vm.RoomConditionValidator);
            var newCondtionWindow = new NewConditionWindow.NewConditionWindow()
            {
                DataContext = wvm
            };
            var result = await newCondtionWindow.ShowDialog<ValueCondition?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                vm.Item.RoomConditions.Add(result);
                vm.Item.Arrange();
            }
        }
    }
    public async void OnRemoveRoomCondition(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditShortcutWindowViewModel vm)
        {
            if (sender is not null && sender is Button bn && bn.DataContext is ValueCondition c)
            {
                if (await AppUI.Confirm("删除", "确定要删除该元素吗？") == false) return;
                vm.Item.RoomConditions.Remove(c);
            }
        }
    }


}