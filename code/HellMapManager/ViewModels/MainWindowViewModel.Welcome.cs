using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public static ObservableCollection<ExternalLink> HelpLinks
    {
        get => [
            new ExternalLink("基本概念", "https://github.com/hellclient-scripts/hellmapmanager","了解HMM中地图数据的基本概念和使用的最佳实践。"),
            new ExternalLink("脚本支持", "https://github.com/hellclient-scripts/hellmapmanager","了解怎么在各脚本语言中使用HMM的地图数据。"),
            new ExternalLink("接口说明", "https://github.com/hellclient-scripts/hellmapmanager","了解HMM提供的HTTP接口调用。"),
        ];
    }
}