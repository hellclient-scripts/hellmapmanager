using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace HellMapManager.Windows.PatchWindow;

public partial class Markers : UserControl
{
    public Markers()
    {
        InitializeComponent();
    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            vm.FilterMarkers();
        }
    }
    public void OnSelectUpdate(object sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            if (sender is not null && sender is CheckBox bn && bn.DataContext is PatchItem item)
            {
                vm.SelectedTab.ReCount();
                vm.ReCount(true);
            }
        }
    }
    public async void OnCompare(object sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            if (sender is not null && sender is Button bn && bn.DataContext is PatchItem item)
            {
                var w = new CompareWindow();
                w.DataContext = item;
                await w.ShowDialog((TopLevel.GetTopLevel(this) as Window)!);
            }
        }
    }

    public void OnDataGridLoadingRow(object sender, DataGridRowEventArgs e)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            if (e.Row.DataContext is PatchItem item)

                vm.SetPatchItemRowBackground(item, e.Row);
        }
    }

}