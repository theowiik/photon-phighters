using Godot;

public partial class StartSceen : Node2D
{
    public override void _Ready()
    {
        var timer = GetNode<Timer>("Timer");
        timer.Start();
        timer.Timeout += OnTimeout;
    }

    private void OnTimeout()
    {
        GetTree().ChangeSceneToFile("res://Scenes/World.tscn");
    }
}
