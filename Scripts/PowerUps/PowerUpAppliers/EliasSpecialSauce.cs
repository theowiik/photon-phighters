// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class EliasSpecialSauce : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    public override string Name => "Elias' Special Sauce";

    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Epic;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.FireRate = 11;
      playerWhoSelected.Gun.BulletDamage = 4;
      playerWhoSelected.Gun.BulletCount = 1;
      playerWhoSelected.Gun.BulletSpread = 0;
    }
  }
}
