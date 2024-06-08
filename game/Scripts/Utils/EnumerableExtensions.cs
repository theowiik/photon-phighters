using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotonPhighters.Scripts.Utils;

public static class EnumerableExtensions
{
  public static IEnumerable<T> Shuffled<T>(this IEnumerable<T> source)
  {
    var rnd = new Random();
    return source.OrderBy(_ => rnd.Next()).ToList();
  }

  public static T Sample<T>(this IEnumerable<T> source)
  {
    var list = source.ToList();

    if (!list.Any())
    {
      throw new ArgumentException("Cannot sample from an empty collection");
    }

    var rnd = new Random();
    var index = rnd.Next(0, list.Count);
    return list.ElementAt(index);
  }
}
