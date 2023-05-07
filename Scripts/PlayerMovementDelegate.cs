using Godot;

namespace PhotonPhighters.Scripts;

public partial class PlayerMovementDelegate : Node
{
    private const float Gravity = 800;
    private float _glideGravityScale = 0.5f;
    private int _jumpCount;
    private bool _onFloorLastCall;
    private Vector2 _velocity;
    public float JumpForce { get; set; } = 600;
    public int MaxJumps { get; set; } = 3;
    public float Speed { get; set; } = 500;
    public CharacterBody2D CharacterBody { get; set; }
    public PlayerEffectsDelegate PlayerEffectsDelegate { get; set; }
    private Vector2 _knockback;
    private float _acceleration = 8f;
    private float _deceleration = 12f;
    private float _knockbackDecayRate = 0.9f;

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        var inputDirection = new Vector2(Input.GetActionStrength("p1_right") - Input.GetActionStrength("p1_left"), 0);

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

            if (!_onFloorLastCall) PlayerEffectsDelegate.AnimationPlayLand();
        }

        _onFloorLastCall = onFloor;

        // Gravity
        var onWall = CharacterBody.IsOnWall() && !onFloor && inputDirection.X != 0;
        _velocity.Y += Gravity * (float)delta;

        // Gliding on walls
        if (onWall)
            _velocity.Y += Gravity * _glideGravityScale * (float)delta;
        else
            _velocity.Y += Gravity * (float)delta;

        if (Input.IsActionJustPressed("p1_jump"))
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
        _knockback *= _knockbackDecayRate;

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
        if (!_onFloorLastCall) return;

        if (CharacterBody.Velocity.X == 0) return;

        if (CharacterBody.Velocity.X > 0)
            PlayerEffectsDelegate.AnimationPlayRunRight();
        else
            PlayerEffectsDelegate.AnimationPlayRunLeft();
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
}