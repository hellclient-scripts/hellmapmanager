using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using HellMapManager.ViewModels;
using Avalonia.Interactivity;

namespace HellMapManager.Views.Mapfile.Rooms;

public partial class Rooms : UserControl
{
    public Rooms()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FilterRooms();
        }
    }
}