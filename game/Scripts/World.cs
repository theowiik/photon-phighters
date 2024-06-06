using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotSharper;
using GodotSharper.AutoGetNode;
using GodotSharper.Instancing;
using PhotonPhighters.Scripts.Events;
using PhotonPhighters.Scripts.Exceptions;
using PhotonPhighters.Scripts.GameMode;
using PhotonPhighters.Scripts.GSAlpha;
using PhotonPhighters.Scripts.OverlayControllers;
using PhotonPhighters.Scripts.PowerUps;
using PhotonPhighters.Scripts.Utils.ResourceWrapper;

namespace PhotonPhighters.Scripts;

public partial class World : Node2D
{
  private const float RespawnTime = 2.3f;
  private const int TimeBetweenCapturePoint = 10;

  private readonly IGameMode _gameMode = new PhotonPhight();
  private readonly PackedScene _ragdollDarkScene = GD.Load<PackedScene>(ObjectResourceWrapper.DarkRagdollPath);
  private readonly PackedScene _ragdollLightScene = GD.Load<PackedScene>(ObjectResourceWrapper.LightRagdollPath);

  [GetNode("FollowingCamera")]
  private FollowingCamera _camera;

  [GetNode("Sfx/DarkWin")]
  private AudioStreamPlayer _darkWin;

  private Team _lastTeamToScore;

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

  [GetNode("CanvasLayer/PowerUpsHUD")]
  private PowerUpsHUD _powerUpsHud;

  [GetNode("RoundTimer")]
  private Timer _roundTimer;

  private Score _score;
  public RoundState RoundState { get; } = new();

  public override void _Ready()
  {
    this.GetNodes();
    _score = new Score();

    if (GlobalGameState.RoundTime > 0)
    {
      RoundState.RoundTime = GlobalGameState.RoundTime;
    }

    if (GlobalGameState.RoundsToWin > 0)
    {
      RoundState.RoundsToWin = GlobalGameState.RoundsToWin;
    }

    // UI
    var uiUpdateTimer = this.GetNodeOrExplode<Timer>("UIUpdateTimer");
    uiUpdateTimer.Timeout += UpdateScore;
    uiUpdateTimer.Timeout += UpdateRoundTimer;
    _roundTimer.Timeout += OnRoundFinished;
    _pauseOverlay.ResumeGame += TogglePause;
    _pauseOverlay.PowerUpPickedListeners += OnPowerUpSelectedBoth;
    _powerUpPicker.Visible = false;
    _powerUpPicker.PowerUpSelectionEndedListeners += OnPowerUpSelected;

    // Setup map
    _mapManager.OutOfBoundsEventListeners += OnOutOfBounds;
    _mapManager.GameMode = _gameMode;

    // Setup players
    SpawnPlayers();

    // Start round
    SetupCapturePoint();
    StartRound();
  }

  /// <summary>
  ///   Done ONCE at the start of the game.
  ///   Players are never removed from the game when they die.
  /// </summary>
  private void SpawnPlayers()
  {
    if (GlobalGameState.Players.Count < 2)
    {
      throw new ArgumentOutOfRangeException(nameof(GlobalGameState.Players), "Two players are required to start the game");
    }

    if (GlobalGameState.Players.Any(x => x.Value == Team.Neutral))
    {
      throw new ArgumentOutOfRangeException(nameof(GlobalGameState.Players), "Cannot have neutral players");
    }

    var lightPlayers = GlobalGameState.Players.Count(x => x.Value == Team.Light);
    var darkPlayers = GlobalGameState.Players.Count(x => x.Value == Team.Dark);

    lightPlayers.TimesDo(() =>
    {
      var packedScene = GDX.LoadOrFail<PackedScene>(ObjectResourceWrapper.LightPLayerPath);
      var player = packedScene.Instantiate<Player>();
      AddChild(player);
    });

    darkPlayers.TimesDo(() =>
    {
      var packedScene = GDX.LoadOrFail<PackedScene>(ObjectResourceWrapper.DarkPlayerPath);
      var player = packedScene.Instantiate<Player>();
      AddChild(player);
    });

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

    var hasBoth = _players.Any(x => x.Team == Team.Dark) && _players.Any(x => x.Team == Team.Light);
    if (!hasBoth)
    {
      throw new NodeNotFoundException("Match does not have both light and dark players");
    }
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event.IsActionPressed("dev_finish_round"))
    {
      _roundTimer.Start(0.00001);
    }

