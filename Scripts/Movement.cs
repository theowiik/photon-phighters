using Godot;

public partial class Movement : Node
{
    public CharacterBody2D CharacterBody { get; set; }
    private const float Gravity = 800;

    private float _speed = 500;
    private float _jumpForce = 600;
    private float _doubleJumpForce = 250;
    private float _wallJumpForce = 250;
    private float _glideGravityScale = 0.5f;
    private bool _onWall = false;
    private bool _onFloor = false;
    private bool _hasDoubleJumped = false;
    private Vector2 _velocity = new Vector2();

    public override void _Ready()
    {
        GD.Print("Ready");
    }

    public override void _PhysicsProcess(double delta)
    {
        GD.Print("Physics process");
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

        // Gravity
        _velocity.Y += Gravity * (float)delta;

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
            _velocity.Y = Mathf.Max(_velocity.Y, 0); // prevent wall stickiness
        }

        // Apply movement
        CharacterBody.Velocity = _velocity;
        CharacterBody.MoveAndSlide();
    }

}
