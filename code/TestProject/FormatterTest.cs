
using HellMapManager.Models;

namespace TestProject;

public class FormatterTest
{
    [Fact]
    public void TestBasic()
    {
        Assert.Equal("\\>\\:\\=\\@\\!\\;\\\\\\,\\&\\!\\n", HMMFormatter.Escaper.Pack(HMMFormatter.Escape(">:=@!;\\,&!\n")));
        Assert.Equal(">:=@!;\\,&!\n>:=@!;,&!\n", HMMFormatter.Unescape(HMMFormatter.Escaper.Unpack("\\>\\:\\=\\@\\!\\;\\\\\\,\\&\\!\\n>:=@!;,&!\n")));
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
        Assert.Equal(HMMFormatter.Level1.SepToken.EncodedCode, HMMFormatter.At(list, 5));
        Assert.Equal("", HMMFormatter.At(list, -1));
        Assert.Equal("", HMMFormatter.At(list, 99));
        Assert.Equal("|", HMMFormatter.UnescapeAt(list, 5));
        Assert.Equal("", HMMFormatter.UnescapeAt(list, -1));
        Assert.Equal("", HMMFormatter.UnescapeAt(list, 99));
        IsListEqual(list, HMMFormatter.EscapeList(unescapedList));
        IsListEqual(list, HMMFormatter.DecodeList(HMMFormatter.Level1, HMMFormatter.EncodeList(HMMFormatter.Level1, list)));
        IsListEqual([], HMMFormatter.DecodeList(HMMFormatter.Level1, ""));
        IsListEqual(list, HMMFormatter.DecodeList(HMMFormatter.Level2, HMMFormatter.EncodeList(HMMFormatter.Level2, list)));
        IsListEqual([], HMMFormatter.DecodeList(HMMFormatter.Level2, ""));
        IsListEqual(list, HMMFormatter.DecodeList(HMMFormatter.Level3, HMMFormatter.EncodeList(HMMFormatter.Level3, list)));
        IsListEqual([], HMMFormatter.DecodeList(HMMFormatter.Level3, ""));
        IsListEqual(list, HMMFormatter.DecodeList(HMMFormatter.Level4, HMMFormatter.EncodeList(HMMFormatter.Level4, list)));
        IsListEqual([], HMMFormatter.DecodeList(HMMFormatter.Level4, ""));

        Assert.NotEqual(HMMFormatter.EncodeList(HMMFormatter.Level1, list), HMMFormatter.EncodeList(HMMFormatter.Level2, list));
        Assert.NotEqual(HMMFormatter.EncodeList(HMMFormatter.Level1, list), HMMFormatter.EncodeList(HMMFormatter.Level3, list));
        Assert.NotEqual(HMMFormatter.EncodeList(HMMFormatter.Level1, list), HMMFormatter.EncodeList(HMMFormatter.Level4, list));
        Assert.NotEqual(HMMFormatter.EncodeList(HMMFormatter.Level2, list), HMMFormatter.EncodeList(HMMFormatter.Level3, list));
        Assert.NotEqual(HMMFormatter.EncodeList(HMMFormatter.Level2, list), HMMFormatter.EncodeList(HMMFormatter.Level4, list));
        Assert.NotEqual(HMMFormatter.EncodeList(HMMFormatter.Level3, list), HMMFormatter.EncodeList(HMMFormatter.Level4, list));
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

        IsKeyValueEqual(kv, HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, HMMFormatter.EncodeKeyValue(HMMFormatter.Level1, kv)));
        Assert.Equal(HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, kv.Key, kv.Value), HMMFormatter.EncodeKeyValue(HMMFormatter.Level1, kv));
        Assert.Equal($"key{HMMFormatter.Level1.KeyToken.Raw}", HMMFormatter.EncodeKeyValue(HMMFormatter.Level1, kv2));
        IsKeyValueEqual(kv2, HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, "key"));

        IsKeyValueEqual(kv, HMMFormatter.DecodeKeyValue(HMMFormatter.Level2, HMMFormatter.EncodeKeyValue(HMMFormatter.Level2, kv)));
        Assert.Equal(HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level2, kv.Key, kv.Value), HMMFormatter.EncodeKeyValue(HMMFormatter.Level2, kv));
        Assert.Equal($"key{HMMFormatter.Level2.KeyToken.Raw}", HMMFormatter.EncodeKeyValue(HMMFormatter.Level2, kv2));
        IsKeyValueEqual(kv2, HMMFormatter.DecodeKeyValue(HMMFormatter.Level2, "key"));

        IsKeyValueEqual(kv, HMMFormatter.DecodeKeyValue(HMMFormatter.Level3, HMMFormatter.EncodeKeyValue(HMMFormatter.Level3, kv)));
        Assert.Equal(HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level3, kv.Key, kv.Value), HMMFormatter.EncodeKeyValue(HMMFormatter.Level3, kv));
        Assert.Equal($"key{HMMFormatter.Level3.KeyToken.Raw}", HMMFormatter.EncodeKeyValue(HMMFormatter.Level3, kv2));
        IsKeyValueEqual(kv2, HMMFormatter.DecodeKeyValue(HMMFormatter.Level3, "key"));

        IsKeyValueEqual(kv, HMMFormatter.DecodeKeyValue(HMMFormatter.Level4, HMMFormatter.EncodeKeyValue(HMMFormatter.Level4, kv)));
        Assert.Equal(HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level4, kv.Key, kv.Value), HMMFormatter.EncodeKeyValue(HMMFormatter.Level4, kv));
        Assert.Equal($"key{HMMFormatter.Level4.KeyToken.Raw}", HMMFormatter.EncodeKeyValue(HMMFormatter.Level4, kv2));
        IsKeyValueEqual(kv2, HMMFormatter.DecodeKeyValue(HMMFormatter.Level4, "key"));

        Assert.NotEqual(HMMFormatter.EncodeKeyValue(HMMFormatter.Level1, kv), HMMFormatter.EncodeKeyValue(HMMFormatter.Level2, kv));
        Assert.NotEqual(HMMFormatter.EncodeKeyValue(HMMFormatter.Level1, kv), HMMFormatter.EncodeKeyValue(HMMFormatter.Level3, kv));
        Assert.NotEqual(HMMFormatter.EncodeKeyValue(HMMFormatter.Level1, kv), HMMFormatter.EncodeKeyValue(HMMFormatter.Level4, kv));
        Assert.NotEqual(HMMFormatter.EncodeKeyValue(HMMFormatter.Level2, kv), HMMFormatter.EncodeKeyValue(HMMFormatter.Level3, kv));
        Assert.NotEqual(HMMFormatter.EncodeKeyValue(HMMFormatter.Level2, kv), HMMFormatter.EncodeKeyValue(HMMFormatter.Level4, kv));
        Assert.NotEqual(HMMFormatter.EncodeKeyValue(HMMFormatter.Level3, kv), HMMFormatter.EncodeKeyValue(HMMFormatter.Level4, kv));

    }

    private static void IsToggleKeyValuesEqual(ToggleKeyValues src, ToggleKeyValues dst)
    {
        Assert.Equal(src.Key, dst.Key);
        Assert.Equal(src.Not, dst.Not);
        IsListEqual(src.Values, dst.Values);
    }

    [Fact]
    public void TestToggleKeyValues()
    {
        var tkv = new ToggleKeyValues(HMMFormatter.Escaper.Unpack("\\>\\:\\=\\@\\|\\;\\,\\&\\!"),
        new([
            HMMFormatter.Escaper.Unpack(""), HMMFormatter.Escaper.Unpack("\\>"), HMMFormatter.Escaper.Unpack("\\:"), HMMFormatter.Escaper.Unpack("\\="), HMMFormatter.Escaper.Unpack("\\@"),
        HMMFormatter.Escaper.Unpack("\\|"), HMMFormatter.Escaper.Unpack("\\;"), HMMFormatter.Escaper.Unpack("\\,"), HMMFormatter.Escaper.Unpack("\\&"), HMMFormatter.Escaper.Unpack("\\!"),
        ]),
        true);
        Assert.Equal(">:=@|;,&!", tkv.ToTypedConditions().Key);
        IsListEqual(["", ">", ":", "=", "@", "|", ";", ",", "&", "!"], tkv.ToTypedConditions().Conditions);
        Assert.True(tkv.ToTypedConditions().Not);

        var tkv2 = new ToggleKeyValues("", [], false);
        Assert.Equal("", tkv2.ToTypedConditions().Key);
        IsListEqual([], tkv2.ToTypedConditions().Conditions);
        Assert.False(tkv2.ToTypedConditions().Not);

        IsToggleKeyValuesEqual(tkv, ToggleKeyValues.FromTypedConditions(tkv.ToTypedConditions()));

        IsToggleKeyValuesEqual(tkv, HMMFormatter.DecodeToggleKeyValues(HMMFormatter.Level1, HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level1, tkv)));
        IsToggleKeyValuesEqual(tkv, HMMFormatter.DecodeToggleKeyValues(HMMFormatter.Level2, HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level2, tkv)));
        IsToggleKeyValuesEqual(tkv, HMMFormatter.DecodeToggleKeyValues(HMMFormatter.Level3, HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level3, tkv)));
        IsToggleKeyValuesEqual(tkv, HMMFormatter.DecodeToggleKeyValues(HMMFormatter.Level4, HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level4, tkv)));

        Assert.NotEqual(HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level1, tkv), HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level2, tkv));
        Assert.NotEqual(HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level1, tkv), HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level3, tkv));
        Assert.NotEqual(HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level1, tkv), HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level4, tkv));
        Assert.NotEqual(HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level2, tkv), HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level3, tkv));
        Assert.NotEqual(HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level2, tkv), HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level4, tkv));
        Assert.NotEqual(HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level3, tkv), HMMFormatter.EncodeToggleKeyValues(HMMFormatter.Level4, tkv));
    }

    private static void IsToggleKeyValueEqual(ToggleKeyValue src, ToggleKeyValue dst)
    {
        Assert.Equal(src.Key, dst.Key);
        Assert.Equal(src.Not, dst.Not);
        Assert.Equal(src.Value, dst.Value);
    }


    [Fact]
    public void TestToggleKeyValue()
    {
        var tkv = new ToggleKeyValue(
            HMMFormatter.Escaper.Unpack("\\>\\:\\=\\@\\|\\;\\,\\&\\!"),
            HMMFormatter.Escaper.Unpack("\\!\\&\\,\\;\\|\\@\\=\\:\\>"),
            true
        );
        Assert.Equal(">:=@|;,&!", tkv.UnescapeKey());
        Assert.Equal("!&,;|@=:>", tkv.UnescapeValue());
        Assert.True(tkv.Not);
        var tkv2 = new ToggleKeyValue("", "", false);
        Assert.Equal("", tkv2.UnescapeKey());
        Assert.Equal("", tkv2.UnescapeValue());
        Assert.False(tkv2.Not);
        RegionItem ri;
        ToggleKeyValue tkvri;
        ri = new ToggleKeyValue("Room", "RoomValue", true).ToRegionItem();
        Assert.Equal(RegionItemType.Room, ri.Type);
        Assert.Equal("RoomValue", ri.Value);
        Assert.True(ri.Not);

        tkvri = ToggleKeyValue.FromRegionItem(ri);
        Assert.Equal("Room", tkvri.UnescapeKey());
        Assert.Equal("RoomValue", tkvri.UnescapeValue());
        Assert.True(tkvri.Not);

        ri = new ToggleKeyValue("Zone", "ZoneValue", false).ToRegionItem();
        Assert.Equal(RegionItemType.Zone, ri.Type);
        Assert.Equal("ZoneValue", ri.Value);
        Assert.False(ri.Not);

        tkvri = ToggleKeyValue.FromRegionItem(ri);
        Assert.Equal("Zone", tkvri.UnescapeKey());
        Assert.Equal("ZoneValue", tkvri.UnescapeValue());
        Assert.False(tkvri.Not);


        ri = new ToggleKeyValue("Other", "OtherValue", true).ToRegionItem();
        Assert.Equal(RegionItemType.Zone, ri.Type);
        Assert.Equal("OtherValue", ri.Value);
        Assert.True(ri.Not);

        tkvri = ToggleKeyValue.FromRegionItem(ri);
        Assert.Equal("Zone", tkvri.UnescapeKey());
        Assert.Equal("OtherValue", tkvri.UnescapeValue());
        Assert.True(tkvri.Not);


        IsToggleKeyValueEqual(tkv, HMMFormatter.DecodeToggleKeyValue(HMMFormatter.Level1, HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level1, tkv)));
        IsToggleKeyValueEqual(tkv, HMMFormatter.DecodeToggleKeyValue(HMMFormatter.Level2, HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level2, tkv)));
        IsToggleKeyValueEqual(tkv, HMMFormatter.DecodeToggleKeyValue(HMMFormatter.Level3, HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level3, tkv)));
        IsToggleKeyValueEqual(tkv, HMMFormatter.DecodeToggleKeyValue(HMMFormatter.Level4, HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level4, tkv)));

        Assert.NotEqual(HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level1, tkv), HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level2, tkv));
        Assert.NotEqual(HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level1, tkv), HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level3, tkv));
        Assert.NotEqual(HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level1, tkv), HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level4, tkv));
        Assert.NotEqual(HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level2, tkv), HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level3, tkv));
        Assert.NotEqual(HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level2, tkv), HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level4, tkv));
        Assert.NotEqual(HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level3, tkv), HMMFormatter.EncodeToggleKeyValue(HMMFormatter.Level4, tkv));
    }
    [Fact]
    public void TestInt()
    {
        Assert.Equal(1, HMMFormatter.UnescapeInt("1", 0));
        Assert.Equal(-1, HMMFormatter.UnescapeInt("-1", 0));
        Assert.Equal(1, HMMFormatter.UnescapeInt("a", 1));
        Assert.Equal(2, HMMFormatter.UnescapeInt("0.1", 2));
        var list = new List<string>(["1", "-1", "a", "0.1"]);
        Assert.Equal(99, HMMFormatter.UnescapeIntAt(list, -1, 99));
        Assert.Equal(99, HMMFormatter.UnescapeIntAt(list, 100, 99));
        Assert.Equal(1, HMMFormatter.UnescapeIntAt(list, 0, 99));
        Assert.Equal(-1, HMMFormatter.UnescapeIntAt(list, 1, 99));
        Assert.Equal(99, HMMFormatter.UnescapeIntAt(list, 2, 99));
        Assert.Equal(99, HMMFormatter.UnescapeIntAt(list, 3, 99));
    }


    private static void IsToggleValueEqual(ToggleValue src, ToggleValue dst)
    {
        Assert.Equal(src.Not, dst.Not);
        Assert.Equal(src.Value, dst.Value);
    }
    [Fact]
    public void TestToggleValue()
    {
        var tv = new ToggleValue(HMMFormatter.Escaper.Unpack("\\>\\:\\=\\@\\|\\;\\,\\&\\!"), true);
        Assert.Equal(">:=@|;,&!", tv.UnescapeValue());
        Assert.True(tv.Not);
        IsToggleValueEqual(tv, HMMFormatter.DecodeToggleValue(HMMFormatter.EncodeToggleValue(tv)));

        var tv2 = new ToggleValue(HMMFormatter.Escaper.Unpack(""), false);
        Assert.Equal("", tv2.UnescapeValue());
        Assert.False(tv2.Not);
        IsToggleValueEqual(tv2, HMMFormatter.DecodeToggleValue(HMMFormatter.EncodeToggleValue(tv2)));

        Condition co;
        co = tv.ToCondition();
        Assert.Equal(">:=@|;,&!", co.Key);
        Assert.True(co.Not);
        var tvco = ToggleValue.FromCondition(co);
        Assert.Equal(">:=@|;,&!", tvco.UnescapeValue());
        Assert.True(tvco.Not);
    }
}

