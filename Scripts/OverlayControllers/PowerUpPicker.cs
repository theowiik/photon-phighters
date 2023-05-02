using Godot;

public partial class PowerUpPicker : Control
{
    private PackedScene _powerUpButtonScene = GD.Load<PackedScene>("res://Objects/UI/PowerUpButton.tscn");
    private const int AmountPowerUps = 5;

    public override void _Ready()
    {
        FillPowerUpDeck();
    }

    private void FillPowerUpDeck()
    {
        for (int i = 0; i < AmountPowerUps; i++)
        {
            var powerUpButton = _powerUpButtonScene.Instantiate<PowerUpButton>();
            powerUpButton.Pressed += () =>
            {
                GD.Print("Powerup button pressed");

                // throw new System.Exception("fix game state");
                // Player losingPlayer = null;
                // powerUpButton.ApplyPowerUp(losingPlayer);
                // GD.Print("Powerup applied");
                // ClearPowerUpDeck();
                // EmitSignal(SignalName.PowerUpSelected);
            };

            AddChild(powerUpButton);
        }
    }

    private void ClearPowerUpDeck()
    {
        foreach (var powerUpButton in this.GetNodes<Button>())
        {
            powerUpButton.QueueFree();
        }
    }
}
