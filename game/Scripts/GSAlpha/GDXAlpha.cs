using System;
using System.Collections.Generic;
using Godot;

namespace PhotonPhighters.Scripts.GSAlpha;

[Obsolete("Migrate to GodotSharper")]
public static class GDXAlpha
{
  [Obsolete("Migrate to GodotSharper")]
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
