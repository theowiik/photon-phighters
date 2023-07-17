using System;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class BulletRain : AbstractPowerUpApplier
  {
    public override string Name => "Bullet Rain";

    public override Rarity Rarity => Rarity.Rare;

    public override string GetMarkName(Player player)
    {
      var times = TimesTakenBy(player);

      switch (times)
      {
        case 0:
          return BuildMarkName(1);
        case >= 1:
          return BuildMarkName(2);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      var times = TimesTakenBy(playerWhoSelected);

      switch (times)
      {
        case 0:
          playerWhoSelected.Gun.BulletCount *= 2; // Double the bullets
          playerWhoSelected.Gun.BulletGravity *= 2; // Bullets drop faster
          break;
        case >= 1:
          playerWhoSelected.Gun.BulletCount *= 2; // Double the bullets again
          playerWhoSelected.Gun.BulletGravity *= 2; // Bullets drop faster again
          playerWhoSelected.Gun.BulletSpeed *= 2; // Increase bullet speed
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
