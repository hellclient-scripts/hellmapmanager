using HellMapManager.Models;

namespace TestProject;

public class ModelEncodeTest
{
    private Room SuffRoom(string suff)
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

}