namespace TestProject;
using HellMapManager.Utils;
using Xunit;
public class EscaperTest
{
    [Fact]
    public void TestBasic()
    {
        var escaper = new Escaper();
        escaper.WithItem("\n", "\\n");
        //转义斜杠
        Assert.Equal("This is a test \\\\ string.", escaper.Escape("This is a test \\ string."));
        Assert.Equal("This is a test \\ string.", escaper.Unescape("This is a test \\\\ string."));
        //测试转义
        Assert.Equal("This is a test \\n string.", escaper.Escape("This is a test \n string."));
        //测试解转义
        Assert.Equal("This is a test \n string.", escaper.Unescape("This is a test \\n string."));
        //测试一致性
        Assert.Equal("This is a test \\ string.", escaper.Unescape(escaper.Escape("This is a test \\ string.")));
        //测试独立的斜杠转解意会被去除
        Assert.Equal("This is a test  string.", escaper.Unescape("This is a test \\ string."));

        //确保内部转义不出错
        Assert.Equal("This is a test $ string.", escaper.Escape("This is a test $ string."));
        Assert.Equal("This is a test $0 string.", escaper.Escape("This is a test $0 string."));
        Assert.Equal("This is a test $1 string.", escaper.Escape("This is a test $1 string."));
        Assert.Equal("This is a test $ string.", escaper.Unescape("This is a test $ string."));
        Assert.Equal("This is a test $0 string.", escaper.Unescape("This is a test $0 string."));
        Assert.Equal("This is a test $1 string.", escaper.Unescape("This is a test $1 string."));
        Assert.Equal("This is a test \\\\$ string.", escaper.Escape("This is a test \\$ string."));
        Assert.Equal("This is a test \\\\$0 string.", escaper.Escape("This is a test \\$0 string."));
        Assert.Equal("This is a test \\\\$1 string.", escaper.Escape("This is a test \\$1 string."));
        Assert.Equal("This is a test \\\\$2 string.", escaper.Escape("This is a test \\$2 string."));
        Assert.Equal("This is a test $ string.", escaper.Unescape("This is a test \\$ string."));
        Assert.Equal("This is a test $0 string.", escaper.Unescape("This is a test \\$0 string."));
        Assert.Equal("This is a test $1 string.", escaper.Unescape("This is a test \\$1 string."));
        Assert.Equal("This is a test $2 string.", escaper.Unescape("This is a test \\$2 string."));
    }
    [Fact]
    public void TestInternal()
    {
        var escaper = new Escaper()
        .WithItem("$", "\\$")
        ;
        Assert.Equal("$", escaper.Unescape("\\$"));
        Assert.Equal("$0", escaper.Unescape("\\$0"));
        Assert.Equal("$1", escaper.Unescape("\\$1"));
        Assert.Equal("$2", escaper.Unescape("\\$2"));

        Assert.Equal("\\$", escaper.Escape("$"));
        Assert.Equal("\\$0", escaper.Escape("$0"));
        Assert.Equal("\\$1", escaper.Escape("$1"));
        Assert.Equal("\\$2", escaper.Escape("$2"));

        Assert.Equal("$$", escaper.Unescape("\\$\\$"));
        Assert.Equal("\\$\\$", escaper.Escape("$$"));
    }
    [Fact]
    public void TestMulti()
    {
        var escaper = new Escaper()
        .WithItem("\n", "\\n")
        .WithItem("%", "\\%")
        .WithItem("$", "\\$")
        ;
        Assert.Equal("a\n%$b\\n\\%\\$$0$1\\$0\\$1", escaper.Unescape("a\\n\\%\\$b\\\\n\\\\%\\\\$\\$0\\$1\\\\$0\\\\$1"));
    }
}
