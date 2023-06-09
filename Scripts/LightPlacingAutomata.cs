using Godot;
using PhotonPhighters.Scripts.Utils;

public partial class LightPlacingAutomata : Node2D
{
  [GetNode("RayCast2D")]
  private RayCast2D _rayCast2D;

  [Signal]
  public delegate void LightPlacedEventHandler(Vector2 globalPosition);

  public override void _Ready()
  {
    this.AutoWire();
  }

  public override void _Process(double delta)
  {
    GlobalPosition = GetGlobalMousePosition();

    var hit = _rayCast2D.GetCollider();
    GD.Print(hit);

    if (hit != null && hit is Node2D node2D && node2D.IsInGroup("floors"))
    {
      var intersection = _rayCast2D.GetCollisionPoint();
      GD.Print(intersection);

      EmitSignal(SignalName.LightPlaced, intersection);
    }
  }
}
