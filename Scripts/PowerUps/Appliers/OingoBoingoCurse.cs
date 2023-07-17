using System;
using Godot;
using PhotonPhighters.Scripts.Events;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class OingoBoingoCurse : AbstractPowerUpApplier
  {
    // Opponent is always bouncing
    public override string Name => "Oingo Boingo Curse";
    public override Rarity Rarity => Rarity.Rare;
    public override bool IsCurse => false;

    public override string GetMarkName(Player player)
    {
      var times = TimesTakenBy(player);

      switch (times)
      {
        case 0:
          return BuildMarkName(1);
        case >= 1:
          return BuildMarkName(2);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      var times = TimesTakenBy(playerWhoSelected);

      switch (times)
      {
        case 0:
          otherPlayer.PlayerMovementDelegate.PlayerLand += _ApplyBounce;
          break;
        case >= 1:
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
}
