using System;
using Godot;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

/// <summary>
///   Curse causing random bounces on landing.
/// </summary>
public class OingoBoingoCurse : AbstractPowerUpApplier
{
  //
  public override string Name => "Oingo Boingo Curse";
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
      case 1:
        otherPlayer.PlayerMovementDelegate.PlayerLand += _ApplyBounce;
        break;
      case >= 2:
        otherPlayer.PlayerMovementDelegate.PlayerLand += _ApplyRandomBounce;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private static void _ApplyBounce(PlayerMovementEvent playerMovementEvent)
  {
    playerMovementEvent.Velocity = new Vector2(playerMovementEvent.Velocity.X, -500);
  }

  private static void _ApplyRandomBounce(PlayerMovementEvent playerMovementEvent)
  {
    // Random bounce between +1000 and -1000
    var bounce = GD.RandRange(-1000, 1000);
    playerMovementEvent.Velocity = new Vector2(playerMovementEvent.Velocity.X, bounce);
  }
}
