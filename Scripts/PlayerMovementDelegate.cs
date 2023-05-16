using Godot;

namespace PhotonPhighters.Scripts;

public partial class PlayerMovementDelegate : Node
{
    private const float Gravity = 800;
    private float _acceleration = 12f;
    private float _deceleration = 12f;
    private float _glideGravityScale = 0.5f;
    private int _jumpCount;
    private Vector2 _knockback;
    private float _knockbackDecayRate = 0.04f;
    private bool _onFloorLastCall;
    private float _speed = 800;
    private Vector2 _velocity;
    public int PlayerNumber { get; set; }
    public float JumpForce { get; set; } = 700;
    public int MaxJumps { get; set; } = 2;

    public float Speed
    {
        get => _speed;
        set => _speed = Mathf.Max(100f, value);
    }

    public CharacterBody2D CharacterBody { get; set; }
    public PlayerEffectsDelegate PlayerEffectsDelegate { get; set; }

    public override void _Ready() { }

    public override void _PhysicsProcess(double delta)
    {
        var inputDirection = new Vector2(
            Input.GetActionStrength($"p{PlayerNumber}_right")
            - Input.GetActionStrength($"p{PlayerNumber}_left"),
            0
        );

        // Walking
        var targetSpeed = inputDirection.X * Speed;
        var acceleration = inputDirection.X != 0 ? _acceleration : _deceleration;
        _velocity.X = Mathf.Lerp(_velocity.X, targetSpeed, acceleration * (float)delta);

        // Jumping
        var onFloor = CharacterBody.IsOnFloor();
        if (onFloor)
        {
            _jumpCount = 0;
            _velocity.Y = 0;

            if (!_onFloorLastCall)
            {
                PlayerEffectsDelegate.AnimationPlayLand();
            }
        }

        _onFloorLastCall = onFloor;

        // Gravity
        var onWall = CharacterBody.IsOnWall() && !onFloor && inputDirection.X != 0;
        _velocity.Y += Gravity * (float)delta;

        // Gliding on walls
        if (onWall)
        {
            _velocity.Y += Gravity * _glideGravityScale * (float)delta;
        }
        else
        {
            _velocity.Y += Gravity * (float)delta;
        }

        if (Input.IsActionJustPressed($"p{PlayerNumber}_jump"))
        {
            if (onFloor || _jumpCount < MaxJumps)
            {
                _velocity.Y = -JumpForce;
                _jumpCount++;
                JumpEffectsHandler();
            }
            else if (onWall)
            {
                _velocity.Y = -JumpForce;
                _velocity.X = -Mathf.Sign(_velocity.X) * JumpForce * 0.75f;
                JumpEffectsHandler();
            }
        }

        // Knockback
        _velocity += _knockback;
        _knockback *= _knockbackDecayRate * (float)delta;

        // Apply movement
        CharacterBody.Velocity = _velocity;
        CharacterBody.MoveAndSlide();

        WalkAnimationHandler();
    }

    public void AddKnockback(Vector2 knockback)
    {
        _knockback += knockback;
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

    public void Reset()
    {
        _velocity = Vector2.Zero;
        _knockback = Vector2.Zero;
    }
}
