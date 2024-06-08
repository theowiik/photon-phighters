using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public class PhotonPhlyer : AbstractPowerUpApplier
{
  // Player can fly
  public override string Name => "Photon Phlyer";
  public override Rarity Rarity => Rarity.Legendary;
  public override bool IsCurse => false;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    playerWhoSelected.PlayerMovementDelegate.PlayerMove += DisableGravity;
  }

  private static void DisableGravity(PlayerMovementEvent playerMovementEvent)
  {
    playerMovementEvent.CanJump = false;
    playerMovementEvent.Gravity = 0;
    playerMovementEvent.Velocity = playerMovementEvent.InputDirection * playerMovementEvent.Speed;
  }
}
