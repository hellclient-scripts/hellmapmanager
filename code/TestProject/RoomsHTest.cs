using HellMapManager.Models;
using HellMapManager.Helpers;

namespace TestProject;

public class RoomsHTest
{
    [Fact]
    public void TestComment()
    {
        var body = "\n  \n \\\\abcd\n\\\\efgh";
        Assert.Empty(RoomsH.Load(body));
        body = "0=123|e:1";
        Assert.Single(RoomsH.Load(body));

    }
    [Fact]
    public void TestDecode()
    {
        Room? room;
        room = RoomFormatter.DecodeRoom("");
        Assert.Null(room);
        room = RoomFormatter.DecodeRoom("rid=rname@rzone+tag1|rcon1>rcon2^15>rcon3<rcon4^-1<rcmd:rto%2,rcmd2:rto2");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("rzone", room.Group);
        Assert.Single(room.Tags);
        Assert.Equal("tag1", room.Tags[0].Key);
        Assert.Equal(2, room.Exits.Count);
        Assert.Equal("rcmd", room.Exits[0].Command);
        Assert.Equal(4, room.Exits[0].Conditions.Count);
        Assert.False(room.Exits[0].Conditions[0].Not);
        Assert.Equal("rcon1", room.Exits[0].Conditions[0].Key);
        Assert.Equal(1, room.Exits[0].Conditions[0].Value);
        Assert.False(room.Exits[0].Conditions[1].Not);
        Assert.Equal("rcon2", room.Exits[0].Conditions[1].Key);
        Assert.Equal(15, room.Exits[0].Conditions[1].Value);
        Assert.True(room.Exits[0].Conditions[2].Not);
        Assert.Equal("rcon3", room.Exits[0].Conditions[2].Key);
        Assert.Equal(1, room.Exits[0].Conditions[2].Value);
        Assert.True(room.Exits[0].Conditions[3].Not);
        Assert.Equal("rcon4", room.Exits[0].Conditions[3].Key);
        Assert.Equal(-1, room.Exits[0].Conditions[3].Value);
        Assert.Equal("rto", room.Exits[0].To);
        Assert.Equal(2, room.Exits[0].Cost);
        Assert.Equal("rcmd2", room.Exits[1].Command);
        Assert.Empty(room.Exits[1].Conditions);
        Assert.Equal("rto2", room.Exits[1].To);
        Assert.Equal(1, room.Exits[1].Cost);
        Assert.Empty(room.Data);


        room = RoomFormatter.DecodeRoom("rid");
        Assert.Null(room);

        room = RoomFormatter.DecodeRoom("rid@+|><,%\\");
        Assert.Null(room);

        room = RoomFormatter.DecodeRoom("rid=");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("", room.Name);
        Assert.Equal("", room.Group);
        Assert.Empty(room.Tags);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);
        room = RoomFormatter.DecodeRoom("rid=@");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("", room.Name);
        Assert.Equal("", room.Group);
        Assert.Empty(room.Tags);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);
        room = RoomFormatter.DecodeRoom("rid=|");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("", room.Name);
        Assert.Equal("", room.Group);
        Assert.Empty(room.Tags);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom(RoomFormatter.Escaper.Unpack("r\\=id="));
        Assert.NotNull(room);
        Assert.Equal("r=id", room.Key);
        room = RoomFormatter.DecodeRoom(RoomFormatter.Escaper.Unpack("r\\=id="));
        Assert.NotNull(room);
        room = RoomFormatter.DecodeRoom("rid=rname");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("", room.Group);
        Assert.Empty(room.Tags);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);
        room = RoomFormatter.DecodeRoom("rid=rname@");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("", room.Group);
        Assert.Empty(room.Tags);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);
        room = RoomFormatter.DecodeRoom("rid=rname@|");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("", room.Group);
        Assert.Empty(room.Tags);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);
        room = RoomFormatter.DecodeRoom("rid=rname|");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("", room.Group);
        Assert.Empty(room.Tags);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);


        room = RoomFormatter.DecodeRoom("rid=rname@rzone");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("rzone", room.Group);
        Assert.Empty(room.Tags);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=rname@rzone+tag1");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("rzone", room.Group);
        Assert.Single(room.Tags);
        Assert.Equal("tag1", room.Tags[0].Key);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=rname@rzone+tag1+tag2");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("rzone", room.Group);
        Assert.Equal(2, room.Tags.Count);
        Assert.Equal("tag1", room.Tags[0].Key);
        Assert.Equal("tag2", room.Tags[1].Key);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=rname@rzone+");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("rzone", room.Group);
        Assert.Empty(room.Tags);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=rname@rzone++tag2");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("rzone", room.Group);
        Assert.Single(room.Tags);
        Assert.Equal("tag2", room.Tags[0].Key);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=rname@+tag1");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("", room.Group);
        Assert.Single(room.Tags);
        Assert.Equal("tag1", room.Tags[0].Key);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=rname@+tag1^0");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("", room.Group);
        Assert.Single(room.Tags);
        Assert.Equal("tag1", room.Tags[0].Key);
        Assert.Equal(0, room.Tags[0].Value);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=rname@+tag1^1");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("", room.Group);
        Assert.Single(room.Tags);
        Assert.Equal("tag1", room.Tags[0].Key);
        Assert.Equal(1, room.Tags[0].Value);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=rname@+tag1^^1");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("", room.Group);
        Assert.Single(room.Tags);
        Assert.Equal("tag1", room.Tags[0].Key);
        Assert.Equal(1, room.Tags[0].Value);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);


        room = RoomFormatter.DecodeRoom("rid=rname@+tag1|");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("", room.Group);
        Assert.Single(room.Tags);
        Assert.Equal("tag1", room.Tags[0].Key);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=|rcmd1:rto1");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("", room.Name);
        Assert.Equal("", room.Group);
        Assert.Empty(room.Tags);
        Assert.Single(room.Exits);
        Assert.Equal("rcmd1", room.Exits[0].Command);
        Assert.Empty(room.Exits[0].Conditions);
        Assert.Equal("rto1", room.Exits[0].To);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=|>rcon1:rto1");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("", room.Name);
        Assert.Equal("", room.Group);
        Assert.Empty(room.Tags);
        Assert.Single(room.Exits);
        Assert.Equal("rcon1", room.Exits[0].Command);
        Assert.Empty(room.Exits[0].Conditions);
        Assert.Equal("rto1", room.Exits[0].To);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=|b<a>rcon1:rto1");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("", room.Name);
        Assert.Equal("", room.Group);
        Assert.Empty(room.Tags);
        Assert.Single(room.Exits);
        Assert.Equal("rcon1", room.Exits[0].Command);
        Assert.Single(room.Exits[0].Conditions);
        Assert.Equal("b<a", room.Exits[0].Conditions[0].Key);
        Assert.False(room.Exits[0].Conditions[0].Not);
        Assert.Equal("rto1", room.Exits[0].To);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=|:");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("", room.Name);
        Assert.Equal("", room.Group);
        Assert.Empty(room.Tags);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);
        room = RoomFormatter.DecodeRoom("rid=|@+|><,%\\");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("", room.Name);
        Assert.Equal("", room.Group);
        Assert.Empty(room.Tags);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom(RoomFormatter.Escaper.Unpack(
            "key\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\" +
            "=name\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\" +
            "@group\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\" +
            "+tag\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\" +
            "+tag2\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\" +
            "|" +
            "con\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\>" +
            "con2\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\<" +
            "cmd\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\" +
            ":to\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\" +
            "%2\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\" +
            "," +
            "cmd2\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\" +
            ":to2\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\"
            ));
        Assert.NotNull(room);
        Assert.Equal("key=@+|><,%:\\", room.Key);
        Assert.Equal("name=@+|><,%:\\", room.Name);
        Assert.Equal("group=@+|><,%:\\", room.Group);
        Assert.Equal(2, room.Tags.Count);
        Assert.Equal("tag=@+|><,%:\\", room.Tags[0].Key);
        Assert.Equal("tag2=@+|><,%:\\", room.Tags[1].Key);
        Assert.Equal(2, room.Exits.Count);
        Assert.Equal(2, room.Exits[0].Conditions.Count);
        Assert.Equal("con=@+|><,%:\\", room.Exits[0].Conditions[0].Key);
        Assert.False(room.Exits[0].Conditions[0].Not);
        Assert.Equal("con2=@+|><,%:\\", room.Exits[0].Conditions[1].Key);
        Assert.True(room.Exits[0].Conditions[1].Not);
        Assert.Equal("cmd=@+|><,%:\\", room.Exits[0].Command);
        Assert.Equal("to=@+|><,%:\\", room.Exits[0].To);
        Assert.Equal(1, room.Exits[0].Cost);
        Assert.Empty(room.Exits[1].Conditions);
        Assert.Equal("cmd2=@+|><,%:\\", room.Exits[1].Command);
        Assert.Equal("to2=@+|><,%:\\", room.Exits[1].To);
        Assert.Equal(1, room.Exits[1].Cost);


        room = RoomFormatter.DecodeRoom(RoomFormatter.Escaper.Unpack(
            "key@+><,%:" +
            "=name=><,%:" +
            "@group=@><,%:" +
            "+tag=@><,%:" +
            "+tag2=@><,%:" +
            "|" +
            "con=@+%<>" +
            "con2=@+%<" +
            "cmd=@+%" +
            ":to=@+><" +
            "%2=@+|><%:" +
            "," +
            "cmd2=@+|%" +
            ":to2=@+|><:"
            ));
        Assert.NotNull(room);
        Assert.Equal("key@+><,%:", room.Key);
        Assert.Equal("name=><,%:", room.Name);
        Assert.Equal("group=@><,%:", room.Group);
        Assert.Equal(2, room.Tags.Count);
        Assert.Equal("tag=@><,%:", room.Tags[0].Key);
        Assert.Equal("tag2=@><,%:", room.Tags[1].Key);
        Assert.Equal(2, room.Exits.Count);
        Assert.Equal(2, room.Exits[0].Conditions.Count);
        Assert.Equal("con=@+%<", room.Exits[0].Conditions[0].Key);
        Assert.False(room.Exits[0].Conditions[0].Not);
        Assert.Equal("con2=@+%", room.Exits[0].Conditions[1].Key);
        Assert.True(room.Exits[0].Conditions[1].Not);
        Assert.Equal("cmd=@+%", room.Exits[0].Command);
        Assert.Equal("to=@+><", room.Exits[0].To);
        Assert.Equal(1, room.Exits[0].Cost);
        Assert.Empty(room.Exits[1].Conditions);
        Assert.Equal("cmd2=@+|%", room.Exits[1].Command);
        Assert.Equal("to2=@+|><:", room.Exits[1].To);
        Assert.Equal(1, room.Exits[1].Cost);
        room = RoomFormatter.DecodeRoom("=rname|rcmd:rto");
        Assert.Null(room);
        room = RoomFormatter.DecodeRoom("rid=rname|a>:rto");
        Assert.NotNull(room);
        Assert.Empty(room.Exits);
        room = RoomFormatter.DecodeRoom("rid=rname|a<:rto");
        Assert.NotNull(room);
        Assert.Empty(room.Exits);
    }
    [Fact]
    public void TestEncode()
    {
        var room = new Room();
        var opt = new RoomsHExportOption();
        room.Key = "=@+|><,%:\\";
        Assert.Equal("\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\=|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Key = "rid";
        Assert.Equal("rid=|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Name = "=@+|><,%:\\";
        Assert.Equal("rid=\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Name = "rname";
        Assert.Equal("rid=rname|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Group = "=@+|><,%:\\";
        Assert.Equal("rid=rname@\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Group = "rzone";
        Assert.Equal("rid=rname@rzone|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Tags = [new ValueTag("=@+|><,%:\\", 1), new ValueTag("=@+|><,%:\\2", 1)];
        Assert.Equal("rid=rname@rzone+\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\+\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\2|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Tags = [new ValueTag("tag", 1), new ValueTag("tag2", 1)];
        Assert.Equal("rid=rname@rzone+tag+tag2|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Group = "";
        Assert.Equal("rid=rname@+tag+tag2|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Group = "rzone";
        room.Exits = [
            new Exit(){
                Command="rcmd=@+|><,%:\\",
                To="rto=@+|><,%:\\",
                Conditions=[new ValueCondition("rcon=@+|><,%:\\",1,false),new ValueCondition("rcon2=@+|><,%:\\",1,true)]
            }
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcon\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\>rcon2\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\<rcmd\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\:rto\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Exits = [
            new Exit(){
                Command="rcmd",
                To="rto",
                Conditions=[new ValueCondition("rcon",0,false),new ValueCondition("rcon2",1,true),
                ],
                Cost=1,

            }
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcon^0>rcon2<rcmd:rto", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Exits.Add(
        new Exit()
        {
            Command = "rcmd2",
            To = "rto2",
            Cost = -5,
        }
        );
        Assert.Equal("rid=rname@rzone+tag+tag2|rcon^0>rcon2<rcmd:rto,rcmd2:rto2%-5", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Exits = [
            new Exit(){
                Command="rcmd",
                To="rto",
                Cost=0,
            }
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcmd:rto%0", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Exits = [
            new Exit(){
                Command="rcmd",
                To="rto",
                Cost=2,
            }
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcmd:rto%2", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Exits = [
            new Exit(){
                Command="rcmd",
                To="rto",
                Conditions=[new ValueCondition("rcon2",1,true)]
            }
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcon2<rcmd:rto", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Exits = [
            new Exit(){
                Command="rcmd",
                To="rto",
                Conditions=[new ValueCondition("rcon",1,false)]
            }
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcon>rcmd:rto", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));
        room.Exits = [
            new Exit(){
                Command="rcmd",
                To="rto",
                Conditions=[new ValueCondition("rcon2",1,true),new ValueCondition("rcon",1,false)]
            },
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcon>rcon2<rcmd:rto", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room, opt)));

        var room2 = new Room()
        {
            Key = "rid2",
            Desc = "desc2",
            Name = "rname2",
            Group = "rzone2",
            Tags = [new ValueTag("tag1", 1), new ValueTag("tag2", 1)],
            Exits = [
                new Exit(){
                    Command="rcmd",
                    To="rto2",
                    Cost=3
                },
            ],
        };
        Assert.Equal("rid2=rname2@rzone2+tag1+tag2|rcmd:rto2%3", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room2, opt)));
        opt.DisableCost = true;
        Assert.Equal("rid2=rname2@rzone2+tag1+tag2|rcmd:rto2", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room2, opt)));
        opt.DisableCost = false;
        opt.DisableRoomDef = true;
        Assert.Equal("rid2=rname2|rcmd:rto2%3", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room2, opt)));
        opt.DisableCost = true;
        opt.DisableRoomDef = true;
        Assert.Equal("rid2=rname2|rcmd:rto2", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room2, opt)));
    }
    [Fact]
    public void TestExport()
    {
        List<Room> rooms = new List<Room>(){
        new ()
            {
                Key = "room1",
            },
        new ()
            {
                Key = "0",
            },
        new ()
            {
                Key = "1",
            },
        new ()
            {
                Key = "-1",
            },
        };
        var result = RoomsH.Export(rooms, new RoomsHExportOption());
        Assert.Equal(4, result.Count);
        Assert.Equal("-1=|", result[0]);
        Assert.Equal("room1=|", result[1]);
        Assert.Equal("0=|", result[2]);
        Assert.Equal("1=|", result[3]);
    }
}

