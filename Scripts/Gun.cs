using Godot;
using System;

public partial class Gun : Node2D
{
    public Light.LightMode LightMode { get; set; }
    public string ShootActionName { get; set; }
    private double BulletSpeed { get; set; } = 750.0f;
    private double FireRate { get; set; } = 5f;
    private float BulletSizeFactor { get; set; } = 1.0f;
    private float BulletCount { get; set; } = 1.0f;
    private float BulletSpread { get; set; } = 0.2f;
    private PackedScene _bulletScene;
    private Timer _shootTimer;
    private bool _loading = true;
    public bool Safety { get; set; }

    public override void _Ready()
    {
        _bulletScene = GD.Load<PackedScene>("res://Objects/Bullet.tscn");
        _shootTimer = GetNode<Timer>("Timer");
        _shootTimer.Timeout += () => _loading = !_loading;
        LightMode = Light.LightMode.Light;
        _shootTimer.WaitTime = 1 / FireRate;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Safety)
            return;

        if (Input.IsActionPressed(ShootActionName) && _loading)
            Shoot();
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
        for (int i = 0; i < BulletCount; i++)
        {
            var bullet = _bulletScene.Instantiate<Bullet>();
            var shotSpread = BulletCount == 1 ? 0 : (float)GD.RandRange(-BulletSpread, BulletSpread);

            bullet.GlobalPosition = GlobalPosition;
            bullet.Rotation = GetParent<Marker2D>().Rotation + shotSpread;
            bullet.Speed = BulletSpeed;
            bullet.Scale *= BulletSizeFactor;
            bullet.LightMode = LightMode;

            EmitSignal(SignalName.ShootDelegate, bullet);
        }

        _loading = false;
        _shootTimer.Start();
    }
}
