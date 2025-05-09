namespace HellMapManager.Models;

public class ItemKey
{
    public static bool Validate(string key)
    {
        return key != "" && !key.Contains('\n');
    }
}