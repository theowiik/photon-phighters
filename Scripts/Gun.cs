using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;
public partial class Gun : Node2D
{
    private readonly PackedScene _bulletScene = GD.Load<PackedScene>("res://Objects/Player/Bullet.tscn");

    [GetNode("ShootPlayer")]
    private AudioStreamPlayer2D _shootPlayer;

    [GetNode("Timer")]
    private Timer _shootTimer;

    public Light.LightMode LightMode { get; set; }
    public string ShootActionName { get; set; }
    public double BulletSpeed { get; set; } = 750.0f;
    public double FireRate { get; set; } = 5f;
    public float BulletSizeFactor { get; set; } = 1.0f;
    public int BulletCount { get; set; } = 2;
    public float BulletSpread { get; set; } = 0.2f;
    public float BulletGravity { get; set; } = 1.0f;
    public int BulletDamage { get; set; } = 10;
    private bool _loading = true;
    public bool Freeze { get; set; }

    public override void _Ready()
    {
        this.AutoWire();
        _shootTimer.Timeout += () => _loading = !_loading;
        LightMode = Light.LightMode.Light;
        _shootTimer.WaitTime = 1 / FireRate;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Freeze)
        {
            return;
        }

        if (Input.IsActionPressed(ShootActionName) && _loading)
        {
            Shoot();
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_right"))
        {
            LightMode = LightMode == Light.LightMode.Light ? Light.LightMode.Dark : Light.LightMode.Light;
        }
    }

    [Signal]
    public delegate void ShootDelegateEventHandler(Node2D bullet);

    private void Shoot()
    {
        _shootPlayer.PitchScale = GetLightPitch();
        _shootPlayer.Play();

        for (var i = 0; i < BulletCount; i++)
        {
            var bullet = _bulletScene.Instantiate<Bullet>();
            var shotSpread = BulletCount == 1 ? 0 : (float)GD.RandRange(-BulletSpread, BulletSpread);

            bullet.GlobalPosition = GlobalPosition;
            bullet.Rotation = GetParent<Marker2D>().Rotation + shotSpread;
            bullet.Speed = GetRandomBetweenRange((float)BulletSpeed * 0.9f, (float)BulletSpeed * 1.1f);
            bullet.Scale *= BulletSizeFactor;
            bullet.GravityFactor = BulletGravity;
            bullet.Damage = BulletDamage;
            bullet.LightMode = LightMode;

            EmitSignal(SignalName.ShootDelegate, bullet);
        }

        _loading = false;
        _shootTimer.Start();
    }

    private static float GetRandomBetweenRange(float min, float max) => (float)GD.RandRange(min, max);

    private float GetLightPitch() => LightMode == Light.LightMode.Light ? (float)GD.RandRange(1.5f, 1.8f) : (float)GD.RandRange(0.7f, 0.9f);
}
