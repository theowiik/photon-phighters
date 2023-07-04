using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class BulletRain : AbstractPowerUpApplier
  {
    public override string Name => "Bullet Rain";
    public override Rarity Rarity => Rarity.Rare;
    public override bool IsCurse => false;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount *= 2; // Double the bullets
      playerWhoSelected.Gun.BulletGravity *= 2; // Bullets drop faster
    }
  }
}
