using System.Collections.Generic;
using Godot;

namespace PhotonPhighters.Scripts.GoSharper;

/// <summary>
///   Extended Godot API.
/// </summary>
public static class GsGDX
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

  public static void ChangeOrExplode(this SceneTree tree, string filePath)
  {
    var error = tree.ChangeSceneToFile(filePath);
    if (error == Error.Ok)
    {
      return;
    }

    var msg = $"Could not change scene to {filePath}";
    GD.PrintErr(msg);
    tree.Quit();
    throw new KeyNotFoundException(msg);
  }
}
