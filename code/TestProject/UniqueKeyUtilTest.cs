namespace TestProject;
using HellMapManager.Models;
using HellMapManager.Utils;

public class UniqueKeyUtilTest
{
    [Fact]
    public void TestBasic()
    {
        Assert.Equal("a\n\\n\n\\\\n", UniqueKeyUtil.Join(["a","\n","\\n"]));
    }
}