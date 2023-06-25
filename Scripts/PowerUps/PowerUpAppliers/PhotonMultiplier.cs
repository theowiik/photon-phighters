using System;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class PhotonMultiplier : AbstractPowerUpApplier
  {
    public override string Name => "Photon Multiplier";

    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount = (int)Math.Ceiling(playerWhoSelected.Gun.BulletCount * 1.5f);
      playerWhoSelected.Gun.BulletSpread *= 1.05f;
    }
  }
}
