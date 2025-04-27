
using HellMapManager.Models;

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
    private static void IsListEqual(List<string> src, List<string> dst)
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
        Assert.Equal(HMMFormatter.TokenSep1.EncodedCode, HMMFormatter.At(list, 5));
        Assert.Equal("", HMMFormatter.At(list, -1));
        Assert.Equal("", HMMFormatter.At(list, 99));
        Assert.Equal("|", HMMFormatter.UnescapeAt(list, 5));
        Assert.Equal("", HMMFormatter.UnescapeAt(list, -1));
        Assert.Equal("", HMMFormatter.UnescapeAt(list, 99));
        IsListEqual(list, HMMFormatter.EscapeList(unescapedList));
        IsListEqual(list, HMMFormatter.DecodeList1(HMMFormatter.EncodeList1(list)));
        IsListEqual([], HMMFormatter.DecodeList1(""));
        IsListEqual(list, HMMFormatter.DecodeList2(HMMFormatter.EncodeList2(list)));
        IsListEqual([], HMMFormatter.DecodeList2(""));
        IsListEqual(list, HMMFormatter.DecodeList3(HMMFormatter.EncodeList3(list)));
        IsListEqual([], HMMFormatter.DecodeList3(""));
        IsListEqual(list, HMMFormatter.DecodeList4(HMMFormatter.EncodeList4(list)));
        IsListEqual([], HMMFormatter.DecodeList4(""));

        Assert.NotEqual(HMMFormatter.EncodeList1(list), HMMFormatter.EncodeList2(list));
        Assert.NotEqual(HMMFormatter.EncodeList1(list), HMMFormatter.EncodeList3(list));
        Assert.NotEqual(HMMFormatter.EncodeList1(list), HMMFormatter.EncodeList4(list));
        Assert.NotEqual(HMMFormatter.EncodeList2(list), HMMFormatter.EncodeList3(list));
        Assert.NotEqual(HMMFormatter.EncodeList2(list), HMMFormatter.EncodeList4(list));
        Assert.NotEqual(HMMFormatter.EncodeList3(list), HMMFormatter.EncodeList4(list));
    }
    private static void IsKeyValueEqual(KeyValue src, KeyValue dst)
    {
        Assert.Equal(src.Key, dst.Key);
        Assert.Equal(src.Value, dst.Value);
    }

    [Fact]
    public void TestKeyValue()
    {
        var kv = new KeyValue(HMMFormatter.Escaper.Unpack("\\>\\!\\=\\@"), HMMFormatter.Escaper.Unpack("\\@\\=\\!\\>"));
        var kv2 = new KeyValue("key", "");
        Assert.Equal(">!=@", kv.UnescapeKey());
        Assert.Equal("@=!>", kv.UnescapeValue());
        var data = kv.ToData();
        Assert.Equal(">!=@", data.Key);
        Assert.Equal("@=!>", data.Value);
        IsKeyValueEqual(kv, KeyValue.FromData(data));

        IsKeyValueEqual(kv, HMMFormatter.DecodeKeyValue1(HMMFormatter.EncodeKeyValue1(kv)));
        Assert.Equal(HMMFormatter.EncodeKeyAndValue1(kv.Key, kv.Value), HMMFormatter.EncodeKeyValue1(kv));
        Assert.Equal($"key{HMMFormatter.TokenKey1.Raw}", HMMFormatter.EncodeKeyValue1(kv2));
        IsKeyValueEqual(kv2, HMMFormatter.DecodeKeyValue1("key"));

        IsKeyValueEqual(kv, HMMFormatter.DecodeKeyValue2(HMMFormatter.EncodeKeyValue2(kv)));
        Assert.Equal(HMMFormatter.EncodeKeyAndValue2(kv.Key, kv.Value), HMMFormatter.EncodeKeyValue2(kv));
        Assert.Equal($"key{HMMFormatter.TokenKey2.Raw}", HMMFormatter.EncodeKeyValue2(kv2));
        IsKeyValueEqual(kv2, HMMFormatter.DecodeKeyValue2("key"));

        IsKeyValueEqual(kv, HMMFormatter.DecodeKeyValue3(HMMFormatter.EncodeKeyValue3(kv)));
        Assert.Equal(HMMFormatter.EncodeKeyAndValue3(kv.Key, kv.Value), HMMFormatter.EncodeKeyValue3(kv));
        Assert.Equal($"key{HMMFormatter.TokenKey3.Raw}", HMMFormatter.EncodeKeyValue3(kv2));
        IsKeyValueEqual(kv2, HMMFormatter.DecodeKeyValue3("key"));

        IsKeyValueEqual(kv, HMMFormatter.DecodeKeyValue4(HMMFormatter.EncodeKeyValue4(kv)));
        Assert.Equal(HMMFormatter.EncodeKeyAndValue4(kv.Key, kv.Value), HMMFormatter.EncodeKeyValue4(kv));
        Assert.Equal($"key{HMMFormatter.TokenKey4.Raw}", HMMFormatter.EncodeKeyValue4(kv2));
        IsKeyValueEqual(kv2, HMMFormatter.DecodeKeyValue4("key"));

        Assert.NotEqual(HMMFormatter.EncodeKeyValue1(kv), HMMFormatter.EncodeKeyValue2(kv));
        Assert.NotEqual(HMMFormatter.EncodeKeyValue1(kv), HMMFormatter.EncodeKeyValue3(kv));
        Assert.NotEqual(HMMFormatter.EncodeKeyValue1(kv), HMMFormatter.EncodeKeyValue4(kv));
        Assert.NotEqual(HMMFormatter.EncodeKeyValue2(kv), HMMFormatter.EncodeKeyValue3(kv));
        Assert.NotEqual(HMMFormatter.EncodeKeyValue2(kv), HMMFormatter.EncodeKeyValue4(kv));
        Assert.NotEqual(HMMFormatter.EncodeKeyValue3(kv), HMMFormatter.EncodeKeyValue4(kv));

    }
}

