using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using HellMapManager.Services;
using HellMapManager.Models;
using HellMapManager.Windows.NewConditionWindow;
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
                AppUI.Alert("验证失败", err);
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
    public async void OnNewCondition(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditExitWindowViewModel vm)
        {
            var wvm = new NewConditionWindowViewModel(null, vm.ConditionValidator);
            var newCondtionWindow = new NewConditionWindow.NewConditionWindow()
            {
                DataContext = wvm
            };
            var result = await newCondtionWindow.ShowDialog<Condition?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                vm.Item.Conditions.Add(result);
                vm.Item.Arrange();
            }
        }
    }
    public async void OnRemoveCondition(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditExitWindowViewModel vm)
        {
            if (sender is not null && sender is Button bn && bn.DataContext is Condition c)
            {
                if (await AppUI.Confirm("删除", "确定要删除该元素吗？") == false) return;
                vm.Item.Conditions.Remove(c);
            }
        }
    }


}