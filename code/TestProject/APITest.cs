using HellMapManager.States;
using HellMapManager.Models;

namespace TestProject;
public class APITest
{
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
        var appState = new AppState();
        appState.MapFileUpdatedEvent += (sender, e) =>
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
        rooms = appState.APIListRooms(opt);
        Assert.Empty(rooms);
        appState.APIInsertRooms([room1, room2, room3]);
        rooms = appState.APIListRooms(opt);
        Assert.Empty(rooms);
        Assert.False(updated);
        appState.APIRemoveRooms(["key1"]);
        rooms = appState.APIListRooms(opt);
        Assert.Empty(rooms);
        Assert.False(updated);
        appState.NewMap();
        appState.APIInsertRooms([room1, room2, room3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        rooms = appState.APIListRooms(opt);
        Assert.Equal(3, rooms.Count);
        Assert.Equal(room2, rooms[0]);
        Assert.Equal(room1, rooms[1]);
        Assert.Equal(room3, rooms[2]);
        opt.Clear().WithGroups([""]);
        rooms = appState.APIListRooms(opt);
        Assert.Single(rooms);
        Assert.Equal(room2, rooms[0]);
        opt.Clear().WithGroups(["group1"]);
        rooms = appState.APIListRooms(opt);
        Assert.Equal(2, rooms.Count);
        Assert.Equal(room1, rooms[0]);
        Assert.Equal(room3, rooms[1]);
        opt.Clear().WithGroups(["notfound"]);
        rooms = appState.APIListRooms(opt);
        Assert.Empty(rooms);
        opt.Clear().WithKeys(["key2"]);
        rooms = appState.APIListRooms(opt);
        Assert.Single(rooms);
        Assert.Equal(room2, rooms[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        rooms = appState.APIListRooms(opt);
        Assert.Empty(rooms);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        rooms = appState.APIListRooms(opt);
        Assert.Single(rooms);
        Assert.Equal(room1, rooms[0]);
        updated = false;
        opt = new APIListOption();
        appState.APIInsertRooms([]);
        Assert.False(updated);
        appState.APIInsertRooms([newroom2, room4]);
        Assert.True(updated);
        updated = false;
        rooms = appState.APIListRooms(opt);
        Assert.Equal(4, rooms.Count);
        Assert.Equal(room1, rooms[0]);
        Assert.Equal(room3, rooms[1]);
        Assert.Equal(newroom2, rooms[2]);
        Assert.Equal(room4, rooms[3]);
        Assert.False(badroom.Validated());
        appState.APIInsertRooms([badroom]);
        Assert.True(updated);
        updated = false;
        rooms = appState.APIListRooms(opt);
        Assert.Equal(4, rooms.Count);
        appState.APIRemoveRooms([]);
        Assert.False(updated);
        appState.APIRemoveRooms(["key1"]);
        Assert.True(updated);
        rooms = appState.APIListRooms(opt);
        Assert.Equal(3, rooms.Count);
        Assert.Equal(room3, rooms[0]);
        Assert.Equal(newroom2, rooms[1]);
        Assert.Equal(room4, rooms[2]);
        appState.APIRemoveRooms(["key1", "key2", "key4"]);
        Assert.True(updated);
        rooms = appState.APIListRooms(opt);
        Assert.Single(rooms);
        Assert.Equal(room3, rooms[0]);
    }
    [Fact]
    public void TestMarkerAPI()
    {
        bool updated = false;
        var appState = new AppState();
        appState.MapFileUpdatedEvent += (sender, e) =>
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
        markers = appState.APIListMarkers(opt);
        Assert.Empty(markers);
        appState.APIInsertMarkers([marker1, marker2, marker3]);
        markers = appState.APIListMarkers(opt);
        Assert.Empty(markers);
        Assert.False(updated);
        appState.APIRemoveMarkers(["key1"]);
        markers = appState.APIListMarkers(opt);
        Assert.Empty(markers);
        Assert.False(updated);
        appState.NewMap();
        appState.APIInsertMarkers([marker1, marker2, marker3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        markers = appState.APIListMarkers(opt);
        Assert.Equal(3, markers.Count);
        Assert.Equal(marker2, markers[0]);
        Assert.Equal(marker1, markers[1]);
        Assert.Equal(marker3, markers[2]);
        opt.Clear().WithGroups([""]);
        markers = appState.APIListMarkers(opt);
        Assert.Single(markers);
        Assert.Equal(marker2, markers[0]);
        opt.Clear().WithGroups(["group1"]);
        markers = appState.APIListMarkers(opt);
        Assert.Equal(2, markers.Count);
        Assert.Equal(marker1, markers[0]);
        Assert.Equal(marker3, markers[1]);
        opt.Clear().WithGroups(["notfound"]);
        markers = appState.APIListMarkers(opt);
        Assert.Empty(markers);
        opt.Clear().WithKeys(["key2"]);
        markers = appState.APIListMarkers(opt);
        Assert.Single(markers);
        Assert.Equal(marker2, markers[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        markers = appState.APIListMarkers(opt);
        Assert.Empty(markers);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        markers = appState.APIListMarkers(opt);
        Assert.Single(markers);
        Assert.Equal(marker1, markers[0]);
        updated = false;
        opt = new APIListOption();
        appState.APIInsertMarkers([]);
        Assert.False(updated);
        appState.APIInsertMarkers([newmarker2, marker4]);
        Assert.True(updated);
        updated = false;
        markers = appState.APIListMarkers(opt);
        Assert.Equal(4, markers.Count);
        Assert.Equal(marker1, markers[0]);
        Assert.Equal(marker3, markers[1]);
        Assert.Equal(newmarker2, markers[2]);
        Assert.Equal(marker4, markers[3]);
        Assert.False(badmarker.Validated());
        appState.APIInsertMarkers([badmarker]);
        Assert.True(updated);
        updated = false;
        markers = appState.APIListMarkers(opt);
        Assert.Equal(4, markers.Count);
        appState.APIRemoveMarkers([]);
        Assert.False(updated);
        appState.APIRemoveMarkers(["key1"]);
        Assert.True(updated);
        markers = appState.APIListMarkers(opt);
        Assert.Equal(3, markers.Count);
        Assert.Equal(marker3, markers[0]);
        Assert.Equal(newmarker2, markers[1]);
        Assert.Equal(marker4, markers[2]);
        appState.APIRemoveMarkers(["key1", "key2", "key4"]);
        Assert.True(updated);
        markers = appState.APIListMarkers(opt);
        Assert.Single(markers);
        Assert.Equal(marker3, markers[0]);
    }
    [Fact]
    public void TestRouteAPI()
    {
        bool updated = false;
        var appState = new AppState();
        appState.MapFileUpdatedEvent += (sender, e) =>
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
        routes = appState.APIListRoutes(opt);
        Assert.Empty(routes);
        appState.APIInsertRoutes([route1, route2, route3]);
        routes = appState.APIListRoutes(opt);
        Assert.Empty(routes);
        Assert.False(updated);
        appState.APIRemoveRoutes(["key1"]);
        routes = appState.APIListRoutes(opt);
        Assert.Empty(routes);
        Assert.False(updated);
        appState.NewMap();
        appState.APIInsertRoutes([route1, route2, route3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        routes = appState.APIListRoutes(opt);
        Assert.Equal(3, routes.Count);
        Assert.Equal(route2, routes[0]);
        Assert.Equal(route1, routes[1]);
        Assert.Equal(route3, routes[2]);

        opt.Clear().WithGroups([""]);
        routes = appState.APIListRoutes(opt);
        Assert.Single(routes);
        Assert.Equal(route2, routes[0]);
        opt.Clear().WithGroups(["group1"]);
        routes = appState.APIListRoutes(opt);
        Assert.Equal(2, routes.Count);
        Assert.Equal(route1, routes[0]);
        Assert.Equal(route3, routes[1]);
        opt.Clear().WithGroups(["notfound"]);
        routes = appState.APIListRoutes(opt);
        Assert.Empty(routes);
        opt.Clear().WithKeys(["key2"]);
        routes = appState.APIListRoutes(opt);
        Assert.Single(routes);
        Assert.Equal(route2, routes[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        routes = appState.APIListRoutes(opt);
        Assert.Empty(routes);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        routes = appState.APIListRoutes(opt);
        Assert.Single(routes);
        Assert.Equal(route1, routes[0]);
        updated = false;
        opt = new APIListOption();
        appState.APIInsertRoutes([]);
        Assert.False(updated);
        appState.APIInsertRoutes([newroute2, route4]);
        Assert.True(updated);
        updated = false;
        routes = appState.APIListRoutes(opt);
        Assert.Equal(4, routes.Count);
        Assert.Equal(route1, routes[0]);
        Assert.Equal(route3, routes[1]);
        Assert.Equal(newroute2, routes[2]);
        Assert.Equal(route4, routes[3]);
        Assert.False(badroute1.Validated());
        appState.APIInsertRoutes([badroute1]);
        Assert.True(updated);
        updated = false;
        routes = appState.APIListRoutes(opt);
        Assert.Equal(4, routes.Count);
        appState.APIRemoveRoutes([]);
        Assert.False(updated);
        appState.APIRemoveRoutes(["key1"]);
        Assert.True(updated);
        routes = appState.APIListRoutes(opt);
        Assert.Equal(3, routes.Count);
        Assert.Equal(route3, routes[0]);
        Assert.Equal(newroute2, routes[1]);
        Assert.Equal(route4, routes[2]);
        appState.APIRemoveRoutes(["key1", "key2", "key4"]);
        Assert.True(updated);
        routes = appState.APIListRoutes(opt);
        Assert.Single(routes);
        Assert.Equal(route3, routes[0]);
    }
    [Fact]
    public void TestTraceAPI()
    {
        bool updated = false;
        var appState = new AppState();
        appState.MapFileUpdatedEvent += (sender, e) =>
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
        traces = appState.APIListTraces(opt);
        Assert.Empty(traces);
        appState.APIInsertTraces([trace1, trace2, trace3]);
        traces = appState.APIListTraces(opt);
        Assert.Empty(traces);
        Assert.False(updated);
        appState.APIRemoveTraces(["key1"]);
        traces = appState.APIListTraces(opt);
        Assert.Empty(traces);
        Assert.False(updated);
        appState.NewMap();
        appState.APIInsertTraces([trace1, trace2, trace3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        traces = appState.APIListTraces(opt);
        Assert.Equal(3, traces.Count);
        Assert.Equal(trace2, traces[0]);
        Assert.Equal(trace1, traces[1]);
        Assert.Equal(trace3, traces[2]);
        opt.WithGroups([""]);
        traces = appState.APIListTraces(opt);
        Assert.Single(traces);
        Assert.Equal(trace2, traces[0]);
        opt.Clear().WithGroups(["group1"]);
        traces = appState.APIListTraces(opt);
        Assert.Equal(2, traces.Count);
        Assert.Equal(trace1, traces[0]);
        Assert.Equal(trace3, traces[1]);
        opt.Clear().WithGroups(["notfound"]);
        traces = appState.APIListTraces(opt);
        Assert.Empty(traces);
        opt.Clear().WithKeys(["key2"]);
        traces = appState.APIListTraces(opt);
        Assert.Single(traces);
        Assert.Equal(trace2, traces[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        traces = appState.APIListTraces(opt);
        Assert.Empty(traces);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        traces = appState.APIListTraces(opt);
        Assert.Single(traces);
        Assert.Equal(trace1, traces[0]);
        updated = false;
        opt = new APIListOption();
        appState.APIInsertTraces([]);
        Assert.False(updated);
        appState.APIInsertTraces([newtrace2, trace4]);
        Assert.True(updated);
        updated = false;
        traces = appState.APIListTraces(opt);
        Assert.Equal(4, traces.Count);
        Assert.Equal(trace1, traces[0]);
        Assert.Equal(trace3, traces[1]);
        Assert.Equal(newtrace2, traces[2]);
        Assert.Equal(trace4, traces[3]);
        Assert.False(badtrace1.Validated());
        appState.APIInsertTraces([badtrace1]);
        Assert.True(updated);
        updated = false;
        traces = appState.APIListTraces(opt);
        Assert.Equal(4, traces.Count);
        appState.APIRemoveTraces([]);
        Assert.False(updated);
        appState.APIRemoveTraces(["key1"]);
        Assert.True(updated);
        traces = appState.APIListTraces(opt);
        Assert.Equal(3, traces.Count);
        Assert.Equal(trace3, traces[0]);
        Assert.Equal(newtrace2, traces[1]);
        Assert.Equal(trace4, traces[2]);
        appState.APIRemoveTraces(["key1", "key2", "key4"]);
        Assert.True(updated);
        traces = appState.APIListTraces(opt);
        Assert.Single(traces);
        Assert.Equal(trace3, traces[0]);
    }
    [Fact]
    public void TestRegionAPI()
    {
        bool updated = false;
        var appState = new AppState();
        appState.MapFileUpdatedEvent += (sender, e) =>
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
        regions = appState.APIListRegions(opt);
        Assert.Empty(regions);
        appState.APIInsertRegions([region1, region2, region3]);
        regions = appState.APIListRegions(opt);
        Assert.Empty(regions);
        Assert.False(updated);
        appState.APIRemoveRegions(["key1"]);
        regions = appState.APIListRegions(opt);
        Assert.Empty(regions);
        Assert.False(updated);
        appState.NewMap();
        appState.APIInsertRegions([region1, region2, region3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        regions = appState.APIListRegions(opt);
        Assert.Equal(3, regions.Count);
        Assert.Equal(region2, regions[0]);
        Assert.Equal(region1, regions[1]);
        Assert.Equal(region3, regions[2]);
        opt.Clear().WithGroups([""]);
        regions = appState.APIListRegions(opt);
        Assert.Single(regions);
        Assert.Equal(region2, regions[0]);
        opt.Clear().WithGroups(["group1"]);
        regions = appState.APIListRegions(opt);
        Assert.Equal(2, regions.Count);
        Assert.Equal(region1, regions[0]);
        Assert.Equal(region3, regions[1]);
        opt.Clear().WithGroups(["notfound"]);
        regions = appState.APIListRegions(opt);
        Assert.Empty(regions);
        opt.Clear().WithKeys(["key2"]);
        regions = appState.APIListRegions(opt);
        Assert.Single(regions);
        Assert.Equal(region2, regions[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        regions = appState.APIListRegions(opt);
        Assert.Empty(regions);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        regions = appState.APIListRegions(opt);
        Assert.Single(regions);
        Assert.Equal(region1, regions[0]);
        updated = false;
        opt = new APIListOption();
        appState.APIInsertRegions([]);
        Assert.False(updated);
        appState.APIInsertRegions([newregion2, region4]);
        Assert.True(updated);
        updated = false;
        regions = appState.APIListRegions(opt);
        Assert.Equal(4, regions.Count);
        Assert.Equal(region1, regions[0]);
        Assert.Equal(region3, regions[1]);
        Assert.Equal(newregion2, regions[2]);
        Assert.Equal(region4, regions[3]);
        Assert.False(badregion1.Validated());
        appState.APIInsertRegions([badregion1]);
        Assert.True(updated);
        updated = false;
        regions = appState.APIListRegions(opt);
        Assert.Equal(4, regions.Count);
        appState.APIRemoveRegions([]);
        Assert.False(updated);
        appState.APIRemoveRegions(["key1"]);
        Assert.True(updated);
        regions = appState.APIListRegions(opt);
        Assert.Equal(3, regions.Count);
        Assert.Equal(region3, regions[0]);
        Assert.Equal(newregion2, regions[1]);
        Assert.Equal(region4, regions[2]);
        appState.APIRemoveRegions(["key1", "key2", "key4"]);
        Assert.True(updated);
        regions = appState.APIListRegions(opt);
        Assert.Single(regions);
        Assert.Equal(region3, regions[0]);
    }
    [Fact]
    public void TestShortcutAPI()
    {
        bool updated = false;
        var appState = new AppState();
        appState.MapFileUpdatedEvent += (sender, e) =>
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
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Empty(shortcuts);
        appState.APIInsertShortcuts([shortcut1, shortcut2, shortcut3]);
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Empty(shortcuts);
        Assert.False(updated);
        appState.APIRemoveShortcuts(["key1"]);
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Empty(shortcuts);
        Assert.False(updated);
        appState.NewMap();
        appState.APIInsertShortcuts([shortcut1, shortcut2, shortcut3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Equal(3, shortcuts.Count);
        Assert.Equal(shortcut2, shortcuts[0]);
        Assert.Equal(shortcut1, shortcuts[1]);
        Assert.Equal(shortcut3, shortcuts[2]);
        opt.Clear().WithGroups([""]);
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Single(shortcuts);
        Assert.Equal(shortcut2, shortcuts[0]);
        opt.Clear().WithGroups(["group1"]);
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Equal(2, shortcuts.Count);
        Assert.Equal(shortcut1, shortcuts[0]);
        Assert.Equal(shortcut3, shortcuts[1]);
        opt.Clear().WithGroups(["notfound"]);
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Empty(shortcuts);
        opt.Clear().WithKeys(["key2"]);
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Single(shortcuts);
        Assert.Equal(shortcut2, shortcuts[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Empty(shortcuts);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Single(shortcuts);
        Assert.Equal(shortcut1, shortcuts[0]);
        updated = false;
        opt = new APIListOption();
        appState.APIInsertShortcuts([]);
        Assert.False(updated);
        appState.APIInsertShortcuts([newshortcut2, shortcut4]);
        Assert.True(updated);
        updated = false;
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Equal(4, shortcuts.Count);
        Assert.Equal(shortcut1, shortcuts[0]);
        Assert.Equal(shortcut3, shortcuts[1]);
        Assert.Equal(newshortcut2, shortcuts[2]);
        Assert.Equal(shortcut4, shortcuts[3]);
        Assert.False(badshortcut1.Validated());
        appState.APIInsertShortcuts([badshortcut1]);
        Assert.True(updated);
        updated = false;
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Equal(4, shortcuts.Count);
        appState.APIRemoveShortcuts([]);
        Assert.False(updated);
        appState.APIRemoveShortcuts(["key1"]);
        Assert.True(updated);
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Equal(3, shortcuts.Count);
        Assert.Equal(shortcut3, shortcuts[0]);
        Assert.Equal(newshortcut2, shortcuts[1]);
        Assert.Equal(shortcut4, shortcuts[2]);
        appState.APIRemoveShortcuts(["key1", "key2", "key4"]);
        Assert.True(updated);
        shortcuts = appState.APIListShortcuts(opt);
        Assert.Single(shortcuts);
        Assert.Equal(shortcut3, shortcuts[0]);
    }
    [Fact]
    public void TestVariableAPI()
    {
        bool updated = false;
        var appState = new AppState();
        appState.MapFileUpdatedEvent += (sender, e) =>
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
        variables = appState.APIListVariables(opt);
        Assert.Empty(variables);
        appState.APIInsertVariables([variable1, variable2, variable3]);
        variables = appState.APIListVariables(opt);
        Assert.Empty(variables);
        Assert.False(updated);
        appState.APIRemoveVariables(["key1"]);
        variables = appState.APIListVariables(opt);
        Assert.Empty(variables);
        Assert.False(updated);
        appState.NewMap();
        appState.APIInsertVariables([variable1, variable2, variable3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        variables = appState.APIListVariables(opt);
        Assert.Equal(3, variables.Count);
        Assert.Equal(variable2, variables[0]);
        Assert.Equal(variable1, variables[1]);
        Assert.Equal(variable3, variables[2]);
        opt.Clear().WithGroups([""]);
        variables = appState.APIListVariables(opt);
        Assert.Single(variables);
        Assert.Equal(variable2, variables[0]);
        opt.Clear().WithGroups(["group1"]);
        variables = appState.APIListVariables(opt);
        Assert.Equal(2, variables.Count);
        Assert.Equal(variable1, variables[0]);
        Assert.Equal(variable3, variables[1]);
        opt.Clear().WithGroups(["notfound"]);
        variables = appState.APIListVariables(opt);
        Assert.Empty(variables);
        opt.Clear().WithKeys(["key2"]);
        variables = appState.APIListVariables(opt);
        Assert.Single(variables);
        Assert.Equal(variable2, variables[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        variables = appState.APIListVariables(opt);
        Assert.Empty(variables);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        variables = appState.APIListVariables(opt);
        Assert.Single(variables);
        Assert.Equal(variable1, variables[0]);
        updated = false;
        opt = new APIListOption();
        appState.APIInsertVariables([]);
        Assert.False(updated);
        appState.APIInsertVariables([newvariable2, variable4]);
        Assert.True(updated);
        updated = false;
        variables = appState.APIListVariables(opt);
        Assert.Equal(4, variables.Count);
        Assert.Equal(variable1, variables[0]);
        Assert.Equal(variable3, variables[1]);
        Assert.Equal(newvariable2, variables[2]);
        Assert.Equal(variable4, variables[3]);
        Assert.False(badvariable1.Validated());
        appState.APIInsertVariables([badvariable1]);
        Assert.True(updated);
        updated = false;
        variables = appState.APIListVariables(opt);
        Assert.Equal(4, variables.Count);
        appState.APIRemoveVariables([]);
        Assert.False(updated);
        appState.APIRemoveVariables(["key1"]);
        Assert.True(updated);
        variables = appState.APIListVariables(opt);
        Assert.Equal(3, variables.Count);
        Assert.Equal(variable3, variables[0]);
        Assert.Equal(newvariable2, variables[1]);
        Assert.Equal(variable4, variables[2]);
        appState.APIRemoveVariables(["key1", "key2", "key4"]);
        Assert.True(updated);
        variables = appState.APIListVariables(opt);
        Assert.Single(variables);
        Assert.Equal(variable3, variables[0]);
    }
    [Fact]
    public void TestLandmarkAPI()
    {
        bool updated = false;
        var appState = new AppState();
        appState.MapFileUpdatedEvent += (sender, e) =>
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
        landmarks = appState.APIListLandmarks(opt);
        Assert.Empty(landmarks);
        appState.APIInsertLandmarks([landmark1, landmark2, landmark3]);
        landmarks = appState.APIListLandmarks(opt);
        Assert.Empty(landmarks);
        Assert.False(updated);
        appState.APIRemoveLandmarks([landmark1.UniqueKey()]);
        landmarks = appState.APIListLandmarks(opt);
        Assert.Empty(landmarks);
        Assert.False(updated);
        appState.NewMap();
        appState.APIInsertLandmarks([landmark1, landmark1t2, landmark2, landmark3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        landmarks = appState.APIListLandmarks(opt);
        Assert.Equal(4, landmarks.Count);
        Assert.Equal(landmark2, landmarks[0]);
        Assert.Equal(landmark1, landmarks[1]);
        Assert.Equal(landmark1t2, landmarks[2]);
        Assert.Equal(landmark3, landmarks[3]);
        opt.Clear().WithGroups([""]);
        landmarks = appState.APIListLandmarks(opt);
        Assert.Single(landmarks);
        Assert.Equal(landmark2, landmarks[0]);
        opt.Clear().WithGroups(["group1"]);
        landmarks = appState.APIListLandmarks(opt);
        Assert.Equal(3, landmarks.Count);
        Assert.Equal(landmark1, landmarks[0]);
        Assert.Equal(landmark1t2, landmarks[1]);
        Assert.Equal(landmark3, landmarks[2]);
        opt.Clear().WithGroups(["notfound"]);
        landmarks = appState.APIListLandmarks(opt);
        Assert.Empty(landmarks);
        opt.Clear().WithKeys(["key2"]);
        landmarks = appState.APIListLandmarks(opt);
        Assert.Single(landmarks);
        Assert.Equal(landmark2, landmarks[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        landmarks = appState.APIListLandmarks(opt);
        Assert.Empty(landmarks);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        landmarks = appState.APIListLandmarks(opt);
        Assert.Equal(2, landmarks.Count);
        Assert.Equal(landmark1, landmarks[0]);
        Assert.Equal(landmark1t2, landmarks[1]);
        updated = false;
        opt = new APIListOption();
        appState.APIInsertLandmarks([]);
        Assert.False(updated);
        appState.APIInsertLandmarks([newlandmark2, landmark4]);
        Assert.True(updated);
        updated = false;
        landmarks = appState.APIListLandmarks(opt);
        Assert.Equal(5, landmarks.Count);
        Assert.Equal(landmark1, landmarks[0]);
        Assert.Equal(landmark1t2, landmarks[1]);
        Assert.Equal(landmark3, landmarks[2]);
        Assert.Equal(newlandmark2, landmarks[3]);
        Assert.Equal(landmark4, landmarks[4]);
        Assert.False(badlandmark1.Validated());
        appState.APIInsertLandmarks([badlandmark1]);
        Assert.True(updated);
        updated = false;
        landmarks = appState.APIListLandmarks(opt);
        Assert.Equal(5, landmarks.Count);
        appState.APIRemoveLandmarks([]);
        Assert.False(updated);
        appState.APIRemoveLandmarks([landmark1.UniqueKey()]);
        Assert.True(updated);
        landmarks = appState.APIListLandmarks(opt);
        Assert.Equal(4, landmarks.Count);
        Assert.Equal(landmark1t2, landmarks[0]);
        Assert.Equal(landmark3, landmarks[1]);
        Assert.Equal(newlandmark2, landmarks[2]);
        Assert.Equal(landmark4, landmarks[3]);
        appState.APIRemoveLandmarks([landmark1.UniqueKey(), landmark1t2.UniqueKey(), landmark2.UniqueKey(), landmark4.UniqueKey()]);
        Assert.True(updated);
        landmarks = appState.APIListLandmarks(opt);
        Assert.Single(landmarks);
        Assert.Equal(landmark3, landmarks[0]);
    }
    [Fact]
    public void TestSnapshotAPI()
    {
        bool updated = false;
        var appState = new AppState();
        appState.MapFileUpdatedEvent += (sender, e) =>
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
        snapshots = appState.APIListSnapshots(opt);
        Assert.Empty(snapshots);
        appState.APIInsertSnapshots([snapshot1, snapshot2, snapshot3]);
        snapshots = appState.APIListSnapshots(opt);
        Assert.Empty(snapshots);
        Assert.False(updated);
        appState.APIRemoveSnapshots([snapshot1.UniqueKey()]);
        snapshots = appState.APIListSnapshots(opt);
        Assert.Empty(snapshots);
        Assert.False(updated);
        appState.NewMap();
        appState.APIInsertSnapshots([snapshot1, snapshot1t2, snapshot2, snapshot3]);
        Assert.True(updated);
        updated = false;
        opt = new APIListOption();
        snapshots = appState.APIListSnapshots(opt);
        Assert.Equal(4, snapshots.Count);
        Assert.Equal(snapshot2, snapshots[0]);
        Assert.Equal(snapshot1, snapshots[1]);
        Assert.Equal(snapshot1t2, snapshots[2]);
        Assert.Equal(snapshot3, snapshots[3]);
        opt.Clear().WithGroups([""]);
        snapshots = appState.APIListSnapshots(opt);
        Assert.Single(snapshots);
        Assert.Equal(snapshot2, snapshots[0]);
        opt.Clear().WithGroups(["group1"]);
        snapshots = appState.APIListSnapshots(opt);
        Assert.Equal(3, snapshots.Count);
        Assert.Equal(snapshot1, snapshots[0]);
        Assert.Equal(snapshot1t2, snapshots[1]);
        Assert.Equal(snapshot3, snapshots[2]);
        opt.Clear().WithGroups(["notfound"]);
        snapshots = appState.APIListSnapshots(opt);
        Assert.Empty(snapshots);
        opt.Clear().WithKeys(["key2"]);
        snapshots = appState.APIListSnapshots(opt);
        Assert.Single(snapshots);
        Assert.Equal(snapshot2, snapshots[0]);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key2"]);
        snapshots = appState.APIListSnapshots(opt);
        Assert.Empty(snapshots);
        opt.Clear().WithGroups(["group1"]).WithKeys(["key1"]);
        snapshots = appState.APIListSnapshots(opt);
        Assert.Equal(2, snapshots.Count);
        Assert.Equal(snapshot1, snapshots[0]);
        Assert.Equal(snapshot1t2, snapshots[1]);
        updated = false;
        opt = new APIListOption();
        appState.APIInsertSnapshots([]);
        Assert.False(updated);
        appState.APIInsertSnapshots([newsnapshot2, snapshot4]);
        Assert.True(updated);
        updated = false;
        snapshots = appState.APIListSnapshots(opt);
        Assert.Equal(5, snapshots.Count);
        Assert.Equal(snapshot1, snapshots[0]);
        Assert.Equal(snapshot1t2, snapshots[1]);
        Assert.Equal(snapshot3, snapshots[2]);
        Assert.Equal(newsnapshot2, snapshots[3]);
        Assert.Equal(snapshot4, snapshots[4]);
        Assert.False(badsnapshot1.Validated());
        appState.APIInsertSnapshots([badsnapshot1]);
        Assert.True(updated);
        updated = false;
        snapshots = appState.APIListSnapshots(opt);
        Assert.Equal(5, snapshots.Count);
        appState.APIRemoveSnapshots([]);
        Assert.False(updated);
        appState.APIRemoveSnapshots([snapshot1.UniqueKey()]);
        Assert.True(updated);
        snapshots = appState.APIListSnapshots(opt);
        Assert.Equal(4, snapshots.Count);
        Assert.Equal(snapshot1t2, snapshots[0]);
        Assert.Equal(snapshot3, snapshots[1]);
        Assert.Equal(newsnapshot2, snapshots[2]);
        Assert.Equal(snapshot4, snapshots[3]);
        appState.APIRemoveSnapshots([snapshot1.UniqueKey(), snapshot1t2.UniqueKey(), snapshot2.UniqueKey(), snapshot4.UniqueKey()]);
        Assert.True(updated);
        snapshots = appState.APIListSnapshots(opt);
        Assert.Single(snapshots);
        Assert.Equal(snapshot3, snapshots[0]);
    }
    [Fact]
    public void TestArrange()
    {
        var appState = new AppState();
        appState.NewMap();
        var room = new Room()
        {
            Key = "room1",
            Group = "group1",
            Desc = "desc1",
            Data = [new Data("key2", "value2"), new Data("key1", "value1")],
            Tags = ["tag1", "tag2"],
            Exits = [new Exit(){
                Command="cmd1",
                To="to1",
                Cost=1,
                Conditions=[new Condition("key1", true),new Condition("key2", true)],
            }],
        };
        appState.APIInsertRooms([room]);
        var rooms = appState.APIListRooms(new APIListOption());
        Assert.Single(rooms);
        Assert.Equal(2, rooms[0].Data.Count);
        Assert.Equal("key1", rooms[0].Data[0].Key);
        Assert.Equal("key2", rooms[0].Data[1].Key);
        Assert.Equal(2, rooms[0].Tags.Count);
        Assert.Equal("tag1", rooms[0].Tags[0]);
        Assert.Equal("tag2", rooms[0].Tags[1]);
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
        appState.APIInsertTraces([trace]);
        var traces = appState.APIListTraces(new APIListOption());
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
            Conditions = [new Condition("key2", true), new Condition("key1", true)],
        };
        appState.APIInsertShortcuts([shortcut]);
        var shortcuts = appState.APIListShortcuts(new APIListOption());
        Assert.Single(shortcuts);
        Assert.Equal(2, shortcuts[0].Conditions.Count);
        Assert.Equal("key1", shortcuts[0].Conditions[0].Key);
        Assert.Equal("key2", shortcuts[0].Conditions[1].Key);
    }
}