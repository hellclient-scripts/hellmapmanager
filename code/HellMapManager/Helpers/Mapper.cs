using HellMapManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace HellMapManager.Helpers;

public class WalkingStep
{
    public static WalkingStep FromExit(WalkingStep? prev, string from, Exit exit, int cost, int TotalCost)
    {
        return new WalkingStep()
        {
            Prev = prev,
            From = from,
            To = exit.To,
            Command = exit.Command,
            Cost = cost,
            TotalCost = TotalCost + cost,
            Remain = cost - 1,
        };
    }
    public Step ToStep()
    {
        return new Step(Command, To, Cost);
    }
    public WalkingStep? Prev { get; set; } = null;
    public string From = "";
    public string To { get; set; } = "";
    public string Command { get; set; } = "";
    public int Cost { get; set; } = 0;
    public int TotalCost { get; set; } = 0;
    public int Remain { get; set; } = 0;
}

public class Walking(Mapper mapper)
{
    private Dictionary<string, WalkingStep> Walked = new();

    public Mapper Mapper { get; } = mapper;
    private static QueryReuslt BuildResult(WalkingStep last, List<string> Targets)
    {
        var result = new QueryReuslt();

        WalkingStep current = last;
        while (current.Prev is not null)
        {
            result.Steps.Add(current.ToStep());
            current = current.Prev;
        }
        result.Steps.Add(current.ToStep());
        result.Steps.Reverse();
        result.From = current.From;
        result.To = last.To;
        foreach (var target in Targets)
        {
            if (target != result.To)
            {
                result.Unvisited.Add(target);
            }
        }
        result.Cost = last.TotalCost;
        return result;
    }
    public QueryReuslt QueryPathAny(List<string> from, List<string> target, int initTotalCost)
    {
        from.RemoveAll(x => x == "");
        target.RemoveAll(x => x == "");
        if (from.Count == 0 || target.Count == 0)
        {
            return QueryReuslt.Fail;
        }
        Walked = new();
        var targets = new Dictionary<string, bool>();
        List<WalkingStep> current;
        List<WalkingStep> pending = [];
        foreach (var t in target)
        {
            targets[t] = true;
        }
        foreach (var f in from)
        {
            if (targets.ContainsKey(f))
            {
                var result = new QueryReuslt();
                result.From = f;
                result.To = f;
                result.Cost = initTotalCost;
                foreach (var to in target)
                {
                    if (to != f)
                    {
                        result.Unvisited.Add(to);
                    }
                }
                return result;
            }
            Walked[f] = new WalkingStep()
            {
                From = "",
                Command = "",
            };
            Mapper.AddRoomWalkingSteps(null, pending, f, initTotalCost);
        }
        while (pending.Count > 0)
        {
            current = pending;
            pending = [];
            foreach (var step in current)
            {
                if (targets.ContainsKey(step.To))
                {
                    return BuildResult(step, target);
                }
                {
                    if (!Walked.ContainsKey(step.To))
                    {
                        if (step.Remain <= 1)
                        {
                            Walked[step.To] = step;
                            Mapper.AddRoomWalkingSteps(step, pending, step.To, step.TotalCost);
                        }
                        else
                        {
                            step.Remain--;
                            pending.Add(step);
                        }
                    }
                }

            }
        }
        return QueryReuslt.Fail;
    }
    public List<string> Dilate(List<string> src, int iterations)
    {
        Walked = new();
        List<WalkingStep> current;
        List<WalkingStep> pending = [];
        foreach (var f in src)
        {
            Walked[f] = new WalkingStep()
            {
                From = "",
                Command = "",
            };
            Mapper.AddRoomWalkingSteps(null, pending, f, 0);
        }
        var i = 0;
        while (pending.Count > 0 && i < iterations)
        {
            current = pending;
            pending = [];
            foreach (var step in current)
            {
                if (!Walked.ContainsKey(step.To))
                {
                    Walked[step.To] = step;
                    Mapper.AddRoomWalkingSteps(step, pending, step.To, step.TotalCost);
                }
            }
            i++;
        }
        return [.. Walked.Keys];
    }
    public QueryReuslt QueryPathAll(string start, List<string> target)
    {
        target.RemoveAll(x => x == "");
        if (target.Count == 0 || start == "")
        {
            return QueryReuslt.Fail;
        }

        var result = QueryPathAny([start], target, 0);
        if (!result.IsSuccess())
        {
            return QueryReuslt.Fail; ;
        }
        var unvisited = result.Unvisited;
        while (unvisited.Count > 0)
        {
            var next = QueryPathAny([start], unvisited, result.Cost);
            if (!next.IsSuccess())
            {
                return result;
            }
            result.Steps.AddRange(next.Steps);
            result.Cost += next.Cost;
            result.Unvisited = next.Unvisited;
            result.To = next.To;
        }
        return result;
    }
    public QueryReuslt QueryPathOrdered(string start, List<string> target)
    {
        target.RemoveAll(x => x == "");
        if (target.Count == 0 || start == "")
        {
            return QueryReuslt.Fail;
        }
        var result = QueryPathAny([start], [target[0]], 0);
        if (!result.IsSuccess())
        {
            return QueryReuslt.Fail; ;
        }
        List<string> unvisited = [];
        for (var i = 1; i < target.Count; i++)
        {
            var next = QueryPathAny([result.To], [target[i]], result.Cost);
            if (!next.IsSuccess())
            {
                return result;
            }
            result.Steps.AddRange(next.Steps);
            result.Cost += next.Cost;
            unvisited = next.Unvisited;
            result.To = next.To;
        }
        return result;
    }
}

