using System;
using HellMapManager.Services.API;

namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitServer()
    {
        APIServer.Instance.UpdatedEvent += (sender, args) =>
        {
            OnPropertyChanged(nameof(IsServerRunning));
        };
    }
    public bool IsServerRunning
    {
        get => APIServer.Instance.Running;
    }

}