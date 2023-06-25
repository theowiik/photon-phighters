// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Godot;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class PhotonMuncher : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    public override string Name => "Mega Photon Muncher";

    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.MaxHealth = Mathf.RoundToInt(playerWhoSelected.MaxHealth * 1.42f);
      playerWhoSelected.PlayerMovementDelegate.Speed -= -200.0f;
      playerWhoSelected.Gun.BulletSpread *= 1.2f;
    }
  }
}
