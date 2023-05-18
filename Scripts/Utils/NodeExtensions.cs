using System.Collections.Generic;
using Godot;

namespace PhotonPhighters.Scripts.Utils;

public static class NodeExtensions
{
  public static IEnumerable<T> GetNodes<T>(this Node node)
    where T : Node
  {
    var output = new List<T>();

    foreach (var child in node.GetChildren())
    {
      if (child is T c)
      {
        output.Add(c);
      }
    }

    return output;
  }

  public static T GetNodeOrExplode<T>(this Node node, string name)
    where T : Node
  {
    var n = node.GetNodeOrNull<T>(name);

    if (n == null)
    {
      GD.PrintErr($"Could not find child {name} on {node.Name}");
      node.GetTree().Quit();
      throw new KeyNotFoundException($"Could not find child {name} on {node.Name}");
    }

    return n;
  }
}
