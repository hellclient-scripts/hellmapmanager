namespace TestProject;

using HellMapManager.Models;
using HellMapManager.Cores;


public class MapTest()
{
    private static void InitMapDatabase(MapDatabase md)
    {
        md.NewMap();
        md.APIInsertRooms([
            new Room(){Key="key1",
                Tags=[],
                Exits=[
                    new Exit(){
                        To ="key2",
                        Command ="1>2",
                        Cost=1,
                    },
                    new Exit(){
                        To ="key3",
                        Command ="1>3",
                        Cost=1,
                    },
                ],
            },
            new Room(){Key="key2",
                Tags=[],
                Exits=[
                    new Exit(){
                        To ="key1",
                        Command ="2>1",
                        Cost=1,
                    },
                    new Exit(){
                        To ="key3",
                        Command ="2>3",
                        Cost=1,
                    },
                ],
            },
            new Room(){Key="key3",
                Tags=[],
                Exits=[
                    new Exit(){
                        To ="key1",
                        Command ="3>1",
                        Cost=1,
                    },
                    new Exit(){
                        To ="key3",
                        Command ="3>3",
                        Cost=1,
                    },
                    new Exit(){
                        To ="key4",
                        Command ="3>4",
                        Cost=1,
                    },

                ],
            },
            new Room(){Key="key4",
                Tags=[],
                Exits=[
                    new Exit(){
                        To ="key3",
                        Command ="4>3",
                        Cost=1,
                    },
                    new Exit(){
                        To ="key5",
                        Command ="4>5",
                        Cost=1,
                    },
                ],
            },
            new Room(){Key="key5",
                Tags=[],
                Exits=[
                    new Exit(){
                        To ="key3",
                        Command ="5>3",
                        Cost=1,
                    },
                ],
            }
        ]);
    }
    private static void InitContext(Context ctx)
    {
        ctx.ClearTags().WithTags([]);
        ctx.ClearRoomConditions().WithRoomConditions([]);
        ctx.ClearRooms().WithRooms([
                new Room(){Key="key6",
                Tags=[],
                Exits=[
                    new Exit(){
                        To ="key3",
                        Command ="6>3",
                        Cost=2,
                    },
                ],
            }

        ]);
        ctx.ClearShortcuts().WithShortcuts([]);
        ctx.ClearPaths().WithPaths([
            new Path(){
                From="key5",
                To="key6",
                Command="5>6",
                Cost=1,
            },
        ]);
        ctx.ClearWhitelist().WithWhitelist([]);
        ctx.ClearBlacklist().WithBlacklist([]);
        ctx.ClearBlockedLinks().WithBlockedLinks([]);
        ctx.ClearCommandCosts().WithCommandCosts([]);
    }
    [Fact]
    public void TestMap()
    {
        var mapDatabase = new MapDatabase();
        var ctx = new Context();
        var opt = new MapperOptions();
        var qr = mapDatabase.APIQueryPathAll("key1", ["key2"], ctx, opt);
        Assert.Null(qr);
        qr = mapDatabase.APIQueryPathAny(["key1"], ["key2"], ctx, opt);
        Assert.Null(qr);
        qr = mapDatabase.APIQueryPathOrdered("key1", ["key2"], ctx, opt);
        Assert.Null(qr);
        InitMapDatabase(mapDatabase);
        InitContext(ctx);
    }
}