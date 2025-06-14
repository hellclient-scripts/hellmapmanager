using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Services;
using HellMapManager.Models;
using HellMapManager.Windows.EditDataWindow;
using HellMapManager.Windows.EditExitWindow;
using HellMapManager.Windows.NewTagWindow;
using System.Linq;
namespace HellMapManager.Windows.EditRoomWindow;

public partial class EditRoomWindow : Window
{
    public EditRoomWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm)
        {
            var err = vm.Item.Validate();
            if (err != "")
            {
                AppUI.Alert("验证失败", err);
                return;
            }
            Close(vm.Item.ToRoom());
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm)
        {
            Close(null);
        }
    }
    public async void OnNewData(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm)
        {
            var wvm = new EditDataWindowViewModel(null, vm.DataValidator);
            var editDataWindow = new EditDataWindow.EditDataWindow()
            {
                DataContext = wvm
            };
            var result = await editDataWindow.ShowDialog<Data?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                vm.Item.Data.Add(result);
                vm.Item.Arrange();
            }
        }
    }
    public async void OnEditData(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm)
        {
            if (sender is not null && sender is Button bn && bn.DataContext is Data data)
            {
                var wvm = new EditDataWindowViewModel(data, vm.DataValidator);
                var editDataWindow = new EditDataWindow.EditDataWindow()
                {
                    DataContext = wvm
                };
                var result = await editDataWindow.ShowDialog<Data?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    foreach (var (index, item) in vm.Item.Data.Index())
                    {
                        if (item == data)
                        {
                            vm.Item.Data[index] = result;
                            break;
                        }
                    }
                    vm.Item.Arrange();
                }
            }
        }
    }
    public async void OnRemoveData(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm)
        {
            if (sender is not null && sender is Button bn && bn.DataContext is Data data)
            {
                if (await AppUI.Confirm("删除", "确定要删除该元素吗？") == false) return;
                vm.Item.Data.Remove(data);
                vm.Item.Arrange();
            }
        }
    }
    public async void OnNewTag(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm)
        {
            var wvm = new NewTagWindowViewModel(null, vm.TagValidator);
            var editDataWindow = new NewTagWindow.NewTagWindow()
            {
                DataContext = wvm
            };
            var result = await editDataWindow.ShowDialog<ValueTag?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                vm.Item.Tags.Add(result);
                vm.Item.Arrange();
            }
        }
    }
    public async void OnRemoveTag(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm)
        {
            if (sender is not null && sender is Button bn && bn.DataContext is ValueTag tag)
            {
                if (await AppUI.Confirm("删除", "确定要删除该元素吗？") == false) return;
                vm.Item.Tags.Remove(tag);
            }
        }
    }

    public async void OnNewExit(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm)
        {
            var wvm = new EditExitWindowViewModel(null, vm.ExitValidator);
            var editExitWindow = new EditExitWindow.EditExitWindow()
            {
                DataContext = wvm
            };
            var result = await editExitWindow.ShowDialog<Exit?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                vm.Item.Exits.Add(result);
            }
        }
    }

    public async void OnEditExit(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm)
        {
            if (sender is not null && sender is Button bn && bn.DataContext is Exit data)
            {
                var wvm = new EditExitWindowViewModel(data, vm.ExitValidator);
                var editExitWindow = new EditExitWindow.EditExitWindow()
                {
                    DataContext = wvm
                };
                var result = await editExitWindow.ShowDialog<Exit?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    foreach (var (index, item) in vm.Item.Exits.Index())
                    {
                        if (item == data)
                        {
                            vm.Item.Exits[index] = result;
                            break;
                        }
                    }
                }
            }
        }
    }
    public async void OnRemoveExit(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm)
        {
            if (sender is not null && sender is Button bn && bn.DataContext is Exit exit)
            {
                if (await AppUI.Confirm("删除", "确定要删除该元素吗？") == false) return;
                vm.Item.Exits.Remove(exit);
            }
        }
    }

    public void OnEnterEditing(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm && vm.Editable)
        {
            vm.EnterEdit();
        }
    }
    public void OnCancel(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm && vm.Editing)
        {
            vm.CancelEdit();
        }
    }
}