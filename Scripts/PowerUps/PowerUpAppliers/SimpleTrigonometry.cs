// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class SimpleTrigonometry : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // Photons move toward the other player
    public override string Name => "Simple Trigonometry";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulSimpleTrigonometry().Apply(playerWhoSelected, otherPlayer);
    }

    private class StatefulSimpleTrigonometry
    {
      private Player OtherPlayer { get; set; }

      public void Apply(Player playerWhoSelected, Player otherPlayer)
      {
        playerWhoSelected.Gun.BulletSpeed /= 1.5f;
        OtherPlayer = otherPlayer;
        playerWhoSelected.Gun.BulletFlying += MoveToOtherPlayer;
      }

      private void MoveToOtherPlayer(BulletEvent bulletEvent)
      {
        var vector = OtherPlayer.Position - bulletEvent.Area2D.Position;
        const int AttractionStrenth = 20;
        bulletEvent.Velocity += vector.Normalized() * AttractionStrenth;
      }
    }
  }
}
