// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Godot;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class SketchyPillsBad : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // Makes the player bigger and slower (gamba)
    public override string Name => "Sketchy Pills";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Scale *= new Vector2(1.25f, 1.25f);
      playerWhoSelected.PlayerMovementDelegate.Speed -= 100;
    }
  }
}
