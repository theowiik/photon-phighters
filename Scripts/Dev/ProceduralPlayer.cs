using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.Dev;

public partial class ProceduralPlayer : StaticBody2D
{
  [GetNode("LeftLeg")]
  private Node2D _leftLeg;

  private Vector2 _leftLegPosition;

  [GetNode("LeftRay")]
  private RayCast2D _leftRay;

  [GetNode("RightLeg")]
  private Node2D _rightLeg;

  private Vector2 _rightLegPosition;

  [GetNode("RightRay")]
  private RayCast2D _rightRay;

  public override void _Ready()
  {
    this.AutoWire();
    _leftRay.Enabled = true;
  }

  public override void _Process(double delta)
  {
    Movement(delta);

    if (_leftRay.IsColliding())
    {
      var globalHitPoint = _leftRay.GetCollisionPoint();

      // Only once
      if (_leftLegPosition == Vector2.Zero)
      {
        _leftLegPosition = globalHitPoint;
        GD.Print("initial");
      }

      // Only if the point is far enough
      if (_leftLegPosition.DistanceTo(globalHitPoint) > 100)
      {
        _leftLegPosition = globalHitPoint;
        GD.Print("switching");
      }

      var angleToLeftPos = GlobalPosition.AngleTo(_leftLegPosition);
      GD.Print(angleToLeftPos);
      _leftLeg.GlobalRotation = angleToLeftPos;
    }
  }

  private void Movement(double delta)
  {
    var input = new Vector2();
    if (Input.IsActionPressed("ui_right"))
    {
      input.X += 1;
    }

    if (Input.IsActionPressed("ui_left"))
    {
      input.X -= 1;
    }

    var speed = 100f;
    var velocity = input.Normalized() * speed;

    Position += velocity * (float)delta;
  }
}
