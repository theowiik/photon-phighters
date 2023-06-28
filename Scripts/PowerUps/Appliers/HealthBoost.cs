using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class HealthBoost : AbstractPowerUpApplier
  {
    public override string Name => "Health Boost";

    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.MaxHealth = (int)(playerWhoSelected.MaxHealth * 1.5f);
    }
  }
}
