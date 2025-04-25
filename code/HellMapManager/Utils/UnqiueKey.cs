using System.Linq;
namespace HellMapManager.Utils;

public static class UniqueKeyUtil
{
    public const string Sep = "\n";
    public const string EscapedSep = "\\n";
    public static readonly Escaper Escaper = (new Escaper())
    .WithItem(Sep, EscapedSep)
    ;
    public static string Escape(string val)
    {
        return Escaper.Escape(val);
    }
    public static string Join(string[] str)
    {
        return string.Join(Sep, str.Select(s => Escape(s)));
    }
}