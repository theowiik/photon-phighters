using Godot;
using System;

public partial class Gun : Node2D
{
    // PROPERTIES
    private double BulletSpeed { get; set; } = 750.0f;
    private double FireRate { get; set; } = 5f;
    private float BulletSizeFactor { get; set; } = 1.0f;
    private float BulletCount { get; set; } = 1.0f;
    private float BulletSpread { get; set; } = 0.2f;

    private PackedScene _bulletScene;
    private Timer _shootTimer;
    private Boolean _canShoot = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _bulletScene = GD.Load<PackedScene>("res://Objects/Bullet.tscn");
        _shootTimer = GetNode<Timer>("Timer");
        _shootTimer.Timeout += () => _canShoot = !_canShoot;
        _shootTimer.WaitTime = 1 / FireRate;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("shoot"))
        {
            if (_canShoot)
                Shoot();
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

            EmitSignal(SignalName.ShootDelegate, bullet);
        }

        _canShoot = false;
        _shootTimer.Start();
    }
}
