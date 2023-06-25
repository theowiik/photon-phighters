// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Godot;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class Chronostasis : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // Photons briefly freeze the opponent
    public override string Name => "Chronostasis";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Epic;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulChronostasis().Apply(otherPlayer);
    }

    private class StatefulChronostasis
    {
      private ulong _msecSinceLastFreeze;

      public void Apply(Player otherPlayer)
      {
        otherPlayer.PlayerHurt += RecordTimeSinceFreeze;
        otherPlayer.PlayerMovementDelegate.PlayerMove += FreezePlayer;
      }

      private void FreezePlayer(PlayerMovementEvent movementEvent)
      {
        var currentTimeMsec = Time.GetTicksMsec();
        if (currentTimeMsec - _msecSinceLastFreeze >= 400)
        {
          return;
        }

        movementEvent.CanMove = false;
        movementEvent.CanJump = false;
      }

      private void RecordTimeSinceFreeze(Player player, int damage, PlayerHurtEvent playerHurtEvent)
      {
        _msecSinceLastFreeze = Time.GetTicksMsec();
      }
    }
  }
}
