using Godot;

public partial class PowerUpPicker : Control
{
    // Non Godot signal since Godot doesnt support custom types
    public delegate void PowerUpPicked(PowerUpManager.IPowerUpApplier powerUpApplier);
    public event PowerUpPicked PowerUpPickedListeners;

    [GetNode("GridContainer")]
    private GridContainer _gridContainer;

    private PackedScene _powerUpButtonScene = GD.Load<PackedScene>("res://Objects/UI/PowerUpButton.tscn");
    private const int AmountPowerUps = 4;

    public override void _Ready()
    {
        this.AutoWire();
        Reset();
    }

    public void Reset()
    {
        Clear();
        Populate();
    }

    private void Populate()
    {
        var powerUps = PowerUpManager.GetUniquePowerUps(AmountPowerUps);

        foreach (var powerUp in powerUps)
        {
            var powerUpButton = _powerUpButtonScene.Instantiate<PowerUpButton>();
            powerUpButton.Text = powerUp.Name;
            powerUpButton.Pressed += () =>
            {
                GD.Print("Powerup button pressed");
                PowerUpPickedListeners?.Invoke(powerUp);
            };

            _gridContainer.AddChild(powerUpButton);
        }
    }

    private void Clear()
    {
        foreach (var powerUpButton in _gridContainer.GetNodes<Button>())
        {
            powerUpButton.QueueFree();
        }
    }
}
