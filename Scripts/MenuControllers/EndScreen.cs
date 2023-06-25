using Godot;
using PhotonPhighters.Scripts.GoSharper;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
public partial class EndScreen : Node2D
{
  [GsAutoWiring("CanvasLayer/VBoxContainer/ExitButton")]
  private Button _exitButton;

  public override void _Ready()
  {
    this.AutoWire();
    _exitButton.Pressed += () => GetTree().ChangeOrExplode("res://Scenes/Screens/StartScreen.tscn");
  }
}
