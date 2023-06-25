using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class EliasSpecialSauce : AbstractPowerUpApplier
  {
    public override string Name => "Elias' Special Sauce";

    public override Rarity Rarity => Rarity.Epic;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.FireRate = 11;
      playerWhoSelected.Gun.BulletDamage = 4;
      playerWhoSelected.Gun.BulletCount = 1;
      playerWhoSelected.Gun.BulletSpread = 0;
    }
  }
}
