using System;
using PhotonPhighters.Scripts.Events;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class Randomizer5000 : AbstractPowerUpApplier
  {
    // Technically stateful but does not deserve its own class
    private readonly Random _rnd = new();

    // Photons are randomized
    public override string Name => "Randomizer 5000";
    public override Rarity Rarity => Rarity.Common;
    public override bool IsCurse => false;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.GunShoot += _ApplyRandomization;
    }

    private void _ApplyRandomization(GunFireEvent shootEvent)
    {
      shootEvent.BulletCount += _rnd.Next(0, 3);
      shootEvent.BulletDamage += _rnd.Next(-2, 2);
      shootEvent.BulletGravity += (float)_rnd.NextDouble();
      shootEvent.BulletSizeFactor += (float)_rnd.NextDouble() * _rnd.Next(-1, 2);
      shootEvent.BulletSpeed += _rnd.Next(-100, 100);
      shootEvent.BulletSpread += (float)_rnd.NextDouble();
    }
  }
}
