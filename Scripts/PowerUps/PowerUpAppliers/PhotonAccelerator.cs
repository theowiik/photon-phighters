// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class PhotonAccelerator : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    public override string Name => "Photon Accelerator";

    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletSpeed += 300.0f;
      playerWhoSelected.Gun.BulletSpread *= 1.05f;
    }
  }
}
