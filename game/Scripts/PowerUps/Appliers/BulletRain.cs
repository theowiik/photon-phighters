using System;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public class BulletRain : AbstractPowerUpApplier
{
  public override string Name => "Bullet Rain";
  public override Rarity Rarity => Rarity.Rare;
  public override bool IsCurse => false;

  public override string GetMarkName(Team team)
  {
    return LazyGetMarkName(2, team);
  }

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    var times = TimesTakenBy(playerWhoSelected.Team);

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
