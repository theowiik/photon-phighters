using Godot;

public partial class PowerUpPicker : Control
{
    private PackedScene _powerUpScene = GD.Load<PackedScene>("res://Objects/UI/PowerUpButton.tscn");

    public override void _Ready()
    {
        var nPowerUps = 20;

        for (var i = 0; i < nPowerUps; i++)
        {
            var powerUpButton = _powerUpScene.Instantiate<PowerUpButton>();
            GetNode("GridContainer").AddChild(powerUpButton);
        }
    }

    public override void _Process(double delta)
    {
    }
}
