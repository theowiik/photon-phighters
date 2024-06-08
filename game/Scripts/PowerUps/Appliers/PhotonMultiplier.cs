using System;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public class PhotonMultiplier : AbstractPowerUpApplier
{
  public override string Name => "Photon Multiplier";
  public override Rarity Rarity => Rarity.Rare;
  public override bool IsCurse => false;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    playerWhoSelected.Gun.BulletCount = (int)Math.Ceiling(playerWhoSelected.Gun.BulletCount * 1.5f);
    playerWhoSelected.Gun.BulletSpread *= 1.05f;
  }
}
