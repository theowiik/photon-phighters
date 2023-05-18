using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.MenuControllers;

public partial class StartSceen : Node2D
{
  public override void _Ready()
  {
    const string ButtonsRoot = "CanvasLayer/VBoxContainer/";
    var startButton = this.GetNodeOrExplode<Button>(ButtonsRoot + "StartButton");
    var quitButton = this.GetNodeOrExplode<Button>(ButtonsRoot + "QuitButton");

    startButton.Pressed += StartGame;
    quitButton.Pressed += QuitGame;
  }

  private void QuitGame()
  {
    GetTree().Quit();
  }

  private void StartGame()
  {
    GetTree().ChangeSceneToFile("res://Scenes/Levels/BaseLevel.tscn");
  }
}
