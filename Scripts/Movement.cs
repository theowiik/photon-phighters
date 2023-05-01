using Godot;

public partial class Movement : Node
{
    [Signal]
    public delegate void MovementJumpedEventHandler();

    public CharacterBody2D CharacterBody { get; set; }
    private const float Gravity = 800;
    private float _speed = 500;
    private float _jumpForce = 600;
    private float _glideGravityScale = 0.5f;
    private int _jumpCount;
    private int _maxJumps = 3;
    private Vector2 _velocity = new();

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        var inputDirection = new Vector2(Input.GetActionStrength("p1_right") - Input.GetActionStrength("p1_left"), 0);

        // Walking
        _velocity.X = inputDirection.X * _speed;

        // Jumping
        var onFloor = CharacterBody.IsOnFloor();
        if (onFloor)
        {
            _jumpCount = 0;
            _velocity.Y = 0;
        }

        // Gravity
        var _onWall = CharacterBody.IsOnWall() && !onFloor && inputDirection.X != 0;
        _velocity.Y += Gravity * (float)delta;

        // Gliding on walls
        if (_onWall)
        {
            _velocity.Y += Gravity * _glideGravityScale * (float)delta;
        }
        else
        {
            _velocity.Y += Gravity * (float)delta;
        }

        if (Input.IsActionJustPressed("p1_jump"))
        {
            if (onFloor || _jumpCount < _maxJumps)
            {
                _velocity.Y = -_jumpForce;
                _jumpCount++;
                JumpEffectsHandler();
            }
            else if (_onWall)
            {
                _velocity.Y = -_jumpForce;
                _velocity.X = -Mathf.Sign(_velocity.X) * _jumpForce * 0.75f;
                JumpEffectsHandler();
            }
        }

        // Apply movement
        CharacterBody.Velocity = _velocity;
        CharacterBody.MoveAndSlide();
    }

    private void JumpEffectsHandler()
    {
        GD.Print("doing jump effects");
    }

    private void LandEffectsHandler()
    {
        GD.Print("doing land effects");
    }
}