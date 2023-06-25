using Godot;
using PhotonPhighters.Scripts.Events;
using PhotonPhighters.Scripts.GoSharper;
using PhotonPhighters.Scripts.GoSharper.Instancing;

namespace PhotonPhighters.Scripts;

[GsInstancer("res://Objects/Player/Bullet.tscn")]
public partial class Bullet : Area2D
{
  [Signal]
  public delegate void BulletCollideFloorDelegateEventHandler(BulletCollideFloorEvent bulletCollideFloorEvent);

  [Signal]
  public delegate void BulletFlyingDelegateEventHandler(BulletEvent bulletEvent);

  private readonly float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
  private Vector2 _velocity;
  public float GravityFactor { get; set; } = 1.0f;
  public int Damage { get; set; } = 10;
  public Light.LightMode LightMode { get; set; } = Light.LightMode.Dark;
  public float Speed { get; set; }

  public override void _PhysicsProcess(double delta)
  {
    var bulletFlyingEvent = new BulletEvent(this, _velocity);
    EmitSignal(SignalName.BulletFlyingDelegate, bulletFlyingEvent);
    bulletFlyingEvent.Velocity += new Vector2(0, _gravity * GravityFactor * (float)delta);
    _velocity = bulletFlyingEvent.Velocity;
    Translate(_velocity * (float)delta);
  }

  public override void _Ready()
  {
    _velocity = Vector2.FromAngle(Rotation) * Speed;
    var lifeTimeTimer = this.GetNodeOrExplode<Timer>("Timer");
    lifeTimeTimer.Timeout += OnTimerTimeout;
    lifeTimeTimer.Start(3);
    AreaEntered += OnAreaEntered;
    BodyEntered += OnBodyEntered;

    var sprite = this.GetNodeOrExplode<Sprite2D>("Sprite2D");
    if (LightMode == Light.LightMode.Dark)
    {
      sprite.Modulate = new Color(0, 0, 0);
    }
  }

  private void OnAreaEntered(Area2D area)
  {
    if (!area.IsInGroup("lights") || area is not Light light)
    {
      return;
    }

    light.SetLight(LightMode);
    var bulletCollideFloorEvent = new BulletCollideFloorEvent(this, _velocity, true);
    EmitSignal(SignalName.BulletCollideFloorDelegate, bulletCollideFloorEvent);
    if (bulletCollideFloorEvent.IsFinished)
    {
      QueueFree();
    }
  }

  private void OnBodyEntered(Node2D body)
  {
    if (!body.IsInGroup("floors"))
    {
      return;
    }

    var bulletCollideFloorEvent = new BulletCollideFloorEvent(this, _velocity, true);
    EmitSignal(SignalName.BulletCollideFloorDelegate, bulletCollideFloorEvent);
    if (bulletCollideFloorEvent.IsFinished)
    {
      QueueFree();
    }
  }

  private void OnTimerTimeout()
  {
    QueueFree();
  }
}
