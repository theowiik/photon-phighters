using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public int PlayerNumber { get; set; }
    private PlayerMovement _playerMovement;
    private bool _freeze;
    public bool Freeze
    {
        get => _freeze;
        set
        {
            _freeze = value;

            // Turn on gun safety when inputs are disabled
            Gun.Safety = _freeze;

            // Turn off movement when inputs are disabled
            _playerMovement.Freeze = _freeze;
        }
    }

    // Health
    public const int MaxHealth = 100;
    private int _health = MaxHealth;

    // Aim
    private Marker2D _gunMarker;
    public Gun Gun { get; private set; }
    private bool _aimWithMouse = true;

    public override void _Ready()
    {
        _gunMarker = GetNode<Marker2D>("Marker2D");
        Gun = _gunMarker.GetNode<Gun>("Gun");
        Gun.ShootActionName = $"p{PlayerNumber}_shoot";
        Gun.LightMode = PlayerNumber == 1 ? Light.LightMode.Light : Light.LightMode.Dark;

        _playerMovement = GetNode<PlayerMovement>("PlayerMovement");
        _playerMovement.PlayerNumber = PlayerNumber;
        _playerMovement.CharacterBody = this;

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

        if (_health <= 0)
        {
            // Dead
            QueueFree();
        }
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
