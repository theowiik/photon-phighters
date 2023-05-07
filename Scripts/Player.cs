using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;
public partial class Player : CharacterBody2D
{
    public delegate void PlayerEffectAdded(Node2D effect, Player who);
    public PlayerEffectAdded PlayerEffectAddedListeners;

    [Signal]
    public delegate void PlayerDiedEventHandler(Player player);

    [Export]
    public int PlayerNumber { get; set; }

    [GetNode("Movement")]
    public PlayerMovementDelegate PlayerMovementDelegate;

    [GetNode("Marker2D")]
    private Marker2D _gunMarker;

    [GetNode("Marker2D/Gun")]
    public Gun Gun { get; set; }

    [GetNode("HealthLabel")]
    private Label _healthLabel;

    [GetNode("PlayerEffectsDelegate")]
    private PlayerEffectsDelegate _playerEffectsDelegate;

    [GetNode("Sprite2D")]
    private Sprite2D _sprite2D;

    private bool _freeze;
    public bool Freeze
    {
        get => _freeze;
        set
        {
            _freeze = value;
            Gun.Freeze = _freeze;
            Health = MaxHealth;

            if (_freeze)
            {
                ProcessMode = ProcessModeEnum.Disabled;
                _sprite2D.Modulate = _seeTroughColor;
            }
            else
            {
                ProcessMode = ProcessModeEnum.Inherit;
                _sprite2D.Modulate = _nonSeeTroughColor;
            }
        }
    }

    private int _health;
    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            _healthLabel.Text = $"{_health}/{MaxHealth}";
        }
    }
    public int MaxHealth { get; set; } = 50;

    private bool _aimWithMouse = true;
    private readonly Color _seeTroughColor = new(1, 1, 1, 0.3f);
    private readonly Color _nonSeeTroughColor = new(1, 1, 1);

    public override void _Ready()
    {
        this.AutoWire();

        Health = MaxHealth;
        Gun.ShootActionName = $"p{PlayerNumber}_shoot";
        Gun.LightMode = PlayerNumber == 1 ? Light.LightMode.Light : Light.LightMode.Dark;

        _playerEffectsDelegate.PlayerSprite = _sprite2D;
        PlayerMovementDelegate.CharacterBody = this;
        PlayerMovementDelegate.PlayerEffectsDelegate = _playerEffectsDelegate;
        PlayerMovementDelegate.PlayerEffectsDelegate.PlayerEffectAddedListeners += effect => PlayerEffectAddedListeners?.Invoke(effect, this);

        // Gun
        var bulletDetectionArea = GetNode<Area2D>("BulletDetectionArea");
        bulletDetectionArea.AreaEntered += OnBulletEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Freeze)
        {
            return;
        }

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
        {
            return;
        }

        Health -= damage;
        _playerEffectsDelegate.EmitHurtParticles();
        _playerEffectsDelegate.PlayHurtSound();
        _playerEffectsDelegate.AnimationPlayHurt();

        if (Health <= 0)
        {
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        _playerEffectsDelegate.EmitDeathParticles();
        EmitSignal(SignalName.PlayerDied, this);
    }

    public void ResetHealth() => Health = MaxHealth;

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

    public enum TeamEnum
    {
        Light,
        Dark
    }

    public TeamEnum Team => PlayerNumber == 1 ? TeamEnum.Light : TeamEnum.Dark;
}
