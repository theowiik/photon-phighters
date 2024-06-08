namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public class BunnyBoost : AbstractPowerUpApplier
{
  public override string Name => "Bunny Boost";
  public override Rarity Rarity => Rarity.Common;
  public override bool IsCurse => false;

  public override string GetMarkName(Team team)
  {
    return LazyGetMarkName(3, team);
  }

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    var times = TimesTakenBy(playerWhoSelected.Team);

    switch (times)
    {
      case 1:
        playerWhoSelected.PlayerMovementDelegate.JumpForce += 300;
        break;
      case 2:
        playerWhoSelected.PlayerMovementDelegate.JumpForce += 600;
        break;
      case >= 3:
        playerWhoSelected.PlayerMovementDelegate.JumpForce += 900;
        playerWhoSelected.Gun.BulletDamage *= 3;
        break;
    }
  }
}
