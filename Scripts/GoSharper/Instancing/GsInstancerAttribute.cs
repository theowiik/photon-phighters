using System;

namespace PhotonPhighters.Scripts.GoSharper.Instancing;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GsInstancerAttribute : Attribute
{
  public GsInstancerAttribute(string path)
  {
    Path = path;
  }

  public string Path { get; }
}
