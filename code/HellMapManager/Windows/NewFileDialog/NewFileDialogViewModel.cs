using System;
using HellMapManager.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.NewFileDialog;

public class NewFileDialogViewModel : ObservableObject
{
    public string Name { get; set; } = "";
    public string Desc { get; set; } = "";
    public event EventHandler? CloseEvent;
    public event EventHandler? CreateEvent;
    public void OnCancel()
    {
        CloseEvent?.Invoke(this, EventArgs.Empty);
    }
    public void OnCreate(){
        CreateEvent?.Invoke(this,EventArgs.Empty);
    }
    public MapFile ToMapFile(){
        return MapFile.Empty(this.Name,this.Desc);
    }
}
