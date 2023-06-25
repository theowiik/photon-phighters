using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using PhotonPhighters.Scripts.Events;
using PhotonPhighters.Scripts.Exceptions;
using PhotonPhighters.Scripts.GoSharper;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.GoSharper.Instancing;
using PhotonPhighters.Scripts.OverlayControllers;
using PhotonPhighters.Scripts.Utils.ResourceWrapper;

namespace PhotonPhighters.Scripts;

public partial class World : Node2D
{
  private const float RespawnTime = 2.3f;
  private const int TimeBetweenCapturePoint = 10;

  private readonly PackedScene _ragdollDarkScene = GD.Load<PackedScene>(ObjectResourceWrapper.DarkRagdollPath);

  private readonly PackedScene _ragdollLightScene = GD.Load<PackedScene>(ObjectResourceWrapper.LightRagdollPath);

  [GetNode("FollowingCamera")]
  private FollowingCamera _camera;

  private Player _darkPlayer;

  [GetNode("Sfx/DarkWin")]
  private AudioStreamPlayer _darkWin;

  private Player _lastPlayerToScore;
  private Player _lightPlayer;

  [GetNode("Sfx/LightWin")]
  private AudioStreamPlayer _lightWin;

  [GetNode("MapManager")]
  private MapManager _mapManager;

  [GetNode("MusicPlayer")]
  private MusicPlayer _musicPlayer;

  [GetNode("CanvasLayer/Overlay")]
  private Overlay _overlay;

  [GetNode("CanvasLayer/PauseOverlay")]
  private PauseOverlay _pauseOverlay;

  private IEnumerable<Player> _players;

  [GetNode("CanvasLayer/PowerUpPicker")]
  private PowerUpPicker _powerUpPicker;

  private int _roundsToWin = 10;
  private int _roundTime = 40;

  [GetNode("RoundTimer")]
  private Timer _roundTimer;

  private Score _score;

