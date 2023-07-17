using System;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class BunnyBoost : AbstractPowerUpApplier
  {
    public override string Name => "Bunny Boost";
    public override Rarity Rarity => Rarity.Common;

    public override string GetMarkName(Player player)
    {
      var times = TimesTakenBy(player);

      switch (times)
      {
        case 0:
          return BuildMarkName(1);
        case 1:
          return BuildMarkName(2);
        case >= 2:
          return BuildMarkName(3);
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
          playerWhoSelected.PlayerMovementDelegate.JumpForce += 300;
          break;
        case 1:
          playerWhoSelected.PlayerMovementDelegate.JumpForce += 600;
          break;
        case >= 2:
          playerWhoSelected.PlayerMovementDelegate.JumpForce += 900;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
