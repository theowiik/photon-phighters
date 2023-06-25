// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class PhotonPhlyer : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // Player can fly
    public override string Name => "Photon Phlyer";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Legendary;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.PlayerMove += DisableGravity;
    }

    private static void DisableGravity(PlayerMovementEvent playerMovementEvent)
    {
      playerMovementEvent.CanJump = false;
      playerMovementEvent.Gravity = 0;
      playerMovementEvent.Velocity = playerMovementEvent.InputDirection * playerMovementEvent.Speed;
    }
  }
}
