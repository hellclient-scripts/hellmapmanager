using HellMapManager.Models;

namespace TestProject;

public class ModelTest
{
    [Fact]
    public void TestRoom()
    {
        var room = new Room()
        {
            Key = "key",
            Name = "name",
            Group = "group",
            Desc = "desc",
            Tags = ["tag1", "tag2"],
            Data = [new Data("dkey1", "dval1"), new Data("dkey2", "dval2")],
            Exits = [
                new Exit(){
                Command="command1",
                To="to1",
                Cost=2,
                Conditions=[
                    new Condition("con1",true),
                    new Condition("con2",false),
                ]
            },
               new Exit(){
                Command="command2",
                To="to2",
                Cost=4,

            }
            ]
        };
        Room room2;

        Assert.False(room.Filter("unknow"));
        Assert.True(room.Filter("key"));
        Assert.True(room.Filter("name"));
        Assert.True(room.Filter("group"));
        Assert.True(room.Filter("tag1"));
        Assert.True(room.Filter("dkey1"));
        Assert.True(room.Filter("dval2"));
        Assert.True(room.Filter("command1"));
        Assert.True(room.Filter("to2"));

        Assert.Equal(2, room.ExitsCount);
        Assert.Equal("tag1,tag2", room.AllTags);
        Assert.False(room.HasExitTo("to999"));
        Assert.True(room.HasExitTo("to1"));
        Assert.True(room.HasExitTo("to2"));

        room2 = room.Clone();
        Assert.True(room2.Validated());
        room2.Key = "";
        Assert.False(room2.Validated());

        room2 = room.Clone();
        Assert.True(room.Equal(room2));

        room2 = room.Clone();
        room2.Key = "";
        Assert.False(room.Equal(room2));

        room2 = room.Clone();
        room2.Desc = "";
        Assert.False(room.Equal(room2));

        room2 = room.Clone();
        room2.Group = "";
        Assert.False(room.Equal(room2));

        room2 = room.Clone();
        room2.Desc = "";
        Assert.False(room.Equal(room2));

        room2 = room.Clone();
        room2.Tags[1] = "";
        Assert.False(room.Equal(room2));

        room2 = room.Clone();
        room2.Tags.Add("");
        Assert.False(room.Equal(room2));

        room2 = room.Clone();
        room2.Exits[0].To = "";
        Assert.False(room.Equal(room2));

        room2 = room.Clone();
        room2.Exits.Add(new Exit());
        Assert.False(room.Equal(room2));

        room2 = room.Clone();
        room2.Data[0].Value = "";
        Assert.False(room.Equal(room2));

        room2 = room.Clone();
        room2.Data.Add(new Data("", ""));
        Assert.False(room.Equal(room2));

    }
}