namespace HellMapManager.Models;

public class MapperOptions
{
    public int MaxExitCost = 0;
    public int MaxTotalCost = 0;
    public bool DisableShortcuts = false;
    public MapperOptions WithMaxExitCost(int cost)
    {
        MaxExitCost = cost;
        return this;
    }
    public MapperOptions WithMaxTotalCost(int cost)
    {
        MaxTotalCost = cost;
        return this;
    }
    public MapperOptions WithDisableShortcuts(bool disable)
    {
        DisableShortcuts = disable;
        return this;
    }

}