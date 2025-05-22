namespace TestProject;

using HellMapManager.Models;
using HellMapManager.Cores;
using HellMapManager.Utils.ControlCode;

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
            },
            new Room(){Key="key7",
                Tags=[],
                Exits=[
                ],
            }
        ]);
        md.APIInsertShortcuts([
            new Shortcut(){
                Key="shortcut1",
                To="key1",
                Command="A>1",
                Cost=10,
            },
        ]);
    }
    private static void InitContext(Context ctx)
    {
        ctx.ClearTags().WithTags([]);
        ctx.ClearRoomConditions().WithRoomConditions([]);
        ctx.ClearRooms().WithRooms([
                new Room(){
                Key ="key6",
                Tags=[new ValueTag("ctxroom", 1)],
                Exits=[
                    new Exit(){
                        To ="key3",
                        Command ="6>3",
                        Cost=2,
                    },
                ],
            }

        ]);
        ctx.ClearShortcuts().WithShortcuts([
            new Shortcut(){
                To="key6",
                Command="A>6C",
                Conditions=[new ValueCondition("noctxpath", 1,true)],
                Cost=2,
            },
        ]);
        ctx.ClearPaths().WithPaths([
            new Path(){
                From="key5",
                To="key6",
                Command="5>6C",
                Conditions=[new ValueCondition("noctxpath", 1,true)],
                Cost=1,
            },
            new Path(){
                From="key1",
                To="key2",
                Conditions=[new ValueCondition("noctxpath", 1,true)],
                Command="1>2C",
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
        var rooms = mapDatabase.APIDilate(["key1", "key6"], 2, ctx, opt);
        Assert.Empty(rooms);
        var exit = mapDatabase.APITrackExit("key1", "1>2", ctx, opt);
        Assert.Equal("", exit);
        InitMapDatabase(mapDatabase);
        InitContext(ctx);
        qr = mapDatabase.APIQueryPathAll("key1", ["key2"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>2", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key1"], ["key2"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>2", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key1", ["key2"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>2", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key1", "key6"], 2, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key2;key3;key4;key6", string.Join(";", rooms));
        exit = mapDatabase.APITrackExit("key1", "1>2", ctx, opt);
        Assert.Equal("key2", exit);
        exit = mapDatabase.APITrackExit("notfound", "1>2", ctx, opt);
        Assert.Equal("", exit);
        exit = mapDatabase.APITrackExit("key1", "notfound", ctx, opt);
        Assert.Equal("", exit);
        //shortcut
        exit = mapDatabase.APITrackExit("key6", "A>1", ctx, opt);
        Assert.Equal("key1", exit);
        exit = mapDatabase.APITrackExit("key1", "A>6C", ctx, opt);
        Assert.Equal("key6", exit);
        qr = mapDatabase.APIQueryPathAll("key6", ["key1", "key2"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1;1>2", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key6"], ["key1", "key2"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key6", ["key1", "key2"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1;1>2", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key6"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));

        opt.WithDisableShortcuts(true);
        exit = mapDatabase.APITrackExit("key6", "A>1", ctx, opt);
        Assert.Equal("", exit);
        exit = mapDatabase.APITrackExit("key1", "A>6C", ctx, opt);
        Assert.Equal("", exit);
        qr = mapDatabase.APIQueryPathAll("key6", ["key1", "key2"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("6>3;3>1;1>2", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key6"], ["key1", "key2"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("6>3;3>1", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key6", ["key1", "key2"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("6>3;3>1;1>2", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key6"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key3;key6", string.Join(";", rooms));

        //tag
        opt = new MapperOptions();
        InitContext(ctx);
        exit = mapDatabase.APITrackExit("key1", "A>6C", ctx, opt);
        Assert.Equal("key6", exit);
        qr = mapDatabase.APIQueryPathAll("key1", ["key3", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>3;A>6C", Step.JoinCommands(";", qr.Steps));

        qr = mapDatabase.APIQueryPathAny(["key1"], ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>6C", Step.JoinCommands(";", qr.Steps));

        qr = mapDatabase.APIQueryPathOrdered("key1", ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>3;3>4;4>5;5>6C", Step.JoinCommands(";", qr.Steps));

        rooms = mapDatabase.APIDilate(["key1"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key2;key3;key6", string.Join(";", rooms));

        ctx.WithTags([new ValueTag("noctxpath", 1)]);
        exit = mapDatabase.APITrackExit("key1", "A>6C", ctx, opt);
        Assert.Equal("", exit);
        qr = mapDatabase.APIQueryPathAll("key1", ["key3", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>3", Step.JoinCommands(";", qr.Steps));

        qr = mapDatabase.APIQueryPathAny(["key1"], ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key1", ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));

        rooms = mapDatabase.APIDilate(["key1"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key2;key3", string.Join(";", rooms));
        //RoomConditions
        opt = new MapperOptions();
        InitContext(ctx);
        exit = mapDatabase.APITrackExit("key1", "A>6C", ctx, opt);
        Assert.Equal("key6", exit);
        qr = mapDatabase.APIQueryPathAll("key1", ["key3", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>3;A>6C", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key1"], ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>6C", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key1", ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>3;3>4;4>5;5>6C", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key1"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key2;key3;key6", string.Join(";", rooms));

        ctx.WithRoomConditions([new ValueCondition("ctxroom", 1, true)]);
        exit = mapDatabase.APITrackExit("key1", "A>6C", ctx, opt);
        Assert.Equal("", exit);
        qr = mapDatabase.APIQueryPathAll("key1", ["key3", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>3", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key1"], ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key1", ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key1"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key2;key3", string.Join(";", rooms));

        //Whitelist
        opt = new MapperOptions();
        InitContext(ctx);

        exit = mapDatabase.APITrackExit("key3", "3>4", ctx, opt);
        Assert.Equal("key4", exit);
        qr = mapDatabase.APIQueryPathAll("key1", ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>6C;6>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key3"], ["key6", "key4"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("3>4", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key1", ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>3;3>4;4>5;5>6C", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key3"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key3;key4;key6", string.Join(";", rooms));

        ctx.WithWhitelist(["key1", "key2", "key3", "key5", "key6"]);
        exit = mapDatabase.APITrackExit("key3", "3>4", ctx, opt);
        Assert.Equal("", exit);
        qr = mapDatabase.APIQueryPathAll("key1", ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>6C", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key3"], ["key6", "key4"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>6C", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key1", ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>6C", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key3"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));

        //blacklist
        opt = new MapperOptions();
        InitContext(ctx);
        exit = mapDatabase.APITrackExit("key1", "1>3", ctx, opt);
        Assert.Equal("key3", exit);
        qr = mapDatabase.APIQueryPathAll("key1", ["key4", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>6C;6>3;3>4", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key4"], ["key6", "key3"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("4>3", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key1", ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("1>3;3>4;4>5;5>6C", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key4"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key3;key4;key5;key6", string.Join(";", rooms));

        ctx.WithBlacklist(["key3"]);
        exit = mapDatabase.APITrackExit("key1", "1>3", ctx, opt);
        Assert.Equal("", exit);
        qr = mapDatabase.APIQueryPathAll("key1", ["key4", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>6C", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key4"], ["key6", "key3"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>6C", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key1", ["key5", "key6"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>6C", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key4"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key4;key5;key6", string.Join(";", rooms));
        //BlockedLinks
        opt = new MapperOptions();
        InitContext(ctx);
        exit = mapDatabase.APITrackExit("key6", "A>1", ctx, opt);
        Assert.Equal("key1", exit);
        qr = mapDatabase.APIQueryPathAll("key6", ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key6"], ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key6", ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key6"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));

        ctx.WithBlockedLinks([new Link("key6", "key1")]);
        exit = mapDatabase.APITrackExit("key6", "A>1", ctx, opt);
        Assert.Equal("", exit);
        qr = mapDatabase.APIQueryPathAll("key6", ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("6>3;3>1;1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key6"], ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("6>3;3>1", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key6", ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("6>3;3>1;1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key6"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key3;key6", string.Join(";", rooms));

        //CommandCosts
        opt = new MapperOptions();
        InitContext(ctx);
        exit = mapDatabase.APITrackExit("key6", "A>1", ctx, opt);
        Assert.Equal("key1", exit);
        qr = mapDatabase.APIQueryPathAll("key6", ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        Assert.Equal(13, qr.Cost);
        qr = mapDatabase.APIQueryPathAny(["key6"], ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1", Step.JoinCommands(";", qr.Steps));
        Assert.Equal(10, qr.Cost);
        qr = mapDatabase.APIQueryPathOrdered("key6", ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key6"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));

        ctx.WithCommandCosts([new CommandCost("A>1", "key1", 99)]);
        exit = mapDatabase.APITrackExit("key6", "A>1", ctx, opt);
        Assert.Equal("key1", exit);
        qr = mapDatabase.APIQueryPathAll("key6", ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        Assert.Equal(102, qr.Cost);
        qr = mapDatabase.APIQueryPathAny(["key6"], ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal(99, qr.Cost);
        Assert.Equal("A>1", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key6", ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal(102, qr.Cost);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key6"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));

        //MaxExitCost
        opt = new MapperOptions();
        InitContext(ctx);
        exit = mapDatabase.APITrackExit("key6", "A>1", ctx, opt);
        Assert.Equal("key1", exit);
        qr = mapDatabase.APIQueryPathAll("key6", ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key6"], ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key6", ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("A>1;1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key6"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key1;key3;key6", string.Join(";", rooms));

        opt.MaxExitCost = 2;
        exit = mapDatabase.APITrackExit("key6", "A>1", ctx, opt);
        Assert.Equal("", exit);
        qr = mapDatabase.APIQueryPathAll("key6", ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("6>3;3>1;1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathAny(["key6"], ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("6>3;3>1", Step.JoinCommands(";", qr.Steps));
        qr = mapDatabase.APIQueryPathOrdered("key6", ["key1", "key5"], ctx, opt);
        Assert.NotNull(qr);
        Assert.Equal("6>3;3>1;1>3;3>4;4>5", Step.JoinCommands(";", qr.Steps));
        rooms = mapDatabase.APIDilate(["key6"], 1, ctx, opt);
        rooms.Sort();
        Assert.Equal("key3;key6", string.Join(";", rooms));

    }
}