using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace HellMapManager.Windows.NewFileDialog;
public partial class NewFileDialog : Window
{
    public NewFileDialog()
    {
        InitializeComponent();
        var vm = new NewFileDialogViewModel();
        this.DataContext = vm;
        vm.CloseEvent += (object? sender, EventArgs e) =>
        {
            Close();
        };
        vm.CreateEvent+=(object? sender, EventArgs e) =>
        {
            Close(vm.ToMapFile());
        };
    }
}