using Godot;

public partial class MultiplayerDemo : Node
{
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("p1_jump"))
        {
            GD.Print("P1 Jump");
        }

        if (Input.IsActionJustPressed("p2_jump"))
        {
            GD.Print("P2 Jump");
        }
    }
}
