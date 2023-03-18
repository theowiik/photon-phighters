using Godot;

public partial class PowerUpButton : Button
{
    private PowerUpManager.IPowerUpApplier _powerUpApplier;

    public override void _Ready()
    {
        _powerUpApplier = PowerUpManager.GetRandomPowerup();
    }

    public void ApplyPowerUp(Player player)
    {
        _powerUpApplier.Apply(player);
    }
}
