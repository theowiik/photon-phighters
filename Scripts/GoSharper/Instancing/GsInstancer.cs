using System;
using System.Collections.Generic;
using System.IO;
using Godot;

namespace PhotonPhighters.Scripts.GoSharper.Instancing;

public static class GsInstancer
{
  private static readonly IDictionary<Type, string> s_typePathLookup = new Dictionary<Type, string>();

  public static T Instanciate<T>() where T : Node
  {
    var type = typeof(T);
    string path;

    if (s_typePathLookup.TryGetValue(type, out var value))
    {
      path = value;
    }
    else
    {
      var attr = (GsInstancerAttribute)Attribute.GetCustomAttribute(type, typeof(GsInstancerAttribute));

      if (attr == null)
      {
        throw new FileNotFoundException("Could not find a PackedSceneAttribute for " + type);
      }

      path = attr.Path;
      s_typePathLookup[type] = path;
    }

    var scene = GsGDX.LoadOrExplode<PackedScene>(path);
    return scene.Instantiate<T>();
  }
}
