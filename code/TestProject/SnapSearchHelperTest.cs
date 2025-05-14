using HellMapManager.Models;
using HellMapManager.Helpers;

namespace TestProject;

public class SnapSearchHelperTest
{
    [Fact]
    public void TestSnapSearch()
    {
        var snapshots = new List<Snapshot>
        {
            new Snapshot(){Key = "test1", Type = "type1", Group = "group1", Value = "value1",Count=1},
            new Snapshot(){Key = "test1", Type = "type1", Group = "group1", Value = "value2",Count=2},
            new Snapshot(){Key = "test2", Type = "type2", Group = "group1", Value = "value1",Count=3},
            new Snapshot(){Key = "test1", Type = "type2", Group = "group1", Value = "value1",Count=4},
            new Snapshot(){Key = "test2", Type = "type1", Group = "group2", Value = "value3",Count=5},
        };
        var ss = new SnapshotSearch(){};
        var sr = SnapshotHelper.Search(ss, snapshots);
        Assert.Equal(2, sr.Count);
        Assert.Equal("test1", sr[0].Key);
        Assert.Equal(3, sr[0].Items.Count);
        Assert.Equal(7, sr[0].Sum);
        Assert.Equal(7, sr[0].Count);
        Assert.Equal("test2", sr[1].Key);
        Assert.Equal(2, sr[1].Items.Count);
        Assert.Equal(8, sr[1].Sum);
        Assert.Equal(8, sr[1].Count);
        ss = new SnapshotSearch()
        {
            Type = "type1",
            Group = "group1",
            Keywords = new List<string> { "va", "lue" },
            PartialMatch = true,
            Any = false,
        };
        sr = SnapshotHelper.Search(ss, snapshots);
        Assert.Single(sr);
        Assert.Equal("test1", sr[0].Key);
        Assert.Equal(2, sr[0].Items.Count);
        Assert.Equal(7, sr[0].Sum);
        Assert.Equal(3, sr[0].Count);
        ss = new SnapshotSearch()
        {
            Type = "type2",
            Keywords = new List<string> { "value1", "value" },
            PartialMatch = false,
            Any = true,
        };
        sr = SnapshotHelper.Search(ss, snapshots);
        Assert.Equal(2, sr.Count);
        Assert.Equal("test1", sr[0].Key);
        Assert.Single(sr[0].Items);
        Assert.Equal(7, sr[0].Sum);
        Assert.Equal(4, sr[0].Count);
        Assert.Equal("test2", sr[1].Key);
        Assert.Single(sr[1].Items);
        Assert.Equal(8, sr[1].Sum);
        Assert.Equal(3, sr[1].Count);
        ss = new SnapshotSearch()
        {
            Keywords = new List<string> { "value1", "value2" },
            PartialMatch = false,
            Any = false,
        };
        sr = SnapshotHelper.Search(ss, snapshots);
        Assert.Empty(sr);
    }
}