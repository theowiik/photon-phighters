using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class GeneratorEngine : AbstractPowerUpApplier
  {
    public override string Name => "Generator Engine";
    public override Rarity Rarity => Rarity.Rare;
    public override bool IsCurse => false;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.FireRate += 0.9f;
      playerWhoSelected.Gun.BulletSpread *= 1.06f;
    }
  }
}
