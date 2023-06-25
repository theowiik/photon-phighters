namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class FakeJordans : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // Messes with the opponents movement
    public override string Name => "Fake Jordans";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.Speed -= 75.0f;
      otherPlayer.PlayerMovementDelegate.JumpForce -= 100.0f;
    }
  }
}
