using System;
using HellMapManager.Models;

namespace HellMapManager.Windows.APIConfigWindow;

public class APIConfigWindowViewModel(APIConfig config)
{
    public APIConfig Config { get; } = config;
}
