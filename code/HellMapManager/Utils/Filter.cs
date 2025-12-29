using System;
using System.Collections.Generic;

namespace HellMapManager.Utils;

public class FilterUtil
{
    public static List<string> SplitFilter(string filter)
    {
        var parts = filter.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return new List<string>(parts);
    }
}