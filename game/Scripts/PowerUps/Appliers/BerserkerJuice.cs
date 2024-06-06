using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

/// <summary>
///   When below 66.6% HP, grants bonus stats.
/// </summary>
public class BerserkerJuice : AbstractPowerUpApplier
{
  public override string Name => "Berserker Juice";
  public override Rarity Rarity => Rarity.Epic;
  public override bool IsCurse => false;

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
