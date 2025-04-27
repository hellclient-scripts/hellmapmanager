namespace TestProject;
using HellMapManager.Models;
using HellMapManager.Utils;

public class UniqueKeyUtilTest
{
    [Fact]
    public void TestBasic()
    {
        Assert.Equal($"a\n{UniqueKeyUtil.EscapedSep}\n{UniqueKeyUtil.EscapedEscapeToken}", UniqueKeyUtil.Join(["a","\n",UniqueKeyUtil.EscapeToken]));
    }
}