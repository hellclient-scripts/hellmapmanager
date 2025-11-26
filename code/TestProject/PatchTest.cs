using HellMapManager.Models;
using HellMapManager.Helpers;
using HellMapManager.Helpers.HMMEncoder;
using System.Text;
using HellMapManager.Helpers.HMPEncoder;
namespace TestProject;

public class PatchTest
{
    [Fact]
    public void TestRoomDiff()
    {
        var rmodel = new RemovedRoomDiff("key1");
        var rmodel2 = new RemovedRoomDiff("key2");
        Assert.Equal(DiffType.RemovedRoomDiff, rmodel.Type);
        Assert.Equal("key1", rmodel.Key);
        Assert.Null(rmodel.Data);
        Assert.Equal(RemovedRoomDiff.Decode(rmodel.Encode()).Key, rmodel.Key);
        Assert.Equal(DiffMode.Removed, rmodel.Mode);
        Assert.True(rmodel.Validated());
        Assert.NotEqual(rmodel.DiffKey, rmodel2.DiffKey);
        Assert.False(new RemovedRoomDiff("").Validated());


        var raw = new Room()
        {
            Key = "key1",
            Name = "roomname",
        };
        var raw2 = new Room()
        {
            Key = "key2",
            Name = "roomname2",
        };
        var model = new RoomDiff(raw);
        var model2 = new RoomDiff(raw2);
        Assert.Equal(DiffType.RoomDiff, model.Type);
        Assert.Equal(raw.Key, model.Key);
        Assert.NotNull(model.Data);
        Assert.True(model.Data.Equal(raw));
        Assert.True(Room.Decode(model.Encode()).Equal(raw));
        Assert.Equal(DiffMode.Normal, model.Mode);
        Assert.NotEqual(model.DiffKey, model2.DiffKey);
        Assert.True(model.Validated());
        Assert.False(new RoomDiff(new Room()).Validated());
        Assert.Equal(Room.EncodeKey, rmodel.Target);
        Assert.Equal(Room.EncodeKey, model.Target);

    }
    [Fact]
    public void TestMarkderDiff()
    {
        var rmodel = new RemovedMarkerDiff("key1");
        var rmodel2 = new RemovedMarkerDiff("key2");
        Assert.Equal(DiffType.RemovedMarkerDiff, rmodel.Type);
        Assert.Equal("key1", rmodel.Key);
        Assert.Null(rmodel.Data);
        Assert.Equal(RemovedMarkerDiff.Decode(rmodel.Encode()).Key, rmodel.Key);
        Assert.Equal(DiffMode.Removed, rmodel.Mode);
        Assert.True(rmodel.Validated());
        Assert.NotEqual(rmodel.DiffKey, rmodel2.DiffKey);
        Assert.False(new RemovedMarkerDiff("").Validated());
        var raw = new Marker()
        {
            Key = "key1",
            Value = "value1",
        };
        var raw2 = new Marker()
        {
            Key = "key2",
            Value = "value2",
        };
        var model = new MarkerDiff(raw);
        var model2 = new MarkerDiff(raw2);
        Assert.Equal(DiffType.MarkerDiff, model.Type);
        Assert.NotNull(model.Data);
        Assert.True(model.Data.Equal(raw));
        Assert.Equal(raw.Key, model.Key);
        Assert.True(Marker.Decode(model.Encode()).Equal(raw));
        Assert.Equal(DiffMode.Normal, model.Mode);
        Assert.NotEqual(model.DiffKey, model2.DiffKey);
        Assert.True(model.Validated());
        Assert.False(new MarkerDiff(new Marker()).Validated());
        Assert.Equal(Marker.EncodeKey, rmodel.Target);
        Assert.Equal(Marker.EncodeKey, model.Target);
    }
    [Fact]
    public void TestRoute()
    {
        var rmodel = new RemovedRouteDiff("key1");
        var rmodel2 = new RemovedRouteDiff("key2");
        Assert.Equal(DiffType.RemovedRouteDiff, rmodel.Type);
        Assert.Equal("key1", rmodel.Key);
        Assert.Null(rmodel.Data);
        Assert.Equal(RemovedRouteDiff.Decode(rmodel.Encode()).Key, rmodel.Key);
        Assert.Equal(DiffMode.Removed, rmodel.Mode);
        Assert.True(rmodel.Validated());
        Assert.NotEqual(rmodel.DiffKey, rmodel2.DiffKey);
        Assert.False(new RemovedRouteDiff("").Validated());
        var raw = new Route()
        {
            Key = "key1",
            Rooms = ["1"],
        };
        var raw2 = new Route()
        {
            Key = "key2",
            Rooms = ["2"],
        };
        var model = new RouteDiff(raw);
        var model2 = new RouteDiff(raw2);
        Assert.Equal(DiffType.RouteDiff, model.Type);
        Assert.NotNull(model.Data);
        Assert.True(model.Data.Equal(raw));
        Assert.Equal(raw.Key, model.Key);
        Assert.True(Route.Decode(model.Encode()).Equal(raw));
        Assert.Equal(DiffMode.Normal, model.Mode);
        Assert.NotEqual(model.DiffKey, model2.DiffKey);
        Assert.True(rmodel.Validated());
        Assert.False(new RouteDiff(new Route()).Validated());
        Assert.Equal(Route.EncodeKey, rmodel.Target);
        Assert.Equal(Route.EncodeKey, model.Target);
    }
    [Fact]
    public void TestTrace()
    {
        var rmodel = new RemovedTraceDiff("key1");
        var rmodel2 = new RemovedTraceDiff("key2");
        Assert.Equal(DiffType.RemovedTraceDiff, rmodel.Type);
        Assert.Equal("key1", rmodel.Key);
        Assert.Null(rmodel.Data);
        Assert.Equal(RemovedTraceDiff.Decode(rmodel.Encode()).Key, rmodel.Key);
        Assert.Equal(DiffMode.Removed, rmodel.Mode);
        Assert.True(rmodel.Validated());
        Assert.NotEqual(rmodel.DiffKey, rmodel2.DiffKey);
        Assert.False(new RemovedTraceDiff("").Validated());
        var raw = new Trace()
        {
            Key = "key1",
            Locations = ["1"],
        };
        var raw2 = new Trace()
        {
            Key = "key2",
            Locations = ["2"],
        };
        var model = new TraceDiff(raw);
        var model2 = new TraceDiff(raw2);
        Assert.Equal(DiffType.TraceDiff, model.Type);
        Assert.NotNull(model.Data);
        Assert.True(model.Data.Equal(raw));
        Assert.Equal(raw.Key, model.Key);
        Assert.True(Trace.Decode(model.Encode()).Equal(raw));
        Assert.Equal(DiffMode.Normal, model.Mode);
        Assert.NotEqual(model.DiffKey, model2.DiffKey);
        Assert.True(model.Validated());
        Assert.False(new TraceDiff(new Trace()).Validated());
        Assert.Equal(Trace.EncodeKey, rmodel.Target);
        Assert.Equal(Trace.EncodeKey, model.Target);
    }
    [Fact]
    public void TestRegion()
    {
        var rmodel = new RemovedRegionDiff("key1");
        var rmodel2 = new RemovedRegionDiff("key2");
        Assert.Equal(DiffType.RemovedRegionDiff, rmodel.Type);
        Assert.Equal("key1", rmodel.Key);
        Assert.Null(rmodel.Data);
        Assert.Equal(RemovedRegionDiff.Decode(rmodel.Encode()).Key, rmodel.Key);
        Assert.Equal(DiffMode.Removed, rmodel.Mode);
        Assert.True(rmodel.Validated());
        Assert.NotEqual(rmodel.DiffKey, rmodel2.DiffKey);
        Assert.False(new RemovedRegionDiff("").Validated());
        var raw = new Region()
        {
            Key = "key1",
            Items = [new RegionItem(RegionItemType.Room, "1", false)],
        };
        var raw2 = new Region()
        {
            Key = "key2",
            Items = [new RegionItem(RegionItemType.Room, "2", false)],
        };
        var model = new RegionDiff(raw);
        var model2 = new RegionDiff(raw2);
        Assert.Equal(DiffType.RegionDiff, model.Type);
        Assert.NotNull(model.Data);
        Assert.True(model.Data.Equal(raw));
        Assert.Equal(raw.Key, model.Key);
        Assert.True(Region.Decode(model.Encode()).Equal(raw));
        Assert.Equal(DiffMode.Normal, model.Mode);
        Assert.NotEqual(model.DiffKey, model2.DiffKey);
        Assert.True(model.Validated());
        Assert.False(new RegionDiff(new Region()).Validated());
        Assert.Equal(Region.EncodeKey, rmodel.Target);
        Assert.Equal(Region.EncodeKey, model.Target);
    }
    [Fact]
    public void TestLandmark()
    {
        var rmodel = new RemovedLandmarkDiff("key1", "type1");
        var rmodel2 = new RemovedLandmarkDiff("key1", "type2");
        var rmodel3 = new RemovedLandmarkDiff("key2", "type2");

        Assert.Equal(DiffType.RemovedLandmarkDiff, rmodel.Type);
        Assert.Equal("key1", rmodel.LandmarkKey);
        Assert.Equal("type1", rmodel.LandmarkType);

        Assert.Null(rmodel.Data);
        Assert.Equal(RemovedLandmarkDiff.Decode(rmodel.Encode()).LandmarkKey, rmodel.LandmarkKey);
        Assert.Equal(RemovedLandmarkDiff.Decode(rmodel.Encode()).LandmarkType, rmodel.LandmarkType);
        Assert.Equal(DiffMode.Removed, rmodel.Mode);
        Assert.True(rmodel.Validated());
        Assert.NotEqual(rmodel.DiffKey, rmodel2.DiffKey);
        Assert.NotEqual(rmodel.DiffKey, rmodel3.DiffKey);

        Assert.False(new RemovedLandmarkDiff("", "").Validated());
        Assert.False(RemovedLandmarkDiff.Decode("").Validated());

        var raw = new Landmark()
        {
            Key = "key1",
            Type = "type1",
            Value = "1",
        };
        var raw2 = new Landmark()
        {
            Key = "key2",
            Type = "type2",
            Value = "2",
        };
        var model = new LandmarkDiff(raw);
        var model2 = new LandmarkDiff(raw2);
        Assert.Equal(DiffType.LandmarkDiff, model.Type);
        Assert.NotNull(model.Data);
        Assert.True(model.Data.Equal(raw));
        Assert.Equal(raw.Key, model.LandmarkKey);
        Assert.Equal(raw.Type, model.LandmarkType);
        Assert.True(Landmark.Decode(model.Encode()).Equal(raw));
        Assert.Equal(DiffMode.Normal, model.Mode);
        Assert.NotEqual(model.DiffKey, model2.DiffKey);
        Assert.True(model.Validated());
        Assert.False(new LandmarkDiff(new Landmark()).Validated());
        Assert.Equal(Landmark.EncodeKey, rmodel.Target);
        Assert.Equal(Landmark.EncodeKey, model.Target);
    }
    [Fact]
    public void TestShortcut()
    {
        var rmodel = new RemovedShortcutDiff("key1");
        var rmodel2 = new RemovedShortcutDiff("key2");
        Assert.Equal(DiffType.RemovedShortcutDiff, rmodel.Type);
        Assert.Equal("key1", rmodel.Key);
        Assert.Null(rmodel.Data);
        Assert.Equal(RemovedShortcutDiff.Decode(rmodel.Encode()).Key, rmodel.Key);
        Assert.Equal(DiffMode.Removed, rmodel.Mode);
        Assert.True(rmodel.Validated());
        Assert.NotEqual(rmodel.DiffKey, rmodel2.DiffKey);
        Assert.False(new RemovedShortcutDiff("").Validated());
        var raw = new Shortcut()
        {
            Key = "key1",
            Command = "cmd1",
            To = "to1",
        };
        var raw2 = new Shortcut()
        {
            Key = "key2",
            Command = "cmd2",
            To = "to2",
        };
        var model = new ShortcutDiff(raw);
        var model2 = new ShortcutDiff(raw2);
        Assert.Equal(DiffType.ShortcutDiff, model.Type);
        Assert.NotNull(model.Data);
        Assert.True(model.Data.Equal(raw));
        Assert.Equal(raw.Key, model.Key);
        Assert.True(Shortcut.Decode(model.Encode()).Equal(raw));
        Assert.Equal(DiffMode.Normal, model.Mode);
        Assert.NotEqual(model.DiffKey, model2.DiffKey);
        Assert.True(model.Validated());
        Assert.False(new RegionDiff(new Region()).Validated());
        Assert.Equal(Shortcut.EncodeKey, rmodel.Target);
        Assert.Equal(Shortcut.EncodeKey, model.Target);
    }
    [Fact]
    public void TestVariable()
    {
        var rmodel = new RemovedVariableDiff("key1");
        var rmodel2 = new RemovedVariableDiff("key2");
        Assert.Equal(DiffType.RemovedVariableDiff, rmodel.Type);
        Assert.Equal("key1", rmodel.Key);
        Assert.Null(rmodel.Data);
        Assert.Equal(RemovedVariableDiff.Decode(rmodel.Encode()).Key, rmodel.Key);
        Assert.Equal(DiffMode.Removed, rmodel.Mode);
        Assert.True(rmodel.Validated());
        Assert.NotEqual(rmodel.DiffKey, rmodel2.DiffKey);
        Assert.False(new RemovedVariableDiff("").Validated());
        var raw = new Variable()
        {
            Key = "key1",
            Value = "value1"
        };
        var raw2 = new Variable()
        {
            Key = "key2",
            Value = "value2",
        };
        var model = new VariableDiff(raw);
        var model2 = new VariableDiff(raw2);
        Assert.Equal(DiffType.VariableDiff, model.Type);
        Assert.NotNull(model.Data);
        Assert.True(model.Data.Equal(raw));
        Assert.Equal(raw.Key, model.Key);
        Assert.True(Variable.Decode(model.Encode()).Equal(raw));
        Assert.Equal(DiffMode.Normal, model.Mode);
        Assert.NotEqual(model.DiffKey, model2.DiffKey);
        Assert.True(model.Validated());
        Assert.False(new VariableDiff(new Variable()).Validated());
        Assert.Equal(Variable.EncodeKey, rmodel.Target);
        Assert.Equal(Variable.EncodeKey, model.Target);
    }
    [Fact]
    public void TestSnapshot()
    {
        var rmodel = new RemovedSnapshotDiff("key1", "type1", "value1");
        var rmodel2 = new RemovedSnapshotDiff("key1", "type2", "value1");
        var rmodel3 = new RemovedSnapshotDiff("key1", "type1", "value2");

        Assert.Equal(DiffType.RemovedSnapshotDiff, rmodel.Type);
        Assert.Equal("key1", rmodel.SnapshotKey);
        Assert.Equal("type1", rmodel.SnapshotType);
        Assert.Equal("value1", rmodel.SnapshotValue);

        Assert.Null(rmodel.Data);
        Assert.Equal(RemovedSnapshotDiff.Decode(rmodel.Encode()).SnapshotKey, rmodel.SnapshotKey);
        Assert.Equal(RemovedSnapshotDiff.Decode(rmodel.Encode()).SnapshotType, rmodel.SnapshotType);
        Assert.Equal(RemovedSnapshotDiff.Decode(rmodel.Encode()).SnapshotValue, rmodel.SnapshotValue);

        Assert.Equal(DiffMode.Removed, rmodel.Mode);
        Assert.True(rmodel.Validated());
        Assert.NotEqual(rmodel.DiffKey, rmodel2.DiffKey);
        Assert.NotEqual(rmodel.DiffKey, rmodel3.DiffKey);

        Assert.False(new RemovedSnapshotDiff("", "", "").Validated());
        Assert.False(RemovedSnapshotDiff.Decode("").Validated());

        var raw = Snapshot.Create(
            "key1",
            "type1",
            "value1",
            "group1"
        );
        var raw2 = Snapshot.Create(
            "key2",
            "type2",
            "value2",
            "group2"
        );
        var model = new SnapshotDiff(raw);
        var model2 = new SnapshotDiff(raw2);
        Assert.Equal(DiffType.SnapshotDiff, model.Type);
        Assert.NotNull(model.Data);
        Assert.True(model.Data.Equal(raw));
        Assert.Equal(raw.Key, model.SnapshotKey);
        Assert.Equal(raw.Type, model.SnapshotType);
        Assert.Equal(raw.Value, model.SnapshotValue);
        Assert.True(Snapshot.Decode(model.Encode()).Equal(raw));
        Assert.Equal(DiffMode.Normal, model.Mode);
        Assert.NotEqual(model.DiffKey, model2.DiffKey);
        Assert.True(model.Validated());
        Assert.False(new SnapshotDiff(new Snapshot()).Validated());
        Assert.Equal(Snapshot.EncodeKey, rmodel.Target);
        Assert.Equal(Snapshot.EncodeKey, model.Target);
    }
    [Fact]
    public void TestDiffsArrange()
    {
        var diffs = new Diffs();
        diffs.Items.Add(new RemovedRoomDiff("r2"));
        diffs.Items.Add(new RemovedRoomDiff("r1"));
        diffs.Items.Add(new RemovedMarkerDiff("r0"));

        Assert.Equal(3, diffs.Items.Count);
        Assert.Equal("r2", diffs.Items[0].DiffKey);
        Assert.Equal("r1", diffs.Items[1].DiffKey);
        Assert.Equal("r0", diffs.Items[2].DiffKey);
        diffs.Arrange();
        Assert.Equal("r0", diffs.Items[0].DiffKey);
        Assert.Equal("r1", diffs.Items[1].DiffKey);
        Assert.Equal("r2", diffs.Items[2].DiffKey);
    }
    [Fact]
    public void TestDiff()
    {
        var mf = new MapFile();
        var mf2 = HMMEncoder.Decode(HMMEncoder.Encode(mf));
        var room = new Room()
        {
            Key = "r",
            Name = "room"
        };
        Assert.True(room.Validated());
        var room1 = new Room()
        {
            Key = "r1",
            Name = "room1"
        };
        Assert.True(room1.Validated());
        var room2 = new Room()
        {
            Key = "r2",
            Name = "room2"
        };
        Assert.True(room2.Validated());
        var room3 = room1.Clone();
        room3.Key = "r3";
        Assert.True(room3.Validated());
        var room4 = room2.Clone();
        room4.Name = "room4";
        mf.InsertRoom(room);
        mf.InsertRoom(room1);
        mf.InsertRoom(room2);
        mf2.InsertRoom(room);
        mf2.InsertRoom(room3);
        mf2.InsertRoom(room4);
        var marker = new Marker()
        {
            Key = "m",
            Value = "marker"
        };
        Assert.True(marker.Validated());
        var marker1 = new Marker()
        {
            Key = "m1",
            Value = "marker1"
        };
        Assert.True(marker1.Validated());
        var marker2 = new Marker()
        {
            Key = "m2",
            Value = "marker2"
        };
        Assert.True(marker2.Validated());
        var marker3 = marker1.Clone();
        marker3.Key = "m3";
        var marker4 = marker2.Clone();
        marker4.Value = "marker4";
        Assert.True(marker3.Validated());
        mf.InsertMarker(marker);
        mf.InsertMarker(marker1);
        mf.InsertMarker(marker2);
        mf2.InsertMarker(marker);
        mf2.InsertMarker(marker3);
        mf2.InsertMarker(marker4);

        var route = new Route()
        {
            Key = "route",
            Rooms = ["r"]
        };
        Assert.True(route.Validated());
        var route1 = new Route()
        {
            Key = "route1",
            Rooms = ["r1", "r2"]
        };
        Assert.True(route1.Validated());
        var route2 = new Route()
        {
            Key = "route2",
            Rooms = ["r2"]
        };
        Assert.True(route2.Validated());
        var route3 = route1.Clone();
        route3.Key = "route3";
        var route4 = route2.Clone();
        route4.Rooms = ["r1"];
        Assert.True(route3.Validated());
        mf.InsertRoute(route);
        mf.InsertRoute(route1);
        mf.InsertRoute(route2);
        mf2.InsertRoute(route);
        mf2.InsertRoute(route3);
        mf2.InsertRoute(route4);
        var trace = new Trace()
        {
            Key = "trace",
            Locations = ["loc1", "loc2"]
        };
        Assert.True(trace.Validated());
        var trace1 = new Trace()
        {
            Key = "trace1",
            Locations = ["loc1", "loc2"]
        };
        Assert.True(trace1.Validated());
        var trace2 = new Trace()
        {
            Key = "trace2",
            Locations = ["loc2"]
        };
        Assert.True(trace2.Validated());
        var trace3 = trace1.Clone();
        trace3.Key = "trace3";
        var trace4 = trace2.Clone();
        trace4.Locations = ["loc1"];
        Assert.True(trace3.Validated());
        mf.InsertTrace(trace);
        mf.InsertTrace(trace1);
        mf.InsertTrace(trace2);
        mf2.InsertTrace(trace);
        mf2.InsertTrace(trace3);
        mf2.InsertTrace(trace4);
        var region = new Region()
        {
            Key = "region",
            Items = [new RegionItem(RegionItemType.Room, "r", false)]
        };
        Assert.True(region.Validated());
        var region1 = new Region()
        {
            Key = "region1",
            Items = [new RegionItem(RegionItemType.Room, "r1", false)]
        };
        Assert.True(region1.Validated());
        var region2 = new Region()
        {
            Key = "region2",
            Items = [new RegionItem(RegionItemType.Room, "r2", false)]
        };
        Assert.True(region2.Validated());
        var region3 = region1.Clone();
        region3.Key = "region3";
        var region4 = region2.Clone();
        region4.Items = [new RegionItem(RegionItemType.Room, "r1", false)];
        Assert.True(region3.Validated());
        mf.InsertRegion(region);
        mf.InsertRegion(region1);
        mf.InsertRegion(region2);
        mf2.InsertRegion(region);
        mf2.InsertRegion(region3);
        mf2.InsertRegion(region4);
        var landmark = new Landmark()
        {
            Key = "landmark",
            Type = "type",
            Value = "value"
        };
        Assert.True(landmark.Validated());
        var landmark1 = new Landmark()
        {
            Key = "landmark1",
            Type = "type1",
            Value = "value1"
        };
        Assert.True(landmark1.Validated());
        var landmark2 = new Landmark()
        {
            Key = "landmark2",
            Type = "type2",
            Value = "value2"
        };
        Assert.True(landmark2.Validated());
        var landmark3 = landmark1.Clone();
        landmark3.Key = "landmark3";
        var landmark4 = landmark2.Clone();
        landmark4.Value = "value4";
        Assert.True(landmark3.Validated());
        mf.InsertLandmark(landmark);
        mf.InsertLandmark(landmark1);
        mf.InsertLandmark(landmark2);
        mf2.InsertLandmark(landmark);
        mf2.InsertLandmark(landmark3);
        mf2.InsertLandmark(landmark4);
        var shortcut = new Shortcut()
        {
            Key = "shortcut",
            Command = "cmd",
            To = "to"
        };
        Assert.True(shortcut.Validated());
        var shortcut1 = new Shortcut()
        {
            Key = "shortcut1",
            Command = "cmd1",
            To = "to1"
        };
        Assert.True(shortcut1.Validated());
        var shortcut2 = new Shortcut()
        {
            Key = "shortcut2",
            Command = "cmd2",
            To = "to2"
        };
        Assert.True(shortcut2.Validated());
        var shortcut3 = shortcut1.Clone();
        shortcut3.Key = "shortcut3";
        var shortcut4 = shortcut2.Clone();
        shortcut4.To = "to4";
        Assert.True(shortcut3.Validated());
        mf.InsertShortcut(shortcut);
        mf.InsertShortcut(shortcut1);
        mf.InsertShortcut(shortcut2);
        mf2.InsertShortcut(shortcut);
        mf2.InsertShortcut(shortcut3);
        mf2.InsertShortcut(shortcut4);
        var variable = new Variable()
        {
            Key = "variable",
            Value = "value"
        };
        Assert.True(variable.Validated());
        var variable1 = new Variable()
        {
            Key = "variable1",
            Value = "value1"
        };
        Assert.True(variable1.Validated());
        var variable2 = new Variable()
        {
            Key = "variable2",
            Value = "value2"
        };
        Assert.True(variable2.Validated());
        var variable3 = variable1.Clone();
        variable3.Key = "variable3";
        var variable4 = variable2.Clone();
        variable4.Value = "value4";
        Assert.True(variable3.Validated());
        mf.InsertVariable(variable);
        mf.InsertVariable(variable1);
        mf.InsertVariable(variable2);
        mf2.InsertVariable(variable);
        mf2.InsertVariable(variable3);
        mf2.InsertVariable(variable4);
        var snapshot = Snapshot.Create(
            "snapshot",
            "type",
            "value",
            "group"
        );
        Assert.True(snapshot.Validated());
        var snapshot1 = Snapshot.Create(
            "snapshot1",
            "type1",
            "value1",
            "group1"
        );
        Assert.True(snapshot1.Validated());
        var snapshot2 = Snapshot.Create(
            "snapshot2",
            "type2",
            "value2",
            "group2"
        );
        Assert.True(snapshot2.Validated());
        var snapshot3 = snapshot1.Clone();
        snapshot3.Key = "snapshot3";
        var snapshot4 = snapshot2.Clone();
        snapshot4.Count = snapshot4.Count + 1;
        Assert.True(snapshot3.Validated());
        mf.InsertSnapshot(snapshot);
        mf.InsertSnapshot(snapshot1);
        mf.InsertSnapshot(snapshot2);
        mf2.InsertSnapshot(snapshot);
        mf2.InsertSnapshot(snapshot3);
        mf2.InsertSnapshot(snapshot4);


        var mfdata = HMMEncoder.Encode(mf);
        Assert.Equal(HMMEncoder.Encode(mf), mfdata);
        var mf2data = HMMEncoder.Encode(mf2);
        Assert.Equal(HMMEncoder.Encode(mf2), mf2data);
        var diffs = DiffHelper.Diff(mf.Map, mf2.Map);
        Assert.Equal(27, diffs.Items.Count);
        diffs.Arrange();
        {
            var i = 0;
            Assert.True(diffs.Items[i] is RemovedLandmarkDiff);
            Assert.Equal(diffs.Items[i].DiffKey, landmark1.UniqueKey().ToString());
            i++;
            Assert.True(diffs.Items[i] is LandmarkDiff);
            var landmarkDiff = diffs.Items[i] as LandmarkDiff;
            Assert.True(landmarkDiff!.Data!.Equal(landmark4));
            i++;
            Assert.True(diffs.Items[i] is LandmarkDiff);
            var landmarkDiff2 = diffs.Items[i] as LandmarkDiff;
            Assert.True(landmarkDiff2!.Data!.Equal(landmark3));
            i++;
            Assert.True(diffs.Items[i] is RemovedMarkerDiff);
            Assert.Equal(diffs.Items[i].DiffKey, marker1.Key);
            i++;
            Assert.True(diffs.Items[i] is MarkerDiff);
            var markerDiff = diffs.Items[i] as MarkerDiff;
            Assert.True(markerDiff!.Data!.Equal(marker4));
            i++;
            Assert.True(diffs.Items[i] is MarkerDiff);
            var markerDiff2 = diffs.Items[i] as MarkerDiff;
            Assert.True(markerDiff2!.Data!.Equal(marker3));
            i++;
            Assert.True(diffs.Items[i] is RemovedRegionDiff);
            Assert.Equal(diffs.Items[i].DiffKey, region1.Key);
            i++;
            Assert.True(diffs.Items[i] is RegionDiff);
            var regionDiff = diffs.Items[i] as RegionDiff;
            Assert.True(regionDiff!.Data!.Equal(region4));
            i++;
            Assert.True(diffs.Items[i] is RegionDiff);
            var regionDiff2 = diffs.Items[i] as RegionDiff;
            Assert.True(regionDiff2!.Data!.Equal(region3));
            i++;
            Assert.True(diffs.Items[i] is RemovedRoomDiff);
            Assert.Equal(diffs.Items[i].DiffKey, room1.Key);
            i++;
            Assert.True(diffs.Items[i] is RoomDiff);
            var roomDiff = diffs.Items[i] as RoomDiff;
            Assert.True(roomDiff!.Data!.Equal(room4));
            i++;
            Assert.True(diffs.Items[i] is RoomDiff);
            var roomDiff2 = diffs.Items[i] as RoomDiff;
            Assert.True(roomDiff2!.Data!.Equal(room3));
            i++;
            Assert.True(diffs.Items[i] is RemovedRouteDiff);
            Assert.Equal(diffs.Items[i].DiffKey, route1.Key);
            i++;
            Assert.True(diffs.Items[i] is RouteDiff);
            var routeDiff = diffs.Items[i] as RouteDiff;
            Assert.True(routeDiff!.Data!.Equal(route4));
            i++;
            Assert.True(diffs.Items[i] is RouteDiff);
            var routeDiff2 = diffs.Items[i] as RouteDiff;
            Assert.True(routeDiff2!.Data!.Equal(route3));
            i++;
            Assert.True(diffs.Items[i] is RemovedShortcutDiff);
            Assert.Equal(diffs.Items[i].DiffKey, shortcut1.Key);
            i++;
            Assert.True(diffs.Items[i] is ShortcutDiff);
            var shortcutDiff = diffs.Items[i] as ShortcutDiff;
            Assert.True(shortcutDiff!.Data!.Equal(shortcut4));
            i++;
            Assert.True(diffs.Items[i] is ShortcutDiff);
            var shortcutDiff2 = diffs.Items[i] as ShortcutDiff;
            Assert.True(shortcutDiff2!.Data!.Equal(shortcut3));
            i++;
            Assert.True(diffs.Items[i] is RemovedSnapshotDiff);
            Assert.Equal(diffs.Items[i].DiffKey, snapshot1.UniqueKey().ToString());
            i++;
            Assert.True(diffs.Items[i] is SnapshotDiff);
            var snapshotDiff = diffs.Items[i] as SnapshotDiff;
            Assert.True(snapshotDiff!.Data!.Equal(snapshot4));
            i++;
            Assert.True(diffs.Items[i] is SnapshotDiff);
            var snapshotDiff2 = diffs.Items[i] as SnapshotDiff;
            Assert.True(snapshotDiff2!.Data!.Equal(snapshot3));
            i++;
            Assert.True(diffs.Items[i] is RemovedTraceDiff);
            Assert.Equal(diffs.Items[i].DiffKey, trace1.Key);
            i++;
            Assert.True(diffs.Items[i] is TraceDiff);
            var traceDiff = diffs.Items[i] as TraceDiff;
            Assert.True(traceDiff!.Data!.Equal(trace4));
            i++;
            Assert.True(diffs.Items[i] is TraceDiff);
            var traceDiff2 = diffs.Items[i] as TraceDiff;
            Assert.True(traceDiff2!.Data!.Equal(trace3));
            i++;
            Assert.True(diffs.Items[i] is RemovedVariableDiff);
            Assert.Equal(diffs.Items[i].DiffKey, variable1.Key);
            i++;
            Assert.True(diffs.Items[i] is VariableDiff);
            var variableDiff = diffs.Items[i] as VariableDiff;
            Assert.True(variableDiff!.Data!.Equal(variable4));
            i++;
            Assert.True(diffs.Items[i] is VariableDiff);
            var variableDiff2 = diffs.Items[i] as VariableDiff;
            Assert.True(variableDiff2!.Data!.Equal(variable3));
            i++;
        }
        var diffs2 = DiffHelper.Diff(mf2.Map, mf.Map);
        Assert.Equal(27, diffs2.Items.Count);
        diffs2.Arrange();
        {
            var i = 0;
            Assert.True(diffs2.Items[i] is LandmarkDiff);
            var landmarkDiff = diffs2.Items[i] as LandmarkDiff;
            Assert.True(landmarkDiff!.Data!.Equal(landmark1));
            i++;
            Assert.True(diffs2.Items[i] is LandmarkDiff);
            var landmarkDiff2 = diffs2.Items[i] as LandmarkDiff;
            Assert.True(landmarkDiff2!.Data!.Equal(landmark2));
            i++;
            Assert.True(diffs2.Items[i] is RemovedLandmarkDiff);
            Assert.Equal(diffs2.Items[i].DiffKey, landmark3.UniqueKey().ToString());
            i++;
            Assert.True(diffs2.Items[i] is MarkerDiff);
            var markerDiff = diffs2.Items[i] as MarkerDiff;
            Assert.True(markerDiff!.Data!.Equal(marker1));
            i++;
            Assert.True(diffs2.Items[i] is MarkerDiff);
            var markerDiff2 = diffs2.Items[i] as MarkerDiff;
            Assert.True(markerDiff2!.Data!.Equal(marker2));
            i++;
            Assert.True(diffs2.Items[i] is RemovedMarkerDiff);
            Assert.Equal(diffs2.Items[i].DiffKey, marker3.Key);
            i++;
            Assert.True(diffs2.Items[i] is RegionDiff);
            var regionDiff = diffs2.Items[i] as RegionDiff;
            Assert.True(regionDiff!.Data!.Equal(region1));
            i++;
            Assert.True(diffs2.Items[i] is RegionDiff);
            var regionDiff2 = diffs2.Items[i] as RegionDiff;
            Assert.True(regionDiff2!.Data!.Equal(region2));
            i++;
            Assert.True(diffs2.Items[i] is RemovedRegionDiff);
            Assert.Equal(diffs2.Items[i].DiffKey, region3.Key);
            i++;
            Assert.True(diffs2.Items[i] is RoomDiff);
            var roomDiff = diffs2.Items[i] as RoomDiff;
            Assert.True(roomDiff!.Data!.Equal(room1));
            i++;
            Assert.True(diffs2.Items[i] is RoomDiff);
            var roomDiff2 = diffs2.Items[i] as RoomDiff;
            Assert.True(roomDiff2!.Data!.Equal(room2));
            i++;
            Assert.True(diffs2.Items[i] is RemovedRoomDiff);
            Assert.Equal(diffs2.Items[i].DiffKey, room3.Key);
            i++;
            Assert.True(diffs2.Items[i] is RouteDiff);
            var routeDiff = diffs2.Items[i] as RouteDiff;
            Assert.True(routeDiff!.Data!.Equal(route1));
            i++;
            Assert.True(diffs2.Items[i] is RouteDiff);
            var routeDiff2 = diffs2.Items[i] as RouteDiff;
            Assert.True(routeDiff2!.Data!.Equal(route2));
            i++;
            Assert.True(diffs2.Items[i] is RemovedRouteDiff);
            Assert.Equal(diffs2.Items[i].DiffKey, route3.Key);
            i++;
            Assert.True(diffs2.Items[i] is ShortcutDiff);
            var shortcutDiff = diffs2.Items[i] as ShortcutDiff;
            Assert.True(shortcutDiff!.Data!.Equal(shortcut1));
            i++;
            Assert.True(diffs2.Items[i] is ShortcutDiff);
            var shortcutDiff2 = diffs2.Items[i] as ShortcutDiff;
            Assert.True(shortcutDiff2!.Data!.Equal(shortcut2));
            i++;
            Assert.True(diffs2.Items[i] is RemovedShortcutDiff);
            Assert.Equal(diffs2.Items[i].DiffKey, shortcut3.Key);
            i++;
            Assert.True(diffs2.Items[i] is SnapshotDiff);
            var snapshotDiff = diffs2.Items[i] as SnapshotDiff;
            Assert.True(snapshotDiff!.Data!.Equal(snapshot1));
            i++;
            Assert.True(diffs2.Items[i] is SnapshotDiff);
            var snapshotDiff2 = diffs2.Items[i] as SnapshotDiff;
            Assert.True(snapshotDiff2!.Data!.Equal(snapshot2));
            i++;
            Assert.True(diffs2.Items[i] is RemovedSnapshotDiff);
            Assert.Equal(diffs2.Items[i].DiffKey, snapshot3.UniqueKey().ToString());
            i++;
            Assert.True(diffs2.Items[i] is TraceDiff);
            var traceDiff = diffs2.Items[i] as TraceDiff;
            Assert.True(traceDiff!.Data!.Equal(trace1));
            i++;
            Assert.True(diffs2.Items[i] is TraceDiff);
            var traceDiff2 = diffs2.Items[i] as TraceDiff;
            Assert.True(traceDiff2!.Data!.Equal(trace2));
            i++;
            Assert.True(diffs2.Items[i] is RemovedTraceDiff);
            Assert.Equal(diffs2.Items[i].DiffKey, trace3.Key);
            i++;
            Assert.True(diffs2.Items[i] is VariableDiff);
            var variableDiff = diffs2.Items[i] as VariableDiff;
            Assert.True(variableDiff!.Data!.Equal(variable1));
            i++;
            Assert.True(diffs2.Items[i] is VariableDiff);
            var variableDiff2 = diffs2.Items[i] as VariableDiff;
            Assert.True(variableDiff2!.Data!.Equal(variable2));
            i++;
            Assert.True(diffs2.Items[i] is RemovedVariableDiff);
            Assert.Equal(diffs2.Items[i].DiffKey, variable3.Key);
            i++;
        }
        DiffHelper.Apply(diffs, mf);
        Assert.Equal(HMMEncoder.Encode(mf), mf2data);
        DiffHelper.Apply(diffs2, mf2);
        Assert.Equal(HMMEncoder.Encode(mf2), mfdata);

        var diffs3=HMPEncoder.Decode(HMPEncoder.Encode(diffs));
        Assert.Equal(diffs.Items.Count, diffs3!.Items.Count);
        for(var i=0;i<diffs.Items.Count;i++)
        {
            Assert.Equal(diffs.Items[i].Type, diffs3.Items[i].Type);
            Assert.Equal(diffs.Items[i].DiffKey, diffs3.Items[i].DiffKey);
            Assert.Equal(diffs.Items[i].Mode, diffs3.Items[i].Mode);
            Assert.Equal(diffs.Items[i].Encode(), diffs3.Items[i].Encode());
        }
        Assert.Null(HMPEncoder.Decode([]));
    }
}