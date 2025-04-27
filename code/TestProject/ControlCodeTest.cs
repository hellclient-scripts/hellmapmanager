namespace TestProject;
using HellMapManager.Utils.ControlCode;
using Xunit;
public class ControlCodeTest
{
    [Fact]
    public void TestEscape()
    {
        var cc = new ControlCode();
        cc.WithCommand(new Command("\\", "0", "\\\\"));
        cc.WithCommand(new Command("\n", "1", "\\n"));
        cc.WithCommand(new Command("", "2", "\\"));
        //转义保留符号
        Assert.Equal("This is a test \x02 string.", cc.Escape("This is a test \x02 string."));
        Assert.Equal("This is a test \x02 string.", cc.Unescape("This is a test \x02 string."));
        Assert.Equal("This is a test \x03 string.", cc.Escape("This is a test \x03 string."));
        Assert.Equal("This is a test \x03 string.", cc.Unescape("This is a test \x03 string."));
        Assert.Equal("This is a test \x04 string.", cc.Escape("This is a test \x04 string."));
        Assert.Equal("This is a test \x04 string.", cc.Unescape("This is a test \x04 string."));

        //转义斜杠
        Assert.Equal("This is a test \\\\ string.", cc.Escape("This is a test \\ string."));
        Assert.Equal("This is a test \\ string.", cc.Unescape("This is a test \\\\ string."));
        //测试转义
        Assert.Equal("This is a test \\n string.", cc.Escape("This is a test \n string."));
        //测试解转义
        Assert.Equal("This is a test \n string.", cc.Unescape("This is a test \\n string."));
        //测试一致性
        Assert.Equal("This is a test \\ string.", cc.Unescape(cc.Escape("This is a test \\ string.")));
        //测试独立的斜杠转解意会被去除
        Assert.Equal("This is a test  string.", cc.Unescape("This is a test \\ string."));

    }
    [Fact]
    public void TestInternal()
    {
        var cc = new ControlCode()
        .WithCommand(new Command("\x02", "a", "\\2"))
        .WithCommand(new Command("\x03", "b", "\\3"))
        .WithCommand(new Command("\x04", "c", "\\4"))
        ;
        Assert.Equal("\x02", cc.Unescape("\\2"));
        Assert.Equal("\x03", cc.Unescape("\\3"));
        Assert.Equal("\x04", cc.Unescape("\\4"));

        Assert.Equal("\\2", cc.Escape("\x02"));
        Assert.Equal("\\3", cc.Escape("\x03"));
        Assert.Equal("\\4", cc.Escape("\x04"));

        Assert.Equal("\x04\x02", cc.Unescape("\\4\\2"));
        Assert.Equal("\\4\\2", cc.Escape("\x04\x02"));
        Assert.Equal("\x04\x03", cc.Unescape("\\4\\3"));
        Assert.Equal("\\4\\3", cc.Escape("\x04\x03"));
        Assert.Equal("\x04\x04", cc.Unescape("\\4\\4"));
        Assert.Equal("\\4\\4", cc.Escape("\x04\x04"));
    }
    [Fact]
    public void TestMulti()
    {
        var cc = new ControlCode()
         .WithCommand(new Command("\\", "0", "\\\\"))
        .WithCommand(new Command("\n", "1", "\\n"))
        .WithCommand(new Command("%", "2", "\\%"))
        .WithCommand(new Command("$", "3", "\\$"))
        .WithCommand(new Command("", "4", "\\"))
        ;
        Assert.Equal("a\n%$b\\n\\%\\$$0$1\\$0\\$1", cc.Unescape("a\\n\\%\\$b\\\\n\\\\%\\\\$\\$0\\$1\\\\$0\\\\$1"));
    }
}
