using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Player : CharacterBody2D
{
  private bool _aimWithMouse = true;

  private bool _freeze;

  [GetNode("Marker2D")]
  private Marker2D _gunMarker;

  private int _health;

  [GetNode("HealthBar")]
  private ProgressBar _healthBar;

  [GetNode("PlayerEffectsDelegate")]
  private PlayerEffectsDelegate _playerEffectsDelegate;

  [GetNode("Sprite2D")]
  private Sprite2D _sprite2D;

  [Signal]
  public delegate void PlayerDiedEventHandler(Player player);

  public delegate void PlayerEffectAdded(Node2D effect, Player who);

  [Signal]
  public delegate void PlayerHurtEventHandler(Player player, int damage);

  public enum TeamEnum
  {
    Light,
    Dark
  }

  public bool Freeze
  {
    get => _freeze;
    set
    {
      _freeze = value;
      Gun.Freeze = _freeze;
      Health = MaxHealth;

      var collisionShape = this.GetNodeOrExplode<CollisionShape2D>("CollisionShape2D");
      collisionShape.Disabled = _freeze;

      if (_freeze)
      {
        PlayerMovementDelegate.ProcessMode = ProcessModeEnum.Disabled;
        _playerEffectsDelegate.AnimationPlaySpawn();
      }
      else
      {
        PlayerMovementDelegate.ProcessMode = ProcessModeEnum.Inherit;
        _sprite2D.Modulate = Colors.White;
      }
    }
  }

  [GetNode("Marker2D/Gun")]
  public Gun Gun { get; private set; }

  public bool IsAlive { get; set; }
  public int MaxHealth { get; set; } = 50;
  public PlayerEffectAdded PlayerEffectAddedListeners { get; set; }

  [GetNode("Movement")]
  public PlayerMovementDelegate PlayerMovementDelegate { get; private set; }

  [Export]
  public int PlayerNumber { get; set; }

  public TeamEnum Team => PlayerNumber == 1 ? TeamEnum.Light : TeamEnum.Dark;

  /// <summary>
  ///   The player's health.
  ///   Dont set this directly, use TakeDamage instead.
  /// </summary>
  private int Health
  {
    get => _health;
    set
    {
      _health = value;
      _healthBar.Value = (float)_health / MaxHealth;
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    if (Freeze)
    {
      return;
    }

    // TODO: Move this to PlayerMovementDelegate
    if (PlayerMovementDelegate.HasReachedAerodynamicHeatingVelocity)
    {
      TakeDamage((int)(999 * delta));
    }

    Aim();
  }

  public override void _Ready()
  {
    this.AutoWire();

    Health = MaxHealth;
    IsAlive = true;
    Gun.ShootActionName = $"p{PlayerNumber}_shoot";
    Gun.LightMode = PlayerNumber == 1 ? Light.LightMode.Light : Light.LightMode.Dark;

    _playerEffectsDelegate.PlayerSprite = _sprite2D;
    PlayerMovementDelegate.PlayerNumber = PlayerNumber;
    PlayerMovementDelegate.CharacterBody = this;
    PlayerMovementDelegate.PlayerEffectsDelegate = _playerEffectsDelegate;
    PlayerMovementDelegate.PlayerEffectsDelegate.PlayerEffectAddedListeners += effect =>
      PlayerEffectAddedListeners?.Invoke(effect, this);

    // Gun
    var bulletDetectionArea = this.GetNodeOrExplode<Area2D>("BulletDetectionArea");
    bulletDetectionArea.AreaEntered += OnBulletEntered;
  }

  public void ResetHealth()
  {
    Health = MaxHealth;
  }

  public void TakeDamage(int damage)
  {
    if (Freeze)
    {
      return;
    }

    if (!IsAlive)
    {
      return;
    }

    Health -= damage;
    EmitSignal(SignalName.PlayerHurt, this, damage);
    _playerEffectsDelegate.EmitHurtParticles();
    _playerEffectsDelegate.PlayHurtSound();
    _playerEffectsDelegate.AnimationPlayHurt();

    if (Health <= 0)
    {
      HandleDeath();
    }
  }

  private void Aim()
  {
    var joystickDeadzone = 0.05f;
    var joystickVector = new Vector2(
      Input.GetJoyAxis(PlayerNumber - 1, JoyAxis.RightX),
      Input.GetJoyAxis(PlayerNumber - 1, JoyAxis.RightY)
    );

    // Controller has priority over mouse.
    if (joystickVector.Length() > joystickDeadzone)
    {
      _gunMarker.Rotation = joystickVector.Angle();
      _aimWithMouse = false;
    }

    // Only player one can play with mouse and keyboard.
    if (PlayerNumber == 1 && _aimWithMouse)
    {
      var direction = GetGlobalMousePosition() - GlobalPosition;
      _gunMarker.Rotation = direction.Angle();
    }
  }

  private void ApplyBulletKnockback(Bullet bullet)
  {
    var pushDirection = bullet.GlobalPosition.DirectionTo(GlobalPosition);
    var knockback = pushDirection.Normalized() * bullet.Speed;
    PlayerMovementDelegate.AddKnockback(knockback);
  }

  private void HandleDeath()
  {
    if (!IsAlive)
    {
      return;
    }

    IsAlive = false;

    PlayerMovementDelegate.Reset();
    _playerEffectsDelegate.PlayDeathSound();
    EmitSignal(SignalName.PlayerDied, this);
  }

  private void OnBulletEntered(Area2D area)
  {
    if (Freeze)
    {
      return;
    }

    if (!IsAlive)
    {
      return;
    }

    if (area is Bullet bullet && bullet.LightMode != Gun.LightMode)
    {
      TakeDamage(bullet.Damage);
      ApplyBulletKnockback(bullet);
      bullet.QueueFree();
    }
  }
}
