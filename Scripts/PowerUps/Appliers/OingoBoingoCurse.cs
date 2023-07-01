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

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerLand += _ApplyBounce;
    }

    private static void _ApplyBounce(PlayerMovementEvent playerMovementEvent)
    {
      playerMovementEvent.Velocity = new Vector2(playerMovementEvent.Velocity.X, -500);
    }
  }
}
