using System.Collections.Generic;
using Avalonia.Remote.Protocol.Viewport;

namespace HellMapManager.Models;

public class Step(string command, string target, int cost)
{
    public string Command = command;
    public string Target = target;
    public int Cost = cost;
    public static string JoinCommands(string sep, List<Step> steps)
    {
        return string.Join(sep, steps.ConvertAll(x => x.Command));
    }
}

public class QueryReuslt
{
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public int Cost { get; set; } = 0;
    public List<Step> Steps { get; set; } = [];
    public List<string> Unvisited { get; set; } = [];

    public bool IsSuccess()
    {
        return From != "" && To != "";
    }
    public static readonly QueryReuslt Fail = new();

    public QueryReuslt? SuccessOrNull()
    {
        if (IsSuccess())
        {
            return this;
        }
        return null;
    }
}