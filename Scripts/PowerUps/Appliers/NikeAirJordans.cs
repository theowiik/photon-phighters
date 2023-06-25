using Godot;
using PhotonPhighters.Scripts.Events;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class NikeAirJordans : AbstractPowerUpApplier
  {
    // Player can double tap to dash
    public override string Name => "Nike Air Jordans";
    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.PlayerDoubleTapped += Dash;
    }

    private static void Dash(PlayerMovementEvent playerMovementEvent)
    {
      playerMovementEvent.Velocity += new Vector2(
        playerMovementEvent.InputDirection.X * (playerMovementEvent.Speed * 7),
        0
      );
    }
  }
}
