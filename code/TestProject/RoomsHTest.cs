using HellMapManager.Services;

namespace TestProject;

public class RoomsHTest
{
    [Fact]
    public void TestCommend()
    {
        var body = "\n  \n \\\\abcd\n\\\\efgh";
        Assert.Empty(RoomsH.Load(body));
        body = "0=123|e:1";
        Assert.Single(RoomsH.Load(body));

    }
}