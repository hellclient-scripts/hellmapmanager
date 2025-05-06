using HellMapManager.Models;

namespace TestProject;

public class BussinessTest
{
    [Fact]
    public void TestExit()
    {
        var exit = new Exit();
        exit.Conditions = [new Condition("con1", true), new Condition("con2", false)];
        exit.Arrange();
        Assert.True(exit.Conditions[0].Equal(new Condition("con2", false)));
        Assert.True(exit.Conditions[1].Equal(new Condition("con1", true)));
        exit.Conditions = [new Condition("con1", false), new Condition("con2", false)];
        exit.Arrange();
        Assert.True(exit.Conditions[0].Equal(new Condition("con1", false)));
        Assert.True(exit.Conditions[1].Equal(new Condition("con2", false)));
        exit.Conditions = [new Condition("con1", false), new Condition("con2", true)];
        exit.Arrange();
        Assert.True(exit.Conditions[0].Equal(new Condition("con1", false)));
        Assert.True(exit.Conditions[1].Equal(new Condition("con2", true)));
        exit.Conditions = [new Condition("con1", true), new Condition("con2", true)];
        exit.Arrange();
        Assert.True(exit.Conditions[0].Equal(new Condition("con1", true)));
        Assert.True(exit.Conditions[1].Equal(new Condition("con2", true)));
    }
    private static Room SuffRoom(string suff)
    {
        return new Room()
        {
            Key = $"key{suff}",
            Name = $"name{suff}",
            Group = $"group{suff}",
            Desc = $"desc{suff}",
            Tags = [$"tag1{suff}", $"tag2{suff}"],
            Exits = [
                        new Exit(){
                    To=$"to1{suff}",
                    Command=$"cmd1{suff}",
                    Cost=1,
                    Conditions=[
                        new Condition($"con1{suff}",true),
                        new Condition($"con2{suff}",false),
                    ]
                },
                new Exit(){
                    To=$"to2{suff}",
                    Command=$"cmd2{suff}",
                }
                    ],
            Data = [
                new Data($"key1{suff}",$"val1{suff}"),
                new Data($"key2{suff}",$"val2{suff}"),
            ],
        };
    }
    [Fact]
    public void TestRoom()
    {
        Room room;
        room = new Room();
        var data1 = new Data("key1", "val1");
        var data2 = new Data("key2", "val2");
        room.Data = [data2, data1];
        room.Arrange();
        Assert.True(room.Data[0].Equal(data1));
        Assert.True(room.Data[1].Equal(data2));
        room.Data = [];
        room.SetData(new Data("key1", "valnew"));
        Assert.True(room.Data[0].Equal(new Data("key1", "valnew")));
        room.SetData(new Data("key2", "valnew2"));
        Assert.True(room.Data[1].Equal(new Data("key2", "valnew2")));
        room.SetData(new Data("key0", "val0"));
        Assert.True(room.Data[0].Equal(new Data("key0", "val0")));
        room.SetData(new Data("key3", "val3"));
        Assert.True(room.Data[3].Equal(new Data("key3", "val3")));
        room.Data = [];
        room.SetDatas([new Data("key0", "val0"), data2, data1, new Data("key3", "val3")]);
        Assert.True(room.Data[0].Equal(new Data("key0", "val0")));
        Assert.True(room.Data[1].Equal(data1));
        Assert.True(room.Data[2].Equal(data2));
        Assert.True(room.Data[3].Equal(new Data("key3", "val3")));

        room = new Room();
        Assert.True(room.Equal(Room.Decode(room.Encode())));
        room = SuffRoom("");
        Assert.True(room.Equal(Room.Decode(room.Encode())));
        room = SuffRoom(">:=@!;\\,&!\n");
        Assert.True(room.Equal(Room.Decode(room.Encode())));
        room = SuffRoom("\\>\\:\\=\\@\\!\\;\\\\\\,\\&\\!\\n");
        Assert.True(room.Equal(Room.Decode(room.Encode())));

    }
    private static Marker SuffMarker(string suff)
    {
        return new Marker()
        {
            Key = $"key{suff}",
            Value = $"value{suff}",
            Group = $"group{suff}",
            Desc = $"desc{suff}",
            Message = $"message{suff}"
        };
    }
    [Fact]
    public void TestMarker()
    {
        var marker = SuffMarker("");
        var marker2 = marker.Clone();
        marker2.Arrange();
        Assert.True(marker.Equal(marker2));
        marker = SuffMarker("");
        Assert.True(marker.Equal(Marker.Decode(marker.Encode())));
        marker = SuffMarker(">:=@!;\\,&!\n");
        Assert.True(marker.Equal(Marker.Decode(marker.Encode())));
        marker = SuffMarker("\\>\\:\\=\\@\\!\\;\\\\\\,\\&\\!\\n");
        Assert.True(marker.Equal(Marker.Decode(marker.Encode())));

    }
    private static Route SuffRoute(string suff)
    {
        return new Route()
        {
            Key = $"key{suff}",
            Rooms = [$"rid{suff}", $"rid{suff}"],
            Group = $"group{suff}",
            Desc = $"desc{suff}",
            Message = $"message{suff}"
        };
    }
    [Fact]
    public void TestRoute()
    {
        var route = SuffRoute("");
        var route2 = route.Clone();
        route2.Arrange();
        Assert.True(route.Equal(route2));
        route = SuffRoute("");
        Assert.True(route.Equal(Route.Decode(route.Encode())));
        route = SuffRoute(">:=@!;\\,&!\n");
        Assert.True(route.Equal(Route.Decode(route.Encode())));
        route = SuffRoute("\\>\\:\\=\\@\\!\\;\\\\\\,\\&\\!\\n");
        Assert.True(route.Equal(Route.Decode(route.Encode())));
    }
    private static Trace SuffTrace(string suff)
    {
        return new Trace()
        {
            Key = $"key{suff}",
            Locations = [$"rid{suff}", $"rid{suff}"],
            Group = $"group{suff}",
            Desc = $"desc{suff}",
            Message = $"message{suff}"
        };
    }
    [Fact]
    public void TestTrace()
    {
        var trace = SuffTrace("");
        Trace trace2;
        trace2 = new Trace();
        trace2.Locations = ["2", "1"];
        trace2.Arrange();
        Assert.Equal(2, trace2.Locations.Count);
        Assert.Equal("1", trace2.Locations[0]);
        Assert.Equal("2", trace2.Locations[1]);
        trace2 = new Trace();
        trace2.Locations = ["1", "2"];
        trace2.RemoveLocations(["2", "3"]);
        Assert.Single(trace2.Locations);
        Assert.Equal("1", trace2.Locations[0]);
        trace2 = new Trace();
        trace2.Locations = ["2", "3"];
        trace2.AddLocations(["1", "3", "4"]);
        Assert.Equal(4, trace2.Locations.Count);
        Assert.Equal("1", trace2.Locations[0]);
        Assert.Equal("2", trace2.Locations[1]);
        Assert.Equal("3", trace2.Locations[2]);
        Assert.Equal("4", trace2.Locations[3]);
        trace2 = trace.Clone();
        Assert.True(trace.Equal(trace2));
        trace = SuffTrace("");
        Assert.True(trace.Equal(Trace.Decode(trace.Encode())));
        trace = SuffTrace(">:=@!;\\,&!\n");
        Assert.True(trace.Equal(Trace.Decode(trace.Encode())));
        trace = SuffTrace("\\>\\:\\=\\@\\!\\;\\\\\\,\\&\\!\\n");
        Assert.True(trace.Equal(Trace.Decode(trace.Encode())));
    }
    private static Region SuffRegion(string suff)
    {
        return new Region()
        {
            Key = $"key{suff}",
            Group = $"group{suff}",
            Desc = $"desc{suff}",
            Items = [
                new RegionItem(RegionItemType.Room, $"val1{suff}",false),
                new RegionItem(RegionItemType.Zone, $"val2{suff}",true),
            ],
            Message = $"message{suff}"
        };
    }
    [Fact]
    public void TestRegion()
    {
        var region = SuffRegion("");
        var region2 = region.Clone();
        region2.Arrange();
        Assert.True(region.Equal(region2));
        region = SuffRegion("");
        Assert.True(region.Equal(Region.Decode(region.Encode())));
        region = SuffRegion(">:=@!;\\,&!\n");
        Assert.True(region.Equal(Region.Decode(region.Encode())));
        region = SuffRegion("\\>\\:\\=\\@\\!\\;\\\\\\,\\&\\!\\n");
        Assert.True(region.Equal(Region.Decode(region.Encode())));
    }
    public Landmark SuffLandmark(string suff)
    {
        return new Landmark()
        {
            Key = $"key{suff}",
            Type = $"type{suff}",
            Value = $"value{suff}",
            Group = $"group{suff}",
            Desc = $"desc{suff}"
        };
    }
    [Fact]
    public void TestLandmark()
    {
        var landmark = SuffLandmark("");
        var landmark2 = landmark.Clone();
        landmark2.Arrange();
        Assert.True(landmark.Equal(landmark2));
        landmark = SuffLandmark("");
        Assert.True(landmark.Equal(Landmark.Decode(landmark.Encode())));
        landmark = SuffLandmark(">:=@!;\\,&!\n");
        Assert.True(landmark.Equal(Landmark.Decode(landmark.Encode())));
        landmark = SuffLandmark("\\>\\:\\=\\@\\!\\;\\\\\\,\\&\\!\\n");
        Assert.True(landmark.Equal(Landmark.Decode(landmark.Encode())));
    }
    [Fact]
    public void TestLandmarkKey()
    {
        var landmark = new Landmark()
        {
            Key = "key",
            Type = "type",
        };
        var landmark2 = new Landmark()
        {
            Key = "key",
            Type = "Type2",
        };
        var key = new LandmarkKey("key", "type");
        var key2 = landmark2.UniqueKey();
        Assert.True(key.Equal(landmark.UniqueKey()));
        Assert.Equal(key.ToString(), landmark.UniqueKey().ToString());
        Assert.False(key.Equal(key2));
        Assert.NotEqual(key.ToString(), key2.ToString());
    }
    private static Shortcut SuffShortcut(string suff)
    {
        return new Shortcut()
        {
            Key = $"key{suff}",
            Group = $"group{suff}",
            Desc = $"desc{suff}",
            RoomConditions = [new Condition($"con1{suff}", false), new Condition($"con2{suff}", true)],
            Command = $"cmd{suff}",
            To = $"to{suff}",
            Conditions = [new Condition($"con3{suff}", false), new Condition($"con4{suff}", true)],
            Cost = 1
        };
    }
    [Fact]
    public void TestShortcut()
    {
        var shortcut = SuffShortcut("");
        shortcut.Arrange();
        Shortcut shortcut2;
        shortcut2 = new Shortcut();
        shortcut2.Conditions = [new Condition("con1", true), new Condition("con2", false)];
        shortcut2.Arrange();
        Assert.True(shortcut2.Conditions[0].Equal(new Condition("con2", false)));
        Assert.True(shortcut2.Conditions[1].Equal(new Condition("con1", true)));
        shortcut2.Conditions = [new Condition("con1", false), new Condition("con2", false)];
        shortcut2.Arrange();
        Assert.True(shortcut2.Conditions[0].Equal(new Condition("con1", false)));
        Assert.True(shortcut2.Conditions[1].Equal(new Condition("con2", false)));
        shortcut2.Conditions = [new Condition("con1", false), new Condition("con2", true)];
        shortcut2.Arrange();
        Assert.True(shortcut2.Conditions[0].Equal(new Condition("con1", false)));
        Assert.True(shortcut2.Conditions[1].Equal(new Condition("con2", true)));
        shortcut2.Conditions = [new Condition("con1", true), new Condition("con2", true)];
        shortcut2.Arrange();
        Assert.True(shortcut2.Conditions[0].Equal(new Condition("con1", true)));
        Assert.True(shortcut2.Conditions[1].Equal(new Condition("con2", true)));

        shortcut2 = new Shortcut();
        shortcut2.RoomConditions = [new Condition("con1", true), new Condition("con2", false)];
        shortcut2.Arrange();
        Assert.True(shortcut2.RoomConditions[0].Equal(new Condition("con2", false)));
        Assert.True(shortcut2.RoomConditions[1].Equal(new Condition("con1", true)));
        shortcut2.RoomConditions = [new Condition("con1", false), new Condition("con2", false)];
        shortcut2.Arrange();
        Assert.True(shortcut2.RoomConditions[0].Equal(new Condition("con1", false)));
        Assert.True(shortcut2.RoomConditions[1].Equal(new Condition("con2", false)));
        shortcut2.RoomConditions = [new Condition("con1", false), new Condition("con2", true)];
        shortcut2.Arrange();
        Assert.True(shortcut2.RoomConditions[0].Equal(new Condition("con1", false)));
        Assert.True(shortcut2.RoomConditions[1].Equal(new Condition("con2", true)));
        shortcut2.RoomConditions = [new Condition("con1", true), new Condition("con2", true)];
        shortcut2.Arrange();
        Assert.True(shortcut2.RoomConditions[0].Equal(new Condition("con1", true)));
        Assert.True(shortcut2.RoomConditions[1].Equal(new Condition("con2", true)));

        shortcut2 = shortcut.Clone();
        shortcut2.Arrange();
        Assert.True(shortcut.Equal(shortcut2));
        shortcut = SuffShortcut("");
        Assert.True(shortcut.Equal(Shortcut.Decode(shortcut.Encode())));
        shortcut = SuffShortcut(">:=@!;\\,&!\n");
        Assert.True(shortcut.Equal(Shortcut.Decode(shortcut.Encode())));
        shortcut = SuffShortcut("\\>\\:\\=\\@\\!\\;\\\\\\,\\&\\!\\n");
        Assert.True(shortcut.Equal(Shortcut.Decode(shortcut.Encode())));
    }
    private static Variable SuffVariable(string suff)
    {
        return new Variable()
        {
            Key = $"key{suff}",
            Value = $"value{suff}",
            Group = $"group{suff}",
            Desc = $"desc{suff}"
        };
    }
    [Fact]
    public void TestVariable()
    {
        var variable = SuffVariable("");
        var variable2 = variable.Clone();
        variable2.Arrange();
        Assert.True(variable.Equal(variable2));
        variable = SuffVariable("");
        Assert.True(variable.Equal(Variable.Decode(variable.Encode())));
        variable = SuffVariable(">:=@!;\\,&!\n");
        Assert.True(variable.Equal(Variable.Decode(variable.Encode())));
        variable = SuffVariable("\\>\\:\\=\\@\\!\\;\\\\\\,\\&\\!\\n");
        Assert.True(variable.Equal(Variable.Decode(variable.Encode())));
    }
    private static Snapshot SuffSnapshot(string suff)
    {
        return new Snapshot()
        {
            Key = $"key{suff}",
            Type = $"type{suff}",
            Value = $"value{suff}",
            Group = $"group{suff}",
            Timestamp = 1234567890
        };
    }
    [Fact]
    public void TestSnapshot()
    {
        var snapshot = SuffSnapshot("");
        var snapshot2 = snapshot.Clone();
        snapshot2.Arrange();
        Assert.True(snapshot.Equal(snapshot2));
        snapshot = SuffSnapshot("");
        Assert.True(snapshot.Equal(Snapshot.Decode(snapshot.Encode())));
        snapshot = SuffSnapshot(">:=@!;\\,&!\n");
        Assert.True(snapshot.Equal(Snapshot.Decode(snapshot.Encode())));
        snapshot = SuffSnapshot("\\>\\:\\=\\@\\!\\;\\\\\\,\\&\\!\\n");
        Assert.True(snapshot.Equal(Snapshot.Decode(snapshot.Encode())));
    }
    [Fact]
    public void TestSnapshotKey()
    {
        var snapshot = new Snapshot()
        {
            Key = "key",
            Type = "type",
            Value = "value",
        };
        var snapshot2 = new Snapshot()
        {
            Key = "key",
            Type = "Type",
            Value = "value2",
        };
        var key = new SnapshotKey("key", "type", "value");
        var key2 = snapshot2.UniqueKey();
        Assert.True(key.Equal(snapshot.UniqueKey()));
        Assert.Equal(key.ToString(), snapshot.UniqueKey().ToString());
        Assert.False(key.Equal(key2));
        Assert.NotEqual(key.ToString(), key2.ToString());
    }

