using Godot;

namespace PhotonPhighters.Scripts;

public partial class PlayerMovementDelegate : Node
{
  /// <summary>
  ///   The speed of which the player start taking damage from aerodynamic heating
  /// </summary>

  [Signal]
  public delegate void PlayerMoveEventHandler(Events.PlayerMovementEvent playerMoveEvent);

  [Signal]
  public delegate void PlayerJumpEventHandler(Events.PlayerMovementEvent playerMoveEvent);

  [Signal]
  public delegate void PlayerLandEventHandler(Events.PlayerMovementEvent playerMoveEvent);

  [Signal]
  public delegate void PlayerWallJumpEventHandler(Events.PlayerMovementEvent playerMoveEvent);

  [Signal]
  public delegate void PlayerStoppedEventHandler(Events.PlayerMovementEvent playerMoveEvent);

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

    var moveEvent = new Events.PlayerMovementEvent(
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

    if (!moveEvent._canMove)
    {
      moveEvent._inputDirection = new Vector2(0, 0);
    }

    // Stopped
    if (
      Godot.Vector2.Equals(moveEvent._inputDirection, new Vector2(0, 0))
      && Godot.Vector2.Equals(moveEvent._velocity, new Vector2(0, 0))
    )
    {
      EmitSignal(SignalName.PlayerStopped, moveEvent);
    }

    // Walking
    var targetSpeed = moveEvent._inputDirection.X * moveEvent._speed;
    var acceleration = moveEvent._inputDirection.X != 0 ? Acceleration : Deceleration;
    moveEvent._velocity.X = Mathf.Lerp(moveEvent._velocity.X, targetSpeed, acceleration * (float)delta);

    // Jumping
    var onFloor = CharacterBody.IsOnFloor();
    if (onFloor)
    {
      _jumpCount = 0;
      moveEvent._velocity.Y = 0;

      if (!_onFloorLastCall)
      {
        PlayerEffectsDelegate.AnimationPlayLand();
        EmitSignal(SignalName.PlayerLand, moveEvent);
      }
    }

    _onFloorLastCall = onFloor;

    // Gravity
    var onWall = CharacterBody.IsOnWall() && !onFloor && moveEvent._inputDirection.X != 0;
    moveEvent._velocity.Y += moveEvent._gravity * (float)delta;

    // Gliding on walls
    if (onWall)
    {
      moveEvent._velocity.Y += moveEvent._gravity * GlideGravityScale * (float)delta;
    }
    else
    {
      moveEvent._velocity.Y += moveEvent._gravity * (float)delta;
    }

    if (Input.IsActionJustPressed($"p{PlayerNumber}_jump"))
    {
      EmitSignal(SignalName.PlayerJump, moveEvent);
      if (moveEvent._canJump)
      {
        if (onFloor || _jumpCount < moveEvent._maxJumps)
        {
          moveEvent._velocity.Y = -moveEvent._jumpForce;
          _jumpCount++;
          JumpEffectsHandler();
        }
        else if (onWall)
        {
          EmitSignal(SignalName.PlayerWallJump, moveEvent);
          moveEvent._velocity.Y = -moveEvent._jumpForce;
          moveEvent._velocity.X = -Mathf.Sign(moveEvent._velocity.X) * moveEvent._jumpForce * 0.75f;
          JumpEffectsHandler();
        }
      }
    }

    // Knockback
    moveEvent._velocity += _knockback;
    _knockback *= KnockbackDecayRate * (float)delta;

    // Apply movement
    _velocity = moveEvent._velocity;
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
