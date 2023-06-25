namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class AirWalker : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    public override string Name => "Air Walker";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      var times = TimesTakenBy(playerWhoSelected);

      playerWhoSelected.PlayerMovementDelegate.MaxJumps++;
    }
  }
}
