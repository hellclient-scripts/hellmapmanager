using HellMapManager.Models;

namespace TestProject;

public class ModelEncodeTest
{
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
}