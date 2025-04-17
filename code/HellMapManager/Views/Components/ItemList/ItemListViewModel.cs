using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Views.Components.ItemList;

public class ItemListViewModel : ObservableObject
{
    public bool CanUp { get; set; } = false;
    public bool CanDown { get; set; } = false;
    public bool CanDelete { get; set; } = false;

    public void SetSelection(ListBox lb)
    {
        if (lb.SelectedItem is null)
        {
            CanUp = false;
            CanDown = false;
            CanDelete = false;
        }
        else
        {
            CanDelete = true;
            CanUp = lb.SelectedIndex > 0;
            CanDown = lb.SelectedIndex < lb.Items.Count - 1;
        }
        OnPropertyChanged(nameof(CanUp));
        OnPropertyChanged(nameof(CanDown));
        OnPropertyChanged(nameof(CanDelete));
    }
}
