using Godot;

public partial class Player : CharacterBody2D
{
    // General movement
    private float Speed { get; set; } = 500.0f;
    private float FrictionAccelerate { get; set; } = 55.0f;
    private float FrictionDecelerate { get; set; } = 30.0f;

    // Jumping
    private float JumpHeight { get; set; } = 100.0f;
    private const float JumpTimeToPeak = 0.4f;
    private const float JumpTimeToDescent = 0.35f;
    private float JumpVelocity { get; set; }
    private float JumpGravity { get; set; }
    private float FallGravity { get; set; }

    // Wall sliding
    private bool _isOnWall = false;
    private int _wallDirection = 0;
    private float _timeToWallUnstick = WallUnstickTime;
    private float WallJumpSpeed;
    private const float WallSlideSpeedMax = 150.0f;
    private const float WallStickTime = 0.25f;
    private const float WallUnstickTime = 0.15f;

    // Multiple jumping
    private int nrPossibleJumps { get; set; } = 1;
    private int _nrCurrentJumps = 0;

    // Weapons
    private Marker2D _gunMarker;
    public Gun Gun { get; private set; }

    // Power up
    private PowerUpManager powerUpManager = new PowerUpManager();

    public override void _Ready()
    {
        _gunMarker = GetNode<Marker2D>("Marker2D");
        Gun = _gunMarker.GetNode<Gun>("Gun");
        InitializeMovementVariables();

        var btnSpeed = GetNode<Button>("/root/World/Control/SpeedUp");
        btnSpeed.FocusMode = Control.FocusModeEnum.None;       // Måste sätta denna till None annars kommer knappen tryckas automatiskt när man trycker space
        btnSpeed.Pressed += () => powerUpManager.IncreaseSpeed(this);

        var btnJumpTimes = GetNode<Button>("/root/World/Control/JumpTimes");
        btnJumpTimes.FocusMode = Control.FocusModeEnum.None;
        btnJumpTimes.Pressed += () => powerUpManager.IncreaseMultipleJump(this);

        var btnJumpVelocity = GetNode<Button>("/root/World/Control/JumpVelocity");
        btnJumpVelocity.FocusMode = Control.FocusModeEnum.None;
        btnJumpVelocity.Pressed += () => powerUpManager.IncreaseJumpVelocity(this);
    }

    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);
        Aim(delta);
    }

    private void Movement(double delta)
    {
        bool isOnGround = IsOnFloor();

        var nextVelocity = Velocity;

        if (!isOnGround)
            nextVelocity.Y += GetGravity() * (float)delta;

        var inputDir = Input.GetVector("left", "right", "up", "down");

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

    private Vector2 HandleMultipleJumps(double delta, Vector2 inputDir, Vector2 nextVelocity, bool isonGround)
    {
        if (isonGround)
        {
            _nrCurrentJumps = 0;
        }
        else if (Input.IsActionJustPressed("jump") && _nrCurrentJumps < nrPossibleJumps)
        {
            nextVelocity.Y = JumpVelocity;
            _nrCurrentJumps++;
        }

        return nextVelocity;
    }

    private void InitializeMovementVariables()
    {
        this.JumpVelocity = ((2.0f * JumpHeight) / JumpTimeToPeak) * -1.0f;
        this.JumpGravity = ((-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1.0f;
        this.FallGravity = ((-2.0f * JumpHeight) / (JumpTimeToDescent * JumpTimeToDescent)) * -1.0f;
        this.WallJumpSpeed = JumpVelocity;

    }

    private float GetGravity()
    {
        return Velocity.Y < 0.0 ? JumpGravity : FallGravity;
    }
    private void Aim(double delta)
    {
        var direction = GetGlobalMousePosition() - GlobalPosition;
        _gunMarker.Rotation = direction.Angle();
    }

    public class PowerUpManager
    {
        public void IncreaseSpeed(Player player)
        {
            player.Speed += 100;
        }

        public void InstantTurn(Player player)
        {
            player.FrictionAccelerate = player.Speed;
            player.FrictionDecelerate = player.Speed;
        }

        public void IncreaseMultipleJump(Player player)
        {
            player.nrPossibleJumps += 1;
        }

        public void DecreaseGravity(Player player)
        {
            player.JumpGravity -= 100.0f;
            player.FallGravity -= 100.0f;
        }

        public void IncreaseJumpVelocity(Player player)
        {
            player.JumpVelocity -= 100.0f;
        }
    }
}
