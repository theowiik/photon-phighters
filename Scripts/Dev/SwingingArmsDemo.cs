using Godot;
using PhotonPhighters.Scripts.Utils;

public partial class SwingingArmsDemo : Node2D
{
  [GetNode("CharacterBody2D")]
  public CharacterBody2D CharacterBody2D { get; set; }

  public override void _Ready()
  {
    this.AutoWire();
  }

  public override void _PhysicsProcess(double delta)
  {
    var velocity = new Vector2();

    if (Input.IsActionPressed("ui_right"))
    {
      velocity.X += 1;
    }
    else if (Input.IsActionPressed("ui_left"))
    {
      velocity.X -= 1;
    }

    CharacterBody2D.Velocity = velocity.Normalized() * 100;
    CharacterBody2D.MoveAndSlide();
  }
}
