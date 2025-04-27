using System.Collections.Generic;
using System.Linq;
using HellMapManager.Models;

namespace HellMapManager.Services;

public enum RelationType
{
    TwoSide = 0,
    OneSideTo = 1,
}
public class Relation(RelationMapItem target, RelationType type)
{
    public RelationMapItem Target = target;
    public RelationType Type = type;

}
public class RelationMapItem(Room room, int depth)
{
    public int Depth = depth;
    public Room Room { get; set; } = room;
    public List<Relation> Relations = [];
    public bool HasRelation(string target)
    {
        foreach (Relation r in Relations)
        {
            if (r.Target.Room.Key == target)
            {
                return true;
            }
        }
        return false;
    }
    public List<string> Dump()
    {
        List<string> result = [];
        result.Add(new string(' ', Depth * 4) + "key:" + this.Room.Key + "(" + this.Room.Name + ")");
        foreach (var r in Relations)
        {
            string mode;
            switch (r.Type)
            {
                case RelationType.OneSideTo:
                    mode = "to";
                    break;
                default:
                    mode = "twoway";
                    break;
            }
            result.Add(new string(' ', r.Target.Depth * 4) + mode);
            result.AddRange(r.Target.Dump());
        }
        return result;
    }
}



public partial class RelationMapper
{
    private class Map(MapFile mf, string start, int maxDepth)
    {
        private void BuildRelations(RelationMapItem item)
        {
            var exits = item.Room.Exits;
            var cache = new Dictionary<string, bool>();
            foreach (var exit in exits)
            {
                if (exit.To == "" || exit.To == item.Room.Key)
                {
                    continue;
                }
                if (cache.ContainsKey(exit.To))
                {
                    continue;
                }
                cache[exit.To] = true;
                var mode = RelationType.OneSideTo;
                Room targetRoom;
                if (!MapFile.Cache.Rooms.ContainsKey(exit.To))
                {
                    continue;
                }
                targetRoom = MapFile.Cache.Rooms[exit.To];
                if (Walked.ContainsKey(exit.To))
                {
                    continue;
                }
                var target = new RelationMapItem(targetRoom, item.Depth + 1);
                Walked[exit.To] = target;
                if (targetRoom.HasExitTo(item.Room.Key))
                {
                    mode = RelationType.TwoSide;
                }
                item.Relations.Add(new Relation(target, mode));
            }
        }
        public RelationMapItem? Exec()
        {
            if (MapFile.Cache.Rooms.ContainsKey(this.Start) == false)
            {
                return null;
            }
            var targetRoom = MapFile.Cache.Rooms[Start];
            var root = new RelationMapItem(targetRoom, 0);
            Walked[Start] = root;
            BuildRelations(root);
            var currentDepth = 1;
            var relations = root.Relations;
            while (currentDepth < MaxDepth)
            {
                if (relations.Count == 0)
                {
                    break;
                }
                var walking = relations;
                relations = [];
                foreach (var item in walking)
                {
                    BuildRelations(item.Target);
                    relations.AddRange(item.Target.Relations);
                }
                currentDepth++;
            }
            return root;
        }
        Dictionary<string, RelationMapItem> Walked = new Dictionary<string, RelationMapItem>();
        readonly MapFile MapFile = mf;
        readonly string Start = start;
        readonly int MaxDepth = maxDepth;
    }
    public static RelationMapItem? RelationMap(MapFile mf, string start, int MaxDepth)
    {
        var rm = new Map(mf, start, MaxDepth);
        return rm.Exec();
    }
}