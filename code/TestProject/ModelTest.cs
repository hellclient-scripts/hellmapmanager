using HellMapManager.Models;

namespace TestProject;

public class ModelTest
{
    [Fact]
    public void TestBase()
    {
        var data = new Data("key", "Value");
        Data data2;
        data2 = data.Clone();
        Assert.True(data.Equal(data2));
        Assert.True(data2.Validated());
        data2.Key = "";
        Assert.False(data.Equal(data2));
        Assert.False(data2.Validated());
        data2 = data.Clone();
        data2.Value = "";
        Assert.False(data.Equal(data2));
        Assert.False(data2.Validated());

        var con = new Condition("cond", true);
        Condition con2;
        con2 = con.Clone();
        Assert.True(con.Equal(con2));
        Assert.True(con2.Validated());
        con2 = con.Clone();
        con2.Key = "";
        Assert.False(con.Equal(con2));
        Assert.False(con2.Validated());
        con2 = con.Clone();
        con2.Not = false;
        Assert.False(con.Equal(con2));
        Assert.True(con2.Validated());

        var tc = new TypedConditions("key", ["1", "2"], true);
        TypedConditions tc2;
        tc2 = tc.Clone();
        Assert.True(tc.Equal(tc2));
        Assert.True(tc2.Validated());
        tc2 = tc.Clone();
        tc2.Key = "";
        Assert.False(tc.Equal(tc2));
        Assert.False(tc2.Validated());
        tc2 = tc.Clone();
        tc2.Conditions[0] = "0";
        Assert.False(tc.Equal(tc2));
        Assert.True(tc2.Validated());
        tc2 = tc.Clone();
        tc2.Conditions.Add("3");
        Assert.False(tc.Equal(tc2));
        Assert.True(tc2.Validated());

        tc2 = tc.Clone();
        tc2.Not = false;
        Assert.False(tc.Equal(tc2));
        Assert.True(tc2.Validated());

    }
    [Fact]
    public void TestExit()
    {
        var exit = new Exit()
        {
            To = "to",
            Command = "cmd",
            Cost = 2,
            Conditions = [new Condition("con1", false), new Condition("con2", true)]
        };
        Exit exit2;
        exit2 = exit.Clone();
        Assert.True(exit.Equal(exit2));
        Assert.True(exit2.Validated());
        exit2 = exit.Clone();
        exit2.Command = "";
        Assert.False(exit.Equal(exit2));
        Assert.False(exit2.Validated());
        exit2 = exit.Clone();
        exit2.To = "";
        Assert.False(exit.Equal(exit2));
        Assert.True(exit2.Validated());
        exit2 = exit.Clone();
        exit2.Cost = -1;
        Assert.False(exit.Equal(exit2));
        Assert.True(exit2.Validated());
        exit2 = exit.Clone();
        exit2.Conditions[0].Key = "wrongkey";
        Assert.False(exit.Equal(exit2));
        Assert.True(exit2.Validated());
        exit2 = exit.Clone();
        exit2.Conditions.Add(new("con3", true));
        Assert.False(exit.Equal(exit2));
        Assert.True(exit2.Validated());
        exit2 = exit.Clone();
        Assert.True(exit2.HasCondition);
        Assert.Equal("con1,! con2", exit2.AllConditions);
        exit2 = exit.Clone();
        exit2.Conditions = [];
        Assert.False(exit2.HasCondition);
        Assert.Equal("", exit2.AllConditions);

        List<ExitLabel> labels;
        exit2 = exit.Clone();
        labels = exit2.Labels;
        Assert.Equal(5, labels.Count);
        Assert.Equal(ExitLabel.Types.Command, labels[0].Type);
        Assert.Equal("cmd", labels[0].Value);
        Assert.True(labels[0].IsCommand);
        Assert.False(labels[0].IsTo);
        Assert.False(labels[0].IsCondition);
        Assert.False(labels[0].IsExCondition);
        Assert.False(labels[0].IsCost);
        Assert.Equal(ExitLabel.Types.To, labels[1].Type);
        Assert.Equal("to", labels[1].Value);
        Assert.False(labels[1].IsCommand);
        Assert.True(labels[1].IsTo);
        Assert.False(labels[1].IsCondition);
        Assert.False(labels[1].IsExCondition);
        Assert.False(labels[1].IsCost);
        Assert.Equal(ExitLabel.Types.Condition, labels[2].Type);
        Assert.Equal("con1", labels[2].Value);
        Assert.False(labels[2].IsCommand);
        Assert.False(labels[2].IsTo);
        Assert.True(labels[2].IsCondition);
        Assert.False(labels[2].IsExCondition);
        Assert.False(labels[2].IsCost);
        Assert.Equal(ExitLabel.Types.ExCondition, labels[3].Type);
        Assert.Equal("con2", labels[3].Value);
        Assert.False(labels[3].IsCommand);
        Assert.False(labels[3].IsTo);
        Assert.False(labels[3].IsCondition);
        Assert.True(labels[3].IsExCondition);
        Assert.False(labels[3].IsCost);
        Assert.Equal(ExitLabel.Types.Cost, labels[4].Type);
        Assert.Equal("2", labels[4].Value);
        Assert.False(labels[4].IsCommand);
        Assert.False(labels[4].IsTo);
        Assert.False(labels[4].IsCondition);
        Assert.False(labels[4].IsExCondition);
        Assert.True(labels[4].IsCost);
        exit2 = new Exit();
        exit2.Command = "cmd";
        exit2.To = "to";
        Assert.Equal(1, exit2.Cost);
        labels = exit2.Labels;
        Assert.Equal(2, labels.Count);
        Assert.Equal(ExitLabel.Types.Command, labels[0].Type);
        Assert.Equal("cmd", labels[0].Value);
        Assert.Equal(ExitLabel.Types.To, labels[1].Type);
        Assert.Equal("to", labels[1].Value);
    }
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