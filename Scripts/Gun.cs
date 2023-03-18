using Godot;
using System;

public partial class Gun : Node2D
{
    private PackedScene _bulletScene;
    private Timer _shootTimer;
    private const double BulletSpeed = 500.0f;
    private const double ShootCooldown = 0.2f;
    private Light.LightMode LightMode { get; set; }
    private bool _canShoot = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _bulletScene = GD.Load<PackedScene>("res://Objects/Bullet.tscn");
        _shootTimer = GetNode<Timer>("Timer");
        _shootTimer.Timeout += () => _canShoot = !_canShoot;
        _shootTimer.WaitTime = ShootCooldown;
        LightMode = Light.LightMode.Light;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("shoot"))
        {
            if (_canShoot)
                Shoot();
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_right"))
        {
            if (LightMode == Light.LightMode.Light)
                LightMode = Light.LightMode.Dark;
            else
                LightMode = Light.LightMode.Light;
        }
    }

    [Signal]
    public delegate void ShootDelegateEventHandler(Node2D bullet);

    private void Shoot()
    {
        var bullet = _bulletScene.Instantiate<Bullet>();
        bullet.GlobalPosition = GlobalPosition;
        bullet.Rotation = GetParent<Marker2D>().Rotation;
        bullet.Speed = BulletSpeed;
        bullet.LightMode = LightMode;
        EmitSignal(SignalName.ShootDelegate, bullet);
        _canShoot = false;
        _shootTimer.Start();
    }
}
