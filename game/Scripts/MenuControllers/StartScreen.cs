using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotSharper;
using GodotSharper.AutoGetNode;
using PhotonPhighters.Scripts.GSAlpha;
using PhotonPhighters.Scripts.Utils.ResourceWrapper;

namespace PhotonPhighters.Scripts.MenuControllers;

public partial class StartScreen : Node2D
{
  private readonly IDictionary<int, Avatar> _avatars = new Dictionary<int, Avatar>();
  private PackedScene _avatarScene = GDX.LoadOrFail<PackedScene>(ObjectResourceWrapper.AvatarPath);

  [GetUniqueNode("AvatarsRoot")]
  private Node2D _avatarsRoot;

  private SpinBox _roundsToWinLineEdit;
  private SpinBox _roundTimeLineEdit;

  public override void _Ready()
  {
    this.GetNodes();
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

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is InputEventJoypadButton e && e.Pressed)
    {
      if (e.ButtonIndex == JoyButton.Start)
      {
        // join game

        AddPlayer(e.Device);
      }
    }
  }

  private void AddPlayer(int deviceId)
  {
    if (deviceId < 0)
    {
      return;
    }

    if (_avatars.ContainsKey(deviceId))
    {
      return;
    }

    var margin = 200; // TODO: better calculation
    var avatar = _avatarScene.Instantiate<Avatar>();
    _avatarsRoot.AddChild(avatar);
    avatar.Team = Team.Light;
    avatar.DeviceId = deviceId;
    avatar.Position = new Vector2(_avatars.Count * margin, 0);

    _avatars.Add(deviceId, avatar);
  }

  private void StartGame()
  {
    if (!ValidTeams())
    {
      return;
    }

    var roundTime = _roundTimeLineEdit.Value;
    var roundsToWin = _roundsToWinLineEdit.Value;

    GlobalGameState.RoundTime = (int)roundTime;
    GlobalGameState.RoundsToWin = (int)roundsToWin;

    foreach (var (deviceId, avatar) in _avatars)
    {
      var playerValues = new GlobalGameState.PlayerValues(avatar.Team, avatar.PlayerName);
      GlobalGameState.Players.Add(deviceId, playerValues);
    }

    GetTree().ChangeSceneToFile(SceneResourceWrapper.WorldPath);
  }

  private bool ValidTeams()
  {
    if (_avatars.Any(p => p.Value.Team == Team.Neutral))
    {
      return false;
    }

    var lightCount = _avatars.Count(p => p.Value.Team == Team.Light);
    var darkCount = _avatars.Count(p => p.Value.Team == Team.Dark);

    return lightCount > 0 && darkCount > 0;
  }
}
