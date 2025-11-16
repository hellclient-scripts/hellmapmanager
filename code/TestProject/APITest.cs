using HellMapManager.Cores;
using HellMapManager.Helpers;
using HellMapManager.Models;

namespace TestProject;

public class APITest
{
    [Fact] 
    public void TestVersion()
    {
        var mapDatabase = new MapDatabase();
        Assert.Equal(MapDatabase.Version,mapDatabase.APIVersion());
    }
    [Fact]
    public void TestAPIListOption()
    {
        var opt = new APIListOption();
        var keys = opt.Keys();
        var groups = opt.Groups();
        Assert.True(opt.IsEmpty());
        Assert.Empty(keys);
        Assert.Empty(groups);
        Assert.True(opt.Validate("key", "group"));
        opt.WithKeys(["key1", "key2"]);
        keys = opt.Keys();
        keys.Sort();
        Assert.Equal("key1 key2", string.Join(' ', keys));
        Assert.False(opt.Validate("key", "group"));
        Assert.False(opt.IsEmpty());
        opt.WithKeys(["key"]);
        keys = opt.Keys();
        keys.Sort();
        Assert.Equal("key key1 key2", string.Join(' ', keys));
        Assert.True(opt.Validate("key", "group"));
        Assert.False(opt.IsEmpty());

        opt.WithGroups(["group1", "group2"]);
        groups = opt.Groups();
        groups.Sort();
        Assert.Equal("group1 group2", string.Join(' ', groups));
        Assert.False(opt.Validate("key", "group"));
        Assert.False(opt.IsEmpty());
        opt.WithGroups(["group"]);
        groups = opt.Groups();
        groups.Sort();
        Assert.Equal("group group1 group2", string.Join(' ', groups));
        Assert.True(opt.Validate("key", "group"));
        Assert.False(opt.IsEmpty());
    }
    [Fact]
    public void TestRoomAPI()
    {
        bool updated = false;
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        List<Room> rooms = new List<Room>();
        var room1 = new Room()
        {
            Key = "key1",
            Group = "group1",
        };
        var room2 = new Room()
        {
            Key = "key2",
            Group = "",
        };
        var newroom2 = new Room()
        {
            Key = "key2",
            Group = "group2",
        };
        var room3 = new Room()
        {
            Key = "key3",
            Group = "group1",
        };
        var room4 = new Room()
        {
            Key = "key4",
            Group = "group2",
        };
        var badroom = new Room()
        {
            Key = "",
            Group = "group1",
        };
        var opt = new APIListOption();
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Empty(rooms);
        mapDatabase.APIInsertRooms([room1, room2, room3]);
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Empty(rooms);
        Assert.False(updated);
        mapDatabase.APIRemoveRooms(["key1"]);
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Empty(rooms);
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertRooms([room1, room2, room3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Equal(3, rooms.Count);
        Assert.Equal(room2, rooms[0]);
        Assert.Equal(room1, rooms[1]);
        Assert.Equal(room3, rooms[2]);
        opt.Clear().WithGroups([""]);
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Single(rooms);
        Assert.Equal(room2, rooms[0]);
        opt.Clear().WithGroups(["group1"]);
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Equal(2, rooms.Count);
        Assert.Equal(room1, rooms[0]);
        Assert.Equal(room3, rooms[1]);
        opt.Clear().WithGroups(["notfound"]);
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Empty(rooms);
        opt.Clear().WithKeys(["key2"]);
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Single(rooms);
        Assert.Equal(room2, rooms[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Empty(rooms);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Single(rooms);
        Assert.Equal(room1, rooms[0]);
        updated = false;
        opt = new APIListOption();
        mapDatabase.APIInsertRooms([]);
        Assert.False(updated);
        mapDatabase.APIInsertRooms([newroom2, room4]);
        Assert.True(updated);
        updated = false;
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Equal(4, rooms.Count);
        Assert.Equal(room1, rooms[0]);
        Assert.Equal(room3, rooms[1]);
        Assert.Equal(newroom2, rooms[2]);
        Assert.Equal(room4, rooms[3]);
        Assert.False(badroom.Validated());
        mapDatabase.APIInsertRooms([badroom]);
        Assert.True(updated);
        updated = false;
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Equal(4, rooms.Count);
        mapDatabase.APIRemoveRooms([]);
        Assert.False(updated);
        mapDatabase.APIRemoveRooms(["key1"]);
        Assert.True(updated);
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Equal(3, rooms.Count);
        Assert.Equal(room3, rooms[0]);
        Assert.Equal(newroom2, rooms[1]);
        Assert.Equal(room4, rooms[2]);
        mapDatabase.APIRemoveRooms(["key1", "key2", "key4"]);
        Assert.True(updated);
        rooms = mapDatabase.APIListRooms(opt);
        Assert.Single(rooms);
        Assert.Equal(room3, rooms[0]);
    }
    [Fact]
    public void TestMarkerAPI()
    {
        bool updated = false;
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        List<Marker> markers = new List<Marker>();
        var marker1 = new Marker()
        {
            Key = "key1",
            Value = "value1",
            Group = "group1",
        };
        var marker2 = new Marker()
        {
            Key = "key2",
            Value = "value2",
            Group = "",
        };
        var newmarker2 = new Marker()
        {
            Key = "key2",
            Value = "value2",
            Group = "group2",
        };
        var marker3 = new Marker()
        {
            Key = "key3",
            Value = "value3",
            Group = "group1",
        };
        var marker4 = new Marker()
        {
            Key = "key4",
            Value = "value4",
            Group = "group2",
        };
        var badmarker = new Marker()
        {
            Key = "badkey",
            Value = "",
            Group = "group1",
        };
        var opt = new APIListOption();
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Empty(markers);
        mapDatabase.APIInsertMarkers([marker1, marker2, marker3]);
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Empty(markers);
        Assert.False(updated);
        mapDatabase.APIRemoveMarkers(["key1"]);
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Empty(markers);
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertMarkers([marker1, marker2, marker3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Equal(3, markers.Count);
        Assert.Equal(marker2, markers[0]);
        Assert.Equal(marker1, markers[1]);
        Assert.Equal(marker3, markers[2]);
        opt.Clear().WithGroups([""]);
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Single(markers);
        Assert.Equal(marker2, markers[0]);
        opt.Clear().WithGroups(["group1"]);
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Equal(2, markers.Count);
        Assert.Equal(marker1, markers[0]);
        Assert.Equal(marker3, markers[1]);
        opt.Clear().WithGroups(["notfound"]);
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Empty(markers);
        opt.Clear().WithKeys(["key2"]);
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Single(markers);
        Assert.Equal(marker2, markers[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Empty(markers);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Single(markers);
        Assert.Equal(marker1, markers[0]);
        updated = false;
        opt = new APIListOption();
        mapDatabase.APIInsertMarkers([]);
        Assert.False(updated);
        mapDatabase.APIInsertMarkers([newmarker2, marker4]);
        Assert.True(updated);
        updated = false;
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Equal(4, markers.Count);
        Assert.Equal(marker1, markers[0]);
        Assert.Equal(marker3, markers[1]);
        Assert.Equal(newmarker2, markers[2]);
        Assert.Equal(marker4, markers[3]);
        Assert.False(badmarker.Validated());
        mapDatabase.APIInsertMarkers([badmarker]);
        Assert.True(updated);
        updated = false;
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Equal(4, markers.Count);
        mapDatabase.APIRemoveMarkers([]);
        Assert.False(updated);
        mapDatabase.APIRemoveMarkers(["key1"]);
        Assert.True(updated);
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Equal(3, markers.Count);
        Assert.Equal(marker3, markers[0]);
        Assert.Equal(newmarker2, markers[1]);
        Assert.Equal(marker4, markers[2]);
        mapDatabase.APIRemoveMarkers(["key1", "key2", "key4"]);
        Assert.True(updated);
        markers = mapDatabase.APIListMarkers(opt);
        Assert.Single(markers);
        Assert.Equal(marker3, markers[0]);
    }
    [Fact]
    public void TestRouteAPI()
    {
        bool updated = false;
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        List<Route> routes = new List<Route>();
        var route1 = new Route()
        {
            Key = "key1",
            Group = "group1",
            Desc = "desc1",
            Message = "message1",
            Rooms = ["key1", "key2"],
        };
        var route2 = new Route()
        {
            Key = "key2",
            Group = "",
            Desc = "desc2",
            Message = "message2",
            Rooms = ["key3"],
        };
        var newroute2 = new Route()
        {
            Key = "key2",
            Group = "group2",
            Desc = "desc2",
            Message = "message2",
            Rooms = ["key3"],
        };
        var route3 = new Route()
        {
            Key = "key3",
            Group = "group1",
            Desc = "desc3",
            Message = "message3",
            Rooms = ["key4"],
        };
        var route4 = new Route()
        {
            Key = "key4",
            Group = "group2",
            Desc = "desc4",
            Message = "message4",
            Rooms = ["key5"],
        };
        var badroute1 = new Route()
        {
            Key = "",
            Group = "",
            Desc = "",
            Message = "",
            Rooms = [],
        };
        var opt = new APIListOption();
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Empty(routes);
        mapDatabase.APIInsertRoutes([route1, route2, route3]);
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Empty(routes);
        Assert.False(updated);
        mapDatabase.APIRemoveRoutes(["key1"]);
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Empty(routes);
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertRoutes([route1, route2, route3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Equal(3, routes.Count);
        Assert.Equal(route2, routes[0]);
        Assert.Equal(route1, routes[1]);
        Assert.Equal(route3, routes[2]);

        opt.Clear().WithGroups([""]);
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Single(routes);
        Assert.Equal(route2, routes[0]);
        opt.Clear().WithGroups(["group1"]);
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Equal(2, routes.Count);
        Assert.Equal(route1, routes[0]);
        Assert.Equal(route3, routes[1]);
        opt.Clear().WithGroups(["notfound"]);
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Empty(routes);
        opt.Clear().WithKeys(["key2"]);
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Single(routes);
        Assert.Equal(route2, routes[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Empty(routes);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Single(routes);
        Assert.Equal(route1, routes[0]);
        updated = false;
        opt = new APIListOption();
        mapDatabase.APIInsertRoutes([]);
        Assert.False(updated);
        mapDatabase.APIInsertRoutes([newroute2, route4]);
        Assert.True(updated);
        updated = false;
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Equal(4, routes.Count);
        Assert.Equal(route1, routes[0]);
        Assert.Equal(route3, routes[1]);
        Assert.Equal(newroute2, routes[2]);
        Assert.Equal(route4, routes[3]);
        Assert.False(badroute1.Validated());
        mapDatabase.APIInsertRoutes([badroute1]);
        Assert.True(updated);
        updated = false;
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Equal(4, routes.Count);
        mapDatabase.APIRemoveRoutes([]);
        Assert.False(updated);
        mapDatabase.APIRemoveRoutes(["key1"]);
        Assert.True(updated);
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Equal(3, routes.Count);
        Assert.Equal(route3, routes[0]);
        Assert.Equal(newroute2, routes[1]);
        Assert.Equal(route4, routes[2]);
        mapDatabase.APIRemoveRoutes(["key1", "key2", "key4"]);
        Assert.True(updated);
        routes = mapDatabase.APIListRoutes(opt);
        Assert.Single(routes);
        Assert.Equal(route3, routes[0]);
    }
    [Fact]
    public void TestTraceAPI()
    {
        bool updated = false;
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        List<Trace> traces = new List<Trace>();
        var trace1 = new Trace()
        {
            Key = "key1",
            Group = "group1",
            Desc = "desc1",
            Message = "message1",
        };
        var trace2 = new Trace()
        {
            Key = "key2",
            Group = "",
            Desc = "desc2",
            Message = "message2",
        };
        var newtrace2 = new Trace()
        {
            Key = "key2",
            Group = "group2",
            Desc = "desc2",
            Message = "message2",
        };
        var trace3 = new Trace()
        {
            Key = "key3",
            Group = "group1",
            Desc = "desc3",
            Message = "message3",
        };
        var trace4 = new Trace()
        {
            Key = "key4",
            Group = "group2",
            Desc = "desc4",
            Message = "message4",
        };
        var badtrace1 = new Trace()
        {
            Key = "",
            Group = "",
            Desc = "",
            Message = "",
        };
        var opt = new APIListOption();
        traces = mapDatabase.APIListTraces(opt);
        Assert.Empty(traces);
        mapDatabase.APIInsertTraces([trace1, trace2, trace3]);
        traces = mapDatabase.APIListTraces(opt);
        Assert.Empty(traces);
        Assert.False(updated);
        mapDatabase.APIRemoveTraces(["key1"]);
        traces = mapDatabase.APIListTraces(opt);
        Assert.Empty(traces);
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertTraces([trace1, trace2, trace3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        traces = mapDatabase.APIListTraces(opt);
        Assert.Equal(3, traces.Count);
        Assert.Equal(trace2, traces[0]);
        Assert.Equal(trace1, traces[1]);
        Assert.Equal(trace3, traces[2]);
        opt.WithGroups([""]);
        traces = mapDatabase.APIListTraces(opt);
        Assert.Single(traces);
        Assert.Equal(trace2, traces[0]);
        opt.Clear().WithGroups(["group1"]);
        traces = mapDatabase.APIListTraces(opt);
        Assert.Equal(2, traces.Count);
        Assert.Equal(trace1, traces[0]);
        Assert.Equal(trace3, traces[1]);
        opt.Clear().WithGroups(["notfound"]);
        traces = mapDatabase.APIListTraces(opt);
        Assert.Empty(traces);
        opt.Clear().WithKeys(["key2"]);
        traces = mapDatabase.APIListTraces(opt);
        Assert.Single(traces);
        Assert.Equal(trace2, traces[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        traces = mapDatabase.APIListTraces(opt);
        Assert.Empty(traces);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        traces = mapDatabase.APIListTraces(opt);
        Assert.Single(traces);
        Assert.Equal(trace1, traces[0]);
        updated = false;
        opt = new APIListOption();
        mapDatabase.APIInsertTraces([]);
        Assert.False(updated);
        mapDatabase.APIInsertTraces([newtrace2, trace4]);
        Assert.True(updated);
        updated = false;
        traces = mapDatabase.APIListTraces(opt);
        Assert.Equal(4, traces.Count);
        Assert.Equal(trace1, traces[0]);
        Assert.Equal(trace3, traces[1]);
        Assert.Equal(newtrace2, traces[2]);
        Assert.Equal(trace4, traces[3]);
        Assert.False(badtrace1.Validated());
        mapDatabase.APIInsertTraces([badtrace1]);
        Assert.True(updated);
        updated = false;
        traces = mapDatabase.APIListTraces(opt);
        Assert.Equal(4, traces.Count);
        mapDatabase.APIRemoveTraces([]);
        Assert.False(updated);
        mapDatabase.APIRemoveTraces(["key1"]);
        Assert.True(updated);
        traces = mapDatabase.APIListTraces(opt);
        Assert.Equal(3, traces.Count);
        Assert.Equal(trace3, traces[0]);
        Assert.Equal(newtrace2, traces[1]);
        Assert.Equal(trace4, traces[2]);
        mapDatabase.APIRemoveTraces(["key1", "key2", "key4"]);
        Assert.True(updated);
        traces = mapDatabase.APIListTraces(opt);
        Assert.Single(traces);
        Assert.Equal(trace3, traces[0]);
    }
    [Fact]
    public void TestRegionAPI()
    {
        bool updated = false;
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        List<Region> regions = new List<Region>();
        var region1 = new Region()
        {
            Key = "key1",
            Group = "group1",
            Desc = "desc1",
            Message = "message1",
        };
        var region2 = new Region()
        {
            Key = "key2",
            Group = "",
            Desc = "desc2",
            Message = "message2",
        };
        var newregion2 = new Region()
        {
            Key = "key2",
            Group = "group2",
            Desc = "desc2",
            Message = "message2",
        };
        var region3 = new Region()
        {
            Key = "key3",
            Group = "group1",
            Desc = "desc3",
            Message = "message3",
        };
        var region4 = new Region()
        {
            Key = "key4",
            Group = "group2",
            Desc = "desc4",
            Message = "message4",
        };
        var badregion1 = new Region()
        {
            Key = "",
            Group = "",
            Desc = "",
            Message = "",
        };
        var opt = new APIListOption();
        regions = mapDatabase.APIListRegions(opt);
        Assert.Empty(regions);
        mapDatabase.APIInsertRegions([region1, region2, region3]);
        regions = mapDatabase.APIListRegions(opt);
        Assert.Empty(regions);
        Assert.False(updated);
        mapDatabase.APIRemoveRegions(["key1"]);
        regions = mapDatabase.APIListRegions(opt);
        Assert.Empty(regions);
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertRegions([region1, region2, region3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        regions = mapDatabase.APIListRegions(opt);
        Assert.Equal(3, regions.Count);
        Assert.Equal(region2, regions[0]);
        Assert.Equal(region1, regions[1]);
        Assert.Equal(region3, regions[2]);
        opt.Clear().WithGroups([""]);
        regions = mapDatabase.APIListRegions(opt);
        Assert.Single(regions);
        Assert.Equal(region2, regions[0]);
        opt.Clear().WithGroups(["group1"]);
        regions = mapDatabase.APIListRegions(opt);
        Assert.Equal(2, regions.Count);
        Assert.Equal(region1, regions[0]);
        Assert.Equal(region3, regions[1]);
        opt.Clear().WithGroups(["notfound"]);
        regions = mapDatabase.APIListRegions(opt);
        Assert.Empty(regions);
        opt.Clear().WithKeys(["key2"]);
        regions = mapDatabase.APIListRegions(opt);
        Assert.Single(regions);
        Assert.Equal(region2, regions[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        regions = mapDatabase.APIListRegions(opt);
        Assert.Empty(regions);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        regions = mapDatabase.APIListRegions(opt);
        Assert.Single(regions);
        Assert.Equal(region1, regions[0]);
        updated = false;
        opt = new APIListOption();
        mapDatabase.APIInsertRegions([]);
        Assert.False(updated);
        mapDatabase.APIInsertRegions([newregion2, region4]);
        Assert.True(updated);
        updated = false;
        regions = mapDatabase.APIListRegions(opt);
        Assert.Equal(4, regions.Count);
        Assert.Equal(region1, regions[0]);
        Assert.Equal(region3, regions[1]);
        Assert.Equal(newregion2, regions[2]);
        Assert.Equal(region4, regions[3]);
        Assert.False(badregion1.Validated());
        mapDatabase.APIInsertRegions([badregion1]);
        Assert.True(updated);
        updated = false;
        regions = mapDatabase.APIListRegions(opt);
        Assert.Equal(4, regions.Count);
        mapDatabase.APIRemoveRegions([]);
        Assert.False(updated);
        mapDatabase.APIRemoveRegions(["key1"]);
        Assert.True(updated);
        regions = mapDatabase.APIListRegions(opt);
        Assert.Equal(3, regions.Count);
        Assert.Equal(region3, regions[0]);
        Assert.Equal(newregion2, regions[1]);
        Assert.Equal(region4, regions[2]);
        mapDatabase.APIRemoveRegions(["key1", "key2", "key4"]);
        Assert.True(updated);
        regions = mapDatabase.APIListRegions(opt);
        Assert.Single(regions);
        Assert.Equal(region3, regions[0]);
    }
    [Fact]
    public void TestShortcutAPI()
    {
        bool updated = false;
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        List<Shortcut> shortcuts = new List<Shortcut>();
        var shortcut1 = new Shortcut()
        {
            Key = "key1",
            Command = "cmd1",
            Group = "group1",
            Desc = "desc1",
        };
        var shortcut2 = new Shortcut()
        {
            Key = "key2",
            Command = "cmd2",
            Group = "",
            Desc = "desc2",
        };
        var newshortcut2 = new Shortcut()
        {
            Key = "key2",
            Command = "cmd2",
            Group = "group2",
            Desc = "desc2",
        };
        var shortcut3 = new Shortcut()
        {
            Key = "key3",
            Command = "cmd3",
            Group = "group1",
            Desc = "desc3",
        };
        var shortcut4 = new Shortcut()
        {
            Key = "key4",
            Command = "cmd4",
            Group = "group2",
            Desc = "desc4",
        };
        var badshortcut1 = new Shortcut()
        {
            Key = "",
            Group = "",
            Desc = "",
        };
        var opt = new APIListOption();
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Empty(shortcuts);
        mapDatabase.APIInsertShortcuts([shortcut1, shortcut2, shortcut3]);
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Empty(shortcuts);
        Assert.False(updated);
        mapDatabase.APIRemoveShortcuts(["key1"]);
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Empty(shortcuts);
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertShortcuts([shortcut1, shortcut2, shortcut3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Equal(3, shortcuts.Count);
        Assert.Equal(shortcut2, shortcuts[0]);
        Assert.Equal(shortcut1, shortcuts[1]);
        Assert.Equal(shortcut3, shortcuts[2]);
        opt.Clear().WithGroups([""]);
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Single(shortcuts);
        Assert.Equal(shortcut2, shortcuts[0]);
        opt.Clear().WithGroups(["group1"]);
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Equal(2, shortcuts.Count);
        Assert.Equal(shortcut1, shortcuts[0]);
        Assert.Equal(shortcut3, shortcuts[1]);
        opt.Clear().WithGroups(["notfound"]);
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Empty(shortcuts);
        opt.Clear().WithKeys(["key2"]);
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Single(shortcuts);
        Assert.Equal(shortcut2, shortcuts[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Empty(shortcuts);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Single(shortcuts);
        Assert.Equal(shortcut1, shortcuts[0]);
        updated = false;
        opt = new APIListOption();
        mapDatabase.APIInsertShortcuts([]);
        Assert.False(updated);
        mapDatabase.APIInsertShortcuts([newshortcut2, shortcut4]);
        Assert.True(updated);
        updated = false;
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Equal(4, shortcuts.Count);
        Assert.Equal(shortcut1, shortcuts[0]);
        Assert.Equal(shortcut3, shortcuts[1]);
        Assert.Equal(newshortcut2, shortcuts[2]);
        Assert.Equal(shortcut4, shortcuts[3]);
        Assert.False(badshortcut1.Validated());
        mapDatabase.APIInsertShortcuts([badshortcut1]);
        Assert.True(updated);
        updated = false;
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Equal(4, shortcuts.Count);
        mapDatabase.APIRemoveShortcuts([]);
        Assert.False(updated);
        mapDatabase.APIRemoveShortcuts(["key1"]);
        Assert.True(updated);
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Equal(3, shortcuts.Count);
        Assert.Equal(shortcut3, shortcuts[0]);
        Assert.Equal(newshortcut2, shortcuts[1]);
        Assert.Equal(shortcut4, shortcuts[2]);
        mapDatabase.APIRemoveShortcuts(["key1", "key2", "key4"]);
        Assert.True(updated);
        shortcuts = mapDatabase.APIListShortcuts(opt);
        Assert.Single(shortcuts);
        Assert.Equal(shortcut3, shortcuts[0]);
    }
    [Fact]
    public void TestVariableAPI()
    {
        bool updated = false;
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        List<Variable> variables = new List<Variable>();
        var variable1 = new Variable()
        {
            Key = "key1",
            Value = "value1",
            Group = "group1",
            Desc = "desc1",
        };
        var variable2 = new Variable()
        {
            Key = "key2",
            Value = "value2",
            Group = "",
            Desc = "desc2",
        };
        var newvariable2 = new Variable()
        {
            Key = "key2",
            Value = "value2",
            Group = "group2",
            Desc = "desc2",
        };
        var variable3 = new Variable()
        {
            Key = "key3",
            Value = "value3",
            Group = "group1",
            Desc = "desc3",
        };
        var variable4 = new Variable()
        {
            Key = "key4",
            Value = "value4",
            Group = "group2",
            Desc = "desc4",
        };
        var badvariable1 = new Variable()
        {
            Key = "",
            Group = "",
            Desc = "",
            Value = "",
        };
        var opt = new APIListOption();
        variables = mapDatabase.APIListVariables(opt);
        Assert.Empty(variables);
        mapDatabase.APIInsertVariables([variable1, variable2, variable3]);
        variables = mapDatabase.APIListVariables(opt);
        Assert.Empty(variables);
        Assert.False(updated);
        mapDatabase.APIRemoveVariables(["key1"]);
        variables = mapDatabase.APIListVariables(opt);
        Assert.Empty(variables);
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertVariables([variable1, variable2, variable3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        variables = mapDatabase.APIListVariables(opt);
        Assert.Equal(3, variables.Count);
        Assert.Equal(variable2, variables[0]);
        Assert.Equal(variable1, variables[1]);
        Assert.Equal(variable3, variables[2]);
        opt.Clear().WithGroups([""]);
        variables = mapDatabase.APIListVariables(opt);
        Assert.Single(variables);
        Assert.Equal(variable2, variables[0]);
        opt.Clear().WithGroups(["group1"]);
        variables = mapDatabase.APIListVariables(opt);
        Assert.Equal(2, variables.Count);
        Assert.Equal(variable1, variables[0]);
        Assert.Equal(variable3, variables[1]);
        opt.Clear().WithGroups(["notfound"]);
        variables = mapDatabase.APIListVariables(opt);
        Assert.Empty(variables);
        opt.Clear().WithKeys(["key2"]);
        variables = mapDatabase.APIListVariables(opt);
        Assert.Single(variables);
        Assert.Equal(variable2, variables[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        variables = mapDatabase.APIListVariables(opt);
        Assert.Empty(variables);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        variables = mapDatabase.APIListVariables(opt);
        Assert.Single(variables);
        Assert.Equal(variable1, variables[0]);
        updated = false;
        opt = new APIListOption();
        mapDatabase.APIInsertVariables([]);
        Assert.False(updated);
        mapDatabase.APIInsertVariables([newvariable2, variable4]);
        Assert.True(updated);
        updated = false;
        variables = mapDatabase.APIListVariables(opt);
        Assert.Equal(4, variables.Count);
        Assert.Equal(variable1, variables[0]);
        Assert.Equal(variable3, variables[1]);
        Assert.Equal(newvariable2, variables[2]);
        Assert.Equal(variable4, variables[3]);
        Assert.False(badvariable1.Validated());
        mapDatabase.APIInsertVariables([badvariable1]);
        Assert.True(updated);
        updated = false;
        variables = mapDatabase.APIListVariables(opt);
        Assert.Equal(4, variables.Count);
        mapDatabase.APIRemoveVariables([]);
        Assert.False(updated);
        mapDatabase.APIRemoveVariables(["key1"]);
        Assert.True(updated);
        variables = mapDatabase.APIListVariables(opt);
        Assert.Equal(3, variables.Count);
        Assert.Equal(variable3, variables[0]);
        Assert.Equal(newvariable2, variables[1]);
        Assert.Equal(variable4, variables[2]);
        mapDatabase.APIRemoveVariables(["key1", "key2", "key4"]);
        Assert.True(updated);
        variables = mapDatabase.APIListVariables(opt);
        Assert.Single(variables);
        Assert.Equal(variable3, variables[0]);
    }
    [Fact]
    public void TestLandmarkAPI()
    {
        bool updated = false;
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        List<Landmark> landmarks = new List<Landmark>();
        var landmark1 = new Landmark()
        {
            Key = "key1",
            Type = "type1",
            Group = "group1",
            Desc = "desc1",
        };
        var landmark1t2 = new Landmark()
        {
            Key = "key1",
            Type = "type2",
            Group = "group1",
            Desc = "desc1",
        };
        var landmark2 = new Landmark()
        {
            Key = "key2",
            Type = "type2",
            Group = "",
            Desc = "desc2",
        };
        var newlandmark2 = new Landmark()
        {
            Key = "key2",
            Type = "type2",
            Group = "group2",
            Desc = "desc2",
        };
        var landmark3 = new Landmark()
        {
            Key = "key3",
            Type = "type1",
            Group = "group1",
            Desc = "desc3",
        };
        var landmark4 = new Landmark()
        {
            Key = "key4",
            Type = "type1",
            Group = "group2",
            Desc = "desc4",
        };
        var badlandmark1 = new Landmark()
        {
            Key = "",
            Group = "",
            Desc = "",
        };
        var opt = new APIListOption();
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Empty(landmarks);
        mapDatabase.APIInsertLandmarks([landmark1, landmark2, landmark3]);
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Empty(landmarks);
        Assert.False(updated);
        mapDatabase.APIRemoveLandmarks([landmark1.UniqueKey()]);
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Empty(landmarks);
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertLandmarks([landmark1, landmark1t2, landmark2, landmark3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Equal(4, landmarks.Count);
        Assert.Equal(landmark2, landmarks[0]);
        Assert.Equal(landmark1, landmarks[1]);
        Assert.Equal(landmark1t2, landmarks[2]);
        Assert.Equal(landmark3, landmarks[3]);
        opt.Clear().WithGroups([""]);
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Single(landmarks);
        Assert.Equal(landmark2, landmarks[0]);
        opt.Clear().WithGroups(["group1"]);
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Equal(3, landmarks.Count);
        Assert.Equal(landmark1, landmarks[0]);
        Assert.Equal(landmark1t2, landmarks[1]);
        Assert.Equal(landmark3, landmarks[2]);
        opt.Clear().WithGroups(["notfound"]);
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Empty(landmarks);
        opt.Clear().WithKeys(["key2"]);
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Single(landmarks);
        Assert.Equal(landmark2, landmarks[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Empty(landmarks);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Equal(2, landmarks.Count);
        Assert.Equal(landmark1, landmarks[0]);
        Assert.Equal(landmark1t2, landmarks[1]);
        updated = false;
        opt = new APIListOption();
        mapDatabase.APIInsertLandmarks([]);
        Assert.False(updated);
        mapDatabase.APIInsertLandmarks([newlandmark2, landmark4]);
        Assert.True(updated);
        updated = false;
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Equal(5, landmarks.Count);
        Assert.Equal(landmark1, landmarks[0]);
        Assert.Equal(landmark1t2, landmarks[1]);
        Assert.Equal(landmark3, landmarks[2]);
        Assert.Equal(newlandmark2, landmarks[3]);
        Assert.Equal(landmark4, landmarks[4]);
        Assert.False(badlandmark1.Validated());
        mapDatabase.APIInsertLandmarks([badlandmark1]);
        Assert.True(updated);
        updated = false;
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Equal(5, landmarks.Count);
        mapDatabase.APIRemoveLandmarks([]);
        Assert.False(updated);
        mapDatabase.APIRemoveLandmarks([landmark1.UniqueKey()]);
        Assert.True(updated);
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Equal(4, landmarks.Count);
        Assert.Equal(landmark1t2, landmarks[0]);
        Assert.Equal(landmark3, landmarks[1]);
        Assert.Equal(newlandmark2, landmarks[2]);
        Assert.Equal(landmark4, landmarks[3]);
        mapDatabase.APIRemoveLandmarks([landmark1.UniqueKey(), landmark1t2.UniqueKey(), landmark2.UniqueKey(), landmark4.UniqueKey()]);
        Assert.True(updated);
        landmarks = mapDatabase.APIListLandmarks(opt);
        Assert.Single(landmarks);
        Assert.Equal(landmark3, landmarks[0]);
    }
    [Fact]
    public void TestSnapshotAPI()
    {
        bool updated = false;
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        List<Snapshot> snapshots = new List<Snapshot>();
        var snapshot1 = new Snapshot()
        {
            Key = "key1",
            Value = "Value1",
            Type = "type1",
            Group = "group1",
            Timestamp = 1234567890,
        };
        var snapshot1t2 = new Snapshot()
        {
            Key = "key1",
            Value = "Value1",
            Type = "type2",
            Group = "group1",
            Timestamp = 1234567890,
        };
        var snapshot2 = new Snapshot()
        {
            Key = "key2",
            Value = "Value2",
            Type = "type2",
            Group = "",
            Timestamp = 1234567890,
        };
        var newsnapshot2 = new Snapshot()
        {
            Key = "key2",
            Value = "Value2",
            Type = "type2",
            Group = "group2",
            Timestamp = 1234567890,
        };
        var snapshot3 = new Snapshot()
        {
            Key = "key3",
            Value = "Value3",
            Type = "type1",
            Group = "group1",
            Timestamp = 1234567890,
        };
        var snapshot4 = new Snapshot()
        {
            Key = "key4",
            Value = "Value4",
            Type = "type1",
            Group = "group2",
            Timestamp = 1234567890,
        };
        var badsnapshot1 = new Snapshot()
        {
            Key = "",
            Group = "",
        };
        var opt = new APIListOption();
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Empty(snapshots);
        mapDatabase.APIInsertSnapshots([snapshot1, snapshot2, snapshot3]);
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Empty(snapshots);
        Assert.False(updated);
        mapDatabase.APIRemoveSnapshots([snapshot1.UniqueKey()]);
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Empty(snapshots);
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertSnapshots([snapshot1, snapshot1t2, snapshot2, snapshot3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Equal(4, snapshots.Count);
        Assert.Equal(snapshot2, snapshots[0]);
        Assert.Equal(snapshot1, snapshots[1]);
        Assert.Equal(snapshot1t2, snapshots[2]);
        Assert.Equal(snapshot3, snapshots[3]);
        opt.Clear().WithGroups([""]);
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Single(snapshots);
        Assert.Equal(snapshot2, snapshots[0]);
        opt.Clear().WithGroups(["group1"]);
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Equal(3, snapshots.Count);
        Assert.Equal(snapshot1, snapshots[0]);
        Assert.Equal(snapshot1t2, snapshots[1]);
        Assert.Equal(snapshot3, snapshots[2]);
        opt.Clear().WithGroups(["notfound"]);
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Empty(snapshots);
        opt.Clear().WithKeys(["key2"]);
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Single(snapshots);
        Assert.Equal(snapshot2, snapshots[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Empty(snapshots);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Equal(2, snapshots.Count);
        Assert.Equal(snapshot1, snapshots[0]);
        Assert.Equal(snapshot1t2, snapshots[1]);
        updated = false;
        opt = new APIListOption();
        mapDatabase.APIInsertSnapshots([]);
        Assert.False(updated);
        mapDatabase.APIInsertSnapshots([newsnapshot2, snapshot4]);
        Assert.True(updated);
        updated = false;
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Equal(5, snapshots.Count);
        Assert.Equal(snapshot1, snapshots[0]);
        Assert.Equal(snapshot1t2, snapshots[1]);
        Assert.Equal(snapshot3, snapshots[2]);
        Assert.Equal(newsnapshot2, snapshots[3]);
        Assert.Equal(snapshot4, snapshots[4]);
        Assert.False(badsnapshot1.Validated());
        mapDatabase.APIInsertSnapshots([badsnapshot1]);
        Assert.True(updated);
        updated = false;
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Equal(5, snapshots.Count);
        mapDatabase.APIRemoveSnapshots([]);
        Assert.False(updated);
        mapDatabase.APIRemoveSnapshots([snapshot1.UniqueKey()]);
        Assert.True(updated);
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Equal(4, snapshots.Count);
        Assert.Equal(snapshot1t2, snapshots[0]);
        Assert.Equal(snapshot3, snapshots[1]);
        Assert.Equal(newsnapshot2, snapshots[2]);
        Assert.Equal(snapshot4, snapshots[3]);
        mapDatabase.APIRemoveSnapshots([snapshot1.UniqueKey(), snapshot1t2.UniqueKey(), snapshot2.UniqueKey(), snapshot4.UniqueKey()]);
        Assert.True(updated);
        snapshots = mapDatabase.APIListSnapshots(opt);
        Assert.Single(snapshots);
        Assert.Equal(snapshot3, snapshots[0]);
    }
    [Fact]
    public void TestArrange()
    {
        var mapDatabase = new MapDatabase();
        mapDatabase.NewMap();
        var room = new Room()
        {
            Key = "room1",
            Group = "group1",
            Desc = "desc1",
            Data = [new Data("key2", "value2"), new Data("key1", "value1")],
            Tags = [new ValueTag("tag1", 0), new ValueTag("tag2", 0)],
            Exits = [new Exit(){
                Command="cmd1",
                To="to1",
                Cost=1,
                Conditions=[new ValueCondition("key1", 0,true),new ValueCondition("key2", 0,true)],
            }],
        };
        mapDatabase.APIInsertRooms([room]);
        var rooms = mapDatabase.APIListRooms(new APIListOption());
        Assert.Single(rooms);
        Assert.Equal(2, rooms[0].Data.Count);
        Assert.Equal("key1", rooms[0].Data[0].Key);
        Assert.Equal("key2", rooms[0].Data[1].Key);
        Assert.Equal(2, rooms[0].Tags.Count);
        Assert.Equal("tag1", rooms[0].Tags[0].Key);
        Assert.Equal("tag2", rooms[0].Tags[1].Key);
        Assert.Single(rooms[0].Exits);
        Assert.Equal(2, rooms[0].Exits[0].Conditions.Count);
        Assert.Equal("key1", rooms[0].Exits[0].Conditions[0].Key);
        Assert.Equal("key2", rooms[0].Exits[0].Conditions[1].Key);

        var trace = new Trace()
        {
            Key = "trace1",
            Group = "group1",
            Desc = "desc1",
            Locations = ["2", "1"],
            Message = "message1",
        };
        mapDatabase.APIInsertTraces([trace]);
        var traces = mapDatabase.APIListTraces(new APIListOption());
        Assert.Single(traces);
        Assert.Equal(2, traces[0].Locations.Count);
        Assert.Equal("1", traces[0].Locations[0]);
        Assert.Equal("2", traces[0].Locations[1]);

        var shortcut = new Shortcut()
        {
            Key = "shortcut1",
            Command = "cmd1",
            Group = "group1",
            Desc = "desc1",
            Conditions = [new ValueCondition("key2", 0, true), new ValueCondition("key1", 0, true)],
        };
        mapDatabase.APIInsertShortcuts([shortcut]);
        var shortcuts = mapDatabase.APIListShortcuts(new APIListOption());
        Assert.Single(shortcuts);
        Assert.Equal(2, shortcuts[0].Conditions.Count);
        Assert.Equal("key1", shortcuts[0].Conditions[0].Key);
        Assert.Equal("key2", shortcuts[0].Conditions[1].Key);
    }
    [Fact]
    public void TestAPIGroupRoom()
    {
        bool updated = false;
        var room = new Room()
        {
            Key = "room1",
            Group = "group1",
            Desc = "desc1",
            Data = [new Data("key2", "value2"), new Data("key1", "value1")],
            Tags = [new ValueTag("tag1", 0), new ValueTag("tag2", 0)],
            Exits = [new Exit(){
                Command="cmd1",
                To="to1",
                Cost=1,
                Conditions=[new ValueCondition("key1", 0,true),new ValueCondition("key2", 0,true)],
            }],
        };
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };

        mapDatabase.APIGroupRoom("room1", "newgroup");
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertRooms([room]);
        updated = false;
        mapDatabase.APIGroupRoom("room1", "group1");
        var rooms = mapDatabase.APIListRooms(new APIListOption().WithKeys(["room1"]));
        Assert.Single(rooms);
        Assert.Equal("group1", rooms[0].Group);
        Assert.False(updated);
        mapDatabase.APIGroupRoom("room1", "newgroup");
        rooms = mapDatabase.APIListRooms(new APIListOption().WithKeys(["room1"]));
        Assert.Single(rooms);
        Assert.Equal("newgroup", rooms[0].Group);
        Assert.True(updated);
        updated = false;
        mapDatabase.APIGroupRoom("roomnotfound", "newgroup");
        rooms = mapDatabase.APIListRooms(new APIListOption().WithKeys(["room1", "roomnotfound"]));
        Assert.Single(rooms);
        Assert.Equal("newgroup", rooms[0].Group);
        Assert.False(updated);
    }
    [Fact]
    public void TestAPISetRoomData()
    {
        bool updated = false;
        var room = new Room()
        {
            Key = "room1",
            Group = "group1",
            Desc = "desc1",
            Data = [new Data("key2", "value2"), new Data("key1", "value1")],
            Tags = [new ValueTag("tag1", 0), new ValueTag("tag2", 0)],
            Exits = [new Exit(){
                Command="cmd1",
                To="to1",
                Cost=1,
                Conditions=[new ValueCondition("key1", 0,true),new ValueCondition("key2", 0,true)],
            }],
        };
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        mapDatabase.APISetRoomData("room1", "key1", "newvalue");
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertRooms([room]);
        updated = false;
        mapDatabase.APISetRoomData("room1", "key1", "value1");
        var rooms = mapDatabase.APIListRooms(new APIListOption().WithKeys(["room1"]));
        Assert.Single(rooms);
        Assert.Equal("value1", rooms[0].GetData("key1"));
        Assert.False(updated);
        mapDatabase.APISetRoomData("room1", "key1", "newdata");
        rooms = mapDatabase.APIListRooms(new APIListOption().WithKeys(["room1"]));
        Assert.Single(rooms);
        Assert.Equal("newdata", rooms[0].GetData("key1"));
        Assert.True(updated);
        updated = false;
        mapDatabase.APISetRoomData("roomnotfound", "key1", "newdata");
        rooms = mapDatabase.APIListRooms(new APIListOption().WithKeys(["room1", "roomnotfound"]));
        Assert.Single(rooms);
        Assert.Equal("newdata", rooms[0].GetData("key1"));
        Assert.False(updated);
    }
    [Fact]
    public void TestAPITagRoom()
    {
        bool updated = false;
        var room = new Room()
        {
            Key = "room1",
            Group = "group1",
            Desc = "desc1",
            Data = [new Data("key2", "value2"), new Data("key1", "value1")],
            Tags = [new ValueTag("tag1", 1), new ValueTag("tag2", 1)],
            Exits = [new Exit(){
                Command="cmd1",
                To="to1",
                Cost=1,
                Conditions=[new ValueCondition("key1", 0,true),new ValueCondition("key2", 0,true)],
            }],
        };
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        mapDatabase.APITagRoom("room1", "tag1", 1);
        Assert.False(updated);
        mapDatabase.APITagRoom("room1", "", 1);
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertRooms([room]);
        updated = false;
        mapDatabase.APITagRoom("room1", "tag1", 1);
        var rooms = mapDatabase.APIListRooms(new APIListOption().WithKeys(["room1"]));
        Assert.Single(rooms);
        Assert.NotEmpty(rooms[0].Tags);
        Assert.Equal("tag1", rooms[0].Tags[0].Key);
        Assert.Equal(1, rooms[0].Tags[0].Value);
        Assert.False(updated);
        mapDatabase.APITagRoom("room1", "tag1", 2);
        rooms = mapDatabase.APIListRooms(new APIListOption().WithKeys(["room1"]));
        Assert.Single(rooms);
        Assert.NotEmpty(rooms[0].Tags);
        Assert.Equal("tag1", rooms[0].Tags[0].Key);
        Assert.Equal(2, rooms[0].Tags[0].Value);
        Assert.True(updated);
        updated = false;
        mapDatabase.APITagRoom("roomnotfound", "tag1", 2);
        rooms = mapDatabase.APIListRooms(new APIListOption().WithKeys(["room1", "roomnotfound"]));
        Assert.Single(rooms);
        Assert.NotEmpty(rooms[0].Tags);
        Assert.Equal("tag1", rooms[0].Tags[0].Key);
        Assert.Equal(2, rooms[0].Tags[0].Value);
        Assert.False(updated);
        updated = false;
        mapDatabase.APITagRoom("room1", "tag1", 0);
        rooms = mapDatabase.APIListRooms(new APIListOption().WithKeys(["room1"]));
        Assert.Single(rooms);
        Assert.Single(rooms[0].Tags);
        Assert.Equal("tag2", rooms[0].Tags[0].Key);
        Assert.Equal(1, rooms[0].Tags[0].Value);
        Assert.True(updated);
    }
    [Fact]
    public void TestAPITraceLocation()
    {
        bool updated = false;
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        var trace = new Trace()
        {
            Key = "trace1",
            Group = "group1",
            Desc = "desc1",
            Locations = ["1", "2"],
            Message = "message1",
        };
        mapDatabase.APITraceLocation("trace1", "3");
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APIInsertTraces([trace]);
        updated = false;
        mapDatabase.APITraceLocation("trace1", "1");
        var traces = mapDatabase.APIListTraces(new APIListOption().WithKeys(["trace1"]));
        Assert.Single(traces);
        Assert.Equal("1;2", string.Join(";", traces[0].Locations));
        Assert.False(updated);
        mapDatabase.APITraceLocation("trace1", "3");
        traces = mapDatabase.APIListTraces(new APIListOption().WithKeys(["trace1"]));
        Assert.Single(traces);
        Assert.Equal("1;2;3", string.Join(";", traces[0].Locations));
        Assert.True(updated);
        updated = false;
        mapDatabase.APITraceLocation("traceNotfound", "3");
        traces = mapDatabase.APIListTraces(new APIListOption().WithKeys(["trace1", "traceNotfound"]));
        Assert.Single(traces);
        Assert.Equal("1;2;3", string.Join(";", traces[0].Locations));
        Assert.False(updated);
        updated = false;
    }
    [Fact]
    public void TestGetVariable()
    {
        var mapDatabase = new MapDatabase();
        mapDatabase.NewMap();
        var variable = mapDatabase.APIGetVariable("key1");
        Assert.Equal("", variable);
        mapDatabase.APIInsertVariables([new (){
            Key = "key1",
            Value = "value1",
            Group = "group1",
            Desc = "desc1",
        }]);
        variable = mapDatabase.APIGetVariable("key1");
        Assert.Equal("value1", variable);
        variable = mapDatabase.APIGetVariable("keynotfound");
        Assert.Equal("", variable);
    }
    [Fact]
    public void TestAPIGetRoom()
    {
        var mapDatabase = new MapDatabase();
        var ctx = new Context().WithRooms([
            new Room()
            {
                Key = "ctx1",
                Group="ctxroom",
            },
            new Room()
            {
                Key = "ctx2",
                Group="ctxroom",
            },
        ]);
        var opt = new MapperOptions();
        var room = mapDatabase.APIGetRoom("key1", ctx, opt);
        Assert.Null(room);
        mapDatabase.NewMap();
        mapDatabase.APIInsertRooms([
            new Room()
            {
                Key = "key1"
            },
            new Room()
            {
                Key = "ctx2"
            },
        ]);
        room = mapDatabase.APIGetRoom("key1", ctx, opt);
        Assert.NotNull(room);
        Assert.Equal("key1", room.Key);
        room = mapDatabase.APIGetRoom("ctx1", ctx, opt);
        Assert.NotNull(room);
        Assert.Equal("ctx1", room.Key);
        room = mapDatabase.APIGetRoom("ctx2", ctx, opt);
        Assert.NotNull(room);
        Assert.Equal("ctx2", room.Key);
        Assert.Equal("ctxroom", room.Group);
    }
    [Fact]
    public void TestAPISnapshotALL()
    {
        bool updated = false;
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
        {
            updated = true;
        };
        mapDatabase.APIClearSnapshot(new SnapshotFilter(null, null, null));
        Assert.False(updated);
        mapDatabase.APITakeSnapshot("key1", "value1", "type1", "group1");
        Assert.False(updated);
        var snapshots = mapDatabase.APISearchSnapshots(new SnapshotSearch());
        Assert.False(updated);
        mapDatabase.NewMap();
        mapDatabase.APITakeSnapshot("key1", "value1", "type1", "group1");
        Assert.True(updated);
        updated = false;
        snapshots = mapDatabase.APISearchSnapshots(new SnapshotSearch());
        Assert.Single(snapshots);
        Assert.Equal("key1", snapshots[0].Key);
        Assert.Equal(1, snapshots[0].Sum);
        mapDatabase.APITakeSnapshot("key1", "value1", "type1", "group1");
        snapshots = mapDatabase.APISearchSnapshots(new SnapshotSearch());
        Assert.Single(snapshots);
        Assert.Equal("key1", snapshots[0].Key);
        Assert.Equal(2, snapshots[0].Sum);
        mapDatabase.APIClearSnapshot(new SnapshotFilter(null, null, null));
        Assert.True(updated);
        updated = false;
        snapshots = mapDatabase.APISearchSnapshots(new SnapshotSearch());
        Assert.Empty(snapshots);
    }
    [Fact]
    public void TestAPIRoomsAll()
    {
        var mapDatabase = new MapDatabase();
        var result = mapDatabase.APISearchRooms(new RoomFilter());
        Assert.Empty(result);
        result = mapDatabase.APIFilterRooms(["key1", "key2", "key3"], new RoomFilter());
        Assert.Empty(result);
        mapDatabase.NewMap();
        mapDatabase.APIInsertRooms([
            new Room()
            {
                Key = "key1",
                Group = "group1",
                Desc = "desc1",
            },
            new Room()
            {
                Key = "key2",
                Group = "group2",
                Desc = "desc2",
            },
            new Room()
            {
                Key = "key3",
                Group = "group3",
                Desc = "desc3",
            },
        ]);
        result = mapDatabase.APIFilterRooms(["key1", "key2", "key3"], new RoomFilter());
        Assert.Equal(3, result.Count);
        result.Sort((a, b) => a.Key.CompareTo(b.Key));
        Assert.Equal("key1;key2;key3", string.Join(";", result.Select(r => r.Key)));
        result = mapDatabase.APIFilterRooms(["key3", "key2", "key2", "key5"], new RoomFilter());
        Assert.Equal(2, result.Count);
        result.Sort((a, b) => a.Key.CompareTo(b.Key));
        Assert.Equal("key2;key3", string.Join(";", result.Select(r => r.Key)));
        var rf = new RoomFilter()
        {
            ContainsAnyKey = ["key1", "key2", "key3"],
        };
        result = mapDatabase.APISearchRooms(rf);
        Assert.Equal(3, result.Count);
        result.Sort((a, b) => a.Key.CompareTo(b.Key));
        Assert.Equal("key1;key2;key3", string.Join(";", result.Select(r => r.Key)));
    }
    [Fact]
    public void TestAPIQueryRegionRooms()
    {
        var mapDatabase = new MapDatabase();
        var result = mapDatabase.APIQueryRegionRooms("key");
        Assert.Empty(result);
        mapDatabase.NewMap();
        mapDatabase.APIInsertRooms([
            new Room()
            {
                Key = "key1",
                Group = "group2",
                Desc = "desc1",
            },
            new Room()
            {
                Key = "key2",
                Group = "group2",
                Desc = "desc2",
            },
            new Room()
            {
                Key = "key3",
                Group = "group3",
                Desc = "desc3",
            },
        ]);
        mapDatabase.APIInsertRegions([
            new Region()
            {
                Key = "key1",
                Items = [
                    new RegionItem(RegionItemType.Room, "key1",false),
                    new RegionItem(RegionItemType.Zone, "group3",false),
                ],
            },
            new Region()
            {
                Key = "key2",
                Items = [
                    new RegionItem(RegionItemType.Room, "notfoundkey",false),
                    new RegionItem(RegionItemType.Zone, "notfoundzone",false),
                ],
            },
            new Region()
            {
                Key = "key3",
                Items = [
                    new RegionItem(RegionItemType.Room, "key1",false),
                    new RegionItem(RegionItemType.Room, "key2",false),
                    new RegionItem(RegionItemType.Room, "key3",false),
                    new RegionItem(RegionItemType.Zone, "group2",true),
                ],
            },
            new Region()
            {
                Key = "key4",
                Items = [
                    new RegionItem(RegionItemType.Zone, "group2",false),
                    new RegionItem(RegionItemType.Room, "key1",true),
                ],
            },
            new Region()
            {
                Key = "key5",
                Items = [
                    new RegionItem(RegionItemType.Zone, "group2",false),
                    new RegionItem(RegionItemType.Room, "key1",true),
                    new RegionItem(RegionItemType.Zone, "group3",false),
                    new RegionItem(RegionItemType.Room, "key2",false),
                    new RegionItem(RegionItemType.Zone, "group2",true),
                ],
            },

        ]);
        result = mapDatabase.APIQueryRegionRooms("notfound");
        Assert.Empty(result);
        result = mapDatabase.APIQueryRegionRooms("key1");
        Assert.Equal("key1;key3", string.Join(";", result));
        result = mapDatabase.APIQueryRegionRooms("key2");
        Assert.Empty(result);
        result = mapDatabase.APIQueryRegionRooms("key3");
        Assert.Equal("key3", string.Join(";", result));
        result = mapDatabase.APIQueryRegionRooms("key4");
        Assert.Equal("key2", string.Join(";", result));
        result = mapDatabase.APIQueryRegionRooms("key5");
        Assert.Equal("key3", string.Join(";", result));
    }
    [Fact]
    public void TestAPIGetRoomExits()
    {
        var mapDatabase = new MapDatabase();
        var ctx = new Context();
        var opt = new MapperOptions();
        var result = mapDatabase.APIGetRoomExits("key", ctx, opt);
        Assert.Empty(result);
        mapDatabase.NewMap();
        var exit1 = new Exit()
        {
            Command = "cmd1",
            To = "key2",
        };
        var exit2 = new Exit()
        {
            Command = "cmd2",
            To = "key2",
        };
        mapDatabase.APIInsertRooms([
            new Room()
            {
                Key = "key1",
                Exits = [exit1,exit2]
            },
            new Room()
            {
                Key = "key2",
            },
        ]);
        result = mapDatabase.APIGetRoomExits("notfound", ctx, opt);
        Assert.Empty(result);
        result = mapDatabase.APIGetRoomExits("key1", ctx, opt);
        Assert.Equal(2, result.Count);
        Assert.Equal(exit1, result[0]);
        Assert.Equal(exit2, result[1]);
        var shortcut1 = new Shortcut()
        {
            Key = "shortcut1",
            To = "key2",
            Command = "sc1",
        };
        mapDatabase.APIInsertShortcuts([shortcut1]);
        result = mapDatabase.APIGetRoomExits("key1", ctx, opt);
        Assert.Equal(3, result.Count);
        Assert.Equal(exit1, result[0]);
        Assert.Equal(exit2, result[1]);
        Assert.Equal(shortcut1, result[2]);
        var path1 = new HellMapManager.Models.Path()
        {
            From = "key1",
            Command = "cmdp1",
            To = "key2",
        };
        var shortcut2 = new Shortcut()
        {
            Key = "shortcut2",
            To = "key2",
            Command = "sc2",
        };
        ctx.WithPaths([path1]);
        ctx.WithShortcuts([shortcut2]);
        result = mapDatabase.APIGetRoomExits("key1", ctx, opt);
        Assert.Equal(5, result.Count);
        Assert.Equal(exit1, result[0]);
        Assert.Equal(exit2, result[1]);
        Assert.Equal(path1, result[2]);
        Assert.Equal(shortcut1, result[3]);
        Assert.Equal(shortcut2, result[4]);
        opt.WithDisableShortcuts(true);
        result = mapDatabase.APIGetRoomExits("key1", ctx, opt);
        Assert.Equal(3, result.Count);
        Assert.Equal(exit1, result[0]);
        Assert.Equal(exit2, result[1]);
        Assert.Equal(path1, result[2]);
    }
}
