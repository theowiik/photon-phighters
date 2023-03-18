using Godot;
using System;

public partial class World : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Player>("Player").Gun.ShootDelegate += OnShoot;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnShoot(Node2D bullet)
    {
        AddChild(bullet);
    }
}
