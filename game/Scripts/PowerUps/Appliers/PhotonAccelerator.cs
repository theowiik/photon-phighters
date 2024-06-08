namespace PhotonPhighters.Scripts.PowerUps.Appliers;

/// <summary>
///   Increases bullet speed by 300 and bullet spread by 5%.
/// </summary>
public class PhotonAccelerator : AbstractPowerUpApplier
{
  public override string Name => "Photon Accelerator";
  public override Rarity Rarity => Rarity.Common;
  public override bool IsCurse => false;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    playerWhoSelected.Gun.BulletSpeed += 300.0f;
    playerWhoSelected.Gun.BulletSpread *= 1.05f;
  }
}
