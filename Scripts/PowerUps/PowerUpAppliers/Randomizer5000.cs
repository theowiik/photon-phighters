// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class Randomizer5000 : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // Technically stateful but does not deserve its own class
    private readonly Random _rnd = new();

    // Photons are randomized
    public override string Name => "Randomizer 5000";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.GunShoot += _ApplyRandomization;
    }

    private void _ApplyRandomization(GunFireEvent shootEvent)
    {
      shootEvent.BulletCount += _rnd.Next(0, 3);
      shootEvent.BulletDamage += _rnd.Next(-2, 2);
      shootEvent.BulletGravity += (float)_rnd.NextDouble();
      shootEvent.BulletSizeFactor += (float)_rnd.NextDouble() * _rnd.Next(-1, 2);
      shootEvent.BulletSpeed += _rnd.Next(-100, 100);
      shootEvent.BulletSpread += (float)_rnd.NextDouble();
    }
  }
}
