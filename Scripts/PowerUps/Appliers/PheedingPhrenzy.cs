using System;
using PhotonPhighters.Scripts.Events;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class PheedingPhrenzy : AbstractPowerUpApplier
  {
    // Hurting the opponent grows your photons
    public override string Name => "Pheeding Phrenzy";
    public override Rarity Rarity => Rarity.Legendary;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulPheedingPhrenzy().Apply(playerWhoSelected, otherPlayer);
    }

    private class StatefulPheedingPhrenzy
    {
      private int _photonDamage;
      private float _photonSize;

      public void Apply(Player playerWhoSelected, Player otherPlayer)
      {
        otherPlayer.PlayerHurt += IncreasePhotonSize;
        playerWhoSelected.Gun.GunShoot += _ApplyPhotonSize;
      }

      private void _ApplyPhotonSize(GunFireEvent shootEvent)
      {
        shootEvent.BulletDamage += _photonDamage;
        shootEvent.BulletSizeFactor += _photonSize;
      }

      private void IncreasePhotonSize(Player player, int damage, PlayerHurtEvent playerHurtEvent)
      {
        _photonDamage = Math.Min(_photonDamage + 1, 30);
        _photonSize = MathF.Min(_photonSize + 0.05f, 10f);
      }
    }
  }
}
