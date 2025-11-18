using HellMapManager.Models;

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
    }
}