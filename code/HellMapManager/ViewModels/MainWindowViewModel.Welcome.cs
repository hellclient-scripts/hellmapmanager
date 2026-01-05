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
            new ExternalLink("最佳实践", Misc.Links.API,"了解使用HMM的最佳实践。"),
        ];
    }
    public static ObservableCollection<ExternalLink> CommunityLinks
    {
        get => [
            new ExternalLink("Github", Misc.Links.Homepage,"访问HMM的GitHub主页，获取最新版本和发布信息。"),
            new ExternalLink("社区", Misc.Links.Forum,"加入HellClient社区，参与讨论并获取帮助。"),
        ];
    }
}