using Godot;
using PhotonPhighters.Scripts.Events;
using PhotonPhighters.Scripts.GoSharper;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.Utils.ResourceWrapper;

namespace PhotonPhighters.Scripts;

public partial class Player : CharacterBody2D
{
  [Signal]
  public delegate void PlayerDiedEventHandler(Player player);

  public delegate void PlayerEffectAdded(Node2D effect, Player who);

  [Signal]
  public delegate void PlayerHurtEventHandler(Player player, int damage, PlayerHurtEvent playerHurtEvent);

  public enum TeamEnum
  {
    Light,
    Dark
  }

  private bool _aimWithMouse = true;

  private bool _canTakeDamage;
  private bool _frozen;
  private int _gamepadIndex;

  [GetNode("Marker2D")]
  private Marker2D _gunMarker;

  private int _health;

  [GetNode("HealthBar")]
  private ProgressBar _healthBar;

  private int _maxHealth = 60;

  [GetNode("Sprite2D")]
  private Sprite2D _sprite2D;

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
        AddChild(GsTimerFactory.OneShotSelfDestructingStartedTimer(1, () => CanTakeDamage = true));
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
  public int PlayerNumber { get; set; }

  public TeamEnum Team => PlayerNumber == 1 ? TeamEnum.Light : TeamEnum.Dark;

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

  public Light.LightMode LightMode => PlayerNumber == 1 ? Light.LightMode.Light : Light.LightMode.Dark;

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
    this.AutoWire();

    Health = MaxHealth;
    IsAlive = true;
    _gamepadIndex = GetGamepadIndex();
    Gun.ShootActionName = $"p{PlayerNumber}_shoot";
    Gun.LightMode = PlayerNumber == 1 ? Light.LightMode.Light : Light.LightMode.Dark;

    EffectsDelegate.PlayerSprite = _sprite2D;
    PlayerMovementDelegate.PlayerNumber = PlayerNumber;
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
    const float JoystickDeadzone = 0.05f;
    var joystickVector = new Vector2(
      Input.GetJoyAxis(PlayerNumber - 1, JoyAxis.RightX),
      Input.GetJoyAxis(PlayerNumber - 1, JoyAxis.RightY)
    );

    // Controller has priority over mouse.
    if (joystickVector.Length() > JoystickDeadzone)
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

    if (area is not Bullet bullet || bullet.LightMode == Gun.LightMode)
    {
      return;
    }

    TakeDamage(bullet.Damage);
    ApplyBulletKnockback(bullet);
    bullet.QueueFree();
  }

  private int GetGamepadIndex()
  {
    var connectedGamePads = Input.GetConnectedJoypads();
    return connectedGamePads.Count == 2 ? connectedGamePads[PlayerNumber - 1] : 0;
  }

  public void VibrateGamepadStrong(float seconds)
  {
    Input.StartJoyVibration(_gamepadIndex, 0.25f, 0.25f, seconds);
  }

  public void VibrateGamepadWeak(float seconds)
  {
    Input.StartJoyVibration(_gamepadIndex, 0.75f, 0.75f, seconds);
  }
}
