using System;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class BunnyBoost : AbstractPowerUpApplier
  {
    public override string Name => "Bunny Boost";
    public override Rarity Rarity => Rarity.Common;
    public override bool IsCurse => false;

    public override string GetMarkName(Player player)
    {
      return LazyGetMarkName(3, player);
    }

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      var times = TimesTakenBy(playerWhoSelected);

      switch (times)
      {
        case 0:
          playerWhoSelected.PlayerMovementDelegate.JumpForce += 300;
          break;
        case 1:
          playerWhoSelected.PlayerMovementDelegate.JumpForce += 600;
          break;
        case >= 2:
          playerWhoSelected.PlayerMovementDelegate.JumpForce += 900;
          playerWhoSelected.Gun.BulletDamage *= 3;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
