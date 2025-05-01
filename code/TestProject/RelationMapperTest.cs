namespace TestProject;
using HellMapManager.Services;
using HellMapManager.Models;
using HellMapManager.States;

public class RelationMapperTest
{
    [Fact]
    public void TestRelationMapper()
    {
        RelationMapItem? rm;
        RelationMapItem? rm1;
        Relation relation;
        var state = new AppState();
        var mf = MapFile.Create("testname", "testdesc");
        state.Current = mf;
        var room1 = new Room() { Key = "key1" };
        var room2 = new Room()
        {
            Key = "key2",
            Exits = [
                new Exit() { Command = "cmd", To = "key3" },
                new Exit() { Command = "cmd1", To = "key3" },
                new Exit() { Command = "cmd2", To = "key4" },
                new Exit() { Command = "cmd3", To = "" },
                new Exit() { Command = "cmd4", To = "notfound" },
                new Exit() { Command = "cmd5", To = "key2" }
            ]
        };
        var room3 = new Room() { Key = "key3", Exits = [new Exit() { Command = "cmd", To = "key4" }, new Exit() { Command = "cmd2", To = "key5" }] };
        var room4 = new Room() { Key = "key4", Exits = [new Exit() { Command = "cmd", To = "key3" }, new Exit() { Command = "cmd2", To = "key2" }] };
        var room5 = new Room() { Key = "key5" };

        state.APIInsertRoom(room1);
        state.APIInsertRoom(room2);
        state.APIInsertRoom(room3);
        state.APIInsertRoom(room4);
        state.APIInsertRoom(room5);

        rm = RelationMapper.RelationMap(mf, "notfound", 1);
        Assert.Null(rm);
        rm = RelationMapper.RelationMap(mf, "key1", 1);
        Assert.NotNull(rm);
        Assert.Equal(0, rm.Depth);
        Assert.Equal(room1, rm.Room);
        Assert.Empty(rm.Relations);
        Assert.False(rm.HasRelation("notfound"));

        rm = RelationMapper.RelationMap(mf, "key2", 1);

        Assert.NotNull(rm);
        Assert.Equal(0, rm.Depth);
        Assert.Equal(room2, rm.Room);
        Assert.NotEmpty(rm.Relations);
        Assert.Equal(2, rm.Relations.Count);
        Assert.False(rm.HasRelation("notfound"));
        Assert.False(rm.HasRelation("key1"));
        Assert.True(rm.HasRelation("key3"));
        Assert.True(rm.HasRelation("key4"));

        relation = rm.Relations[0];
        Assert.Equal(RelationType.OneSideTo, relation.Type);
        rm1 = relation.Target;

        Assert.Equal(1, rm1.Depth);
        Assert.Equal(room3, rm1.Room);
        Assert.Empty(rm1.Relations);
        relation = rm.Relations[1];
        Assert.Equal(RelationType.TwoSide, relation.Type);
        rm1 = relation.Target;
        Assert.Equal(1, rm1.Depth);
        Assert.Equal(room4, rm1.Room);
        Assert.Empty(rm1.Relations);

        rm = RelationMapper.RelationMap(mf, "key2", 9);
        Assert.NotNull(rm);
        Assert.Equal(0, rm.Depth);
        Assert.Equal(room2, rm.Room);
        Assert.NotEmpty(rm.Relations);
        Assert.Equal(2, rm.Relations.Count);

        relation = rm.Relations[0];
        Assert.Equal(RelationType.OneSideTo, relation.Type);
        rm1 = relation.Target;

        Assert.Equal(1, rm1.Depth);
        Assert.Equal(room3, rm1.Room);
        Assert.Single(rm1.Relations);
        relation = rm1.Relations[0];
        Assert.Equal(RelationType.OneSideTo, relation.Type);
        rm1 = relation.Target;
        Assert.Equal(2, rm1.Depth);
        Assert.Equal(room5, rm1.Room);
        Assert.Empty(rm1.Relations);

        relation = rm.Relations[1];
        Assert.Equal(RelationType.TwoSide, relation.Type);
        rm1 = relation.Target;
        Assert.Equal(1, rm1.Depth);
        Assert.Equal(room4, rm1.Room);
        Assert.Empty(rm1.Relations);
    }
}