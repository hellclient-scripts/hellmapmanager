using Avalonia.Controls;
using HellMapManager.Services;
using HellMapManager.Models;
using HellMapManager.States;
using Avalonia.Interactivity;
using System.Threading.Tasks;
namespace HellMapManager.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    public async void OnOpenRecent(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is MenuItem mi && mi.DataContext is RecentFile rf)
        {
            await AppUI.Main.OnOpenRecent(rf.Path);
        }
    }

}
