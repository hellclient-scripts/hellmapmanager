using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace HellMapManager.Windows.PatchWindow;

public partial class CompareWindow : Window
{
    public CompareWindow()
    {
        InitializeComponent();
        this.Focus();
    }
    private void OnWindowKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
    }
}