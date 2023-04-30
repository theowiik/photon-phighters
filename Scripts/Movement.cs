using Godot;

public partial class Movement : Node
{
    [Signal]
    public delegate void PlayerJumpedEventHandler();

    public CharacterBody2D CharacterBody { get; set; }
    private const float Gravity = 800;
    private float _speed = 500;
    private float _jumpForce = 600;
    private float _glideGravityScale = 0.5f;
    private bool _onWall = false;
    private bool _onFloor = false;
    private int _jumpCount = 0;
    private int _maxJumps = 3;
    private Vector2 _velocity = new Vector2();

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
            _jumpCount = 0;
            _velocity.Y = 0;
        }

        // Gravity
        _velocity.Y += Gravity * (float)delta;

        if (Input.IsActionJustPressed("p1_jump"))
        {
            if (_onFloor || _jumpCount < _maxJumps)
            {
                _velocity.Y = -_jumpForce;
                _jumpCount++;
            }
            else if (_onWall)
            {
                _velocity.Y = -_jumpForce;
                _velocity.X = -Mathf.Sign(_velocity.X) * _jumpForce * 0.75f;
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