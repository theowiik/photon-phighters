// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Godot;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class PhotonReversifierCurse : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // The opponent's movement is reversed
    public override string Name => "Photon Reversifier Curse";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Epic;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerMove += ReverseMovement;
    }

    private static void ReverseMovement(PlayerMovementEvent movementEvent)
    {
      movementEvent.InputDirection *= new Vector2(-1, 1);
    }
  }
}
