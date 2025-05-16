using HellMapManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace HellMapManager.Helpers;

public class SnapshotHelper
{
    public static List<SnapshotSearchResult> Search(SnapshotSearch search, List<Snapshot> snapshots)
    {
        Dictionary<string, SnapshotSearchResult> all = new();
        foreach (var snapshot in snapshots)
        {
            if (!all.ContainsKey(snapshot.Key))
            {
                all[snapshot.Key] = new SnapshotSearchResult()
                {
                    Key = snapshot.Key,
                    Sum = 0,
                    Count = 0,
                    Items = new(),
                };

            }
            all[snapshot.Key].Add(snapshot, search.Validate(snapshot));
        }
        var values = all.Values.ToList();
        var results = new List<SnapshotSearchResult>();
        foreach (var item in values)
        {
            if (item.Items.Count > 0)
            {
                results.Add(item);
            }
        }
        results.Sort((x, y) => x.Key.CompareTo(y.Key));
        return results;
    }
}
