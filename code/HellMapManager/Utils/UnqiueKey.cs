using System.Linq;
namespace HellMapManager.Utils;

public static class UniqueKeyUtil
{
    public const string Sep = "\n";
    public const string EscapedSep = "%0A";
    public const string Token = "%";
    public const string EscapedToken = "%25";
    public static string Escape(string str)
    {
        return str.Replace(Sep, EscapedSep).Replace(Token, EscapedToken);
    }
    public static string Join(string[] str)
    {
        return string.Join(Sep, str.Select(s => Escape(s)));
    }
}