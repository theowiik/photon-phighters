// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Godot;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class OingoBoingoCurse : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // Opponent is always bouncing
    public override string Name => "Oingo Boingo Curse";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerLand += _ApplyBounce;
    }

    private static void _ApplyBounce(PlayerMovementEvent playerMovementEvent)
    {
      playerMovementEvent.Velocity = new Vector2(playerMovementEvent.Velocity.X, -500);
    }
  }
}
