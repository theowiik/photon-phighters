using Godot;

// Delegate class for the player movement
public partial class PlayerMovement : Node
{
    public CharacterBody2D CharacterBody { get; set; }
    public int PlayerNumber { get; set; }
    public bool Freeze { get; set; }

    // General movement
    public float Speed { get; set; } = 500.0f;
    public float FrictionAccelerate { get; set; } = 55.0f;
    public float FrictionDecelerate { get; set; } = 30.0f;

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
        UpdateMovementVars();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CharacterBody == null)
            return;

        if (Freeze)
            return;

        Movement(delta);
    }

    private void Movement(double delta)
    {
        bool isOnGround = CharacterBody.IsOnFloor();

        var nextVelocity = CharacterBody.Velocity;

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

        CharacterBody.Velocity = nextVelocity;
        CharacterBody.MoveAndSlide();
    }

    // Some movement variables are based on other variables, and need to be updated
    public void UpdateMovementVars()
    {
        this.JumpVelocity = ((2.0f * JumpHeight) / JumpTimeToPeak) * -1.0f;
        this.JumpGravity = ((-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1.0f;
        this.FallGravity = ((-2.0f * JumpHeight) / (JumpTimeToDescent * JumpTimeToDescent)) * -1.0f;
        this.WallJumpSpeed = JumpVelocity;
    }

    public Vector2 HandleWallJump(double delta, Vector2 inputDir, Vector2 nextVelocity, bool isOnGround)
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
        else if (Input.IsActionJustPressed($"p{PlayerNumber}_jump") && _nrCurrentJumps < NrPossibleJumps)
        {
            nextVelocity.Y = JumpVelocity;
            _nrCurrentJumps++;
        }

        return nextVelocity;
    }

    private float GetGravity()
    {
        return CharacterBody.Velocity.Y < 0.0 ? JumpGravity : FallGravity;
    }
}
