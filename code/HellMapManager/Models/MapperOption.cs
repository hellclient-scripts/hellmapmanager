namespace HellMapManager.Models;

//地图规划选项
public class MapperOptions
{
    //最大单步消耗，为0不限制
    public int MaxExitCost = 0;
    //最大总消耗，为0不限制
    public int MaxTotalCost = 0;
    //是否禁用捷径
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