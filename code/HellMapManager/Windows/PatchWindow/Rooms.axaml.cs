using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Models;


namespace HellMapManager.Windows.PatchWindow;

public partial class Rooms : UserControl
{
    public Rooms()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            vm.FilterRooms();
        }
    }
    public void OnSelectUpdate(object sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            if (sender is not null && sender is CheckBox bn && bn.DataContext is PatchItem item)
            {
                vm.ReCount();
            }
        }
    }
    public void OnCompare(object sender, RoutedEventArgs args)
    {
        if (DataContext is PatchWindowViewModel vm)
        {
            if (sender is not null && sender is CheckBox bn && bn.DataContext is PatchItem item)
            {
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