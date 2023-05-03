using Godot;

public partial class Bullet : Area2D
{
    public double Speed { get; set; }
    public float GravityFactor { get; set; } = 1.0f;
    public int Damage { get; set; } = 10;
    public Light.LightMode LightMode { get; set; } = Light.LightMode.Dark;
    private Vector2 _velocity;
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        _velocity = Vector2.FromAngle(Rotation) * (float)Speed;
        var lifeTimeTimer = GetNode<Timer>("Timer");
        lifeTimeTimer.Timeout += OnTimerTimeout;
        lifeTimeTimer.Start(5);
        AreaEntered += OnAreaEntered;

        var sprite = GetNode<Sprite2D>("Sprite2D");
        if (LightMode == Light.LightMode.Dark)
        {
            sprite.Modulate = new Color(0, 0, 0, 1);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        _velocity.Y += _gravity * GravityFactor * (float)delta;
        Translate(_velocity * (float)delta);
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
