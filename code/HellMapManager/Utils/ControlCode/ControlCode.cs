using System.Collections.Generic;

namespace HellMapManager.Utils.ControlCode;

//简易的带控制码的字符串转换类
//共有3个状态
//1.转义字符串，用于储存。用于人工读写的格式。
//2.控制码字符串，用于根据控制码进行附加操作，用于根据控制码进行操作。
//3.原始字符串，实际使用的字符串，代码中使用的实际业务格式。
//字符串共有3种操作
//Escape:原生字符串到转义字符串，a=>\a.对应Unescape
//Unpack:编码:转义字符串=>控制码字符串,\a=>%61对应Pack
//Decode:控制码到原生字符串,%61=>a,对应Encode

//命令实例，包含原始字符串，原始代码字符串和转义字符串。
//为了避免不可预期表现，原始代码字符串应该有独立的取值空间，不会被raw和escaped中使用。
public class Command(string raw, string rawcode, string escaped)
{
    //转义后的字符
    public readonly string Escaped = escaped;
    //编码后的控制代码
    public readonly string EncodedCode = ControlCode.EncodeCommand(rawcode);
    //原始字符
    public readonly string Raw = raw;
    //预处理后的字符，用于避免内部Token转换的问题
    public readonly string Encoded = ControlCode.PreEscape(raw);
}
public class ControlCode
{
    //指令开始字符
    public const string CodeStart = "\x02";
    //指令结束字符
    public const string CodeEnd = "\x03";
    //指令开始/结束转义字符
    public const string CodeEscape = "\x04";
    public const string EncodedEscape = "\x04\x04";
    public const string EncodedStart = "\x04\x05";
    public const string EncodedEnd = "\x04\x06";
    public static string PreEscape(string val)
    {
        return val.Replace(CodeEscape, EncodedEscape).Replace(CodeEnd, EncodedEnd).Replace(CodeStart, EncodedStart);
    }
    public static string PreUnescape(string val)
    {
        return val.Replace(EncodedStart, CodeStart).Replace(EncodedEnd, CodeEnd).Replace(EncodedEscape, CodeEscape);
    }
    public static string EncodeCommand(string val)
    {
        return CodeStart + PreEscape(val) + CodeEnd;
    }
    public List<Command> Commands = [];
    public ControlCode WithCommand(Command command)
    {
        Commands.Add(command);
        return this;
    }
    public string Encode(string val)
    {
        val = PreEscape(val);
        for (int i = 0; i < Commands.Count; i++)
        {
            var c = Commands[i];
            if (c.Encoded != c.EncodedCode && c.Encoded != "")
            {
                val = val.Replace(c.Encoded, c.EncodedCode);
            }
        }
        return val;
    }
    public string Decode(string val)
    {
        for (int i = 0; i < Commands.Count; i++)
        {
            var c = Commands[i];
            if (c.Raw != c.EncodedCode && c.EncodedCode != "")
            {
                val = val.Replace(c.EncodedCode, c.Encoded);
            }
        }
        val = PreUnescape(val);
        return val;
    }

    public string Pack(string val)
    {
        for (int i = 0; i < Commands.Count; i++)
        {
            var c = Commands[i];
            if (c.EncodedCode != c.Escaped && c.EncodedCode != "")
            {
                val = val.Replace(c.EncodedCode, c.Escaped);
            }
        }
        val = PreUnescape(val);
        return val;
    }
    public string Unpack(string val)
    {
        val = PreEscape(val);
        for (int i = 0; i < Commands.Count; i++)
        {
            var c = Commands[i];
            if (c.EncodedCode != c.Escaped && c.Escaped != "")
            {
                val = val.Replace(c.Escaped, c.EncodedCode);
            }
        }
        return val;
    }

    public string Escape(string val)
    {
        val = Encode(val);
        val = Pack(val);
        return val;
    }
    public string Unescape(string val)
    {
        val = Unpack(val);
        val = Decode(val);
        return val;
    }
}