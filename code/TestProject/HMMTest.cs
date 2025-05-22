using HellMapManager.Models;
using HellMapManager.Helpers.HMMEncoder;
using System.Security.Cryptography.X509Certificates;

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

        mf2 = HMMEncoder.Decode(Array.Empty<byte>());
        Assert.Null(mf2);
        var bytes = new byte[3] { 0x01, 0x02, 0x03 };
        mf2 = HMMEncoder.Decode(bytes);
        Assert.Null(mf2);
    }
    private static Room? TestEncodeRoomHook(Room room)
    {
        if (room.Key == "empty")
        {
            return null;
        }
        var model = room.Clone();
        model.Key = room.Key + "-encoded";
        return model;
    }
    private static Shortcut? TestEncodeShortcutHook(Shortcut shortcut)
    {
        if (shortcut.Key == "empty")
        {
            return null;
        }
        var model = shortcut.Clone();
        model.Key = shortcut.Key + "-encoded";
        return model;
    }
    private static Room? TestDecodeRoomHook(Room room)
    {
        if (room.Key == "empty")
        {
            return null;
        }
        var model = room.Clone();
        model.Key = room.Key + "-decoded";
        return model;
    }
    private static Shortcut? TestDecodeShortcutHook(Shortcut shortcut)
    {
        if (shortcut.Key == "empty")
        {
            return null;
        }
        var model = shortcut.Clone();
        model.Key = shortcut.Key + "-decoded";
        return model;
    }
    [Fact]
    public void TestHook()
    {
        var mf = MapFile.Create("testname", "testdesc");
        var room = new Room()
        {
            Key = "empty",
        };
        var room2 = new Room()
        {
            Key = "key1",
        };
        var shortcut = new Shortcut()
        {
            Key = "empty",
            Command = "to",
        };
        var shortcut2 = new Shortcut()
        {
            Key = "key1",
            Command = "to2",
        };
        mf.InsertRoom(room);
        mf.InsertRoom(room2);
        mf.InsertShortcut(shortcut);
        mf.InsertShortcut(shortcut2);
        var rawdata = HMMEncoder.Encode(mf);
        HMMEncoder.EncodeRoomHook = TestEncodeRoomHook;
        HMMEncoder.EncodeShortcutHook = TestEncodeShortcutHook;
        var data = HMMEncoder.Encode(mf);
        var mf2 = HMMEncoder.Decode(data);
        Assert.NotNull(mf2);
        Assert.Single(mf2.Map.Rooms);
        Assert.Equal("key1-encoded", mf2.Map.Rooms[0].Key);
        Assert.Single(mf2.Map.Shortcuts);
        Assert.Equal("key1-encoded", mf2.Map.Shortcuts[0].Key);
        HMMEncoder.DecodeRoomHook = TestDecodeRoomHook;
        HMMEncoder.DecodeShortcutHook = TestDecodeShortcutHook;
        var mf3 = HMMEncoder.Decode(rawdata);
        Assert.NotNull(mf3);
        Assert.Single(mf3.Map.Rooms);
        Assert.Equal("key1-decoded", mf3.Map.Rooms[0].Key);
        Assert.Single(mf2.Map.Shortcuts);
        Assert.Equal("key1-decoded", mf3.Map.Shortcuts[0].Key);
        Assert.Equal(TestEncodeRoomHook, HMMEncoder.EncodeRoomHook);
        Assert.Equal(TestEncodeShortcutHook, HMMEncoder.EncodeShortcutHook);
        Assert.Equal(TestDecodeRoomHook, HMMEncoder.DecodeRoomHook);
        Assert.Equal(TestDecodeShortcutHook, HMMEncoder.DecodeShortcutHook);
        HMMEncoder.ResetHooks();
        Assert.Equal(DefaultHmmEncoderHooks.RoomHook, HMMEncoder.EncodeRoomHook);
        Assert.Equal(DefaultHmmEncoderHooks.ShortcutHook, HMMEncoder.EncodeShortcutHook);
        Assert.Equal(DefaultHmmEncoderHooks.RoomHook, HMMEncoder.DecodeRoomHook);
        Assert.Equal(DefaultHmmEncoderHooks.ShortcutHook, HMMEncoder.DecodeShortcutHook);
        var mf4 = HMMEncoder.Decode(rawdata);
        Assert.NotNull(mf4);
        Assert.Equal(2, mf4.Map.Rooms.Count);
        Assert.True(mf.Map.Rooms[0].Equal(mf4.Map.Rooms[0]));
        Assert.True(mf.Map.Rooms[1].Equal(mf4.Map.Rooms[1]));
        Assert.Equal(2, mf4.Map.Shortcuts.Count);
        Assert.True(mf.Map.Shortcuts[0].Equal(mf4.Map.Shortcuts[0]));
        Assert.True(mf.Map.Shortcuts[1].Equal(mf4.Map.Shortcuts[1]));
    }
}
