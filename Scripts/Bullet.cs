using Godot;
using System;

public partial class Bullet : Node2D
{
    // PROPERTIES
    public double Speed { get; set; }
    private float GravityFactor { get; set; } = 1.0f;
    private float Damage { get; set; } = 1.0f;

    private Timer _timer;
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private Vector2 _velocity;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _velocity = Vector2.FromAngle(Rotation) * (float)Speed;
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
        _timer.Start(5);
    }

    public override void _PhysicsProcess(double delta)
    {
        _velocity.Y += _gravity * (float)delta;
        Translate(_velocity * (float)delta);
    }

    private void OnTimerTimeout()
    {
        QueueFree();
    }
}
