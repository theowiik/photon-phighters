using Godot;

public partial class Player : CharacterBody2D
{
    // General movement
    [Export]
    private const float Speed = 500.0f;
    [Export]
    private const float FrictionAccelerate = 50.0f;
    [Export]
    private const float FrictionDecelerate = 30.0f;
    
    // Jumping
    [Export]
    private const float JumpHeight = 100;
    [Export]
    private const float JumpTimeToPeak = 0.4f;
    [Export]
    private const float JumpTimeToDescent = 0.35f;
    private const float JumpVelocity = ((2.0f * JumpHeight) / JumpTimeToPeak) * -1.0f;
    private const float JumpGravity = ((-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1.0f;
    private const float FallGravity = ((-2.0f * JumpHeight) / (JumpTimeToDescent * JumpTimeToDescent)) * -1.0f;

    // Wall sliding
    private bool _isOnWall = false;
    private int _wallDirection = 0;
    private float _timeToWallUnstick = WallUnstickTime;
    private const float WallJumpSpeed = -1000.0f;
    private const float WallSlideSpeedMax = 150.0f;
    private const float WallStickTime = 0.25f;
    private const float WallUnstickTime = 0.15f;

    // Double jumping
    private bool _hasDoubleJumped = false;

    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);
    }

    private void Movement(double delta)
    {
        bool isOnGround = IsOnFloor();

        var nextVelocity = Velocity;

        if (!isOnGround)
            nextVelocity.Y += GetGravity() * (float)delta;

        var inputDir = Input.GetVector("left", "right", "up", "down");
        if (Input.IsActionJustPressed("jump")) {
            nextVelocity.Y = JumpVelocity;
        }
        if (inputDir != Vector2.Zero)
        {
            var dir = inputDir.X > 0.0f ? 1 : -1;
            nextVelocity.X = Mathf.MoveToward(Velocity.X, dir * Speed, FrictionAccelerate);
        }
        else
        {
            nextVelocity.X = Mathf.MoveToward(Velocity.X, 0, FrictionDecelerate);
        }

        nextVelocity = HandleWalljump(delta, inputDir, nextVelocity, isOnGround);

        nextVelocity = HandleDoubleJump(delta, inputDir, nextVelocity, isOnGround);

        Velocity = nextVelocity;
        MoveAndSlide();
    }

    public Vector2 HandleWalljump(double delta, Vector2 inputDir, Vector2 nextVelocity, bool isOnGround)
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
        if (Input.IsActionJustPressed("jump"))
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

    private Vector2 HandleDoubleJump(double delta, Vector2 inputDir, Vector2 nextVelocity, bool isonGround)
    {
        if (isonGround)
        {
            _hasDoubleJumped = false;
        }
        else if (Input.IsActionJustPressed("jump") && !_hasDoubleJumped)
        {
            nextVelocity.Y = JumpVelocity;
            _hasDoubleJumped = true;
        }

        return nextVelocity;
    }

    private float GetGravity() {
        return Velocity.Y < 0.0 ? JumpGravity : FallGravity;
    }
}
