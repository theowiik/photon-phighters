using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.Dev;

public partial class CollisionSpawnTest : Node2D
{
  [GetNode("Player")]
  private RigidBody2D _player;

  [GetNode("Area2D")]
  private Area2D _area2D;

  public override void _Ready()
  {
    this.AutoWire();
    _area2D.BodyEntered += OnBodyEntered;
    _area2D.BodyExited += OnBodyExited;
  }

  private void OnBodyExited(Node2D body)
  {
    GD.Print("body exited");
  }

  private void OnBodyEntered(Node2D body)
  {
    GD.Print("body entered");
  }

  public override void _Process(double delta)
  {
    HandleInput(delta);
  }

  private void HandleInput(double delta)
  {
    var velocity = new Vector2();

    if (Input.IsActionPressed("ui_right"))
    {
      velocity.X++;
    }

    if (Input.IsActionPressed("ui_left"))
    {
      velocity.X--;
    }

    _player.Position += velocity * 100 * (float)delta;
  }
}
