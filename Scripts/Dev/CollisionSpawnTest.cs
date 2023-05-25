using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.Dev;

public partial class CollisionSpawnTest : Node2D
{
  [GetNode("Player")]
  private CharacterBody2D _player;

  [GetNode("Area2D")]
  private Area2D _area2D;

  public override void _Ready()
  {
    this.AutoWire();
    _area2D.BodyEntered += OnBodyEntered;
    _area2D.BodyExited += OnBodyExited;
  }

  public override void _PhysicsProcess(double delta)
  {
    if (Input.IsActionJustPressed("ui_up"))
    {
    }

    if (Input.IsActionJustPressed("ui_down"))
    {
      _player.Position = GetGlobalMousePosition();
    }
  }

  private void OnBodyExited(Node2D body)
  {
    GD.Print("body exited");
  }

  private void OnBodyEntered(Node2D body)
  {
    GD.Print("body entered");
  }

}
