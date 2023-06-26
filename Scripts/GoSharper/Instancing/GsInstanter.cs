using System;
using System.Collections.Generic;
using System.IO;
using Godot;

namespace PhotonPhighters.Scripts.GoSharper.Instancing;

public static class GsInstanter
{
  private static readonly IDictionary<Type, string> s_typePathLookup = new Dictionary<Type, string>();

  public static T Instantiate<T>()
    where T : Node
  {
    var type = typeof(T);
    string path;

    if (s_typePathLookup.TryGetValue(type, out var value))
    {
      path = value;
    }
    else
    {
      var attr = (InstantiableAttribute)Attribute.GetCustomAttribute(type, typeof(InstantiableAttribute));

      if (attr == null)
      {
        throw new FileNotFoundException("Could not find a PackedSceneAttribute for " + type);
      }

      path = attr.Path;
      s_typePathLookup[type] = path;
    }

    var scene = Gs.LoadOrExplode<PackedScene>(path);
    return scene.Instantiate<T>();
  }
}
