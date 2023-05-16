using System;
using Godot;

namespace PhotonPhighters.Scripts;

public partial class DamageAmountIndicator : Node2D
{
  private const float Gravity = 1500;
  private const float Speed = 600;
  private Vector2 _velocity;

  public string Message
  {
    set
    {
      var label = GetNode<Label>("Label");
      label.Text = value;
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    _velocity.Y += Gravity * (float)delta;
    Position += _velocity * (float)delta;
  }

  public override void _Ready()
  {
    _velocity = Vector2.Right.Rotated(-(float)GD.RandRange(0, Math.PI)) * Speed;
  }
}
