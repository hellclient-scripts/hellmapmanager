using HellMapManager.Models;
using System.Collections.Generic;
// 路径规划模块
namespace HellMapManager.Helpers;

// 规划中的每一个步骤
public class WalkingStep
{
    //从 出口信息.上一步，出发点，移动消耗，移动总消耗生成一个新的规划步骤
    //prev是上一个步骤，为空说明是第一步
    //from是移动起点,因为exit可以在多个房间复用，并不包含起点信息
    //exit 具体的房间出口信息
    //cost 这个步骤有多少移动消耗
    //totalcost 总的移动消耗，如果消耗超过Context中最大的移动消耗，路径规划就会失败
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
    //上一个步骤，为空说明是第一步
    public WalkingStep? Prev { get; set; } = null;
    public string From = "";
    //移动起点,因为exit可以在多个房间复用，并不包含起点信息
    public string To { get; set; } = "";
    //目的地
    public string Command { get; set; } = "";
    //移动消耗
    public int Cost { get; set; } = 0;
    //总移动消耗
    public int TotalCost { get; set; } = 0;
    //剩余步数，每个计算回合-1,为0说明这一步移动完成。
    public int Remain { get; set; } = 0;
}

//规划类
public class Walking(Mapper mapper)
{
    //已经移动过的房间信息
    private Dictionary<string, WalkingStep> Walked = new();
    //对应的mapper
    public Mapper Mapper { get; } = mapper;
    //根据最后成功的最后一步移动(last),生成移动结果。targets为目标列表，会在去除移动目标并在结果里记录剩余目标
    private static QueryResult BuildResult(WalkingStep last, List<string> targets)
    {
        var result = new QueryResult();

        WalkingStep current = last;
        //没有prev说明是第一步
        while (current.Prev is not null)
        {
            result.Steps.Add(current.ToStep());
            current = current.Prev;
        }
        //第一步也要加入
        result.Steps.Add(current.ToStep());
        //逆向一下，最早的步骤应该在最前面
        result.Steps.Reverse();
        result.From = current.From;
        result.To = last.To;
        //统计剩余目标(移动支持多目标)
        foreach (var target in targets)
        {
            if (target != result.To)
            {
                result.Unvisited.Add(target);
            }
        }
        //计算消耗
        result.Cost = last.TotalCost;
        return result;
    }
    //在多个起点/终点之间规划一个最近路线
    //from 起点列表
    //target 重点目标
    //initTotalCost 初始移动总消耗，用于缺点多步逼近规划里限制总消耗
    public QueryResult QueryPathAny(List<string> from, List<string> target, int initTotalCost)
    {
        from.RemoveAll(x => x == "");
        target.RemoveAll(x => x == "");
        if (from.Count == 0 || target.Count == 0)
        {
            return QueryResult.Fail;
        }
        Walked = new();
        //从列表转字典
        var targets = new Dictionary<string, bool>();
        List<WalkingStep> current;
        List<WalkingStep> pending = [];
        foreach (var t in target)
        {
            targets[t] = true;
        }
        //起点准备
        foreach (var f in from)
        {
            //终点包含起点则直接到达
            if (targets.ContainsKey(f))
            {
                var result = new QueryResult();
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
            //标记起点
            Walked[f] = new WalkingStep()
            {
                From = "",
                Command = "",
            };
            //加入移动中列表
            Mapper.AddRoomWalkingSteps(null, pending, f, initTotalCost);
        }
        //移动一轮
        while (pending.Count > 0)
        {
            current = pending;
            pending = [];
            //循环每个待移动步骤
            foreach (var step in current)
            {
                //确定这个步骤还需要计算。
                //如果目标已经到达，则放弃
                if (!Walked.ContainsKey(step.To))
                {

                    if (step.Remain <= 1)
                    {
                        //移动到达
                        if (targets.ContainsKey(step.To))
                        {
                            //到达终点
                            return BuildResult(step, target);
                        }
                        Walked[step.To] = step;
                        //还没走完，延迟到下一轮判断
                        Mapper.AddRoomWalkingSteps(step, pending, step.To, step.TotalCost);
                    }
                    else
                    {
                        //移动中
                        step.Remain--;
                        pending.Add(step);
                    }
                }
            }
        }
        //计算失败
        return QueryResult.Fail;
    }
    //膨胀，在给定的房间边上膨胀iterations个房间
    //一般用于npc会多步随机移动时，遍历目标和目标周边房间
    public List<string> Dilate(List<string> src, int iterations)
    {
        Walked = new();
        List<WalkingStep> current;
        List<WalkingStep> pending = [];
        foreach (var f in src)
        {
            //将有效房间加入待计算列表
            if (Mapper.GetRoom(f) is not null)
            {

                Walked[f] = new WalkingStep()
                {
                    From = "",
                    Command = "",
                };
                Mapper.AddRoomWalkingSteps(null, pending, f, 0);
            }
        }
        var i = 0;
        while (pending.Count > 0 && i < iterations)
        {
            current = pending;
            pending = [];
            foreach (var step in current)
            {
                //未到达过的新房间加入下一步计算列表
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
    //通过模拟多步行走的方式，动态计算全遍历路径
    public QueryResult QueryPathAll(string start, List<string> target)
    {
        target.RemoveAll(x => x == "");
        if (target.Count == 0 || start == "")
        {
            return QueryResult.Fail;
        }
        var result = new QueryResult
        {
            From = start,
            To = start,
        };
        //未走到的房间列表
        var pending = target;
        //循环移动，直到全部走完
        while (pending.Count > 0)
        {
            //计算单步
            var next = QueryPathAny([result.To], pending, result.Cost);
            if (next.IsSuccess())
            {
                // 累积移动结果，循环下一轮
                result.Steps.AddRange(next.Steps);
                result.Cost = next.Cost;
                result.Unvisited = next.Unvisited;
                //更新新的计算列表
                pending = result.Unvisited;
                result.To = next.To;
            }
            else
            {
                //移动失败。结束
                pending = [];
            }
        }
        //如果一步也没动过，就是完全失败
        if (result.Steps.Count == 0)
        {
            return QueryResult.Fail;
        }
        return result;
    }
    //通过模拟多步行走的方式，动态计算顺序路径
    //targets为带顺序的房间列表
    //如果有部分房间无法进入，会跳过无法进入的房间。
    //用于在有某些限制条件的情况下，是指最新的上下文，然后尽可能多的遍历原路径中的房间
    public QueryResult QueryPathOrdered(string start, List<string> target)
    {
        target.RemoveAll(x => x == "");
        if (target.Count == 0 || start == "")
        {
            return QueryResult.Fail;
        }
        var result = new QueryResult
        {
            From = start,
            To = start,
        };
        for (var i = 0; i < target.Count; i++)
        {
            var next = QueryPathAny([result.To], [target[i]], result.Cost);
            //成功则记录路径，不成功则跳过房间，将失败的房间加入Unvisited,继续计算
            if (next.IsSuccess())
            {
                result.Steps.AddRange(next.Steps);
                result.Cost = next.Cost;
                result.To = next.To;
            }
            else
            {
                result.Unvisited.Add(target[i]);
            }
        }
        //如果一步也没动过，就是完全失败
        if (result.Steps.Count == 0)
        {
            return QueryResult.Fail;
        }
        return result;
    }
}

//地图计算器
//mapfile为地图信息
//context为移动上下文,多个移动可能使用相同的上下文
//options为移动选项，每次移动都可能不一样
public class Mapper(MapFile mapFile, Context context, MapperOptions options)
{
    public Context Context { get; } = context;
    public MapFile MapFile { get; } = mapFile;
    public MapperOptions Options { get; } = options;
    //通过key获取有效房间信息
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
    //根据上下文，获取出口的移动消耗
    public int GetExitCost(Exit exit)
    {
        //判断上下文中是否有单独的出口消耗定义覆盖原始值
        if (Context.CommandCosts.TryGetValue(exit.Command, out var costs))
        {
            if (costs.TryGetValue(exit.To, out var cost))
            {
                return cost;
            }
            if (costs.TryGetValue("", out var cost2))
            {
                return cost2;
            }

        }
        return exit.Cost;
    }
    //获取房间出口信息
    public List<Exit> GetRoomExits(Room room)
    {
        List<Exit> result = [.. room.Exits];
        //加入上下文中的临时出口
        if (Context.Paths.TryGetValue(room.Key, out var list))
        {
            result.AddRange(list);
        }
        //判断是否禁用捷径(飞行)出口
        if (!Options.DisableShortcuts)
        {
            //原始捷径信息
            MapFile.Map.Shortcuts.ForEach(e =>
            {
                //符合条件则加入列表
                if (ValueTag.ValidateConditions(room.Tags, e.RoomConditions))
                {
                    result.Add(e);
                }
            });
            //上下文的中的临时捷径
            Context.Shortcuts.ForEach(e =>
            {
                //符合条件则加入列表
                if (ValueTag.ValidateConditions(room.Tags, e.RoomConditions))
                {
                    result.Add(e);
                }
            });
        }
        return result;
    }
    //验证出口
    //start为出发房间
    //exit是出口信息
    //cost为移动消耗
    public bool ValidateExit(string start, Exit exit, int cost)
    {
        //判断房间有效
        var room = GetRoom(exit.To);
        if (room == null)
        {
            return false;
        }
        //验证房间符合当前移动的上下文设置
        if (!ValidateRoom(room))
        {
            return false;
        }
        //判断房间不在上下文中的拦截名单里
        if (Context.IsBlocked(start, exit.To))
        {
            return false;
        }
        //判断出口不超时
        if (Options.MaxExitCost > 0 && cost > Options.MaxExitCost)
        {
            return false;
        }
        //判断出口不超过整个移动的最大消耗
        if (Options.MaxTotalCost > 0 && cost > Options.MaxTotalCost)
        {
            return false;
        }
        //判断出口的条件是否匹配当前上下文
        if (!Context.ValidateConditions(exit.Conditions))
        {
            return false;
        }
        return true;
    }
    //验证房间
    public bool ValidateRoom(Room room)
    {
        //验证房间是否在黑名单
        if (Context.Blacklist.ContainsKey(room.Key))
        {
            return false;
        }
        //验证如果启用白名单的话，房间是否在白名单内
        if (Context.Whitelist.Count > 0 && !Context.Whitelist.ContainsKey(room.Key))
        {
            return false;
        }
        //验证房间的标签是否匹配上下文中的房间条件
        if (!ValueTag.ValidateConditions(room.Tags, Context.RoomConditions))
        {
            return false;
        }
        return true;
    }
    //验证路径
    public bool ValidatePath(string start, Exit exit)
    {
        //验证路径是否在黑名单
        if (Context.IsBlocked(start, exit.To))
        {
            return false;
        }
        return ValidateExit(start, exit, GetExitCost(exit));
    }
    //验证并转换路径
    //如果路径无效，返回空
    public WalkingStep? ValidateToWalkingStep(WalkingStep? prev, string from, Exit exit, int TotalCost)
    {
        if (exit.To == "" || exit.To == from)
        {
            return null;
        }
        var cost = GetExitCost(exit);
        //验证出口
        if (!ValidateExit(from, exit, cost))
        {
            return null;
        }
        //判断最大消耗
        if (Options.MaxTotalCost > 0 && Options.MaxTotalCost < (cost + TotalCost))
        {
            return null;
        }
        //转换
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