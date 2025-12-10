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
            OnPropertyChanged(nameof(ServerStatusText));
        };
    }
    public bool IsServerRunning
    {
        get => APIServer.Instance.Running;
    }
    public string ServerStatusText
    {
        get => APIServer.Instance.Running ? $"接口服务正在监听{APIServer.Instance.Port}端口" : "接口服务未启动";
    }
}