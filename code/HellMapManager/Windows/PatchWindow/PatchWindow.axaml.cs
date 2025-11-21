using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;



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

}