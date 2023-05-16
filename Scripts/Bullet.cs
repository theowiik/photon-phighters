using Godot;

namespace PhotonPhighters.Scripts;

public partial class Bullet : Area2D
{
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private Vector2 _velocity;
    public float Speed { get; set; }
    public float GravityFactor { get; set; } = 1.0f;
    public int Damage { get; set; } = 10;
    public Light.LightMode LightMode { get; set; } = Light.LightMode.Dark;

    public override void _Ready()
    {
        _velocity = Vector2.FromAngle(Rotation) * Speed;
        var lifeTimeTimer = GetNode<Timer>("Timer");
        lifeTimeTimer.Timeout += OnTimerTimeout;
        lifeTimeTimer.Start(5);
        AreaEntered += OnAreaEntered;
        BodyEntered += OnBodyEntered;

        var sprite = GetNode<Sprite2D>("Sprite2D");
        if (LightMode == Light.LightMode.Dark)
            sprite.Modulate = new Color(0, 0, 0);
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

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("floors"))
            QueueFree();
    }
}
