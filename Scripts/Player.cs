using Godot;

public partial class Player : CharacterBody2D
{
    private const float Speed = 300.0f;
    private const float JumpVelocity = -400.0f;
    private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private Marker2D _gunMarker;

    public override void _Ready()
    {
        _gunMarker = GetNode<Marker2D>("Marker2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);
        Aim(delta);
    }

    private void Movement(double delta)
    {
        var nextVelocity = Velocity;

        if (!IsOnFloor())
            nextVelocity.Y += _gravity * (float)delta;

        // TODO: Use "jump"
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            nextVelocity.Y = JumpVelocity;

        // TODO: Update to use "move_left", "move_right" and such
        var inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (inputDir != Vector2.Zero)
            nextVelocity.X = inputDir.X * Speed;
        else
            nextVelocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

        Velocity = nextVelocity;
        MoveAndSlide();
    }

    private void Aim(double delta)
    {
        var direction = GetGlobalMousePosition() - Position;
        _gunMarker.Rotation = direction.Angle();
    }
}
