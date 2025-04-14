using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using HellMapManager.Services;
using HellMapManager.States;
using HellMapManager.Windows.RelationMapWindow;
namespace HellMapManager.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    public required AppState AppState;
    public void InitWindow()
    {
        AppState.ShowRelationMapEvent += this.ShowRelationMap;
    }
    public async void ShowRelationMap(object? sender, RelationMapItem rm)
    {
        var vm = new RelationMapWindowViewModel(AppState, rm);
        var Window = new RelationMapWindow(vm);
        await Window.ShowDialog(this);
    }
}