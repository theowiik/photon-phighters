using System;
using System.Collections.Generic;
using Godot;

namespace PhotonPhighters.Scripts.GoSharper;

public static class GsNodeExtensions
{
  public static T GetNodeOrExplode<T>(this Node node, string name)
    where T : Node
  {
    if (node == null)
    {
      var msg = $"Node to retrieve child from is null, tried to get {name}";
      GD.PrintErr(msg);
      throw new ArgumentException(msg);
    }

    var n = node.GetNodeOrNull<T>(name);

    if (n != null)
    {
      return n;
    }

    {
      var msg = $"Could not find child {name} on {node.Name}";
      GD.PrintErr(msg);
      node.GetTree().Quit();
      throw new KeyNotFoundException(msg);
    }
  }

  public static IEnumerable<T> GetNodesOfType<T>(this Node node)
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
}
