using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.MenuControllers;

public partial class StartScreen : Node2D
{
  private SpinBox _roundsToWinLineEdit;
  private SpinBox _roundTimeLineEdit;

  public override void _Ready()
  {
    this.AutoWire();
    GetTree().Paused = false;

    const string ButtonsRoot = "CanvasLayer/VBoxContainer/";
    var startButton = this.GetNodeOrExplode<Button>(ButtonsRoot + "StartButton");
    var quitButton = this.GetNodeOrExplode<Button>(ButtonsRoot + "QuitButton");
    _roundTimeLineEdit = this.GetNodeOrExplode<SpinBox>(ButtonsRoot + "RoundTimeSpinBox");
    _roundsToWinLineEdit = this.GetNodeOrExplode<SpinBox>(ButtonsRoot + "RoundsToWinSpinBox");

    startButton.Pressed += StartGame;
    quitButton.Pressed += QuitGame;

    startButton.GrabFocus();
  }

  private void QuitGame()
  {
    GetTree().Quit();
  }

  private void StartGame()
  {
    var roundTime = _roundTimeLineEdit.Value;
    var roundsToWin = _roundsToWinLineEdit.Value;

    GlobalGameState.RoundTime = (int)roundTime;
    GlobalGameState.RoundsToWin = (int)roundsToWin;

    GetTree().ChangeSceneToFile("res://Scenes/World.tscn");
  }
}
