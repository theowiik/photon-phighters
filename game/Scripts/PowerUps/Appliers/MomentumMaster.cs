namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public class MomentumMaster : AbstractPowerUpApplier
{
  public override string Name => "Momentum Master";
  public override Rarity Rarity => Rarity.Rare;
  public override bool IsCurse => false;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    playerWhoSelected.PlayerMovementDelegate.Speed += 300;
    playerWhoSelected.PlayerMovementDelegate.Acceleration += 6;
  }
}
