using Godot;
using PhotonPhighters.Scripts.Events;
using PhotonPhighters.Scripts.Gamepad;

namespace PhotonPhighters.Scripts;

public partial class PlayerMovementDelegate : Node
{
  [Signal]
  public delegate void PlayerJumpEventHandler(PlayerMovementEvent playerMoveEvent);

  [Signal]
  public delegate void PlayerLandEventHandler(PlayerMovementEvent playerMoveEvent);

  /// <summary>
  ///   The speed of which the player start taking damage from aerodynamic heating
  /// </summary>
  [Signal]
  public delegate void PlayerMoveEventHandler(PlayerMovementEvent playerMoveEvent);

  [Signal]
  public delegate void PlayerStoppedEventHandler(PlayerMovementEvent playerMoveEvent);

  [Signal]
  public delegate void PlayerWallJumpEventHandler(PlayerMovementEvent playerMoveEvent);

  private const int AerodynamicHeatingVelocity = 10_000;
  private float _gravity = 800;
  private const float Deceleration = 12f;
  private const float GlideGravityScale = 0.5f;
  private const float KnockbackDecayRate = 0.04f;
  private bool _isLeftArrowPressed;
  private bool _isRightArrowPressed;
  private bool _isWaitingForSecondTap;
  private int _jumpCount;
  private Vector2 _knockback;
  private bool _onFloorLastCall;
  private float _speed = 300;
  private Vector2 _velocity;
  public IGamepad Gamepad { get; set; }
  public float Acceleration { get; set; } = 12f;
  public CharacterBody2D CharacterBody { get; set; }
  public bool HasReachedAerodynamicHeatingVelocity => _velocity.Length() > AerodynamicHeatingVelocity;
  public float JumpForce { get; set; } = 500;
  public int MaxJumps { get; set; } = 2;
  public PlayerEffectsDelegate PlayerEffectsDelegate { get; set; }

  public float Speed
  {
    get => _speed;
    set => _speed = Mathf.Max(100f, value);
  }

  public float Gravity
  {
    get => _gravity;
    set => _gravity = value;
  }

  public override void _PhysicsProcess(double delta)
  {
    var inputDirection = Gamepad.GetMovement();

    var moveEvent = new PlayerMovementEvent(
      Gravity,
      _speed,
      _velocity,
      inputDirection,
      true,
      CharacterBody,
      true,
      JumpForce,
      MaxJumps
    );
    EmitSignal(SignalName.PlayerMove, moveEvent);

    if (!moveEvent.CanMove)
    {
      moveEvent.InputDirection = new Vector2(0, 0);
    }

    // Stopped
    if (Equals(moveEvent.InputDirection, new Vector2(0, 0)) && Equals(moveEvent.Velocity, new Vector2(0, 0)))
    {
      EmitSignal(SignalName.PlayerStopped, moveEvent);
    }

    // Walking
    var targetSpeed = moveEvent.InputDirection.X * moveEvent.Speed;
    var acceleration = moveEvent.InputDirection.X != 0 ? Acceleration : Deceleration;
    moveEvent.Velocity = new Vector2(
      Mathf.Lerp(moveEvent.Velocity.X, targetSpeed, acceleration * (float)delta),
      moveEvent.Velocity.Y
    );

    var onFloor = CharacterBody.IsOnFloor();
    var onCeiling = CharacterBody.IsOnCeiling();

    // Hitting the floor or the ceiling should stop the player's vertical movement
    moveEvent.Velocity = onFloor || onCeiling ? new Vector2(moveEvent.Velocity.X, 0) : moveEvent.Velocity;

    // Hitting the floor should reset the number of jumps
    if (onFloor && !_onFloorLastCall)
    {
      _jumpCount = 0;
      PlayerEffectsDelegate.AnimationPlayLand();
      EmitSignal(SignalName.PlayerLand, moveEvent);
    }

    _onFloorLastCall = onFloor;

    // Gravity
    var onWall = CharacterBody.IsOnWall() && !onFloor && moveEvent.InputDirection.X != 0;
    moveEvent.Velocity += new Vector2(0, moveEvent.Gravity * (float)delta);

    // Gliding on walls
    if (onWall)
    {
      moveEvent.Velocity += new Vector2(0, moveEvent.Gravity * GlideGravityScale * (float)delta);
    }
    else
    {
      moveEvent.Velocity += new Vector2(0, moveEvent.Gravity * (float)delta);
    }

    // Jumping + walljumping
    if (Gamepad.IsJumpPressed())
    {
      EmitSignal(SignalName.PlayerJump, moveEvent);

      if (moveEvent.CanJump)
      {
        if (onFloor || _jumpCount < moveEvent.MaxJumps)
        {
          moveEvent.Velocity = new Vector2(moveEvent.Velocity.X, -moveEvent.JumpForce);
          _jumpCount++;
          JumpEffectsHandler();
        }
        else if (onWall)
        {
          EmitSignal(SignalName.PlayerWallJump, moveEvent);
          moveEvent.Velocity = new Vector2(
            -Mathf.Sign(moveEvent.Velocity.X) * moveEvent.JumpForce * 0.75f,
            -moveEvent.JumpForce
          );
          JumpEffectsHandler();
        }
      }
    }

    // Knockback
    moveEvent.Velocity += _knockback;
    _knockback *= KnockbackDecayRate * (float)delta;

    // Apply movement
    _velocity = moveEvent.Velocity;
    CharacterBody.Velocity = _velocity;
    CharacterBody.MoveAndSlide();

    WalkAnimationHandler();
  }

  public void AddKnockback(Vector2 knockback)
  {
    _knockback += knockback;
  }

  public void Reset()
  {
    _velocity = Vector2.Zero;
    _knockback = Vector2.Zero;
  }

  private void JumpEffectsHandler()
  {
    PlayerEffectsDelegate.EmitJumpParticles();
    PlayerEffectsDelegate.PlayJumpSound();
    PlayerEffectsDelegate.AnimationPlayJump();
  }

  private void LandEffectsHandler()
  {
    PlayerEffectsDelegate.AnimationPlayLand();
  }

  private void WalkAnimationHandler()
  {
    if (!_onFloorLastCall)
    {
      return;
    }

    switch (CharacterBody.Velocity.X)
    {
      case 0:
        return;
      case > 0:
        PlayerEffectsDelegate.AnimationPlayRunRight();
        break;
      default:
        PlayerEffectsDelegate.AnimationPlayRunLeft();
        break;
    }
  }
}
