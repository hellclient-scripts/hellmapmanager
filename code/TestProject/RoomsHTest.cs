using HellMapManager.Models;
using HellMapManager.Services;

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
        room = RoomFormatter.DecodeRoom("rid=rname@rzone+tag1|rcon1>rcon2>rcon3<rcon4<rcmd:rto%2,rcmd2:rto2");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("rzone", room.Group);
        Assert.Single(room.Tags);
        Assert.Equal("tag1", room.Tags[0]);
        Assert.Equal(2, room.Exits.Count);
        Assert.Equal("rcmd", room.Exits[0].Command);
        Assert.Equal(4, room.Exits[0].Conditions.Count);
        Assert.False(room.Exits[0].Conditions[0].Not);
        Assert.Equal("rcon1", room.Exits[0].Conditions[0].Key);
        Assert.False(room.Exits[0].Conditions[1].Not);
        Assert.Equal("rcon2", room.Exits[0].Conditions[1].Key);
        Assert.True(room.Exits[0].Conditions[2].Not);
        Assert.Equal("rcon3", room.Exits[0].Conditions[2].Key);
        Assert.True(room.Exits[0].Conditions[3].Not);
        Assert.Equal("rcon4", room.Exits[0].Conditions[3].Key);
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
        Assert.Equal("tag1", room.Tags[0]);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=rname@rzone+tag1+tag2");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("rzone", room.Group);
        Assert.Equal(2, room.Tags.Count);
        Assert.Equal("tag1", room.Tags[0]);
        Assert.Equal("tag2", room.Tags[1]);
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
        Assert.Equal("tag2", room.Tags[0]);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=rname@+tag1");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("", room.Group);
        Assert.Single(room.Tags);
        Assert.Equal("tag1", room.Tags[0]);
        Assert.Empty(room.Exits);
        Assert.Empty(room.Data);

        room = RoomFormatter.DecodeRoom("rid=rname@+tag1|");
        Assert.NotNull(room);
        Assert.Equal("rid", room.Key);
        Assert.Equal("rname", room.Name);
        Assert.Equal("", room.Group);
        Assert.Single(room.Tags);
        Assert.Equal("tag1", room.Tags[0]);
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
        Assert.Equal("tag=@+|><,%:\\", room.Tags[0]);
        Assert.Equal("tag2=@+|><,%:\\", room.Tags[1]);
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
        Assert.Equal("tag=@><,%:", room.Tags[0]);
        Assert.Equal("tag2=@><,%:", room.Tags[1]);
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
        room.Key = "=@+|><,%:\\";
        Assert.Equal("\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\=|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Key = "rid";
        Assert.Equal("rid=|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Name = "=@+|><,%:\\";
        Assert.Equal("rid=\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Name = "rname";
        Assert.Equal("rid=rname|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Group = "=@+|><,%:\\";
        Assert.Equal("rid=rname@\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Group = "rzone";
        Assert.Equal("rid=rname@rzone|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Tags = ["=@+|><,%:\\", "=@+|><,%:\\2"];
        Assert.Equal("rid=rname@rzone+\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\+\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\2|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Tags = ["tag", "tag2"];
        Assert.Equal("rid=rname@rzone+tag+tag2|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Group = "";
        Assert.Equal("rid=rname@+tag+tag2|", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Group = "rzone";
        room.Exits = [
            new Exit(){
                Command="rcmd=@+|><,%:\\",
                To="rto=@+|><,%:\\",
                Conditions=[new Condition("rcon=@+|><,%:\\",false),new Condition("rcon2=@+|><,%:\\",true)]
            }
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcon\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\>rcon2\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\<rcmd\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\:rto\\=\\@\\+\\|\\>\\<\\,\\%\\:\\\\", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Exits = [
            new Exit(){
                Command="rcmd",
                To="rto",
                Conditions=[new Condition("rcon",false),new Condition("rcon2",true),
                ],
                Cost=1,

            }
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcon>rcon2<rcmd:rto", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Exits.Add(
        new Exit()
        {
            Command = "rcmd2",
            To = "rto2",
            Cost = -5,
        }
        );
        Assert.Equal("rid=rname@rzone+tag+tag2|rcon>rcon2<rcmd:rto,rcmd2:rto2%-5", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Exits = [
            new Exit(){
                Command="rcmd",
                To="rto",
                Cost=0,
            }
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcmd:rto%0", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Exits = [
            new Exit(){
                Command="rcmd",
                To="rto",
                Cost=2,
            }
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcmd:rto%2", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Exits = [
            new Exit(){
                Command="rcmd",
                To="rto",
                Conditions=[new Condition("rcon2",true)]
            }
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcon2<rcmd:rto", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Exits = [
            new Exit(){
                Command="rcmd",
                To="rto",
                Conditions=[new Condition("rcon",false)]
            }
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcon>rcmd:rto", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
        room.Exits = [
            new Exit(){
                Command="rcmd",
                To="rto",
                Conditions=[new Condition("rcon2",true),new Condition("rcon",false)]
            },
        ];
        Assert.Equal("rid=rname@rzone+tag+tag2|rcon>rcon2<rcmd:rto", RoomFormatter.Escaper.Pack(RoomFormatter.EncodeRoom(room)));
    }

}