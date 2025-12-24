using System.Collections.Generic;

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

    public Dictionary<string, bool> CommandWhitelist = new();
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
    public MapperOptions WithCommandWhitelist(List<string> list)
    {
        foreach (var item in list)
        {
            CommandWhitelist[item] = true;
        }
        return this;
    }
    public MapperOptions ClearCommandWhitelist()
    {
        CommandWhitelist.Clear();
        return this;
    }
    public bool ValidateCommand(string command)
    {
        if (CommandWhitelist.Count == 0) return true;
        return CommandWhitelist.ContainsKey(command);
    }
}