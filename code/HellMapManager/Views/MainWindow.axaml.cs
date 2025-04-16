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
    public void InitWindow()
    {
        AppState.Main.ShowRelationMapEvent += this.ShowRelationMap;
    }
    public async void ShowRelationMap(object? sender, RelationMapItem rm)
    {
        var vm = new RelationMapWindowViewModel(rm);
        var Window = new RelationMapWindow(vm);
        await Window.ShowDialog(this);
    }
}