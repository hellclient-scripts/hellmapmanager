using HellMapManager.Models;
using HellMapManager.Helpers.HMMEncoder;

namespace TestProject;

public class HMMTest
{
    [Fact]
    public void TestEncoder()
    {
        var mf = MapFile.Create("testname", "testdesc");
        var room = new Room()
        {
            Key = "testroom世界",
        };
        var marker = new Marker()
        {
            Key = "testmarker世界",
            Value = "value"
        };
        var route = new Route()
        {
            Key = "testroute世界",
        };
        var trace = new Trace()
        {
            Key = "testtrace世界",
        };
        var region = new Region()
        {
            Key = "testregion世界",
        };
        var Landmark = new Landmark()
        {
            Key = "testlandmark世界",
        };
        var variable = new Variable()
        {
            Key = "testvariable世界",
        };
        var shortcut = new Shortcut()
        {
            Key = "testshortcut世界",
            Command = "to",
        };
        var snapshot = new Snapshot()
        {
            Key = "testsnapshot世界",
            Timestamp = 1,
        };
        mf.InsertRoom(room);
        mf.InsertMarker(marker);
        mf.InsertRoute(route);
        mf.InsertTrace(trace);
        mf.InsertRegion(region);
        mf.InsertLandmark(Landmark);
        mf.InsertVariable(variable);
        mf.InsertShortcut(shortcut);
        mf.InsertSnapshot(snapshot);

        var data = HMMEncoder.Encode(mf);
        var mf2 = HMMEncoder.Decode(data);
        Assert.NotNull(mf2);
        Assert.Equal(mf.Map.Info.Name, mf2.Map.Info.Name);
        Assert.Equal(mf.Map.Info.Desc, mf2.Map.Info.Desc);
        Assert.Equal(mf.Map.Rooms.Count, mf2.Map.Rooms.Count);
        Assert.Equal(mf.Map.Markers.Count, mf2.Map.Markers.Count);
        Assert.Equal(mf.Map.Routes.Count, mf2.Map.Routes.Count);
        Assert.Equal(mf.Map.Traces.Count, mf2.Map.Traces.Count);
        Assert.Equal(mf.Map.Regions.Count, mf2.Map.Regions.Count);
        Assert.Equal(mf.Map.Landmarks.Count, mf2.Map.Landmarks.Count);
        Assert.Equal(mf.Map.Variables.Count, mf2.Map.Variables.Count);
        Assert.Equal(mf.Map.Shortcuts.Count, mf2.Map.Shortcuts.Count);
        Assert.Equal(mf.Map.Snapshots.Count, mf2.Map.Snapshots.Count);
        Assert.True(mf.Map.Rooms[0].Equal(mf2.Map.Rooms[0]));
        Assert.True(mf.Map.Markers[0].Equal(mf2.Map.Markers[0]));
        Assert.True(mf.Map.Routes[0].Equal(mf2.Map.Routes[0]));
        Assert.True(mf.Map.Traces[0].Equal(mf2.Map.Traces[0]));
        Assert.True(mf.Map.Regions[0].Equal(mf2.Map.Regions[0]));
        Assert.True(mf.Map.Landmarks[0].Equal(mf2.Map.Landmarks[0]));
        Assert.True(mf.Map.Variables[0].Equal(mf2.Map.Variables[0]));
        Assert.True(mf.Map.Shortcuts[0].Equal(mf2.Map.Shortcuts[0]));
        Assert.True(mf.Map.Snapshots[0].Equal(mf2.Map.Snapshots[0]));
        Assert.Equal(mf.Map.Rooms[0].Key, mf2.Map.Rooms[0].Key);
        mf.Map.Encoding = MapEncoding.GB18030;
        var data2 = HMMEncoder.Encode(mf);
        Assert.NotEqual(data, data2);
        mf2 = HMMEncoder.Decode(data2);
        Assert.NotNull(mf2);
        Assert.Equal(mf.Map.Info.Name, mf2.Map.Info.Name);
        Assert.Equal(mf.Map.Info.Desc, mf2.Map.Info.Desc);
        Assert.Equal(mf.Map.Rooms.Count, mf2.Map.Rooms.Count);
        Assert.Equal(mf.Map.Markers.Count, mf2.Map.Markers.Count);
        Assert.Equal(mf.Map.Routes.Count, mf2.Map.Routes.Count);
        Assert.Equal(mf.Map.Traces.Count, mf2.Map.Traces.Count);
        Assert.Equal(mf.Map.Regions.Count, mf2.Map.Regions.Count);
        Assert.Equal(mf.Map.Landmarks.Count, mf2.Map.Landmarks.Count);
        Assert.Equal(mf.Map.Variables.Count, mf2.Map.Variables.Count);
        Assert.Equal(mf.Map.Shortcuts.Count, mf2.Map.Shortcuts.Count);
        Assert.Equal(mf.Map.Snapshots.Count, mf2.Map.Snapshots.Count);
        Assert.True(mf.Map.Rooms[0].Equal(mf2.Map.Rooms[0]));
        Assert.True(mf.Map.Markers[0].Equal(mf2.Map.Markers[0]));
        Assert.True(mf.Map.Routes[0].Equal(mf2.Map.Routes[0]));
        Assert.True(mf.Map.Traces[0].Equal(mf2.Map.Traces[0]));
        Assert.True(mf.Map.Regions[0].Equal(mf2.Map.Regions[0]));
        Assert.True(mf.Map.Landmarks[0].Equal(mf2.Map.Landmarks[0]));
        Assert.True(mf.Map.Variables[0].Equal(mf2.Map.Variables[0]));
        Assert.True(mf.Map.Shortcuts[0].Equal(mf2.Map.Shortcuts[0]));
        Assert.True(mf.Map.Snapshots[0].Equal(mf2.Map.Snapshots[0]));
        Assert.Equal(mf.Map.Rooms[0].Key, mf2.Map.Rooms[0].Key);
        Assert.Equal(MapEncoding.GB18030, mf2.Map.Encoding);

        mf2= HMMEncoder.Decode(Array.Empty<byte>());
        Assert.Null(mf2);
        var bytes= new byte[3]{ 0x01, 0x02, 0x03 };
        mf2= HMMEncoder.Decode(bytes);
        Assert.Null(mf2);
    }

}
