using HellMapManager.Services;
using HellMapManager.Helpers;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Threading.Tasks;
using HellMapManager.Models;



namespace HellMapManager.Windows.PatchWindow;

public partial class PatchWindow : Window
{
    public PatchWindow()
    {
        InitializeComponent();
    }
    public void OnSelectedTab(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            vm.OnSelectUpdate();
        }
    }
    public void Apply(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            var diffs = vm.Patch.Apply();
            Close(diffs);
        }
    }
    public void Export(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            var diffs = vm.Patch.Apply();
            if (diffs.Items.Count > 0)
            {
                AppUI.Main.SavePatch(diffs);
            }
        }

    }
    public void OnSelectAll(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            vm.SelectAll(true);
        }
    }
    public void OnUnSelectAll(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            vm.SelectAll(false);
        }
    }
    public void OnSelectNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            vm.SelectByMode(DiffMode.New, true);
        }
    }
    public void OnUnSelectNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            vm.SelectByMode(DiffMode.New, false);
        }
    }
    public void OnSelectNormal(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            vm.SelectByMode(DiffMode.Normal, true);
        }
    }
    public void OnUnSelectNormal(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            vm.SelectByMode(DiffMode.Normal, false);
        }
    }
    public void OnSelectRemoved(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            vm.SelectByMode(DiffMode.Removed, true);
        }
    }
    public void OnUnSelectRemoved(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            vm.SelectByMode(DiffMode.Removed, false);
        }
    }
}