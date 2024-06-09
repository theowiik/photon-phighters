using Godot;
using GodotSharper;
using GodotSharper.AutoGetNode;
using GodotSharper.Instancing;
using PhotonPhighters.Scripts.Events;
using PhotonPhighters.Scripts.Gamepad;
using PhotonPhighters.Scripts.GSAlpha;
using PhotonPhighters.Scripts.Utils.ResourceWrapper;

namespace PhotonPhighters.Scripts;

[Scene("res://Objects/Player/Player.tscn")]
public partial class Player : CharacterBody2D
{
  [Signal]
  public delegate void PlayerDiedEventHandler(Player player);

  public delegate void PlayerEffectAdded(Node2D effect, Player who);

  [Signal]
  public delegate void PlayerHurtEventHandler(Player player, int damage, PlayerHurtEvent playerHurtEvent);

  private const float JoystickDeadzone = 0.05f;
  private bool _canTakeDamage;
  private bool _frozen;

  [GetNode("Marker2D")]
  private Marker2D _gunMarker;

  private int _health;

  [GetNode("HealthBar")]
  private ProgressBar _healthBar;

  private int _maxHealth = 60;

  [GetNode("Sprite2D")]
  private Sprite2D _sprite2D;

  [GetNode("NameLabel")]
  private Label _nameLabel;

  public IGamepad Gamepad { get; set; }

  [GetNode("PlayerEffectsDelegate")]
  public PlayerEffectsDelegate EffectsDelegate { get; private set; }

  private bool CanTakeDamage
  {
    get => _canTakeDamage;
    set
    {
      _canTakeDamage = value;
      ApplyInvincibilityShader(!_canTakeDamage);
    }
  }

  /// <summary>
  ///   A player exits if they are alive and not frozen.
  /// </summary>
  public bool Exists => IsAlive && !Frozen;

  public bool Frozen
  {
    get => _frozen;
    set
    {
      // TODO: This basically acts as a reset for the player. Maybe refactor?

      _frozen = value;
      Gun.Frozen = _frozen;
      Health = MaxHealth;
      PlayerMovementDelegate.Reset();

      // Disable damage or add invincibility to prevent spawn camping
      if (_frozen)
      {
        CanTakeDamage = false;
      }
      else
      {
        AddChild(TimerFactory.StartedSelfDestructingOneShot(1, () => CanTakeDamage = true));
      }

      // Disable collisions
      var collisionShape = this.GetNodeOrExplode<CollisionShape2D>("CollisionShape2D");
      collisionShape.CallDeferred("set_disabled", _frozen);

      PlayerMovementDelegate.ProcessMode = _frozen ? ProcessModeEnum.Disabled : ProcessModeEnum.Inherit;
    }
  }

  [GetNode("Marker2D/Gun")]
  public Gun Gun { get; private set; }

  public bool IsAlive { get; set; }

  public int MaxHealth
  {
    get => _maxHealth;
    set => _maxHealth = Mathf.Max(value, 1);
  }

  public PlayerEffectAdded PlayerEffectAddedListeners { get; set; }

  [GetNode("Movement")]
  public PlayerMovementDelegate PlayerMovementDelegate { get; private set; }

  [Export]
  public Team Team { get; set; }

  /// <summary>
  ///   The player's health.
  ///   Dont set this directly, use TakeDamage instead.
  /// </summary>
  public int Health
  {
    get => _health;
    private set
    {
      _health = value;
      _healthBar.Value = (float)_health / MaxHealth;
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    if (!Exists)
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
    this.GetNodes();

    Health = MaxHealth;
    IsAlive = true;
    Gun.Gamepad = Gamepad;
    Gun.Team = Team;

    EffectsDelegate.PlayerSprite = _sprite2D;
    PlayerMovementDelegate.Gamepad = Gamepad;
    PlayerMovementDelegate.CharacterBody = this;
    PlayerMovementDelegate.PlayerEffectsDelegate = EffectsDelegate;
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

  public void SetName(string name)
  {
    _nameLabel.Text = name;
  }

  public void FlipSprite()
  {
    _sprite2D.FlipV = !_sprite2D.FlipV;
  }

  private void ApplyInvincibilityShader(bool apply)
  {
    if (apply)
    {
      _sprite2D.Material = new ShaderMaterial { Shader = ShaderResourceWrapper.RainbowShader };
    }
    else
    {
      _sprite2D.Material = null;
    }
  }

  public void TakeDamage(int damage)
  {
    if (!Exists || !CanTakeDamage)
    {
      return;
    }

    var playerHurtEvent = new PlayerHurtEvent(damage);
    EmitSignal(SignalName.PlayerHurt, this, damage, playerHurtEvent);
    Health -= playerHurtEvent.Damage;
    EffectsDelegate.EmitHurtParticles();
    EffectsDelegate.PlayHurtSound();
    EffectsDelegate.AnimationPlayHurt();

    if (Health <= 0)
    {
      HandleDeath();
    }
  }

  private void Aim()
  {
    var joystickVector = Gamepad.GetAim();
    if (joystickVector.Length() > JoystickDeadzone)
    {
      _gunMarker.Rotation = joystickVector.Angle();
    }

    var flip = _gunMarker.RotationDegrees is > 90 or < -90;
    _sprite2D.FlipH = flip;
    Gun.FlipTexture(flip);
  }

  private void ApplyBulletKnockback(Bullet bullet)
  {
    var pushDirection = bullet.GlobalPosition.DirectionTo(GlobalPosition);
    var knockback = pushDirection.Normalized() * bullet.Speed * 0.65f;
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
    EffectsDelegate.PlayDeathSound();
    EmitSignal(SignalName.PlayerDied, this);
  }

  private void OnBulletEntered(Area2D area)
  {
    if (!Exists)
    {
      return;
    }

    if (area is not Bullet bullet || bullet.Team == Gun.Team)
    {
      return;
    }

    TakeDamage(bullet.Damage);
    ApplyBulletKnockback(bullet);
    bullet.QueueFree();
  }

  public void VibrateGamepad()
  {
    Gamepad.Vibrate();
  }
}
