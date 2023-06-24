using System.Collections.Generic;
using Godot;

namespace PhotonPhighters.Scripts.Utils;

/// <summary>
///   Extended Godot API.
/// </summary>
public static class GDX
{
  public static T LoadOrExplode<T>(string path)
    where T : class
  {
    var node = GD.Load<T>(path);

    if (node != null)
    {
      return node;
    }

    var msg = $"Could not load resource at {path}";
    GD.PrintErr(msg);
    throw new KeyNotFoundException(msg);
  }
}
