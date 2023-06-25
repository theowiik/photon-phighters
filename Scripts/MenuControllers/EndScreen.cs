using Godot;
using PhotonPhighters.Scripts.Utils;

public partial class EndScreen : Node2D
{
  [GetNode("CanvasLayer/VBoxContainer/ExitButton")]
  private Button _exitButton;

  public override void _Ready()
  {
    this.AutoWire();
    _exitButton.Pressed += () => GetTree().ChangeOrExplode("res://Scenes/Screens/StartScreen.tscn");
  }
}