  public override void _Ready()
  {
    this.AutoWire();
    _score = new Score();

    if (GlobalGameState.RoundTime > 0)
    {
      _roundTime = GlobalGameState.RoundTime;
    }

    if (GlobalGameState.RoundsToWin > 0)
    {
      _roundsToWin = GlobalGameState.RoundsToWin;
    }

    // UI
    var uiUpdateTimer = this.GetNodeOrExplode<Timer>("UIUpdateTimer");
    uiUpdateTimer.Timeout += UpdateScore;
    uiUpdateTimer.Timeout += UpdateRoundTimer;
    _roundTimer.Timeout += OnRoundFinished;
    _pauseOverlay.ResumeGame += TogglePause;
    _pauseOverlay.PowerUpPickedListeners += OnPowerUpSelectedBoth;
    _powerUpPicker.Visible = false;
    _powerUpPicker.PowerUpPickedListeners += OnPowerUpSelected;

    // Setup map
    _mapManager.OutOfBoundsEventListeners += OnOutOfBounds;

    // Setup players
    _players = GetTree().GetNodesInGroup("players").Cast<Player>().ToList();
    foreach (var player in _players)
    {
      player.Frozen = true;
      player.PlayerDied += OnPlayerDied;
      player.PlayerHurt += OnPlayerHurt;
      player.Gun.ShootDelegate += OnShoot;
      _camera.AddTarget(player);
      player.PlayerEffectAddedListeners += OnPlayerEffectAdded;
    }

    _lightPlayer = _players.First(p => p.PlayerNumber == 1);
    _darkPlayer = _players.First(p => p.PlayerNumber == 2);

    if (_lightPlayer == null || _darkPlayer == null)
    {
      throw new NodeNotFoundException("Could not find both players");
    }

    // Start round
    SetupCapturePoint();
    StartRound();
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event.IsActionPressed("dev_finish_round"))
    {
      _roundTimer.Start(0.00001);
    }
  }

  private static string GetRandomDeathMessage()
  {
    var deathMessages = new List<string>
    {
      "oof",
      "ouch",
      "ow",
      "yikes",
      "rip",
      "x_x",
      "-_-",
      "T_T",
      "u_u",
      "X_X",
      "@_@",
      "(X_X)",
      "[-_-]",
      "{x_x}",
      "[T_T]",
      "{@_@}",
      "<x_x>",
      "(.-.)",
      "[._.]",
      "<@_@>",
      "(xOx)",
      "[x_x]",
      "<-_->",
      "{-_-}",
      "(XoX)"
    };
    return deathMessages[GD.RandRange(0, deathMessages.Count - 1)];
  }

  private static void OnOutOfBounds(Player player)
  {
    if (player.Exists)
    {
      player.TakeDamage(99999);
    }
  }

  private Results GetResults()
  {
    var lights = GetTree().GetNodesInGroup("lights");
    var results = new Results();

    foreach (var light in lights)
    {
      if (light is not Light lightNode)
      {
        throw new NodeNotFoundException("Light node is not a Light!!");
      }

      switch (lightNode.LightState)
      {
        case Light.LightMode.Light:
          results.Light++;
          break;

        case Light.LightMode.Dark:
          results.Dark++;
          break;

        case Light.LightMode.None:
          results.Neutral++;
          break;

        default:
          throw new ArgumentOutOfRangeException(nameof(lightNode), "Light state is not a valid state");
      }
    }

    return results;
  }

  private void OnCapturePointCaptured(CapturePoint which, Player.TeamEnum team)
  {
    var light = team == Player.TeamEnum.Light ? Light.LightMode.Light : Light.LightMode.Dark;
    SpawnExplosion(which, light, Explosion.ExplosionRadiusEnum.Large);
    which.QueueFree();
  }

  private void OnPlayerDied(Player player)
  {
    var oppositeLight = player.Team == Player.TeamEnum.Light ? Light.LightMode.Dark : Light.LightMode.Light;

    SpawnRagdoll(player);
    SpawnExplosion(player, oppositeLight, Explosion.ExplosionRadiusEnum.Medium);
    SpawnHurtIndicator(player, GetRandomDeathMessage());

    player.Frozen = true;
    player.GlobalPosition = _mapManager.GetRandomSpawnPoint().GlobalPosition;

    var liveTimer = GsTimerFactory.OneShotSelfDestructingStartedTimer(
      RespawnTime,
      () =>
      {
        player.Frozen = false;
        player.IsAlive = true;
      }
    );

    AddChild(liveTimer);
    liveTimer.Start();
  }

  private void OnPlayerEffectAdded(Node2D effect, Player who)
  {
    AddChild(effect);
    effect.GlobalPosition = who.GlobalPosition;
  }

  private void OnPlayerHurt(Player player, int damage, PlayerHurtEvent playerHurtEvent)
  {
    SpawnHurtIndicator(player, damage.ToString());
  }

  private void OnPowerUpSelected(PowerUps.IPowerUpApplier powerUpApplier)
  {
    _powerUpPicker.Visible = false;

    var loser = _lastPlayerToScore.Team == Player.TeamEnum.Light ? _darkPlayer : _lightPlayer;
    var winner = _lastPlayerToScore.Team == Player.TeamEnum.Light ? _lightPlayer : _darkPlayer;
    powerUpApplier.Apply(loser, winner);

    StartRound();
  }

  /// <summary>
  ///   Called when a power up is selected for both players
  /// </summary>
  private void OnPowerUpSelectedBoth(PowerUps.IPowerUpApplier powerUpApplier)
  {
    powerUpApplier.Apply(_lightPlayer, _darkPlayer);
    powerUpApplier.Apply(_darkPlayer, _lightPlayer);
  }

  private void OnRoundFinished()
  {
    foreach (var player in _players)
    {
      player.Frozen = true;
    }

    // Remove all bullets
    foreach (var bullet in GetTree().GetNodesInGroup("bullets"))
    {
      bullet.QueueFree();
    }

    // Remove all capture points
    foreach (var capturePoint in GetTree().GetNodesInGroup("capture_points"))
    {
      capturePoint.QueueFree();
    }

    var results = GetResults();
    if (results.Light == results.Dark)
    {
      _score.Ties++;
      StartRound();
      return;
    }

    if (results.Light > results.Dark)
    {
      _score.Light++;
      _lastPlayerToScore = _lightPlayer;
      _lightWin.Play();
    }
    else
    {
      _score.Dark++;
      _lastPlayerToScore = _darkPlayer;
      _darkWin.Play();
    }

    _overlay.SetTotalScore($"Light vs Dark: {_score.Light} - {_score.Dark}");
    if (_score.Dark >= _roundsToWin || _score.Light >= _roundsToWin)
    {
      GetTree()
        .ChangeSceneToFile(
          _score.Light > _score.Dark
            ? SceneResourceWrapper.EndScreenLightnessPath
            : SceneResourceWrapper.EndScreenDarknessPath
        );
    }

    _musicPlayer.SetPitch(_score.Light, _score.Dark, _roundsToWin);
    StartPowerUpSelection();
  }

  private void OnShoot(Node2D bullet)
  {
    AddChild(bullet);
  }

  private void ResetLights()
  {
    foreach (var light in GetTree().GetNodesInGroup("lights"))
    {
      if (light is not Light lightNode)
      {
        throw new NodeNotFoundException("Light node is not a Light!!");
      }

      lightNode.SetLight(Light.LightMode.None);
    }
  }

  private void SetupCapturePoint()
  {
    const int MaxConcurrentCapturePoints = 2;
    var timer = GsTimerFactory.StartedTimer(TimeBetweenCapturePoint);

    AddChild(timer);

    timer.Timeout += () =>
    {
      if (GetTree().GetNodesInGroup("capture_points").Count >= MaxConcurrentCapturePoints)
      {
        return;
      }

      SpawnRandomCapturePoint();
    };
  }

  private void SpawnRandomCapturePoint()
  {
    var capturePoint = GsInstanter.Instantiate<CapturePoint>();
    AddChild(capturePoint);
    capturePoint.CapturedListeners += OnCapturePointCaptured;

    var offset = new Vector2(GD.RandRange(-25, 25), GD.RandRange(-25, 25));
    capturePoint.GlobalPosition = _mapManager.GetRandomSpawnPoint().GlobalPosition + offset;

    // De-spawn after a while
    var timer = GsTimerFactory.OneShotStartedTimer(
      30,
      () =>
      {
        capturePoint.QueueFree();
      }
    );

    capturePoint.AddChild(timer);
  }

  private void SpawnExplosion(Node2D where, Light.LightMode who, Explosion.ExplosionRadiusEnum explosionRadius)
  {
    var explosion = GsInstanter.Instantiate<Explosion>();
    explosion.LightMode = who;
    explosion.Radius = explosionRadius;
    CallDeferred("add_child", explosion);
    explosion.GlobalPosition = where.GlobalPosition;
    _camera.Shake(0.6f, FollowingCamera.ShakeStrength.Strong);
  }

  private void SpawnHurtIndicator(Node2D player, string msg)
  {
    var indicator = GsInstanter.Instantiate<DamageAmountIndicator>();
    indicator.AddChild(GsTimerFactory.OneShotStartedTimer(6, () => indicator.QueueFree()));
    AddChild(indicator);
    indicator.GlobalPosition = player.GlobalPosition;
    indicator.SetMessage(msg);
  }

  private void SpawnRagdoll(Player player)
  {
    var ragdoll =
      player.Team == Player.TeamEnum.Light
        ? _ragdollLightScene.Instantiate<RigidBody2D>()
        : _ragdollDarkScene.Instantiate<RigidBody2D>();
    var timer = GsTimerFactory.OneShotStartedTimer(5, () => ragdoll.QueueFree());
    ragdoll.AddChild(timer);
    CallDeferred("add_child", ragdoll);

    ragdoll.GlobalPosition = player.GlobalPosition;
    var angleVec = -Vector2.Right.Rotated((float)GD.RandRange(0, Math.PI));
    ragdoll.ApplyCentralImpulse(angleVec * (float)GD.RandRange(600f, 900f));
    ragdoll.AngularVelocity = GD.RandRange(-50, 50);
  }

  private void StartPowerUpSelection()
  {
    var losingPlayer = _lastPlayerToScore == _lightPlayer ? _darkPlayer : _lightPlayer;
    _powerUpPicker.Visible = true;
    _powerUpPicker.GrabFocus();
    _powerUpPicker.Reset(losingPlayer);
  }

  private void StartRound()
  {
    _mapManager.InitNextMap();
    ResetLights();

    _lightPlayer.GlobalPosition = _mapManager.GetRandomSpawnPoint().GlobalPosition;
    _darkPlayer.GlobalPosition = _mapManager.GetRandomSpawnPoint().GlobalPosition;
    ForceUpdateTransform();

    foreach (var player in _players)
    {
      player.Frozen = false;
    }

    // TODO: Hack to ensure players are moved before activating the map
    AddChild(GsTimerFactory.OneShotSelfDestructingStartedTimer(1, () => _mapManager.StartNextMap()));
    // _mapManager.StartNextMap(); // <- Should be done similar to this
    _roundTimer.Start(_roundTime);
  }

  private void TogglePause()
  {
    var isPaused = !_pauseOverlay.Enabled;
    _pauseOverlay.Enabled = isPaused;
    GetTree().Paused = isPaused;
  }

  private void UpdateRoundTimer()
  {
    _overlay.SetTime($"{_roundTimer.TimeLeft:0.0}");
  }

  private void UpdateScore()
  {
    var results = GetResults();

    if (results is { Light: 0, Dark: 0 })
    {
      return;
    }

    _overlay.SetRoundScore(results);
  }

  public struct Results : IEquatable<Results>
  {
    public int Dark { get; set; }
    public int Light { get; set; }
    public int Neutral { get; set; }

    public bool Equals(Results other)
    {
      return Dark == other.Dark && Light == other.Light && Neutral == other.Neutral;
    }

    public override bool Equals(object obj)
    {
      return obj is Results other && Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Dark, Light, Neutral);
    }

    public static bool operator ==(Results left, Results right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Results left, Results right)
    {
      return !left.Equals(right);
    }
  }

  private struct Score : IEquatable<Score>
  {
    public int Dark { get; set; }
    public int Light { get; set; }
    public int Ties { get; set; }

    public bool Equals(Score other)
    {
      return Dark == other.Dark && Light == other.Light && Ties == other.Ties;
    }

    public override bool Equals(object obj)
    {
      return obj is Score other && Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Dark, Light, Ties);
    }

    public static bool operator ==(Score left, Score right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Score left, Score right)
    {
      return !left.Equals(right);
    }
  }
}
