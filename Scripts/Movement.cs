using Godot;

public partial class Movement : Node
{
    public CharacterBody2D CharacterBody { get; set; }

    private float _speed = 400;
    private float _jumpForce = 600;
    private float _doubleJumpForce = 250;
    private float _wallJumpForce = 250;
    private float _glideGravityScale = 0.5f;
    private float _gravity = 980f;

    private Vector2 _velocity = new();
    private bool _onWall;
    private bool _onFloor;
    private bool _hasDoubleJumped;

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        var inputDirection = new Vector2(Input.GetActionStrength("p1_right") - Input.GetActionStrength("p1_left"), 0);

        // Walking
        _velocity.X = inputDirection.X * _speed;

        // Jumping
        _onFloor = CharacterBody.IsOnFloor();
        if (_onFloor)
        {
            _hasDoubleJumped = false;
            _velocity.Y = 0;
        }

        if (Input.IsActionJustPressed("p1_jump"))
        {
            if (_onFloor)
            {
                _velocity.Y = -_jumpForce;
            }
            else if (!_hasDoubleJumped)
            {
                _velocity.Y = -_doubleJumpForce;
                _hasDoubleJumped = true;
            }
            else if (_onWall)
            {
                _velocity.Y = -_wallJumpForce;
                _velocity.X = -Mathf.Sign(_velocity.X) * _wallJumpForce * 0.75f;
            }
        }

        // Gliding on walls
        _onWall = CharacterBody.IsOnWall() && !_onFloor && inputDirection.X != 0;
        if (_onWall)
        {
            _velocity.Y += _gravity * _glideGravityScale * (float)delta;
        }
        else
        {
            _velocity.Y += _gravity * (float)delta;
        }

        // Apply movement
        CharacterBody.Velocity = _velocity;
        CharacterBody.MoveAndSlide();
    }
}