public class Mapper(MapFile mapFile, Context context, MapperOptions options)
{
    public Context Context { get; } = context;
    public MapFile MapFile { get; } = mapFile;
    public MapperOptions Options { get; } = options;
    public Room? GetRoom(string key)
    {
        if (!Context.Rooms.TryGetValue(key, out var room))
        {
            if (!MapFile.Cache.Rooms.TryGetValue(key, out room))
            {
                return null;
            }
        }
        return room;
    }
    public int GetExitCost(Exit exit)
    {
        if (Context.CommandCosts.TryGetValue(exit.Command, out var costs))
        {
            if (costs.TryGetValue(exit.To, out var cost))
            {
                return cost;
            }
        }
        return exit.Cost;
    }
    public List<Exit> GetRoomExits(Room room)
    {
        List<Exit> result = [.. room.Exits];
        if (Context.Paths.TryGetValue(room.Key, out var list))
        {
            result.AddRange(list);
        }
        if (!Options.DisableShortcuts)
        {
            MapFile.Map.Shortcuts.ForEach(e =>
            {
                if (ValueTag.ValidteConditions(room.Tags, e.RoomConditions))
                {
                    result.Add(e);
                }
            });
            Context.Shortcuts.ForEach(e =>
            {
                if (ValueTag.ValidteConditions(room.Tags, e.RoomConditions))
                {
                    result.Add(e);
                }
            });
        }
        return result;
    }
    public bool ValidateExit(string start, Exit exit, int cost)
    {

        var room = GetRoom(exit.To);
        if (room == null)
        {
            return false;
        }
        if (!ValidateRoom(room))
        {
            return false;
        }

        if (Context.IsBlocked(start, exit.To))
        {
            return false;
        }
        if (Options.MaxExitCost > 0 && cost > Options.MaxExitCost)
        {
            return false;
        }
        if (Options.MaxTotalCost > 0 && cost > Options.MaxTotalCost)
        {
            return false;
        }
        if (!Context.ValidteConditions(exit.Conditions))
        {
            return false;
        }
        return true;
    }
    public bool ValidateRoom(Room room)
    {
        if (Context.Blacklist.ContainsKey(room.Key))
        {
            return false;
        }
        if (Context.Whitelist.Count > 0 && !Context.Whitelist.ContainsKey(room.Key))
        {
            return false;
        }
        if (!ValueTag.ValidteConditions(room.Tags, Context.RoomConditions))
        {
            return false;
        }
        return true;
    }
    public bool ValidatePath(string start, Exit exit)
    {
        if (Context.IsBlocked(start, exit.To))
        {
            return false;
        }
        return ValidateExit(start, exit, GetExitCost(exit));
    }
    public WalkingStep? ValidateToWalkingStep(WalkingStep? prev, string from, Exit exit, int TotalCost)
    {
        if (exit.To == "" || exit.To == from)
        {
            return null;
        }
        var cost = GetExitCost(exit);
        if (!ValidateExit(from, exit, cost))
        {
            return null;
        }
        if (Options.MaxTotalCost > 0 && Options.MaxTotalCost < (cost + TotalCost))
        {
            return null;
        }
        return WalkingStep.FromExit(prev, from, exit, cost, TotalCost);

    }
    public void AddRoomWalkingSteps(WalkingStep? prev, List<WalkingStep> list, string from, int TotalCost)
    {
        var room = GetRoom(from);
        if (room is not null)
        {
            foreach (var exit in GetRoomExits(room))
            {
                var step = ValidateToWalkingStep(prev, from, exit, TotalCost);
                if (step is not null)
                {
                    list.Add(step);
                }
            }
        }
    }
}