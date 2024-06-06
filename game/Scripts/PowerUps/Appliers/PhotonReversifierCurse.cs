using Godot;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

/// <summary>
///   The opponent's movement is reversed.
/// </summary>
public class PhotonReversifierCurse : AbstractPowerUpApplier
{
  public override string Name => "Photon Reversifier Curse";
  public override Rarity Rarity => Rarity.Epic;
  public override bool IsCurse => true;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    otherPlayer.PlayerMovementDelegate.PlayerMove += ReverseMovement;
  }

  private static void ReverseMovement(PlayerMovementEvent movementEvent)
  {
    movementEvent.InputDirection *= new Vector2(-1, 1);
  }
}
