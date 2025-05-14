using System.Reflection;
using HellMapManager.Models;
using Xunit.Sdk;

namespace TestProject;

public class ModelTest
{
    [Fact]
    public void TestBase()
    {
        var data = new Data("key", "Value");
        Data data2;
        data2 = data.Clone();
        Assert.True(data.Equal(data2));
        Assert.True(data2.Validated());
        data2.Key = "";
        Assert.False(data.Equal(data2));
        Assert.False(data2.Validated());
        data2 = data.Clone();
        data2.Value = "";
        Assert.False(data.Equal(data2));
        Assert.False(data2.Validated());

        var con = new Condition("cond", true);
        Condition con2;
        con2 = con.Clone();
        Assert.True(con.Equal(con2));
        Assert.True(con2.Validated());
        con2 = con.Clone();
        con2.Key = "";
        Assert.False(con.Equal(con2));
        Assert.False(con2.Validated());
        con2 = con.Clone();
        con2.Not = false;
        Assert.False(con.Equal(con2));
        Assert.True(con2.Validated());

        var tc = new TypedConditions("key", ["1", "2"], true);
        TypedConditions tc2;
        tc2 = tc.Clone();
        Assert.True(tc.Equal(tc2));
        Assert.True(tc2.Validated());
        tc2 = tc.Clone();
        tc2.Key = "";
        Assert.False(tc.Equal(tc2));
        Assert.False(tc2.Validated());
        tc2 = tc.Clone();
        tc2.Conditions[0] = "0";
        Assert.False(tc.Equal(tc2));
        Assert.True(tc2.Validated());
        tc2 = tc.Clone();
        tc2.Conditions.Add("3");
        Assert.False(tc.Equal(tc2));
        Assert.True(tc2.Validated());

        tc2 = tc.Clone();
        tc2.Not = false;
        Assert.False(tc.Equal(tc2));
        Assert.True(tc2.Validated());

        var vt = new ValueTag("key", 0);
        ValueTag vt2;
        vt2 = vt.Clone();
        Assert.True(vt.Equal(vt2));
        Assert.True(vt2.Validated());
        Assert.Equal("key", vt2.ToString());
        vt2 = vt.Clone();
        vt2.Key = "";
        Assert.False(vt.Equal(vt2));
        Assert.False(vt2.Validated());
        Assert.Equal("", vt2.ToString());
        vt2 = vt.Clone();
        vt2.Value = -1;
        Assert.False(vt.Equal(vt2));
        Assert.True(vt2.Validated());
        Assert.Equal("key:-1", vt2.ToString());
        vt2 = vt.Clone();
        vt2.Value = 1;
        vt2.Key = "key2";
        Assert.False(vt.Equal(vt2));
        Assert.Equal("key2:1", vt2.ToString());

        Assert.True(vt.Match("key", 0));
        Assert.True(vt.Match("key", -1));
        Assert.False(vt.Match("key", 1));
        Assert.False(vt.Match("key2", 0));

        var vc = new ValueCondition("key", 0, true);
        ValueCondition vc2;
        vc2 = vc.Clone();
        Assert.True(vc.Equal(vc2));
        Assert.True(vc2.Validated());
        Assert.Equal("!key", vc2.ToString());
        Assert.Equal("key", vc2.KeyLabel);
        vc2 = vc.Clone();
        vc2.Key = "";
        Assert.False(vc.Equal(vc2));
        Assert.False(vc2.Validated());
        Assert.Equal("!", vc2.ToString());
        Assert.Equal("", vc2.KeyLabel);
        vc2 = vc.Clone();
        vc2.Value = -1;
        Assert.False(vc.Equal(vc2));
        Assert.True(vc2.Validated());
        Assert.Equal("!key:-1", vc2.ToString());
        Assert.Equal("key:-1", vc2.KeyLabel);
        vc2 = vc.Clone();
        vc2.Not = false;
        vc2.Value = 1;
        vc2.Key = "key2";
        Assert.False(vc.Equal(vc2));
        Assert.True(vc2.Validated());
        Assert.Equal("key2:1", vc2.ToString());
        Assert.Equal("key2:1", vc2.KeyLabel);

        List<ValueTag> tags = [
            new ("key1", 0),
            new ("key2", 5),
        ];
        Assert.True(ValueTag.HasTag(tags, "key1", 0));
        Assert.True(ValueTag.HasTag(tags, "key2", 5));
        Assert.False(ValueTag.HasTag(tags, "key1", 1));
        Assert.False(ValueTag.HasTag(tags, "key2", 6));

        Assert.True(ValueTag.ValidteConditions([], []));
        Assert.False(ValueTag.ValidteConditions([], [new("key1", 0, false)]));
        Assert.True(ValueTag.ValidteConditions(tags, []));
        Assert.True(ValueTag.ValidteConditions(tags, [new("key1", 0, false)]));
        Assert.False(ValueTag.ValidteConditions(tags, [new("key1", 0, true)]));
        Assert.True(ValueTag.ValidteConditions(tags, [new("key1", 0, false), new("key2", 0, false)]));
        Assert.True(ValueTag.ValidteConditions(tags, [new("key1", 0, false), new("key2", 10, true)]));
        Assert.False(ValueTag.ValidteConditions(tags, [new("key1", 0, false), new("key2", 10, false)]));
        Assert.False(ValueTag.ValidteConditions(tags, [new("key1", 0, false), new("key2", 4, true)]));
        Assert.False(ValueTag.ValidteConditions(tags, [new("key1", 0, true), new("key2", 0, false)]));
        Assert.True(ValueTag.ValidteConditions(tags, [new("key1", 0, false), new("key3", 0, true)]));
        Assert.False(ValueTag.ValidteConditions(tags, [new("key1", 0, false), new("key3", 0, false)]));
    }
    [Fact]
    public void TestExit()
    {
        var exit = new Exit()
        {
            To = "to",
            Command = "cmd",
            Cost = 2,
            Conditions = [new ValueCondition("con1", 0, false), new ValueCondition("con2", 0, true)]
        };
        Exit exit2;
        exit2 = exit.Clone();
        Assert.True(exit.Equal(exit2));
        Assert.True(exit2.Validated());
        exit2 = exit.Clone();
        exit2.Command = "";
        Assert.False(exit.Equal(exit2));
        Assert.False(exit2.Validated());
        exit2 = exit.Clone();
        exit2.To = "";
        Assert.False(exit.Equal(exit2));
        Assert.True(exit2.Validated());
        exit2 = exit.Clone();
        exit2.Cost = -1;
        Assert.False(exit.Equal(exit2));
        Assert.True(exit2.Validated());
        exit2 = exit.Clone();
        exit2.Conditions[0].Key = "wrongkey";
        Assert.False(exit.Equal(exit2));
        Assert.True(exit2.Validated());
        exit2 = exit.Clone();
        exit2.Conditions.Add(new("con3", 0, true));
        Assert.False(exit.Equal(exit2));
        Assert.True(exit2.Validated());
        exit2 = exit.Clone();
        Assert.True(exit2.HasCondition);
        Assert.Equal("con1,! con2", exit2.AllConditions);
        exit2 = exit.Clone();
        exit2.Conditions = [];
        Assert.False(exit2.HasCondition);
        Assert.Equal("", exit2.AllConditions);

        List<ExitLabel> labels;
        exit2 = exit.Clone();
        labels = exit2.Labels;
        Assert.Equal(5, labels.Count);
        Assert.Equal(ExitLabel.Types.Command, labels[0].Type);
        Assert.Equal("cmd", labels[0].Value);
        Assert.True(labels[0].IsCommand);
        Assert.False(labels[0].IsTo);
        Assert.False(labels[0].IsCondition);
        Assert.False(labels[0].IsExCondition);
        Assert.False(labels[0].IsCost);
        Assert.Equal(ExitLabel.Types.To, labels[1].Type);
        Assert.Equal("to", labels[1].Value);
        Assert.False(labels[1].IsCommand);
        Assert.True(labels[1].IsTo);
        Assert.False(labels[1].IsCondition);
        Assert.False(labels[1].IsExCondition);
        Assert.False(labels[1].IsCost);
        Assert.Equal(ExitLabel.Types.Condition, labels[2].Type);
        Assert.Equal("con1", labels[2].Value);
        Assert.False(labels[2].IsCommand);
        Assert.False(labels[2].IsTo);
        Assert.True(labels[2].IsCondition);
        Assert.False(labels[2].IsExCondition);
        Assert.False(labels[2].IsCost);
        Assert.Equal(ExitLabel.Types.ExCondition, labels[3].Type);
        Assert.Equal("con2", labels[3].Value);
        Assert.False(labels[3].IsCommand);
        Assert.False(labels[3].IsTo);
        Assert.False(labels[3].IsCondition);
        Assert.True(labels[3].IsExCondition);
        Assert.False(labels[3].IsCost);
        Assert.Equal(ExitLabel.Types.Cost, labels[4].Type);
        Assert.Equal("2", labels[4].Value);
        Assert.False(labels[4].IsCommand);
        Assert.False(labels[4].IsTo);
        Assert.False(labels[4].IsCondition);
        Assert.False(labels[4].IsExCondition);
        Assert.True(labels[4].IsCost);
        exit2 = new Exit();
        exit2.Command = "cmd";
        exit2.To = "to";
        Assert.Equal(1, exit2.Cost);
        labels = exit2.Labels;
        Assert.Equal(2, labels.Count);
        Assert.Equal(ExitLabel.Types.Command, labels[0].Type);
        Assert.Equal("cmd", labels[0].Value);
        Assert.Equal(ExitLabel.Types.To, labels[1].Type);
        Assert.Equal("to", labels[1].Value);
    }
    [Fact]
    public void TestRoom()
    {
        var room = new Room()
        {
            Key = "key",
            Name = "name",
            Group = "group",
            Desc = "desc",
            Tags = [new ValueTag("tag1", 0), new ValueTag("tag2", 0)],
            Data = [new Data("dkey1", "dval1"), new Data("dkey2", "dval2")],
            Exits = [
                new Exit(){
                Command="command1",
                To="to1",
                Cost=2,
                Conditions=[
                    new ValueCondition("con1",0,true),
                    new ValueCondition("con2",0,false),
                ]
            },
               new Exit(){
                Command="command2",
                To="to2",
                Cost=4,

            }
            ]
        };

        Assert.True(room.HasTag("tag1", 0));
        Assert.True(room.HasTag("tag2", 0));
        Assert.False(room.HasTag("notexists", 0));
        Room room2;

        Assert.False(room.Filter("unknow"));
        Assert.True(room.Filter("key"));
        Assert.True(room.Filter("name"));
        Assert.True(room.Filter("group"));
        Assert.True(room.Filter("tag1"));
        Assert.True(room.Filter("dkey1"));
        Assert.True(room.Filter("dval2"));
        Assert.True(room.Filter("command1"));
        Assert.True(room.Filter("to2"));

        Assert.Equal(2, room.ExitsCount);
        Assert.Equal("tag1,tag2", room.AllTags);
        Assert.False(room.HasExitTo("to999"));
        Assert.True(room.HasExitTo("to1"));
        Assert.True(room.HasExitTo("to2"));

        room2 = room.Clone();
        Assert.True(room.Equal(room2));
        Assert.True(room2.Validated());

        room2 = room.Clone();
        room2.Key = "";
        Assert.False(room2.Validated());
        Assert.False(room.Equal(room2));

        room2 = room.Clone();
        room2.Desc = "";
        Assert.False(room.Equal(room2));
        Assert.True(room2.Validated());

        room2 = room.Clone();
        room2.Group = "";
        Assert.False(room.Equal(room2));
        Assert.True(room2.Validated());

        room2 = room.Clone();
        room2.Desc = "";
        Assert.False(room.Equal(room2));
        Assert.True(room2.Validated());

        room2 = room.Clone();
        room2.Tags[1] = new ValueTag("", 0);
        Assert.False(room.Equal(room2));
        Assert.True(room2.Validated());

        room2 = room.Clone();
        room2.Tags.Add(new ValueTag("", 0));
        Assert.False(room.Equal(room2));
        Assert.True(room2.Validated());

        room2 = room.Clone();
        room2.Exits[0].To = "";
        Assert.False(room.Equal(room2));
        Assert.True(room2.Validated());

        room2 = room.Clone();
        room2.Exits.Add(new Exit());
        Assert.False(room.Equal(room2));
        Assert.True(room2.Validated());

        room2 = room.Clone();
        room2.Data[0].Value = "";
        Assert.False(room.Equal(room2));
        Assert.True(room2.Validated());

        room2 = room.Clone();
        room2.Data.Add(new Data("", ""));
        Assert.False(room.Equal(room2));
        Assert.True(room2.Validated());

        var room3 = new Room();
        Assert.Equal(-1, room3.NumberID);
        room3.Key = "1";
        Assert.Equal(1, room3.NumberID);
        room3.Key = " 2";
        Assert.Equal(2, room3.NumberID);
        room3.Key = "-1";
        Assert.Equal(-1, room3.NumberID);
        room3.Key = "abc";
        Assert.Equal(-1, room3.NumberID);
    }
    [Fact]
    public void TestMarker()
    {
        var marker = new Marker()
        {
            Key = "key1",
            Value = "value1",
            Group = "group1",
            Desc = "desc1",
            Message = "message1",

        };

        Assert.True(marker.Filter("key"));
        Assert.True(marker.Filter("value"));
        Assert.True(marker.Filter("group"));
        Assert.True(marker.Filter("desc"));
        Assert.True(marker.Filter("message"));
        Assert.False(marker.Filter("notfound"));
        Marker marker2;
        marker2 = marker.Clone();
        Assert.True(marker2.Validated());
        marker2.Key = "";
        Assert.False(marker2.Validated());
        Assert.False(marker.Equal(marker2));
        marker2 = marker.Clone();
        marker2.Value = "";
        Assert.False(marker2.Validated());
        Assert.False(marker.Equal(marker2));
        marker2 = marker.Clone();
        Assert.True(marker.Equal(marker2));
        marker2 = marker.Clone();
        marker2.Group = "";
        Assert.True(marker2.Validated());
        Assert.False(marker.Equal(marker2));

        marker2 = marker.Clone();
        marker2.Desc = "";
        Assert.True(marker2.Validated());
        Assert.False(marker.Equal(marker2));

        marker2 = marker.Clone();
        marker2.Message = "";
        Assert.True(marker2.Validated());
        Assert.False(marker.Equal(marker2));

    }
    [Fact]
    public void TestRoute()
    {
        var route = new Route()
        {
            Key = "key1",
            Rooms = ["rid1a", "rid1b"],
            Desc = "desc1",
            Group = "group1",
            Message = "message1",
        };
        Assert.True(route.Filter("key"));
        Assert.True(route.Filter("rid1"));
        Assert.True(route.Filter("desc"));
        Assert.True(route.Filter("group"));
        Assert.True(route.Filter("message"));
        Assert.False(route.Filter("NotFound"));

        Assert.Equal("rid1a;rid1b", route.AllRooms);
        Assert.Equal(2, route.RoomsCount);
        Route route2;
        route2 = route.Clone();
        Assert.True(route2.Validated());
        Assert.True(route.Equal(route2));
        route2.Key = "";
        Assert.False(route2.Validated());
        Assert.False(route.Equal(route2));
        route2 = route.Clone();
        route2.Rooms[0] = "rid0";
        Assert.True(route2.Validated());
        Assert.False(route.Equal(route2));
        route2 = route.Clone();
        route2.Rooms.Add("rid3");
        Assert.True(route2.Validated());
        Assert.False(route.Equal(route2));
        route2 = route.Clone();
        route2.Group = "";
        Assert.True(route2.Validated());
        Assert.False(route.Equal(route2));
        route2 = route.Clone();
        route2.Desc = "";
        Assert.True(route2.Validated());
        Assert.False(route.Equal(route2));
        route2 = route.Clone();
        route2.Message = "";
        Assert.True(route2.Validated());
        Assert.False(route.Equal(route2));
    }
    [Fact]
    public void TestTrace()
    {
        var trace = new Trace()
        {
            Key = "key1",
            Locations = ["rid1", "rid2"],
            Group = "group1",
            Desc = "desc1",
            Message = "message1",
        };
        Assert.True(trace.Filter("key"));
        Assert.True(trace.Filter("rid"));
        Assert.True(trace.Filter("desc"));
        Assert.True(trace.Filter("group"));
        Assert.True(trace.Filter("message"));
        Assert.False(trace.Filter("NotFound"));

        Assert.Equal(2, trace.LocationsCount);

        Trace trace2;
        trace2 = trace.Clone();
        Assert.True(trace.Equal(trace2));
        Assert.True(trace2.Validated());

        trace2.Key = "";
        Assert.False(trace.Equal(trace2));
        Assert.False(trace2.Validated());

        trace2 = trace.Clone();
        trace2.Locations[0] = "rid0";
        Assert.False(trace.Equal(trace2));
        Assert.True(trace2.Validated());

        trace2 = trace.Clone();
        trace2.Locations.Add("rid3");
        Assert.False(trace.Equal(trace2));
        Assert.True(trace2.Validated());

        trace2 = trace.Clone();
        trace2.Group = "";
        Assert.False(trace.Equal(trace2));
        Assert.True(trace2.Validated());

        trace2 = trace.Clone();
        trace2.Desc = "";
        Assert.False(trace.Equal(trace2));
        Assert.True(trace2.Validated());

        trace2 = trace.Clone();
        trace2.Message = "";
        Assert.False(trace.Equal(trace2));
        Assert.True(trace2.Validated());
    }
    [Fact]
    public void TestRegion()
    {
        var region = new Region()
        {
            Key = "key1",
            Group = "group1",
            Desc = "desc1",
            Message = "message1",
            Items = [new RegionItem(RegionItemType.Room, "room1", false), new RegionItem(RegionItemType.Zone, "zone1", true)]
        };
        Assert.True(region.Filter("key"));
        Assert.True(region.Filter("room"));
        Assert.True(region.Filter("zone"));
        Assert.True(region.Filter("desc"));
        Assert.True(region.Filter("group"));
        Assert.True(region.Filter("message"));
        Assert.False(region.Filter("NotFound"));

        Assert.Equal(2, region.ItemsCount);
        Region region2;
        region2 = region.Clone();
        Assert.True(region.Equal(region2));
        Assert.True(region2.Validated());
        region2.Key = "";
        Assert.False(region.Equal(region2));
        Assert.False(region2.Validated());
        region2 = region.Clone();
        region2.Items[0].Value = "room0";
        Assert.False(region.Equal(region2));
        Assert.True(region2.Validated());
        region2 = region.Clone();
        region2.Items.Add(new RegionItem(RegionItemType.Room, "room3", false));
        Assert.False(region.Equal(region2));
        Assert.True(region2.Validated());
        region2 = region.Clone();
        region2.Group = "";
        Assert.False(region.Equal(region2));
        Assert.True(region2.Validated());
        region2 = region.Clone();
        region2.Desc = "";
        Assert.False(region.Equal(region2));
        Assert.True(region2.Validated());
        region2 = region.Clone();
        region2.Message = "";
        Assert.False(region.Equal(region2));
        Assert.True(region2.Validated());
    }
    [Fact]
    public void TestRegionItem()
    {
        var ri = new RegionItem(RegionItemType.Room, "room1", false);
        RegionItem ri2;
        Assert.True(ri.Validated());
        Assert.True(ri.IsRoom);
        Assert.Equal("+", ri.ExcludeLabel);
        Assert.Equal("加入房间 room1", ri.Label);
        ri2 = ri.Clone();
        Assert.True(ri.Equal(ri2));
        Assert.True(ri2.Validated());
        ri2 = ri.Clone();
        ri2.Value = "";
        Assert.False(ri.Equal(ri2));
        Assert.False(ri2.Validated());
        ri2 = ri.Clone();
        ri2.Type = RegionItemType.Zone;
        Assert.False(ri.Equal(ri2));
        Assert.True(ri2.Validated());
        Assert.False(ri2.IsRoom);
        Assert.Equal("+", ri2.ExcludeLabel);
        Assert.Equal("加入区域 room1", ri2.Label);
        ri2 = ri.Clone();
        ri2.Not = true;
        Assert.False(ri.Equal(ri2));
        Assert.True(ri2.Validated());
        Assert.Equal("-", ri2.ExcludeLabel);
        Assert.Equal("排除房间 room1", ri2.Label);
        ri2 = ri.Clone();
        ri2.Not = true;
        ri2.Type = RegionItemType.Zone;
        Assert.False(ri.Equal(ri2));
        Assert.True(ri2.Validated());
        Assert.Equal("-", ri2.ExcludeLabel);
        Assert.Equal("排除区域 room1", ri2.Label);
    }
    [Fact]
    public void TestLandmark()
    {
        var lm = new Landmark()
        {
            Key = "key1",
            Type = "type1",
            Value = "value1",
            Group = "group1",
            Desc = "desc1"
        };
        Assert.True(lm.Filter("key"));
        Assert.True(lm.Filter("type"));
        Assert.True(lm.Filter("value"));
        Assert.True(lm.Filter("group"));
        Assert.True(lm.Filter("desc"));
        Assert.False(lm.Filter("NotFound"));

        Landmark lm2;
        lm2 = lm.Clone();
        Assert.True(lm.Equal(lm2));
        Assert.True(lm2.Validated());
        Assert.Equal(lm.UniqueKey().ToString(), lm2.UniqueKey().ToString());
        lm2.Key = "";
        Assert.False(lm.Equal(lm2));
        Assert.False(lm2.Validated());
        Assert.NotEqual(lm.UniqueKey, lm2.UniqueKey);
        lm2 = lm.Clone();
        lm2.Type = "";
        Assert.False(lm.Equal(lm2));
        Assert.True(lm2.Validated());
        Assert.NotEqual(lm.UniqueKey, lm2.UniqueKey);
        lm2 = lm.Clone();
        lm2.Value = "";
        Assert.False(lm.Equal(lm2));
        Assert.True(lm2.Validated());
        Assert.Equal(lm.UniqueKey().ToString(), lm2.UniqueKey().ToString());
        lm2 = lm.Clone();
        lm2.Group = "";
        Assert.False(lm.Equal(lm2));
        Assert.True(lm2.Validated());
        Assert.Equal(lm.UniqueKey().ToString(), lm2.UniqueKey().ToString());
        lm2 = lm.Clone();
        lm2.Desc = "";
        Assert.False(lm.Equal(lm2));
        Assert.True(lm2.Validated());
        Assert.Equal(lm.UniqueKey().ToString(), lm2.UniqueKey().ToString());
    }
    [Fact]
    public void TestShortcut()
    {
        var sc = new Shortcut()
        {
            Key = "key1",
            Group = "group1",
            Desc = "desc1",
            Command = "command1",
            To = "to1",
            Cost = 2,
            RoomConditions = [new ValueCondition("con1", 0, false), new ValueCondition("con2", 0, true)],
            Conditions = [new ValueCondition("con3", 0, false), new ValueCondition("con4", 0, true)]
        };
        Assert.True(sc.Filter("key"));
        Assert.True(sc.Filter("command"));
        Assert.True(sc.Filter("to"));
        Assert.True(sc.Filter("group"));
        Assert.True(sc.Filter("desc"));
        Assert.False(sc.Filter("con"));
        Assert.False(sc.Filter("NotFound"));
        Shortcut sc2;
        sc2 = sc.Clone();
        Assert.True(sc.Equal(sc2));
        Assert.True(sc2.Validated());

        sc2.Key = "";
        Assert.False(sc.Equal(sc2));
        Assert.False(sc2.Validated());

        sc2 = sc.Clone();
        sc2.Command = "";
        Assert.False(sc.Equal(sc2));
        Assert.False(sc2.Validated());
        sc2 = sc.Clone();
        sc2.To = "";
        Assert.False(sc.Equal(sc2));
        Assert.True(sc2.Validated());
        sc2 = sc.Clone();
        sc2.Cost = -1;
        Assert.False(sc.Equal(sc2));
        Assert.True(sc2.Validated());
        sc2 = sc.Clone();
        sc2.Conditions[0].Key = "wrongkey";
        Assert.False(sc.Equal(sc2));
        Assert.True(sc2.Validated());
        sc2 = sc.Clone();
        sc2.Conditions.Add(new("con5", 0, true));
        Assert.False(sc.Equal(sc2));
        Assert.True(sc2.Validated());
        sc2 = sc.Clone();
        Assert.True(sc2.HasCondition);
        Assert.Equal("con3,! con4", sc2.AllConditions);
        sc2 = sc.Clone();
        sc2.Conditions = [];
        Assert.False(sc2.HasCondition);
        Assert.Equal("", sc2.AllConditions);
        sc2 = sc.Clone();
        sc2.RoomConditions[0].Key = "wrongkey";
        Assert.False(sc.Equal(sc2));
        Assert.True(sc2.Validated());
        sc2 = sc.Clone();
        sc2.RoomConditions.Add(new("con5", 0, true));
        Assert.False(sc.Equal(sc2));
        Assert.True(sc2.Validated());
    }
    [Fact]
    public void TestVariable()
    {
        var var1 = new Variable()
        {
            Key = "key1",
            Value = "value1",
            Group = "group1",
            Desc = "desc1"
        };
        Assert.True(var1.Filter("key"));
        Assert.True(var1.Filter("value"));
        Assert.True(var1.Filter("group"));
        Assert.True(var1.Filter("desc"));
        Assert.False(var1.Filter("NotFound"));

        Variable var2;
        var2 = var1.Clone();
        Assert.True(var1.Equal(var2));
        Assert.True(var2.Validated());
        var2.Key = "";
        Assert.False(var1.Equal(var2));
        Assert.False(var2.Validated());
        var2 = var1.Clone();
        var2.Value = "";
        Assert.False(var1.Equal(var2));
        Assert.True(var2.Validated());
        var2 = var1.Clone();
        var2.Group = "";
        Assert.False(var1.Equal(var2));
        Assert.True(var2.Validated());
        var2 = var1.Clone();
        var2.Desc = "";
        Assert.False(var1.Equal(var2));
        Assert.True(var2.Validated());
    }
    [Fact]
    public void TestSnapshot()
    {
        var snapshot = new Snapshot()
        {
            Key = "key1",
            Type = "type1",
            Value = "value1",
            Group = "group1",
            Timestamp = 1234567890
        };
        Assert.True(snapshot.Filter("key"));
        Assert.True(snapshot.Filter("type"));
        Assert.True(snapshot.Filter("value"));
        Assert.True(snapshot.Filter("group"));
        Assert.False(snapshot.Filter("NotFound"));
        Assert.True(snapshot.Validated());

        Snapshot snapshot2;
        snapshot2 = snapshot.Clone();
        Assert.True(snapshot.Equal(snapshot2));
        Assert.True(snapshot2.Validated());
        snapshot2.Key = "";
        Assert.False(snapshot.Equal(snapshot2));
        Assert.False(snapshot2.Validated());
        snapshot2 = snapshot.Clone();
        snapshot2.Type = "";
        Assert.False(snapshot.Equal(snapshot2));
        Assert.True(snapshot2.Validated());
        snapshot2 = snapshot.Clone();
        snapshot2.Value = "";
        Assert.False(snapshot.Equal(snapshot2));
        Assert.True(snapshot2.Validated());
        snapshot2 = snapshot.Clone();
        snapshot2.Group = "";
        Assert.False(snapshot.Equal(snapshot2));
        Assert.True(snapshot2.Validated());
        snapshot2 = Snapshot.Create("key1", "type1", "value1", "group1");
        Assert.True(snapshot2.Validated());
        Assert.Equal("key1", snapshot2.Key);
        Assert.Equal("type1", snapshot2.Type);
        Assert.Equal("value1", snapshot2.Value);
        Assert.Equal("group1", snapshot2.Group);
        Assert.True(snapshot2.Timestamp > 0);

        snapshot2 = new Snapshot()
        {
            Timestamp = 123456789,
        };
        var timelabel = DateTimeOffset.FromUnixTimeSeconds(123456789).LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        Assert.Equal(timelabel, snapshot2.TimeLabel);

        Assert.Equal(1, snapshot2.Count);
        snapshot2.Repeat();
        Assert.Equal(2, snapshot2.Count);
    }
    [Fact]
    public void TestMap()
    {
        var map = new Map()
        {

        };
        Assert.Equal(MapEncoding.Default, map.Encoding);
        map = Map.Create("name", "desc");
        Assert.Equal("name", map.Info.Name);
        Assert.Equal("desc", map.Info.Desc);
    }
    [Fact]
    public void TestMapInfo()
    {
        var mapInfo = new MapInfo()
        {
            UpdatedTime = -1,
        };
        Assert.False(mapInfo.Validated());
        mapInfo = new MapInfo()
        {
            Name = "name",
            UpdatedTime = 0,
            Desc = "desc"
        };
        Assert.True(mapInfo.Validated());
        Assert.Equal("name", mapInfo.Name);
        Assert.Equal(0, mapInfo.UpdatedTime);
        Assert.Equal("desc", mapInfo.Desc);
        Assert.Equal("name", mapInfo.NameLabel);
        Assert.Equal("desc", mapInfo.DescLabel);
        mapInfo = MapInfo.Create("name", "desc");
        Assert.Equal("name", mapInfo.Name);
        Assert.True(mapInfo.UpdatedTime > 0);
        Assert.Equal("desc", mapInfo.Desc);
        Assert.Equal("name", mapInfo.NameLabel);
        Assert.Equal("desc", mapInfo.DescLabel);
        mapInfo = MapInfo.Create("", "");
        Assert.Equal("", mapInfo.Name);
        Assert.True(mapInfo.UpdatedTime > 0);
        Assert.Equal("", mapInfo.Desc);
        Assert.Equal("<未命名>", mapInfo.NameLabel);
        Assert.Equal("<无描述>", mapInfo.DescLabel);
        mapInfo = MapInfo.Create("name", "Desc");
        MapInfo mapInfo2;
        mapInfo2 = mapInfo.Clone();
        Assert.True(mapInfo.Equal(mapInfo2));
        Assert.True(mapInfo2.Validated());
        mapInfo2 = mapInfo.Clone();
        mapInfo2.Name = "";
        Assert.False(mapInfo.Equal(mapInfo2));
        Assert.True(mapInfo2.Validated());
        mapInfo2 = mapInfo.Clone();
        mapInfo2.UpdatedTime = -1;
        Assert.False(mapInfo.Equal(mapInfo2));
        Assert.False(mapInfo2.Validated());
        mapInfo2 = mapInfo.Clone();
        mapInfo2.Desc = "";
        Assert.False(mapInfo.Equal(mapInfo2));
        Assert.True(mapInfo2.Validated());
    }
    [Fact]
    public void TestMapFile()
    {
        var mf = new MapFile();
        MapSettings settings;
        Assert.True(mf.Modified);
        Assert.Equal("", mf.Path);
        Assert.Equal(MapEncoding.Default, mf.Map.Encoding);
        Assert.Equal("", mf.Map.Info.Name);
        Assert.Equal("", mf.Map.Info.Desc);
        settings = mf.ToSettings();
        Assert.Equal(MapEncoding.Default, settings.Encoding);
        Assert.Equal("", settings.Name);
        Assert.Equal("", settings.Desc);
        mf = MapFile.Create("name", "desc");
        Assert.True(mf.Modified);
        Assert.Equal("", mf.Path);
        Assert.Equal("name", mf.Map.Info.Name);
        Assert.Equal("desc", mf.Map.Info.Desc);
        settings = mf.ToSettings();
        Assert.Equal(MapEncoding.Default, settings.Encoding);
        Assert.Equal("name", settings.Name);
        Assert.Equal("desc", settings.Desc);
        mf.Map.Encoding = MapEncoding.GB18030;
        settings = mf.ToSettings();
        Assert.Equal(MapEncoding.GB18030, settings.Encoding);
        Assert.Equal("name", settings.Name);
        Assert.Equal("desc", settings.Desc);
        Assert.Empty(mf.Map.Rooms);
        Assert.Empty(mf.Map.Markers);
        Assert.Empty(mf.Map.Routes);
        Assert.Empty(mf.Map.Traces);
        Assert.Empty(mf.Map.Regions);
        Assert.Empty(mf.Map.Landmarks);
        Assert.Empty(mf.Map.Shortcuts);
        Assert.Empty(mf.Map.Variables);
        Assert.Empty(mf.Map.Snapshots);

        mf.RemoveRoom("notfound");
        Assert.Empty(mf.Map.Rooms);
        var room = new Room()
        {
            Key = "key1",
        };
        mf.InsertRoom(room);
        Assert.Single(mf.Map.Rooms);
        Assert.Equal(room, mf.Map.Rooms[0]);
        var room2 = new Room()
        {
            Key = "key1",
        };
        mf.InsertRoom(room2);
        Assert.Single(mf.Map.Rooms);
        Assert.Equal(room2, mf.Map.Rooms[0]);
        var room3 = new Room()
        {
            Key = "key2",
        };
        mf.InsertRoom(room3);
        Assert.Equal(2, mf.Map.Rooms.Count);
        Assert.Equal(room2, mf.Map.Rooms[0]);
        Assert.Equal(room3, mf.Map.Rooms[1]);
        mf.RemoveRoom("key1");
        Assert.Single(mf.Map.Rooms);
        Assert.Equal(room3, mf.Map.Rooms[0]);

        mf.RemoveMarker("notfound");
        Assert.Empty(mf.Map.Markers);
        var marker = new Marker()
        {
            Key = "key1",
        };
        mf.InsertMarker(marker);
        Assert.Single(mf.Map.Markers);
        Assert.Equal(marker, mf.Map.Markers[0]);
        var marker2 = new Marker()
        {
            Key = "key1",
        };
        mf.InsertMarker(marker2);
        Assert.Single(mf.Map.Markers);
        Assert.Equal(marker2, mf.Map.Markers[0]);
        var marker3 = new Marker()
        {
            Key = "key2",
        };
        mf.InsertMarker(marker3);
        Assert.Equal(2, mf.Map.Markers.Count);
        Assert.Equal(marker2, mf.Map.Markers[0]);
        Assert.Equal(marker3, mf.Map.Markers[1]);
        mf.RemoveMarker("key1");
        Assert.Single(mf.Map.Markers);
        Assert.Equal(marker3, mf.Map.Markers[0]);
        mf.RemoveRoute("notfound");
        Assert.Empty(mf.Map.Routes);
        var route = new Route()
        {
            Key = "key1",
        };
        mf.InsertRoute(route);
        Assert.Single(mf.Map.Routes);
        Assert.Equal(route, mf.Map.Routes[0]);
        var route2 = new Route()
        {
            Key = "key1",
        };
        mf.InsertRoute(route2);
        Assert.Single(mf.Map.Routes);
        Assert.Equal(route2, mf.Map.Routes[0]);
        var route3 = new Route()
        {
            Key = "key2",
        };
        mf.InsertRoute(route3);
        Assert.Equal(2, mf.Map.Routes.Count);
        Assert.Equal(route2, mf.Map.Routes[0]);
        Assert.Equal(route3, mf.Map.Routes[1]);
        mf.RemoveRoute("key1");
        Assert.Single(mf.Map.Routes);
        Assert.Equal(route3, mf.Map.Routes[0]);

        mf.RemoveTrace("notfound");
        Assert.Empty(mf.Map.Traces);
        var trace = new Trace()
        {
            Key = "key1",
        };
        mf.InsertTrace(trace);
        Assert.Single(mf.Map.Traces);
        Assert.Equal(trace, mf.Map.Traces[0]);
        var trace2 = new Trace()
        {
            Key = "key1",
        };
        mf.InsertTrace(trace2);
        Assert.Single(mf.Map.Traces);
        Assert.Equal(trace2, mf.Map.Traces[0]);
        var trace3 = new Trace()
        {
            Key = "key2",
        };
        mf.InsertTrace(trace3);
        Assert.Equal(2, mf.Map.Traces.Count);
        Assert.Equal(trace2, mf.Map.Traces[0]);
        Assert.Equal(trace3, mf.Map.Traces[1]);
        mf.RemoveTrace("key1");
        Assert.Single(mf.Map.Traces);
        Assert.Equal(trace3, mf.Map.Traces[0]);
        mf.RemoveRegion("notfound");
        Assert.Empty(mf.Map.Regions);
        var region = new Region()
        {
            Key = "key1",
        };
        mf.InsertRegion(region);
        Assert.Single(mf.Map.Regions);
        Assert.Equal(region, mf.Map.Regions[0]);
        var region2 = new Region()
        {
            Key = "key1",
        };
        mf.InsertRegion(region2);
        Assert.Single(mf.Map.Regions);
        Assert.Equal(region2, mf.Map.Regions[0]);
        var region3 = new Region()
        {
            Key = "key2",
        };
        mf.InsertRegion(region3);
        Assert.Equal(2, mf.Map.Regions.Count);
        Assert.Equal(region2, mf.Map.Regions[0]);
        Assert.Equal(region3, mf.Map.Regions[1]);
        mf.RemoveRegion("key1");
        Assert.Single(mf.Map.Regions);
        Assert.Equal(region3, mf.Map.Regions[0]);
        mf.RemoveLandmark(new LandmarkKey("notfound", ""));
        Assert.Empty(mf.Map.Landmarks);
        var landmark = new Landmark()
        {
            Key = "key1",
        };
        mf.InsertLandmark(landmark);
        Assert.Single(mf.Map.Landmarks);
        Assert.Equal(landmark, mf.Map.Landmarks[0]);
        var landmark2 = new Landmark()
        {
            Key = "key1",
        };
        mf.InsertLandmark(landmark2);
        Assert.Single(mf.Map.Landmarks);
        Assert.Equal(landmark2, mf.Map.Landmarks[0]);
        var landmark3 = new Landmark()
        {
            Key = "key2",
        };
        mf.InsertLandmark(landmark3);
        Assert.Equal(2, mf.Map.Landmarks.Count);
        Assert.Equal(landmark2, mf.Map.Landmarks[0]);
        Assert.Equal(landmark3, mf.Map.Landmarks[1]);
        mf.RemoveLandmark(new LandmarkKey("key1", ""));
        Assert.Single(mf.Map.Landmarks);
        Assert.Equal(landmark3, mf.Map.Landmarks[0]);
        mf.RemoveShortcut("notfound");
        Assert.Empty(mf.Map.Shortcuts);
        var shortcut = new Shortcut()
        {
            Key = "key1",
        };
        mf.InsertShortcut(shortcut);
        Assert.Single(mf.Map.Shortcuts);
        Assert.Equal(shortcut, mf.Map.Shortcuts[0]);
        var shortcut2 = new Shortcut()
        {
            Key = "key1",
        };
        mf.InsertShortcut(shortcut2);
        Assert.Single(mf.Map.Shortcuts);
        Assert.Equal(shortcut2, mf.Map.Shortcuts[0]);
        var shortcut3 = new Shortcut()
        {
            Key = "key2",
        };
        mf.InsertShortcut(shortcut3);
        Assert.Equal(2, mf.Map.Shortcuts.Count);
        Assert.Equal(shortcut2, mf.Map.Shortcuts[0]);
        Assert.Equal(shortcut3, mf.Map.Shortcuts[1]);
        mf.RemoveShortcut("key1");
        Assert.Single(mf.Map.Shortcuts);
        Assert.Equal(shortcut3, mf.Map.Shortcuts[0]);
        mf.RemoveVariable("notfound");
        Assert.Empty(mf.Map.Variables);
        var variable = new Variable()
        {
            Key = "key1",
        };
        mf.InsertVariable(variable);
        Assert.Single(mf.Map.Variables);
        Assert.Equal(variable, mf.Map.Variables[0]);
        var variable2 = new Variable()
        {
            Key = "key1",
        };
        mf.InsertVariable(variable2);
        Assert.Single(mf.Map.Variables);
        Assert.Equal(variable2, mf.Map.Variables[0]);
        var variable3 = new Variable()
        {
            Key = "key2",
        };
        mf.InsertVariable(variable3);
        Assert.Equal(2, mf.Map.Variables.Count);
        Assert.Equal(variable2, mf.Map.Variables[0]);
        Assert.Equal(variable3, mf.Map.Variables[1]);
        mf.RemoveVariable("key1");
        Assert.Single(mf.Map.Variables);
        Assert.Equal(variable3, mf.Map.Variables[0]);
        mf.RemoveSnapshot(new SnapshotKey("notfound", "", ""));
        Assert.Empty(mf.Map.Snapshots);
        var snapshot = new Snapshot()
        {
            Key = "key1",
        };
        mf.InsertSnapshot(snapshot);
        Assert.Single(mf.Map.Snapshots);
        Assert.Equal(snapshot, mf.Map.Snapshots[0]);
        var snapshot2 = new Snapshot()
        {
            Key = "key1",
        };
        mf.InsertSnapshot(snapshot2);
        Assert.Single(mf.Map.Snapshots);
        Assert.Equal(snapshot2, mf.Map.Snapshots[0]);
        var snapshot3 = new Snapshot()
        {
            Key = "key2",
        };
        mf.InsertSnapshot(snapshot3);
        Assert.Equal(2, mf.Map.Snapshots.Count);
        Assert.Equal(snapshot2, mf.Map.Snapshots[0]);
        Assert.Equal(snapshot3, mf.Map.Snapshots[1]);
        mf.RemoveSnapshot(new SnapshotKey("key1", "", ""));
        Assert.Single(mf.Map.Snapshots);
        Assert.Equal(snapshot3, mf.Map.Snapshots[0]);
        mf = new MapFile();
        mf.TakeSnapshot("key3", "type3", "value3", "group3");
        Assert.Single(mf.Map.Snapshots);
        Assert.Equal(1, mf.Map.Snapshots[0].Count);
        mf.TakeSnapshot("key3", "type3", "value3", "group2");
        Assert.Single(mf.Map.Snapshots);
        Assert.Equal(2, mf.Map.Snapshots[0].Count);

        mf = new MapFile();
        mf.Modified = false;
        Assert.False(mf.Modified);
        Assert.Equal(0, mf.Map.Info.UpdatedTime);
        mf.MarkAsModified();
        Assert.True(mf.Modified);
        Assert.NotEqual(0, mf.Map.Info.UpdatedTime);
        mf = new MapFile();
        var rf = mf.ToRecentFile();
        Assert.Equal("", rf.Path);
        Assert.Equal("", rf.Name);
        mf.Path = "/mypath";
        mf.Map.Info.Name = "myname";
        rf = mf.ToRecentFile();
        Assert.Equal("/mypath", rf.Path);
        Assert.Equal("myname", rf.Name);
    }
    [Fact]
    public void TestItemKey()
    {
        Assert.True(ItemKey.Validate("ab"));
        Assert.False(ItemKey.Validate(""));
        Assert.False(ItemKey.Validate("a\nb"));
        var room = new Room()
        {
            Key = "ab",
        };
        Assert.True(room.Validated());
        room.Key = "";
        Assert.False(room.Validated());
        room.Key = "a\nb";
        Assert.False(room.Validated());
        var marker = new Marker()
        {
            Key = "ab",
            Value = "value",
        };
        Assert.True(marker.Validated());
        marker.Key = "";
        Assert.False(marker.Validated());
        marker.Key = "a\nb";
        Assert.False(marker.Validated());
        var route = new Route()
        {
            Key = "ab",
            Rooms = ["room1", "room2"],
        };
        Assert.True(route.Validated());
        route.Key = "";
        Assert.False(route.Validated());
        route.Key = "a\nb";
        Assert.False(route.Validated());
        var trace = new Trace()
        {
            Key = "ab",
            Locations = ["room1", "room2"],
        };
        Assert.True(trace.Validated());
        trace.Key = "";
        Assert.False(trace.Validated());
        trace.Key = "a\nb";
        Assert.False(trace.Validated());
        var region = new Region()
        {
            Key = "ab",
            Items = [new RegionItem(RegionItemType.Room, "room1", false), new RegionItem(RegionItemType.Zone, "zone1", true)]
        };
        Assert.True(region.Validated());
        region.Key = "";
        Assert.False(region.Validated());
        region.Key = "a\nb";
        Assert.False(region.Validated());
        var landmark = new Landmark()
        {
            Key = "ab",
            Type = "type1",
            Value = "value1",
        };
        Assert.True(landmark.Validated());
        landmark.Key = "";
        Assert.False(landmark.Validated());
        landmark.Key = "a\nb";
        Assert.False(landmark.Validated());
        var shortcut = new Shortcut()
        {
            Key = "ab",
            Command = "command1",
            To = "to1",
        };
        Assert.True(shortcut.Validated());
        shortcut.Key = "";
        Assert.False(shortcut.Validated());
        shortcut.Key = "a\nb";
        Assert.False(shortcut.Validated());
        var variable = new Variable()
        {
            Key = "ab",
            Value = "value1",
        };
        Assert.True(variable.Validated());
        variable.Key = "";
        Assert.False(variable.Validated());
        variable.Key = "a\nb";
        Assert.False(variable.Validated());
        var snapshot = new Snapshot()
        {
            Key = "ab",
            Timestamp = 123456,
        };
        Assert.True(snapshot.Validated());
        snapshot.Key = "";
        Assert.False(snapshot.Validated());
        snapshot.Key = "a\nb";
        Assert.False(snapshot.Validated());
    }
    [Fact]
    public void TestContext()
    {
        var ctx = new Context();
        Assert.Empty(ctx.Tags);
        Assert.Equal(ctx, ctx.WithTags([new ValueTag("tag1", 1), new ValueTag("tag2", 2)]));
        Assert.Equal(2, ctx.Tags.Count);
        Assert.Equal(1, ctx.Tags["tag1"]);
        Assert.Equal(2, ctx.Tags["tag2"]);
        Assert.Empty(ctx.RoomConditions);
        Assert.Equal(ctx, ctx.WithRoomConditions([new ValueCondition("con1", 0, false), new ValueCondition("con2", 0, true)]));
        Assert.Equal(2, ctx.RoomConditions.Count);
        Assert.Equal("con1", ctx.RoomConditions[0].Key);
        Assert.Equal(0, ctx.RoomConditions[0].Value);
        Assert.False(ctx.RoomConditions[0].Not);
        Assert.Equal("con2", ctx.RoomConditions[1].Key);
        Assert.Empty(ctx.Rooms);
        Assert.Equal(ctx, ctx.WithRooms([new Room() { Key = "room1" }, new Room() { Key = "room2" }]));
        Assert.Equal(2, ctx.Rooms.Count);
        Assert.Equal("room1", ctx.Rooms["room1"].Key);
        Assert.Equal("room2", ctx.Rooms["room2"].Key);
        Assert.Empty(ctx.Whitelist);
        Assert.Equal(ctx, ctx.WithWhitelist(["room1", "room2"]));
        Assert.Equal(2, ctx.Whitelist.Count);
        Assert.True(ctx.Whitelist["room1"]);
        Assert.True(ctx.Whitelist["room2"]);
        Assert.Empty(ctx.Blacklist);
        Assert.Equal(ctx, ctx.WithBlacklist(["room3", "room4"]));
        Assert.Equal(2, ctx.Blacklist.Count);
        Assert.True(ctx.Blacklist["room3"]);
        Assert.True(ctx.Blacklist["room4"]);
        Assert.Empty(ctx.Shortcuts);
        Assert.Equal(ctx, ctx.WithShortcuts([
            new RoomConditionExit() {  Command = "cmd1",To="to1" },
            new RoomConditionExit() { Command = "cmd2" ,To="to2"},
        ]));
        Assert.Equal(2, ctx.Shortcuts.Count);
        Assert.Equal("to1", ctx.Shortcuts[0].To);
        Assert.Equal("cmd1", ctx.Shortcuts[0].Command);
        Assert.Equal("to2", ctx.Shortcuts[1].To);
        Assert.Equal("cmd2", ctx.Shortcuts[1].Command);
        Assert.Empty(ctx.Paths);
        Assert.Equal(ctx, ctx.WithPaths([
            new HellMapManager.Models.Path() { To = "to1" ,From="from1",Command="cmd1"},
            new HellMapManager.Models.Path() { To = "to2" ,From="from2",Command="cmd2"},
            new HellMapManager.Models.Path() { To = "to3" ,From="from1",Command="cmd3"},
        ]));
        Assert.Equal(2, ctx.Paths.Count);
        Assert.Equal("to1", ctx.Paths["from1"][0].To);
        Assert.Equal("cmd1", ctx.Paths["from1"][0].Command);
        Assert.Equal("to3", ctx.Paths["from1"][1].To);
        Assert.Equal("cmd3", ctx.Paths["from1"][1].Command);
        Assert.Equal("to2", ctx.Paths["from2"][0].To);
        Assert.Equal("cmd2", ctx.Paths["from2"][0].Command);
        Assert.Empty(ctx.BlockedLinks);
        Assert.Equal(ctx, ctx.WithBlockedLinks([
            new Link() { From = "from1", To = "to1" },
            new Link() { From = "from2", To = "to2" },
            new Link() { From = "from1", To = "to3" },
        ]));
        Assert.Equal(2, ctx.BlockedLinks.Count);
        Assert.True(ctx.BlockedLinks["from1"]["to1"]);
        Assert.True(ctx.BlockedLinks["from1"]["to3"]);
        Assert.True(ctx.BlockedLinks["from2"]["to2"]);
        Assert.True(ctx.IsBlocked("from1", "to1"));
        Assert.True(ctx.IsBlocked("from2", "to2"));
        Assert.True(ctx.IsBlocked("from1", "to3"));
        Assert.False(ctx.IsBlocked("notexist1", "notexist2"));
        Assert.False(ctx.IsBlocked("from1", "notexist2"));
        Assert.False(ctx.IsBlocked("from1", "notexist2"));
        Assert.False(ctx.IsBlocked("notexist1", "to1"));
        Assert.False(ctx.IsBlocked("notexist1", "to2"));
        Assert.False(ctx.IsBlocked("notexist1", "to3"));

        Assert.Empty(ctx.CommandCosts);
        Assert.Equal(ctx, ctx.WithCommandCosts([
            new CommandCost() { Command = "cmd1",To="to1", Cost = 1 },
            new CommandCost() { Command = "cmd2",To="to1", Cost = 2 },
            new CommandCost() { Command = "cmd1",To="to3", Cost = 3 },
        ]));
        Assert.Equal(2, ctx.CommandCosts.Count);
        Assert.Equal(1, ctx.CommandCosts["cmd1"]["to1"]);
        Assert.Equal(2, ctx.CommandCosts["cmd2"]["to1"]);
        Assert.Equal(3, ctx.CommandCosts["cmd1"]["to3"]);
        Assert.Equal(ctx, ctx.ClearTags());
        Assert.Empty(ctx.Tags);
        Assert.Equal(ctx, ctx.ClearRoomConditions());
        Assert.Empty(ctx.RoomConditions);
        Assert.Equal(ctx, ctx.ClearRooms());
        Assert.Empty(ctx.Rooms);
        Assert.Equal(ctx, ctx.ClearWhitelist());
        Assert.Empty(ctx.Whitelist);
        Assert.Equal(ctx, ctx.ClearBlacklist());
        Assert.Empty(ctx.Blacklist);
        Assert.Equal(ctx, ctx.ClearShortcuts());
        Assert.Empty(ctx.Shortcuts);
        Assert.Equal(ctx, ctx.ClearPaths());
        Assert.Empty(ctx.Paths);
        Assert.Equal(ctx, ctx.ClearBlockedLinks());
        Assert.Empty(ctx.BlockedLinks);
        Assert.Equal(ctx, ctx.ClearCommandCosts());
        Assert.Empty(ctx.CommandCosts);
    }
    [Fact]
    public void TestEnvironment()
    {
        var env = new HellMapManager.Models.Environment()
        {
            Tags = [new ValueTag("tag1", 1), new ValueTag("tag2", 2)],
            RoomConditions = [new ValueCondition("con1", 0, false), new ValueCondition("con2", 0, true)],
            Rooms = [new Room() { Key = "room1" }, new Room() { Key = "room2" }],
            Whitelist = ["room1", "room2"],
            Blacklist = ["room3", "room4"],
            Shortcuts = [new RoomConditionExit() { Command = "cmd1", To = "to1" }, new RoomConditionExit() { Command = "cmd2", To = "to2" }],
            Paths = [new HellMapManager.Models.Path() { To = "to1", From = "from1", Command = "cmd1" }, new HellMapManager.Models.Path() { To = "to2", From = "from2", Command = "cmd2" }, new HellMapManager.Models.Path() { To = "to3", From = "from1", Command = "cmd3" }],
            BlockedLinks = [new Link() { From = "from1", To = "to1" }, new Link() { From = "from2", To = "to2" }, new Link() { From = "from1", To = "to3" }],
            CommandCosts = [new CommandCost() { Command = "cmd1", To = "to1", Cost = 1 }, new CommandCost() { Command = "cmd2", To = "to1", Cost = 2 }, new CommandCost() { Command = "cmd1", To = "to3", Cost = 3 }],
        };
        var ctx = Context.FromEnvironment(env);
        Assert.Equal(2, ctx.Tags.Count);
        Assert.Equal(1, ctx.Tags["tag1"]);
        Assert.Equal(2, ctx.Tags["tag2"]);
        Assert.Equal(2, ctx.RoomConditions.Count);
        Assert.Equal("con1", ctx.RoomConditions[0].Key);
        Assert.Equal(0, ctx.RoomConditions[0].Value);
        Assert.False(ctx.RoomConditions[0].Not);
        Assert.Equal("con2", ctx.RoomConditions[1].Key);
        Assert.Equal(2, ctx.Rooms.Count);
        Assert.Equal("room1", ctx.Rooms["room1"].Key);
        Assert.Equal("room2", ctx.Rooms["room2"].Key);
        Assert.Equal(2, ctx.Whitelist.Count);
        Assert.True(ctx.Whitelist["room1"]);
        Assert.True(ctx.Whitelist["room2"]);
        Assert.Equal(2, ctx.Blacklist.Count);
        Assert.True(ctx.Blacklist["room3"]);
        Assert.True(ctx.Blacklist["room4"]);
        Assert.Equal(2, ctx.Shortcuts.Count);
        Assert.Equal("to1", ctx.Shortcuts[0].To);
        Assert.Equal("cmd1", ctx.Shortcuts[0].Command);
        Assert.Equal("to2", ctx.Shortcuts[1].To);
        Assert.Equal("cmd2", ctx.Shortcuts[1].Command);
        Assert.Equal(2, ctx.Paths.Count);
        Assert.Equal("to1", ctx.Paths["from1"][0].To);
        Assert.Equal("cmd1", ctx.Paths["from1"][0].Command);
        Assert.Equal("to3", ctx.Paths["from1"][1].To);
        Assert.Equal("cmd3", ctx.Paths["from1"][1].Command);
        Assert.Equal("to2", ctx.Paths["from2"][0].To);
        Assert.Equal("cmd2", ctx.Paths["from2"][0].Command);
        Assert.Equal(2, ctx.BlockedLinks.Count);
        Assert.True(ctx.BlockedLinks["from1"]["to1"]);
        Assert.True(ctx.BlockedLinks["from1"]["to3"]);
        Assert.True(ctx.BlockedLinks["from2"]["to2"]);
        Assert.Equal(2, ctx.CommandCosts.Count);
        Assert.Equal(1, ctx.CommandCosts["cmd1"]["to1"]);
        Assert.Equal(2, ctx.CommandCosts["cmd2"]["to1"]);
        Assert.Equal(3, ctx.CommandCosts["cmd1"]["to3"]);
        var env2 = ctx.ToEnvironment();
        Assert.Equal(env.Tags.Count, env2.Tags.Count);
        env.Tags.Sort((a, b) => a.Key.CompareTo(b.Key));
        env2.Tags.Sort((a, b) => a.Key.CompareTo(b.Key));
        for (var i = 0; i < env.Tags.Count; i++)
        {
            Assert.Equal(env.Tags[i].Key, env2.Tags[i].Key);
            Assert.Equal(env.Tags[i].Value, env2.Tags[i].Value);
        }
        Assert.Equal(env.RoomConditions.Count, env2.RoomConditions.Count);
        env.RoomConditions.Sort((a, b) => a.Key.CompareTo(b.Key));
        env2.RoomConditions.Sort((a, b) => a.Key.CompareTo(b.Key));
        for (var i = 0; i < env.RoomConditions.Count; i++)
        {
            Assert.Equal(env.RoomConditions[i].Key, env2.RoomConditions[i].Key);
            Assert.Equal(env.RoomConditions[i].Value, env2.RoomConditions[i].Value);
            Assert.Equal(env.RoomConditions[i].Not, env2.RoomConditions[i].Not);
        }
        Assert.Equal(env.Rooms.Count, env2.Rooms.Count);
        env.Rooms.Sort((a, b) => a.Key.CompareTo(b.Key));
        env2.Rooms.Sort((a, b) => a.Key.CompareTo(b.Key));
        for (var i = 0; i < env.Rooms.Count; i++)
        {
            Assert.Equal(env.Rooms[i].Key, env2.Rooms[i].Key);
        }
        Assert.Equal(env.Whitelist.Count, env2.Whitelist.Count);
        env.Whitelist.Sort();
        env2.Whitelist.Sort();
        for (var i = 0; i < env.Whitelist.Count; i++)
        {
            Assert.Equal(env.Whitelist[i], env2.Whitelist[i]);
        }
        Assert.Equal(env.Blacklist.Count, env2.Blacklist.Count);
        env.Blacklist.Sort();
        env2.Blacklist.Sort();
        for (var i = 0; i < env.Blacklist.Count; i++)
        {
            Assert.Equal(env.Blacklist[i], env2.Blacklist[i]);
        }
        Assert.Equal(env.Shortcuts.Count, env2.Shortcuts.Count);
        env.Shortcuts.Sort((a, b) => a.To.CompareTo(b.To));
        env2.Shortcuts.Sort((a, b) => a.To.CompareTo(b.To));
        for (var i = 0; i < env.Shortcuts.Count; i++)
        {
            Assert.Equal(env.Shortcuts[i].Command, env2.Shortcuts[i].Command);
            Assert.Equal(env.Shortcuts[i].To, env2.Shortcuts[i].To);
        }
        Assert.Equal(env.Paths.Count, env2.Paths.Count);
        env.Paths.Sort((a, b) => a.To.CompareTo(b.To));
        env2.Paths.Sort((a, b) => a.To.CompareTo(b.To));
        for (var i = 0; i < env.Paths.Count; i++)
        {
            Assert.Equal(env.Paths[i].To, env2.Paths[i].To);
            Assert.Equal(env.Paths[i].From, env2.Paths[i].From);
            Assert.Equal(env.Paths[i].Command, env2.Paths[i].Command);
        }
        Assert.Equal(env.BlockedLinks.Count, env2.BlockedLinks.Count);
        env.BlockedLinks.Sort((a, b) => a.From.CompareTo(b.From));
        env2.BlockedLinks.Sort((a, b) => a.From.CompareTo(b.From));
        for (var i = 0; i < env.BlockedLinks.Count; i++)
        {
            Assert.Equal(env.BlockedLinks[i].From, env2.BlockedLinks[i].From);
            Assert.Equal(env.BlockedLinks[i].To, env2.BlockedLinks[i].To);
        }
        Assert.Equal(env.CommandCosts.Count, env2.CommandCosts.Count);
        env.CommandCosts.Sort((a, b) => a.To.CompareTo(b.To));
        env2.CommandCosts.Sort((a, b) => a.To.CompareTo(b.To));
        for (var i = 0; i < env.CommandCosts.Count; i++)
        {
            Assert.Equal(env.CommandCosts[i].Command, env2.CommandCosts[i].Command);
            Assert.Equal(env.CommandCosts[i].To, env2.CommandCosts[i].To);
            Assert.Equal(env.CommandCosts[i].Cost, env2.CommandCosts[i].Cost);
        }
    }
    [Fact]
    public void TestMapperOption()
    {
        var opt = new MapperOptions();
        Assert.Equal(0, opt.MaxExitCost);
        Assert.Equal(0, opt.MaxTotalCost);
        Assert.False(opt.DisableShortcuts);
        Assert.Equal(opt, opt.WithMaxExitCost(5));
        Assert.Equal(5, opt.MaxExitCost);
        Assert.Equal(opt, opt.WithMaxTotalCost(50));
        Assert.Equal(50, opt.MaxTotalCost);
        Assert.Equal(opt, opt.WithDisableShortcuts(true));
        Assert.True(opt.DisableShortcuts);
    }
    [Fact]
    public void TestStep()
    {
        var step = new Step("cmd1", "to1", 5);
        Assert.Equal("cmd1", step.Command);
        Assert.Equal("to1", step.Target);
        Assert.Equal(5, step.Cost);
        var result = new QueryReuslt();
        Assert.Equal(0, result.Cost);
        Assert.Equal("", result.From);
        Assert.Equal("", result.To);
        Assert.Empty(result.Steps);
        Assert.Empty(result.Unvisited);
        Assert.False(result.IsSuccess());
        Assert.Null(result.SuccessOrNull());
        result.To = "to1";
        Assert.False(result.IsSuccess());
        Assert.Null(result.SuccessOrNull());
        result.To = "";
        result.From = "from1";
        Assert.False(result.IsSuccess());
        Assert.Null(result.SuccessOrNull());
        result.To = "to1";
        result.From = "from1";
        Assert.True(result.IsSuccess());
        Assert.NotNull(result.SuccessOrNull());
        var result2 = QueryReuslt.Fail;
        Assert.False(result2.IsSuccess());
        Assert.Null(result2.SuccessOrNull());
    }
    [Fact]
    public void TestSnapshotFilter()
    {
        var snapshot = new Snapshot()
        {
            Key = "key1",
            Type = "type1",
            Value = "value1",
            Group = "group1",
            Timestamp = 1234567890
        };
        var sf = new SnapshotFilter("key1", "type1", "group1");
        Assert.Equal("key1", sf.Key);
        Assert.Equal("type1", sf.Type);
        Assert.Equal("group1", sf.Group);
        Assert.True(new SnapshotFilter(null, null, null).Validate(snapshot));
        Assert.True(new SnapshotFilter("key1", null, null).Validate(snapshot));
        Assert.True(new SnapshotFilter("key1", "type1", null).Validate(snapshot));
        Assert.True(new SnapshotFilter("key1", "type1", "group1").Validate(snapshot));
        Assert.True(new SnapshotFilter(null, "type1", null).Validate(snapshot));
        Assert.True(new SnapshotFilter(null, "type1", "group1").Validate(snapshot));
        Assert.True(new SnapshotFilter(null, null, "group1").Validate(snapshot));
        Assert.False(new SnapshotFilter("keynotfound", null, null).Validate(snapshot));
        Assert.False(new SnapshotFilter(null, "typenotfound", null).Validate(snapshot));
        Assert.False(new SnapshotFilter(null, null, "groupnotfound").Validate(snapshot));
        Assert.False(new SnapshotFilter("keynotfound", "typenotfound", "groupnotfound").Validate(snapshot));
    }
    [Fact]
    public void TestSnapshotSearch()
    {
        var snapshot = new Snapshot()
        {
            Key = "key1",
            Type = "type1",
            Value = "value1\nvalue2",
            Group = "group1",
            Timestamp = 1234567890
        };
        var ss = new SnapshotSearch();
        Assert.Null(ss.Type);
        Assert.Null(ss.Group);
        Assert.Empty(ss.Keywords);
        Assert.True(ss.PartialMatch);
        Assert.False(ss.Any);
        Assert.True(ss.Validate(snapshot));
        ss.Type = "type2";
        Assert.False(ss.Validate(snapshot));
        ss.Type = "type1";
        Assert.True(ss.Validate(snapshot));
        ss.Group = "group2";
        Assert.False(ss.Validate(snapshot));
        ss.Group = "group1";
        Assert.True(ss.Validate(snapshot));
        ss.Keywords = ["valuenotfound"];
        Assert.False(ss.Validate(snapshot));
        ss.Keywords = ["value1"];
        Assert.True(ss.Validate(snapshot));
        ss.Keywords = ["value1", "value3"];
        Assert.False(ss.Validate(snapshot));
        ss.Any = true;
        Assert.True(ss.Validate(snapshot));
        ss.PartialMatch = false;
        ss.Keywords = ["value1"];
        Assert.False(ss.Validate(snapshot));
        ss.Keywords = ["value1\nvalue2"];
        Assert.True(ss.Validate(snapshot));
    }
    [Fact]
    public void TestSnapshotSearchResult()
    {
        var sr = new SnapshotSearchResult();
        Assert.Equal(0, sr.Count);
        Assert.Equal(0, sr.Sum);
        Assert.Equal("", sr.Key);
        Assert.Empty(sr.Items);
        var snapshot = new Snapshot()
        {
            Key = "key1",
            Type = "type1",
            Value = "value1\nvalue2",
            Group = "group1",
            Timestamp = 1234567890,
            Count = 15
        };
        var snapshot2 = new Snapshot()
        {
            Key = "key1",
            Type = "type1",
            Value = "value1\nvalue2",
            Group = "group1",
            Timestamp = 1234567890,
            Count = 30
        };
        sr.Add(snapshot, true);
        Assert.Equal(15, sr.Count);
        Assert.Equal(15, sr.Sum);
        Assert.Equal("", sr.Key);
        Assert.Single(sr.Items);
        Assert.Equal(snapshot, sr.Items[0]);
        sr.Add(snapshot2, false);
        Assert.Equal(15, sr.Count);
        Assert.Equal(45, sr.Sum);
        Assert.Equal("", sr.Key);
        Assert.Single(sr.Items);
        Assert.Equal(snapshot, sr.Items[0]);
        Assert.Equal(snapshot, sr.Items[0]);
    }
}