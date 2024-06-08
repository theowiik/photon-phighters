using Godot;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public class Phart : AbstractPowerUpApplier
{
  public override string Name => "Phart";
  public override Rarity Rarity => Rarity.Rare;
  public override bool IsCurse => false;

  public override string GetMarkName(Team player)
  {
    return LazyGetMarkName(2, player);
  }

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    var times = TimesTakenBy(playerWhoSelected.Team);

    switch (times)
    {
      case 1:
        playerWhoSelected.Gun.BulletSpread += Mathf.Pi / 4;
        playerWhoSelected.Gun.BulletCount += 3;
        playerWhoSelected.Gun.BulletSpeed /= 2f;
        break;
      case >= 2:
        playerWhoSelected.Gun.BulletSpread += Mathf.Pi / 4;
        playerWhoSelected.Gun.BulletCount += 10;
        break;
    }
  }
}
