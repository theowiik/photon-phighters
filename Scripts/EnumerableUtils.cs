using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotonPhighters.Scripts;

public static class EnumerableUtils
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        var rnd = new Random();
        return source.OrderBy((_) => rnd.Next());
    }
}