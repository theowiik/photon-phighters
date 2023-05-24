using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Map : Node2D
{
  [GetNode("DarkSpawn")]
  public Node2D DarkSpawn { get; private set; }

  [GetNode("LightSpawn")]
  public Node2D LightSpawn { get; private set; }

  [GetNode("OB")]
  public Area2D OutOfBounds { get; private set; }

  public override void _Ready()
  {
    this.AutoWire();
  }

  public void SetCollisionsEnabled(bool value) => OutOfBounds.Monitoring = value;
}