    [Fact]
    public void TestMap()
    {
        var map = new Map();
        var room1 = new Room() { Key = "1", Group = "2" };
        var room2 = new Room() { Key = "2", Group = "2" };
        var room3 = new Room() { Key = "3", Group = "1" };
        map.Rooms = [room1, room2];
        map.Arrange();
        Assert.Equal(room1, map.Rooms[0]);
        Assert.Equal(room2, map.Rooms[1]);
        map.Rooms = [room2, room1];
        map.Arrange();
        Assert.Equal(room1, map.Rooms[0]);
        Assert.Equal(room2, map.Rooms[1]);
        map.Rooms = [room1, room3];
        map.Arrange();
        Assert.Equal(room3, map.Rooms[0]);
        Assert.Equal(room1, map.Rooms[1]);
        var marker1 = new Marker() { Key = "1", Group = "2" };
        var marker2 = new Marker() { Key = "2", Group = "2" };
        var marker3 = new Marker() { Key = "3", Group = "1" };
        map.Markers = [marker1, marker2];
        map.Arrange();
        Assert.Equal(marker1, map.Markers[0]);
        Assert.Equal(marker2, map.Markers[1]);
        map.Markers = [marker2, marker1];
        map.Arrange();
        Assert.Equal(marker1, map.Markers[0]);
        Assert.Equal(marker2, map.Markers[1]);
        map.Markers = [marker1, marker3];
        map.Arrange();
        Assert.Equal(marker3, map.Markers[0]);
        Assert.Equal(marker1, map.Markers[1]);
        var route1 = new Route() { Key = "1", Group = "2" };
        var route2 = new Route() { Key = "2", Group = "2" };
        var route3 = new Route() { Key = "3", Group = "1" };
        map.Routes = [route1, route2];
        map.Arrange();
        Assert.Equal(route1, map.Routes[0]);
        Assert.Equal(route2, map.Routes[1]);
        map.Routes = [route2, route1];
        map.Arrange();
        Assert.Equal(route1, map.Routes[0]);
        Assert.Equal(route2, map.Routes[1]);
        map.Routes = [route1, route3];
        map.Arrange();
        Assert.Equal(route3, map.Routes[0]);
        Assert.Equal(route1, map.Routes[1]);
        var trace1 = new Trace() { Key = "1", Group = "2" };
        var trace2 = new Trace() { Key = "2", Group = "2" };
        var trace3 = new Trace() { Key = "3", Group = "1" };
        map.Traces = [trace1, trace2];
        map.Arrange();
        Assert.Equal(trace1, map.Traces[0]);
        Assert.Equal(trace2, map.Traces[1]);
        map.Traces = [trace2, trace1];
        map.Arrange();
        Assert.Equal(trace1, map.Traces[0]);
        Assert.Equal(trace2, map.Traces[1]);
        map.Traces = [trace1, trace3];
        map.Arrange();
        Assert.Equal(trace3, map.Traces[0]);
        Assert.Equal(trace1, map.Traces[1]);
        var region1 = new Region() { Key = "1", Group = "2" };
        var region2 = new Region() { Key = "2", Group = "2" };
        var region3 = new Region() { Key = "3", Group = "1" };
        map.Regions = [region1, region2];
        map.Arrange();
        Assert.Equal(region1, map.Regions[0]);
        Assert.Equal(region2, map.Regions[1]);
        map.Regions = [region2, region1];
        map.Arrange();
        Assert.Equal(region1, map.Regions[0]);
        Assert.Equal(region2, map.Regions[1]);
        map.Regions = [region1, region3];
        map.Arrange();
        Assert.Equal(region3, map.Regions[0]);
        Assert.Equal(region1, map.Regions[1]);
        var variable1 = new Variable() { Key = "1", Group = "2" };
        var variable2 = new Variable() { Key = "2", Group = "2" };
        var variable3 = new Variable() { Key = "3", Group = "1" };
        map.Variables = [variable1, variable2];
        map.Arrange();
        Assert.Equal(variable1, map.Variables[0]);
        Assert.Equal(variable2, map.Variables[1]);
        map.Variables = [variable2, variable1];
        map.Arrange();
        Assert.Equal(variable1, map.Variables[0]);
        Assert.Equal(variable2, map.Variables[1]);
        map.Variables = [variable1, variable3];
        map.Arrange();
        Assert.Equal(variable3, map.Variables[0]);
        Assert.Equal(variable1, map.Variables[1]);
        var shortcut1 = new Shortcut() { Key = "1", Group = "2" };
        var shortcut2 = new Shortcut() { Key = "2", Group = "2" };
        var shortcut3 = new Shortcut() { Key = "3", Group = "1" };
        map.Shortcuts = [shortcut1, shortcut2];
        map.Arrange();
        Assert.Equal(shortcut1, map.Shortcuts[0]);
        Assert.Equal(shortcut2, map.Shortcuts[1]);
        map.Shortcuts = [shortcut2, shortcut1];
        map.Arrange();
        Assert.Equal(shortcut1, map.Shortcuts[0]);
        Assert.Equal(shortcut2, map.Shortcuts[1]);
        map.Shortcuts = [shortcut1, shortcut3];
        map.Arrange();
        Assert.Equal(shortcut3, map.Shortcuts[0]);
        Assert.Equal(shortcut1, map.Shortcuts[1]);
        var landmark1 = new Landmark() { Key = "1", Group = "2", Type = "2" };
        var landmark2 = new Landmark() { Key = "2", Group = "2", Type = "2" };
        var landmark3 = new Landmark() { Key = "3", Group = "1", Type = "2" };
        var landmark4 = new Landmark() { Key = "4", Group = "2", Type = "1" };
        var landmark5 = new Landmark() { Key = "4", Group = "2", Type = "1" };
        map.Landmarks = [landmark1, landmark2];
        map.Arrange();
        Assert.Equal(landmark1, map.Landmarks[0]);
        Assert.Equal(landmark2, map.Landmarks[1]);
        map.Landmarks = [landmark2, landmark1];
        map.Arrange();
        Assert.Equal(landmark1, map.Landmarks[0]);
        Assert.Equal(landmark2, map.Landmarks[1]);
        map.Landmarks = [landmark1, landmark3];
        map.Arrange();
        Assert.Equal(landmark3, map.Landmarks[0]);
        Assert.Equal(landmark1, map.Landmarks[1]);
        map.Landmarks = [landmark4, landmark3];
        map.Arrange();
        Assert.Equal(landmark3, map.Landmarks[0]);
        Assert.Equal(landmark4, map.Landmarks[1]);
        map.Landmarks = [landmark5, landmark4];
        map.Arrange();
        Assert.Equal(landmark5, map.Landmarks[0]);
        Assert.Equal(landmark4, map.Landmarks[1]);
        var snapshot = new Snapshot() { Key = "1", Group = "2", Type = "2", Timestamp = 1, Value = "2" };
        var snapshot2 = new Snapshot() { Key = "2", Group = "2", Type = "2", Timestamp = 1, Value = "2" };
        var snapshot3 = new Snapshot() { Key = "3", Group = "1", Type = "2", Timestamp = 1, Value = "2" };
        var snapshot4 = new Snapshot() { Key = "3", Group = "1", Type = "2", Timestamp = 0, Value = "2" };
        var snapshot5 = new Snapshot() { Key = "2", Group = "2", Type = "1", Timestamp = 1, Value = "2" };
        var snapshot6 = new Snapshot() { Key = "2", Group = "2", Type = "1", Timestamp = 1, Value = "1" };
        map.Snapshots = [snapshot, snapshot2];
        map.Arrange();
        Assert.Equal(snapshot, map.Snapshots[0]);
        Assert.Equal(snapshot2, map.Snapshots[1]);
        map.Snapshots = [snapshot2, snapshot];
        map.Arrange();
        Assert.Equal(snapshot, map.Snapshots[0]);
        Assert.Equal(snapshot2, map.Snapshots[1]);
        map.Snapshots = [snapshot, snapshot3];
        map.Arrange();
        Assert.Equal(snapshot3, map.Snapshots[0]);
        Assert.Equal(snapshot, map.Snapshots[1]);
        map.Snapshots = [snapshot4, snapshot3];
        map.Arrange();
        Assert.Equal(snapshot4, map.Snapshots[0]);
        Assert.Equal(snapshot3, map.Snapshots[1]);
        map.Snapshots = [snapshot5, snapshot2];
        map.Arrange();
        Assert.Equal(snapshot5, map.Snapshots[0]);
        Assert.Equal(snapshot2, map.Snapshots[1]);
        map.Snapshots = [snapshot5, snapshot6];
        map.Arrange();
        Assert.Equal(snapshot6, map.Snapshots[0]);
        Assert.Equal(snapshot5, map.Snapshots[1]);
    }
    private static MapInfo SuffMapInfo(string suff)
    {
        return new MapInfo()
        {
            Name = $"name{suff}",
            Desc = $"desc{suff}",
        };
    }
    [Fact]
    public void TestMapInfo()
    {
        var mapInfo = SuffMapInfo("");
        var mapInfo2 = mapInfo.Clone();
        Assert.True(mapInfo.Equal(mapInfo2));
        mapInfo = SuffMapInfo("");
        Assert.True(mapInfo.Equal(MapInfo.Decode(mapInfo.Encode())));
        mapInfo = SuffMapInfo(">:=@!;\\,&!\n");
        Assert.True(mapInfo.Equal(MapInfo.Decode(mapInfo.Encode())));
        mapInfo = SuffMapInfo("\\>\\:\\=\\@\\!\\;\\\\\\,\\&\\!\\n");
        Assert.True(mapInfo.Equal(MapInfo.Decode(mapInfo.Encode())));
    }
}