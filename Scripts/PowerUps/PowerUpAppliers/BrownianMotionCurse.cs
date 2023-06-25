// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Godot;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class BrownianMotionCurse : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // Technically stateful but does not deserve its own class
    private readonly Random _rnd = new();

    // Opponent's photons move erratically
    public override string Name => "Brownian Motion Curse";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.Gun.BulletFlying += RandomizeDirection;
    }

    private void RandomizeDirection(BulletEvent bulletFlyingEvent)
    {
      bulletFlyingEvent.Velocity += new Vector2(_rnd.Next(-150, 150), _rnd.Next(-150, 150));
    }
  }
}
