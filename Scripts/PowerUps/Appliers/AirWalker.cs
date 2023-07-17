using System;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class AirWalker : AbstractPowerUpApplier
  {
    public override string Name => "Air Walker";
    public override Rarity Rarity => Rarity.Common;
    public override bool IsCurse => false;

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
          playerWhoSelected.PlayerMovementDelegate.MaxJumps++;
          break;
        case >= 1:
          playerWhoSelected.PlayerMovementDelegate.MaxJumps++;
          playerWhoSelected.PlayerMovementDelegate.JumpForce += 200;
          break;
      }
    }
  }
}
