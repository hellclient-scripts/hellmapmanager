namespace TestProject;
using HellMapManager.Helpers;
using HellMapManager.Models;
using HellMapManager.States;
using HellMapManager.Windows.RelationMapWindow;


[Collection("MainState")]
public class RelationMapperTest
{
    [Fact]
    public void TestRelationMapper()
    {
        RelationMapItem? rm;
        RelationMapItem? rm1;
        Relation relation;
        var state = new AppState();
        var mf = MapFile.Create("testname", "testdesc");
        state.Current = mf;
        var room1 = new Room() { Key = "key1" };
        var room2 = new Room()
        {
            Key = "key2",
            Exits = [
                new Exit() { Command = "cmd", To = "key3" },
                new Exit() { Command = "cmd1", To = "key3" },
                new Exit() { Command = "cmd2", To = "key4" },
                new Exit() { Command = "cmd3", To = "" },
                new Exit() { Command = "cmd4", To = "notfound" },
                new Exit() { Command = "cmd5", To = "key2" }
            ]
        };
        var room3 = new Room() { Key = "key3", Exits = [new Exit() { Command = "cmd", To = "key4" }, new Exit() { Command = "cmd2", To = "key5" }] };
        var room4 = new Room() { Key = "key4", Exits = [new Exit() { Command = "cmd", To = "key3" }, new Exit() { Command = "cmd2", To = "key2" }] };
        var room5 = new Room() { Key = "key5" };

        state.APIInsertRooms([room1, room2, room3, room4, room5]);

        rm = RelationMapper.RelationMap(mf, "notfound", 1);
        Assert.Null(rm);
        rm = RelationMapper.RelationMap(mf, "key1", 1);
        Assert.NotNull(rm);
        Assert.Equal(0, rm.Depth);
        Assert.Equal(room1, rm.Room);
        Assert.Empty(rm.Relations);
        Assert.False(rm.HasRelation("notfound"));

        rm = RelationMapper.RelationMap(mf, "key2", 1);

        Assert.NotNull(rm);
        Assert.Equal(0, rm.Depth);
        Assert.Equal(room2, rm.Room);
        Assert.NotEmpty(rm.Relations);
        Assert.Equal(2, rm.Relations.Count);
        Assert.False(rm.HasRelation("notfound"));
        Assert.False(rm.HasRelation("key1"));
        Assert.True(rm.HasRelation("key3"));
        Assert.True(rm.HasRelation("key4"));

        relation = rm.Relations[0];
        Assert.Equal(RelationType.OneSideTo, relation.Type);
        rm1 = relation.Target;

        Assert.Equal(1, rm1.Depth);
        Assert.Equal(room3, rm1.Room);
        Assert.Empty(rm1.Relations);
        relation = rm.Relations[1];
        Assert.Equal(RelationType.TwoSide, relation.Type);
        rm1 = relation.Target;
        Assert.Equal(1, rm1.Depth);
        Assert.Equal(room4, rm1.Room);
        Assert.Empty(rm1.Relations);

        rm = RelationMapper.RelationMap(mf, "key2", 9);
        Assert.NotNull(rm);
        Assert.Equal(0, rm.Depth);
        Assert.Equal(room2, rm.Room);
        Assert.NotEmpty(rm.Relations);
        Assert.Equal(2, rm.Relations.Count);

        relation = rm.Relations[0];
        Assert.Equal(RelationType.OneSideTo, relation.Type);
        rm1 = relation.Target;

        Assert.Equal(1, rm1.Depth);
        Assert.Equal(room3, rm1.Room);
        Assert.Single(rm1.Relations);
        relation = rm1.Relations[0];
        Assert.Equal(RelationType.OneSideTo, relation.Type);
        rm1 = relation.Target;
        Assert.Equal(2, rm1.Depth);
        Assert.Equal(room5, rm1.Room);
        Assert.Empty(rm1.Relations);

        relation = rm.Relations[1];
        Assert.Equal(RelationType.TwoSide, relation.Type);
        rm1 = relation.Target;
        Assert.Equal(1, rm1.Depth);
        Assert.Equal(room4, rm1.Room);
        Assert.Empty(rm1.Relations);
    }
    [Fact]
    public void TestViewModel()
    {
        AppState.Main.CloseCurrent();
        AppState.Main.NewMap();
        AppState.Main.APIInsertRooms([
            new Room() {
                Key = "key1",
                 Exits = [
                    new Exit() {
                    Command = "cmd",
                    To="key2"
                    },
                ],
            },
            new Room() {
                Key = "key2",
                Name="name2",
                Group="group2",
                Desc="desc2",
                Tags=["tag1", "tag2"],
                Exits = [
                    new Exit() {
                    Command = "cmd",
                    To="key3" },
                    new Exit() {
                    Command = "cmd",
                    To="key1" },
                ],
            },
            new Room() {
                Key = "key3",
                Exits = [
                    new Exit() {
                    Command = "cmd",
                    To="key4" },
                    new Exit() {
                    Command = "cmd",
                    To="key1" },
                ],
            },
            new Room() {
                Key = "key4",
                Exits = [
                    new Exit() {
                    Command = "cmd",
                    To="key5" },
                ],
            },
            new Room() {
                Key = "key5",

            },
        ]);
        var vi = RelationMapper.RelationMap(AppState.Main.Current!, "key1", 9);
        Assert.NotNull(vi);
        var vm = new RelationMapWindowViewModel(vi);
        Assert.Equal("房间   (key1)   关系地图", vm.Title);
        var graph = vm.MyGraph;
        Assert.Equal(4, graph.Edges.Count);
        Assert.False(vm.HasHistory);
        foreach (var item in graph.Edges)
        {
            Assert.False(item is LonelyEdge);
        }
        var viewitems = new List<ViewItem>();
        graph.Edges.AsEnumerable().ToList().ForEach(x =>
        {
            var tail = (ViewItem)x.Tail;
            var head = (ViewItem)x.Head;
            if (viewitems.Contains(tail) == false)
            {
                viewitems.Add(tail);
            }
            if (viewitems.Contains(head) == false)
            {
                viewitems.Add(head);
            }
        });
        viewitems.Sort((x, y) => x.Item.Depth.CompareTo(y.Item.Depth));
        Assert.Equal(5, viewitems.Count);
        Assert.Equal("key1", viewitems[0].Item.Room.Key);
        Assert.Equal("key2", viewitems[1].Item.Room.Key);
        Assert.Equal("key3", viewitems[2].Item.Room.Key);
        Assert.Equal("key4", viewitems[3].Item.Room.Key);
        Assert.Equal("key5", viewitems[4].Item.Room.Key);
        Assert.True(viewitems[0].IsLevel0);
        Assert.False(viewitems[0].IsLevel1);
        Assert.False(viewitems[0].IsLevel2);
        Assert.False(viewitems[0].IsLevelOther);
        Assert.False(viewitems[1].IsLevel0);
        Assert.True(viewitems[1].IsLevel1);
        Assert.False(viewitems[1].IsLevel2);
        Assert.False(viewitems[1].IsLevelOther);
        Assert.False(viewitems[2].IsLevel0);
        Assert.False(viewitems[2].IsLevel1);
        Assert.True(viewitems[2].IsLevel2);
        Assert.False(viewitems[2].IsLevelOther);
        Assert.False(viewitems[3].IsLevel0);
        Assert.False(viewitems[3].IsLevel1);
        Assert.False(viewitems[3].IsLevel2);
        Assert.True(viewitems[3].IsLevelOther);
        Assert.False(viewitems[4].IsLevel0);
        Assert.False(viewitems[4].IsLevel1);
        Assert.False(viewitems[4].IsLevel2);
        Assert.True(viewitems[4].IsLevelOther);

        Assert.Equal("<无房间名>", viewitems[0].NameInfo);
        Assert.True(viewitems[0].IsNameEmpty);
        Assert.Equal("<无分组>", viewitems[0].GroupInfo);
        Assert.True(viewitems[0].IsGroupEmpty);
        Assert.Equal("<无描述>", viewitems[0].DescInfo);
        Assert.True(viewitems[0].IsDescEmpty);
        Assert.Equal("<无标签>", viewitems[0].TagsInfo);
        Assert.True(viewitems[0].IsTagsEmpty);
        Assert.Equal("name2", viewitems[1].NameInfo);
        Assert.False(viewitems[1].IsNameEmpty);
        Assert.Equal("group2", viewitems[1].GroupInfo);
        Assert.False(viewitems[1].IsGroupEmpty);
        Assert.Equal("desc2", viewitems[1].DescInfo);
        Assert.False(viewitems[1].IsDescEmpty);
        Assert.Equal("tag1,tag2", viewitems[1].TagsInfo);
        Assert.False(viewitems[1].IsTagsEmpty);
        Assert.Equal("(key1)", viewitems[0].LabelWithKey);
        Assert.Equal("name2(key2)/group2", viewitems[1].LabelWithKey);
        Assert.Empty(vm.Histories);
        Assert.Equal(viewitems[0].Item, vm.Item);
        vm.OnHistoryBack();
        Assert.Empty(vm.Histories);
        Assert.Equal(viewitems[0].Item, vm.Item);
        vm.EnterViewItem(vm);
        Assert.Empty(vm.Histories);
        Assert.Equal(viewitems[0].Item, vm.Item);
        vm.EnterRoomKey("");
        Assert.Empty(vm.Histories);
        Assert.Equal(viewitems[0].Item, vm.Item);
        vm.EnterRoomKey("notfound");
        Assert.Empty(vm.Histories);
        Assert.Equal(viewitems[0].Item, vm.Item);
        vm.EnterViewItem(viewitems[0]);
        Assert.Empty(vm.Histories);
        Assert.Equal(viewitems[0].Item, vm.Item);
        vm.EnterViewItem(viewitems[1]);
        Assert.Single(vm.Histories);
        Assert.Equal(viewitems[1].Item.Room.Key, vm.Item.Room.Key);
        vm.OnHistoryBack();
        Assert.Empty(vm.Histories);
        Assert.Equal(viewitems[0].Item.Room.Key, vm.Item.Room.Key);
        for (var i = 0; i < AppPreset.RelationMaxHistoriesCount; i++)
        {
            vm.EnterViewItem(viewitems[i % 2 + 1]);
        }
        vm.EnterViewItem(viewitems[3]);
        vm.EnterViewItem(viewitems[4]);
        Assert.Equal(AppPreset.RelationMaxHistoriesCount, vm.Histories.Count);
        Assert.NotEqual(viewitems[0].Item.Room.Key, vm.Item.Room.Key);
        Assert.Equal(viewitems[4].Item.Room.Key, vm.Item.Room.Key);
        var edges = vm.MyGraph.Edges;
        Assert.Single(edges);
        vm.MyGraph.Edges.AsEnumerable().ToList().ForEach(x =>
        {
            Assert.True(x is LonelyEdge);
        });
    }
}
