// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.PowerUpAppliers;

public static partial class PowerUps
{
  public class BerserkerJuice : Scripts.PowerUps.PowerUps.AbstractPowerUpApplier
  {
    // When below 50% HP, grants bonus stats
    public override string Name => "Berserker Juice";
    public override Scripts.PowerUps.PowerUps.Rarity Rarity => Scripts.PowerUps.PowerUps.Rarity.Epic;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulBerserkerJuice().Apply(playerWhoSelected);
    }

    private class StatefulBerserkerJuice
    {
      private const float Treshold = 0.666f;
      private Player _player;

      public void Apply(Player playerWhoSelected)
      {
        _player = playerWhoSelected;
        playerWhoSelected.PlayerMovementDelegate.PlayerMove += IncreaseSpeed;
        playerWhoSelected.PlayerMovementDelegate.PlayerJump += IncreaseJump;
        playerWhoSelected.Gun.GunShoot += IncreaseDamage;
      }

      private void IncreaseSpeed(PlayerMovementEvent playerMovementEvent)
      {
        if (_player.Health < _player.MaxHealth * Treshold)
        {
          playerMovementEvent.Speed += 150;
        }
      }

      private void IncreaseJump(PlayerMovementEvent playerMovementEvent)
      {
        if (_player.Health <= _player.MaxHealth * Treshold)
        {
          return;
        }

        playerMovementEvent.JumpForce += 100;
        playerMovementEvent.MaxJumps++;
      }

      private void IncreaseDamage(GunFireEvent shootEvent)
      {
        if (_player.Health >= _player.MaxHealth * Treshold)
        {
          return;
        }

        shootEvent.BulletDamage += 5;
        shootEvent.BulletCount += 1;
      }
    }
  }
}
