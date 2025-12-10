using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.Misc;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public static ObservableCollection<ExternalLink> HelpLinks
    {
        get => [
            new ExternalLink("基本概念", Misc.Links.Term,"了解HMM中地图数据的基本概念和使用的最佳实践。"),
            new ExternalLink("脚本支持", Misc.Links.ScriptInro,"了解怎么在各脚本语言中使用HMM的地图数据。"),
            new ExternalLink("接口文档", Misc.Links.API,"了解怎么通过HTTP接口使用HMM的地图数据。"),
        ];
    }
}