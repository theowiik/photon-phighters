using Godot;

public partial class Player : CharacterBody2D
{
    [Signal]
    public delegate void PlayerDiedEventHandler(Player player);

    [Export]
    public int PlayerNumber { get; set; }

    [GetNode("PlayerMovement")]
    public PlayerMovement PlayerMovementDelegate;

    [GetNode("DeathPlayer")]
    private AudioStreamPlayer2D _deathPlayer;

    [GetNode("HurtPlayer")]
    private AudioStreamPlayer2D _hurtPlayer;

    [GetNode("FallDeathPlayer")]
    private AudioStreamPlayer2D _fallDeathPlayer;

    [GetNode("Marker2D")]
    private Marker2D _gunMarker;

    [GetNode("Marker2D/Gun")]
    public Gun Gun { get; set; }

    [GetNode("Sprite2D/ExplosionParticle")]
    private CpuParticles2D _explosionParticleEmitter;

    [GetNode("Sprite2D/JumpParticles")]
    private CpuParticles2D _jumpParticleEmitter;

    private bool _freeze;
    public bool Freeze
    {
        get => _freeze;
        set
        {
            _freeze = value;
            Gun.Freeze = _freeze;
            PlayerMovementDelegate.Freeze = _freeze;
            _health = MaxHealth;
        }
    }

    public int MaxHealth { get; set; } = 50;
    private int _health;
    private bool _aimWithMouse = true;

    public override void _Ready()
    {
        NodeAutoWire.AutoWire(this);

        PlayerMovementDelegate = GetNode<PlayerMovement>("PlayerMovement");

        _health = MaxHealth;
        Gun.ShootActionName = $"p{PlayerNumber}_shoot";
        Gun.LightMode = PlayerNumber == 1 ? Light.LightMode.Light : Light.LightMode.Dark;

        // Movement
        PlayerMovementDelegate.PlayerNumber = PlayerNumber;
        PlayerMovementDelegate.CharacterBody = this;
        PlayerMovementDelegate.CharacterAnimation = GetNode<AnimationPlayer>("AnimationPlayer");

        // Gun
        var bulletDetectionArea = GetNode<Area2D>("BulletDetectionArea");
        bulletDetectionArea.AreaEntered += OnBulletEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Freeze)
            return;

        Aim();
    }

    private void OnBulletEntered(Area2D area)
    {
        if (area is Bullet bullet)
        {
            TakeDamage(bullet.Damage);
            bullet.QueueFree();
        }
    }

    public void TakeDamage(int damage)
    {
        if (Freeze)
            return;

        _health -= damage;
        _explosionParticleEmitter.Emitting = true;
        _hurtPlayer.Play();

        if (_health <= 0)
        {
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        _explosionParticleEmitter.Emitting = true;
        EmitSignal(SignalName.PlayerDied, this);
    }

    public void ResetHealth()
    {
        _health = MaxHealth;
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
