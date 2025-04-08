using System;
using Avalonia.Controls.Chrome;
using CommunityToolkit.Mvvm.ComponentModel;
using AvaloniaGraphControl;
using HellMapManager.Services;
using System.Collections.Generic;
using HellMapManager.Models;
using Avalonia.Controls;
using HellMapManager.States;
using Avalonia;
namespace HellMapManager.Windows.RelationMapWindow;
public class ViewItem
{
    public ViewItem(RelationMapItem item)
    {
        this.Item = item;
    }
    public RelationMapItem Item { get; set; }
    public bool IsLevel0 { get => Item.Depth == 0; }
    public bool IsLevel1 { get => Item.Depth == 1; }
    public bool IsLevel2 { get => Item.Depth == 2; }
    public bool IsLevelOther { get => Item.Depth > 2; }
    private string ZoneInfo{
        get{
            return Item.Room.Zone==""?"":$"/{Item.Room.Zone}";
        }
    }
    public string LabelWithKey
    {
        get
        {
            return $"{Item.Room.Name}({Item.Room.Key}){ZoneInfo}";
        }
    }
    public string Tags
    {
        get => string.Join(",", Item.Room.Tags);
    }
}

public class RelationMapWindowViewModel : ObservableObject
{
    public RelationMapWindowViewModel(AppState state, RelationMapItem item)
    {
        AppState = state;
        Item = item;
    }
    public AppState AppState;
    public RelationMapItem Item;
    public string Title
    {
        get => "房间   " + Item.Room.Name + "(" + Item.Room.Key + ")" + "   关系地图";
    }
    public Graph MyGraph
    {
        get
        {
            var graph = new Graph();
            var ViewItems = new Dictionary<string, ViewItem>();
            List<RelationMapItem> items = [Item];
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
                        ViewItem to;
                        if (ViewItems.ContainsKey(t.Target.Room.Key))
                        {
                            to = ViewItems[t.Target.Room.Key];
                        }
                        else
                        {
                            to = new ViewItem(t.Target);
                            ViewItems[t.Target.Room.Key] = to;
                        }
                        var edge = new Edge(from, to, tailSymbol: t.Type == RelationType.OneSideTo ? Edge.Symbol.None : Edge.Symbol.Arrow, headSymbol: Edge.Symbol.Arrow);
                        graph.Edges.Add(edge);
                        items.Add(t.Target);
                    }
                }
            }
            return graph;
        }
    }
    public void OnContextMenuEnter(object obj)
    {
        if (obj is ViewItem && AppState.Current is not null)
        {
            var vi = (ViewItem)obj;
            var item = Mapper.RelationMap(AppState.Current, vi.Item.Room.Key, 5);
            if (item is not null)
            {
                this.Item = item;
                OnPropertyChanged(nameof(this.MyGraph));
                OnPropertyChanged(nameof(this.Title));
                RefreshEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public void OnClick(object sender)
    {
        Console.WriteLine("Click");
        if (sender is GraphPanel)
        {
            var gp = (GraphPanel)sender;
            Console.WriteLine(gp.DesiredSize.Width);
            Console.WriteLine(gp.DesiredSize.Height);
            Console.WriteLine(gp.Bounds.Width);
            Console.WriteLine(gp.Bounds.Height);
            Console.WriteLine(gp.Width);
            Console.WriteLine(gp.Height);
        }
    }
    public event EventHandler? RefreshEvent;
}
