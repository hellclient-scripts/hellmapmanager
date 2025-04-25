
using HellMapManager.Models;

namespace TestProject;

public class FormatterTest
{
    [Fact]
    public void TestBasic()
    {
        Assert.Equal("\\>\\:\\=\\@\\!\\;\\,\\&\\!\\n", HMMFormatter.Escape(">:=@!;,&!\n"));
        Assert.Equal(">:=@!;,&!\n>:=@!;,&!\n", HMMFormatter.Unescape("\\>\\:\\=\\@\\!\\;\\,\\&\\!\\n>:=@!;,&!\n"));
        Assert.Equal(">:=@!;,&!\n", HMMFormatter.Unescape(HMMFormatter.Escape(">:=@!;,&!\n")));
    }

}
