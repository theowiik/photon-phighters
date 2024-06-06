namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public class AirWalker : AbstractPowerUpApplier
{
  public override string Name => "Air Walker";
  public override Rarity Rarity => Rarity.Common;
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
