using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.NewFileDialog;

public class NewFileDialogViewModel:ObservableObject
{
    public string Name{ get; set;} = "";
    public string Desc{ get; set;} = "";
}
