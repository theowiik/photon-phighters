using Godot;

// Delegate class for the player movement
public partial class PlayerMovement : Node
{
    public CharacterBody2D CharacterBody { get; set; }
    public AnimationPlayer CharacterAnimation { get; set; }
    private Vector2 OriginalSpriteScale = new Vector2(0.131f, 0.131f);
    public int PlayerNumber { get; set; }
    public bool Freeze { get; set; }

    [GetNode("JumpPlayer")]
    private AudioStreamPlayer2D _jumpPlayer;

    // General movement
    public float Speed { get; set; } = 500.0f;
    public float FrictionAccelerate { get; set; } = 55.0f;
    public float FrictionDecelerate { get; set; } = 30.0f;
    private bool _hitTheGround = false;

    // Jumping
    public float JumpHeight { get; set; } = 100;
    private const float JumpTimeToPeak = 0.4f;
    private const float JumpTimeToDescent = 0.35f;
    private float JumpVelocity;
    private float JumpGravity;
    private float FallGravity;

    // Wall sliding
    private bool _isOnWall = false;
    private int _wallDirection = 0;
    private float _timeToWallUnstick = WallUnstickTime;
    private float WallJumpSpeed;
    private const float WallSlideSpeedMax = 150.0f;
    private const float WallStickTime = 0.25f;
    private const float WallUnstickTime = 0.15f;

    // Multiple jumping
    public int NrPossibleJumps { get; set; } = 1;
    private int _nrCurrentJumps = 0;

    public override void _Ready()
    {
        NodeAutoWire.AutoWire(this);
        UpdateMovementVars();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CharacterBody == null || CharacterAnimation == null)
            return;

        if (Freeze)
            return;

        Movement(delta);
    }

    private void Movement(double delta)
    {
        var oldVelocity = CharacterBody.Velocity;
        var nextVelocity = oldVelocity;

        bool isOnGround = CharacterBody.IsOnFloor();
        bool isOnWall = CharacterBody.IsOnWall();
        bool isRunning = Mathf.Abs(oldVelocity.X) > 0;

        var inputDir = Input.GetVector($"p{PlayerNumber}_left", $"p{PlayerNumber}_right", $"p{PlayerNumber}_up", $"p{PlayerNumber}_down");

        // Apply gravity
        if (!isOnGround)
            nextVelocity.Y += GetGravity() * (float)delta;

        // Movement modifiers
        nextVelocity = HandleMoving(delta, inputDir, nextVelocity);

        nextVelocity = HandleWallJump(delta, inputDir, nextVelocity, isOnGround);

        nextVelocity = HandleMultipleJumps(delta, inputDir, nextVelocity, isOnGround);

        // Animations
        HandleJumpStretchSquishAnimation(isOnGround, isOnWall, isRunning);

        HandleRunningAnimation(isOnGround, isRunning, inputDir);

        CharacterBody.Velocity = nextVelocity;
        CharacterBody.MoveAndSlide();
    }

    private void HandleJumpStretchSquishAnimation(bool isOnGround, bool isOnWall, bool isRunning)
    {
        // Stretch the character while in the air
        if (_hitTheGround && !isOnGround)
        {
            _hitTheGround = false;
            CharacterAnimation.Play("stretch_jump");
        }

        // Squish the character on collision with the ground
        if (!_hitTheGround && isOnGround)
        {
            _hitTheGround = true;
            CharacterAnimation.Play("squish_land");
        }

        // Squish the character on collision with a wall
        if (!isOnGround && isOnWall && isRunning)
        {
            CharacterAnimation.Play("squish_wall");
        }
    }

    private void HandleRunningAnimation(bool isOnGround, bool isRunning, Vector2 inputDir)
    {
        if (isOnGround && isRunning)
        {
            if (inputDir.X > 0)
            {
                CharacterAnimation.Play("running_right");
            }
            else
            {
                CharacterAnimation.Play("running_left");
            }
        }
    }

    // Some movement variables are based on other variables, and need to be updated
    public void UpdateMovementVars()
    {
        JumpVelocity = ((2.0f * JumpHeight) / JumpTimeToPeak) * -1.0f;
        JumpGravity = ((-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1.0f;
        FallGravity = ((-2.0f * JumpHeight) / (JumpTimeToDescent * JumpTimeToDescent)) * -1.0f;
        WallJumpSpeed = JumpVelocity;
    }

    private Vector2 HandleMoving(double delta, Vector2 inputDir, Vector2 velocity)
    {
        if (inputDir != Vector2.Zero)
        {
            velocity.X = Mathf.MoveToward(velocity.X, Speed * inputDir.X, FrictionAccelerate);
        }
        else
        {
            velocity.X = Mathf.MoveToward(velocity.X, 0, FrictionDecelerate);
        }

        return velocity;
    }

    private Vector2 HandleWallJump(double delta, Vector2 inputDir, Vector2 velocity, bool isOnGround)
    {
        // Check if the player is on a wall
        bool isOnWall = false;
        int wallDirection = 0;
        if (inputDir.X != 0.0f)
        {
            if ((CharacterBody.IsOnWall() && inputDir.X != (int)CharacterBody.GetWallNormal().X) || CharacterBody.IsOnCeiling())
            {
                isOnWall = true;
                wallDirection = inputDir.X > 0.0f ? 1 : -1;
            }
        }

        // Handle wall sliding
        bool isWallSliding = false;
        if (isOnWall && !_isOnWall && velocity.Y > 0.0f)
        {
            isWallSliding = true;
            if (velocity.Y > WallSlideSpeedMax)
            {
                velocity.Y = WallSlideSpeedMax;
            }
        }

        // Handle wall sticking
        if (isOnWall && !_isOnWall && velocity.Y <= 0.0f)
        {
            _isOnWall = true;
            _wallDirection = wallDirection;
            _timeToWallUnstick = WallStickTime;
        }

        // Handle wall jumping
        if (Input.IsActionJustPressed($"p{PlayerNumber}_jump"))
        {
            _jumpPlayer.Play();

            if (isOnGround)
            {
                velocity.Y = JumpVelocity;
            }
            else if (isWallSliding)
            {
                velocity.Y = WallJumpSpeed;
                velocity.X = _wallDirection * WallJumpSpeed;
            }
            else if (_isOnWall && _timeToWallUnstick > 0.0f && inputDir.X != _wallDirection)
            {
                velocity.Y = JumpVelocity;
                velocity.X = _wallDirection * WallJumpSpeed;
            }
        }


        // Handle wall sticking time
        if (_isOnWall && _timeToWallUnstick > 0.0f)
        {
            velocity.X = 0.0f;
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

        return velocity;
    }

    private Vector2 HandleMultipleJumps(double delta, Vector2 inputDir, Vector2 nextVelocity, bool isonGround)
    {
        if (isonGround)
        {
            _nrCurrentJumps = 0;
        }
        else if (Input.IsActionJustPressed($"p{PlayerNumber}_jump") && _nrCurrentJumps < NrPossibleJumps)
        {
            _jumpPlayer.Play();
            nextVelocity.Y = JumpVelocity;
            _nrCurrentJumps++;
        }

        return nextVelocity;
    }

    private float GetGravity()
    {
        return CharacterBody.Velocity.Y < 0.0 ? JumpGravity : FallGravity;
    }

    private float RangeLerp(float value, float istart, float istop, float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }
}
