using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public int PlayerNumber { get; set; }

    // General movement
    private const float Speed = 500.0f;
    private const float FrictionAccelerate = 55.0f;
    private const float FrictionDecelerate = 30.0f;

    // Jumping
    private const float JumpHeight = 100;
    private const float JumpTimeToPeak = 0.4f;
    private const float JumpTimeToDescent = 0.35f;
    private const float JumpVelocity = ((2.0f * JumpHeight) / JumpTimeToPeak) * -1.0f;
    private const float JumpGravity = ((-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1.0f;
    private const float FallGravity = ((-2.0f * JumpHeight) / (JumpTimeToDescent * JumpTimeToDescent)) * -1.0f;

    // Wall sliding
    private bool _isOnWall = false;
    private int _wallDirection = 0;
    private float _timeToWallUnstick = WallUnstickTime;
    private const float WallJumpSpeed = JumpVelocity;
    private const float WallSlideSpeedMax = 150.0f;
    private const float WallStickTime = 0.25f;
    private const float WallUnstickTime = 0.15f;

    // Multiple jumping
    private int _nrPossibleJumps = 1;
    private int _nrCurrentJumps = 0;

    // Aim
    private Marker2D _gunMarker;
    public Gun Gun { get; private set; }
    private bool _aimWithMouse = true;

    private bool _allowInputs;
    public bool AllowInputs
    {
        get => _allowInputs;
        set
        {
            _allowInputs = value;

            // Turn on gu safety when inputs are disabled
            Gun.Safety = !_allowInputs;
        }
    }

    public override void _Ready()
    {
        _gunMarker = GetNode<Marker2D>("Marker2D");
        Gun = _gunMarker.GetNode<Gun>("Gun");
        Gun.ShootActionName = $"p{PlayerNumber}_shoot";
        Gun.LightMode = PlayerNumber == 1 ? Light.LightMode.Light : Light.LightMode.Dark;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!AllowInputs)
            return;

        Movement(delta);
        Aim();
    }

    private void Movement(double delta)
    {
        bool isOnGround = IsOnFloor();

        var nextVelocity = Velocity;

        if (!isOnGround)
            nextVelocity.Y += GetGravity() * (float)delta;

        var inputDir = Input.GetVector($"p{PlayerNumber}_left", $"p{PlayerNumber}_right", $"p{PlayerNumber}_up", $"p{PlayerNumber}_down");

        if (inputDir != Vector2.Zero)
        {
            nextVelocity.X = Mathf.MoveToward(nextVelocity.X, Speed * inputDir.X, FrictionAccelerate);
        }
        else
        {
            nextVelocity.X = Mathf.MoveToward(nextVelocity.X, 0, FrictionDecelerate);
        }

        nextVelocity = HandleWallJump(delta, inputDir, nextVelocity, isOnGround);

        nextVelocity = HandleMultipleJumps(delta, inputDir, nextVelocity, isOnGround);

        Velocity = nextVelocity;
        MoveAndSlide();
    }

    public Vector2 HandleWallJump(double delta, Vector2 inputDir, Vector2 nextVelocity, bool isOnGround)
    {
        // Check if the player is on a wall
        bool isOnWall = false;
        int wallDirection = 0;
        if (inputDir.X != 0.0f)
        {
            if ((IsOnWall() && inputDir.X != (int)GetWallNormal().X) || IsOnCeiling())
            {
                isOnWall = true;
                wallDirection = inputDir.X > 0.0f ? 1 : -1;
            }
        }

        // Handle wall sliding
        bool isWallSliding = false;
        if (isOnWall && !_isOnWall && nextVelocity.Y > 0.0f)
        {
            isWallSliding = true;
            if (nextVelocity.Y > WallSlideSpeedMax)
            {
                nextVelocity.Y = WallSlideSpeedMax;
            }
        }

        // Handle wall sticking
        if (isOnWall && !_isOnWall && nextVelocity.Y <= 0.0f)
        {
            _isOnWall = true;
            _wallDirection = wallDirection;
            _timeToWallUnstick = WallStickTime;
        }

        // Handle wall jumping
        if (Input.IsActionJustPressed($"p{PlayerNumber}_jump"))
        {
            if (isOnGround)
            {
                nextVelocity.Y = JumpVelocity;
            }
            else if (isWallSliding)
            {
                nextVelocity.Y = WallJumpSpeed;
                nextVelocity.X = _wallDirection * WallJumpSpeed;
            }
            else if (_isOnWall && _timeToWallUnstick > 0.0f && inputDir.X != _wallDirection)
            {
                nextVelocity.Y = JumpVelocity;
                nextVelocity.X = _wallDirection * WallJumpSpeed;
            }
        }


        // Handle wall sticking time
        if (_isOnWall && _timeToWallUnstick > 0.0f)
        {
            nextVelocity.X = 0.0f;
            _timeToWallUnstick -= (float)delta;
            if (inputDir.X == _wallDirection * -1)
            {
                _timeToWallUnstick = WallUnstickTime;
            }
        }
        else
        {
            _isOnWall = false;
            _timeToWallUnstick = WallUnstickTime;
        }

        // Reset the wall direction if the player is no longer on a wall
        if (!isOnWall)
        {
            _wallDirection = 0;
        }

        return nextVelocity;
    }

    private Vector2 HandleMultipleJumps(double delta, Vector2 inputDir, Vector2 nextVelocity, bool isonGround)
    {
        if (isonGround)
        {
            _nrCurrentJumps = 0;
        }
        else if (Input.IsActionJustPressed($"p{PlayerNumber}_jump") && _nrCurrentJumps < _nrPossibleJumps)
        {
            nextVelocity.Y = JumpVelocity;
            _nrCurrentJumps++;
        }

        return nextVelocity;
    }

    private float GetGravity()
    {
        return Velocity.Y < 0.0 ? JumpGravity : FallGravity;
    }
    private void Aim()
    {
        var joystickDeadzone = 0.05f;
        var joystickVector = new Vector2(Input.GetJoyAxis(PlayerNumber - 1, JoyAxis.RightX), Input.GetJoyAxis(PlayerNumber - 1, JoyAxis.RightY));

        // Controller has priority over mouse.
        if (joystickVector.Length() > joystickDeadzone)
        {
            _gunMarker.Rotation = joystickVector.Angle();
            _aimWithMouse = false;
        }

        // Only player one can play with mouse and keyboard.
        if (PlayerNumber == 1 && _aimWithMouse)
        {
            var direction = GetGlobalMousePosition() - GlobalPosition;
            _gunMarker.Rotation = direction.Angle();
        }
    }
}
