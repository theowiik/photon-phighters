using System.Collections.Generic;
using System.Linq;
using Godot;

namespace PhotonPhighters.Scripts;

public partial class LightFloor : StaticBody2D
{
  private readonly PackedScene _lightScene = GD.Load<PackedScene>("res://Objects/Light.tscn");

  public override void _Ready()
  {
    BasicLightPlacement();
  }

  private static IEnumerable<Vector2> BuildSide(Vector2 baseVector, int lightsOnSide, float apart)
  {
    var output = new List<Vector2>();

    for (var i = 0; i < lightsOnSide; i++)
    {
      output.Add(baseVector + new Vector2(0, apart * i));
    }

    return output;
  }

  private void BasicLightPlacement()
  {
    // TODO: Dont hardcode side length
    const int SideLength = 130;

    const int Half = SideLength / 2;
    const int Lights = 20;
    const int LightsPerSide = Lights / 4;
    const int DistancePerSide = SideLength / LightsPerSide;
    var baseSide = BuildSide(new Vector2(Half, -Half), LightsPerSide, DistancePerSide);

    for (var i = 0; i < 4; i++)
    {
      var side = baseSide.Select(v => v.Rotated(Mathf.Pi / 2 * i));
      Placee(side);
    }
  }

  private void Placee(IEnumerable<Vector2> vectors)
  {
    foreach (var vector in vectors)
    {
      var light = _lightScene.Instantiate<Node2D>();
      AddChild(light);
      light.Position = vector;
    }
  }
}
