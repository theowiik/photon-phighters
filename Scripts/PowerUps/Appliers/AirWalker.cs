using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class AirWalker : AbstractPowerUpApplier
  {
    public override string Name => "Air Walker";
    public override Rarity Rarity => Rarity.Common;
    public override bool IsCurse => false;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      var times = TimesTakenBy(playerWhoSelected);

      playerWhoSelected.PlayerMovementDelegate.MaxJumps++;
    }
  }
}
