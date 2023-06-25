// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class MiniGun : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    public override string Name => "1 000 000 lumen";

    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Legendary;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount += 4;
      playerWhoSelected.Gun.BulletDamage = 1;
      playerWhoSelected.Gun.BulletSpread += 0.3f;
      playerWhoSelected.Gun.BulletSpeed /= 1.4f;
      playerWhoSelected.Gun.FireRate += 1.8f;
    }
  }
}
