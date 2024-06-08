namespace PhotonPhighters.Scripts.PowerUps.Appliers;

/// <summary>
///   Increases player speed by 200.
/// </summary>
public class PhotonBoost : AbstractPowerUpApplier
{
  public override string Name => "Photon Boost";
  public override Rarity Rarity => Rarity.Common;
  public override bool IsCurse => false;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    playerWhoSelected.PlayerMovementDelegate.Speed += 200;
  }
}
