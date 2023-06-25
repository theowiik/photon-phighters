// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Godot;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class NikeAirJordans : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // Player can double tap to dash
    public override string Name => "Nike Air Jordans";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.PlayerDoubleTapped += Dash;
    }

    private static void Dash(PlayerMovementEvent playerMovementEvent)
    {
      playerMovementEvent.Velocity += new Vector2(
        playerMovementEvent.InputDirection.X * (playerMovementEvent.Speed * 7),
        0
      );
    }
  }
}
