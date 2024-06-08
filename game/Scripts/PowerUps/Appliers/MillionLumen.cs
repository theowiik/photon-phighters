using Godot;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public class MillionLumen : AbstractPowerUpApplier
{
  public override string Name => "1.000.000 Lumen";
  public override Rarity Rarity => Rarity.Legendary;
  public override bool IsCurse => false;

  public override string GetMarkName(Team team)
  {
    return LazyGetMarkName(2, team);
  }

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    var taken = TimesTakenBy(playerWhoSelected.Team);

    GD.Print("Taken: " + taken);

    switch (taken)
    {
      case 1:
        playerWhoSelected.Gun.BulletCount += 4;
        playerWhoSelected.Gun.BulletDamage = 1;
        playerWhoSelected.Gun.BulletSpread += 0.3f;
        playerWhoSelected.Gun.BulletSpeed /= 1.4f;
        playerWhoSelected.Gun.FireRate += 1.8f;
        break;
      case >= 2:
        playerWhoSelected.Gun.BulletSpread = Mathf.Pi * 2;
        playerWhoSelected.Gun.BulletCount += 10;
        break;
    }
  }
}
