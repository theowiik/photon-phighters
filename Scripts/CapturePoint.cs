using System.Collections.Generic;
using System.Linq;
using Godot;
using PhotonPhighters.Scripts.Exceptions;
using PhotonPhighters.Scripts.GoSharper;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.GoSharper.Instancing;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

[Instantiable("res://Objects/CapturePoint.tscn")]
public partial class CapturePoint : Node2D
{
  public delegate void CapturedEvent(CapturePoint which, Player.TeamEnum team);

  public enum CapturePointReward
  {
    Explosion,
    Kill
  }

  private const float TimeToCapture = 6f;

  private readonly ISet<CapturePointReward> _capturePointRewards = new HashSet<CapturePointReward>
  {
    CapturePointReward.Explosion,
    CapturePointReward.Kill
  };

  private readonly ICollection<Player> _playersInside = new List<Player>();

  private bool _captured;

  /// <summary>
  ///   Light should reach +TimeToCapture.
  ///   Dark should reach -TimeToCapture.
  /// </summary>
  private float _captureTime;

  [GetNode("ChargePlayer")]
  private AudioStreamPlayer2D _chargePlayer;

  [GetNode("ProgressBar")]
  private ProgressBar _progressBar;

  private float _radius;

  [GetNode("RewardLabel")]
  private Label _rewardLabel;

  public CapturePointReward Reward { get; private set; }

  public CapturedEvent CapturedListeners { get; set; }

  private bool ChargePlayerPlaying
  {
    get => _chargePlayer.Playing;
    set
    {
      if (ChargePlayerPlaying == value)
      {
        return;
      }

      if (value)
      {
        _chargePlayer.Play();
      }
      else
      {
        _chargePlayer.Stop();
      }
    }
  }

  public override void _Draw()
  {
    var noneColor = new Color(0, 1, 0.2f, 0.3f);
    var lightColor = new Color(1, 1, 1, 0.3f);
    var darkColor = new Color(0, 0, 0, 0.3f);
    var tiedColor = new Color(1, 0f, 0, 0.2f);

    var diffPlayers = CalcActiveCaptureDiff();

    Color color;
    if (!_playersInside.Any())
    {
      color = noneColor;
    }
    else
    {
      color = diffPlayers switch
      {
        0 => tiedColor,
        > 0 => lightColor,
        _ => darkColor
      };
    }

    DrawCircle(Vector2.Zero, _radius, color);
  }

  public override void _Process(double delta)
  {
    if (_captured)
    {
      return;
    }

    QueueRedraw();
    var diffPlayers = CalcActiveCaptureDiff();
    if (diffPlayers == 0)
    {
      ChargePlayerPlaying = false;
      return;
    }

    ChargePlayerPlaying = true;

    var diff = diffPlayers > 0 ? 1 : -1;
    _captureTime += diff * (float)delta;

    if (_captureTime >= TimeToCapture)
    {
      _captured = true;
      CapturedListeners?.Invoke(this, Player.TeamEnum.Light);
    }
    else if (_captureTime <= -TimeToCapture)
    {
      _captured = true;
      CapturedListeners?.Invoke(this, Player.TeamEnum.Dark);
    }

    UpdateProgress();
  }

  public override void _Ready()
  {
    this.AutoWire();
    var area = this.GetNodeOrExplode<Area2D>("Area2D");
    area.BodyEntered += OnBodyEntered;
    area.BodyExited += OnBodyExited;

    if (area.GetNodeOrExplode<CollisionShape2D>("CollisionShape2D").Shape is not CircleShape2D circle)
    {
      throw new NodeNotFoundException("CollisionShape2D.Shape is not a CircleShape2D");
    }

    Reward = _capturePointRewards.Sample();
    _rewardLabel.Text = Reward.ToString();

    _radius = circle.Radius;
  }

  private int CalcActiveCaptureDiff()
  {
    var lightPlayers = _playersInside.Count(p => p.Team == Player.TeamEnum.Light);
    var darkPlayers = _playersInside.Count(p => p.Team == Player.TeamEnum.Dark);
    return lightPlayers - darkPlayers;
  }

  private void OnBodyEntered(Node2D body)
  {
    if (body is Player player)
    {
      _playersInside.Add(player);
    }
  }

  private void OnBodyExited(Node2D body)
  {
    if (body is Player player)
    {
      _playersInside.Remove(player);
    }
  }

  private void UpdateProgress()
  {
    var progress = (_captureTime + TimeToCapture) / (TimeToCapture * 2);
    _progressBar.Value = progress;
  }
}
