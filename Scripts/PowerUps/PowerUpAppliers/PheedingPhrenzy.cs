// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class PheedingPhrenzy : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // Hurting the opponent grows your photons
    public override string Name => "Pheeding Phrenzy";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Legendary;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulPheedingPhrenzy().Apply(playerWhoSelected, otherPlayer);
    }

    private class StatefulPheedingPhrenzy
    {
      private int _photonDamage;
      private float _photonSize;

      public void Apply(Player playerWhoSelected, Player otherPlayer)
      {
        otherPlayer.PlayerHurt += IncreasePhotonSize;
        playerWhoSelected.Gun.GunShoot += _ApplyPhotonSize;
      }

      private void _ApplyPhotonSize(GunFireEvent shootEvent)
      {
        shootEvent.BulletDamage += _photonDamage;
        shootEvent.BulletSizeFactor += _photonSize;
      }

      private void IncreasePhotonSize(Player player, int damage, PlayerHurtEvent playerHurtEvent)
      {
        _photonDamage = Math.Min(_photonDamage + 1, 30);
        _photonSize = MathF.Min(_photonSize + 0.05f, 10f);
      }
    }
  }
}
