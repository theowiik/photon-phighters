using Godot;

namespace PhotonPhighters.Scripts;

public partial class PlayerMovementDelegate : Node
{
  /// <summary>
  ///   The speed of which the player start taking damage from aerodynamic heating
  /// </summary>

  [Signal]
  public delegate void PlayerMoveEventHandler(MovementEvents.PlayerMovementEvent playerMoveEvent);

  [Signal]
  public delegate void PlayerJumpEventHandler(MovementEvents.PlayerMovementEvent playerMoveEvent);

  [Signal]
  public delegate void PlayerLandEventHandler(MovementEvents.PlayerMovementEvent playerMoveEvent);

  [Signal]
  public delegate void PlayerWallJumpEventHandler(MovementEvents.PlayerMovementEvent playerMoveEvent);

  [Signal]
  public delegate void PlayerStoppedEventHandler(MovementEvents.PlayerMovementEvent playerMoveEvent);

  private const int AerodynamicHeatingVelocity = 10_000;
  private const float Gravity = 800;
  private const float Deceleration = 12f;
  private const float GlideGravityScale = 0.5f;
  private const float KnockbackDecayRate = 0.04f;

  private readonly float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
  private int _jumpCount;
  private Vector2 _knockback;
  private bool _onFloorLastCall;
  private float _speed = 300;
  private Vector2 _velocity;
  private bool _canJump = true;
  private bool _canMove = true;
  public float Acceleration { get; set; } = 12f;
  public CharacterBody2D CharacterBody { get; set; }
  public bool HasReachedAerodynamicHeatingVelocity => _velocity.Length() > AerodynamicHeatingVelocity;
  public float JumpForce { get; set; } = 500;
  public int MaxJumps { get; set; } = 2;
  public PlayerEffectsDelegate PlayerEffectsDelegate { get; set; }
  public int PlayerNumber { get; set; }

  public float Speed
  {
    get => _speed;
    set => _speed = Mathf.Max(100f, value);
  }

  public override void _PhysicsProcess(double delta)
  {
    var inputDirection = new Vector2(
      Input.GetActionStrength($"p{PlayerNumber}_right") - Input.GetActionStrength($"p{PlayerNumber}_left"),
      0
    );

    var moveEvent = new MovementEvents.PlayerMovementEvent(
      Gravity,
      _speed,
      _velocity,
      inputDirection,
      _canMove,
      CharacterBody,
      _canJump,
      JumpForce,
      MaxJumps
    );
    EmitSignal(SignalName.PlayerMove, moveEvent);

    if (!moveEvent.CanMove)
      moveEvent.InputDirection = new Vector2(0, 0);

    // Stopped
    if (
      Godot.Vector2.Equals(moveEvent.InputDirection, new Vector2(0, 0))
      && Godot.Vector2.Equals(moveEvent.Velocity, new Vector2(0, 0))
    )
    {
      EmitSignal(SignalName.PlayerStopped, moveEvent);
    }

    // Walking
    var targetSpeed = moveEvent.InputDirection.X * moveEvent.Speed;
    var acceleration = moveEvent.InputDirection.X != 0 ? Acceleration : Deceleration;
    moveEvent.Velocity.X = Mathf.Lerp(moveEvent.Velocity.X, targetSpeed, acceleration * (float)delta);

    // Jumping
    var onFloor = CharacterBody.IsOnFloor();
    if (onFloor)
    {
      _jumpCount = 0;
      moveEvent.Velocity.Y = 0;

      if (!_onFloorLastCall)
      {
        PlayerEffectsDelegate.AnimationPlayLand();
        EmitSignal(SignalName.PlayerLand, moveEvent);
      }
    }

    _onFloorLastCall = onFloor;

    // Gravity
    var onWall = CharacterBody.IsOnWall() && !onFloor && moveEvent.InputDirection.X != 0;
    moveEvent.Velocity.Y += moveEvent.Gravity * (float)delta;

    // Gliding on walls
    if (onWall)
    {
      moveEvent.Velocity.Y += moveEvent.Gravity * GlideGravityScale * (float)delta;
    }
    else
    {
      moveEvent.Velocity.Y += moveEvent.Gravity * (float)delta;
    }

    if (Input.IsActionJustPressed($"p{PlayerNumber}_jump"))
    {
      EmitSignal(SignalName.PlayerJump, moveEvent);
      if (moveEvent.CanJump)
      {
        if (onFloor || _jumpCount < moveEvent.MaxJumps)
        {
          moveEvent.Velocity.Y = -moveEvent.JumpForce;
          _jumpCount++;
          JumpEffectsHandler();
        }
        else if (onWall)
        {
          EmitSignal(SignalName.PlayerWallJump, moveEvent);
          moveEvent.Velocity.Y = -moveEvent.JumpForce;
          moveEvent.Velocity.X = -Mathf.Sign(moveEvent.Velocity.X) * moveEvent.JumpForce * 0.75f;
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

    if (CharacterBody.Velocity.X == 0)
    {
      return;
    }

    if (CharacterBody.Velocity.X > 0)
    {
      PlayerEffectsDelegate.AnimationPlayRunRight();
    }
    else
    {
      PlayerEffectsDelegate.AnimationPlayRunLeft();
    }
  }
}
