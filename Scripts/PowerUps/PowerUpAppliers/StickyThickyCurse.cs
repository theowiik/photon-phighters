// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class StickyThickyCurse : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    public override string Name => "Sticky Thicky Curse";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Legendary;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.Speed /= 2;
      otherPlayer.PlayerMovementDelegate.Acceleration /= 3;
      otherPlayer.PlayerMovementDelegate.JumpForce /= 2;
      otherPlayer.MaxHealth += 50; // TODO: Possibly make it relative to the player's max health
    }
  }
}
