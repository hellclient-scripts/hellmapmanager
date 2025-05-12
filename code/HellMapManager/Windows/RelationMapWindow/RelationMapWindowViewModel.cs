using System;
using CommunityToolkit.Mvvm.ComponentModel;
using AvaloniaGraphControl;
using HellMapManager.Helpers;
using System.Collections.Generic;
using HellMapManager.Models;
using HellMapManager.Cores;
using System.Collections.ObjectModel;
using System.Linq;
namespace HellMapManager.Windows.RelationMapWindow;
public class ViewItem
{
    public ViewItem(RelationMapItem item)
    {
        Item = item;
    }
    public RelationMapItem Item { get; set; }
    public bool IsLevel0 { get => Item.Depth == 0; }
    public bool IsLevel1 { get => Item.Depth == 1; }
    public bool IsLevel2 { get => Item.Depth == 2; }
    public bool IsLevelOther { get => Item.Depth > 2; }
    private string GroupSuffix
    {
        get
        {
            return Item.Room.Group == "" ? "" : $"/{Item.Room.Group}";
        }
    }
    public string LabelWithKey
    {
        get
        {
            return $"{Item.Room.Name}({Item.Room.Key}){GroupSuffix}";
        }
    }
    public string Tags
    {
        get => string.Join(",", Item.Room.Tags);
    }
    public bool IsDataEmpty
    {
        get => Item.Room.Data.Count == 0;
    }
    public bool HasData
    {
        get => Item.Room.Data.Count > 0;
    }

    public bool IsDescEmpty
    {
        get => Item.Room.Desc == "";
    }
    public string DescInfo
    {
        get => Item.Room.Desc == "" ? "<无描述>" : Item.Room.Desc;
    }
    public bool IsNameEmpty
    {
        get => Item.Room.Name == "";
    }
    public string NameInfo
    {
        get => Item.Room.Name == "" ? "<无房间名>" : Item.Room.Name;
    }
    public bool IsGroupEmpty
    {
        get => Item.Room.Group == "";
    }
    public string GroupInfo
    {
        get => Item.Room.Group == "" ? "<无分组>" : Item.Room.Group;
    }
    public bool IsTagsEmpty
    {
        get => Item.Room.Tags.Count == 0;
    }
    public string TagsInfo
    {
        get => Item.Room.Tags.Count == 0 ? "<无标签>" : Tags;
    }
    public ObservableCollection<Exit> Exits
    {
        get
        {
            var exits = new ObservableCollection<Exit>(Item.Room.Exits.ToArray());
            return exits;
        }
    }
}

public class RelationMapWindowViewModel : ObservableObject
{
    public RelationMapWindowViewModel(RelationMapItem item)
    {
        Item = item;
    }
    public List<string> Histories = [];
    public RelationMapItem Item;
    public string Title
    {
        get => "房间   " + Item.Room.Name + "(" + Item.Room.Key + ")" + "   关系地图";
    }
    public ViewItem Current
    {
        get => new ViewItem(Item);
    }
    public Graph MyGraph
    {
        get
        {
            var graph = new Graph();
            var ViewItems = new Dictionary<string, ViewItem>();
            List<RelationMapItem> items = [Item];
            if (Item.Relations.Count > 0)
            {
                while (items.Count > 0)
                {
                    var walking = items;
                    items = [];
                    foreach (var w in walking)
                    {
                        foreach (var t in w.Relations)
                        {
                            ViewItem from;
                            if (ViewItems.ContainsKey(w.Room.Key))
                            {
                                from = ViewItems[w.Room.Key];
                            }
                            else
                            {
                                from = new ViewItem(w);
                                ViewItems[w.Room.Key] = from;
                            }
                            //relationmap的设定，不可能有回头路，不需要判断
                            ViewItem to;
                            to = new ViewItem(t.Target);
                            ViewItems[t.Target.Room.Key] = to;
                            var edge = new Edge(from, to, tailSymbol: t.Type == RelationType.OneSideTo ? Edge.Symbol.None : Edge.Symbol.Arrow, headSymbol: Edge.Symbol.Arrow);
                            graph.Edges.Add(edge);
                            items.Add(t.Target);
                        }
                    }
                }
            }
            else
            {
                var vi = new ViewItem(Item);
                var edge = new LonelyEdge(vi);
                graph.Edges.Add(edge);
            }
            return graph;
        }
    }
    public bool HasHistory
    {
        get => Histories.Count > 0;
    }
    private void AddHistory(string key)
    {
        Histories.Add(key);
        if (Histories.Count > AppPreset.RelationMaxHistoriesCount)
        {
            Histories = Histories.Slice(Histories.Count - AppPreset.RelationMaxHistoriesCount, AppPreset.RelationMaxHistoriesCount);
        }
    }
    public void HistoryBack()
    {
        if (Histories.Count > 0)
        {
            var last = Histories.Last();
            Histories = Histories.Slice(0, Histories.Count - 1);
            DoEnterRoomKey(last, false);
        }
    }
    public void EnterViewItem(object obj)
    {
        if (obj is ViewItem item && AppKernel.Instance.MapDatabase.Current is not null)
        {
            var vi = item;
            EnterRoomKey(vi.Item.Room.Key);
        }
    }
    public void EnterRoomKey(string key)
    {
        DoEnterRoomKey(key, true);
    }
    private void DoEnterRoomKey(string key, bool modfiyHistory)
    {
        if (key != "" && key != Item.Room.Key && AppKernel.Instance.MapDatabase.Current is not null)
        {
            var item = RelationMapper.RelationMap(AppKernel.Instance.MapDatabase.Current, key, AppPreset.RelationMaxDepth);
            if (item is not null)
            {
                if (modfiyHistory)
                {
                    AddHistory(Item.Room.Key);
                }
                Item = item;
                OnPropertyChanged(nameof(MyGraph));
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Current));
                OnPropertyChanged(nameof(HasHistory));
                RefreshEvent?.Invoke(this, EventArgs.Empty);
            }

        }
    }
    public void OnHistoryBack()
    {
        HistoryBack();
    }
    public event EventHandler? RefreshEvent;
}

public class LonelyEdge(ViewItem from) : Edge(from, from)
{
}