using Godot;
using System;

public partial class Bullet : Node2D
{
    private Timer _timer;
    public double Speed { get; set; }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
        _timer.Start(2);
    }

    public override void _PhysicsProcess(double delta)
    {
        Translate(Vector2.FromAngle(Rotation) * (float)Speed * (float)delta);
    }

    private void OnTimerTimeout()
    {
        QueueFree();
    }
}
