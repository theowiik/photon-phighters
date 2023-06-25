// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Godot;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class WallSpider : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // Wall-jumping briefly increases movement speed
    public override string Name => "Wall Spider";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulWallSpider().Apply(playerWhoSelected);
    }

    private class StatefulWallSpider
    {
      private ulong _msecSinceLastWallJump;

      private void GiveSpeedBoost(PlayerMovementEvent playerMovementEvent)
      {
        var currentTimeMsec = Time.GetTicksMsec();
        if (currentTimeMsec - _msecSinceLastWallJump < 6000)
        {
          playerMovementEvent.Speed *= 1.5f;
        }
      }

      private void RecordTimeSinceWallJump(PlayerMovementEvent playerMovementEvent)
      {
        _msecSinceLastWallJump = Time.GetTicksMsec();
      }

      public void Apply(Player playerWhoSelected)
      {
        playerWhoSelected.PlayerMovementDelegate.PlayerMove += GiveSpeedBoost;
        playerWhoSelected.PlayerMovementDelegate.PlayerWallJump += RecordTimeSinceWallJump;
      }
    }
  }
}
