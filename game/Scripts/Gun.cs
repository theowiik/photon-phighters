using Godot;
using GodotSharper.AutoGetNode;
using GodotSharper.Instancing;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts;

public partial class Gun : Node2D
{
  [Signal]
  public delegate void BulletCollideFloorEventHandler(BulletCollideFloorEvent bulletEvent);

  [Signal]
  public delegate void BulletFlyingEventHandler(BulletEvent bulletEvent);

  [Signal]
  public delegate void GunShootEventHandler(GunFireEvent shootEvent);

  [Signal]
  public delegate void ShootDelegateEventHandler(Node2D bullet);

  private readonly Color _loadedModulationColor = new(1, 1, 1);

  private readonly Color _reloadModulationColor = new(1, 1, 1, 0.1f);

  private float _fireRate;

  private bool _loading;

  [GetNode("ShootPlayer")]
  private AudioStreamPlayer2D _shootPlayer;

  [GetNode("Timer")]
  private Timer _shootTimer;

  [GetNode("Sprite2D")]
  private Sprite2D _sprite;

  public int BulletCount { get; set; } = 3;
  public int BulletDamage { get; set; } = 5;
  public float BulletGravity { get; set; } = 1.0f;
  public float BulletSizeFactor { get; set; } = 1.0f;
  public float BulletSpeed { get; set; } = 500;
  public GamepadWrapper Gamepad { get; set; }

  /// <summary>
  ///   The spread of the bullets in radians.
  /// </summary>
  public float BulletSpread { get; set; } = 0.1f;

  /// <summary>
  ///   The rate of fire in bullets per second.
  /// </summary>
  public float FireRate
  {
    get => _fireRate;
    set
    {
      _fireRate = value;
      _shootTimer.WaitTime = 1 / _fireRate;
    }
  }

  public bool Frozen { get; set; }
  public Light.LightMode LightMode { get; set; }

  private bool Loading
  {
    get => _loading;
    set
    {
      _loading = value;
      Modulate = _loading ? _reloadModulationColor : _loadedModulationColor;
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    if (Frozen)
    {
      return;
    }

    if (Gamepad.IsShootPressed() && !Loading)
    {
      Shoot();
    }
  }

  public override void _Ready()
  {
    this.GetNodes();
    FireRate = 3f;
    _shootTimer.Timeout += () => Loading = !Loading;
    LightMode = Light.LightMode.Light;
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event.IsActionPressed("dev_switch_light_mode"))
    {
      LightMode = LightMode == Light.LightMode.Light ? Light.LightMode.Dark : Light.LightMode.Light;
    }
  }

  private float GetLightPitch()
  {
    return LightMode == Light.LightMode.Light ? (float)GD.RandRange(1.5f, 1.8f) : (float)GD.RandRange(0.7f, 0.9f);
  }

  private void Shoot()
  {
    _shootPlayer.PitchScale = GetLightPitch();
    _shootPlayer.Play();
    var shootEvent = new GunFireEvent(
      BulletCount,
      BulletDamage,
      BulletGravity,
      BulletSizeFactor,
      BulletSpeed,
      BulletSpread
    );
    EmitSignal(SignalName.GunShoot, shootEvent);

    for (var i = 0; i < shootEvent.BulletCount; i++)
    {
      var bullet = Instanter.Instantiate<Bullet>();

      bullet.BulletCollideFloorDelegate += HandleBulletCollideFloor;
      bullet.BulletFlyingDelegate += HandleBulletFlying;

      var shotSpread = (float)GD.RandRange(-shootEvent.BulletSpread, shootEvent.BulletSpread);

      bullet.GlobalPosition = GlobalPosition;
      bullet.Rotation = GetParent<Marker2D>().Rotation + shotSpread;
      bullet.Speed = (float)GD.RandRange(shootEvent.BulletSpeed * 0.9f, shootEvent.BulletSpeed * 1.1f);
      bullet.Scale *= shootEvent.BulletSizeFactor;
      bullet.GravityFactor = shootEvent.BulletGravity;
      bullet.Damage = shootEvent.BulletDamage;
      bullet.LightMode = LightMode;

      EmitSignal(SignalName.ShootDelegate, bullet);
    }

    Loading = true;
    _shootTimer.Start();
  }

  private void HandleBulletCollideFloor(BulletCollideFloorEvent bulletCollideFloorEvent)
  {
    EmitSignal(SignalName.BulletCollideFloor, bulletCollideFloorEvent);
  }

  private void HandleBulletFlying(BulletEvent bulletEvent)
  {
    EmitSignal(SignalName.BulletFlying, bulletEvent);
  }

  public void FlipTexture(bool flip)
  {
    _sprite.FlipV = flip;
  }
}
