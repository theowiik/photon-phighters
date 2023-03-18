using Godot;

public partial class Bullet : Area2D
{
    private Timer _timer;
    public int Damage { get; private set; } = 10;
    public double Speed { get; set; }
    public Light.LightMode LightMode { get; set; } = Light.LightMode.Dark;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
        _timer.Start(2);
        AreaEntered += OnAreaEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        Translate(Vector2.FromAngle(Rotation) * (float)Speed * (float)delta);
    }

    private void OnTimerTimeout()
    {
        QueueFree();
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area.IsInGroup("lights") && area is Light light)
        {
            light.SetLight(LightMode);
            QueueFree();
        }
    }
}
