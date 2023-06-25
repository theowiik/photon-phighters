// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Godot;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class FluorescentBurst : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // Getting hurt briefly increases movement speed
    public override string Name => "Fluorescent Burst";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulFluorescentBurst().Apply(playerWhoSelected);
    }

    private class StatefulFluorescentBurst
    {
      private ulong _msecSinceLastHurt;

      public void Apply(Player playerWhoSelected)
      {
        playerWhoSelected.PlayerMovementDelegate.PlayerMove += GiveSpeedBoost;
        playerWhoSelected.PlayerHurt += RecordTimeSinceHurt;
      }

      private void GiveSpeedBoost(PlayerMovementEvent playerMovementEvent)
      {
        var currentTimeMsec = Time.GetTicksMsec();
        if (currentTimeMsec - _msecSinceLastHurt < 2000)
        {
          playerMovementEvent.Speed *= 1.5f;
        }
      }

      private void RecordTimeSinceHurt(Player player, int damage, PlayerHurtEvent playerHurtEvent)
      {
        _msecSinceLastHurt = Time.GetTicksMsec();
      }
    }
  }
}
