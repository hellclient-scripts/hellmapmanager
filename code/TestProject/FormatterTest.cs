
using HellMapManager.Models;
using Microsoft.VisualBasic;

namespace TestProject;

public class FormatterTest
{
    [Fact]
    public void TestBasic()
    {
        Assert.Equal("\\>\\:\\=\\@\\!\\;\\,\\&\\!\\n", HMMFormatter.Escaper.Pack(HMMFormatter.Escape(">:=@!;,&!\n")));
        Assert.Equal(">:=@!;,&!\n>:=@!;,&!\n", HMMFormatter.Unescape(HMMFormatter.Escaper.Unpack("\\>\\:\\=\\@\\!\\;\\,\\&\\!\\n>:=@!;,&!\n")));
        Assert.Equal(">:=@!;,&!\n", HMMFormatter.Escaper.Pack(HMMFormatter.Unescape(HMMFormatter.Escape(">:=@!;,&!\n"))));
    }
    public static void IsListEqual(List<string> src, List<string> dst)
    {
        Assert.Equal(src.Count, dst.Count);

        for (var i = 0; i < src.Count; i++)
        {
            Assert.Equal(src[i], dst[i]);
        }
    }
    [Fact]
    public void TestList()
    {
        var list = new List<string>(["1", "2", "\n", "", "", "|", ",", ";", "&", "\\"]).ConvertAll(HMMFormatter.Escaper.Unpack).ConvertAll(HMMFormatter.Escape);
        var unescapedList = HMMFormatter.UnescapeList(list);
        IsListEqual(list, HMMFormatter.EscapeList(unescapedList));
        IsListEqual(list, HMMFormatter.DecodeList1(HMMFormatter.EncodeList1(list)));
    }
}
