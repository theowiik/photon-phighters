using Godot;
using PhotonPhighters.Scripts.GoSharper;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.Utils.ResourceWrapper;

namespace PhotonPhighters.Scripts.MenuControllers;

public partial class EndScreen : Node2D
{
  [GetNode("CanvasLayer/VBoxContainer/ExitButton")]
  private Button _exitButton;

  public override void _Ready()
  {
    this.AutoWire();
    _exitButton.Pressed += () => GetTree().ChangeOrExplode(SceneResourceWrapper.StartScreenPath);
  }
}