    // Test vibration
    if (@event is InputEventKey keyEvent && int.TryParse(keyEvent.AsTextKeycode(), out var number))
    {
      new GamepadWrapper(number - 1).Vibrate();
    }
  }

  private static void OnOutOfBounds(Player player)
  {
    if (player.Exists)
    {
      player.TakeDamage(99999);
    }
  }

  private Score GetResults()
  {
    return _gameMode.GetResults(this);
  }

  private static Team Opposite(Team team)
  {
    if (team == Team.Neutral)
    {
      throw new ArgumentOutOfRangeException(nameof(team), team, "Cannot get opposite of neutral team");
    }

    return team == Team.Dark ? Team.Dark : Team.Light;
  }

  private void OnCapturePointCaptured(CapturePoint which, Team team)
  {
    var otherTeam = Opposite(team);

    switch (which.Reward)
    {
      case CapturePoint.CapturePointReward.Explosion:
        SpawnExplosion(which, team, Explosion.ExplosionSize.Large);
        break;
      case CapturePoint.CapturePointReward.Kill:
        _players.Where(p => p.Team == otherTeam).ToList().ForEach(p => p.TakeDamage(999_999));
        break;
    }

    //

    which.QueueFree();
  }

  private void OnPlayerDied(Player player)
  {
    RoundState.IncrementDeathForTeam(player.Team);
    var oppositeLight = player.Team == Team.Light ? Team.Dark : Team.Light;

    SpawnRagdoll(player);
    SpawnExplosion(player, oppositeLight, Explosion.ExplosionSize.Medium);

    player.Frozen = true;
    player.GlobalPosition = _mapManager.GetRandomSpawnPoint().GlobalPosition;

    var liveTimer = TimerFactory.StartedSelfDestructingOneShot(
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

  private void OnPowerUpSelected(IPowerUpApplier powerUpApplier)
  {
    _powerUpsHud.Add(powerUpApplier, Opposite(_lastTeamToScore));
    StartRound();
  }

  // TODO: Remove
  private Team GetLosingTeam()
  {
    return Opposite(_lastTeamToScore);
  }

  // TODO: Remove
  private Team GetWinningLight()
  {
    return _lastTeamToScore;
  }

  /// <summary>
  ///   Called when a power up is selected for both players
  /// </summary>
  private void OnPowerUpSelectedBoth(IPowerUpApplier powerUpApplier)
  {
    foreach (var player in _players)
    {
      powerUpApplier.Apply(player, player);
    }
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
      _lastTeamToScore = Team.Light;
      _lightWin.Play();
    }
    else
    {
      _score.Dark++;
      _lastTeamToScore = Team.Dark;
      _darkWin.Play();
    }

    _overlay.SetTotalScore($"Light vs Dark: {_score.Light} - {_score.Dark}");
    if (_score.Dark >= RoundState.RoundsToWin || _score.Light >= RoundState.RoundsToWin)
    {
      GetTree()
        .ChangeSceneToFile(
          _score.Light > _score.Dark
            ? SceneResourceWrapper.EndScreenLightnessPath
            : SceneResourceWrapper.EndScreenDarknessPath
        );
    }

    _musicPlayer.SetPitch(_score.Light, _score.Dark, RoundState.RoundsToWin);
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

      lightNode.SetLight(Team.Neutral);
    }
  }

  private void SetupCapturePoint()
  {
    const int MaxConcurrentCapturePoints = 2;
    var timer = TimerFactory.StartedRepeating(TimeBetweenCapturePoint);

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
    var capturePoint = Instanter.Instantiate<CapturePoint>();
    AddChild(capturePoint);
    capturePoint.CapturedListeners += OnCapturePointCaptured;

    var offset = new Vector2(GD.RandRange(-25, 25), GD.RandRange(-25, 25));
    capturePoint.GlobalPosition = _mapManager.GetRandomSpawnPoint().GlobalPosition + offset;

    // De-spawn after a while
    var timer = TimerFactory.StartedSelfDestructingOneShot(
      30,
      () =>
      {
        capturePoint.QueueFree();
      }
    );

    capturePoint.AddChild(timer);
  }

  private void SpawnExplosion(Node2D where, Team who, Explosion.ExplosionSize explosionRadius)
  {
    var explosion = Instanter.Instantiate<Explosion>();
    explosion.Team = who;
    explosion.Size = explosionRadius;
    CallDeferred("add_child", explosion);
    explosion.GlobalPosition = where.GlobalPosition;
    _camera.Shake(0.6f, FollowingCamera.ShakeStrength.Strong);
  }

  private void SpawnHurtIndicator(Node2D player, string msg)
  {
    var indicator = Instanter.Instantiate<DamageAmountIndicator>();
    indicator.AddChild(TimerFactory.StartedSelfDestructingOneShot(6, () => indicator.QueueFree()));
    AddChild(indicator);
    indicator.GlobalPosition = player.GlobalPosition;
    indicator.SetMessage(msg);
  }

  private void SpawnRagdoll(Player player)
  {
    var ragdoll =
      player.Team == Team.Light
        ? _ragdollLightScene.Instantiate<RigidBody2D>()
        : _ragdollDarkScene.Instantiate<RigidBody2D>();
    var timer = TimerFactory.StartedSelfDestructingOneShot(5, () => ragdoll.QueueFree());
    ragdoll.AddChild(timer);
    CallDeferred("add_child", ragdoll);

    ragdoll.GlobalPosition = player.GlobalPosition;
    var angleVec = -Vector2.Right.Rotated((float)GD.RandRange(0, Math.PI));
    ragdoll.ApplyCentralImpulse(angleVec * (float)GD.RandRange(600f, 900f));
    ragdoll.AngularVelocity = GD.RandRange(-50, 50);
  }

  private void StartPowerUpSelection()
  {
    var loser = GetLosingTeam();
    var winner = GetWinningLight();
    _powerUpPicker.BeginPowerUpSelection(winner, loser, _players.ToList());
  }

  private void StartRound()
  {
    _gameMode.RoundStarted(this);

    RoundState.Reset();
    _mapManager.InitNextMap();
    ResetLights();

    _players.ForEach(p => p.GlobalPosition = _mapManager.GetRandomSpawnPoint().GlobalPosition);
    ForceUpdateTransform();
    _players.ForEach(p => p.Frozen = false);

    // TODO: Hack to ensure players are moved before activating the map
    AddChild(TimerFactory.StartedSelfDestructingOneShot(1, () => _mapManager.StartNextMap()));
    // _mapManager.StartNextMap(); // <- Should be done similar to this
    _roundTimer.Start(RoundState.RoundTime);
  }

  private void TogglePause()
  {
    var isPaused = !_pauseOverlay.Enabled;
    _pauseOverlay.Enabled = isPaused;
    GetTree().Paused = isPaused;
  }

  private void UpdateRoundTimer()
  {
    _overlay.SetTime($"{_roundTimer.TimeLeft:0}");
  }

  private void UpdateScore()
  {
    _overlay.SetRoundScore(GetResults());
  }
}
