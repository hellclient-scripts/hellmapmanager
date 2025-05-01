using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Services;
using HellMapManager.Models;
using HellMapManager.Windows.EditRegionItemWindow;
using System.Linq;

namespace HellMapManager.Windows.EditRegionWindow;

public partial class EditRegionWindow : Window
{
    public EditRegionWindow()
    {
        InitializeComponent();
    }
    public async void OnNewItem(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRegionWindowViewModel vm)
        {
            var wvm = new EditRegionItemWindowViewModel(null, vm.RegionItemValidator);
            var editExitWindow = new EditRegionItemWindow.EditRegionItemWindow()
            {
                DataContext = wvm
            };
            var result = await editExitWindow.ShowDialog<RegionItem?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                vm.Item.Items.Add(result);
            }
        }
    }

    public async void OnEditItem(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRegionWindowViewModel vm)
        {
            if (sender is not null && sender is Button bn && bn.DataContext is RegionItem data)
            {
                var wvm = new EditRegionItemWindowViewModel(data, vm.RegionItemValidator);
                var editExitWindow = new EditRegionItemWindow.EditRegionItemWindow()
                {
                    DataContext = wvm
                };
                var result = await editExitWindow.ShowDialog<RegionItem?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    foreach (var (index, item) in vm.Item.Items.Index())
                    {
                        if (item == data)
                        {
                            vm.Item.Items[index] = result;
                            break;
                        }
                    }
                }
            }
        }
    }
    public async void OnRemoveItem(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRegionWindowViewModel vm)
        {
            if (sender is not null && sender is Button bn && bn.DataContext is RegionItem regionItem)
            {
                if (await AppUI.Confirm("删除", "确定要删除该元素吗？") == false) return;
                vm.Item.Items.Remove(regionItem);
            }
        }
    }

    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRegionWindowViewModel vm)
        {
            var err = vm.Item.Validate();
            if (err != "")
            {
                AppUI.Alert("验证失败", err);
                return;
            }
            Close(vm.Item.ToRegion());
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRegionWindowViewModel vm)
        {
            Close(null);
        }
    }

    public void OnEnterEditing(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRegionWindowViewModel vm && vm.Editable)
        {
            vm.EnterEdit();
        }
    }
    public void OnCancel(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRegionWindowViewModel vm && vm.Editing)
        {
            vm.CancelEdit();
        }
    }

}