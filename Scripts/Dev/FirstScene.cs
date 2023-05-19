using Godot;
using PhotonPhighters.Scripts.Utils;

public partial class FirstScene : Node2D
{
  [GetNode("Button")]
  private Button _button;

  public override void _Ready()
  {
    this.AutoWire();
    _button.Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/Dev/Nav/Second.tscn");
  }
}
