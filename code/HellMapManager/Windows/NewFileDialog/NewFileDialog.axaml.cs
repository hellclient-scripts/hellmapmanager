using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HellMapManager.Windows.NewFileDialog;
public partial class NewFileDialog : Window
{
    public NewFileDialog()
    {
        InitializeComponent();
        this.DataContext=new NewFileDialogViewModel();
    }
}