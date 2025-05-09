using Avalonia.Input;
using HellMapManager.Models;

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

    }
    [Fact]
    public void TestExit()
    {
        var exit = new Exit()
        {
            To = "to",
            Command = "cmd",
            Cost = 2,
            Conditions = [new Condition("con1", false), new Condition("con2", true)]
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
        exit2.Conditions.Add(new("con3", true));
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
            Tags = ["tag1", "tag2"],
            Data = [new Data("dkey1", "dval1"), new Data("dkey2", "dval2")],
            Exits = [
                new Exit(){
                Command="command1",
                To="to1",
                Cost=2,
                Conditions=[
                    new Condition("con1",true),
                    new Condition("con2",false),
                ]
            },
               new Exit(){
                Command="command2",
                To="to2",
                Cost=4,

            }
            ]
        };
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
        room2.Tags[1] = "";
        Assert.False(room.Equal(room2));
        Assert.True(room2.Validated());

        room2 = room.Clone();
        room2.Tags.Add("");
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
            RoomConditions = [new Condition("con1", false), new Condition("con2", true)],
            Conditions = [new Condition("con3", false), new Condition("con4", true)]
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
        sc2.Conditions.Add(new("con5", true));
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
        sc2.RoomConditions.Add(new("con5", true));
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
        snapshot2 = snapshot.Clone();
        snapshot2.Timestamp = -1;
        Assert.False(snapshot.Equal(snapshot2));
        Assert.False(snapshot2.Validated());

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
}