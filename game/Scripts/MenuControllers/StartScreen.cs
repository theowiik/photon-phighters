using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotSharper;
using GodotSharper.AutoGetNode;
using PhotonPhighters.Scripts.GameModes;
using PhotonPhighters.Scripts.Utils.ResourceWrapper;

namespace PhotonPhighters.Scripts.MenuControllers;

public partial class StartScreen : Node2D
{
  private readonly List<GameMode> _gameModes = new() { GameMode.PhotonPhight, GameMode.BotBrawl };
  private readonly IDictionary<int, Avatar> _avatars = new Dictionary<int, Avatar>();
  private PackedScene _avatarScene = GDX.LoadOrFail<PackedScene>(ObjectResourceWrapper.AvatarPath);

  [GetUniqueNode("AvatarsRoot")]
  private Node2D _avatarsRoot;

  [GetUniqueNode("GameModeOptionButton")]
  private OptionButton _gameModeOptionButton;

  [GetUniqueNode("StartButton")]
  private Button _startButton;

  [GetUniqueNode("QuitButton")]
  private Button _quitButton;

  [GetUniqueNode("RoundTimeSpinBox")]
  private SpinBox _roundTimeSpinBox;

  [GetUniqueNode("RoundsToWinSpinBox")]
  private SpinBox _roundsToWinSpinBox;

  public override void _Ready()
  {
    this.GetNodes();
    GetTree().Paused = false;

    foreach (var gameMode in _gameModes)
    {
      _gameModeOptionButton.AddItem(gameMode.Title());
    }

    _startButton.Pressed += StartGame;
    _quitButton.Pressed += QuitGame;

    _startButton.GrabFocus();
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
    var gameMode = _gameModes.First(gm => gm.Title() == _gameModeOptionButton.Text);
    if (gameMode != GameMode.BotBrawl && !ValidTeams())
    {
      return;
    }

    GlobalGameState.RoundTime = (int)_roundTimeSpinBox.Value;
    GlobalGameState.RoundsToWin = (int)_roundsToWinSpinBox.Value;
    GlobalGameState.GameMode = gameMode;

    foreach (var (deviceId, avatar) in _avatars)
    {
      var playerValues = new GlobalGameState.PlayerValues(avatar.Team, avatar.PlayerName);
      var tuple = new Tuple<int, GlobalGameState.PlayerValues>(deviceId, playerValues);
      GlobalGameState.Players.Add(tuple);
    }

    GetTree().ChangeSceneToFile(SceneResourceWrapper.WorldPath);
  }

  private bool ValidTeams()
  {
    if (_avatars.Any(p => p.Value.Team == Team.Neutral))
    {
      return false;
    }

    return _avatars.Any();
  }
}
