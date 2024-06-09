using Godot;
using GodotSharper.AutoGetNode;
using PhotonPhighters.Scripts.GSAlpha;
using PhotonPhighters.Scripts.Utils.ResourceWrapper;

namespace PhotonPhighters.Scripts.MenuControllers;

public partial class EndScreen : Node2D
{
  [GetNode("CanvasLayer/VBoxContainer/ExitButton")]
  private Button _exitButton;

  public override void _Ready()
  {
    this.GetNodes();
    _exitButton.Pressed += () => GetTree().ChangeSceneToFile(SceneResourceWrapper.StartScreenPath);
  }
}
