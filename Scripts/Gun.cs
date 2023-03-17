using Godot;
using System;

public partial class Gun : Node2D
{
    private PackedScene _bulletScene;
    private Timer _shootTimer;
    private const double _bulletSpeed = 500.0f;
    private const double _shootCooldown = 0.2f;
    private Boolean _canShoot = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _bulletScene = GD.Load<PackedScene>("res://Objects/Bullet.tscn");
        // _shootTimer = GetNode<Timer>("Timer");
        // _shootTimer.Timeout += () => _canShoot = !_canShoot;
        // _shootTimer.WaitTime = _shootCooldown;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("shoot"))
        {
            if (_canShoot)
                Shoot();
            // _shootTimer.Start();
        }
    }

    private void Shoot()
    {
        var bullet = _bulletScene.Instantiate<Bullet>();
        bullet.GlobalPosition = GlobalPosition;
        bullet.Rotation = GetParent<Marker2D>().Rotation;
        bullet.Speed = _bulletSpeed;
        GetParent().GetParent().GetParent().AddChild(bullet);
        // _canShoot = false;
    }
}
