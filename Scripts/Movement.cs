using Godot;

public partial class Movement : Node
{
    [Export] public float Speed = 200;
    [Export] public float JumpForce = 300;
    [Export] public float DoubleJumpForce = 250;
    [Export] public float WallJumpForce = 250;
    [Export] public float GlideGravityScale = 0.5f;

    public CharacterBody2D CharacterBody { get; set; }

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
        _velocity.X = inputDirection.X * Speed;

        // Jumping
        _onFloor = CharacterBody.IsOnFloor();
        if (_onFloor)
        {
            _hasDoubleJumped = false;
        }

        if (Input.IsActionJustPressed("p1_jump"))
        {
            if (_onFloor)
            {
                _velocity.Y = -JumpForce;
            }
            else if (!_hasDoubleJumped)
            {
                _velocity.Y = -DoubleJumpForce;
                _hasDoubleJumped = true;
            }
            else if (_onWall)
            {
                _velocity.Y = -WallJumpForce;
                _velocity.X = -Mathf.Sign(_velocity.X) * WallJumpForce * 0.75f;
            }
        }

        // Gliding on walls
        _onWall = CharacterBody.IsOnWall() && !_onFloor && inputDirection.X != 0;
        if (_onWall)
        {
            CharacterBody.GravityScale = GlideGravityScale;
        }
        else
        {
            CharacterBody.GravityScale = 1;
        }

        // Apply movement
        CharacterBody.Velocity = _velocity;
        CharacterBody.MoveAndSlide();
    }
}
