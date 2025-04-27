using System.Linq;
namespace HellMapManager.Utils;

public static class UniqueKeyUtil
{
    public const string EscapedSep="\x1B1";
    public const string EscapeToken="\x1B";
    public const string EscapedEscapeToken="\x1B0";
    public static string Escape(string val)
    {
        return val.Replace(EscapeToken, EscapedEscapeToken).Replace("\n", EscapedSep);
    }
    public static string Join(string[] str)
    {
        var escaped=str.Select(s => Escape(s)).ToArray();
        return string.Join("\n", escaped);
    }
}